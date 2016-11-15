using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class EmployeeAddressModel
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public Int64 AddressId { get; set; }
        public Int64 EmpId { get; set; }
        #endregion
        #region Constructor
        public EmployeeAddressModel()
        {

        }
        #endregion
  
    }
}
