using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NIMBOLE.Models.Models
{
    public class EmployeeModel
    {

        #region Properties
        public Int64 Id { get; set; }
    
        public Guid TenantId { get; set; }
        public string EmpCode { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        [Required(ErrorMessageResourceType = typeof(NIMBOLE.GlobalResources.Resources), ErrorMessageResourceName = "Required")]
        public string FirstName { get; set; }
		
        public string LastName { get; set; }

        public string FullName { get; set; }
		
        [Display(Name = "Email", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        [Required(ErrorMessageResourceType = typeof(NIMBOLE.GlobalResources.Resources), ErrorMessageResourceName = "Required")]
        public string EmployeeEmail { get; set; }

        public string Password { get; set; }
		
        public long ReportingTo { get; set; }
        public Int64 LoginId { get; set; }
		
        [Display(Name = "Role", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        [Required(ErrorMessageResourceType = typeof(NIMBOLE.GlobalResources.Resources), ErrorMessageResourceName = "Required")]
        public Int64 EmpRoleId { get; set; }

        public Int64 EmpDesiganationRoleId { get; set; }
		
        [Display(Name = "Location", ResourceType = typeof(NIMBOLE.GlobalResources.Resources))]
        [Required(ErrorMessageResourceType = typeof(NIMBOLE.GlobalResources.Resources), ErrorMessageResourceName = "Required")]
        public string Location { get; set; }
		
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public string RoleCode { get; set; }
        public bool IsSelected { get; set; }
        public bool Status { get; set; }
        public string Active { get; set; }
        public string Change_status { get; set; }
        public string EmployeeImageURL { get; set; }

        //private DateTime _CreatedDate;
        //public DateTime CreatedDate
        //{
        //    get
        //    {
        //        return CreatedDate.Year == 1 ? DateTime.Now : CreatedDate;
        //    }
        //    set
        //    {
        //        _CreatedDate = value;
        //    }
        //}
        public DateTime ModifiedDate { get; set; }

        public string BornPlace { get; set; }

        //public DateTime BornDate { get; set; }
        public Nullable<System.DateTime> BornDate { get; set; }

        public DateTime ? JoinDate { get; set; }
        public DateTime ? ResignDate { get; set; }
        public string BankId { get; set; }
        public string BankNumber { get; set; }
        public string BankDetails { get; set; }
        public string GcmId { get; set; }

        #region Navigation Properties
        //[JsonIgnore]
        [IgnoreDataMember]
        private EmployeeRoleModel _EmployeeRoleModel;
        //[JsonIgnore]
        public EmployeeRoleModel objEmployeeRoleModel
        {
            get
            {
                return _EmployeeRoleModel ?? (_EmployeeRoleModel = new EmployeeRoleModel());
            }
            set
            {
                _EmployeeRoleModel = value;
            }
        }
        
        //[JsonIgnore]
         [IgnoreDataMember]
        private LoginModel _LoginModel;
        //[JsonIgnore]
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

        //[JsonIgnore]
        [IgnoreDataMember]
        private AddressModel _AddressModel;
        //[JsonIgnore]
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
        #endregion
        
        [IgnoreDataMember]
        private SettingModel _SettingModel;
        //[JsonIgnore]
        public SettingModel objSettingModel
        {
            get
            {
                return _SettingModel ?? (_SettingModel = new SettingModel());
            }
            set
            {
                _SettingModel = value;
            }
        }

        #endregion

        #region Constructor
        public EmployeeModel()
        {
            
        }
        #endregion 
    }
        public class EmployeeDisplayModel
        {

            #region Properties
            public Int64 Id { get; set; }
            public Int64 ParentId { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public Int64 EmpRoleId { get; set; }

            public string Title { get; set; }
            public Int64 RoleOrder { get; set; }
            public string Mail { get; set; }

            public string Image { get; set; }

            #endregion

            #region Constructor
            public EmployeeDisplayModel()
            {

            }
            #endregion
       
    }
}
