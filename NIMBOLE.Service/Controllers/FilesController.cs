using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NIMBOLE.Service.Controllers
{
    public class AzureBlobStorageMultipartProvider : MultipartFileStreamProvider
    {
        private CloudBlobContainer _container;
        public AzureBlobStorageMultipartProvider(CloudBlobContainer container)
            : base(Path.GetTempPath())
        {
            _container = container;
            Files = new List<FileDetails>();
        }

        public List<FileDetails> Files { get; set; }

        public override Task ExecutePostProcessingAsync()
        {
            // Upload the files to azure blob storage and remove them from local disk
            foreach (var fileData in this.FileData)
            {
                string fileName = Path.GetFileName(fileData.Headers.ContentDisposition.FileName.Trim('"'));
                string extension = System.IO.Path.GetExtension(fileName);
                string result = fileName.Substring(0, fileName.Length - extension.Length);

                string formatted = DateTime.Now.ToString("MMddyyyyHHmmssfff");
                fileName = result + formatted + extension;
                // Retrieve reference to a blob
                CloudBlob blob = _container.GetBlobReference(fileName);
                blob.Properties.ContentType = fileData.Headers.ContentType.MediaType;
                blob.UploadFile(fileData.LocalFileName);
                File.Delete(fileData.LocalFileName);
                Files.Add(new FileDetails
                {
                    ContentType = blob.Properties.ContentType,
                    Name = blob.Name,
                    Size = blob.Properties.Length,
                    Location = blob.Uri.AbsoluteUri
                });
            }

            return base.ExecutePostProcessingAsync();
        }
    }
    
    public class FilesController : ApiController
        {
            public Task<List<FileDetails>> Post()
            {
                //if (!Request.Content.IsMimeMultipartContent("form-data"))
                //{
                //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                //}


                var multipartStreamProvider = new AzureBlobStorageMultipartProvider(BlobHelper.GetWebApiContainer());
                return Request.Content.ReadAsMultipartAsync<AzureBlobStorageMultipartProvider>(multipartStreamProvider).ContinueWith<List<FileDetails>>(t =>
                {
                    if (t.IsFaulted)
                    {
                        throw t.Exception;
                    }

                    AzureBlobStorageMultipartProvider provider = t.Result;
                    return provider.Files;
                });
            }

            public IEnumerable<FileDetails> Get()
            {
                CloudBlobContainer container = BlobHelper.GetWebApiContainer();
                foreach (CloudBlockBlob blob in container.ListBlobs())
                {
                    yield return new FileDetails
                    {
                        Name = blob.Name,
                        Size = blob.Properties.Length,
                        ContentType = blob.Properties.ContentType,
                        Location = blob.Uri.AbsoluteUri
                    };
                }
            }
        }

        public class FileDetails
        {
            public string Name { get; set; }
            public long Size { get; set; }
            public string ContentType { get; set; }
            public string Location { get; set; }
        }
    
}
