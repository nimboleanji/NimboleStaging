using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class EmpHierarchyModel
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public string EDescription { get; set; }
        public Int32 ParentId { get; set; }
        public string TxtNode { get; set; }
        public string TxtOldName { get; set; }
        public string TxtNewName { get; set; }
        public string Data { get; set; }

        #endregion

        public List<EmpHierarchyModel> lstEHierarchyModel;

        #region Constructor
        public EmpHierarchyModel()
        {
            lstEHierarchyModel = new List<EmpHierarchyModel>();
        }
        #endregion
    }
}
