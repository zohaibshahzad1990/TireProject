using System;
using System.IO;
using System.Threading.Tasks;
using Android;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Pdf;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using TireProject.Droid;
using static Android.Graphics.Pdf.PdfRenderer;

[assembly: Xamarin.Forms.Dependency(typeof(ConvertToPng))]
namespace TireProject.Droid
{
    public class ConvertToPng : IConvertToPng
    {
        private TaskCompletionSource<bool> permissionTaskCompletionSource;
        private static int STORAGE_PERMISSION_CODE = 1000;

        public async Task<string> Convert(string PdfPath)
        {
            // Check and request permissions properly with await
            if (!await CheckAndRequestPermissionsAsync())
            {
                return null; // Permission denied
            }

            ParcelFileDescriptor fileDescriptor = null;
            try
            {
                fileDescriptor = ParcelFileDescriptor.Open(new Java.IO.File(PdfPath), ParcelFileMode.ReadOnly);

                using (PdfRenderer renderer = new PdfRenderer(fileDescriptor))
                {
                    int pageCount = renderer.PageCount;
                    if (pageCount > 0)
                    {
                        // Just convert the first page for simplicity
                        using (Page page = renderer.OpenPage(0))
                        {
                            // Create bitmap
                            Bitmap bmp = Bitmap.CreateBitmap(page.Width, page.Height, Bitmap.Config.Argb8888);
                            bmp.EraseColor(Color.White);
                            // Render page as bitmap
                            page.Render(bmp, null, null, PdfRenderMode.ForDisplay);

                            byte[] bitmapData;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                bmp.Compress(Bitmap.CompressFormat.Png, 100, ms);
                                bmp.Recycle();
                                bitmapData = ms.ToArray();
                            }

                            // Use app-specific directory rather than external storage root
                            string fileName = "PdfFile.png";
                            string outputPath;

                            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                            {
                                // For Android 10 and above, use app-specific directory
                                outputPath = System.IO.Path.Combine(Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath, fileName);
                            }
                            else
                            {
                                // For older Android versions
                                var documentsPath = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,
                                    Android.OS.Environment.DirectoryDownloads);
                                Directory.CreateDirectory(documentsPath); // Ensure directory exists
                                outputPath = System.IO.Path.Combine(documentsPath, fileName);
                            }

                            // Write the file
                            File.WriteAllBytes(outputPath, bitmapData);
                            return outputPath;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error converting PDF: {e.Message}");
                Console.WriteLine(e.StackTrace);
                return null;
            }
            finally
            {
                fileDescriptor?.Close();
            }

            return null;
        }

        private async Task<bool> CheckAndRequestPermissionsAsync()
        {
            // For Android 10+ (API 29+), we don't need to request external storage permissions
            // for app-specific directories
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            {
                return true;
            }

            // For older Android versions, check and request permissions
            if (ContextCompat.CheckSelfPermission(Android.App.Application.Context,
                Manifest.Permission.WriteExternalStorage) == Permission.Granted &&
                ContextCompat.CheckSelfPermission(Android.App.Application.Context,
                Manifest.Permission.ReadExternalStorage) == Permission.Granted)
            {
                return true;
            }

            // Create a task completion source to await permission result
            permissionTaskCompletionSource = new TaskCompletionSource<bool>();

            // Register the permission callback in MainActivity
            MainActivity.RequestPermissionAction = (requestCode, permissions, grantResults) =>
            {
                if (requestCode == STORAGE_PERMISSION_CODE)
                {
                    bool allGranted = true;
                    for (int i = 0; i < grantResults.Length; i++)
                    {
                        if (grantResults[i] != Permission.Granted)
                        {
                            allGranted = false;
                            break;
                        }
                    }
                    permissionTaskCompletionSource.SetResult(allGranted);
                }
            };

            // Request permissions
            ActivityCompat.RequestPermissions(MainActivity.MyActivity,
                new String[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage },
                STORAGE_PERMISSION_CODE);

            // Await the result
            return await permissionTaskCompletionSource.Task;
        }
    }
}