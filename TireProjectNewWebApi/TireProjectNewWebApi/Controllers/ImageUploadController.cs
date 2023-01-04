using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace TireProjectNewWebApi.Controllers
{
    [Route("api/[controller]")]
    public class UploadPics : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        public UploadPics(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public class FIleUploadAPI
        {
            public IFormFile Files { get; set; }
        }

        [HttpPost]
        public async Task<string> Post(FIleUploadAPI filess)
        {
            try
            {
                var baseUrl = Request.Host.Value;
                if (!Directory.Exists(Path.Combine(_environment.WebRootPath, "Pics")))
                {
                    Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, "Pics"));
                }

                var uploadLocation = Path.Combine(_environment.WebRootPath, "Pics");
                var fileName = filess.Files.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();
                var fullpath = Path.Combine(_environment.WebRootPath, "Pics", fileName);

                if (filess.Files.Length > 0)                 {                     using (var stream = new FileStream(Path.Combine(uploadLocation, fileName), FileMode.Create))                     {                         await filess.Files.CopyToAsync(stream);
                        stream.Flush();
                        return baseUrl + "/" + "Pics" + "/" + fileName;                     }                 }
                else
                {
                    return "Failed";
                }
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}




