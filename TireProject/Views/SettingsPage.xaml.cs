using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Media;
using Xamarin.Forms;

namespace TireProject
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();

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

        async void EveBack(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(en1.Text))
            {
                Application.Current.Properties["CName"] = en1.Text.Trim();
                lblmsg.Text = "Saved!";
                msgbox.IsVisible = true;
                await Task.Delay(2000);
                Application.Current.MainPage = new NavigationPage(new MainPage());

            }
            else
            {
                lblmsg.Text = "Enter Company Name First!";
                msgbox.IsVisible = true;
            }

            if (!string.IsNullOrWhiteSpace(en2.Text))
            {
                Application.Current.Properties["CAddress"] = en2.Text.Trim();
                lblmsg.Text = "Saved!";
                msgbox.IsVisible = true;
                await Task.Delay(2000);
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                lblmsg.Text = "Enter Company Address First!";
                msgbox.IsVisible = true;
            }

            if (!string.IsNullOrWhiteSpace(en1.Text))
            {
                Application.Current.Properties["CTerms"] = en1.Text.Trim();
                lblmsg.Text = "Saved!";
                msgbox.IsVisible = true;
                await Task.Delay(2000);
                Application.Current.MainPage = new NavigationPage(new MainPage());

            }
            else
            {
                lblmsg.Text = "Enter Terms and Conditions First!";
                msgbox.IsVisible = true;
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
    }
}
