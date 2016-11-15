using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class MySettingModel
    {
        #region Constructor
        public MySettingModel()
        {

        }
        #endregion

        #region Properties
        public Int64 Id { get; set; }
        [Display(Name = "ProfileName", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        [Required(ErrorMessageResourceType = typeof(NIMBOLE.GlobalResources.Resources), ErrorMessageResourceName = "ProfileNameError")]
        [StringLength(50, ErrorMessageResourceType = typeof(NIMBOLE.GlobalResources.Resources), ErrorMessageResourceName = "ProfileNameError")]
        public string SettingName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string TimeZone { get; set; }
        public string CompanyName { get; set; }
        public string LanguageName { get; set; }
        public string LanguageCode { get; set; }
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "User Name required.")]
        //[System.Web.Mvc.Remote("doesUserNameExist", "Account", HttpMethod = "POST", ErrorMessage = "User Name not available")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string RetypePassword { get; set; }
        public string UserType { get; set; }
        public Guid TenantId { get; set; }

        #endregion
    }
}
