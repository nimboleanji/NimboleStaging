using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class TransContactModel  
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public Int64 ContactId { get; set; }
        //[Display(Name = "Accounts", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        //[Required(ErrorMessageResourceType = typeof(NIMBOLE.GlobalResources.Resources), ErrorMessageResourceName = "Required")]
        public Int64 AccountId { get; set; }
        public Int64 ContactRoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        #endregion

        #region Constructor
        public TransContactModel()
        {

        }
        #endregion
    }
}
