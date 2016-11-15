using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    class TransEmployeetargetModel
    {
        public Int64 Id { get; set; }
        public Int64 EmpId { get; set; }
        public Int64 EmproleId { get; set; }
        public decimal Target { get; set; }
        public Int64 FinancialYearId { get; set; }
        //public decimal TargetHike { get; set; }
        public bool IsAutomatic { get; set; }
        public decimal QuarterlyTarget { get; set; }
        public decimal MonthlyTarget { get; set; }
        public decimal WeeklyTarget { get; set; }
        public string EmployeeRole { get; set; }
        public string FinancialYear { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

    }
}
