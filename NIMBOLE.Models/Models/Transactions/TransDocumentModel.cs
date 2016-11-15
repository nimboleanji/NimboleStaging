using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    //public class TransLeadProductCompModel : TblTransLeadCompetitor
    //{
    //    public ProductModel objProductModel { get; set; }
    //    public CompetitorModel objCompetitorModel { get; set; }
    //}
    public class TransDocumentModel 
    {  
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public Int64 DocumentId { get; set; }
        public Int64 ActivityId { get; set; }
        public Int64 LeadId { get; set; }
        public long LeadSourceId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        #endregion

        #region Navigation Property
        public LeadModel objLeadModel { get; set; }
        public DocumentModel objDocumentModel { get; set; }
        #endregion

        #region Constructor
        public TransDocumentModel()
        {

        }
        #endregion
    }
}
