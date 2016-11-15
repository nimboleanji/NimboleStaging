using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace NIMBOLE.Models.Models
{
   public class DocumentModel  
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public string URL { get; set; }
        public List<string> URLs { get; set; }
        public string DocumentName { get; set; }
        public Int64 UploadedById { get; set; }
        public DateTime UploadDateTime { get; set; }
        public string DocumentType { get; set; }
        public Int64 SizeOfDocument { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }

        public Int64 TransactionId { get; set; }
        public Int64 DocumentId { get; set; }
        #endregion

        #region Constructor
        public DocumentModel()
        {
            URLs = new List<string>();
        }
        #endregion 
    }

   public class DocumentWithMultipleUrlsModel
   {
       #region Properties
       public long Id { get; set; }
       public List<long> lstUrlBasedId { get; set; }
       public string DocumentName { get; set; }
       public List<string> lstDocumentUrl { get; set; }
       public DateTime CreatedDate { get; set; }
       public DateTime ModifiedDate { get; set; }
       #endregion

       #region Constructor
       public DocumentWithMultipleUrlsModel()
       {
           lstUrlBasedId = new List<long> { };
           lstDocumentUrl = new List<string> { };
       }
       #endregion
   }
}
