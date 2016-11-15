using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class CompetitorModel  
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Int64 LeadId { get; set; }
        public Int64 RowId { get; set; }
        public Int64 ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        #endregion

        #region Constructor
        public CompetitorModel()
        {

        }
        #endregion
    }
}
