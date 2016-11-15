using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models.Transactions
{
    public class TransLeadCompetitorModel 
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public Int64 CompetitorId { get; set; }
        public Int64 LeadId { get; set; }
        public Int64 ProductId { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        public List<decimal> CompPrice { get; set; }
        #endregion

        #region Constructor
        public TransLeadCompetitorModel()
        {
            CompPrice = new List<decimal>();
        }
        #endregion
    }
}
