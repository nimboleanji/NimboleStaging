using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text;


namespace NIMBOLE.Models.Models
{
    public class KeyValueModel
    {
        #region Properties
        public Int64 Id { get; set; }
        public string Name { get; set; }
        #endregion
    }

    public class EmpRoleModel
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
    }

    public class KeyValueFormatModel
    {
        #region Properties
        public string Id { get; set; }
        public string Name { get; set; }
        #endregion
    }


    public class KeyValueRoleModel
    {
        #region Properties
        public Int64 Id { get; set; }
        public string Description { get; set; }
        #endregion
    }
    public class SingleValueModel
    {
        #region Properties
        public string SingleValueData { get; set; }
        #endregion
    }
    public class GeneralValuesModel
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

        #region Constructor GeneralValuesModel
        public GeneralValuesModel()
        {
            
        }
        #endregion 
    }
}
