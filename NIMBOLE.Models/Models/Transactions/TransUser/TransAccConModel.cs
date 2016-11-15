using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class TransAccConModel
    {
        public AccountModel objAccountModel { get; set; }

        public ContactModel objContactModel { get; set; }

        public ContactRoleModel objContactRoleModel { get; set; }

        public TransContactModel objTransContactModel { get; set; }

        public AddressModel objAddressModel { get; set; }
        public TransAccConModel()
        {
            this.objAddressModel=new AddressModel();
            this.objAccountModel = new AccountModel();
            this.objContactModel = new ContactModel();
            this.objContactRoleModel = new ContactRoleModel();
            this.objTransContactModel = new TransContactModel();
        }
    }
    public class TranAddressContactModel
    {
        public ContactModel objContactModel { get; set; }
        public AddressModel objAddressModel { get; set; }
    }
}
