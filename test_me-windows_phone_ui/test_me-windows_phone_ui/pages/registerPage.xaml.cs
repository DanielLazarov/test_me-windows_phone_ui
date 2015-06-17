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

namespace test_me_windows_phone_ui.pages
{
    public partial class registerPage : PhoneApplicationPage
    {
        public registerPage()
        {
            InitializeComponent();
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            txtEmail.Text = "";
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == "")
                txtEmail.Text = "Email";
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

        private void txtFirstName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = "";
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtFirstName.Text == "")
                txtFirstName.Text = "First Name";
        }

        private void txtLastName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = "";
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtLastName.Text == "")
                txtLastName.Text = "Last Name";
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtUsername.Text) || txtUsername.Text == "Username")
            {
                MessageBox.Show("Please enter Username.");
            }
            else if (String.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("Please enter Password.");
            }
            else if (String.IsNullOrEmpty(txtFirstName.Text) || txtFirstName.Text == "First Name")
            {
                MessageBox.Show("Please enter First name.");
            }
            else if (String.IsNullOrEmpty(txtLastName.Text) || txtLastName.Text == "Last Name")
            {
                MessageBox.Show("Please enter Last name.");
            }
            else if (String.IsNullOrEmpty(txtEmail.Text) || txtEmail.Text == "Email")
            {
                MessageBox.Show("Please enter Email.");
            }
            else
            {
                RequestHandler rq = new RequestHandler();
                rq.ResponseArrived += new EventHandler<ResponseArrivedEventArgs>(rq_ResponseArrived);
                Dictionary<string, string> reqData = new Dictionary<string, string>();

                reqData.Add("action", "create_account");
                reqData.Add("username", txtUsername.Text);
                reqData.Add("password", txtPassword.Password);
                reqData.Add("first_name", txtFirstName.Text);
                reqData.Add("last_name", txtLastName.Text);
                reqData.Add("email", txtEmail.Text);

                rq.SendRequest(reqData);
            }
        }

        private void rq_ResponseArrived(object sender, ResponseArrivedEventArgs e)
        {
            ServerResponseData srd = e.response;

            if (srd.result != null && srd.status == "OK")
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    MessageBox.Show("Successful registration, please Log In");
                    this.NavigationService.Navigate(
                    new Uri("//pages/loginPage.xaml", UriKind.Relative));
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
}