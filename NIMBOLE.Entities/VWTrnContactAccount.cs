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
    
    public partial class VWTrnContactAccount
    {
        public long Id { get; set; }
        public System.Guid TenantId { get; set; }
        public Nullable<long> LeadSourceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactEmail { get; set; }
        public Nullable<long> DepartmentId { get; set; }
        public string ContactImageURL { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> Status { get; set; }
        public string WorkEmail { get; set; }
        public long AccountId { get; set; }
        public string AccountName { get; set; }
    }
}
