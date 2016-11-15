using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIMBOLE.UI.Models
{
    public class CloudFilesModel
    {
        public CloudFilesModel()
            : this(null)
        {
            Files = new List<DocumentViewModel>();
        }

        public CloudFilesModel(IEnumerable<IListBlobItem> list)
        {
            int count = list.Count();
            Files = new List<DocumentViewModel>();
            if (list != null && list.Count<IListBlobItem>() > 0)
            {
                foreach (var item in list)
                {
                    DocumentViewModel info = DocumentViewModel.CreateFromIListBlobItem(item);
                    if (info != null)
                    {
                        Files.Add(info);
                    }
                }
            }
        }
        public List<DocumentViewModel> Files { get; set; }
    }
}