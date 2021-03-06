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
    
    public partial class TblLead
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TblLead()
        {
            this.TblLeadContacts = new HashSet<TblLeadContact>();
            this.TblTranLeadContacts = new HashSet<TblTranLeadContact>();
            this.TblTransDocuments = new HashSet<TblTransDocument>();
            this.TblTransLeads = new HashSet<TblTransLead>();
            this.TblTransLeadCompetitors = new HashSet<TblTransLeadCompetitor>();
            this.TblTransLeadPriceDiscounts = new HashSet<TblTransLeadPriceDiscount>();
        }
    
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
    
        public virtual TblAccount TblAccount { get; set; }
        public virtual TblLeadSource TblLeadSource { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblLeadContact> TblLeadContacts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblTranLeadContact> TblTranLeadContacts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblTransDocument> TblTransDocuments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblTransLead> TblTransLeads { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblTransLeadCompetitor> TblTransLeadCompetitors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblTransLeadPriceDiscount> TblTransLeadPriceDiscounts { get; set; }
    }
}
