using System;
using Xamarin.Forms;
using Android.OS;
using Android.Print;
using TireProject.Droid;

[assembly:Dependency(typeof(PrintFile))]
namespace TireProject.Droid
{
    public class PrintFile : IPrint
    {
        public void printpdf(string filePath)
        {
            MyPrinter myPrinter = new MyPrinter(filePath);

            var printMgr = (Android.Print.PrintManager)MainActivity.MyActivity.GetSystemService(Android.Content.Context.PrintService);
            printMgr.Print("Print", myPrinter, null);
        }
    }

    public class MyPrinter : PrintDocumentAdapter
    {
        string sad;
        public MyPrinter(string ss)
        {
            sad = ss;
        }
        public override void OnLayout(PrintAttributes oldAttributes, PrintAttributes newAttributes, CancellationSignal cancellationSignal, LayoutResultCallback callback, Bundle extras)
        {
            if (cancellationSignal.IsCanceled)
            {
                callback.OnLayoutCancelled();
                return;
            }


            PrintDocumentInfo pdi = new PrintDocumentInfo.Builder("sample.pdf").SetContentType(Android.Print.PrintContentType.Document).Build();

            callback.OnLayoutFinished(pdi, true);
        }

        public override void OnWrite(PageRange[] pages, ParcelFileDescriptor destination, CancellationSignal cancellationSignal, WriteResultCallback callback)
        {
            Java.IO.InputStream input = null;
            Java.IO.OutputStream output = null;

            try
            {

                input = new Java.IO.FileInputStream(sad);
                output = new Java.IO.FileOutputStream(destination.FileDescriptor);

                byte[] buf = new byte[1024];
                int bytesRead;

                while ((bytesRead = input.Read(buf)) > 0)
                {
                    output.Write(buf, 0, bytesRead);
                }

                callback.OnWriteFinished(new PageRange[] { PageRange.AllPages });

            }
            catch (Java.IO.FileNotFoundException)
            {
                //Catch exception
            }
            finally
            {
                try
                {
                    input.Close();
                    output.Close();
                }
                catch (Java.IO.IOException e)
                {
                    e.PrintStackTrace();
                }
            }
        }
    }
}
