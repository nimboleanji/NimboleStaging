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
    
    public partial class VWLgnEmployee
    {
        public long Id { get; set; }
        public System.Guid TenantId { get; set; }
        public Nullable<long> ReportToId { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeImageURL { get; set; }
        public Nullable<long> LoginId { get; set; }
        public long EmpRoleId { get; set; }
        public string Location { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> Status { get; set; }
        public string BornPlace { get; set; }
        public Nullable<System.DateTime> BornDate { get; set; }
        public Nullable<System.DateTime> JoinDate { get; set; }
        public Nullable<System.DateTime> ResignDate { get; set; }
        public string BankId { get; set; }
        public string BankNumber { get; set; }
        public string BankDetails { get; set; }
        public string GcmId { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string RoleCode { get; set; }
        public string Description { get; set; }
        public string SelectedModules { get; set; }
    }
}
