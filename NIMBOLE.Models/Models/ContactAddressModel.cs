using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class ContactAddressModel//:NIMBOLE.Entities.TblAddressContact
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public Int64 ContactId { get; set; }
        public Int64 AddressId { get; set; }
        #endregion

        #region Constructor
        public ContactAddressModel()
        {

        }
        #endregion
    }
}
