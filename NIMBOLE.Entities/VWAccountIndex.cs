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
    
    public partial class VWAccountIndex
    {
        public long id { get; set; }
        public string accountname { get; set; }
        public string accountowner { get; set; }
        public System.Guid tenantid { get; set; }
        public Nullable<int> NoofContacts { get; set; }
        public Nullable<bool> status { get; set; }
    }
}