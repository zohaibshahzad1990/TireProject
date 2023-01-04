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

            Application.Current.Properties.Remove("CName");
            Application.Current.Properties.Remove("CAddress");
            Application.Current.Properties.Remove("CTerms");

            if (!Application.Current.Properties.ContainsKey("CName"))
            {
                Application.Current.Properties["CName"] = "TOTAL TIRE INC";
            }
            if (!Application.Current.Properties.ContainsKey("CAddress"))
            {
                Application.Current.Properties["CAddress"] = "4235 FAIRVIEW STREET\nBURLINGTON ON L7L 2A4\nPHONE: 905-632-3500\nPHONE;  905-632-9898\nwww.totaltire.com";
            }
            if (!Application.Current.Properties.ContainsKey("CTerms"))
            {
                Application.Current.Properties["CTerms"] = "The storage fee charged is for the previous season only. An average seasonal storage charges are $15 per tire.  The season means the current winter to coming  spring/summer and current summer means coming fall/winter, approximately six to max seventh months. In case customer fails to pickup or renew  the subscription for any particular season, TTI (TOTAL TIRE INC) has right to charge for additional period as per applicable rates. In case customer fails to renew or pickup his/her tires after the expiry of the seasonal contract TTI will attempt to contact the customer by all possible means but in the failure of establishing contact TTI has the right to dispose of the customer tires/rims two years after expiry of the contract period at no obligation or claim of compensation from the customer. The customer must notify TTI by phone at least 48 hours prior to the desired date and time for tire changing or or picking up from TTI. (excluding holidays/weekends). The customer hereby acknowledges that the customers tires/rims might be damaged/aged prior to storage at TTI and TTI is not responsible for such damages. TTI will not be responsible for any normal wear and tear or aging of tires that may occur  during the storage period. In case the TTI fails to return the customers stored tires/rims due to any reason  either lost/misplaced or not in the condition when stored TTI is liable to compensate for the un-used pro-rate portion of the tires/rims based on the info/picture of the tires/rims appended in this contract. Any disputer arising out of this agreement if initiated by the customer shall be referred to arbitration with single arbitration in Ont, Canada in accordance with the applicable rules of the arbitration .\nI have read and under stand the terms and conditions.";
            }
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