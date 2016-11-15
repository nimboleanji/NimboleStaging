using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace NIMBOLE.Models.Models
{
    public class CountryModel
    {
        public long CountryId { get; set; }
        public Guid TenantId { get; set; }
        public string CountryName { get; set; }
        public string Code { get; set; }
        public string MobileCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
    }
    public class StateModel
    {
        public long StateId { get; set; }
        public Guid TenantId { get; set; }
        public string Code { get; set; }
        public string StateName { get; set; }
        public long CountryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
    }

    public class CityModel
    {
        public long Id { get; set; }
        public Guid TenantId { get; set; }
        public string Code { get; set; }
        public long StateId { get; set; }
        public string CityName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
    }
}
