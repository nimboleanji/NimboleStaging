using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class SettingModel  
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }

        public string Logo { get; set; }

        public string LogoURL { get; set; }

        public string FullName { get; set; }
        public string PhoneNo { get; set; }
        public string URL { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string CurrencyCode { get; set; }
        public string ReportingCurrency { get; set; }
        public int NoOfLicenses { get; set; }
        public string LanguageCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        public string Email { get; set; }
        public string DefaultMilestone { get; set; }
        #endregion

        #region Constructor
        public SettingModel()
        {
            
        }
        #endregion

    }
}
