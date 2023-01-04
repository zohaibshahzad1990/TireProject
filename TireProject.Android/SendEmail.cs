using System;
using System.Threading.Tasks;
using Android.Content;
using TireProject.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(SendEmail))]
namespace TireProject.Droid
{
    public partial class SendEmail : InterfaceEmail
    {
        public Task sendEmailFunc(string subject, string message,string pathName)
        {

            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(Android.App.Application.Context, Android.Manifest.Permission.ReadExternalStorage) != Android.Content.PM.Permission.Granted || Android.Support.V4.Content.ContextCompat.CheckSelfPermission(Android.App.Application.Context, Android.Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
            {
                Android.Support.V4.App.ActivityCompat.RequestPermissions(MainActivity.MyActivity, new String[] { Android.Manifest.Permission.ReadExternalStorage, Android.Manifest.Permission.WriteExternalStorage }, 0);
            }
            var bb = System.IO.File.ReadAllBytes(pathName);
            var externalPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            externalPath = System.IO.Path.Combine(externalPath, "tireinc.pdf");
            System.IO.File.WriteAllBytes(externalPath, bb);


            var emailIntent = new Intent(Android.Content.Intent.ActionSend);
            //emailIntent.SetType("text/plain");
            emailIntent.SetType("image/*");
            //emailIntent.SetType("application/pdf");
            //emailIntent.SetData(Android.Net.Uri.Parse("mailto:"));



            emailIntent.PutExtra(Intent.ExtraStream,Android.Net.Uri.Parse("file://"+externalPath));
            emailIntent.PutExtra(Android.Content.Intent.ExtraEmail, new[] { "" });
            emailIntent.PutExtra(Android.Content.Intent.ExtraSubject, subject);
            emailIntent.PutExtra(Android.Content.Intent.ExtraText, message);

            emailIntent.PutExtra(Android.Content.ActivityFlags.ClearTop.ToString(), true);
            emailIntent.PutExtra(Android.Content.ActivityFlags.NewTask.ToString(), true);

            MainActivity.MyActivity.StartActivity(Intent.CreateChooser(emailIntent, "Send E-Mail"));

            return Task.FromResult(true);
        }

        //public void SendSMS(string phone,string mesage)
        //{
        //    Android.Net.Uri uri = Android.Net.Uri.Parse("smsto:"+ phone);
        //    Intent smsIntent = new Intent(Intent.ActionSendto, uri);
        //    smsIntent.PutExtra("sms_body", "The SMS text");

        //    smsIntent.PutExtra(Android.Content.ActivityFlags.ClearTop.ToString(), true);
        //    smsIntent.PutExtra(Android.Content.ActivityFlags.NewTask.ToString(), true);

        //    MainActivity.MyActivity.StartActivity(Intent.CreateChooser(smsIntent, "Send E-Mail"));
        //}
    }
}