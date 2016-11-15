using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.Net.Mime;
using System.IO;
using NIMBOLE.Models.Models;
namespace NIMBOLE.UI.Helpers
{
    public class ContactCloudStorage
    {
         public static readonly CloudStorageAccount cloudAccountGlobal = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("contactimages"));

        protected static CloudBlobClient BlobConfig()
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

    }
}

    
