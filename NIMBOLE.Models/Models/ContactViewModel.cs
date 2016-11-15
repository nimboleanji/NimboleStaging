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
    public class ContactViewModel
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string ContactEmail { get; set; }
        public string WorkEmail { get; set; }    
        public string AccountName { get; set; }
        public string ContactImageURL { get; set; }
        public bool Status { get; set; }
        public String Active { get; set; }
        public string Change_status { get; set; }
        public bool IsSelected { get; set; }
        #endregion
    }
       
}

