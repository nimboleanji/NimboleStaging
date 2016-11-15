using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class AddressModel 
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public string HouseNo { get; set; }
        public string StreetName { get; set; }

        public Int64 CountryId { get; set; }
        public string Country { get; set; }

        public Int64 StateId { get; set; }
        public string State { get; set; }

        public Int64 CityId { get; set; }
		public string City { get; set; }

        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string HomePhone { get; set; }
       // public string OtherPhone { get; set; }
        public string Fax { get; set; }
        //public string Email { get; set; }
        //public string SecondaryEmail { get; set; }
        public string SkypeName { get; set; }
        public string Address_Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        #endregion

        #region Constructor
        public AddressModel()
        {

        }
        #endregion

        
    }
}
