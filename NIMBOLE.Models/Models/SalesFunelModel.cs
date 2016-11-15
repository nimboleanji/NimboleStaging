using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class SalesFunelModel
    {
        #region Properties
        public long Id { get; set; }
        public Guid TenantId { get; set; }
        public string LeadTitle { get; set; }
        public string LeadDescription { get; set; }
        public long LeadOwnerId { get; set; }
        public long LeadSourceId { get; set; }
        public string LeadTypeId { get; set; }
        public decimal Budget { get; set; }
        public string LeadStatus { get; set; }
        public string TimeFrame { get; set; }
        public long AccountId { get; set; }
        public long Size { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int LeadStage { get; set; }
        public List<string> LeadEmployees { get; set; }
        public string MilestoneCode { get; set; }
        public string MilestoneName { get; set; }
        public string Milestone { get; set; }
        public int Count { get; set; }
        public decimal Percent { get; set; }

        #endregion

        #region Constructor
        public SalesFunelModel()
        {

        }

        #endregion
    }
}
