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
//using Artifex.MuPdf;
using TireProject.Droid;
using static Android.Graphics.Pdf.PdfRenderer;

[assembly: Xamarin.Forms.Dependency(typeof(ConvertToPng))]
namespace TireProject.Droid
{
    public class ConvertToPng : IConvertToPng
    {
        public async Task<string> Convert(string PdfPath)
        {
            if (ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.WriteExternalStorage) != Permission.Granted || ContextCompat.CheckSelfPermission(Android.App.Application.Context, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(MainActivity.MyActivity, new String[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage }, 0);
                return null;
            }
            else
            {
                ParcelFileDescriptor fileDescriptor = null;
                try
                {

                    fileDescriptor = ParcelFileDescriptor.Open(new Java.IO.File(PdfPath), ParcelFileMode.ReadOnly);


                    //initialize PDFRenderer by passing PDF file from location.
                    using (PdfRenderer renderer = new PdfRenderer(fileDescriptor))
                    {
                        int pageCount = renderer.PageCount;
                        for (int i = 0; i < pageCount; i++)
                        {
                            // Use `openPage` to open a specific page in PDF.
                            using (Page page = renderer.OpenPage(i))
                            {
                                //Creates bitmap
                                Bitmap bmp = Bitmap.CreateBitmap(page.Width, page.Height, Bitmap.Config.Argb8888);
                                bmp.EraseColor(Color.White);
                                //renderes page as bitmap, to use portion of the page use second and third parameter
                                page.Render(bmp, null, null, PdfRenderMode.ForDisplay);
                                //Save the bitmap
                                //SaveImage(bmp);


                                byte[] bb;


                                using (MemoryStream ms = new MemoryStream())
                                {
                                    bmp.Compress(Bitmap.CompressFormat.Png, 100, ms);
                                    bmp.Recycle();
                                    bb = ms.ToArray();
                                }

                                var documentsPath = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "PdfFile.png");

                                System.IO.File.WriteAllBytes(documentsPath, bb);
                                return documentsPath;
                            }
                        }
                    }


                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}
