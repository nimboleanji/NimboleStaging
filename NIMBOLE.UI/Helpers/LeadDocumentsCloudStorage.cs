using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mime;
using System.IO;
using NIMBOLE.Models;
using NIMBOLE.UI.Models;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.Configuration;

namespace NIMBOLE.UI.Helpers
{
    public class LeadDocumentsCloudStorage
    {
        public static readonly CloudStorageAccount cloudAccountGlobal = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("LeadFilesConnectionString"));
        public static List<DocumentViewModel> Files { get; set; }

        public LeadDocumentsCloudStorage()
        {
        }

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

        public static void CreateBlob(string fileName, string containerName, Stream streamContent)
        {
            try
            {
                string[] fileExtension = fileName.Split('.');
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
                switch (fileExtension.Last())
                {
                    case "txt": blob.Properties.ContentType = MediaTypeNames.Text.Plain;
                        break;
                    case "rtf": blob.Properties.ContentType = MediaTypeNames.Application.Rtf;
                        break;
                    case "pdf": blob.Properties.ContentType = MediaTypeNames.Application.Pdf;
                        break;
                    case "docx": blob.Properties.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case "doc": blob.Properties.ContentType = "application/vnd.msword";
                        break;
                    default: break;
                }
                blob.UploadFromStream(streamContent, options);
                blob.SetMetadata();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<DocumentViewModel> ReadBlob(string containerName)
        {
            try
            {                
                CloudBlobContainer container = BlobConfig().GetContainerReference(containerName);

                int count = container.ListBlobs().Count();
                Files = new List<DocumentViewModel>();
                if (count > 0)
                {
                    foreach (IListBlobItem item in container.ListBlobs())
                    {
                        DocumentViewModel info = DocumentViewModel.CreateFromIListBlobItem(item);
                        if (info != null)
                        {
                            Files.Add(info);
                        }
                    }
                }
                return Files;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string StoreInBlob(Stream mstream, Guid tenantId, string docName, string docType)
        {
            string fileName = string.Empty;
            string documentURL = string.Empty;

            //To store in azure blob and different containers for differnet tenatnid's
            string blobUri = ConfigurationManager.AppSettings["BlobUrl"].ToString();

            if (mstream.Length > 0)
            {
                fileName = docType + "_" + DateTime.Now.ToString("ddMMyyyyhhmmssffff") + "_" + docName;
                //containerName = "documents";

                LeadDocumentsCloudStorage.CreateBlob(fileName, tenantId.ToString(), mstream);
            }
            documentURL = blobUri + tenantId.ToString() + "/" + fileName;
            return documentURL;
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
    }
}