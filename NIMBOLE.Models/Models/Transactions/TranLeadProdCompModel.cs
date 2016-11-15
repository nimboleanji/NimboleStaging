using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class TranLeadProdCompModel
    {
        public ProductModel Prod { get; set; }
        public long LeadId { get; set; }
        public Guid TenantId { get; set; }
        public Int64 Id { get; set; }
        public Int64 LeadValue { get; set; }
        public Int64 Pro1RowId { get; set; }
        public Int64 Com1RowId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public Int64 ProdId { get; set; }
        public decimal Price { get; set; }
		public Int64 Quantity { get; set; }
        public decimal Amount { get; set; }
        public Int64 Comp1ProdId { get; set; }
		public string Comp1Name { get; set; }       
        public decimal Comp1Price { get; set; }
        public decimal Discount { get; set; }
        public string DiscountType { get; set; }
        public List<CompetitorModel> lstCompetitorModel { get; set; }
        public CompetitorModel Comp1 { get; set; }

        public string TempTransactionType { get; set; }
        public TranLeadProdCompModel()
        {
            lstCompetitorModel = new List<CompetitorModel>();
        }
    }
}
