using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Net;
using Android.OS;
using TireProject.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(ShareClass))]
namespace TireProject.Droid
{
    public class ShareClass : IShare
    {
        private readonly Context _context;
        public ShareClass()
        {
            _context = Android.App.Application.Context;
        }

        public Task Show(string title, string message, string email, string number, string filePath)
        {
            try
            {
                // For older Android versions
                StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
                StrictMode.SetVmPolicy(builder.Build());

                var extension = filePath.Substring(filePath.LastIndexOf(".") + 1).ToLower();
                var contentType = string.Empty;

                switch (extension)
                {
                    case "jpg":
                        contentType = "image/jpeg";
                        break;
                    case "png":
                        contentType = "image/png";
                        break;
                    case "pdf":
                        contentType = "application/pdf";
                        break;
                    default:
                        contentType = "application/octet-stream";
                        break;
                }

                var intent = new Intent(Intent.ActionSend);

                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    // Create a Java File object
                    Java.IO.File file = new Java.IO.File(filePath);
                    Console.WriteLine($"File exists: {file.Exists()}, Path: {filePath}");

                    // Get the authority from your manifest - match this exactly
                    string authority = "com.appmedia.tireproject.fileprovider";

                    // Get URI through FileProvider
                    Android.Net.Uri fileUri = Android.Support.V4.Content.FileProvider.GetUriForFile(
                        _context,
                        authority,
                        file);

                    Console.WriteLine($"File URI: {fileUri}");

                    intent.SetType(contentType);
                    intent.PutExtra(Intent.ExtraStream, fileUri);
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                }
                else
                {
                    intent.SetType("text/plain");
                }

                // Clipboard operations
                ClipboardManager clipboard = (ClipboardManager)MainActivity.MyActivity.GetSystemService(Context.ClipboardService);
                ClipData clip = ClipData.NewPlainText("WordKeeper", number);
                clipboard.PrimaryClip = clip;

                // Add other extras
                intent.PutExtra(Intent.ExtraText, message ?? string.Empty);
                intent.PutExtra(Intent.ExtraSubject, title ?? string.Empty);

                if (!string.IsNullOrEmpty(email))
                {
                    string[] to = { email };
                    intent.PutExtra(Intent.ExtraEmail, to);
                }

                if (!string.IsNullOrEmpty(number))
                {
                    intent.PutExtra(Intent.ExtraPhoneNumber, number);
                }

                var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
                chooserIntent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                _context.StartActivity(chooserIntent);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Share: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return Task.FromResult(false);
            }
        }
    }
}
