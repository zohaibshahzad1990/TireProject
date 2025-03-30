using Android.Graphics;
using Java.Util.Streams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZXing;
using ZXing.Mobile;

[assembly: Xamarin.Forms.Dependency (typeof (TireProject.Droid.Scanner))]

namespace TireProject.Droid
{
    public class Scanner : IScanner
    {
        #region IScanner implementation

        public async Task<string> Scan()
        {
            try
            {
                // ZXing should already be initialized in MainActivity
                // Create scanner options focused on QR codes
                var options = new ZXing.Mobile.MobileBarcodeScanningOptions
                {
                    PossibleFormats = new List<ZXing.BarcodeFormat> { ZXing.BarcodeFormat.QR_CODE },
                    TryHarder = true,
                    AutoRotate = true,
                    UseNativeScanning = true  // Try native scanning for better performance
                };

                // Create and configure scanner
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                scanner.TopText = "Align QR code within frame";
                scanner.BottomText = "Scanning...";
                scanner.CancelButtonText = "Cancel";

                // Ensure autofocus is on
                scanner.AutoFocus();

                // Start scanning
                Debug.WriteLine("Starting QR code scan...");
                var result = await scanner.Scan(options);

                if (result != null)
                {
                    Debug.WriteLine($"QR code scanned successfully: {result.Text}");
                    return result.Text;
                }
                else
                {
                    Debug.WriteLine("Scan was cancelled or failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during QR code scanning: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }

        public byte[] GenerateBarcode(string content, ZXing.BarcodeFormat bb)
        {
            var barcodeWriter = new ZXing.Mobile.BarcodeWriter
            {
                //Format = BarcodeFormat.QR_CODE,

                //Format = ZXing.BarcodeFormat.CODE_128,

                Format = bb,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 500,
                    Margin = 1
                }

            };

            var barcode = barcodeWriter.Write(content);

            byte[] bitmapData;
            using (var stream = new System.IO.MemoryStream())
            {
                barcode.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
            }

            return bitmapData;
        }

        

        


        #endregion


    }
}

