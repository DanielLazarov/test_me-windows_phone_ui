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
    public partial class testPreviewPage : PhoneApplicationPage
    {
        private string test_id;

        public testPreviewPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string t_id = "";

            if (NavigationContext.QueryString.TryGetValue("test_id", out t_id))
            {
                this.test_id = t_id;
            }
            else
            {
                this.test_id = "";
            }
            getTest();
        }

        private void getTest()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            RequestHandler rq = new RequestHandler();
            rq.ResponseArrived += new EventHandler<ResponseArrivedEventArgs>(rq_TestArived);
            Dictionary<string, string> reqData = new Dictionary<string, string>();

            reqData.Add("action", "get_tests");
            reqData.Add("account_id", settings["account_id"].ToString());
            reqData.Add("session_token", settings["session_token"].ToString());
            reqData.Add("test_id", this.test_id);

            rq.SendRequest(reqData);
        }

        private void rq_TestArived(object sender, ResponseArrivedEventArgs e)
        {
            ServerResponseData srd = e.response;
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (srd.result != null && srd.status == "OK")
            {
                Tests respData = JsonConvert.DeserializeObject<Tests>(srd.resultString);

                Test test = respData.tests[0];
                this.Dispatcher.BeginInvoke(delegate()
                {
                    this.PageTitle.Text = test.name;
                    this.txtBlDifficulty.Text = test.difficulty__name;
                    this.txtBlTopic.Text = test.topic__name;

                    if (settings["account_id"].ToString() == test.account_id)
                    {
                        if (this.FindName("btnEdit") == null)
                        {
                            Button b = new Button();
                            b.Content = "Edit";
                            b.Height = 72;
                            b.Width = 160;
                            b.HorizontalAlignment = HorizontalAlignment.Left;
                            b.Margin = new Thickness(290, 172, 0, 0);
                            b.Name = "btnEdit";
                            b.VerticalAlignment = VerticalAlignment.Top;
                            b.Click += btnEdit_Click;
                            this.ContentPanel.Children.Add(b);
                        }
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

        private void btnBeginThisTest_Click(object sender, RoutedEventArgs e)
        {
            beginTest(this.test_id);
        }

        private void beginTest(string test_id)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            RequestHandler rq = new RequestHandler();
            rq.ResponseArrived += new EventHandler<ResponseArrivedEventArgs>(rq_BeginTestArived);
            Dictionary<string, string> reqData = new Dictionary<string, string>();

            reqData.Add("action", "begin_test");
            reqData.Add("account_id", settings["account_id"].ToString());
            reqData.Add("session_token", settings["session_token"].ToString());
            reqData.Add("random_test", "0");
            reqData.Add("test_id", test_id);

            rq.SendRequest(reqData);
        }

        private void rq_BeginTestArived(object sender, ResponseArrivedEventArgs e)
        {
            ServerResponseData srd = e.response;
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (srd.result != null && srd.status == "OK")
            {
                TestData respData = JsonConvert.DeserializeObject<TestData>(srd.resultString);

                this.Dispatcher.BeginInvoke(delegate()
                {
                    if (settings.Contains("test_session_token"))
                    {
                        settings.Remove("test_session_token");
                    }
                    if (settings.Contains("test_hours_left"))
                    {
                        settings.Remove("test_hours_left");
                    }
                    if (settings.Contains("test_minutes_left"))
                    {
                        settings.Remove("test_minutes_left");
                    }
                    if (settings.Contains("test_seconds_left"))
                    {
                        settings.Remove("test_seconds_left");
                    }

                    settings.Add("test_session_token", respData.test_session_token);//TODO ASSERT DEFINED  
                    settings.Add("test_hours_left", respData.test_hours_left.ToString());
                    settings.Add("test_minutes_left", respData.test_minutes_left.ToString());
                    settings.Add("test_seconds_left", respData.test_seconds_left.ToString());
                    settings.Save();

                    this.NavigationService.Navigate(
                    new Uri("//pages/testPage.xaml", UriKind.Relative));
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
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This function will be available soon.");
        }
    }
}