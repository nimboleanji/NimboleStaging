using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
//using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.ServiceRuntime;

using System.Configuration;

namespace NIMBOLE.UI.Controllers
{
    public class BlobStorageService
    {
        public CloudBlobContainer GetCloudBlobContainer()
        {
            try
            {
                Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("LeadFilesConnectionString"));
                CloudBlobClient storageClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer storageContainer = storageClient.GetContainerReference(ConfigurationManager.AppSettings.Get("DocumentsStorage"));

                if (storageContainer.CreateIfNotExists())
                {
                    storageContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                return storageContainer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}