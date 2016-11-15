using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace NIMBOLE.Models.Models
{
    public class EmployeeTargetModel
    {
        #region Properties
        public Int64 Id { get; set; }
        public Int64 EmpId { get; set; }
        public Int64 EmproleId { get; set; }
        public decimal Target { get; set; }
        public Guid TenantId { get; set; }
        public Int64 FinancialYearId { get; set; }
        public Int64 EmployeeRoleId { get; set; }
        public string EmployeeRole { get; set; }
        public string FinancialYear { get; set; }
        public string Description { get; set; }
        public string Name { get; set; } 
        public decimal Budget { get; set; }
        public bool IsAutomatic { get; set; }
        public decimal QuarterlyTarget { get; set; }
        public decimal MonthlyTarget { get; set; }
        public decimal WeeklyTarget { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        public string Type { get; set; }
        public decimal TargetHike { get; set; }
        public Int64 EmployeeId { get; set; }
        #endregion

        #region Constructor
        public EmployeeTargetModel()
        {

        }
        #endregion
    }  
}

