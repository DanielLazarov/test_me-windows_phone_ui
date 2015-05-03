using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace ServerAPI
{
    public class RequestHandler
    {
        public event EventHandler<ResponseArrivedEventArgs> ResponseArrived;

        public void SendRequest(Dictionary<string, string> requestParams)
        {

            try
            {
                string dataToSend = "action=" + requestParams["action"];//ASSERT ACTION IS DEFINED and put all this in try catch
                foreach (KeyValuePair<string, string> kvp in requestParams)
                {
                    if (kvp.Key != "action")
                    {
                        dataToSend += "&" + kvp.Key + "=" + kvp.Value;
                    }
                }
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(dataToSend);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://78.90.52.246:88");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

                request.BeginGetResponse(new AsyncCallback(ReadWebRequestCallback), request);
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
            myResponse.Dispose();
        }

        protected virtual void OnResponseArrived(ResponseArrivedEventArgs e)
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
        public string resultString { get; set; }
    }
}
