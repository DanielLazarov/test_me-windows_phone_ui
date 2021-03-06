﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Schema;



namespace ServerAPI
{
    public class RequestHandler
    {
        public event EventHandler<ResponseArrivedEventArgs> ResponseArrived;
        private byte[] data;
        private string dataString;
        
        public void SendRequest(Dictionary<string, string> requestParams)
        {

            try
            {
                string dataToSend = "action=" + requestParams["action"];//ASSERT ACTION IS DEFINED and put all this in try catch
                foreach (KeyValuePair<string, string> kvp in requestParams)
                {
                    if (kvp.Key != "action")
                    {
                        dataToSend += "&" + kvp.Key + "=" + Uri.EscapeDataString(kvp.Value);
                    }
                }
                this.dataString = dataToSend;
                UTF8Encoding encoding = new UTF8Encoding();
                this.data = encoding.GetBytes(dataToSend);
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri("http://192.168.0.102:88"));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

                // Start the asynchronous operation to send the request data
                request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);
            }
            catch (Exception ex)
            {
                ServerResponseData respData = new ServerResponseData();
                respData.result = null;
                respData.status = "Unknown Error";
                respData.message = ex.Message;
                ResponseArrivedEventArgs resp = new ResponseArrivedEventArgs();
                resp.response = respData;

                this.OnResponseArrived(resp);
            }
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            postStream.Write(this.data, 0, this.dataString.Length);
            postStream.Close();

            // Start the asynchronous operation to get the response
            request.BeginGetResponse(new AsyncCallback(ReadWebRequestCallback), request);
        }
        
        private void ReadWebRequestCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.EndGetResponse(callbackResult);

            using (StreamReader httpwebStreamReader = new StreamReader(myResponse.GetResponseStream()))
            {
                string results = httpwebStreamReader.ReadToEnd();
                JsonSchema schema = JsonSchema.Parse(results);
                ServerResponseData respData = JsonConvert.DeserializeObject<ServerResponseData>(results);
                respData.resultString = JsonConvert.SerializeObject(respData.result);
                
                ResponseArrivedEventArgs resp = new ResponseArrivedEventArgs();
                resp.response = respData;

                this.OnResponseArrived(resp);     
            }
            myResponse.Close();
        }

        public virtual void OnResponseArrived(ResponseArrivedEventArgs e)
        {
            EventHandler<ResponseArrivedEventArgs> handler = ResponseArrived;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class ResponseArrivedEventArgs : EventArgs
    {
        public ServerResponseData response { get; set; }
    }

    public struct ServerResponseData
    {
        public string status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
        public string error_code { get; set; }
        public string resultString { get; set; }
    }
}
