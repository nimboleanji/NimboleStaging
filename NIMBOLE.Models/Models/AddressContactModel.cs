using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class AddressContactModel
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public Int64 ContactId { get; set; }
        public Int64 AddressId { get; set; }
        public String ContactImageURL { get; set; }

        #endregion
    }
}
