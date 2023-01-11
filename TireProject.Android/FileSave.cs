using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Support.V4.Content;
using Android.Webkit;
using Java.IO;
using TireProject.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileSave))]
namespace TireProject.Droid
{
    public class FileSave : IFileSave
    {
        public void SavePicture(Stream data)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "Logo");
            Directory.CreateDirectory(documentsPath);

            string filePath = Path.Combine(documentsPath, "logo.png");

            byte[] bArray = new byte[data.Length];
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (data)
                {
                    data.Read(bArray, 0, (int)data.Length);
                }
                int length = bArray.Length;
                fs.Write(bArray, 0, length);
            }
        }

        public string GetPicture()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            string filePath = Path.Combine(documentsPath,"Logo","logo.png");

            return filePath;
        }
        public string GetPath()
        {
            var state=Android.OS.Environment.ExternalStorageState;
             var path = MainActivity.MyActivity.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
            var documentsPath = Path.Combine(path.AbsolutePath,"ExcelData");

            if (!Directory.Exists(documentsPath))
                Directory.CreateDirectory(documentsPath);

            return documentsPath;
        }

        public async Task SaveFileTxt(string data)
        {
            var path = MainActivity.MyActivity.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
            Java.IO.File sdCard = path;
            Java.IO.File dir = new Java.IO.File(sdCard.AbsolutePath + "/Backup Tire Inc");
            dir.Mkdirs();
            Java.IO.File file = new Java.IO.File(dir, DateTime.Now.ToLocalTime().ToString("yyyyMMddHHmmss") + ".txt");
            if (!file.Exists())
            {
                file.CreateNewFile();
                file.Mkdir();
                FileWriter writer = new FileWriter(file);
                // Writes the content to the file
                writer.Write(data);
                writer.Flush();
                writer.Close();
            }
        }

        public void OpenExcel(string filePath)
        {
            //Java.IO.File file = new Java.IO.File(filePath);
            //file.SetReadable(true);

            //string application = "";
            //string extension = Path.GetExtension(filePath);

            //// get mimeTye
            //switch (extension.ToLower())
            //{
            //    case ".txt":
            //        application = "text/plain";
            //        break;
            //    case ".doc":
            //    case ".docx":
            //        application = "application/msword";
            //        break;
            //    case ".pdf":
            //        application = "application/pdf";
            //        break;
            //    case ".xls":
            //    case ".xlsx":
            //        application = "application/vnd.ms-excel";
            //        break;
            //    case ".jpg":
            //    case ".jpeg":
            //    case ".png":
            //        application = "image/jpeg";
            //        break;
            //    default:
            //        application = "*/*";
            //        break;
            //}

            ////Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filePath);
            ////Android.Net.Uri uri = Android.Net.Uri.FromFile(file);


            //Intent intent = new Intent(Intent.ActionView);

            //Android.Net.Uri apkURI = FileProvider.GetUriForFile(Forms.Context.ApplicationContext, "com.appmedia.tireproject.fileprovider", file);
            ////Android.Net.Uri apkURI = FileProvider.GetUriForFile(
            ////                MainActivity.MyActivity,
            ////                MainActivity.MyActivity.ApplicationContext
            ////                .PackageName + ".provider", file);

            //intent.SetDataAndType(apkURI, application);

            if (System.IO.File.Exists(filePath))
            {
                Android.Net.Uri uri = FileProvider.GetUriForFile(Forms.Context.ApplicationContext, "com.appmedia.tireproject.fileprovider", new Java.IO.File(filePath));
                Intent intent = new Intent(Intent.ActionView);
                var mimetype = MimeTypeMap.Singleton.GetMimeTypeFromExtension(MimeTypeMap.GetFileExtensionFromUrl((string)uri).ToLower());
                intent.SetDataAndType(uri, mimetype);
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

                try
                {
                    Xamarin.Forms.Forms.Context.StartActivity(intent);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("file not found");
            }

            //intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            //intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

            
        }
    }
}
