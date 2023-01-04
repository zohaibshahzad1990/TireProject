
using Android.App;
using Android.Support.V7.App; 
using TireProject.Droid;

[Activity(Label = "Total Tire Inc.", Icon = "@drawable/icon", Theme = "@style/splashscreen", MainLauncher = true, NoHistory = true)]
public class SplashActivity : AppCompatActivity
{
    protected override void OnResume()
    {
        base.OnResume();
        StartActivity(typeof(MainActivity));
    }
}