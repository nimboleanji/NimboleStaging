using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mime;
using System.IO;
using NIMBOLE.Models.Models;
using NIMBOLE.UI.Models;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.Configuration;

namespace NIMBOLE.UI.Helpers
{
    public class PhotoCloudStorage
    {

        public static readonly CloudStorageAccount cloudAccountGlobal = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("ContactImagesConnectionString"));
        public PhotoCloudStorage()
        {
        }
        public string StoreInBlob(HttpPostedFileBase uploadFile, Guid tenantGuid, int id, string type)
        {
            string fileName = string.Empty;
            string logoURL = string.Empty;
            string containerName = string.Empty;

            string blobUri = ConfigurationManager.AppSettings["ContactImageUrl"].ToString();

            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                MemoryStream ms = new MemoryStream();

                HttpPostedFileBase hpf = uploadFile;

                string savedFileName = Path.Combine(
                   AppDomain.CurrentDomain.BaseDirectory,
                   Path.GetFileName(hpf.FileName));

                hpf.SaveAs(savedFileName);

                byte[] byteImageData = System.IO.File.ReadAllBytes(savedFileName);

                Stream slideshowStream = new MemoryStream(byteImageData);

                fileName = type + id.ToString() + "-" + tenantGuid.ToString();

                containerName = "contacts";

                CreateBlob(fileName, containerName, slideshowStream);
            }

            logoURL = blobUri + fileName;

            return logoURL;
        }
        public static void CreateBlob(string fileName, string containerName, Stream imageStreamContent)
        {
            try
            {
                BlobRequestOptions options = new BlobRequestOptions();
                options.Timeout = new TimeSpan(1, 0, 0);
                options.RetryPolicy = RetryPolicies.Retry(20, new TimeSpan(0, 0, 2));
                CloudBlobContainer cloudBlob = null;
                cloudBlob = BlobConfig().GetContainerReference(containerName);
                cloudBlob.CreateIfNotExist();
                BlobContainerPermissions blobPermission = new BlobContainerPermissions();
                blobPermission.PublicAccess = BlobContainerPublicAccessType.Container;
                cloudBlob.SetPermissions(blobPermission);
                CloudBlob blob = cloudBlob.GetBlobReference(fileName);
                blob.DeleteIfExists();
                blob.Properties.ContentType = MediaTypeNames.Image.Jpeg;
                blob.UploadFromStream(imageStreamContent, options);
                // blob.Metadata["lnno"] = ln.ToString();
                blob.SetMetadata();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void DeleteBlob(string containerName, string blobURL)
        {
            try
            {
                CloudBlobContainer cloudBlob = null;
                cloudBlob = BlobConfig().GetContainerReference(containerName);

                CloudBlob blob = cloudBlob.GetBlobReference(blobURL);
                blob.DeleteIfExists();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static CloudBlobClient BlobConfig()
        {
            CloudBlobClient cloudClient = null;
            try
            {
                cloudClient = cloudAccountGlobal.CreateCloudBlobClient();
            }
            catch (Exception)
            {
                throw;
            }
            return cloudClient;
        }
    }
}



