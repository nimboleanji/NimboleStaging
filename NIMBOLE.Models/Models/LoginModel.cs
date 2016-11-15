using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NIMBOLE.Models.Models
{
    public class LoginModel
    {
        #region Properties
        public  Int64 Id { get; set; }
        public  Guid TenantId { get; set; }
        public  string EmailAddress { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [AllowHtml]
        public  string Password { get; set; }
        public  DateTime CreatedDate { get; set; }
        public  DateTime ModifiedDate { get; set; }
        public  bool Status { get; set; }
        public Int64 LoginId { get; set; }
        public string GcmId { get; set; }

        #endregion

        #region Constructor
        public LoginModel()
        {

        }
        #endregion 
    }
}
