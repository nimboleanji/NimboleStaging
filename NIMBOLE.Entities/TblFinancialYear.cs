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
    
    public partial class TblFinancialYear
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TblFinancialYear()
        {
            this.TblEmployeeTargets = new HashSet<TblEmployeeTarget>();
        }
    
        public long Id { get; set; }
        public System.Guid TenantId { get; set; }
        public string FinancialYear { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblEmployeeTarget> TblEmployeeTargets { get; set; }
    }
}
