using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using _300858525_Jiang__ASS.Models;
using Google.Cloud.Storage.V1;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace _300858525_Jiang__ASS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Download()
        {
            ViewData["Message"] = "Your Download page.";

            return View();
        }

        public IActionResult Upload()
        {
            ViewData["Message"] = "Your Upload page.";

            return View();
        }

        private void DownloadObject(string bucketName, string objectName,
    string localPath = null)
        {
            var storage = StorageClient.Create();
            using (var stream = System.IO.File.OpenWrite("DownloadedSpanner.mp4"))
            {
                storage.DownloadObject(bucketName, "123", stream);
            }
            //Console.WriteLine($"downloaded {objectName} to {localPath}.");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost("Home/")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                        var _storageClient = StorageClient.Create();
                        var imageAcl = PredefinedObjectAcl.PublicRead;

                        //using (var stream = new System.IO.FileStream(filePath, .Create))
                        //{
                        //    await formFile.CopyToAsync(stream);
                        //}

                        var imageObject = await _storageClient.UploadObjectAsync(
                            bucket: "book_hao",
                            objectName: "123",
                            contentType: formFile.ContentType,
                            source: stream,
                            options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        );
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePath });
        }

        #region snippet1
        // 1. Disable the form value model binding here to take control of handling 
        //    potentially large files.
        // 2. Typically antiforgery tokens are sent in request body, but since we 
        //    do not want to read the request body early, the tokens are made to be 
        //    sent via headers. The antiforgery token filter first looks for tokens
        //    in the request header and then falls back to reading the body.
       
        #endregion
    }
}
