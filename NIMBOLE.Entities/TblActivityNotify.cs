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
    
    public partial class TblActivityNotify
    {
        public long Id { get; set; }
        public System.Guid TenantId { get; set; }
        public Nullable<long> EmployeeId { get; set; }
        public Nullable<long> ActivityId { get; set; }
    
        public virtual TblActivity TblActivity { get; set; }
        public virtual TblEmployee TblEmployee { get; set; }
    }
}
