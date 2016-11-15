using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class UserAddressModel : NIMBOLE.Entities.TblUserAddress
    {
        #region Properties
        public new Int64 Id { get; set; }
        public new Guid TenantId { get; set; }
        public new Int64 UserId { get; set; }
        public new Int64 AddressId { get; set; }
        #endregion

        #region Constructor
        public UserAddressModel()
        {

        }
        #endregion
    }
}
