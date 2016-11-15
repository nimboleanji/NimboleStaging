using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class RegistrationModel
    {
        public long Id { get; set; }
        public string Url { get; set; }
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Invalid email format.")]
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Siteurl { get; set; }
        public int Numberoflicenses { get; set; }
        public int TrialPeriod { get; set; }
        public Guid TenantID { get; set; }
    }
}
