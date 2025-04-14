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
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

[assembly: Xamarin.Forms.Dependency(typeof(TireProject.Droid.Scanner))]
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

        public byte[] GenerateBarcode(string content, ZXing.BarcodeFormat format)
        {
            try
            {
                // Use higher resolution for better scanning
                var barcodeWriter = new ZXing.Mobile.BarcodeWriter
                {
                    Format = format,
                    Options = new QrCodeEncodingOptions
                    {
                        Width = 600, // Increased from 300 for higher resolution
                        Height = 600, // Increased from 300 for higher resolution
                        Margin = 1,
                        ErrorCorrection = ErrorCorrectionLevel.H, // Higher error correction
                        DisableECI = true,
                        CharacterSet = "UTF-8"
                    }
                };

                var barcode = barcodeWriter.Write(content);

                // Ensure we're getting a high-quality PNG
                byte[] bitmapData;
                using (var stream = new System.IO.MemoryStream())
                {
                    barcode.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 100, stream);
                    bitmapData = stream.ToArray();
                }

                return bitmapData;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error generating barcode: {ex.Message}");
                return null;
            }
        }
        #endregion
    }
}