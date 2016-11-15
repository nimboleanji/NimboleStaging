using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIMBOLE.Entities;

namespace NIMBOLE.Models.Models
{
    public class LeadModel//:NIMBOLE.Entities.TblLead
    {
        #region Properties
        public long Id { get; set; }
        public Guid TenantId { get; set; }
        public string LeadTitle { get; set; }
        public string LeadDescription { get; set; }
        public long LeadOwnerId { get; set; }
        public string LeadOwnerName { get; set; }
        public long LeadSourceId { get; set; }
        public string LeadSourceName { get; set; }
        public string LeadType { get; set; }
        public decimal Budget { get; set; }
        public string LeadStatus { get; set; }
        public string MileStoneStage { get; set; }
        public string MasterMilestoneStage { get; set; }
        public string Location { get; set; }
        public string TimeFrame { get; set; }
        public long AccountId { get; set; }
        public long Size { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        public int LeadStage { get; set; }
        public List<string> LeadEmployees { get; set; }
        public string Active { get; set; }
        public string Change_status { get; set; }
        public string MilestoneCode { get; set; }
        public string MilestoneName { get; set; }
        public bool IsSelected { get; set; }
        public bool ModuleStatus { get; set; }
        public string ProductString { get; set; }
        public string TransactionString { get; set; }
        public string ContactJsonArray { get; set;}
        public LeadTransactionInfoModel objLeadTransactionInfoModel { get; set; }
       
        #endregion

        #region Navigation Properties
        public List<TblActivity> lstTblActivity { get; set; }
        public List<ActivityModel> lstActivityModel { get; set; }
        public ActivityModel objActivityModel { get; set; }
        public DocumentModel objDocumentModel { get; set; }
        public LeadPriceDiscountModel objLeadPriceDiscountModel { get; set; }
        public TransLeadProductCompModel objTransLeadProductCompModel { get; set; }

        #endregion

        #region Constructor
        public LeadModel()
        {

        }
        #endregion
    }
}
