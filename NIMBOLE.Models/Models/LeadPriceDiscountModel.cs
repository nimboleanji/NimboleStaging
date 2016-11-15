using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
   public class LeadPriceDiscountModel
    {
        	#region Properties
		public Int64 Id {get; set;}
		public Guid TenantId {get; set;}
		public Int64 LeadId {get; set;}
        public DateTime DiscountedDate { get; set; }
        public decimal DiscountedPrice { get; set; }
		public Int64 EmployeeId {get; set;}
        public bool ApprovalStatus { get; set; }
		public Int64 ApprovedBy {get; set;}
		public DateTime ApprovedDate {get; set;}
		public string Comments {get; set;}
		public DateTime CreatedDate {get; set;}
		public DateTime ModifiedDate {get; set;}
		public bool Status {get; set;}
	    #endregion 

		#region Constructor
        public LeadPriceDiscountModel()
		{
			 
		}
		#endregion 
    }
}
