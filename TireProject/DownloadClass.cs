using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TireProject
{
    public static class FileDownloader
    {
        private const string GOOGLE_DRIVE_DOMAIN = "drive.google.com";
        private const string GOOGLE_DRIVE_DOMAIN2 = "https://drive.google.com";

        public static Task<long> GetFileSize(string url)
        {
            Int64 bytes_total;
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                client.OpenRead(url);
                bytes_total = Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
            }
            return Task.FromResult(bytes_total);
        }


        public static async Task<bool> DownloadFile(string url, string path, WebClient web)
        {
            if (url.StartsWith(GOOGLE_DRIVE_DOMAIN) || url.StartsWith(GOOGLE_DRIVE_DOMAIN2))
            {
                var fileInfo = await DownloadGoogleDriveFileFromURLToPath(url, path, web);
                if (fileInfo != null)
                    return true;
                else
                    return false;

            }

            else
            {
                var fileInfo = await DownloadFileFromURLToPath(url, path, web);
                if (fileInfo != null)
                    return true;
                else
                    return false;
            }
        }

        private static async Task<FileInfo> DownloadFileFromURLToPath(string url, string path, WebClient web)
        {
            try
            {
                //return null;
                await web.DownloadFileTaskAsync(new Uri(url), path);
                //FileInfo fileII=null;
                //using(fileII=new FileInfo(path) ) 
                //{

                //}
                return new FileInfo(path);
            }
            catch (WebException)
            {
                return null;
            }
        }

        // Downloading large files from Google Drive prompts a warning screen and
        // requires manual confirmation. Consider that case and try to confirm the download automatically
        // if warning prompt occurs
        private static async Task<FileInfo> DownloadGoogleDriveFileFromURLToPath(string url, string path, WebClient web)
        {

            // Sometimes Drive returns an NID cookie instead of a download_warning cookie at first attempt,
            // but works in the second attempt
            for (int i = 0; i < 2; i++)
            {
                var downloadedFile = await DownloadFileFromURLToPath(url, path, web);
                if (downloadedFile == null)
                    return null;

                // Confirmation page is around 50KB, shouldn't be larger than 60KB
                if (downloadedFile.Length > 60000)
                    return downloadedFile;


                // Downloaded file might be the confirmation page, check it
                string content;
                using (var reader = downloadedFile.OpenText())
                {
                    // Confirmation page starts with <!DOCTYPE html>, which can be preceeded by a newline
                    char[] header = new char[20];
                    int readCount = reader.ReadBlock(header, 0, 20);
                    if (readCount < 20 || !(new string(header).Contains("<!DOCTYPE html>")))
                        return downloadedFile;

                    content = reader.ReadToEnd();
                }

                int linkIndex = content.LastIndexOf("href=\"/uc?");
                if (linkIndex < 0)
                    return downloadedFile;

                linkIndex += 6;
                int linkEnd = content.IndexOf('"', linkIndex);
                if (linkEnd < 0)
                    return downloadedFile;

                url = "https://drive.google.com" + content.Substring(linkIndex, linkEnd - linkIndex).Replace("&amp;", "&");
            }

            var downloadedFile1 = await DownloadFileFromURLToPath(url, path, web);

            return downloadedFile1;
        }
    }
}
