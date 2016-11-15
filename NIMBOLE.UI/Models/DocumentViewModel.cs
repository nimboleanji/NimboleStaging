using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace NIMBOLE.UI.Models
{
    public class DocumentViewModel
    {
        public string DocumentName { get; set; }
        public string URL { get; set; }
        public long Size { get; set; }

        public static DocumentViewModel CreateFromIListBlobItem(IListBlobItem item)
        {
            var itemtype = item.GetType();
            var blbtype = typeof(CloudBlockBlob);
            if (item.GetType() == typeof(CloudBlockBlob))
            {
                var blob = (CloudBlockBlob)item;
                return new DocumentViewModel
                {
                    DocumentName = blob.Name,
                    URL = blob.Uri.ToString(),
                    Size = blob.Properties.Length
                };
            }
            return null;
        }
    }
}
