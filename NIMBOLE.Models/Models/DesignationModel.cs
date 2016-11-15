using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class DesignationModel
    {
        public Int64 Id { get; set; }
        public string DesignationCode { get; set; }
        public string TenantId { get; set; }
        public string DesignationName { get; set; }
        public string Description { get; set; }

    }
}