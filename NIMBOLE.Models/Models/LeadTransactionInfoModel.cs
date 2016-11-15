using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class LeadTransactionInfoModel
    {
         #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public Int64 TransactionId { get; set; }
         public Int64 LeadId { get; set; }
         public string TransName { get; set; }
        public string Address { get; set; }
        public string PlateNumber { get; set; }
        public string ProductId { get; set; }
         public string BPKBNumber { get; set; }
         public string TenorScheme { get; set; }       
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }

        public List<string> lstURLs { get; set; }

        public string currentUser { get; set; }

        public DocumentModel objDocumentModel { get; set; }

        public List<TransInfoDocumetModel> objTransInfoDocumetModel { get; set; }

        #endregion

        public class TransInfoDocumetModel
        {
            public Int64 Id { get; set; }
            public Guid TenantId { get; set; }
            public Int64 TransactionId { get; set; }
            public Int64 DocumentId { get; set; }
            public Int64 TransId { get; set; }
            public Int64 LeadId { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime ModifiedDate { get; set; }
            public bool Status { get; set; }
        }
       

    }
}
