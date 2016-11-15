using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class EmployeeRoleModel //: NIMBOLE.Entities.TblEmployeeRole
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public string ERoleCode { get; set; }
        public string SelectedModules { get; set; }
        public Dictionary<string, string> dicModules { get; set; }
        public string Description { get; set; }
        public decimal RoleOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        public String Active { get; set; }
        public string Change_status { get; set; }
        public decimal ParentId { get; set; }
        public string TxtNode { get; set; }
        public string TxtOldName { get; set; }
        public string TxtNewName { get; set; }
        public string Data { get; set; }

        #endregion

        #region Override ToString()
        public override string ToString()
        {
            if (this.ERoleCode == null)
                return "-";
            string strResult = this.ERoleCode + "," + this.Description + "," + this.SelectedModules;
            return strResult;
        }
        #endregion

        public List<EmployeeRoleModel> lstEmployeeRoleModel;
                
        #region Constructor
        public EmployeeRoleModel()
        {
            dicModules = new Dictionary<string, string>();
            lstEmployeeRoleModel = new List<EmployeeRoleModel>();
            
        }
        #endregion
    }
}
