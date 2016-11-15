using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class OwnershipModel //: NIMBOLE.Entities.TblEmployeeRole
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        #endregion

        #region Constructor
        public OwnershipModel()
        {

        }
        #endregion
    }
}
