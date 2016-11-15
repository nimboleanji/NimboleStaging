//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NIMBOLE.Models.Models
//{
//    public class OrgHierarchyModel
//    {
//        #region Properties
//        public Int64 Id { get; set; }
//        public Guid TenantId { get; set; }
//        public string OrgFullName { get; set;}
//        public string TxtNode { get; set; }
//        public string TxtOldName { get; set; }
//        public string TxtNewName { get; set; }
//        public Int64 ParentId { get; set; }
//        #endregion

//        public List<OrgHierarchyModel> lstOrgHierarchyModel;

//        #region Constructor
//        public OrgHierarchyModel()
//        {
//            lstOrgHierarchyModel = new List<OrgHierarchyModel>();
//        }
//        #endregion
//    }
//}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class OrgHierarchyModel
    {
        #region Properties
        public new Int64 Id { get; set; }
        public new Guid TenantId { get; set; }
        public new string OrgFullName { get; set; }
        public string TxtNode { get; set; }
        public string TxtOldName { get; set; }
        public string TxtNewName { get; set; }
        public new Int64 ParentId { get; set; }
        public new Int64 LevelDepth { get; set; }
        #endregion

        public List<OrgHierarchyModel> lstOrgHierarchyModel;

        #region Constructor
        public OrgHierarchyModel()
        {
            lstOrgHierarchyModel = new List<OrgHierarchyModel>();
        }
        #endregion
    }
}
