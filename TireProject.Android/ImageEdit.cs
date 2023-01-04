using System;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using TireProject.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(ImageEdit))]
namespace TireProject.Droid
{
    public class ImageEdit : IImageResize
    {
        
        public Task<string> ResizeImage(byte[] imageData, float width, float height)
        {
            try
            {
                BitmapFactory.Options options = new BitmapFactory.Options()
                {
                    InPurgeable = true,
                };
                Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options);
                var newfilebyte = CreateImage(originalImage, width, height);
                System.IO.File.WriteAllBytes(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "UserPic", "tempreg.png"),newfilebyte);

                return Task.FromResult(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "UserPic", "tempreg.png"));
            }
            catch
            {
                return null;
            }
        }

        public Task<string> ResizeImage(string imagePath,string output, float width, float height)
        {
            try
            {
                BitmapFactory.Options options = new BitmapFactory.Options()
                {
                    InPurgeable = true,
                };
                Bitmap originalImage = BitmapFactory.DecodeFile(imagePath, options);

                var newfilebyte = CreateImage(originalImage, width, height);
                System.IO.File.WriteAllBytes(System.IO.Path.Combine(System.IO.Path.GetTempPath(), output), newfilebyte);

                return Task.FromResult(System.IO.Path.Combine(System.IO.Path.GetTempPath(), output));
            }
            catch
            {
                return null;
            }
        }

        private byte[] CreateImage(Bitmap bitmap, float width, float height)
        {
            float newHeight = 0;
            float newWidth = 0;

            var originalHeight = bitmap.Height;
            var originalWidth = bitmap.Width;

            if (originalHeight > originalWidth)
            {
                newHeight = height;
                float ratio = originalHeight / height;
                newWidth = originalWidth / ratio;
            }
            else
            {
                newWidth = width;
                float ratio = originalWidth / width;
                newHeight = originalHeight / ratio;
            }

            int hh = (int)newHeight;
            int ww = (int)newWidth;

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(
             bitmap,
             ww,
             hh,
             true
             );

            bitmap.Recycle();

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Png, 30, ms);
                resizedImage.Recycle();
                return ms.ToArray();
            }
        }






        //private byte[] CropImage(Bitmap bitmap, float width, float height)
        //{
        //    //float newHeight = 0;
        //    //float newWidth = 0;

        //    //var originalHeight = bitmap.Height;
        //    //var originalWidth = bitmap.Width;

        //    //if (originalHeight > originalWidth)
        //    //{
        //    //    newHeight = height;
        //    //    float ratio = originalHeight / height;
        //    //    newWidth = originalWidth / ratio;
        //    //}
        //    //else
        //    //{
        //    //    newWidth = width;
        //    //    float ratio = originalWidth / width;
        //    //    newHeight = originalHeight / ratio;
        //    //}

        //    //int hh = (int)newHeight;
        //    //int ww = bitmap.Width;

        //    //       Bitmap resizedImage = Bitmap.CreateScaledBitmap(
        //    //bitmap,
        //    //ww,
        //    //hh,
        //    //true
        //    //);
        //    //Bitmap resizedImage = Bitmap.CreateBitmap(
        //     //bitmap,
        //     //0,
        //     //300,
        //     //bitmap.Width,
        //     //bitmap.Height - 300
        //     //);
        //    Bitmap resizedImage = Bitmap.CreateBitmap(
        //     bitmap,
        //     0,
        //     0,
        //     bitmap.Width,
        //     bitmap.Height
        //     );
        //    bitmap.Recycle();

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        resizedImage.Compress(Bitmap.CompressFormat.Png, 100, ms);
        //        resizedImage.Recycle();
        //        return ms.ToArray();
        //    }
        //}


    }
}
