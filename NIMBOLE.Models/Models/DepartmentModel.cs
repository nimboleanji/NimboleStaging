using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;


namespace NIMBOLE.Models.Models
{
    public class DepartmentModel  
    {
        #region Properties
        public Int64 Id { get; set; }
        public string Code { get; set; }
        public Guid TenantId { get; set; }
        public Int64 DeptId { get; set; }        
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Active { get; set; }
        public string Change_status { get; set; }
        public bool Status { get; set; }
        #endregion

        #region Constructor
        public DepartmentModel()
        {

        }
        #endregion
    }
}
