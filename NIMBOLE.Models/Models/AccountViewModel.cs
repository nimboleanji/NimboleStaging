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
    public class AccountViewModel
    {
        #region Properties
        public Int64 Id { get; set; }
        public bool IsSelected { get; set; }
        public Guid TenantId { get; set; }
        public string AccountName { get; set; }
        public string AccountOwner { get; set; }
        public Int32 NoofContacts { get; set; }
        public string Active { get; set; }
        public string Change_status { get; set; }
        public bool Status { get; set; }

        
        #endregion
    }
       
}

