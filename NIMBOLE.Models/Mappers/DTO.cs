using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIMBOLE.Common;
using NIMBOLE.Entities;
using NIMBOLE.Models.Models.Transactions;

namespace NIMBOLE.Models.Mappers
{
    public class DTO
    {
        #region LeadSource
        public LeadSourceModel MapTable2Model(TblLeadSource objTblLeadSource)
        {
            LeadSourceModel objLeadSourceModel = new LeadSourceModel();
            objLeadSourceModel.Id = objTblLeadSource.Id;
            if (objTblLeadSource.Id == 0)
                objLeadSourceModel.CreatedDate = DateTime.Now;
            else
                objLeadSourceModel.CreatedDate = objTblLeadSource.CreatedDate.Value.ToDefaultDate();
            objLeadSourceModel.ModifiedDate = objTblLeadSource.ModifiedDate.Value.ToDefaultDate();

            objLeadSourceModel.TenantId = objTblLeadSource.TenantId;
            objLeadSourceModel.Description = objTblLeadSource.Description;
            objLeadSourceModel.Code = objTblLeadSource.Code;
            objLeadSourceModel.Status = objTblLeadSource.Status ?? true;
            return (objLeadSourceModel);
        }
        public TblLeadSource MapModel2Table(LeadSourceModel objLeadSourceModel)
        {
            TblLeadSource objTblLeadSource = new TblLeadSource();
            objTblLeadSource.Id = objLeadSourceModel.Id;
            if (objLeadSourceModel.Id == 0)
                objTblLeadSource.CreatedDate = DateTime.Now;
            else
                objTblLeadSource.CreatedDate = objLeadSourceModel.CreatedDate.ToDefaultDateIfTooEarly();
            objTblLeadSource.ModifiedDate = objLeadSourceModel.ModifiedDate.ToDefaultDateIfTooEarly();
            objTblLeadSource.TenantId = objLeadSourceModel.TenantId;
            objTblLeadSource.Description = objLeadSourceModel.Description;
            objTblLeadSource.Code = objLeadSourceModel.Code;
            objTblLeadSource.CreatedDate = objLeadSourceModel.CreatedDate.ToDefaultDateIfTooEarly();
            objTblLeadSource.ModifiedDate = objLeadSourceModel.ModifiedDate.ToDefaultDateIfTooEarly();
            objTblLeadSource.Status = true;
            return (objTblLeadSource);
        }
        #endregion

        #region ContactRole
        public ContactRoleModel MapTable2Model(TblContactRole objTblContactRole)
        {
            ContactRoleModel objContactRoleModel = new ContactRoleModel();
            objContactRoleModel.Id = objTblContactRole.Id;
            if (objTblContactRole.Id == 0)
                objContactRoleModel.CreateDate = DateTime.Now;
            else
                objContactRoleModel.CreateDate = objTblContactRole.CreatedDate.Value.ToDefaultDate();
            objContactRoleModel.ModifiedDate = objTblContactRole.ModifiedDate.Value.ToDefaultDate();
            objContactRoleModel.TenantId = objTblContactRole.TenantId;
            objContactRoleModel.RoleCode = objTblContactRole.Code;
            objContactRoleModel.Description = objTblContactRole.Description;
            objContactRoleModel.Status = Convert.ToBoolean(objTblContactRole.Status);
            return (objContactRoleModel);
        }

        public TblContactRole MapModel2Table(ContactRoleModel objContactRoleModel)
        {
            TblContactRole objTblContactRole = new TblContactRole();
            objTblContactRole.Id = objContactRoleModel.Id;
            if (objContactRoleModel.Id == 0)
                objTblContactRole.CreatedDate = DateTime.Now;
            else
                objTblContactRole.CreatedDate = objContactRoleModel.CreateDate.ToDefaultDate();
            objTblContactRole.ModifiedDate = objContactRoleModel.ModifiedDate.ToDefaultDate();
            objTblContactRole.TenantId = objContactRoleModel.TenantId;
            objTblContactRole.Code = objContactRoleModel.RoleCode;
            objTblContactRole.Description = objContactRoleModel.Description;
            objTblContactRole.Status = objContactRoleModel.Status;
            return (objTblContactRole);
        }
        #endregion

        #region Nimbole2Account
        public AccountModel MapNimbole2Account(NimboleAccountModel objNimboleAccountModel)
        {
            AccountModel objAccountModel = new AccountModel();
            objAccountModel.Id = objNimboleAccountModel.Id;
            if (objNimboleAccountModel.Id == 0)
                objAccountModel.CreatedDate = DateTime.Now;
            else
                objAccountModel.CreatedDate = objNimboleAccountModel.CreatedDate.ToDefaultDate();
            objAccountModel.ModifiedDate = objNimboleAccountModel.ModifiedDate.ToDefaultDate();

            objAccountModel.TenantId = objNimboleAccountModel.TenantId;
            objAccountModel.AccountCode = objNimboleAccountModel.AccountCode;
            objAccountModel.AccountName = objNimboleAccountModel.AccountName;
            objAccountModel.AccountOwner = objNimboleAccountModel.AccountOwner;
            objAccountModel.AccountClassification = objNimboleAccountModel.AccountClassification;
            objAccountModel.ParentAccount = objNimboleAccountModel.ParentAccount;
            objAccountModel.Employees = objNimboleAccountModel.Employees;
            objAccountModel.OwnerShip = objNimboleAccountModel.OwnerShipName;
            objAccountModel.Industry = objNimboleAccountModel.Industry;
            objAccountModel.AccountType = objNimboleAccountModel.AccountType;
            objAccountModel.IsParentAccount = objNimboleAccountModel.IsParentAccount;
            objAccountModel.Distributor = objNimboleAccountModel.DistributerName;
            objAccountModel.Subsidiary = objNimboleAccountModel.Subsidiary;
            objAccountModel.Region = objNimboleAccountModel.Region;
            objAccountModel.Phone = objNimboleAccountModel.Phone;
            objAccountModel.Fax = objNimboleAccountModel.Fax;
            objAccountModel.Email = objNimboleAccountModel.Email;
            objAccountModel.Rating = objNimboleAccountModel.Rating;
            objAccountModel.SICCode = objNimboleAccountModel.SICCode;
            objAccountModel.AnnualRevenue = objNimboleAccountModel.AnnualRevenue;
            objAccountModel.Website = objNimboleAccountModel.Website;
            objAccountModel.Status = objNimboleAccountModel.Status;
            return (objAccountModel);
        }
        public NimboleAccountModel MapTable2Model(TblAccount objTblAccount)
        {
            NimboleAccountModel objNimboleAccountModel = new NimboleAccountModel();
            objNimboleAccountModel.Id = objTblAccount.Id;
            if (objTblAccount.Id == 0)
                objNimboleAccountModel.CreatedDate = DateTime.Now;
            else
                objNimboleAccountModel.CreatedDate = objTblAccount.CreatedDate.Value.ToDefaultDate();
            objNimboleAccountModel.ModifiedDate = objTblAccount.ModifiedDate.Value.ToDefaultDate();

            objNimboleAccountModel.TenantId = objTblAccount.TenantId;
            objNimboleAccountModel.AccountCode = objTblAccount.Code;
            objNimboleAccountModel.AccountId = objTblAccount.Id;
            objNimboleAccountModel.AccountName = objTblAccount.AccountName;
            objNimboleAccountModel.AccountOwner = objTblAccount.AccountOwner;
            objNimboleAccountModel.AccountClassification = objTblAccount.AccountClassification;
            objNimboleAccountModel.ParentAccount = objTblAccount.ParentAccount;
            objNimboleAccountModel.Employees = objTblAccount.Employees ?? 0;
            objNimboleAccountModel.OwnerShip = objTblAccount.OwnerShip;
            objNimboleAccountModel.OwnerShipName = objTblAccount.OwnerShip;
            objNimboleAccountModel.Industry = objTblAccount.Industry;
            objNimboleAccountModel.AccountType = objTblAccount.AccountType;
            objNimboleAccountModel.IsParentAccount = Convert.ToBoolean(objTblAccount.IsParentAccount);
            objNimboleAccountModel.DistributerName = objTblAccount.DistributerName;
            objNimboleAccountModel.Subsidiary = objTblAccount.Subsidiary;
            objNimboleAccountModel.Region = objTblAccount.Region;
            objNimboleAccountModel.Phone = objTblAccount.Phone;
            objNimboleAccountModel.Fax = objTblAccount.Fax;
            objNimboleAccountModel.Email = objTblAccount.Email;
            objNimboleAccountModel.Rating = objTblAccount.Rating;
            objNimboleAccountModel.SICCode = objTblAccount.SICCode;
            objNimboleAccountModel.AnnualRevenue = objTblAccount.AnnualRevenue ?? 0;
            objNimboleAccountModel.Website = objTblAccount.Website;
            objNimboleAccountModel.Status = objTblAccount.Status ?? true;
            //objNimboleAccountModel.HouseNo = objTblAccount.HouseNo;
            //objNimboleAccountModel.StreetName = objTblAccount.StreetName;
            //objNimboleAccountModel.CityId = objTblAccount.CityId;
            //objNimboleAccountModel.ZipCode = objTblAccount.ZipCode;
            return (objNimboleAccountModel);
        }
        public TblAccount MapModel2Table(NimboleAccountModel objNimboleAccountModel)
        {
            TblAccount objTblAccount = new TblAccount();
            objTblAccount.Id = objNimboleAccountModel.Id;
            if (objNimboleAccountModel.Id == 0)
                objTblAccount.CreatedDate = DateTime.Now;
            else
                objTblAccount.CreatedDate = objNimboleAccountModel.CreatedDate.ToDefaultDate();
            objTblAccount.ModifiedDate = objNimboleAccountModel.ModifiedDate.ToDefaultDate();
            objTblAccount.TenantId = objNimboleAccountModel.TenantId;
            objTblAccount.Code = objNimboleAccountModel.AccountCode;
            objTblAccount.AccountName = objNimboleAccountModel.AccountName;
            objTblAccount.AccountOwner = objNimboleAccountModel.AccountOwner;
            objTblAccount.AccountClassification = objNimboleAccountModel.AccountClassification;
            objTblAccount.ParentAccount = objNimboleAccountModel.ParentAccount;
            objTblAccount.Employees = objNimboleAccountModel.Employees;
            objTblAccount.OwnerShip = objNimboleAccountModel.OwnerShip;
            objTblAccount.Industry = objNimboleAccountModel.Industry;
            objTblAccount.AccountType = objNimboleAccountModel.AccountType;
            objTblAccount.IsParentAccount = Convert.ToBoolean(objNimboleAccountModel.IsParentAccount);
            objTblAccount.DistributerName = objNimboleAccountModel.DistributerName;
            objTblAccount.Subsidiary = objNimboleAccountModel.Subsidiary;
            objTblAccount.Region = objNimboleAccountModel.Region;
            objTblAccount.Phone = objNimboleAccountModel.Phone;
            objTblAccount.Fax = objNimboleAccountModel.Fax;
            objTblAccount.Email = objNimboleAccountModel.Email;
            objTblAccount.Rating = objNimboleAccountModel.Rating;
            objTblAccount.SICCode = objNimboleAccountModel.SICCode;
            objTblAccount.AnnualRevenue = objNimboleAccountModel.AnnualRevenue;
            //objTblAccount.ZipCode = objNimboleAccountModel.ZipCode;
            //objTblAccount.StreetName = objNimboleAccountModel.StreetName;
            //objTblAccount.HouseNo = objNimboleAccountModel.HouseNo;
            //objTblAccount.CityId = objNimboleAccountModel.CityId;
            objTblAccount.Website = objNimboleAccountModel.Website;
            objTblAccount.Status = true;
            return (objTblAccount);
        }
        #endregion

        #region Contact
        public ContactModel MapTable2Model(TblContact objTblContact)
        {
            ContactModel objContactModel = new ContactModel();
            objContactModel.Id = objTblContact.Id;
            if (objTblContact.Id == 0)
                objContactModel.CreatedDate = DateTime.Now;
            else
                objContactModel.CreatedDate = objTblContact.CreatedDate.Value.ToDefaultDate();
            objContactModel.ModifiedDate = objTblContact.ModifiedDate.Value.ToDefaultDate();
            objContactModel.TenantId = objTblContact.TenantId;
            objContactModel.LeadSourceId = objTblContact.LeadSourceId ?? 0;
            objContactModel.FirstName = objTblContact.FirstName;
            objContactModel.LastName = objTblContact.LastName;
            objContactModel.ContactImageURL = objTblContact.ContactImageURL;
            objContactModel.ContactEmail = objTblContact.ContactEmail;
            objContactModel.WorkEmail = objTblContact.WorkEmail;
            objContactModel.DepartmentId = objTblContact.DepartmentId ?? 0;
            objContactModel.Comments = objTblContact.Comments;
            objContactModel.Status = objTblContact.Status ?? true;
            return (objContactModel);
        }
        public TblContact MapModel2Table(ContactModel objContactModel)
        {
            TblContact objTblContact = new TblContact();
            objTblContact.Id = objContactModel.Id;
            if (objContactModel.Id == 0)
                objTblContact.CreatedDate = DateTime.Now;
            else
                objTblContact.CreatedDate = objContactModel.CreatedDate.ToDefaultDate();
            objTblContact.ModifiedDate = objContactModel.ModifiedDate.ToDefaultDate();
            objTblContact.TenantId = objContactModel.TenantId;
            objTblContact.LeadSourceId = objContactModel.LeadSourceId;
            objTblContact.FirstName = objContactModel.FirstName;
            objTblContact.LastName = objContactModel.LastName;
            objTblContact.ContactEmail = objContactModel.ContactEmail;
            objTblContact.WorkEmail = objContactModel.WorkEmail;
            objTblContact.DepartmentId = objContactModel.DepartmentId;
            objTblContact.Comments = objContactModel.Comments;
            objTblContact.Status = true;
            return (objTblContact);
        }
        #endregion

        #region Address
        public TblAddress MapModel2Table(AddressModel objAddressModel)
        {
            TblAddress objTblAddress = new TblAddress();
            objTblAddress.Id = objAddressModel.Id;
            if (objAddressModel.Id == 0)
                objTblAddress.CreatedDate = DateTime.Now;
            else
                objTblAddress.CreatedDate = objAddressModel.CreatedDate.ToDefaultDate();
            objTblAddress.ModifiedDate = objAddressModel.ModifiedDate.ToDefaultDate();
            objTblAddress.TenantId = objAddressModel.TenantId;
            objTblAddress.HouseNo = objAddressModel.HouseNo;
            objTblAddress.StreetName = objAddressModel.StreetName;
            objTblAddress.CountryId = objAddressModel.CountryId;
            objTblAddress.StateId = objAddressModel.StateId;
            objTblAddress.CityId = objAddressModel.CityId;
            objTblAddress.ZipCode = objAddressModel.ZipCode;
            objTblAddress.Phone = objAddressModel.Phone;
            objTblAddress.Mobile = objAddressModel.Mobile;
            objTblAddress.HomePhone = objAddressModel.HomePhone;
            objTblAddress.Fax = objAddressModel.Fax;
            objTblAddress.SkypeName = objAddressModel.SkypeName;
            objTblAddress.Address_Type = objAddressModel.Address_Type ?? "C";
            objTblAddress.Status = true;
            return (objTblAddress);
        }
        public AddressModel MapTable2Model(TblAddress objTblAddress)
        {
            AddressModel objAddressModel = new AddressModel();
            objAddressModel.Id = objTblAddress.Id;
            if (objTblAddress.Id == 0)
                objAddressModel.CreatedDate = DateTime.Now;
            else
                objAddressModel.CreatedDate = objTblAddress.CreatedDate.Value.ToDefaultDate();
            objAddressModel.ModifiedDate = objTblAddress.ModifiedDate.Value.ToDefaultDate();
            objAddressModel.TenantId = objTblAddress.TenantId;
            objAddressModel.HouseNo = objTblAddress.HouseNo;
            objAddressModel.StreetName = objTblAddress.StreetName;
            objAddressModel.CountryId = objTblAddress.CountryId ?? 0;
            objAddressModel.StateId = objTblAddress.StateId ?? 0;
            objAddressModel.CityId = objTblAddress.CityId ?? 0;
            objAddressModel.ZipCode = objTblAddress.ZipCode;
            objAddressModel.Phone = objTblAddress.Phone;
            objAddressModel.Mobile = objTblAddress.Mobile;
            objAddressModel.HomePhone = objTblAddress.HomePhone;
            objAddressModel.Fax = objTblAddress.Fax;
            objAddressModel.SkypeName = objTblAddress.SkypeName;
            objAddressModel.Address_Type = objTblAddress.Address_Type;
            objAddressModel.Status = objTblAddress.Status ?? true;
            return (objAddressModel);
        }
        #endregion

        #region AddressEmployee
        public TblAddressEmployee MapModel2Table(EmployeeAddressModel objEmployeeAddressModel)
        {
            TblAddressEmployee objTblAddressEmployee = new TblAddressEmployee();
            objTblAddressEmployee.Id = objEmployeeAddressModel.Id;
            objTblAddressEmployee.TenantId = objEmployeeAddressModel.TenantId;
            objTblAddressEmployee.AddressId = objEmployeeAddressModel.AddressId;
            objTblAddressEmployee.EmployeeId = objEmployeeAddressModel.EmpId;

            return (objTblAddressEmployee);
        }
        public EmployeeAddressModel MapTable2Model(TblAddressEmployee objTblAddressEmployee)
        {
            EmployeeAddressModel objEmployeeAddressModel = new EmployeeAddressModel();
            objEmployeeAddressModel.Id = objTblAddressEmployee.Id;
            objEmployeeAddressModel.AddressId = objTblAddressEmployee.AddressId ?? 0;
            objEmployeeAddressModel.EmpId = objTblAddressEmployee.EmployeeId ?? 0;

            return (objEmployeeAddressModel);
        }
        #endregion

        #region TransContact
        public TblTransContact MapModel2Table(TransContactModel objTransContactModel)
        {
            TblTransContact objTblTransContact = new TblTransContact();
            objTblTransContact.Id = objTransContactModel.Id;
            if (objTransContactModel.Id == 0)
                objTblTransContact.CreatedDate = DateTime.Now;
            else
                objTblTransContact.CreatedDate = objTransContactModel.CreatedDate.ToDefaultDate();
            objTblTransContact.ModifiedDate = objTransContactModel.ModifiedDate.ToDefaultDate();
            objTblTransContact.TenantId = objTransContactModel.TenantId;
            objTblTransContact.AccountId = objTransContactModel.AccountId;
            objTblTransContact.ContactId = objTransContactModel.ContactId;
            objTblTransContact.ContactRoleId = objTransContactModel.ContactRoleId;
            objTblTransContact.Status = true;
            return (objTblTransContact);
        }
        public TransContactModel MapTable2Model(TblTransContact objTblTransContact)
        {
            TransContactModel objTransContactModel = new TransContactModel();
            objTransContactModel.Id = objTblTransContact.Id;
            if (objTblTransContact.Id == 0)
                objTransContactModel.CreatedDate = DateTime.Now;
            else
                objTransContactModel.CreatedDate = objTblTransContact.CreatedDate.Value.ToDefaultDate();
            objTransContactModel.ModifiedDate = objTblTransContact.ModifiedDate.Value.ToDefaultDate();
            objTransContactModel.TenantId = objTransContactModel.TenantId;
            objTransContactModel.AccountId = objTblTransContact.AccountId ?? 0;
            objTransContactModel.ContactId = objTblTransContact.ContactId ?? 0;
            objTransContactModel.ContactRoleId = objTblTransContact.ContactRoleId ?? 0;
            objTransContactModel.Status = objTblTransContact.Status ?? true;
            return (objTransContactModel);
        }
        #endregion

        #region AddressContact
        public TblAddressContact MapModel2Table(ContactAddressModel objAddressContactModel)
        {
            TblAddressContact objTblAddressContact = new TblAddressContact();
            objTblAddressContact.Id = objAddressContactModel.Id;
            objTblAddressContact.TenantId = objAddressContactModel.TenantId;
            objTblAddressContact.AddressId = objAddressContactModel.AddressId;
            objTblAddressContact.ContactId = objAddressContactModel.ContactId;
            return (objTblAddressContact);
        }
        public AddressContactModel MapTable2Model(TblAddressContact objTblAddressContact)
        {
            AddressContactModel objAddressContactModel = new AddressContactModel();
            objAddressContactModel.Id = objTblAddressContact.Id;
            objAddressContactModel.TenantId = objTblAddressContact.TenantId;
            objAddressContactModel.AddressId = objTblAddressContact.AddressId ?? 0;
            objAddressContactModel.ContactId = objTblAddressContact.ContactId ?? 0;
            return (objAddressContactModel);
        }
        public TblUserAddress MapModel2Table(UserAddressModel objUserAddressModel)
        {
            TblUserAddress objTblUserAddress = new TblUserAddress();
            objTblUserAddress.Id = objUserAddressModel.Id;
            objTblUserAddress.TenantId = objUserAddressModel.TenantId;
            objTblUserAddress.AddressId = objUserAddressModel.AddressId;
            objTblUserAddress.UserId = objUserAddressModel.UserId;
            return (objTblUserAddress);
        }
        #endregion

        #region UserAddress
        public UserAddressModel MapTable2Model(TblUserAddress objTblUserAddress)
        {
            UserAddressModel objUserAddressModel = new UserAddressModel();
            objUserAddressModel.Id = objTblUserAddress.Id;
            objUserAddressModel.TenantId = objTblUserAddress.TenantId;
            objUserAddressModel.AddressId = objTblUserAddress.AddressId ?? 0;
            objUserAddressModel.UserId = objTblUserAddress.UserId ?? 0;
            return (objUserAddressModel);
        }
        #endregion

        #region User
        public UserModel MapTable2Model(TblUser objTblUser)
        {
            UserModel objUserModel = new UserModel();
            objUserModel.Id = objTblUser.Id;
            if (objTblUser.Id == 0)
                objUserModel.CreatedDate = DateTime.Now;
            else
                objUserModel.CreatedDate = objTblUser.CreatedDate.Value.ToDefaultDate();
            objUserModel.ModifiedDate = objTblUser.ModifiedDate.Value.ToDefaultDate();
            objUserModel.TenantId = objTblUser.TenantId;
            objUserModel.UserCode = objTblUser.Code;
            objUserModel.FirstName = objTblUser.FirstName;
            objUserModel.LastName = objTblUser.LastName;
            objUserModel.LoginId = Convert.ToInt32(objTblUser.LoginId);
            objUserModel.MobileNo = objTblUser.MobileNo;
            objUserModel.DOB = Convert.ToDateTime(objTblUser.DOB);
            objUserModel.Status = Convert.ToBoolean(objTblUser.Status);
            return (objUserModel);
        }
        public TblUser MapModel2Table(UserModel objUserModel)
        {
            TblUser objTblUser = new TblUser();
            objTblUser.Id = objUserModel.Id;
            if (objUserModel.Id == 0)
                objTblUser.CreatedDate = DateTime.Now;
            else
                objTblUser.CreatedDate = objUserModel.CreatedDate.ToDefaultDate();
            objTblUser.ModifiedDate = objUserModel.ModifiedDate.ToDefaultDate();
            objTblUser.TenantId = objUserModel.TenantId;
            objTblUser.Code = objUserModel.UserCode;
            objTblUser.FirstName = objUserModel.FirstName;
            objTblUser.LastName = objUserModel.LastName;
            objTblUser.LoginId = Convert.ToInt32(objUserModel.LoginId);
            objTblUser.MobileNo = objUserModel.MobileNo;
            objTblUser.DOB = Convert.ToDateTime(objUserModel.DOB);
            objTblUser.Status = true;
            return (objTblUser);
        }
        #endregion

        //#region Lead
        //public LeadModel MapTable2Model(TblLead objTblLead)
        //{
        //    LeadModel objLeadModel = new LeadModel();
        //    objLeadModel.Id = objTblLead.Id;
        //    if (objTblLead.Id == 0)
        //        objLeadModel.CreatedDate = DateTime.Now;
        //    else
        //        objLeadModel.CreatedDate = objTblLead.CreatedDate.Value.ToDefaultDate();
        //    objLeadModel.ModifiedDate = objTblLead.ModifiedDate.Value.ToDefaultDate();
        //    objLeadModel.TenantId = objTblLead.TenantId;
        //    objLeadModel.LeadTitle = objTblLead.LeadTitle;
        //    objLeadModel.LeadDescription = objTblLead.LeadDescription;
        //    objTblLead.LeadOwnerId = objLeadModel.LeadOwnerId;
        //    objLeadModel.LeadSourceId = objTblLead.LeadSourceId;
        //    objLeadModel.LeadType = objTblLead.LeadType;
        //    objLeadModel.Budget = objTblLead.Budget ?? 0;
        //    objLeadModel.LeadStatus = objTblLead.LeadStatus;
        //    objLeadModel.Location = objTblLead.Location;
        //    objLeadModel.TimeFrame = objTblLead.TimeFrame;
        //    objLeadModel.AccountId = objTblLead.AccountId;
        //    objLeadModel.Size = objTblLead.Size ?? 0;
        //    objLeadModel.Status = objTblLead.Status ?? true;
        //    objLeadModel.LeadStage = objTblLead.LeadStage ?? 0;
        //    objLeadModel.LeadEmployees = objTblLead.LeadEmployees != null ? objTblLead.LeadEmployees.Split(',').ToList() : null;
        //  //  objLeadModel.LeadEmployees = objTblLead.LeadEmployees;
        //    return (objLeadModel);
        //}
        //public TblLead MapModel2Table(LeadModel objLeadModel)
        //{
        //    TblLead objTblLead = new TblLead();
        //    objTblLead.Id = objLeadModel.Id;
        //    if (objLeadModel.Id == 0)
        //    {
        //        objTblLead.CreatedDate = DateTime.Now;
        //        objTblLead.Status = true;
        //    }
        //    else
        //    {
        //        objTblLead.Status = objLeadModel.Status;
        //    }
        //    objTblLead.ModifiedDate = objLeadModel.ModifiedDate.ToDefaultDate();
        //    objTblLead.LeadTitle = objLeadModel.LeadTitle;
        //    objTblLead.LeadDescription = objLeadModel.LeadDescription;
        //    objTblLead.LeadOwnerId = objLeadModel.LeadOwnerId;
        //    objTblLead.LeadSourceId = objLeadModel.LeadSourceId;
        //    objTblLead.LeadType = objLeadModel.LeadType;
        //    objTblLead.Budget = objLeadModel.Budget;
        //    objTblLead.LeadStatus = objLeadModel.LeadStatus;
        //    objTblLead.Location = objLeadModel.Location;
        //    objTblLead.TimeFrame = objLeadModel.TimeFrame;
        //    objTblLead.AccountId = objLeadModel.AccountId;
        //    objTblLead.Size = objLeadModel.Size;
        //    objTblLead.TenantId = objLeadModel.TenantId;            
        //    objTblLead.LeadStage = objLeadModel.LeadStage;
        //   objTblLead.LeadEmployees = objLeadModel.LeadEmployees != null ? objLeadModel.LeadEmployees[objLeadModel.LeadEmployees.Count - 1].ToString() : string.Empty;
        // //   objTblLead.LeadEmployees = objLeadModel.LeadEmployees.ToString();
        //    return (objTblLead);
        //}
        //#endregion

        #region LeadNew
        public LeadNewModel MapTable2Model(TblLead objTblLead)
        {
            LeadNewModel objLeadModel = new LeadNewModel();
            objLeadModel.Id = objTblLead.Id;
            if (objTblLead.Id == 0)
                objLeadModel.CreatedDate = DateTime.Now;
            else
                objLeadModel.CreatedDate = objTblLead.CreatedDate.Value.ToDefaultDate();
            objLeadModel.ModifiedDate = objTblLead.ModifiedDate.Value.ToDefaultDate();
            objLeadModel.TenantId = objTblLead.TenantId;
            objLeadModel.LeadTitle = objTblLead.LeadTitle;
            objLeadModel.LeadDescription = objTblLead.LeadDescription;
            objLeadModel.LeadOwnerId = objTblLead.LeadOwnerId??0;
            objLeadModel.LeadSourceId = objTblLead.LeadSourceId;
            objLeadModel.LeadType = objTblLead.LeadType;
            objLeadModel.Budget = objTblLead.Budget ?? 0;
            objLeadModel.LeadStatus = objTblLead.LeadStatus;
            objLeadModel.Location = objTblLead.Location;
            objLeadModel.TimeFrame = objTblLead.TimeFrame;
            objLeadModel.AccountId = objTblLead.AccountId;
            objLeadModel.Size = objTblLead.Size ?? 0;
            objLeadModel.Status = objTblLead.Status ?? true;
            objLeadModel.LeadStage = objTblLead.LeadStage ?? 0;
            objLeadModel.LeadEmployees = objTblLead.LeadEmployees != null ? objTblLead.LeadEmployees.Split(',').ToList() : null;

            //  objLeadModel.LeadEmployees = objTblLead.LeadEmployees;
            return (objLeadModel);
        }
        public TblLead MapModel2Table(LeadNewModel objLeadModel)
        {
            TblLead objTblLead = new TblLead();
            objTblLead.Id = objLeadModel.Id;
            if (objLeadModel.Id == 0)
            {
                objTblLead.CreatedDate = DateTime.Now;
                objTblLead.Status = true;
            }
            else
            {
                objTblLead.Status = objLeadModel.Status;
            }
            objTblLead.ModifiedDate = objLeadModel.ModifiedDate.ToDefaultDate();
            objTblLead.LeadTitle = objLeadModel.LeadTitle;
            objTblLead.LeadDescription = objLeadModel.LeadDescription;
            objTblLead.LeadOwnerId = objLeadModel.LeadOwnerId;
            objTblLead.LeadSourceId = objLeadModel.LeadSourceId;
            objTblLead.LeadType = objLeadModel.LeadType;
            objTblLead.Budget = objLeadModel.Budget;
            objTblLead.LeadStatus = objLeadModel.LeadStatus;
            objTblLead.Location = objLeadModel.Location;
            objTblLead.TimeFrame = objLeadModel.TimeFrame;
            objTblLead.AccountId = objLeadModel.AccountId;
            objTblLead.Size = objLeadModel.Size;
            objTblLead.TenantId = objLeadModel.TenantId;
            objTblLead.LeadStage = objLeadModel.LeadStage;
            objTblLead.LeadEmployees = objLeadModel.LeadEmployees != null ? objLeadModel.LeadEmployees[objLeadModel.LeadEmployees.Count - 1].ToString() : string.Empty;

            //   objTblLead.LeadEmployees = objLeadModel.LeadEmployees.ToString();
            return (objTblLead);
        }
        #endregion

        #region Activity
        public ActivityModel MapTable2Model(TblActivity objTblActivity)
        {
            ActivityModel objActivityModel = new ActivityModel();
            objActivityModel.Id = objTblActivity.Id;
            if (objTblActivity.Id == 0)
                objActivityModel.CreatedDate = DateTime.Now;
            else
                objActivityModel.CreatedDate = objTblActivity.CreatedDate.Value.ToDefaultDate();
            objActivityModel.ModifiedDate = objTblActivity.ModifiedDate.Value.ToDefaultDate();
            objActivityModel.TenantId = objTblActivity.TenantId;
            objActivityModel.MileStoneId = objTblActivity.MileStoneId ?? 1;
            objActivityModel.Title = objTblActivity.Title;
            objActivityModel.Type_A_E_M = objTblActivity.Type_A_E_M == "" ? "Activity" : objTblActivity.Type_A_E_M;
            objActivityModel.ActivityDate = objTblActivity.ActivityDate ?? DateTime.Now;
            objActivityModel.ActivityComments = objTblActivity.Comments;
            objActivityModel.RequireNotify = objTblActivity.RequireNotify ?? false;
            objActivityModel.ReferenceId = objTblActivity.ReferenceId;
            objActivityModel.Status = objTblActivity.Status ?? true;
            return (objActivityModel);
        }
        public TblActivity MapModel2Table(ActivityModel objActivityModel)
        {
            TblActivity objTblActivity = new TblActivity();
            objTblActivity.Id = objActivityModel.Id;
            if (objActivityModel.Id == 0)
                objTblActivity.CreatedDate = DateTime.Now;
            else
                objTblActivity.CreatedDate = objActivityModel.CreatedDate.ToDefaultDate();
            objTblActivity.ModifiedDate = objActivityModel.ModifiedDate.ToDefaultDate();
            objTblActivity.MileStoneId = objActivityModel.MileStoneId;
            objTblActivity.Title = objActivityModel.Title;
            objTblActivity.Type_A_E_M = "Activity";
            objTblActivity.ActivityDate = objActivityModel.ActivityDate;
            objTblActivity.Comments = objActivityModel.ActivityComments;
            objTblActivity.RequireNotify = objActivityModel.RequireNotify;
            objTblActivity.ReferenceId = objActivityModel.ReferenceId;
            objTblActivity.TenantId = objActivityModel.TenantId;
            objTblActivity.Status = true;
            return objTblActivity;
        }
        #endregion

        #region Document
        public DocumentModel MapTable2Model(TblDocument objTblDocument)
        {
            DocumentModel objDocumentModel = new DocumentModel();
            objDocumentModel.Id = objTblDocument.Id;
            if (objTblDocument.Id == 0)
                objDocumentModel.CreatedDate = DateTime.Now;
            else
                objDocumentModel.CreatedDate = objTblDocument.CreatedDate.Value.ToDefaultDate();
            objDocumentModel.ModifiedDate = objTblDocument.ModifiedDate.Value.ToDefaultDate();
            objDocumentModel.TenantId = objTblDocument.TenantId;
            objDocumentModel.URL = objTblDocument.URL;
            objDocumentModel.DocumentName = objTblDocument.DocumentName;
            objDocumentModel.UploadedById = objTblDocument.UploadedById ?? 1;
            objDocumentModel.UploadDateTime = objTblDocument.UploadDateTime ?? DateTime.Now;
            objDocumentModel.DocumentType = objTblDocument.DocumentType;
            objDocumentModel.SizeOfDocument = objTblDocument.SizeOfDocument ?? 0;
            objDocumentModel.Status = objTblDocument.Status ?? true;
            return (objDocumentModel);
        }
        public TblDocument MapModel2Table(DocumentModel objDocumentModel)
        {
            TblDocument objTblDocument = new TblDocument();
            objTblDocument.Id = objDocumentModel.Id;
            if (objDocumentModel.Id == 0)
                objTblDocument.CreatedDate = DateTime.Now;
            else
                objTblDocument.CreatedDate = objDocumentModel.CreatedDate.ToDefaultDate();
            objTblDocument.ModifiedDate = objDocumentModel.ModifiedDate.ToDefaultDate();
            objTblDocument.URL = objDocumentModel.URL;
            objTblDocument.DocumentName = objDocumentModel.DocumentName;
            objTblDocument.UploadedById = objDocumentModel.UploadedById;
            objTblDocument.UploadDateTime = objDocumentModel.UploadDateTime.ToDefaultDateIfTooEarly();
            objTblDocument.DocumentType = objDocumentModel.DocumentType;
            objTblDocument.SizeOfDocument = objDocumentModel.SizeOfDocument;
            objTblDocument.TenantId = objDocumentModel.TenantId;
            objTblDocument.Status = true;
            return (objTblDocument);
        }
        #endregion

        //#region Employee
        //public TblEmployee MapModel2Table(EmployeeModel objEmployeeModel)
        //{
        //    TblEmployee objTblEmployee = new TblEmployee();
        //    objTblEmployee.Id = objEmployeeModel.Id;
        //    if (objEmployeeModel.Id == 0)
        //        objTblEmployee.CreatedDate = DateTime.Now;
        //    else
        //        objTblEmployee.CreatedDate = objEmployeeModel.CreatedDate.ToDefaultDate();
        //    objTblEmployee.ModifiedDate = objEmployeeModel.ModifiedDate.ToDefaultDate();
        //    objTblEmployee.TenantId = objEmployeeModel.TenantId;
        //    objTblEmployee.ReportToId = objEmployeeModel.ReportingTo;
        //    objTblEmployee.Code = "E" + objEmployeeModel.Id;
        //    objTblEmployee.FirstName = objEmployeeModel.FirstName;
        //    objTblEmployee.LastName = objEmployeeModel.LastName;
        //    objTblEmployee.EmployeeEmail = objEmployeeModel.EmployeeEmail;
        //    objTblEmployee.LoginId = objEmployeeModel.LoginId;
        //    objTblEmployee.EmpRoleId = objEmployeeModel.EmpRoleId;
        //    objTblEmployee.Location = objEmployeeModel.Location;
        //    objTblEmployee.Comments = objEmployeeModel.Comments ?? ".";
        //    objTblEmployee.Status = true;
        //    return (objTblEmployee);
        //}
        //public EmployeeModel MapTable2Model(TblEmployee objTblEmployee)
        //{
        //    EmployeeModel objEmployeeModel = new EmployeeModel();
        //    objEmployeeModel.Id = objTblEmployee.Id;
        //    if (objTblEmployee.Id == 0)
        //        objEmployeeModel.CreatedDate = DateTime.Now;
        //    else
        //        objEmployeeModel.CreatedDate = objTblEmployee.CreatedDate.Value.ToDefaultDate();
        //    objEmployeeModel.ModifiedDate = objTblEmployee.ModifiedDate.Value.ToDefaultDate();
        //    objEmployeeModel.TenantId = objTblEmployee.TenantId;
        //    objEmployeeModel.ReportingTo = Convert.ToInt64(objTblEmployee.ReportToId);
        //    objEmployeeModel.EmpCode = objTblEmployee.Code;
        //    objEmployeeModel.FirstName = objTblEmployee.FirstName;
        //    objEmployeeModel.LastName = objTblEmployee.LastName;
        //    objEmployeeModel.EmployeeEmail = objTblEmployee.EmployeeEmail;
        //    objEmployeeModel.LoginId = objTblEmployee.Id;
        //    objEmployeeModel.EmpRoleId = Convert.ToInt32(objTblEmployee.EmpRoleId);
        //    objEmployeeModel.Location = objTblEmployee.Location;
        //    objEmployeeModel.Comments = objTblEmployee.Comments;
        //    objEmployeeModel.Status = Convert.ToBoolean(objTblEmployee.Status);
        //    return (objEmployeeModel);
        //}
        //#endregion

        #region Login
        public TblLogin MapModel2Table(LoginModel objLoginModel)
        {
            TblLogin objTblLogin = new TblLogin();
            objTblLogin.Id = objLoginModel.Id;
            if (objLoginModel.Id == 0)
                objTblLogin.CreatedDate = DateTime.Now;
            else
                objTblLogin.CreatedDate = objLoginModel.CreatedDate.ToDefaultDate();
            objTblLogin.ModifiedDate = objLoginModel.ModifiedDate.ToDefaultDate();
            objTblLogin.EmailAddress = objLoginModel.EmailAddress;
            objTblLogin.Password = objLoginModel.Password != null ? objLoginModel.Password : Helper.CreateRandomPassword(5).ToString();
            objTblLogin.TenantId = objLoginModel.TenantId;
            objTblLogin.Status = true;
            return (objTblLogin);
        }
        public LoginModel MapTable2Model(TblLogin objTblLogin)
        {
            LoginModel objLoginModel = new LoginModel();
            objLoginModel.Id = objTblLogin.Id;
            if (objTblLogin.Id == 0)
                objLoginModel.CreatedDate = DateTime.Now;
            else
                objLoginModel.CreatedDate = objTblLogin.CreatedDate.Value.ToDefaultDate();
            objLoginModel.ModifiedDate = objTblLogin.ModifiedDate.Value.ToDefaultDate();
            objLoginModel.TenantId = objTblLogin.TenantId;
            objLoginModel.EmailAddress = objTblLogin.EmailAddress;
            objLoginModel.Password = objTblLogin.Password != null ? objTblLogin.Password : new Random(5).ToString();
            objLoginModel.Status = objTblLogin.Status ?? true;
            return (objLoginModel);
        }
        #endregion

        #region LeadPriceDiscount
        public LeadPriceDiscountModel MapTable2Model(TblTransLeadPriceDiscount objTblLPDiscount)
        {
            LeadPriceDiscountModel objLeadPriceDiscountModel = new LeadPriceDiscountModel();
            if (objTblLPDiscount == null)
                return (objLeadPriceDiscountModel);
            objLeadPriceDiscountModel.Id = objTblLPDiscount.Id;
            if (objTblLPDiscount.Id == 0)
                objLeadPriceDiscountModel.CreatedDate = DateTime.Now;
            else
                objLeadPriceDiscountModel.CreatedDate = objTblLPDiscount.CreatedDate.Value.ToDefaultDate();
            objLeadPriceDiscountModel.ModifiedDate = objTblLPDiscount.ModifiedDate.Value.ToDefaultDate();
            objLeadPriceDiscountModel.TenantId = objTblLPDiscount.TenantId;
            objLeadPriceDiscountModel.LeadId = objTblLPDiscount.LeadId ?? 1;
            objLeadPriceDiscountModel.DiscountedDate = objTblLPDiscount.DiscountedDate ?? DateTime.Now;
            objLeadPriceDiscountModel.DiscountedPrice = objTblLPDiscount.DiscountedPrice ?? 0.00M;
            objLeadPriceDiscountModel.EmployeeId = objTblLPDiscount.EmployeeId ?? 1;
            objLeadPriceDiscountModel.ApprovalStatus = Convert.ToBoolean(objTblLPDiscount.ApprovalStatus);
            objLeadPriceDiscountModel.ApprovedBy = objTblLPDiscount.ApprovedBy ?? 1;
            objLeadPriceDiscountModel.ApprovedDate = objTblLPDiscount.ApprovedDate ?? DateTime.Now;
            objLeadPriceDiscountModel.Comments = objTblLPDiscount.Comments;
            objLeadPriceDiscountModel.Status = objTblLPDiscount.Status ?? true;

            return (objLeadPriceDiscountModel);
        }
        #endregion

        #region TransLeadPriceDiscount
        public TblTransLeadPriceDiscount MapModel2Table(LeadPriceDiscountModel objLeadPriceDiscountModel)
        {
            TblTransLeadPriceDiscount objTblLPDiscount = new TblTransLeadPriceDiscount();
            objTblLPDiscount.Id = objLeadPriceDiscountModel.Id;
            if (objLeadPriceDiscountModel.Id == 0)
                objTblLPDiscount.CreatedDate = DateTime.Now;
            else
                objTblLPDiscount.CreatedDate = objLeadPriceDiscountModel.CreatedDate.ToDefaultDate();
            objTblLPDiscount.ModifiedDate = objLeadPriceDiscountModel.ModifiedDate.ToDefaultDate();
            objTblLPDiscount.LeadId = objLeadPriceDiscountModel.LeadId;
            objTblLPDiscount.DiscountedDate = objLeadPriceDiscountModel.DiscountedDate;
            objTblLPDiscount.DiscountedPrice = objLeadPriceDiscountModel.DiscountedPrice;
            objTblLPDiscount.EmployeeId = objLeadPriceDiscountModel.EmployeeId;
            objTblLPDiscount.ApprovalStatus = (objLeadPriceDiscountModel.ApprovalStatus).ToString();
            objTblLPDiscount.ApprovedBy = objLeadPriceDiscountModel.ApprovedBy;
            objTblLPDiscount.ApprovedDate = objLeadPriceDiscountModel.ApprovedDate;
            objTblLPDiscount.Comments = objLeadPriceDiscountModel.Comments;
            objTblLPDiscount.TenantId = objLeadPriceDiscountModel.TenantId;
            objTblLPDiscount.Status = objLeadPriceDiscountModel.Status;
            return (objTblLPDiscount);
        }
        #endregion

        #region Product
        public TblProduct MapModel2Table(ProductModel objProductModel)
        {
            TblProduct objTblProduct = new TblProduct();
            objTblProduct.Id = objProductModel.Id;
            if (objProductModel.Id == 0)
                objTblProduct.CreatedDate = DateTime.Now;
            else
                objTblProduct.CreatedDate = objProductModel.CreatedDate.ToDefaultDate();
            objTblProduct.ModifiedDate = objProductModel.ModifiedDate.ToDefaultDate();
            objTblProduct.Code = objProductModel.ProductCode;
            objTblProduct.ProductName = objProductModel.ProductName;
            objTblProduct.ProductType = objProductModel.ProductType;
            objTblProduct.Price = objProductModel.Price;
            objTblProduct.ExpiryDate = objProductModel.ExpiryDate.ToDefaultDate();
            objTblProduct.ManufacturerName = objProductModel.ManufacturerName;
            objTblProduct.ManufacturerDate = objProductModel.ManufacturerDate.ToDefaultDate();
            objTblProduct.Comments = objProductModel.Comments;
            objTblProduct.TenantId = objProductModel.TenantId;
            objTblProduct.Status = objProductModel.Status;
            return (objTblProduct);
        }

        public ProductModel MapTable2Model(TblProduct objTblProduct)
        {
            ProductModel objProductModel = new ProductModel();
            objProductModel.Id = objTblProduct.Id;
            if (objTblProduct.Id == 0)
                objProductModel.CreatedDate = DateTime.Now;
            else
                objProductModel.CreatedDate = objTblProduct.CreatedDate.Value.ToDefaultDate();
            objProductModel.ModifiedDate = objTblProduct.ModifiedDate.Value.ToDefaultDate();
            objProductModel.TenantId = objTblProduct.TenantId;
            objProductModel.ProductName = objTblProduct.ProductName;
            objProductModel.ProductCode = objTblProduct.Code;
            objProductModel.ProductType = objTblProduct.ProductType;
            objProductModel.Price = Convert.ToDecimal(objTblProduct.Price);
            objProductModel.ExpiryDate = objTblProduct.ExpiryDate.Value.ToDefaultDate();
            objProductModel.Comments = objTblProduct.Comments;
            objProductModel.ManufacturerName = objTblProduct.ManufacturerName;
            objProductModel.ManufacturerDate = DateTime.Now.Date;
            objProductModel.Status = objTblProduct.Status ?? true;
            return (objProductModel);
        }
        #endregion

        #region Setting
        public SettingModel MapTable2Model(TblSetting objTblSetting)
        {
            SettingModel objSettingModel = new SettingModel();
            objSettingModel.Id = objTblSetting.Id;
            if (objTblSetting.Id == 0)
                objSettingModel.CreatedDate = DateTime.Now;
            else
                objSettingModel.CreatedDate = objTblSetting.CreatedDate.Value.ToDefaultDate();
            objSettingModel.ModifiedDate = objTblSetting.ModifiedDate.Value.ToDefaultDate();
            objSettingModel.TenantId = objTblSetting.TenantId;
            objSettingModel.CurrencyCode = objTblSetting.CurrencyCode;
            objSettingModel.ReportingCurrency = objTblSetting.ReportingCurrency;
            objSettingModel.LanguageCode = objTblSetting.LanguageCode;
            objSettingModel.FullName = objTblSetting.FullName;
            objSettingModel.PhoneNo = objTblSetting.PhoneNo;
            objSettingModel.URL = objTblSetting.URL;
            objSettingModel.DateFormat = objTblSetting.DateFormat;
            objSettingModel.Email = objTblSetting.DefaultEmail;
            objSettingModel.DefaultMilestone = objTblSetting.DefaultMilestone;
            objSettingModel.TimeFormat = objTblSetting.TimeFormat;
            objSettingModel.NoOfLicenses = objTblSetting.NoOfLicenses ?? 0;
            objSettingModel.Status = objTblSetting.Status ?? true;
            return (objSettingModel);
        }
        public TblSetting MapModel2Table(SettingModel objSettingModel)
        {
            TblSetting objTblSetting = new TblSetting();
            objTblSetting.Id = objSettingModel.Id;
            if (objSettingModel.Id == 0)
                objTblSetting.CreatedDate = DateTime.Now;
            else
                objTblSetting.CreatedDate = objSettingModel.CreatedDate.ToDefaultDate();
            objTblSetting.ModifiedDate = objSettingModel.ModifiedDate.ToDefaultDate();
            objTblSetting.CurrencyCode = objSettingModel.CurrencyCode;
            objTblSetting.LanguageCode = objSettingModel.LanguageCode;
            objTblSetting.FullName = objSettingModel.FullName;
            objTblSetting.PhoneNo = objSettingModel.PhoneNo;
            objTblSetting.DateFormat = objSettingModel.DateFormat;
            objTblSetting.URL = objSettingModel.URL;
            objTblSetting.DefaultEmail = objSettingModel.Email;
            objTblSetting.DefaultMilestone = objSettingModel.DefaultMilestone;
            objTblSetting.TimeFormat = objSettingModel.TimeFormat;
            objTblSetting.NoOfLicenses = objSettingModel.NoOfLicenses;
            objTblSetting.TenantId = objSettingModel.TenantId;
            objTblSetting.Status = true;
            return (objTblSetting);
        }
        #endregion

        #region Ownership
        public OwnershipModel MapTable2Model(TblOwnership objTblOwnership)
        {
            OwnershipModel objOwnershipModel = new OwnershipModel();
            objOwnershipModel.Id = objTblOwnership.Id;
            if (objTblOwnership.Id == 0)
                objOwnershipModel.CreatedDate = DateTime.Now;
            else
                objOwnershipModel.CreatedDate = objTblOwnership.CreatedDate.Value.ToDefaultDate();
            objOwnershipModel.ModifiedDate = objTblOwnership.ModifiedDate.Value.ToDefaultDate();
            objOwnershipModel.TenantId = objTblOwnership.TenantId;
            objOwnershipModel.Code = objTblOwnership.Code;
            objOwnershipModel.Description = objTblOwnership.Description;
            objOwnershipModel.Status = objTblOwnership.Status ?? true;
            return (objOwnershipModel);
        }
        public TblOwnership MapModel2Table(OwnershipModel objOwnershipModel)
        {
            TblOwnership objTblOwnership = new TblOwnership();
            objTblOwnership.Id = objOwnershipModel.Id;
            if (objOwnershipModel.Id == 0)
                objTblOwnership.CreatedDate = DateTime.Now;
            else
                objTblOwnership.CreatedDate = objOwnershipModel.CreatedDate.ToDefaultDate();
            objTblOwnership.ModifiedDate = objOwnershipModel.ModifiedDate.ToDefaultDate();
            objTblOwnership.Code = objOwnershipModel.Code;
            objTblOwnership.Description = objOwnershipModel.Description;
            objTblOwnership.TenantId = objOwnershipModel.TenantId;
            objTblOwnership.Status = true;
            return (objTblOwnership);
        }
        #endregion

        #region EmployeeRole
        public TblEmployeeRole MapModel2Table(EmployeeRoleModel ObjEmployeeRoleModel)
        {
            TblEmployeeRole ObjTblEmployeeRole = new TblEmployeeRole();
            ObjTblEmployeeRole.Id = ObjEmployeeRoleModel.Id;
            if (ObjEmployeeRoleModel.Id == 0)
                ObjTblEmployeeRole.CreatedDate = DateTime.Now;
            else
                ObjTblEmployeeRole.CreatedDate = ObjEmployeeRoleModel.CreatedDate.ToDefaultDate();
            ObjTblEmployeeRole.ModifiedDate = ObjEmployeeRoleModel.ModifiedDate.ToDefaultDate();
            ObjTblEmployeeRole.TenantId = ObjEmployeeRoleModel.TenantId;
            ObjTblEmployeeRole.Code = ObjEmployeeRoleModel.ERoleCode;
            ObjTblEmployeeRole.RoleOrder = 0;
            ObjTblEmployeeRole.Description = ObjEmployeeRoleModel.Description;
            ObjTblEmployeeRole.SelectedModules = ObjEmployeeRoleModel.SelectedModules;
            ObjTblEmployeeRole.Status = Convert.ToBoolean(ObjEmployeeRoleModel.Status);
            return (ObjTblEmployeeRole);
        }
        public EmployeeRoleModel MapTable2Model(TblEmployeeRole ObjTblEmployeeRole)
        {
            EmployeeRoleModel ObjEmployeeRoleModel = new EmployeeRoleModel();
            ObjEmployeeRoleModel.Id = ObjTblEmployeeRole.Id;
            if (ObjTblEmployeeRole.Id == 0)
                ObjEmployeeRoleModel.CreatedDate = DateTime.Now;
            else
                ObjEmployeeRoleModel.CreatedDate = ObjTblEmployeeRole.CreatedDate.Value.ToDefaultDate();
            ObjEmployeeRoleModel.ModifiedDate = ObjTblEmployeeRole.ModifiedDate.Value.ToDefaultDate();
            ObjEmployeeRoleModel.TenantId = ObjEmployeeRoleModel.TenantId;
            ObjEmployeeRoleModel.ERoleCode = ObjTblEmployeeRole.Code;
            ObjEmployeeRoleModel.Description = ObjTblEmployeeRole.Description;
            ObjEmployeeRoleModel.SelectedModules = ObjTblEmployeeRole.SelectedModules;
            ObjEmployeeRoleModel.Status = Convert.ToBoolean(ObjTblEmployeeRole.Status);
            return (ObjEmployeeRoleModel);
        }
        #endregion

        #region MileStone
        public TblMileStone MapModel2Table(MilestoneModel ObjMileStoneModel)
        {
            TblMileStone objTblMileStone = new TblMileStone();
            objTblMileStone.Id = ObjMileStoneModel.Id;
            if (ObjMileStoneModel.Id == 0)
                objTblMileStone.CreatedDate = DateTime.Now;
            else
                objTblMileStone.CreatedDate = ObjMileStoneModel.CreatedDate.ToDefaultDate();
            objTblMileStone.ModifiedDate = ObjMileStoneModel.ModifiedDate.ToDefaultDate();
            objTblMileStone.Description = ObjMileStoneModel.Description;
            objTblMileStone.Code = ObjMileStoneModel.Code;
            objTblMileStone.MSOrder = ObjMileStoneModel.MSOrder;
            objTblMileStone.TenantId = ObjMileStoneModel.TenantId;
            objTblMileStone.Status = Convert.ToBoolean(ObjMileStoneModel.Status);
            return (objTblMileStone);
        }
        public MilestoneModel MapTable2Model(TblMileStone objTblMileStone)
        {
            MilestoneModel ObjMileStoneModel = new MilestoneModel();
            ObjMileStoneModel.Id = objTblMileStone.Id;
            if (objTblMileStone.Id == 0)
                ObjMileStoneModel.CreatedDate = DateTime.Now;
            else
                ObjMileStoneModel.CreatedDate = objTblMileStone.CreatedDate.Value.ToDefaultDate();
            ObjMileStoneModel.ModifiedDate = objTblMileStone.ModifiedDate.Value.ToDefaultDate();
            ObjMileStoneModel.TenantId = objTblMileStone.TenantId;
            ObjMileStoneModel.Code = objTblMileStone.Code;
            ObjMileStoneModel.MSOrder = (int)objTblMileStone.MSOrder;
            ObjMileStoneModel.Description = objTblMileStone.Description;
            ObjMileStoneModel.Status = Convert.ToBoolean(objTblMileStone.Status);
            return (ObjMileStoneModel);
        }
        #endregion

        #region MileStoneStages
        public TblMileStoneStage MapModel2Table(MilestoneStageModel ObjMilestoneStageModel)
        {
            TblMileStoneStage objTblMileStoneStage = new TblMileStoneStage();
            objTblMileStoneStage.Id = ObjMilestoneStageModel.Id;
            objTblMileStoneStage.MileStoneStage = ObjMilestoneStageModel.MileStoneStage;
            objTblMileStoneStage.Roles = ObjMilestoneStageModel.Roles != null ? ObjMilestoneStageModel.Roles[ObjMilestoneStageModel.Roles.Count - 1].ToString() : string.Empty;

            return (objTblMileStoneStage);
        }
        public MilestoneStageModel MapTable2Model(TblMileStoneStage objTblMileStoneStage)
        {
            MilestoneStageModel ObjMilestoneStageModel = new MilestoneStageModel();
            ObjMilestoneStageModel.Id = objTblMileStoneStage.Id;
            ObjMilestoneStageModel.MileStoneStage = objTblMileStoneStage.MileStoneStage;
            ObjMilestoneStageModel.Roles = objTblMileStoneStage.Roles != null ? objTblMileStoneStage.Roles.Split(',').ToList() : null;
            return (ObjMilestoneStageModel);
        }
        #endregion

        #region ParentAccount
        public ParentAccountModel MapTable2Model(TblParentAccount objTblParentAccount)
        {
            ParentAccountModel objParentAccountModel = new ParentAccountModel();
            objParentAccountModel.Id = objTblParentAccount.Id;
            if (objTblParentAccount.Id == 0)
                objParentAccountModel.CreatedDate = DateTime.Now;
            else
                objParentAccountModel.CreatedDate = objTblParentAccount.CreatedDate.Value.ToDefaultDate();
            objParentAccountModel.ModifiedDate = objTblParentAccount.ModifiedDate.Value.ToDefaultDate();
            objParentAccountModel.TenantId = objTblParentAccount.TenantId;
            objParentAccountModel.Code = objTblParentAccount.Code;
            objParentAccountModel.Description = objTblParentAccount.Description;
            objParentAccountModel.Status = objTblParentAccount.Status ?? true;
            return (objParentAccountModel);
        }
        public TblParentAccount MapModel2Table(ParentAccountModel objParentAccountModel)
        {
            TblParentAccount objTblParentAccount = new TblParentAccount();
            objTblParentAccount.Id = objParentAccountModel.Id;
            if (objParentAccountModel.Id == 0)
                objTblParentAccount.CreatedDate = DateTime.Now;
            else
                objTblParentAccount.CreatedDate = objParentAccountModel.CreatedDate.ToDefaultDate();
            objTblParentAccount.ModifiedDate = objParentAccountModel.ModifiedDate.ToDefaultDate();
            objTblParentAccount.Code = objParentAccountModel.Code;
            objTblParentAccount.Description = objParentAccountModel.Description;
            objTblParentAccount.TenantId = objParentAccountModel.TenantId;
            objTblParentAccount.Status = true;
            return (objTblParentAccount);
        }
        #endregion

        #region   Industry
        public IndustryModel MapTable2Model(TblIndustry objTblIndustry)
        {
            IndustryModel objIndustryModel = new IndustryModel();
            objIndustryModel.Id = objTblIndustry.Id;
            if (objTblIndustry.Id == 0)
                objIndustryModel.CreatedDate = DateTime.Now;
            else
                objIndustryModel.CreatedDate = objTblIndustry.CreatedDate.Value.ToDefaultDate();
            objIndustryModel.ModifiedDate = objTblIndustry.ModifiedDate.Value.ToDefaultDate();
            objIndustryModel.TenantId = objTblIndustry.TenantId;
            objIndustryModel.Code = objTblIndustry.Code;
            objIndustryModel.Description = objTblIndustry.Description;
            objIndustryModel.Status = objTblIndustry.Status ?? true;
            return (objIndustryModel);
        }
        public TblIndustry MapModel2Table(IndustryModel objIndustryModel)
        {
            TblIndustry objTblIndustry = new TblIndustry();
            objTblIndustry.Id = objIndustryModel.Id;
            if (objIndustryModel.Id == 0)
                objTblIndustry.CreatedDate = DateTime.Now;
            else
                objTblIndustry.CreatedDate = objIndustryModel.CreatedDate.ToDefaultDate();
            objTblIndustry.ModifiedDate = objIndustryModel.ModifiedDate.ToDefaultDate();
            objTblIndustry.Code = objIndustryModel.Code;
            objTblIndustry.Description = objIndustryModel.Description;
            objTblIndustry.TenantId = objIndustryModel.TenantId;
            objTblIndustry.Status = true;
            return (objTblIndustry);
        }
        #endregion

        #region AccountType
        public AccountTypeModel MapTable2Model(TblAccountType objTblAccountType)
        {
            AccountTypeModel objAccountTypeModel = new AccountTypeModel();
            objAccountTypeModel.Id = objTblAccountType.Id;
            if (objTblAccountType.Id == 0)
                objAccountTypeModel.CreatedDate = DateTime.Now;
            else
                objAccountTypeModel.CreatedDate = objTblAccountType.CreatedDate.Value.ToDefaultDate();
            objAccountTypeModel.ModifiedDate = objTblAccountType.ModifiedDate.Value.ToDefaultDate();
            objAccountTypeModel.TenantId = objTblAccountType.TenantId;
            objAccountTypeModel.Code = objTblAccountType.Code;
            objAccountTypeModel.Description = objTblAccountType.Description;
            objAccountTypeModel.Status = objTblAccountType.Status ?? true;
            return (objAccountTypeModel);
        }
        public TblAccountType MapModel2Table(AccountTypeModel objAccountTypeModel)
        {
            TblAccountType objTblAccountType = new TblAccountType();
            objTblAccountType.Id = objAccountTypeModel.Id;
            if (objAccountTypeModel.Id == 0)
                objTblAccountType.CreatedDate = DateTime.Now;
            else
                objTblAccountType.CreatedDate = objAccountTypeModel.CreatedDate.ToDefaultDate();
            objTblAccountType.ModifiedDate = objAccountTypeModel.ModifiedDate.ToDefaultDate();
            objTblAccountType.Code = objAccountTypeModel.Code;
            objTblAccountType.Description = objAccountTypeModel.Description;
            objTblAccountType.TenantId = objAccountTypeModel.TenantId;
            objTblAccountType.Status = true;
            return (objTblAccountType);
        }
        #endregion

        #region TblTranExtContact & TranExtContactModel
        public TranLeadContactModel MapTable2Model(TblTranLeadContact objTblTranLeadContact)
        {
            TranLeadContactModel objTranLeadContactModel = new TranLeadContactModel();
            objTranLeadContactModel.Id = objTblTranLeadContact.Id;
            objTranLeadContactModel.TenantId = objTblTranLeadContact.TenantId;
            objTranLeadContactModel.LeadId = objTblTranLeadContact.LeadId;
            objTranLeadContactModel.ContactId = objTblTranLeadContact.ContactId;
            objTranLeadContactModel.ContactRoleId = objTblTranLeadContact.ContactRoleId;
            return (objTranLeadContactModel);
        }
        public TblTranLeadContact MapModel2Table(TranLeadContactModel objTranLeadContactModel)
        {
            TblTranLeadContact objTblTranLeadContact = new TblTranLeadContact();
            objTblTranLeadContact.Id = objTranLeadContactModel.Id;
            objTblTranLeadContact.TenantId = objTranLeadContactModel.TenantId;
            objTblTranLeadContact.LeadId = objTranLeadContactModel.LeadId;
            objTblTranLeadContact.ContactId = objTranLeadContactModel.ContactId;
            objTblTranLeadContact.ContactRoleId = objTranLeadContactModel.ContactRoleId;
            return (objTblTranLeadContact);
        }
        #endregion

        #region AddressContact
        public TblAddressContact MapTable2Model(TransAccConModel objTransAccConModel)
        {
            TblAddressContact objTblAddressContact = new TblAddressContact();
            objTblAddressContact.TenantId = objTransAccConModel.objContactModel.TenantId;
            objTblAddressContact.ContactId = objTransAccConModel.objContactModel.Id;
            objTblAddressContact.AddressId = objTransAccConModel.objAddressModel.Id;
            return (objTblAddressContact);
        }
        #endregion

        #region Department
        public TblDepartment MapModel2Table(DepartmentModel ObjDepartmentModel)
        {
            TblDepartment ObjTblDepartment = new TblDepartment();
            ObjTblDepartment.Id = ObjDepartmentModel.Id;
            if (ObjDepartmentModel.Id == 0)
                ObjTblDepartment.CreatedDate = DateTime.Now;
            else
                ObjTblDepartment.CreatedDate = ObjDepartmentModel.CreatedDate.ToDefaultDate();
            ObjTblDepartment.ModifiedDate = ObjDepartmentModel.ModifiedDate.ToDefaultDate();
            ObjTblDepartment.Code = ObjDepartmentModel.Code;
            ObjTblDepartment.DepartmentName = ObjDepartmentModel.DepartmentName;
            ObjTblDepartment.Description = ObjDepartmentModel.Description;
            ObjTblDepartment.TenantId = ObjDepartmentModel.TenantId;
            ObjTblDepartment.Status = ObjDepartmentModel.Status;
            return (ObjTblDepartment);
        }
        public DepartmentModel MapTable2Model(TblDepartment ObjTblDepartment)
        {
            DepartmentModel ObjDepartmentModel = new DepartmentModel();
            ObjDepartmentModel.Id = ObjTblDepartment.Id;
            if (ObjTblDepartment.Id == 0)
                ObjDepartmentModel.CreatedDate = DateTime.Now;
            else
                ObjDepartmentModel.CreatedDate = ObjTblDepartment.CreatedDate.Value.ToDefaultDate();
            ObjDepartmentModel.ModifiedDate = ObjTblDepartment.ModifiedDate.Value.ToDefaultDate();
            ObjDepartmentModel.Code = ObjTblDepartment.Code;
            ObjDepartmentModel.DepartmentName = ObjTblDepartment.DepartmentName;
            ObjDepartmentModel.Description = ObjTblDepartment.Description;
            ObjDepartmentModel.TenantId = ObjTblDepartment.TenantId;
            ObjDepartmentModel.Status = Convert.ToBoolean(ObjTblDepartment.Status);
            return (ObjDepartmentModel);
        }
        #endregion

        #region FinancialYear
        public TblFinancialYear MapModel2Table(FinancialYearModel objFinancialYearModel)
        {
            TblFinancialYear objTblFinancialYear = new TblFinancialYear();
            objTblFinancialYear.Id = objFinancialYearModel.Id;
            if (objTblFinancialYear.Id == 0)
                objTblFinancialYear.CreatedDate = DateTime.Now;
            else
                objTblFinancialYear.CreatedDate = objFinancialYearModel.CreatedDate.ToDefaultDate();
            objTblFinancialYear.ModifiedDate = objFinancialYearModel.ModifiedDate.ToDefaultDate();
            objTblFinancialYear.FinancialYear = objFinancialYearModel.FinancialYear;
            objTblFinancialYear.Description = objFinancialYearModel.Description;
            objTblFinancialYear.TenantId = objFinancialYearModel.TenantId;
            objTblFinancialYear.Status = objFinancialYearModel.Status;
            return (objTblFinancialYear);
        }
        public FinancialYearModel MapTable2Model(TblFinancialYear objTblFinancialYear)
        {
            FinancialYearModel objFinancialYearModel = new FinancialYearModel();
            objFinancialYearModel.Id = objTblFinancialYear.Id;
            if (objTblFinancialYear.Id == 0)
                objFinancialYearModel.CreatedDate = DateTime.Now;
            else
                objFinancialYearModel.CreatedDate = objTblFinancialYear.CreatedDate.Value.ToDefaultDate();
            objFinancialYearModel.ModifiedDate = objTblFinancialYear.ModifiedDate.Value.ToDefaultDate();
            objFinancialYearModel.FinancialYear = objTblFinancialYear.FinancialYear;
            objFinancialYearModel.Description = objTblFinancialYear.Description;
            objFinancialYearModel.TenantId = objTblFinancialYear.TenantId;
            objFinancialYearModel.Status = Convert.ToBoolean(objTblFinancialYear.Status);
            return (objFinancialYearModel);
        }
        #endregion

        #region EmployeeTarget
        public EmployeeTargetModel MapTable2Model(TblEmployeeTarget objTblEmployeeTarget)
        {
            EmployeeTargetModel objEmployeeTargetModel = new EmployeeTargetModel();
            objEmployeeTargetModel.Id =Convert.ToInt32(objTblEmployeeTarget.Id);
            if (objTblEmployeeTarget.Id == 0)
                objEmployeeTargetModel.CreatedDate = DateTime.Now;
            else
                objEmployeeTargetModel.CreatedDate = objTblEmployeeTarget.CreatedDate.Value.ToDefaultDate();
            objEmployeeTargetModel.ModifiedDate = objTblEmployeeTarget.ModifiedDate.Value.ToDefaultDate();
            objEmployeeTargetModel.TenantId = objTblEmployeeTarget.TenantId;
            objEmployeeTargetModel.FinancialYearId = objTblEmployeeTarget.FinancialYearId ?? 0;
            objEmployeeTargetModel.EmployeeRoleId = objTblEmployeeTarget.EmployeeRoleId ?? 0;
            objEmployeeTargetModel.IsAutomatic = objTblEmployeeTarget.IsAutomatic ?? true;
            objEmployeeTargetModel.QuarterlyTarget = objTblEmployeeTarget.QuarterlyTarget ?? 0;
            objEmployeeTargetModel.MonthlyTarget = objTblEmployeeTarget.MonthlyTarget ?? 0;
            objEmployeeTargetModel.WeeklyTarget = objTblEmployeeTarget.WeeklyTarget ?? 0;
            objEmployeeTargetModel.Budget = objTblEmployeeTarget.Budget ?? 0;
            objEmployeeTargetModel.Status = objTblEmployeeTarget.Status ?? true;
            return (objEmployeeTargetModel);
        }
        public TblEmployeeTarget MapModel2Table(EmployeeTargetModel objEmployeeTargetModel)
        {
            TblEmployeeTarget objTblEmployeeTarget = new TblEmployeeTarget();
            if (objEmployeeTargetModel.FinancialYear != null && objEmployeeTargetModel.EmployeeRole != null)
            {
                objEmployeeTargetModel.FinancialYearId = Convert.ToInt64(objEmployeeTargetModel.FinancialYear);
                objEmployeeTargetModel.EmployeeRoleId = Convert.ToInt64(objEmployeeTargetModel.EmployeeRole);
            }
            objTblEmployeeTarget.Id = objEmployeeTargetModel.Id;
            if (objEmployeeTargetModel.Id == 0)
            {
                objTblEmployeeTarget.CreatedDate = DateTime.Now;
                objEmployeeTargetModel.Status = true;
            }
            else
                objTblEmployeeTarget.CreatedDate = objEmployeeTargetModel.CreatedDate.ToDefaultDate();
            objTblEmployeeTarget.ModifiedDate = objEmployeeTargetModel.ModifiedDate.ToDefaultDate();
            objTblEmployeeTarget.TenantId = objEmployeeTargetModel.TenantId;
            objTblEmployeeTarget.FinancialYearId = objEmployeeTargetModel.FinancialYearId;
            objTblEmployeeTarget.EmployeeRoleId = objEmployeeTargetModel.EmployeeRoleId;
            objTblEmployeeTarget.IsAutomatic = objEmployeeTargetModel.IsAutomatic;
            objTblEmployeeTarget.TargetHike = objEmployeeTargetModel.TargetHike;
            objTblEmployeeTarget.QuarterlyTarget = objEmployeeTargetModel.QuarterlyTarget;
            objTblEmployeeTarget.MonthlyTarget = objEmployeeTargetModel.MonthlyTarget;
            objTblEmployeeTarget.WeeklyTarget = objEmployeeTargetModel.WeeklyTarget;
            objTblEmployeeTarget.Budget = objEmployeeTargetModel.Budget;
            objTblEmployeeTarget.Status = objEmployeeTargetModel.Status;
            return (objTblEmployeeTarget);
        }
        #endregion

        #region EmployeeTask

        public TblEmpTask MapModel2Table(EmployeeTaskModel ObjEmployeeTaskModel)
        {
            TblEmpTask objTblEmpTask = new TblEmpTask();
            objTblEmpTask.Id = ObjEmployeeTaskModel.Id;
            if (ObjEmployeeTaskModel.Id == 0)
                objTblEmpTask.CreatedDate = DateTime.Now;
            else
                objTblEmpTask.CreatedDate = ObjEmployeeTaskModel.CreatedDate.ToDefaultDate();
            objTblEmpTask.ModifiedDate = ObjEmployeeTaskModel.ModifiedDate.ToDefaultDate();
            objTblEmpTask.TaskDate = ObjEmployeeTaskModel.TaskDate;
            objTblEmpTask.TaskOwnerId = ObjEmployeeTaskModel.TaskOwnerId;
            objTblEmpTask.Title = ObjEmployeeTaskModel.Title;
            objTblEmpTask.Type_Task = ObjEmployeeTaskModel.Type_Task;
            objTblEmpTask.TenantId = ObjEmployeeTaskModel.TenantId;
            objTblEmpTask.Comments = ObjEmployeeTaskModel.Comments;
            objTblEmpTask.StartDate = ObjEmployeeTaskModel.StartDate;
            objTblEmpTask.EndDate = ObjEmployeeTaskModel.EndDate;
            objTblEmpTask.Status = ObjEmployeeTaskModel.Status;
            return (objTblEmpTask);
        }
        public EmployeeTaskModel MapTable2Model(TblEmpTask ObjTblEmpTask)
        {
            EmployeeTaskModel objEmployeeTaskModel = new EmployeeTaskModel();
            objEmployeeTaskModel.Id = Convert.ToInt32(ObjTblEmpTask.Id);
            if (ObjTblEmpTask.Id == 0)
                objEmployeeTaskModel.CreatedDate = DateTime.Now;
            else
                objEmployeeTaskModel.CreatedDate = ObjTblEmpTask.CreatedDate.Value.ToDefaultDate();
            objEmployeeTaskModel.ModifiedDate = ObjTblEmpTask.ModifiedDate.Value.ToDefaultDate();
            objEmployeeTaskModel.TaskDate = ObjTblEmpTask.TaskDate.Value.ToDefaultDate();
            objEmployeeTaskModel.TaskOwnerId = ObjTblEmpTask.TaskOwnerId;
            objEmployeeTaskModel.Title = ObjTblEmpTask.Title;
            objEmployeeTaskModel.StartDate = ObjTblEmpTask.StartDate.Value;
            objEmployeeTaskModel.EndDate = ObjTblEmpTask.EndDate.Value.AddMinutes(2);
            objEmployeeTaskModel.Type_Task = ObjTblEmpTask.Type_Task;
            objEmployeeTaskModel.TenantId = ObjTblEmpTask.TenantId;
            objEmployeeTaskModel.Comments = ObjTblEmpTask.Comments;
            objEmployeeTaskModel.Status = ObjTblEmpTask.Status ?? true;
            return (objEmployeeTaskModel);

        }
        public List<EmployeeTaskModel> MapTable2Model(IEnumerable<TblEmpTask> query)
        {
            List<EmployeeTaskModel> lstEmployeeTaskModel = new List<EmployeeTaskModel>();
            foreach (var item in query)
            {
                lstEmployeeTaskModel.Add(MapTable2Model(item));
            }
            return (lstEmployeeTaskModel);
        }
        #endregion

        #region TransLeadCompetitor

        public TblTransLeadCompetitor MapModel2Table(TransLProductModel objTransLProductModel)
        {
            TblTransLeadCompetitor objTblTransLeadCompetitor = new TblTransLeadCompetitor();
            objTblTransLeadCompetitor.Id = objTransLProductModel.Id;
            if (objTransLProductModel.Id == 0)
                objTblTransLeadCompetitor.CreatedDate = DateTime.Now;
            else
                objTblTransLeadCompetitor.CreatedDate = objTransLProductModel.CreatedDate.Value.ToDefaultDate();
            objTblTransLeadCompetitor.TenantId = objTransLProductModel.TenantId;
            objTblTransLeadCompetitor.CompetitorId = objTransLProductModel.CompetitorId;
            objTblTransLeadCompetitor.LeadId = objTransLProductModel.LeadId;
            objTblTransLeadCompetitor.ProductId = objTransLProductModel.ProductId;
            objTblTransLeadCompetitor.Quantity = objTransLProductModel.Quantity;
            objTblTransLeadCompetitor.Price = objTransLProductModel.Price;
            objTblTransLeadCompetitor.Amount = objTransLProductModel.Price * objTransLProductModel.Quantity;
            objTblTransLeadCompetitor.ModifiedDate = objTransLProductModel.ModifiedDate;
            objTblTransLeadCompetitor.Status = objTransLProductModel.Status;
            return (objTblTransLeadCompetitor);
        }
        #endregion

        #region Country
        public CountryModel MapTable2Model(TblCountry objTblCountry)
        {
            CountryModel objCountryModel = new CountryModel();
            objCountryModel.CountryId = objTblCountry.CountryId;
            if (objTblCountry.CountryId == 0)
                objCountryModel.CreatedDate = DateTime.Now;
            else
                objCountryModel.CreatedDate = objTblCountry.CreatedDate.Value.ToDefaultDate();
            objCountryModel.ModifiedDate = objTblCountry.ModifiedDate.Value.ToDefaultDate();
            //objCountryModel.TenantId = objTblCountry.TenantId;
            objCountryModel.Code = objTblCountry.Code;
            objCountryModel.CountryName = objTblCountry.CountryName;
            objCountryModel.MobileCode = objTblCountry.MobileCode;
            objCountryModel.Status = objTblCountry.Status ?? true;
            return (objCountryModel);
        }

        public TblCountry MapModel2Table(CountryModel objCountryModel)
        {
            TblCountry objTblCountry = new TblCountry();
            objTblCountry.CountryId = objCountryModel.CountryId;
            if (objCountryModel.CountryId == 0)
                objTblCountry.CreatedDate = DateTime.Now;
            else
                objTblCountry.CreatedDate = objCountryModel.CreatedDate.ToDefaultDate();
            objTblCountry.ModifiedDate = objCountryModel.ModifiedDate.ToDefaultDate();
            objTblCountry.Code = objCountryModel.Code;
           // objTblCountry.TenantId = objCountryModel.TenantId;
            objTblCountry.CountryName = objCountryModel.CountryName;
            objTblCountry.MobileCode = objCountryModel.MobileCode;
            objTblCountry.Status = true;
            return (objTblCountry);
        }


        #endregion

        #region State
        public StateModel MapTable2Model(TblState objTblState)
        {
            StateModel objStateModel = new StateModel();
            objStateModel.StateId = objTblState.StateId;
            if (objTblState.StateId == 0)
                objStateModel.CreatedDate = DateTime.Now;
            else
                objStateModel.CreatedDate = objTblState.CreatedDate.Value.ToDefaultDate();
            objStateModel.ModifiedDate = objTblState.ModifiedDate.Value.ToDefaultDate();
            objStateModel.Code = objTblState.Code;
            //objStateModel.TenantId = objTblState.TenantId;
            objStateModel.StateName = objTblState.StateName;
            objStateModel.CountryId = Convert.ToInt64(objTblState.CountryId);
            objStateModel.Status = objTblState.Status ?? true;
            return (objStateModel);
        }

        public TblState MapModel2Table(StateModel objStateModel)
        {
            TblState objTblState = new TblState();
            objTblState.StateId = objStateModel.StateId;
            if (objStateModel.StateId == 0)
                objTblState.CreatedDate = DateTime.Now;
            else
                objTblState.CreatedDate = objStateModel.CreatedDate.ToDefaultDate();
            objTblState.ModifiedDate = objStateModel.ModifiedDate.ToDefaultDate();
            objTblState.Code = objStateModel.Code;
            //objTblState.TenantId = objStateModel.TenantId;
            objTblState.StateName = objStateModel.StateName;
            objTblState.CountryId = objStateModel.CountryId;
            objTblState.Status = true;
            return (objTblState);
        }
        #endregion

        #region City
        public CityModel MapTable2Model(TblCity objTblCity)
        {
            CityModel objCityModel = new CityModel();
            objCityModel.Id = objTblCity.Id;
            if (objTblCity.Id == 0)
                objCityModel.CreatedDate = DateTime.Now;
            else
                objCityModel.CreatedDate = objTblCity.CreatedDate.Value.ToDefaultDate();
            objCityModel.ModifiedDate = objTblCity.ModifiedDate.Value.ToDefaultDate();
            objCityModel.Code = objTblCity.Code;
            //objCityModel.TenantId = objTblCity.TenantId;
            objCityModel.CityName = objTblCity.CityName;
            objCityModel.StateId = Convert.ToInt64(objTblCity.StateId);
            objCityModel.Status = objTblCity.Status ?? true;
            return (objCityModel);
        }

        public TblCity MapModel2Table(CityModel objCityModel)
        {
            TblCity objTblCity = new TblCity();
            objTblCity.Id = objCityModel.Id;
            if (objCityModel.Id == 0)
                objTblCity.CreatedDate = DateTime.Now;
            else
                objTblCity.CreatedDate = objCityModel.CreatedDate.ToDefaultDate();
            objTblCity.ModifiedDate = objCityModel.ModifiedDate.ToDefaultDate();
            objTblCity.Code = objCityModel.Code;
            //objTblCity.TenantId = objCityModel.TenantId;
            objTblCity.CityName = objCityModel.CityName;
            objTblCity.StateId = objCityModel.StateId;
            objTblCity.Status = true;
            return (objTblCity);
        }
        #endregion


        #region Incentive
        public IncentiveModel MapTable2Model(TblIncentive objTblIncentive)
        {
            IncentiveModel objIncentiveModel = new IncentiveModel();
            objIncentiveModel.Id = objTblIncentive.Id;
            if (objTblIncentive.Id == 0)
                objIncentiveModel.CreatedDate = DateTime.Now;
            else
                objIncentiveModel.CreatedDate = objTblIncentive.CreatedDate.Value.ToDefaultDate();
            objIncentiveModel.ModifiedDate = objTblIncentive.ModifiedDate.Value.ToDefaultDate();
            objIncentiveModel.TenantId = objTblIncentive.TenantId;
            objIncentiveModel.IncFrom = objTblIncentive.IncFrom;
            objIncentiveModel.IncTo = objTblIncentive.IncTo;
            objIncentiveModel.Percentage = objTblIncentive.Percentage;
            objIncentiveModel.Comments = objTblIncentive.Comments;
            objIncentiveModel.Status = objTblIncentive.Status ?? true;
            return (objIncentiveModel);
        }
        public TblIncentive MapModel2Table(IncentiveModel objIncentiveModel)
        {
            TblIncentive objTblIncentive = new TblIncentive();
            objTblIncentive.Id = objIncentiveModel.Id;
            if (objIncentiveModel.Id == 0)
                objTblIncentive.CreatedDate = DateTime.Now;
            else
                objTblIncentive.CreatedDate = objIncentiveModel.CreatedDate.ToDefaultDate();
            objTblIncentive.ModifiedDate = objIncentiveModel.ModifiedDate.ToDefaultDate();
            objTblIncentive.IncFrom = objIncentiveModel.IncFrom;
            objTblIncentive.IncTo = objIncentiveModel.IncTo;
            objTblIncentive.Percentage = objIncentiveModel.Percentage;
            objTblIncentive.Comments = objIncentiveModel.Comments;
            objTblIncentive.TenantId = objIncentiveModel.TenantId;
            objTblIncentive.Status = true;
            return (objTblIncentive);
        }
        #endregion

        #region Location
        public LocationModel MapTable2Model(TblLocation objTblLocation)
        {
            LocationModel objLocationModel = new LocationModel();
            objLocationModel.Id = objTblLocation.Id;
            if (objTblLocation.Id == 0)
                objLocationModel.CreatedDate = DateTime.Now;
            else
                objLocationModel.CreatedDate = objTblLocation.CreatedDate.Value.ToDefaultDate();
            objLocationModel.ModifiedDate = objTblLocation.ModifiedDate.Value.ToDefaultDate();
            objLocationModel.TenantId = objTblLocation.TenantId;
            objLocationModel.Code = objTblLocation.Code;
            objLocationModel.Description = objTblLocation.Description;
            objLocationModel.Status = objTblLocation.Status ?? true;
            return (objLocationModel);
        }
        public TblLocation MapModel2Table(LocationModel objLocationModel)
        {
            TblLocation objTblLocation = new TblLocation();
            objTblLocation.Id = objLocationModel.Id;
            if (objLocationModel.Id == 0)
                objTblLocation.CreatedDate = DateTime.Now;
            else
                objTblLocation.CreatedDate = objLocationModel.CreatedDate.ToDefaultDate();
            objTblLocation.ModifiedDate = objLocationModel.ModifiedDate.ToDefaultDate();
            objTblLocation.Code = objLocationModel.Code;
            objTblLocation.Description = objLocationModel.Description;
            objTblLocation.TenantId = objLocationModel.TenantId;
            objTblLocation.Status = true;
            return (objTblLocation);
        }
        #endregion

        #region Employee
        public TblEmployee MapModel2Table(EmployeeModel objEmployeeModel)
        {
            TblEmployee objTblEmployee = new TblEmployee();
            objTblEmployee.Id = objEmployeeModel.Id;
            if (objEmployeeModel.Id == 0)
                objTblEmployee.CreatedDate = DateTime.Now;
            else
                objTblEmployee.CreatedDate = objEmployeeModel.CreatedDate.ToDefaultDate();
            objTblEmployee.ModifiedDate = objEmployeeModel.ModifiedDate.ToDefaultDate();
            objTblEmployee.TenantId = objEmployeeModel.TenantId;
            objTblEmployee.ReportToId = objEmployeeModel.ReportingTo;
            objTblEmployee.Code = "E" + objEmployeeModel.Id;
            objTblEmployee.FirstName = objEmployeeModel.FirstName;
            objTblEmployee.LastName = objEmployeeModel.LastName;
            objTblEmployee.EmployeeEmail = objEmployeeModel.EmployeeEmail;
            objTblEmployee.LoginId = objEmployeeModel.LoginId;
            objTblEmployee.EmpRoleId = objEmployeeModel.EmpRoleId;
            objTblEmployee.Location = objEmployeeModel.Location;
            objTblEmployee.Comments = objEmployeeModel.Comments ?? ".";
            objTblEmployee.Status = true;
            objTblEmployee.BornPlace = objEmployeeModel.BornPlace;
            //DateTime validValue;
            //objTblEmployee.BornDate = DateTime.TryParse(BankDetails, out validValue)
            //    ? validValue
            //    : (DateTime?)null;
            objTblEmployee.BornDate = objEmployeeModel.BornDate;
            objTblEmployee.JoinDate = objEmployeeModel.JoinDate;
            objTblEmployee.ResignDate = objEmployeeModel.ResignDate;
            objTblEmployee.BankId = objEmployeeModel.BankId;
            objTblEmployee.BankNumber = objEmployeeModel.BankNumber;
            objTblEmployee.BankDetails = objEmployeeModel.BankDetails;
            objTblEmployee.EmployeeImageURL = objEmployeeModel.EmployeeImageURL;

            return (objTblEmployee);
        }
        public EmployeeModel MapTable2Model(TblEmployee objTblEmployee)
        {
            EmployeeModel objEmployeeModel = new EmployeeModel();
            objEmployeeModel.Id = objTblEmployee.Id;
            if (objTblEmployee.Id == 0)
                objEmployeeModel.CreatedDate = DateTime.Now;
            else
                objEmployeeModel.CreatedDate = objTblEmployee.CreatedDate.Value.ToDefaultDate();
            objEmployeeModel.ModifiedDate = objTblEmployee.ModifiedDate.Value.ToDefaultDate();
            objEmployeeModel.TenantId = objTblEmployee.TenantId;
            objEmployeeModel.ReportingTo = Convert.ToInt64(objTblEmployee.ReportToId);
            objEmployeeModel.EmpCode = objTblEmployee.Code;
            objEmployeeModel.FirstName = objTblEmployee.FirstName;
            objEmployeeModel.LastName = objTblEmployee.LastName;
            objEmployeeModel.EmployeeEmail = objTblEmployee.EmployeeEmail;
            objEmployeeModel.LoginId = objTblEmployee.Id;
            objEmployeeModel.EmpRoleId = Convert.ToInt32(objTblEmployee.EmpRoleId);
            objEmployeeModel.Location = objTblEmployee.Location;
            objEmployeeModel.Comments = objTblEmployee.Comments;
            objEmployeeModel.Status = Convert.ToBoolean(objTblEmployee.Status);
            objEmployeeModel.BornPlace = objTblEmployee.BornPlace;
            objEmployeeModel.BornDate = objTblEmployee.BornDate;
            objEmployeeModel.JoinDate = objTblEmployee.JoinDate;
            objEmployeeModel.ResignDate = objTblEmployee.ResignDate;
            objEmployeeModel.BankId = objTblEmployee.BankId;
            objEmployeeModel.BankNumber = objTblEmployee.BankNumber;
            objEmployeeModel.BankDetails = objTblEmployee.BankDetails;
            objEmployeeModel.EmployeeImageURL = objTblEmployee.EmployeeImageURL;

            return (objEmployeeModel);
        }
        #endregion
    }
}
