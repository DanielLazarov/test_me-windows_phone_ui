﻿#pragma checksum "D:\FlashDrive\Projects\test_me-windows_phone_ui\test_me-windows_phone_ui\test_me-windows_phone_ui\pages\profilePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "94937D37FA3CC46DA196F8F742E0501F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace test_me_windows_phone_ui.pages {
    
    
    public partial class profilePage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.Image imgRank;
        
        internal System.Windows.Controls.TextBlock txtBlRank;
        
        internal System.Windows.Controls.TextBlock txtBlFirstName;
        
        internal System.Windows.Controls.Button btnStartRandomTest;
        
        internal System.Windows.Controls.Button button1;
        
        internal System.Windows.Controls.TextBlock txtBlProgress;
        
        internal System.Windows.Controls.Button btnBrowseTests;
        
        internal System.Windows.Controls.Button btnMyTests;
        
        internal System.Windows.Controls.Button btnMyResults;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/test_me-windows_phone_ui;component/pages/profilePage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.imgRank = ((System.Windows.Controls.Image)(this.FindName("imgRank")));
            this.txtBlRank = ((System.Windows.Controls.TextBlock)(this.FindName("txtBlRank")));
            this.txtBlFirstName = ((System.Windows.Controls.TextBlock)(this.FindName("txtBlFirstName")));
            this.btnStartRandomTest = ((System.Windows.Controls.Button)(this.FindName("btnStartRandomTest")));
            this.button1 = ((System.Windows.Controls.Button)(this.FindName("button1")));
            this.txtBlProgress = ((System.Windows.Controls.TextBlock)(this.FindName("txtBlProgress")));
            this.btnBrowseTests = ((System.Windows.Controls.Button)(this.FindName("btnBrowseTests")));
            this.btnMyTests = ((System.Windows.Controls.Button)(this.FindName("btnMyTests")));
            this.btnMyResults = ((System.Windows.Controls.Button)(this.FindName("btnMyResults")));
        }
    }
}

