using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models.Transactions.TransUser
{
    public class TansUserModel
    {
        public UserModel objUserModel { get; set; }

        public LoginModel objLoginModel { get; set; }

        public LoginHistoryModel objLoginHistoryModel { get; set; }
    }
}
