using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class LeadSourceModel
    {
        #region Constructor
        public LeadSourceModel()
        {

        }
        #endregion

        #region Properties
        //[Key ]
        //[Display(Name = "Id", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        public Int64 Id { get; set; }

        public Int64 LSId { get; set; }
                
        //[Key, Column(Order = 2)]
        //[Display(Name = "TenantId", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        public Guid TenantId { get; set; }
        //[Display(Name = "Description", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        //[Required(ErrorMessageResourceType = typeof(NIMBOLE.GlobalResources.Resources), ErrorMessageResourceName = "DescriptionError")]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Code { get; set; }
        public DateTime ModifiedDate { get; set; }
        public String Active { get; set; }
        public string Change_status { get; set; }
        public bool Status { get; set; }
        #endregion
    }
}
