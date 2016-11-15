using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace NIMBOLE.Models.Models
{
    public class ExtContactModel
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public Int64 LeadId { get; set; }
        public Int64 ExtContactId { get; set; }
        public Int64 ExtContactRoleId { get; set; }
        public string ExtContactRole { get; set; }
        public string FullName { get; set; }
        public string ContactEmail { get; set; }
        public string WorkEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        #endregion

        public ExtContactModel()
        {
        }
    }
}

