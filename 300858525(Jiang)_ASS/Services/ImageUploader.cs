using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;


namespace _300858525_Jiang__ASS.Services
{
    public class ImageUploader
    {

        private readonly string _bucketName;
        private readonly StorageClient _storageClient;

        public ImageUploader(string bucketName)
        {
            _bucketName = bucketName;
            // [START storageclient]
            _storageClient = StorageClient.Create();
            // [END storageclient]
        }

        // [START uploadimage]
        public async Task<String> UploadImage(IFormFile image, long id)
        {
            //var imageAcl = PredefinedObjectAcl.PublicRead;

            ////using (var stream = new System.IO.FileStream(filePath, .Create))
            ////{
            ////    await formFile.CopyToAsync(stream);
            ////}

            //var imageObject = await _storageClient.UploadObjectAsync(
            //    bucket: _bucketName,
            //    objectName: id.ToString(),
            //    contentType: image.ContentType,
            //    source: image.OpenReadStream(),
            //    options: new UploadObjectOptions { PredefinedAcl = imageAcl }
            //);
            var filePath = Path.GetTempFileName();

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
                var _storageClient = StorageClient.Create();
                var imageAcl = PredefinedObjectAcl.PublicRead;

                //using (var stream = new System.IO.FileStream(filePath, .Create))
                //{
                //    await formFile.CopyToAsync(stream);
                //}

                var imageObject = await _storageClient.UploadObjectAsync(
                    bucket: _bucketName,
                    objectName: id.ToString(),
                    contentType: image.ContentType,
                    source: stream,
                    options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                );

                return imageObject.MediaLink;
            }

           
        }
        // [END uploadimage]


        public void DownloadObject(string bucketName, string objectName,
string localPath = null)
        {
            var storage = StorageClient.Create();
            using (var stream = System.IO.File.OpenWrite(objectName+".mp4"))
            {
                storage.DownloadObject(bucketName, objectName, stream);
            }
            Console.WriteLine($"downloaded {objectName} to {localPath}.");
        }

        public async Task DeleteUploadedImage(long id)
        {
            try
            {
                await _storageClient.DeleteObjectAsync(_bucketName, id.ToString());
            }
            catch (Google.GoogleApiException exception)
            {
                // A 404 error is ok.  The image is not stored in cloud storage.
                if (exception.Error.Code != 404)
                    throw;
            }
        }
    }


}
