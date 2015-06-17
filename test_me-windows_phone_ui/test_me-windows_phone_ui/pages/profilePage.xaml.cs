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

using Newtonsoft.Json;

using ServerAPI;
using System.Windows.Media.Imaging;


namespace test_me_windows_phone_ui.pages
{
    public partial class profilePage : PhoneApplicationPage
    {
        private string session_token;
        private string account_id;
        public profilePage()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            this.account_id = settings["account_id"].ToString();//ASSERT not null
            this.session_token = settings["session_token"].ToString();//ASSERT not null
            InitializeComponent();
            getAccountInfo();
        }

        private void getAccountInfo()
        {
            RequestHandler rq = new RequestHandler();
            rq.ResponseArrived += new EventHandler<ResponseArrivedEventArgs>(rq_AccountDetailsArived);
            Dictionary<string, string> reqData = new Dictionary<string, string>();

            reqData.Add("action", "get_account_details");
            reqData.Add("account_id", this.account_id);
            reqData.Add("session_token", this.session_token);

            rq.SendRequest(reqData);
        }

        private void rq_AccountDetailsArived(object sender, ResponseArrivedEventArgs e)
        {
            ServerResponseData srd = e.response;

            if (srd.result != null && srd.status == "OK")
            {
                ProfileData respData = JsonConvert.DeserializeObject<ProfileData>(srd.resultString);

                this.Dispatcher.BeginInvoke(delegate()
                {
                    this.txtBlFirstName.Text = respData.first_name;
                    this.txtBlRank.Text = respData.rank__name;
                    this.txtBlProgress.Text = respData.progress;
                    this.imgRank.Source = (ImageSource)new ImageSourceConverter().ConvertFromString("../images/star2_" + respData.rank_id.ToString() + ".png");
                    this.imgRank.Stretch = Stretch.Fill;
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

        private void beginRandomTest()
        {
            RequestHandler rq = new RequestHandler();
            rq.ResponseArrived += new EventHandler<ResponseArrivedEventArgs>(rq_BeginRandomTestArived);
            Dictionary<string, string> reqData = new Dictionary<string, string>();

            reqData.Add("action", "begin_test");
            reqData.Add("account_id", this.account_id);
            reqData.Add("session_token", this.session_token);
            reqData.Add("random_test", "1");

            rq.SendRequest(reqData);
        }

        private void rq_BeginRandomTestArived(object sender, ResponseArrivedEventArgs e)
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

                    settings.Add("test_session_token", respData.test_session_token);//TODO ASSERT DEFINED  
                    settings.Add("test_hours_left", respData.test_hours_left);
                    settings.Add("test_minutes_left", respData.test_minutes_left);
                    settings.Add("test_seconds_left", respData.test_seconds_left);
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
        private void btnStartRandomTest_Click(object sender, RoutedEventArgs e)
        {
            beginRandomTest();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            settings.Clear();
            settings.Save();
            this.NavigationService.Navigate(
                    new Uri("//pages/loginPage.xaml", UriKind.Relative));
        }

        private void btnBrowseTests_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(
                    new Uri("//pages/browseTestsPage.xaml", UriKind.Relative));
        }

        private void btnMyTests_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(
                    new Uri("//pages/browseTestsPage.xaml?account_id=" + this.account_id, UriKind.Relative));
        }

        private void btnMyResults_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(
                    new Uri("//pages/myResultsPage.xaml", UriKind.Relative));
        }
    }

    public struct ProfileData
    {
        public int rank_id;
        public string account_id;
        public string first_name;
        public string last_name;
        public string rank__name;
        public string email;
        public string progress;
    }

    public struct TestData
    {
        public string test_session_token;
        public int test_hours_left;
        public int test_minutes_left;
        public int test_seconds_left;
    }
}