//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NIMBOLE.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class TblSetting
    {
        public long Id { get; set; }
        public System.Guid TenantId { get; set; }
        public string FullName { get; set; }
        public string PhoneNo { get; set; }
        public string URL { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string CurrencyCode { get; set; }
        public string ReportingCurrency { get; set; }
        public string DefaultEmail { get; set; }
        public string DefaultMilestone { get; set; }
        public Nullable<int> NoOfLicenses { get; set; }
        public string LanguageCode { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Password { get; set; }
    }
}
