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
    
    public partial class VWLeadLsEmp
    {
        public long Id { get; set; }
        public System.Guid TenantId { get; set; }
        public string LeadTitle { get; set; }
        public string LeadDescription { get; set; }
        public Nullable<long> LeadOwnerId { get; set; }
        public long LeadSourceId { get; set; }
        public string LeadType { get; set; }
        public Nullable<decimal> Budget { get; set; }
        public string LeadStatus { get; set; }
        public string Location { get; set; }
        public string TimeFrame { get; set; }
        public long AccountId { get; set; }
        public Nullable<long> Size { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> LeadStage { get; set; }
        public string LeadEmployees { get; set; }
        public string MilestoneStage { get; set; }
        public string Milestone { get; set; }
        public string Roles { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
