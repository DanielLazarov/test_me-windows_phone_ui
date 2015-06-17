<<<<<<< HEAD
﻿using System;
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
=======
﻿using test_me_windows_phone_ui.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
>>>>>>> origin/master

using Newtonsoft.Json;

using ServerAPI;
<<<<<<< HEAD
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
=======
using Windows.Storage;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace test_me_windows_phone_ui.pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class loginPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public loginPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            txtEmail.Text = "";
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == "")
                txtEmail.Text = "Email";
>>>>>>> origin/master
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
<<<<<<< HEAD
            this.NavigationService.Navigate(
                      new Uri("//pages/registerPage.xaml", UriKind.Relative));
=======
            //TODO Navigate to register Page
>>>>>>> origin/master
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            RequestHandler rq = new RequestHandler();
            rq.ResponseArrived += new EventHandler<ResponseArrivedEventArgs>(rq_ResponseArrived);
<<<<<<< HEAD
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
=======
            Dictionary<string, string> reqData = new Dictionary<string, string>();

            reqData.Add("action", "login");
            reqData.Add("email", txtEmail.Text);
            reqData.Add("password", txtPassword.Password);

            rq.SendRequest(reqData);
        }

        private async void rq_ResponseArrived(object sender, ResponseArrivedEventArgs e)
        {
            MessageDialog msgbox;
            ServerResponseData srd = e.response;
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            if (srd.result != null && srd.status == "OK")
            {
                LoginData respData = JsonConvert.DeserializeObject<LoginData>(srd.resultString);
                msgbox = new MessageDialog(respData.first_name);
                await msgbox.ShowAsync();
                //TODO Navigate to profile page
            }
            else
            {
                msgbox = new MessageDialog(srd.message);
            }         
        }
    }

>>>>>>> origin/master
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
<<<<<<< HEAD
}
=======


    #endregion

}
>>>>>>> origin/master
