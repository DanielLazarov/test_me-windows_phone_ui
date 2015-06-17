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

using test_me_windows_phone_ui.pages;

namespace test_me_windows_phone_ui
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(getUserData);
        }

        private void getUserData(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings.Contains("account_id") && !string.IsNullOrEmpty(settings["account_id"].ToString()) && settings.Contains("session_token") && !string.IsNullOrEmpty(settings["session_token"].ToString()))
            {
                this.NavigationService.Navigate(
                       new Uri("//pages/profilePage.xaml", UriKind.Relative));   
            }
            else
            {
                this.NavigationService.Navigate(
                       new Uri("//pages/loginPage.xaml", UriKind.Relative));
            }
        }
    }
}