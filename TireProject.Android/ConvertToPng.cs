using System;
using System.IO;
using System.Threading.Tasks;
using Android;
using Android.Content;
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
                Console.WriteLine("Permission denied");
                return null;
            }

            ParcelFileDescriptor fileDescriptor = null;
            try
            {
                // Verify the PDF file exists
                if (!System.IO.File.Exists(PdfPath))
                {
                    Console.WriteLine($"PDF file does not exist: {PdfPath}");
                    return null;
                }

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
                            if (bmp == null)
                            {
                                Console.WriteLine("Failed to create bitmap");
                                return null;
                            }

                            bmp.EraseColor(Color.White);
                            // Render page as bitmap
                            page.Render(bmp, null, null, PdfRenderMode.ForDisplay);

                            string outputFileName = $"PdfFile_{DateTime.Now.Ticks}.png";
                            string outputPath = null;

                            // Try different approaches for different Android versions
                            try
                            {
                                // First approach - app-specific Pictures directory
                                var appPicturesDir = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures);
                                if (appPicturesDir != null && appPicturesDir.CanWrite())
                                {
                                    outputPath = System.IO.Path.Combine(appPicturesDir.AbsolutePath, outputFileName);
                                    Console.WriteLine($"Using app-specific pictures directory: {outputPath}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error using app-specific directory: {ex.Message}");
                            }

                            // If first approach failed, try alternative 
                            if (string.IsNullOrEmpty(outputPath))
                            {
                                try
                                {
                                    // Try Downloads directory
                                    var downloadDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
                                    if (downloadDir != null && downloadDir.CanWrite())
                                    {
                                        outputPath = System.IO.Path.Combine(downloadDir.AbsolutePath, outputFileName);
                                        Console.WriteLine($"Using Downloads directory: {outputPath}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error using Downloads directory: {ex.Message}");
                                }
                            }

                            // Final fallback
                            if (string.IsNullOrEmpty(outputPath))
                            {
                                outputPath = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, outputFileName);
                                Console.WriteLine($"Using external storage root: {outputPath}");
                            }

                            Console.WriteLine($"Final output path: {outputPath}");

                            try
                            {
                                // Create a clean Java File object
                                Java.IO.File outputFile = new Java.IO.File(outputPath);

                                // Create a memory stream to hold the bitmap data
                                using (var memStream = new MemoryStream())
                                {
                                    // Compress the bitmap to the memory stream
                                    bmp.Compress(Bitmap.CompressFormat.Png, 100, memStream);

                                    // Convert to byte array
                                    byte[] bitmapData = memStream.ToArray();

                                    // Write bytes to file using C# method
                                    System.IO.File.WriteAllBytes(outputPath, bitmapData);
                                    Console.WriteLine($"File written successfully to {outputPath}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error saving bitmap: {ex.Message}");
                                return null;
                            }

                            // Recycle bitmap to free resources
                            bmp.Recycle();

                            // Verify file was created successfully
                            if (System.IO.File.Exists(outputPath))
                            {
                                Console.WriteLine($"File created successfully: {outputPath}");

                                // Make sure the file is accessible by the media scanner
                                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                                Android.Net.Uri contentUri = Android.Net.Uri.FromFile(new Java.IO.File(outputPath));
                                mediaScanIntent.SetData(contentUri);
                                Android.App.Application.Context.SendBroadcast(mediaScanIntent);

                                return outputPath;
                            }
                            else
                            {
                                Console.WriteLine("File creation verification failed");
                                return null;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("PDF has no pages");
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
                if (fileDescriptor != null)
                {
                    fileDescriptor.Close();
                }
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