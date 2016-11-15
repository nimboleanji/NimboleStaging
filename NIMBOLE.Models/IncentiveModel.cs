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
    public class IncentiveModel
    {
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public string IncFrom { get; set; }
        public string IncTo { get; set; }
        public string Percentage { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }

        public string Active { get; set; }
        public string Change_status { get; set; }
    }
}
