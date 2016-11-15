using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class ActualVsExpectedModel
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public string Month { get; set; }
        public Int64 FinancialYearId { get; set; }
        public string FinancialYear { get; set; }
        public Decimal Target { get; set; }
        public Decimal Yearly { get; set; }
        public Decimal Quarterly { get; set; }
        public Decimal Weekly { get; set; }
        public Decimal Monthly { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64 ActualTarget { get; set; }
        public Decimal Percent { get; set; }

        #endregion

        #region Constructor
        public ActualVsExpectedModel()
        {

        }

        #endregion
    }
}
