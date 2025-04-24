using PdfSharp.Fonts;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TireProject
{
    public partial class App : Application
    {
        public static int StatusBarHeight { get; set; }
        public static App myapp;
        public static double ScreenHeight { get; set; }
        public static double ScreenWidth { get; set; }
        public static bool report { get; set; }

        public static string pdfpath { get; set; } = "";

        public App()
        {
            InitializeComponent();
            App.report = false;
            myapp = this;
            MainPage = new NavigationPage(new MainPage());
           
            GlobalFontSettings.FontResolver = new FileFontResolver();
        }

        public static void GetUri(Uri uri)
        {
            myapp.MainPage = new NavigationPage(new NewExcelPage(uri));
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);
            GetUri(uri);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}