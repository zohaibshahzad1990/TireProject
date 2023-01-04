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
                    contentType = "application/octetstream";
                    break;
            }

            var intent = new Intent(Intent.ActionSend);
            if(!string.IsNullOrWhiteSpace(filePath))
            {
                intent.SetType(contentType);
                intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + filePath));
            }
            else
            {
                intent.SetType("text/plain");
                //intent.SetType("message/rfc822");
            }

            ClipboardManager clipboard = (ClipboardManager)MainActivity.MyActivity.GetSystemService(Context.ClipboardService);
            ClipData clip = ClipData.NewPlainText("WordKeeper", number);
            clipboard.PrimaryClip = clip;

            intent.PutExtra(Intent.ExtraText, message ?? string.Empty);
            intent.PutExtra(Intent.ExtraSubject, title ?? string.Empty);
            intent.PutExtra(Intent.ExtraPhoneNumber, number);
            string[] to = new string[] { email };

            intent.PutExtra(Intent.ExtraEmail, to);




            //intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);
            _context.StartActivity(chooserIntent);




            return Task.FromResult(true);
        }
    }
}
