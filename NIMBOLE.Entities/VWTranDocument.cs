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
    
    public partial class VWTranDocument
    {
        public long Id { get; set; }
        public System.Guid TenantId { get; set; }
        public string Code { get; set; }
        public string URL { get; set; }
        public string DocumentName { get; set; }
        public Nullable<long> UploadedById { get; set; }
        public Nullable<System.DateTime> UploadDateTime { get; set; }
        public string DocumentType { get; set; }
        public Nullable<long> SizeOfDocument { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<long> DocumentId { get; set; }
        public Nullable<long> ActivityId { get; set; }
        public Nullable<long> LeadId { get; set; }
    }
}
