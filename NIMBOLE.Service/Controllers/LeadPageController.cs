using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using Microsoft.Azure;
namespace NIMBOLE.Service.Controllers
{

   
    public static class BlobHelper
    {
        public static CloudBlobContainer GetWebApiContainer()
        {
            // Retrieve storage account from connection-string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("LeadFilesConnectionString"));

            // Create the blob client 
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container 
            // Container name must use lower case
            CloudBlobContainer container = blobClient.GetContainerReference("leadfiles");

            // Create the container if it doesn't already exist
            container.CreateIfNotExist();

            // Enable public access to blob
            var permissions = container.GetPermissions();
            if (permissions.PublicAccess == BlobContainerPublicAccessType.Off)
            {
                permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                container.SetPermissions(permissions);
            }

            return container;
        }
    }
    
}
