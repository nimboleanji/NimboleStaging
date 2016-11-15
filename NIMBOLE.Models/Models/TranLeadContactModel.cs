using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models.Transactions
{
    public class TranLeadContactModel
    {
        #region Properties
        public long Id { get; set; }
        public System.Guid TenantId { get; set; }
        public Nullable<long> LeadId { get; set; }
        public Nullable<long> ContactId { get; set; }
        public Nullable<long> ContactRoleId { get; set; }
        #endregion Properties
        #region Constructor
        public TranLeadContactModel()
        { 
            
        }
        #endregion Constructor
    }
}
