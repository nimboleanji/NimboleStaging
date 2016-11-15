using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
   public class UserModel//:NIMBOLE.Entities.TblUser
	{
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public string UserCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }
        public Int64 LoginId { get; set; }
        public string MobileNo { get; set; }
        public DateTime DOB { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        #endregion

        #region Navigation Properties
        [JsonIgnore]
        [IgnoreDataMember]
        private AddressModel _AddressModel;
        [JsonIgnore]
        public AddressModel objAddressModel
        {
            get
            {
                return _AddressModel ?? (_AddressModel = new AddressModel());
            }
            set
            {
                _AddressModel = value;
            }
        }
        [JsonIgnore]
        [IgnoreDataMember]
        private LoginModel _LoginModel;
        [JsonIgnore]
        public LoginModel objLoginModel
        {
            get
            {
                return _LoginModel ?? (_LoginModel = new LoginModel());
            }
            set
            {
                _LoginModel = value;
            }
        }

        #endregion


        #region Constructor
        public UserModel()
        {

        }
        #endregion 
	}

     

}
