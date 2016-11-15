using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class TransLeadProductCompModel : NIMBOLE.Entities.TblTransLeadCompetitor
    {
        //public string DiscountType { set; get; }
        public ProductModel objProductModel { get; set; }
        public CompetitorModel objCompetitorModel { get; set; }
    }
    public class TransLProductModel 
    {
        public long Id { get; set; }
        public System.Guid TenantId { get; set; }
        public Nullable<long> CompetitorId { get; set; }
        public Nullable<long> LeadId { get; set; }
        public Nullable<long> ProductId { get; set; }
        public string Code { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
