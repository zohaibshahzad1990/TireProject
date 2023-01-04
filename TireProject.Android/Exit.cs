using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using TireProject.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(CloseApplication))]
namespace TireProject.Droid
{
    public class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            MainActivity.MyActivity.FinishAffinity();
        }

        public bool storagepermission()
        {
            if (ContextCompat.CheckSelfPermission(MainActivity.MyActivity, Android.Manifest.Permission.WriteExternalStorage) == Permission.Denied || ContextCompat.CheckSelfPermission(MainActivity.MyActivity, Android.Manifest.Permission.ReadExternalStorage) == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(MainActivity.MyActivity, new string[] { Android.Manifest.Permission.ReadExternalStorage, Android.Manifest.Permission.WriteExternalStorage }, 1);
                return false;
            }
            else {
                return true;
            }
        }
    }
}
