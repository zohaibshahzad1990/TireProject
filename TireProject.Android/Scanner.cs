using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency (typeof (TireProject.Droid.Scanner))]

namespace TireProject.Droid
{
	public class Scanner: IScanner
	{
		#region IScanner implementation

		async public Task<string> Scan ()
		{
			//NOTE: On Android you MUST pass a Context into the Constructor!
			var scanner = new ZXing.Mobile.MobileBarcodeScanner();
			var result = await scanner.Scan(MainActivity.MyActivity,ZXing.Mobile.MobileBarcodeScanningOptions.Default);

            if (result != null)
                return result.Text;
            else
                return null;
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

