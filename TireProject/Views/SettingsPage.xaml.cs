using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using Plugin.Media;
using Xamarin.Forms;

namespace TireProject
{
    public partial class SettingPage : ContentPage
    {
        
        private Settings _currentSettings;
        public SettingPage()
        {
            InitializeComponent();
            LoadSettings();
        }

        private async void LoadSettings()
        {
            try
            {
                _currentSettings = await GetSettingsAsync();

                if (_currentSettings != null)
                {
                    companyCodeEntry.Text = _currentSettings.CompanyCode;
                    en1.Text = _currentSettings.CompanyName;
                    en2.Text = _currentSettings.CompanyAddress;
                    en3.Text = _currentSettings.TermsAndConditions;
                }

                if (Application.Current.Properties.ContainsKey("CName"))
                {
                    en1.Text = Application.Current.Properties["CName"].ToString();
                }
                if (Application.Current.Properties.ContainsKey("CAddress"))
                {
                    en2.Text = Application.Current.Properties["CAddress"].ToString();
                }
                if (Application.Current.Properties.ContainsKey("CTerms"))
                {
                    en3.Text = Application.Current.Properties["CTerms"].ToString();
                }


                var pathImage = DependencyService.Get<IFileSave>().GetPicture();
                imglogo.Source = pathImage;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to load settings", "OK");
            }
        }

        async void EveBack(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(companyCodeEntry.Text))
            {
                lblmsg.Text = "Enter Company Code First!";
                msgbox.IsVisible = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(en1.Text))
            {
                lblmsg.Text = "Enter Company Name First!";
                msgbox.IsVisible = true;
                return;
            }

            Application.Current.Properties["CName"] = en1.Text.Trim();

            if (string.IsNullOrWhiteSpace(en2.Text))
            {
                lblmsg.Text = "Enter Company Address First!";
                msgbox.IsVisible = true;
                return;
            }

            Application.Current.Properties["CAddress"] = en2.Text.Trim();

            if (string.IsNullOrWhiteSpace(en1.Text))
            {
                lblmsg.Text = "Enter Terms and Conditions First!";
                msgbox.IsVisible = true;
                return;
            }

            Application.Current.Properties["CTerms"] = en1.Text.Trim();

            try
            {
                var settings = new Settings
                {
                    CompanyCode = companyCodeEntry.Text.Trim(),
                    CompanyName = en1.Text?.Trim(),
                    CompanyAddress = en2.Text?.Trim(),
                    TermsAndConditions = en3.Text?.Trim()
                };

                await SaveSettingsAsync(settings);

                lblmsg.Text = "Saved!";
                msgbox.IsVisible = true;
                await Task.Delay(2000);
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to save settings", "OK");
            }

        }

        async void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            var getCommand = (ImageButton)sender;
            getCommand.IsEnabled = false;



            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            var editImage = await DependencyService.Get<IImageResize>().ResizeImage(file.Path, "logotemp.png", 400, 400);


            using (var ms = new System.IO.FileStream(editImage, System.IO.FileMode.Open))
            {
                DependencyService.Get<IFileSave>().SavePicture(ms);
                ms.Close();
                ms.Dispose();
            }


            var pathImage = DependencyService.Get<IFileSave>().GetPicture();
            imglogo.Source = pathImage;

            getCommand.IsEnabled = true;
        }



        public async Task<Settings> GetSettingsAsync()
        {
            using (var _httpClient = new HttpClient())
            {
                var response = await _httpClient.GetAsync($"{Constants.BASE_URL}/api/settings");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Settings>(content);
            }
        }

        public async Task SaveSettingsAsync(Settings settings)
        {
            using (var _httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(settings), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{Constants.BASE_URL}/api/settings", content);
                response.EnsureSuccessStatusCode();
            }
        }


        public class Settings
        {
            public string Id { get; set; }
            public string CompanyCode { get; set; }
            public string CompanyName { get; set; }
            public string CompanyAddress { get; set; }
            public string TermsAndConditions { get; set; }
            public List<string> WareHouse { get; set; } = new List<string>();
        }
    }
}
