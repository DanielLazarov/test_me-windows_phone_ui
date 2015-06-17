using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using ServerAPI;
using Newtonsoft.Json;

namespace test_me_windows_phone_ui.pages
{
    public partial class myResultsPage : PhoneApplicationPage
    {
        public myResultsPage()
        {
            InitializeComponent();
            getResults();
        }

        private void getResults()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            RequestHandler rq = new RequestHandler();
            rq.ResponseArrived += new EventHandler<ResponseArrivedEventArgs>(rq_ResultsArived);
            Dictionary<string, string> reqData = new Dictionary<string, string>();


            reqData.Add("action", "get_results");
            reqData.Add("account_id", settings["account_id"].ToString());
            reqData.Add("session_token", settings["session_token"].ToString());


            rq.SendRequest(reqData);
        }

        private void rq_ResultsArived(object sender, ResponseArrivedEventArgs e)
        {
            ServerResponseData srd = e.response;

            if (srd.result != null && srd.status == "OK")
            {
                Results respData = JsonConvert.DeserializeObject<Results>(srd.resultString);
                this.Dispatcher.BeginInvoke(delegate()
                {
                    foreach (ResultByTest result in respData.results)
                    {
                        TextBlock bl = new TextBlock();
                        bl.Text = result.test_date + ":   " + result.test__name + " " + result.earned.ToString() + "/" + result.total.ToString();
                        bl.Tag = result.test_id;
                        bl.Margin = new Thickness(20);

                        this.listBTests.Items.Add(bl);
                    }
                });
            }
            else
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    MessageBox.Show(srd.message);
                });
            }
        }

    }
    public struct Results
    {
        public List<ResultByTest> results;
    }

    public struct ResultByTest
    {
        public string test__name;
        public int earned;
        public int total;
        public int test_id;
        public string test_date;
    }
}