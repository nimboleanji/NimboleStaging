using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
   public class LeadProductModel
    {

        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public Int64 LeadId { get; set; }
        public Int64 ProductId { get; set; }
        public decimal Quntity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        #endregion

        #region Constructor
        public LeadProductModel()
        {

        }
        #endregion 
    }
}
