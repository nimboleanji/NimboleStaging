using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class TransLoginModel
    {
        
        #region Properties
        
        #region Navigation Properties
     
        public LoginModel objLoginModel { get; set; }

        public EmployeeModel objEmployeeModel { get; set; }

        public EmployeeRoleModel objRoleModel { get; set; }
        
        #endregion
        #endregion
    }
}
