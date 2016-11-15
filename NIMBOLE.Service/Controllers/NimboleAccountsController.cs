using AutoMapper;
using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Service.Models;
using NIMBOLE.Common;
using NIMBOLE.Entities;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/NimboleAccounts")]
    public class NimboleAccountsController : ApiController
    {
        private static Object thisLock = new Object();
        private NimboleStagingEntities dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        #region POST
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Post([FromBody] NimboleAccountModel objNimboleAccountModel)
        {
            System.Data.Entity.DbContextTransaction dbTran = dbcontext.Database.BeginTransaction();
            try
            {
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);

                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                using (dbTran)
                {
                    TblAccount objTblAccount = objNIMBOLEMapper.MapModel2Table(objNimboleAccountModel);
                    lock (thisLock)
                    {
                        #region Account

                        if (!dbcontext.TblAccounts.Any(u => u.AccountName == objNimboleAccountModel.AccountName && u.TenantId == objNimboleAccountModel.TenantId))
                        {
                            dbcontext.TblAccounts.Add(objTblAccount);
                            dbcontext.SaveChanges();
                            objNimboleAccountModel.Id = objTblAccount.Id;

                            if (objNimboleAccountModel.objAddressModel != null && (objNimboleAccountModel.objAddressModel.CityId != 0 || objNimboleAccountModel.objAddressModel.CountryId != 0 || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.Mobile) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.Phone) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.HouseNo) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.Fax) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.HomePhone) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.StreetName) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.ZipCode) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.SkypeName)))
                            {
                                #region Address
                                objNimboleAccountModel.objAddressModel.Address_Type = "A";
                                objNimboleAccountModel.objAddressModel.TenantId = objNimboleAccountModel.TenantId;
                                TblAddress objTblAddress = objNIMBOLEMapper.MapModel2Table(objNimboleAccountModel.objAddressModel);
                                dbcontext.TblAddresses.Add(objTblAddress);
                                dbcontext.SaveChanges();
                                objNimboleAccountModel.objAddressModel.Id = objTblAddress.Id;
                                #endregion

                                #region AddressAccount
                                TblTranAccAdd objTblTranAccAdd = new TblTranAccAdd();
                                //objTblTranAccAdd.TenantId = objNimboleAccountModel.TenantId.ToDefaultTenantId();
                                objTblTranAccAdd.TenantId = objNimboleAccountModel.TenantId;
                                objTblTranAccAdd.AddressId = objTblAddress.Id;
                                objTblTranAccAdd.AccountId = objTblAccount.Id;
                                objTblTranAccAdd.CreatedDate = DateTime.Now;
                                objTblTranAccAdd.ModifiedDate = DateTime.Now;
                                objTblTranAccAdd.Status = true;
                                dbcontext.TblTranAccAdds.Add(objTblTranAccAdd);
                                dbcontext.SaveChanges();
                                #endregion
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("Record already exist/inactive.");
                        }
                        #endregion
                        dbTran.Commit();
                        return Ok(objNimboleAccountModel.Id.ToString());
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {

                if (dbTran.UnderlyingTransaction.Connection != null)
                    dbTran.Rollback();
                #region Model Validation Exception Handling
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
                #endregion
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        /// <summary>
        /// Modified by Ravi for Combobox
        /// </summary>
        /// <param name="objNimboleAccountModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertAccount")]
        public IHttpActionResult PostAccount([FromBody] NimboleAccountModel objNimboleAccountModel)
        {
            try
            {
                lock (thisLock)
                {
                    HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);

                    #region If ModelState is not Valid
                    if (response.StatusCode != HttpStatusCode.OK)
                        return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                    #endregion

                    TblAccount objTblAccount = objNIMBOLEMapper.MapModel2Table(objNimboleAccountModel);
                    if (!dbcontext.TblAccounts.Any(u => u.AccountName == objNimboleAccountModel.AccountName && u.TenantId==objNimboleAccountModel.TenantId))
                    {
                        dbcontext.TblAccounts.Add(objTblAccount);
                        dbcontext.SaveChanges();
                    }
                    else
                    {
                        //TblAccount _objTblAccount = dbcontext.TblAccounts.Where(u => u.AccountName == objNimboleAccountModel.AccountName).FirstOrDefault();
                        //if (_objTblAccount.Status == false)
                        //throw new InvalidOperationException("Record already exist.");

                        throw new InvalidOperationException("Record already exist/inactive.");

                    }
                    objNimboleAccountModel = objNIMBOLEMapper.MapTable2Model(objTblAccount);
                    objNimboleAccountModel.Id = objTblAccount.Id;

                    var data = (from x in dbcontext.TblAccounts where x.Status == true && x.TenantId==objNimboleAccountModel.TenantId select new { Id = x.Id, Name = x.AccountName }).ToList();
                    return Ok(data);
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = dbEx.InnerException.InnerException.Message
                });

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("InsertAccountType")]
        public IHttpActionResult PostAccountType([FromBody] AccountTypeModel objAccountTypeModel)
        {
            try
            {
                lock (thisLock)
                {
                    HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);

                    #region If ModelState is not Valid
                    if (response.StatusCode != HttpStatusCode.OK)
                        return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                    #endregion
                    TblAccountType objTblAccountType = objNIMBOLEMapper.MapModel2Table(objAccountTypeModel);
                    if (!dbcontext.TblAccountTypes.Any(u => u.Description == objAccountTypeModel.Description))
                    {
                        dbcontext.TblAccountTypes.Add(objTblAccountType);
                        dbcontext.SaveChanges();
                    }
                    else
                    {
                        //TblAccountType _objTblAccountType = dbcontext.TblAccountTypes.Where(u => u.Description == objAccountTypeModel.Description).FirstOrDefault();
                        //if (_objTblAccountType.Status == false)
                        //   throw new InvalidOperationException("Record already exist.");
                        throw new InvalidOperationException("Record already exist/inactive.");
                    }
                    objAccountTypeModel = objNIMBOLEMapper.MapTable2Model(objTblAccountType);

                    var data = (from x in dbcontext.TblAccountTypes where x.Status == true select new { Id = x.Id, Name = x.Description }).ToList();
                    return Ok(data);
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = dbEx.InnerException.InnerException.Message
                });
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        #endregion

        #region PUT
        [HttpPut]
        [Route("Edit")]
        public IHttpActionResult Edit([FromBody] NimboleAccountModel objNimboleAccountModel)
        {
            System.Data.Entity.DbContextTransaction dbTran = dbcontext.Database.BeginTransaction();
            try
            {
              double num;
              var parentAccount = objNimboleAccountModel.ParentAccount;
              var distributerAccount = objNimboleAccountModel.DistributerName;
              string checkPnum = parentAccount;
              string checkDnum = distributerAccount;
                if (double.TryParse(checkPnum, out num)) {
                    // It's a number!
                }
                else 
                {
                   long parentId = (from a in dbcontext.TblAccounts where a.AccountName == objNimboleAccountModel.ParentAccount select a.Id).FirstOrDefault();
                    objNimboleAccountModel.ParentAccount = Convert.ToString(parentId);
                }
                 if (double.TryParse(checkDnum, out num)){
                    // It's a number!
                }
                else 
                {
                   long distributerId = (from a in dbcontext.TblAccounts where a.AccountName == objNimboleAccountModel.DistributerName select a.Id).FirstOrDefault();
                   objNimboleAccountModel.DistributerName = Convert.ToString(distributerId);
                }
                using (dbTran)
                {
                    if (objNimboleAccountModel != null)
                    {
                        TblAccount _objAccount = (from x in dbcontext.TblAccounts where x.Id == objNimboleAccountModel.Id select x).FirstOrDefault();
                        if (_objAccount == null)
                            throw new InvalidOperationException("Record is not found");

                        if (_objAccount.AccountName == objNimboleAccountModel.AccountName)
                        {
                            _objAccount.Code = objNimboleAccountModel.AccountCode;
                            _objAccount.AccountOwner = objNimboleAccountModel.AccountOwner;
                            _objAccount.AccountClassification = objNimboleAccountModel.AccountClassification;
                            _objAccount.ParentAccount = objNimboleAccountModel.IsParentAccount ? null : objNimboleAccountModel.ParentAccount;
                            _objAccount.IsParentAccount = objNimboleAccountModel.IsParentAccount;
                            _objAccount.Employees = objNimboleAccountModel.Employees;
                            _objAccount.OwnerShip = objNimboleAccountModel.OwnerShip;
                            _objAccount.Industry = objNimboleAccountModel.Industry;
                            _objAccount.AccountType = objNimboleAccountModel.AccountType;
                            _objAccount.DistributerName = objNimboleAccountModel.DistributerName;
                            _objAccount.Subsidiary = objNimboleAccountModel.Subsidiary;
                            _objAccount.Region = objNimboleAccountModel.Region;
                            _objAccount.Phone = objNimboleAccountModel.Phone;
                            _objAccount.Fax = objNimboleAccountModel.Fax;
                            _objAccount.Email = objNimboleAccountModel.Email;
                            _objAccount.Rating = objNimboleAccountModel.Rating;
                            _objAccount.SICCode = objNimboleAccountModel.SICCode;
                            _objAccount.AnnualRevenue = objNimboleAccountModel.AnnualRevenue;
                            _objAccount.Website = objNimboleAccountModel.Website;
                            _objAccount.Status = objNimboleAccountModel.Status;
                            _objAccount.ModifiedDate = DateTime.Now;// entity.ModifiedDate; 
                            dbcontext.SaveChanges();
                        }
                        else
                        {
                            if (!dbcontext.TblAccounts.Any(u => u.AccountName == objNimboleAccountModel.AccountName))
                            {
                                _objAccount.AccountName = objNimboleAccountModel.AccountName;
                                _objAccount.Code = objNimboleAccountModel.AccountCode;
                                _objAccount.AccountOwner = objNimboleAccountModel.AccountOwner;
                                _objAccount.AccountClassification = objNimboleAccountModel.AccountClassification;
                                _objAccount.ParentAccount = objNimboleAccountModel.IsParentAccount ? null : objNimboleAccountModel.ParentAccount;
                                _objAccount.Employees = objNimboleAccountModel.Employees;
                                _objAccount.OwnerShip = objNimboleAccountModel.OwnerShip;
                                _objAccount.Industry = objNimboleAccountModel.Industry;
                                _objAccount.AccountType = objNimboleAccountModel.AccountType;
                                _objAccount.IsParentAccount = objNimboleAccountModel.IsParentAccount;
                                _objAccount.DistributerName = objNimboleAccountModel.DistributerName;
                                _objAccount.Subsidiary = objNimboleAccountModel.Subsidiary;
                                _objAccount.Region = objNimboleAccountModel.Region;
                                _objAccount.Phone = objNimboleAccountModel.Phone;
                                _objAccount.Fax = objNimboleAccountModel.Fax;
                                _objAccount.Email = objNimboleAccountModel.Email;
                                _objAccount.Rating = objNimboleAccountModel.Rating;
                                _objAccount.SICCode = objNimboleAccountModel.SICCode;
                                _objAccount.AnnualRevenue = objNimboleAccountModel.AnnualRevenue;
                                _objAccount.Website = objNimboleAccountModel.Website;
                                _objAccount.Status = objNimboleAccountModel.Status;
                                _objAccount.ModifiedDate = DateTime.Now;// entity.ModifiedDate; 
                                dbcontext.SaveChanges();
                            }
                            else
                            {
                                throw new InvalidOperationException("Record already exist/inactive.");
                            }
                        }
                    }

                    TblTranAccAdd objTblTranAccAdd = (from adAcc in dbcontext.TblTranAccAdds where adAcc.AccountId == objNimboleAccountModel.Id select adAcc).FirstOrDefault();
                    if (objTblTranAccAdd != null)
                    {
                        var objTblAddress = (from adModel in dbcontext.TblAddresses where adModel.Id == objTblTranAccAdd.AddressId select adModel).FirstOrDefault();

                        if (objTblAddress != null && (objNimboleAccountModel.objAddressModel.CityId != 0 || objNimboleAccountModel.objAddressModel.CountryId != 0 || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.Mobile) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.Phone) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.HouseNo) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.Fax) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.HomePhone) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.StreetName) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.ZipCode) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.SkypeName)))
                        {
                            objNimboleAccountModel.objAddressModel.Id = objTblAddress.Id;
                           // objTblAddress.TenantId = objNimboleAccountModel.TenantId;
                            objTblAddress.Address_Type = "A";
                            objTblAddress.StreetName = objNimboleAccountModel.objAddressModel.StreetName;
                            objTblAddress.HouseNo = objNimboleAccountModel.objAddressModel.HouseNo;
                            objTblAddress.CityId = objNimboleAccountModel.objAddressModel.CityId;
                            objTblAddress.StateId = objNimboleAccountModel.objAddressModel.StateId;
                            objTblAddress.CountryId = objNimboleAccountModel.objAddressModel.CountryId;
                            objTblAddress.ZipCode = objNimboleAccountModel.objAddressModel.ZipCode;
                            objTblAddress.ModifiedDate = DateTime.Now;
                            dbcontext.SaveChanges();
                        }
                        else
                        {
                            throw new InvalidOperationException("Record not found.");
                        }
                    }
                    else if (objNimboleAccountModel.objAddressModel != null && (objNimboleAccountModel.objAddressModel.CityId != 0 || objNimboleAccountModel.objAddressModel.CountryId != 0 || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.Mobile) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.Phone) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.HouseNo) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.Fax) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.HomePhone) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.StreetName) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.ZipCode) || !string.IsNullOrEmpty(objNimboleAccountModel.objAddressModel.SkypeName)))
                    {
                        #region Address
                        objNimboleAccountModel.objAddressModel.Address_Type = "A";
                        objNimboleAccountModel.objAddressModel.TenantId = objNimboleAccountModel.TenantId;
                        TblAddress objTblAddress = objNIMBOLEMapper.MapModel2Table(objNimboleAccountModel.objAddressModel);
                        dbcontext.TblAddresses.Add(objTblAddress);
                        dbcontext.SaveChanges();
                        objNimboleAccountModel.objAddressModel.Id = objTblAddress.Id;
                        #endregion

                        #region AddressAccount
                        TblTranAccAdd objTblTranAccAdd1 = new TblTranAccAdd();
                        //objTblTranAccAdd1.TenantId = objNimboleAccountModel.TenantId.ToDefaultTenantId();
                        objTblTranAccAdd1.TenantId = objNimboleAccountModel.TenantId;
                        objTblTranAccAdd1.AddressId = objTblAddress.Id;
                        objTblTranAccAdd1.AccountId = objNimboleAccountModel.Id;
                        objTblTranAccAdd1.CreatedDate = DateTime.Now;
                        objTblTranAccAdd1.ModifiedDate = DateTime.Now;
                        objTblTranAccAdd1.Status = true;
                        dbcontext.TblTranAccAdds.Add(objTblTranAccAdd1);
                        dbcontext.SaveChanges();
                        #endregion
                    }
                    dbTran.Commit();
                    return Ok(objNimboleAccountModel);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        #endregion

        #region GET

        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                //sreedhar changed on 16/10/2015
                //var strQuery = "select a.id,a.accountname,a.accountowner,count(tc.accountid) as 'NoofContacts' from tblaccount a left outer join tbltranscontact tc on a.id=tc.accountid and a.status=tc.status where a.status = 1 group by a.id,a.accountname,a.accountowner";
                //var query = dbcontext.Database.SqlQuery<AccountViewModel>(strQuery).ToList<AccountViewModel>();
                //List<AccountViewModel> lstAccountModel = new List<AccountViewModel>();
                //foreach (var item in query)
                //{  
                //    lstAccountModel.Add(new AccountViewModel()
                //    {
                //        Id = item.Id,                    
                //        AccountName = item.AccountName,
                //        AccountOwner = item.AccountOwner,
                //        NoofContacts = item.NoofContacts });
                //}
                var lstAccountModel = (from a in dbcontext.VWAccountIndexes where  a.tenantid == Tid orderby a.id descending select a).ToList();
                return Ok(lstAccountModel);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id, Guid Tid)
        {
            try
            {
                var objTblAccounts = (from a in dbcontext.TblAccounts where a.Id == id && a.TenantId == Tid select a).FirstOrDefault();
                if (objTblAccounts == null)
                    return Json("Failure");
                else
                {
                    NimboleAccountModel objNimboleAccountModel = new NimboleAccountModel();

                    objNimboleAccountModel = objNIMBOLEMapper.MapTable2Model(objTblAccounts);
                    //if (objNimboleAccountModel.ParentAccount != null)
                    //{
                    //    long parentid = Convert.ToInt64(objNimboleAccountModel.ParentAccount);
                    //    var objparentaccount = (from a in dbcontext.TblAccounts where a.Id == parentid select a.AccountName).FirstOrDefault();
                    //    if (objparentaccount != null)
                    //    {
                    //        objNimboleAccountModel.ParentAccount = objparentaccount;
                    //    }
                    //}
                    //if (objNimboleAccountModel.DistributerName != null)
                    //{
                    //    long distributerid = Convert.ToInt64(objNimboleAccountModel.DistributerName);
                    //    var objDistributerName = (from a in dbcontext.TblAccounts where a.Id == distributerid select a.AccountName).FirstOrDefault();
                    //    if (objDistributerName != null)
                    //    {
                    //        objNimboleAccountModel.DistributerName = objDistributerName;
                    //    }
                    //}
                    TblTranAccAdd objTblTranAccAdd = (from adAcc in dbcontext.TblTranAccAdds where adAcc.AccountId == id && adAcc.TenantId== Tid select adAcc).FirstOrDefault();
                    if (objTblTranAccAdd != null)
                    {
                        var objTblAddress = (from adModel in dbcontext.TblAddresses where adModel.Id == objTblTranAccAdd.AddressId && adModel.TenantId== Tid select adModel).FirstOrDefault();

                        if (objTblAddress != null)
                        {
                            objNimboleAccountModel.objAddressModel = new AddressModel();
                            objNimboleAccountModel.objAddressModel = objNIMBOLEMapper.MapTable2Model(objTblAddress);

                            var iCityId = objNimboleAccountModel.objAddressModel.CityId;
                            var iStateId = objNimboleAccountModel.objAddressModel.StateId;
                            var iCountryId = objNimboleAccountModel.objAddressModel.CountryId;

                            if (iCityId > 0)
                            {
                                CommanMethods objCommanMethods = new CommanMethods();
                                GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                                objGeneralValuesModel = objCommanMethods.GetCountryStateCity("City", objNimboleAccountModel.objAddressModel.CityId);
                                objNimboleAccountModel.objAddressModel.City = objGeneralValuesModel.Description;
                            }
                            if (iStateId > 0)
                            {
                                CommanMethods objCommanMethods = new CommanMethods();
                                GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                                objGeneralValuesModel = objCommanMethods.GetCountryStateCity("State", objNimboleAccountModel.objAddressModel.StateId);
                                objNimboleAccountModel.objAddressModel.State = objGeneralValuesModel.Description;
                                objNimboleAccountModel.objAddressModel.StateId = objGeneralValuesModel.Id;
                            }
                            if (iCountryId > 0)
                            {
                                CommanMethods objCommanMethods = new CommanMethods();
                                GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                                objGeneralValuesModel = objCommanMethods.GetCountryStateCity("Country", objNimboleAccountModel.objAddressModel.CountryId);
                                objNimboleAccountModel.objAddressModel.Country = objGeneralValuesModel.Description;
                                objNimboleAccountModel.objAddressModel.CountryId = objGeneralValuesModel.Id;
                                //objTransAccConModel.objAddressModel.Country = objGeneralValuesModel.Description;
                            }

                            //var iCityId = objNimboleAccountModel.objAddressModel.CityId;
                            //if (iCityId > 0)
                            //{
                            //    CommanMethods objCommanMethods = new CommanMethods();
                            //    GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                            //    objGeneralValuesModel = objCommanMethods.GetCountryStateCity("City", objNimboleAccountModel.objAddressModel.CityId);
                            //    objNimboleAccountModel.objAddressModel.City = objGeneralValuesModel.Description;
                            //    objGeneralValuesModel = new GeneralValuesModel();
                            //    objGeneralValuesModel = objCommanMethods.GetCountryStateCity("State", objNimboleAccountModel.objAddressModel.CityId);
                            //    objNimboleAccountModel.objAddressModel.StateId = objGeneralValuesModel.Id;
                            //    objNimboleAccountModel.objAddressModel.State = objGeneralValuesModel.Description;
                            //    objGeneralValuesModel = objCommanMethods.GetCountryStateCity("Country", objGeneralValuesModel.Id);
                            //    objNimboleAccountModel.objAddressModel.CountryId = objGeneralValuesModel.Id;
                            //    objNimboleAccountModel.objAddressModel.Country = objGeneralValuesModel.Description;
                            //}
                        }
                    }
                    return Ok(objNimboleAccountModel);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("GetByAccName")]
        public IHttpActionResult GetByAccName(string AccName, Guid Tid)
        {
            try
            {
                var objTblAccounts = (from a in dbcontext.TblAccounts where a.AccountName == AccName && a.TenantId == Tid select a).FirstOrDefault();
                if (objTblAccounts == null)
                    throw new InvalidOperationException("Record not found.");
                else
                {
                    NimboleAccountModel objNimboleAccountModel = objNIMBOLEMapper.MapTable2Model(objTblAccounts);
                    return Ok(objNimboleAccountModel);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("GetAllForExport")]
        public IHttpActionResult GetAllForExport(Guid Tid)
        {
            try
            {
                var lstAccountModel = (from na in dbcontext.VWAccountExports where na.Status == true && na.TenantId == Tid select na).ToList();
                return Ok(lstAccountModel);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("SelectAllAccount")]
        public async Task<IHttpActionResult> SelectAllAccount(Guid Tid)
        {
            try
            {
                var data = await (from x in dbcontext.TblAccounts where x.Status == true && x.TenantId == Tid orderby x.AccountName select new { Id = x.Id, Name = x.AccountName }).Distinct().ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("SelectAllDistributorNames")]
        public IHttpActionResult SelectAllDistributorNames(Guid Tid)
        {
            try
            {
                //Anji changed on 30th May 2016 from sql to Entity (EDMX)
                var data = (from x in dbcontext.TblAccounts where x.Status == true && x.TenantId == Tid orderby x.AccountName select new { Id = x.Id, Name = x.AccountName }).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("GetByAccType")]
        public IHttpActionResult GetByAccType(string AccType, Guid Tid)
        {
            try
            {
                var data = (from x in dbcontext.TblAccountTypes where x.Description == AccType && x.TenantId == Tid select x).FirstOrDefault();

                if (data == null)
                {
                    AccountTypeModel objAccountTypeModel = new AccountTypeModel();
                    objAccountTypeModel.Description = AccType;
                    objAccountTypeModel.Status = true;
                    TblAccountType objTblAccountType = objNIMBOLEMapper.MapModel2Table(objAccountTypeModel);

                    dbcontext.TblAccountTypes.Add(objTblAccountType);
                    dbcontext.SaveChanges();

                    objAccountTypeModel = objNIMBOLEMapper.MapTable2Model(objTblAccountType);
                    objAccountTypeModel.Id = objTblAccountType.Id;

                    return Ok(objAccountTypeModel.Id);
                }
                else
                {
                    return Ok(data.Id);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        //[HttpGet]
        //[Route("SelectAllDistributorNamesForCombobox")]
        //public async Task<IHttpActionResult> SelectAllDistributorNamesForCombobox(Guid Tid)
        //{
        //    try
        //    {
        //        dbcontext.Configuration.AutoDetectChangesEnabled = false;
        //        var data = await (from a in dbcontext.TblAccounts join a1 in dbcontext.TblAccounts on a.Id.ToString() equals a1.DistributerName where a.TenantId == Tid && a.Status == true orderby a.AccountName select new { Id = a.Id, Name = a.AccountName }).Distinct().ToListAsync();

        //        //var shortest = data.ToList();
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            ReasonPhrase = ex.Message
        //        });
        //    }
        //    finally
        //    {
        //        dbcontext.Configuration.AutoDetectChangesEnabled = true;
        //    }
        //}

        [HttpGet]
        [Route("GetIdByAccName")]
        public IHttpActionResult GetIdByAccName(string AccName, Guid Tid)
        {
            try
            {
                var objTblAccounts = (from a in dbcontext.TblAccounts where a.AccountName == AccName && a.TenantId == Tid select a).FirstOrDefault();
                if (objTblAccounts == null)
                {
                    NimboleAccountModel objNimboleAccountModel = new NimboleAccountModel();
                    objNimboleAccountModel.AccountName = AccName;
                    objNimboleAccountModel.Status = true;
                    TblAccount objTblAccount = objNIMBOLEMapper.MapModel2Table(objNimboleAccountModel);

                    dbcontext.TblAccounts.Add(objTblAccount);
                    dbcontext.SaveChanges();

                    objNimboleAccountModel = objNIMBOLEMapper.MapTable2Model(objTblAccount);
                    objNimboleAccountModel.Id = objTblAccount.Id;

                    return Ok(objNimboleAccountModel.Id);
                }
                else
                {
                    return Ok(objTblAccounts.Id);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        #endregion

        #region DELETE
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(string selectedId, bool status)
        {
            try
            {
                //dynamic ids = new List<string>();
                List<string> StrTempIds = new List<string>();
                string[] ids = { selectedId };
                if (!string.IsNullOrEmpty(selectedId))
                {
                    ids = selectedId.Split(',');
                }

                foreach (string tempId in ids)
                {
                    int id = Convert.ToInt32(tempId);
                    var query = dbcontext.TblAccounts.Where(x => x.Id == id).FirstOrDefault();
                    if (query != null)
                    {
                        if (status != true)
                        {
                            var leadquery = (from l in dbcontext.TblLeads where l.AccountId == id select l.AccountId).Count();
                            var transcontact = dbcontext.TblTransContacts.Where(x => x.AccountId == id).Count();

                            string strId = Convert.ToString(id);

                            var AccChild = dbcontext.TblAccounts.Where(pa => pa.ParentAccount == strId || pa.DistributerName == strId || pa.Industry == strId).Count();

                            if (leadquery == 0 && transcontact == 0 && AccChild == 0)
                            {
                                query.Status = status;
                                dbcontext.SaveChanges();
                            }
                            else
                            {
                                // throw new InvalidOperationException("Record is associated.");
                                StrTempIds.Add(tempId);
                            }
                        }
                        else
                        {
                            query.Status = status;
                            dbcontext.SaveChanges();
                        }
                        // return Ok(query);
                    }
                    else
                    {
                        // throw new InvalidOperationException("Record not found.");
                    }
                }
                if (StrTempIds.Count > 0)
                {
                    throw new InvalidOperationException("Record is associated.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        #endregion

        #region Associated
        [HttpDelete]
        [Route("DeleteRead")]
        public IHttpActionResult DeleteRead(long id, bool status)
        {
            try
            {
                var query = dbcontext.TblAccounts.Where(x => x.Id == id).FirstOrDefault();
                if (query != null)
                {
                    if (status == true)
                    {
                        var leadquery = (from l in dbcontext.TblLeads where l.AccountId == id select l.AccountId).Count();
                        var transcontact = dbcontext.TblTransContacts.Where(x => x.AccountId == id).Count();

                        string strId = Convert.ToString(id);

                        var AccChild = dbcontext.TblAccounts.Where(pa => pa.ParentAccount == strId || pa.DistributerName == strId || pa.Industry == strId).Count();

                        if (leadquery == 0 && transcontact == 0 && AccChild == 0)
                        {
                            query.Status = status;
                            dbcontext.SaveChanges();
                        }
                        else
                        {
                            throw new InvalidOperationException("Record is associated.");
                        }
                    }
                    else
                    {
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        #endregion

        #region DELETE
        [HttpDelete]
        // [ArrayInput("selectedId")]
        [Route("DeleteById")]
        public IHttpActionResult DeleteById(string selectedId)
        {
            try
            {
                string[] ids = { selectedId };
                List<string> StrTempIds = new List<string>();

                if (!string.IsNullOrEmpty(selectedId))
                {
                    ids = selectedId.Split(',');
                }

                foreach (string tempId in ids)
                {
                    int id = Convert.ToInt32(tempId);
                    var query = dbcontext.TblAccounts.Where(x => x.Id == id).FirstOrDefault();
                    if (query != null)
                    {
                        var leadquery = (from l in dbcontext.TblLeads where l.AccountId == id && l.Status == true select l.AccountId).Count();
                        var transcontact = dbcontext.TblTransContacts.Where(x => x.AccountId == id).Count();

                        string strId = Convert.ToString(id);

                        var AccChild = dbcontext.TblAccounts.Where(pa => pa.ParentAccount == strId || pa.DistributerName == strId || pa.Industry == strId).Count();

                        if (leadquery == 0 && transcontact == 0 && AccChild == 0)
                        {
                            dbcontext.TblAccounts.Remove(query);
                            dbcontext.SaveChanges();

                            var tquery = dbcontext.TblTranAccAdds.Where(t => t.AccountId == id).FirstOrDefault();
                            if (tquery != null)
                            {
                                var aquery = (from a in dbcontext.TblAddresses where tquery.AddressId == a.Id && id == tquery.AccountId select a).FirstOrDefault();
                                if (aquery != null)
                                {
                                    dbcontext.TblAddresses.Remove(aquery);
                                    dbcontext.SaveChanges();
                                }
                                dbcontext.TblTranAccAdds.Remove(tquery);
                                dbcontext.SaveChanges();
                            }
                            // return Ok(query);
                        }
                        else
                        {
                            //throw new InvalidOperationException("Record is associated.");
                            StrTempIds.Add(tempId);
                        }
                    }
                    else
                    {
                        // throw new InvalidOperationException("Record not Found.");
                    }
                }
                if (StrTempIds.Count > 0)
                {
                    throw new InvalidOperationException("Record is associated.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        //public IHttpActionResult DeleteById(long id)
        //{
        //    try
        //    {
        //        var query = dbcontext.TblAccounts.Where(x => x.Id == id).FirstOrDefault();
        //        if (query != null)
        //        {
        //            var leadquery = (from l in dbcontext.TblLeads where l.AccountId == id && l.Status == true select l.AccountId).Count();
        //            var transcontact = dbcontext.TblTransContacts.Where(x => x.AccountId == id).Count();

        //            string strId = Convert.ToString(id);

        //            var AccChild = dbcontext.TblAccounts.Where(pa => pa.ParentAccount == strId || pa.DistributerName == strId || pa.Industry == strId).Count();

        //            if (leadquery == 0 && transcontact == 0 && AccChild == 0)
        //            {
        //                dbcontext.TblAccounts.Remove(query);
        //                dbcontext.SaveChanges();

        //                var tquery = dbcontext.TblTranAccAdds.Where(t => t.AccountId == id).FirstOrDefault();
        //                if (tquery != null)
        //                {
        //                    var aquery = (from a in dbcontext.TblAddresses where tquery.AddressId == a.Id && id == tquery.AccountId select a).FirstOrDefault();
        //                    if (aquery != null)
        //                    {
        //                        dbcontext.TblAddresses.Remove(aquery);
        //                        dbcontext.SaveChanges();
        //                    }
        //                    dbcontext.TblTranAccAdds.Remove(tquery);
        //                    dbcontext.SaveChanges();
        //                }
        //                return Ok(query);
        //            }
        //            else
        //            {
        //                throw new InvalidOperationException("Record is associated.");

        //            }
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException("Record not Found.");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            ReasonPhrase = ex.Message
        //        });
        //    }
        //}

        #endregion

        #region SelectAccountType

        [HttpGet]
        [Route("SelectAllAccountType")]
        public IHttpActionResult SelectAllAccountType(Guid Tid)
        {
            try
            {
                var data = (from x in dbcontext.TblAccountTypes where x.Status == true && x.TenantId == Tid orderby x.Description select new { Id = x.Id, Name = x.Description }).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("SelectAllAccountTypesForCombobox")]
        public async Task<IHttpActionResult> SelectAllAccountTypesForCombobox(Guid Tid)
        {
            try
            {
                dbcontext.Configuration.AutoDetectChangesEnabled = false;
                var data = await (from x in dbcontext.TblAccountTypes where x.Status == true && x.TenantId == Tid orderby x.Description select new { Id = x.Id, Name = x.Description }).ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
            finally
            {
                dbcontext.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        #endregion

        #region GetAllOwnerships

        [HttpGet]
        [Route("GetAllOwnerships")]
        public IHttpActionResult GetAllOwnerships(Guid Tid)
        {
            try
            {
                var data = (from x in dbcontext.TblOwnerships where x.Status == true && x.TenantId == Tid orderby x.Description select new { Id = x.Id, Name = x.Description }).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("GetAllOwnershipsForCombobox")]
        public async Task<IHttpActionResult> GetAllOwnershipsForCombobox(Guid Tid)
        {
            try
            {
                dbcontext.Configuration.AutoDetectChangesEnabled = false;
                var data = await (from x in dbcontext.TblOwnerships where x.Status == true && x.TenantId == Tid orderby x.Description select new { Id = x.Id, Name = x.Description }).ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
            finally
            {
                dbcontext.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        #endregion

        #region SelectParentAccount
        //[HttpGet]
        //[Route("SelectAllParentAccounts")]
        //public IHttpActionResult SelectAllParentAccounts(Guid Tid)
        //{
        //    try
        //    {
        //        //var data = (from x in dbcontext.TblParentAccounts where x.Status == true select new { Id = x.Id, Description = x.Description }).ToList();
        //        var data = (from x in dbcontext.TblAccounts where x.Status == true && x.IsParentAccount == true && x.TenantId == Tid orderby x.AccountName select new { Id = x.Id, Name = x.AccountName }).ToList();
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            ReasonPhrase = ex.Message
        //        });
        //    }
        //}
        [HttpGet]
        [Route("SelectAllParentAccountsForCombobox")]
        public async Task<IHttpActionResult> SelectAllParentAccountsForCombobox(Guid Tid)
        {
            try
            {
                dbcontext.Configuration.AutoDetectChangesEnabled = false;
                var data = await (from x in dbcontext.TblAccounts where x.Status == true && x.IsParentAccount == true && x.TenantId == Tid orderby x.AccountName select new { Id = x.Id, Name = x.AccountName }).ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
            finally
            {
                dbcontext.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        #endregion

        #region SelectIndustry

        [HttpGet]
        [Route("SelectAllIndustries")]
        public IHttpActionResult SelectAllIndustries(Guid Tid)
        {
            try
            {
                var data = (from x in dbcontext.TblIndustries where x.Status == true && x.TenantId == Tid orderby x.Description select new { Id = x.Id, Name = x.Description }).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("SelectAllIndustriesForCombobox")]
        public async Task<IHttpActionResult> SelectAllIndustriesForCombobox(Guid Tid)
        {
            try
            {
                dbcontext.Configuration.AutoDetectChangesEnabled = false;
                var data = await (from x in dbcontext.TblIndustries where x.Status == true && x.TenantId == Tid orderby x.Description select new { Id = x.Id, Name = x.Description }).ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
            finally
            {
                dbcontext.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        #endregion
    }
}
