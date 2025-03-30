using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using Android.Content;
using Xamarin.Forms;
using FFImageLoading.Forms.Platform;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using ZXing.Mobile;

namespace TireProject.Droid
{
    [Activity(Label = "TestingPdf", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    [IntentFilter(new[] { Android.Content.Intent.ActionView },
                       AutoVerify = true,
                       Categories = new[]
                       {
                            Android.Content.Intent.CategoryDefault,
                            Android.Content.Intent.CategoryBrowsable
                       },
                       DataScheme = "com.googleusercontent.apps.976132508244-rsm3j0u6udqg5bea3k31tdi5aiv34ktr"
                       )]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static Activity MyActivity { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Forms.SetFlags("CollectionView_Experimental");
         
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
           
            MyActivity = this;
			CachedImageRenderer.Init(true);
			//Android.Glide.Forms.Init();
			CrossCurrentActivity.Current.Init(this, savedInstanceState);
            App.ScreenHeight = (double)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.ScreenWidth = (double)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#e74c3c"));
            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.WriteExternalStorage) == Permission.Denied || ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.ReadExternalStorage) == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Android.Manifest.Permission.ReadExternalStorage, Android.Manifest.Permission.WriteExternalStorage }, 1);
            }

            // Request camera permission as well
            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.WriteExternalStorage) == Permission.Denied ||
                ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.ReadExternalStorage) == Permission.Denied ||
                ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.Camera) == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(this, new string[] {
                    Android.Manifest.Permission.ReadExternalStorage,
                    Android.Manifest.Permission.WriteExternalStorage,
                    Android.Manifest.Permission.Camera
                }, 1);
            }

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

