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
using ServerAPI;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;
using System.Collections.ObjectModel;

namespace test_me_windows_phone_ui.pages
{
    public partial class browseTestsPage : PhoneApplicationPage
    {
        private string account_id;
        public browseTestsPage()
        {   
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string acc_id = "";

            if (NavigationContext.QueryString.TryGetValue("account_id", out acc_id))
            {
                this.account_id = acc_id;
                this.PageTitle.Text = "My Tests";
            }
            else
            {
                this.PageTitle.Text = "Browse Tests";
            }
            getTests();
        }

        private void getTests()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            RequestHandler rq = new RequestHandler();
            rq.ResponseArrived += new EventHandler<ResponseArrivedEventArgs>(rq_TestsArived);
            Dictionary<string, string> reqData = new Dictionary<string, string>();

            if (!String.IsNullOrEmpty(this.account_id))
            {
                reqData.Add("search_account_id", this.account_id);
            }
            else
            {
                reqData.Add("search_account_id", "0");
            }

            reqData.Add("action", "get_tests");
            reqData.Add("account_id", settings["account_id"].ToString());
            reqData.Add("session_token", settings["session_token"].ToString());
        

            rq.SendRequest(reqData);
        }

        private void rq_TestsArived(object sender, ResponseArrivedEventArgs e)
        {
            ServerResponseData srd = e.response;

            if (srd.result != null && srd.status == "OK")
            {
                Tests respData = JsonConvert.DeserializeObject<Tests>(srd.resultString);
                this.Dispatcher.BeginInvoke(delegate()
                {
                    if (!String.IsNullOrEmpty(this.account_id))
                    {
                        if (this.FindName("btnAddNewTest") == null)
                        {
                            Button b = new Button();
                            b.Content = "Add new Test";
                            b.Height = 72;
                            b.Width = 438;
                            b.HorizontalAlignment = HorizontalAlignment.Left;
                            b.Margin = new Thickness(12, 517, 0, 0);
                            b.Name = "btnAddNewTest";
                            b.VerticalAlignment = VerticalAlignment.Top;
                            b.Click += btnAddNewTest_Click;
                            this.ContentPanel.Children.Add(b);
                        }   
                    }
                    
                    this.listBTests.Items.Clear();
                    foreach (Test test in respData.tests)
                    {
                        TextBlock bl = new TextBlock();
                        bl.Text = test.name;
                        bl.Tag = test.id;
                        bl.Margin = new Thickness(20);
                        bl.Tap += test_Tap;

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
        private void test_Tap(object sender, RoutedEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            this.Dispatcher.BeginInvoke(delegate()
            {
                this.NavigationService.Navigate(
                new Uri("//pages/testPreviewPage.xaml?test_id=" + tb.Tag, UriKind.Relative));
            });
        }

        private void btnAddNewTest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This function will be available soon.");
        } 
    }

    public struct Test
    {
        public int id;
        public string topic__name;
        public string difficulty__name;
        public string name;
        public string account_id;
    }

    public struct Tests
    {
        public List<Test> tests;
    }
}