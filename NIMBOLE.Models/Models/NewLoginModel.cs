using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;


namespace NIMBOLE.Models.Models
{
    public class NewLoginModel
    {
       
        public class LoginModel
        {
            [Required(ErrorMessage = "User Name required.")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Password required.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public class Changepassword
        {
            [Required(ErrorMessage = "Current Password is required")]
            public string CurrentPassword { get; set; }

            [Required(ErrorMessage = "New Password is required")]           
            [RegularExpression(@"((?=.*\d)(?=.*[!~+()=_^&*@#$%]).{6,10})", ErrorMessage = "Password must contain at least 1 number, 1 special character having length min 6 and max 10 characters.")]
            public string NewPassword { get; set; }

            [Required(ErrorMessage = "Confirm Password is required")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

        }

       
    }
}
