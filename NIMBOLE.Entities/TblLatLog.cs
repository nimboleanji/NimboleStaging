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
    
    public partial class TblLatLog
    {
        public long Id { get; set; }
        public System.Guid TenantId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Nullable<System.DateTime> PostDateTime { get; set; }
        public Nullable<long> ActivityId { get; set; }
        public Nullable<long> LeadId { get; set; }
        public Nullable<long> EmpId { get; set; }
    }
}
