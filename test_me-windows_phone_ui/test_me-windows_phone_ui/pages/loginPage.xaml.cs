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

using Newtonsoft.Json;

using ServerAPI;
using System.IO.IsolatedStorage;


namespace test_me_windows_phone_ui.pages
{
    public partial class loginPage : PhoneApplicationPage
    {
        public loginPage()
        {
            InitializeComponent();

            txtUsername.Text = "Username";
            txtPasswordWatermark.Text = "Password";
        }

        private void txtUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            txtUsername.Text = "";
        }

        private void txtUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == "")
                txtUsername.Text = "Username";
        }

        private void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            var passwordEmpty = string.IsNullOrEmpty(txtPassword.Password);
            txtPasswordWatermark.Opacity = passwordEmpty ? 100 : 0;
            txtPassword.Opacity = passwordEmpty ? 0 : 100;
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPasswordWatermark.Opacity = 0;
            txtPassword.Opacity = 100;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(
                      new Uri("//pages/registerPage.xaml", UriKind.Relative));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            RequestHandler rq = new RequestHandler();
            rq.ResponseArrived += new EventHandler<ResponseArrivedEventArgs>(rq_ResponseArrived);
            Dictionary<string,string> reqData = new Dictionary<string,string>();

            if (String.IsNullOrEmpty(txtUsername.Text) || txtUsername.Text == "Username")
            {
                MessageBox.Show("Please enter Username.");
            }
            else if (String.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("Please enter Password.");
            }
            else
            {
                reqData.Add("action", "login");
                reqData.Add("username", txtUsername.Text);
                reqData.Add("password", txtPassword.Password);

                rq.SendRequest(reqData);
            }
        }

        private void rq_ResponseArrived(object sender, ResponseArrivedEventArgs e)
        {
            ServerResponseData srd = e.response;
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (srd.result != null && srd.status == "OK")
            {
                LoginData respData = JsonConvert.DeserializeObject<LoginData>(srd.resultString);
                settings.Add("account_id", respData.account_id);
                settings.Add("session_token", respData.session_token);
                settings.Save();

                this.Dispatcher.BeginInvoke(delegate()
                {

                    this.NavigationService.Navigate(
                    new Uri("//pages/profilePage.xaml", UriKind.Relative));
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
    public struct LoginData
    {
        public int rank_id;
        public string account_id;
        public string session_token;
        public string first_name;
        public string last_name;
        public string rank__name;
        public string email;
    }
}