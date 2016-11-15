using AutoMapper;
using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Common;
using System.Data.Entity;
using NIMBOLE.Entities;
using System.Threading.Tasks;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Contacts")]
    public class ContactsController : ApiController
    {
        private static Object thisLock = new Object();
        DTO objNIMBOLEMapper = new DTO();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();

        #region GET
        [HttpGet]
        [Route("GetAllContact")]
        public IHttpActionResult GetAllContact(Guid Tid)
        {
            try
            {
            //    var contactList = (from x in _dbcontext.TblContacts
            //                       where x.Status == true && x.TenantId == Tid
            //                       orderby x.FirstName, x.LastName
            //                       select new { Id = x.Id, FullName = x.LastName == null || x.LastName == "" ? x.FirstName : x.FirstName + " " + x.LastName, FirstName = x.FirstName, LastName = x.LastName, ContactEmail = x.ContactEmail ?? "no email", WorkEmail = x.WorkEmail ?? "no email" }).ToList();

                // sreedhar changed on 15/10/2015

                //List<ContactViewModel> contactList = new List<ContactViewModel>();

                //foreach (var item in query)
                //{
                //    contactList.Add(new ContactViewModel() { Id = item.Id, FullName = item.FirstName + " " + item.LastName, FirstName = item.FirstName, LastName = item.LastName, ContactEmail = item.ContactEmail ?? "no email" });
                //}
                var contactList = (from x in _dbcontext.VWContactExports where x.Status == true && x.TenantId == Tid select new { Id = x.Id,FirstName = x.FirstName, LastName = x.LastName, FullName = x.FirstName +" "+ x.LastName, ContactEmail = x.ContactEmail, WorkEmail = x.WorkEmail, LeadSource = x.LeadSourceName, DepartmentName = x.DepartmentName, Designation = x.ContactRole, AccountId = x.AccountId, AccountName = x.AccountName, Address1 = x.HouseNo, Address2 = x.StreetName, CountryName = x.CountryName, StateName = x.StateName, CityName = x.CityName, ZipCode = x.ZipCode, Mobile = x.Mobile, OfficePhone = x.Phone, Fax = x.Fax, HomePhone = x.HomePhone, SkypeName = x.SkypeName, Comments = x.Comments }).ToList();
                if (contactList != null)
                    return Json(contactList);
                else
                    throw new InvalidOperationException("Record not Found.");
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
        [Route("GetAllContact1")]
        public HttpResponseMessage GetAllContact1(Guid Tid)
        {
            try
            {
                var contactList = (from x in _dbcontext.TblContacts
                                   where x.Status == true && x.TenantId == Tid
                                   orderby x.FirstName, x.LastName
                                   select new { Id = x.Id, FullName = x.LastName == null || x.LastName == "" ? x.FirstName : x.FirstName + " " + x.LastName, FirstName = x.FirstName, LastName = x.LastName, ContactEmail = x.ContactEmail ?? "no email", WorkEmail = x.WorkEmail ?? "no email" }).ToList();

                if (contactList != null)
                    return Request.CreateResponse(HttpStatusCode.OK, contactList);
                else
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        // sreedhar added on 09Dec2015
        [HttpGet]
        [Route("GetAllContactbyAccountId")]

        public async Task<IHttpActionResult> GetAllContactbyAccountId(int id, Guid Tid)
        {
            try
            {
                //var contactList = await (from x in _dbcontext.VWTrnContactAccounts
                //                   where x.Status == true && x.AccountId == id && x.TenantId == Tid
                //                   orderby x.FirstName, x.LastName
                //                   select new { Id = x.Id, FullName = x.LastName == null || x.LastName == "" ? x.FirstName : x.FirstName + " " + x.LastName, FirstName = x.FirstName, LastName = x.LastName, ContactEmail = x.ContactEmail ?? "no email", WorkEmail = x.WorkEmail ?? "no email" }).ToListAsync();

                // sreedhar changed on 15/10/2015

                //List<ContactViewModel> contactList = new List<ContactViewModel>();

                //foreach (var item in query)
                //{
                //    contactList.Add(new ContactViewModel() { Id = item.Id, FullName = item.FirstName + " " + item.LastName, FirstName = item.FirstName, LastName = item.LastName, ContactEmail = item.ContactEmail ?? "no email" });
                //}

               //  sreedhar changed on 25/6/2016

                //List<ContactViewModel> lstContactViewModel = await  (from c in _dbcontext.VWTrnContactAccounts where c.AccountId == id && c.TenantId == Tid && c.Status == true orderby c.FirstName, c.LastName select c)                    
                // .Select(m => new ContactViewModel()
                // {
                //     Id = m.Id,
                //     FirstName = m.FirstName,
                //     LastName = m.LastName,
                //     FullName = m.FirstName + " " + m.LastName,
                //     ContactEmail = m.ContactEmail,
                //     WorkEmail = m.WorkEmail
                // }).ToListAsync();

                var lstContactViewModel = await (from c in _dbcontext.VWTrnContactAccounts where c.AccountId == id && c.TenantId == Tid && c.Status == true orderby c.FirstName, c.LastName select  new 
                {
                     Id = c.Id,
                     FirstName = c.FirstName,
                     LastName = c.LastName,
                     FullName = c.FirstName + " " + c.LastName,
                     ContactEmail = c.ContactEmail,
                     WorkEmail = c.WorkEmail

                 }).ToListAsync();

                return Json(lstContactViewModel);
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
        [Route("GetAllContactExport")]
        public IHttpActionResult GetAllContactExport(Guid Tid)
        {
            try
            {
                //var query = (from x in _dbcontext.TblContacts where x.Status == true && x.TenantId == Tid select x).ToList();
                var query = (from x in _dbcontext.VWContactExports where x.Status == true && x.TenantId == Tid select new { FirstName = x.FirstName, LastName = x.LastName, ContactEmail = x.ContactEmail, WorkEmail = x.WorkEmail, LeadSource = x.LeadSourceName, DepartmentName = x.DepartmentName, Designation = x.ContactRole, AccountName = x.AccountName, Address1 = x.HouseNo, Address2 = x.StreetName, CountryName = x.CountryName, StateName = x.StateName, CityName = x.CityName, ZipCode = x.ZipCode, Mobile = x.Mobile, OfficePhone = x.Phone, Fax = x.Fax, HomePhone = x.HomePhone, SkypeName = x.SkypeName, Comments = x.Comments }).ToList();

                //FirstName = x.FirstName,LastName=item.LastName,ContactEmail=item.ContactEmail,WorkEmail=item.WorkEmail,LeadSource=item.LeadSource,Department=item.Department,Designation=item.Designation,AccountName=item.AccountName,Address1=item.Address1,Address2=item.Address2,Country=item.Country,State=item.State,City=item.City,ZipCode=item.ZipCode,Mobile=item.Mobile,OfficePhone=item.OfficePhone,Fax=item.Fax,HomePhone=item.HomePhone,SkypeName=item.SkypeName,Comments=item.Comments
                if (query != null)
                    return Json(query);
                else
                    throw new InvalidOperationException("Record not Found.");
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
        [Route("GetAll1")]
        //public IHttpActionResult GetAll(int id,int contId)
        public IHttpActionResult GetAll1(int id, Guid Tid)
        {
            try
            {
                dynamic contactList;
                //sreedhar changed on 15/10/2015
                //query = (from c in _dbcontext.TblContacts
                //         join
                //             tc in _dbcontext.TblTransContacts on c.Id equals tc.ContactId
                //         join
                //             a in _dbcontext.TblAccounts on tc.AccountId equals a.Id
                //         where tc.Status == true && a.Status == true && c.Status == true
                //         orderby c.Id                    
                //         select new
                //         {
                //             c.Id,
                //             c.FirstName,
                //             c.LastName,
                //             c.ContactEmail,
                //             a.AccountName,
                //             c.ContactImageURL
                //         }).ToList().Distinct();


                //query = (from c in _dbcontext.VWTrnContactAccounts orderby c.Id  select c).ToList().Distinct();

                //if (id != 0 && contId != 0)
                //{
                //    query = (from c in _dbcontext.TblContacts.Where(e => e.Status == true && e.Id == contId)
                //             join
                //                 tc in _dbcontext.TblTransContacts on c.Id equals tc.ContactId                            
                //             join
                //                 a in _dbcontext.TblAccounts.Where(aa => aa.Id == id) on tc.AccountId equals a.Id
                //             where tc.Status == true && a.Status == true && c.Status == true
                //             orderby c.Id                    
                //             select new
                //             {
                //                 c.Id,
                //                 c.FirstName,
                //                 c.LastName,
                //                 c.ContactEmail,
                //                 a.AccountName,
                //                 c.ContactImageURL
                //             }).ToList().Distinct();
                //}

                //if (id != 0  && contId == 0)
                //{
                //    query = (from c in _dbcontext.TblContacts.Where(e => e.Status == true)
                //             join
                //                 tc in _dbcontext.TblTransContacts on c.Id equals tc.ContactId                            
                //             join
                //                 a in _dbcontext.TblAccounts.Where(aa => aa.Id == id) on tc.AccountId equals a.Id
                //             where tc.Status == true && a.Status == true && c.Status == true
                //             orderby c.Id                    
                //             select new
                //             {
                //                 c.Id,
                //                 c.FirstName,
                //                 c.LastName,
                //                 c.ContactEmail,
                //                 a.AccountName,
                //                 c.ContactImageURL
                //             }).ToList().Distinct();
                //}

                //if (contId != 0 && id == 0)
                //{
                //    query = (from c in _dbcontext.TblContacts.Where(e => e.Status == true && e.Id == contId)
                //             join
                //                 tc in _dbcontext.TblTransContacts on c.Id equals tc.ContactId                          
                //             join
                //                 a in _dbcontext.TblAccounts on tc.AccountId equals a.Id
                //             where tc.Status == true && a.Status == true && c.Status == true
                //             orderby c.Id                    
                //             select new
                //             {
                //                 c.Id,
                //                 c.FirstName,
                //                 c.LastName,
                //                 c.ContactEmail,
                //                 a.AccountName,
                //                 c.ContactImageURL
                //             }).ToList().Distinct();
                //}


                // List<ContactViewModel> contactList = new List<ContactViewModel>();

                //foreach (var item in query)
                //{
                //    contactList.Add(new ContactViewModel()
                //    {
                //        Id = item.Id,
                //        FullName = item.FirstName + " " + item.LastName,
                //        FirstName = item.FirstName,
                //        LastName = item.LastName,
                //        ContactEmail = item.ContactEmail ?? "no email",
                //        AccountName = item.AccountName,
                //        ContactImageURL = item.ContactImageURL
                //    });
                //}

                if (id != 0)
                {
                    contactList = (from c in _dbcontext.VWTrnContactAccounts
                                   orderby c.Id
                                   where c.AccountId == id && c.TenantId == Tid
                                   select new
                                   {
                                       Id = c.Id,
                                       FullName = c.LastName == null || c.LastName == "" ? c.FirstName : c.FirstName + " " + c.LastName,
                                       FirstName = c.FirstName,
                                       LastName = c.LastName,
                                       ContactEmail = c.ContactEmail ?? "no email",
                                       WorkEmail = c.WorkEmail ?? "no email",
                                       ContactImageURL = c.ContactImageURL,
                                       AccountName = c.AccountName,
                                       Status = c.Status
                                   }
                               ).ToList();//.Distinct();
                }
                else
                    contactList = (from c in _dbcontext.VWTrnContactAccounts
                                   orderby c.Id
                                   where c.TenantId == Tid
                                   select new
                                   {
                                       Id = c.Id,
                                       FullName = c.LastName == null || c.LastName == "" ? c.FirstName : c.FirstName + " " + c.LastName,
                                       FirstName = c.FirstName,
                                       LastName = c.LastName,
                                       ContactEmail = c.ContactEmail ?? "no email",
                                       WorkEmail = c.WorkEmail ?? "no email",
                                       ContactImageURL = c.ContactImageURL,
                                       AccountName = c.AccountName,
                                       Status = c.Status
                                   }).ToList();//.Distinct();
                if (contactList != null)
                    return Json(contactList);
                else
                    throw new InvalidOperationException("Record not Found.");
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
        [Route("GetAll")]
        //public IHttpActionResult GetAll(int id,int contId)
        public HttpResponseMessage GetAll(int id, Guid Tid)
        {
            try
            {
                if (id != 0)
                {
                    var contactList = (from c in _dbcontext.VWTrnContactAccounts
                                       orderby  c.Id descending
                                       where c.AccountId == id && c.TenantId == Tid
                                       select new
                                       {
                                           Id = c.Id,
                                           FullName = c.LastName == null || c.LastName == "" ? c.FirstName : c.FirstName + " " + c.LastName,
                                           FirstName = c.FirstName,
                                           LastName = c.LastName,
                                           ContactEmail = c.ContactEmail ?? "no email",
                                           WorkEmail = c.WorkEmail ?? "no email",
                                           ContactImageURL = c.ContactImageURL,
                                           AccountName = c.AccountName,
                                           Status = c.Status
                                       }
                               ).OrderByDescending(c => c.Id).ToList();//.Distinct();
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found.");
                }
                else
                {
                    var contactList = (from c in _dbcontext.VWTrnContactAccounts
                                       orderby c.Id
                                       where c.TenantId == Tid
                                       select new
                                       {
                                           Id = c.Id,
                                           FullName = c.LastName == null || c.LastName == "" ? c.FirstName : c.FirstName + " " + c.LastName,
                                           FirstName = c.FirstName,
                                           LastName = c.LastName,
                                           ContactEmail = c.ContactEmail ?? "no email",
                                           WorkEmail = c.WorkEmail ?? "no email",
                                           ContactImageURL = c.ContactImageURL,
                                           AccountName = c.AccountName,
                                           Status = c.Status
                                       }).OrderByDescending(c => c.Id).ToList();//.Distinct();
                        return Request.CreateResponse(HttpStatusCode.OK, contactList);
                   
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id, Guid Tid)
        {
            try
            {
                //var objTblContact = (from c in _dbcontext.TblContacts where c.Id == id && c.TenantId == Tid select c).FirstOrDefault();
                var objTblContact = _dbcontext.TblContacts.FirstOrDefault(c => c.Id == id && c.TenantId == Tid);
                if (objTblContact != null)
                {
                    TransAccConModel objTransAccConModel = new TransAccConModel();
                    #region Contact
                    objTransAccConModel.objContactModel = new ContactModel();
                    objTransAccConModel.objContactModel = objNIMBOLEMapper.MapTable2Model(objTblContact);
                    #endregion

                    #region AddressContacts & Address
                    //TblAddressContact objTblAddressContacts = (from adCont in _dbcontext.TblAddressContacts where adCont.ContactId == id select adCont).FirstOrDefault();
                    TblAddressContact objTblAddressContacts = _dbcontext.TblAddressContacts.FirstOrDefault(adCont => adCont.ContactId == id);


                    AddressContactModel objAddressContactModel = new AddressContactModel();
                    TblAddress objTblAddress = new TblAddress();
                    if (objTblAddressContacts != null)
                    {
                        //objTblAddress = (from adModel in _dbcontext.TblAddresses where adModel.Id == objTblAddressContacts.AddressId select adModel).FirstOrDefault();
                        objTblAddress = _dbcontext.TblAddresses.FirstOrDefault(adModel => adModel.Id == objTblAddressContacts.AddressId);
                        objAddressContactModel.Id = objTblAddressContacts.Id;
                        objAddressContactModel.AddressId = objTblAddress.Id;
                        objAddressContactModel.ContactId = objTblContact.Id;
                        objAddressContactModel.TenantId = objTblContact.TenantId.ToDefaultTenantId();

                        objTransAccConModel.objAddressModel = new AddressModel();
                        objTransAccConModel.objAddressModel = objNIMBOLEMapper.MapTable2Model(objTblAddress);
                    }
                    #endregion

                    #region TransContact
                    TblTransContact objTblTransContact = new TblTransContact();
                    //objTblTransContact = (from tc in _dbcontext.TblTransContacts where tc.ContactId == id select tc).FirstOrDefault();// ) _dbcontext.TblTransContacts.Where(tc => tc.ContactId == id).FirstOrDefault();
                    objTblTransContact = _dbcontext.TblTransContacts.FirstOrDefault(tc => tc.ContactId == id);

                    objTransAccConModel.objTransContactModel = new TransContactModel();
                    if (objTblTransContact != null)
                    {
                        objTransAccConModel.objTransContactModel.AccountId = Convert.ToInt64(objTblTransContact.AccountId);
                        objTransAccConModel.objTransContactModel.ContactRoleId = Convert.ToInt64(objTblTransContact.ContactRoleId);
                        objTransAccConModel.objTransContactModel = objNIMBOLEMapper.MapTable2Model(objTblTransContact);
                    }
                    #endregion

                    var iCityId = objTransAccConModel.objAddressModel.CityId;
                    var iStateId = objTransAccConModel.objAddressModel.StateId;
                    var iCountryId = objTransAccConModel.objAddressModel.CountryId;

                    //var _objTblContacts = _dbcontext.TblContacts.Where(c => c.Id == id).FirstOrDefault();
                    var _objTblContacts = _dbcontext.TblContacts.FirstOrDefault(c => c.Id == id);
                    objTransAccConModel.objContactModel = objNIMBOLEMapper.MapTable2Model(_objTblContacts);

                    //var _accountId = _dbcontext.TblTransContacts.Where(tc => tc.ContactId == id).FirstOrDefault().AccountId;
                    var _accountId = _dbcontext.TblTransContacts.FirstOrDefault(tc => tc.ContactId == id).AccountId;
                    TblAccount objTblAccount = _dbcontext.TblAccounts.FirstOrDefault(acc => acc.Id == _accountId);

                    var _NimboleAccount = objNIMBOLEMapper.MapTable2Model(objTblAccount);
                    objTransAccConModel.objAccountModel = objNIMBOLEMapper.MapNimbole2Account(_NimboleAccount);
                    objTransAccConModel.objTransContactModel.AccountId = objTransAccConModel.objAccountModel.Id;

                    if (iCityId > 0)
                    {
                        CommanMethods objCommanMethods = new CommanMethods();
                        GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                        objGeneralValuesModel = objCommanMethods.GetCountryStateCity("City", objTransAccConModel.objAddressModel.CityId);
                        objTransAccConModel.objAddressModel.City = objGeneralValuesModel.Description;
                        objTransAccConModel.objAddressModel.CityId = objGeneralValuesModel.Id;
                    }
                    if (iStateId > 0)
                    {
                        CommanMethods objCommanMethods = new CommanMethods();
                        GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                        objGeneralValuesModel = objCommanMethods.GetCountryStateCity("State", objTransAccConModel.objAddressModel.StateId);
                        objTransAccConModel.objAddressModel.State = objGeneralValuesModel.Description;
                        objTransAccConModel.objAddressModel.StateId = objGeneralValuesModel.Id;
                    }
                    if (iCountryId > 0)
                    {
                        CommanMethods objCommanMethods = new CommanMethods();
                        GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                        objGeneralValuesModel = objCommanMethods.GetCountryStateCity("Country", objTransAccConModel.objAddressModel.CountryId);
                        objTransAccConModel.objAddressModel.Country = objGeneralValuesModel.Description;
                        objTransAccConModel.objAddressModel.CountryId = objGeneralValuesModel.Id;
                        //objTransAccConModel.objAddressModel.Country = objGeneralValuesModel.Description;
                    }
                    return Ok(objTransAccConModel);
                }
                else
                    throw new InvalidOperationException("Record not Found.");
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
        [Route("GetByEmail")]
        public IHttpActionResult GetByEmail(string Email, Guid Tid)
        {
            try
            {
                // var objTblContact = (from c in _dbcontext.TblContacts where c.ContactEmail == Email && c.TenantId==Tid select c).FirstOrDefault();
                //var objTblContact = (from c in _dbcontext.TblContacts where c.WorkEmail == Email && c.TenantId == Tid select c).FirstOrDefault();
                var objTblContact = _dbcontext.TblContacts.FirstOrDefault(c => c.WorkEmail == Email && c.TenantId == Tid);
                if (objTblContact != null)
                {
                    ContactModel objContactModel = new ContactModel();
                    #region Contact
                    objContactModel = objNIMBOLEMapper.MapTable2Model(objTblContact);
                    #endregion
                    if (objContactModel != null)
                        return Ok(objContactModel);
                    else
                        throw new InvalidOperationException("Record not Found.");
                }
                return null;
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
        [Route("GetTranContactById")]
        public IHttpActionResult GetTranContactById(int id)
        {
            try
            {
                var lstTblContacts = (from cont in _dbcontext.TblContacts where cont.Id == id && cont.Status == true select cont).ToList();
                var lstTransContactAccount = (from tca in _dbcontext.TblTransContacts where tca.ContactId == id && tca.Status == true select tca).ToList();
                if (lstTransContactAccount == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                    return Ok(lstTransContactAccount);
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

        #region POST
        [HttpPost]
        [Route("InsertImage")]
        public IHttpActionResult InsertImage([FromBody] TransAccConModel objTransAccConModel)
        {
            try
            {
                int id = Convert.ToInt32(objTransAccConModel.objContactModel.Id);

                var query = (from c in _dbcontext.TblContacts where c.Id == id && c.TenantId==objTransAccConModel.objContactModel.TenantId select c).FirstOrDefault();
                if (query != null)
                {
                    query.ContactImageURL = objTransAccConModel.objContactModel.ContactImageURL;
                    _dbcontext.SaveChanges();
                }
                return Ok(objTransAccConModel);
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
        [Route("Insert")]
        [ModelValidator]
        public IHttpActionResult Post([FromBody] TransAccConModel objTransAccConModel)
        {
            System.Data.Entity.DbContextTransaction dbTran = _dbcontext.Database.BeginTransaction();
            try
            {
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                using (dbTran)
                {
                    lock (thisLock)
                    {
                        TblContact objTblContact = objNIMBOLEMapper.MapModel2Table(objTransAccConModel.objContactModel);
                        if (!_dbcontext.TblContacts.Any(u => u.WorkEmail == objTransAccConModel.objContactModel.WorkEmail && u.TenantId == objTransAccConModel.objContactModel.TenantId))
                        {
                            _dbcontext.TblContacts.Add(objTblContact);
                            _dbcontext.SaveChanges();
                            objTransAccConModel.objContactModel.Id = objTblContact.Id;
                        }
                        else
                        {
                            throw new InvalidOperationException("Record already exist/inactive.");
                        }

                        if (objTransAccConModel.objAddressModel != null && (objTransAccConModel.objAddressModel.CityId != 0 || objTransAccConModel.objAddressModel.CountryId != 0 || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.Mobile) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.Phone) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.HouseNo) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.Fax) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.HomePhone) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.StreetName) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.ZipCode) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.SkypeName)))
                        {

                            objTransAccConModel.objAddressModel.Address_Type = "C";
                            objTransAccConModel.objAddressModel.TenantId = objTransAccConModel.objContactModel.TenantId;
                            TblAddress objTblAddress = objNIMBOLEMapper.MapModel2Table(objTransAccConModel.objAddressModel);
                            _dbcontext.TblAddresses.Add(objTblAddress);
                            _dbcontext.SaveChanges();

                            TblAddressContact objTblAddressContact = new TblAddressContact();
                            objTransAccConModel.objAddressModel.Id = objTblAddress.Id;
                            objTransAccConModel.objContactModel.Id = objTblContact.Id;
                            objTblAddressContact.TenantId = objTransAccConModel.objContactModel.TenantId;

                            objTblAddressContact = objNIMBOLEMapper.MapTable2Model(objTransAccConModel);
                            _dbcontext.TblAddressContacts.Add(objTblAddressContact);
                            _dbcontext.SaveChanges();

                            AddressContactModel objAddressContactModel = new AddressContactModel();
                            objAddressContactModel.TenantId = objTransAccConModel.objContactModel.TenantId;
                            objAddressContactModel = objNIMBOLEMapper.MapTable2Model(objTblAddressContact);

                        }
                        objTransAccConModel.objTransContactModel.ContactId = objTblContact.Id;
                        objTransAccConModel.objTransContactModel.TenantId = objTblContact.TenantId;
                        objTransAccConModel.objTransContactModel.ContactRoleId = objTransAccConModel.objTransContactModel.ContactRoleId;

                        TblTransContact objTblTransContact = objNIMBOLEMapper.MapModel2Table(objTransAccConModel.objTransContactModel);
                        _dbcontext.TblTransContacts.Add(objTblTransContact);
                        _dbcontext.SaveChanges();

                        //objTblAddressContact = objNIMBOLEMapper.MapTable2Model(objTransAccConModel);
                        //_dbcontext.TblAddressContacts.Add(objTblAddressContact);
                        //_dbcontext.SaveChanges();
                        //AddressContactModel objAddressContactModel = new AddressContactModel();
                        //objAddressContactModel = objNIMBOLEMapper.MapTable2Model(objTblAddressContact);

                        dbTran.Commit();
                        return Ok(objTransAccConModel.objTransContactModel);
                    }
                }//Close Using Block
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
        [HttpPost]
        [Route("LeadInsert")]
        [ModelValidator]
        public IHttpActionResult PostLead([FromBody] TransAccConModel objTransAccConModel)
        {
            HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
            #region If ModelState is not Valid
            if (response.StatusCode != HttpStatusCode.OK)
                return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
            #endregion

            using (System.Data.Entity.DbContextTransaction dbTran = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    lock (thisLock)
                    {
                        TblContact objTblContact = objNIMBOLEMapper.MapModel2Table(objTransAccConModel.objContactModel);

                        if (!_dbcontext.TblContacts.Any(u => u.WorkEmail == objTransAccConModel.objContactModel.WorkEmail && u.TenantId==objTransAccConModel.objContactModel.TenantId))
                        {
                            _dbcontext.TblContacts.Add(objTblContact);
                            _dbcontext.SaveChanges();
                        }
                        else
                        {
                            throw new InvalidOperationException("Record already exist.");
                        }
                        objTransAccConModel.objTransContactModel.ContactId = objTblContact.Id;
                        objTransAccConModel.objTransContactModel.TenantId = objTblContact.TenantId;
                        objTransAccConModel.objContactModel.WorkEmail = objTblContact.WorkEmail;
                        
                        TblTransContact objTblTransContact = objNIMBOLEMapper.MapModel2Table(objTransAccConModel.objTransContactModel);
                        _dbcontext.TblTransContacts.Add(objTblTransContact);
                        _dbcontext.SaveChanges();

                        dbTran.Commit();

                        // added by sreedhar for lead contact combo bind on 19/08/2015


                        //List<TblContact> lstTblContact = _dbcontext.TblContacts.Where(cr => cr.Status == true).ToList();
                        //var contact = (from c in _dbcontext.TblContacts where c.Status == true select c).ToList();
                        //List<ContactViewModel> lstContactViewModel = new List<ContactViewModel>();
                        //foreach (var item in contact)
                        //{
                        //    lstContactViewModel.Add(new ContactViewModel() { Id = item.Id, FirstName = item.FirstName, LastName = item.LastName, FullName = item.FirstName + " " + item.LastName, ContactEmail = item.ContactEmail, WorkEmail = item.WorkEmail });
                        //}
                        //return Ok(lstContactViewModel);

                  //      List<ContactViewModel> lstContactViewModel = (from c in _dbcontext.VWTrnContactAccounts where c.Status==true select c)
                  //.Select(m => new ContactViewModel()
                  //{
                  //   Id = m.Id, FirstName = m.FirstName, LastName = m.LastName, FullName = m.FirstName + " " + m.LastName, ContactEmail = m.ContactEmail, WorkEmail = m.WorkEmail 
                  //}).ToList();

                        var lstContactViewModel = (from c in _dbcontext.VWTrnContactAccounts
                                                   where c.Status == true && c.TenantId==objTransAccConModel.objContactModel.TenantId
                                                   select new
                                                   {
                                                       Id = c.Id,
                                                       FirstName = c.FirstName,
                                                       LastName = c.LastName,
                                                       FullName = c.FirstName + " " + c.LastName,
                                                       ContactEmail = c.ContactEmail,
                                                       WorkEmail = c.WorkEmail
                                                   }).ToList();

                        return Ok(lstContactViewModel);

                        //List<ContactViewModel> lstContactViewModel = (from x in _dbcontext.VWTrnContactAccounts where x.AccountId == objTransAccConModel.objTransContactModel.AccountId && x.TenantId == objTransAccConModel.objTransContactModel.TenantId select new { Id = x.Id, FirstName = x.FirstName, LastName = x.LastName, FullName = x.FirstName + " " + x.LastName, ContactEmail = x.ContactEmail, WorkEmail = x.WorkEmail }).ToList();

                    }
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    //Rollback transaction if exception occurs
                    dbTran.Rollback();
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        ReasonPhrase = ex.InnerException.InnerException.Message
                    });
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        ReasonPhrase = ex.Message
                    });
                }
            }//Close Using Block

        }
        #endregion

        #region PUT
        [HttpPut]
        [Route("Edit")]
        public IHttpActionResult Edit([FromBody] TransAccConModel objTransAccConModel)
        {
            System.Data.Entity.DbContextTransaction dbTran = _dbcontext.Database.BeginTransaction();
            try
            {
                using (dbTran)
                {
                    var objContacts = (from x in _dbcontext.TblContacts where x.Id == objTransAccConModel.objContactModel.Id && x.TenantId== objTransAccConModel.objContactModel.TenantId select x).FirstOrDefault();
                    if (objContacts != null)
                    {
                        if (!_dbcontext.TblContacts.Any(u => u.WorkEmail == objTransAccConModel.objContactModel.WorkEmail && u.TenantId == objTransAccConModel.objContactModel.TenantId))
                        {
                            objContacts.WorkEmail = objTransAccConModel.objContactModel.WorkEmail;
                        }

                        objContacts.FirstName = objTransAccConModel.objContactModel.FirstName;
                        objContacts.LastName = objTransAccConModel.objContactModel.LastName;
                        objContacts.ContactEmail = objTransAccConModel.objContactModel.ContactEmail;
                        objContacts.DepartmentId = objTransAccConModel.objContactModel.DepartmentId;
                        objContacts.LeadSourceId = objTransAccConModel.objContactModel.LeadSourceId;
                        objContacts.Comments = objTransAccConModel.objContactModel.Comments;
                        objContacts.Status = objTransAccConModel.objContactModel.Status;

                        if (!string.IsNullOrEmpty(objTransAccConModel.objContactModel.ContactImageURL))
                            objContacts.ContactImageURL = objTransAccConModel.objContactModel.ContactImageURL;
                        objContacts.ModifiedDate = DateTime.Now;
                        _dbcontext.SaveChanges();

                        var query = _dbcontext.TblTransContacts.Where(ac => ac.ContactId == objContacts.Id && ac.TenantId == objTransAccConModel.objContactModel.TenantId).FirstOrDefault();
                        if (query != null)
                        {
                            //query.AccountId = objTransAccConModel.objTransContactModel.AccountId;
                            query.ContactRoleId = objTransAccConModel.objTransContactModel.ContactRoleId;
                            _dbcontext.SaveChanges();
                        }

                        TblAddressContact objTblAddressContact = (from cAddr in _dbcontext.TblAddressContacts where cAddr.ContactId == objTransAccConModel.objContactModel.Id && cAddr.AddressId == objTransAccConModel.objAddressModel.Id && cAddr.TenantId == objTransAccConModel.objContactModel.TenantId select cAddr).FirstOrDefault();
                        if (objTblAddressContact != null)
                        {
                            var objTblAddress = (from a in _dbcontext.TblAddresses where a.Id == objTransAccConModel.objAddressModel.Id && a.TenantId == objTransAccConModel.objContactModel.TenantId select a).FirstOrDefault();
                            if (objTblAddress != null && (objTransAccConModel.objAddressModel.CityId != 0 || objTransAccConModel.objAddressModel.CountryId != 0 || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.Mobile) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.Phone) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.HouseNo) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.Fax) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.HomePhone) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.StreetName) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.ZipCode) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.SkypeName)))
                            {
                                objTblAddress.StreetName = objTransAccConModel.objAddressModel.StreetName;
                                objTblAddress.HouseNo = objTransAccConModel.objAddressModel.HouseNo;
                                objTblAddress.CountryId = objTransAccConModel.objAddressModel.CountryId;
                                objTblAddress.StateId = objTransAccConModel.objAddressModel.StateId;
                                objTblAddress.CityId = objTransAccConModel.objAddressModel.CityId;
                                objTblAddress.ZipCode = objTransAccConModel.objAddressModel.ZipCode;
                                objTblAddress.Phone = objTransAccConModel.objAddressModel.Phone;
                                objTblAddress.Mobile = objTransAccConModel.objAddressModel.Mobile;
                                objTblAddress.HomePhone = objTransAccConModel.objAddressModel.HomePhone;
                                objTblAddress.Fax = objTransAccConModel.objAddressModel.Fax;
                                objTblAddress.SkypeName = objTransAccConModel.objAddressModel.SkypeName;
                                objTblAddress.Address_Type = "C";
                                objTblAddress.ModifiedDate = DateTime.Now;
                                _dbcontext.SaveChanges();
                            }
                        }
                        else if (objTransAccConModel.objAddressModel != null && (objTransAccConModel.objAddressModel.CityId != 0 || objTransAccConModel.objAddressModel.CountryId != 0 || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.Mobile) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.Phone) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.HouseNo) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.Fax) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.HomePhone) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.StreetName) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.ZipCode) || !string.IsNullOrEmpty(objTransAccConModel.objAddressModel.SkypeName)))
                        {
                            #region Address
                            objTransAccConModel.objAddressModel.Address_Type = "C";
                            //objTransAccConModel.objAddressModel.TenantId = objTransAccConModel.objContactModel.TenantId;
                            objTransAccConModel.objAddressModel.TenantId = objTransAccConModel.objContactModel.TenantId;
                            TblAddress objTblAddress = objNIMBOLEMapper.MapModel2Table(objTransAccConModel.objAddressModel);
                            _dbcontext.TblAddresses.Add(objTblAddress);
                            _dbcontext.SaveChanges();
                            objTransAccConModel.objAddressModel.Id = objTblAddress.Id;

                            #endregion

                            #region AddressEmployee

                            TblAddressContact objTblAddressContact1 = new TblAddressContact();

                            objTblAddressContact1.TenantId = objTransAccConModel.objContactModel.TenantId;
                            //objTransAccConModel.objAddressModel.TenantId.ToDefaultTenantId();
                            objTblAddressContact1.AddressId = objTblAddress.Id;
                            objTblAddressContact1.ContactId = objTransAccConModel.objContactModel.Id;

                            _dbcontext.TblAddressContacts.Add(objTblAddressContact1);
                            _dbcontext.SaveChanges();
                            #endregion
                        }
                        //var _stateId = _dbcontext.TblCities.Where(c => c.Id == objTransAccConModel.objAddressModel.CityId).Select(c => c.StateId).FirstOrDefault();
                        //var _countryId = _dbcontext.TblStates.Where(s => s.StateId == _stateId).Select(s => s.CountryId ?? 0).FirstOrDefault();
                        //objTransAccConModel.objAddressModel.Country = _dbcontext.TblCountries.Where(c => c.CountryId == _countryId).Select(c => c.CountryName).FirstOrDefault();
                        dbTran.Commit();
                        return Ok(objTransAccConModel);
                    }
                    else
                    {
                        throw new InvalidOperationException("Record not found.");
                    }
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

        [HttpPut]
        [Route("EditById")]
        public IHttpActionResult EditById([FromBody]  ContactModel objContactModel, Guid Tid)
        {
            try
            {
                var record = (from x in _dbcontext.TblContacts where x.Id == objContactModel.Id && x.TenantId == Tid select x).FirstOrDefault();
                if (record == null)
                    throw new InvalidOperationException("Record not found.");

                record.ContactEmail = objContactModel.ContactEmail;
                record.ModifiedDate = DateTime.Now;
                _dbcontext.SaveChanges();

                objContactModel = objNIMBOLEMapper.MapTable2Model(record);

                //List<ContactViewModel> lstContactViewModel = new List<ContactViewModel>();
                //var contact = (from c in _dbcontext.TblContacts where c.Status == true && c.TenantId == Tid select c).ToList();

                //foreach (var item in contact)
                //{
                //    lstContactViewModel.Add(new ContactViewModel() { Id = item.Id, FirstName = item.FirstName, LastName = item.LastName, FullName = item.FirstName + " " + item.LastName, ContactEmail = item.ContactEmail });
                //}
                //return Ok(lstContactViewModel);

                //List<ContactViewModel> lstContactViewModel = (from c in _dbcontext.TblContacts where c.Status == true && c.TenantId == Tid select c)
                //.Select(m => new ContactViewModel()
                //{
                //    Id = m.Id,
                //    FirstName = m.FirstName,
                //    LastName = m.LastName,
                //    FullName = m.FirstName + " " + m.LastName,
                //    ContactEmail = m.ContactEmail,
                //    WorkEmail = m.WorkEmail
                //}).ToList();

               var lstContactViewModel = (from c in _dbcontext.TblContacts where c.Status == true && c.TenantId == Tid select new 
              {
                  Id = c.Id,
                  FirstName = c.FirstName,
                  LastName = c.LastName,
                  FullName = c.FirstName + " " + c.LastName,
                  ContactEmail = c.ContactEmail,
                  WorkEmail = c.WorkEmail
              }).ToList();

                return Ok(lstContactViewModel);
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
                List<string> StrTempIds = new List<string>();
                string[] ids = { selectedId };
                if (!string.IsNullOrEmpty(selectedId))
                {
                    ids = selectedId.Split(',');
                }

                foreach (string tempId in ids)
                {
                    int id = Convert.ToInt32(tempId);

                var query = _dbcontext.TblContacts.Where(x => x.Id == id).FirstOrDefault();
                if (query != null)
                {
                    if (status != true)
                    {
                        var Tcquery = _dbcontext.TblTranLeadContacts.Where(x => x.ContactId == id).Count();
                        if (Tcquery == 0)
                        {
                            query.Status = status;
                            _dbcontext.SaveChanges();
                        }
                        else
                        {
                          //  throw new InvalidOperationException("Record is associated.");
                            StrTempIds.Add(tempId);
                        }
                    }
                    else
                    {
                        query.Status = status;
                        _dbcontext.SaveChanges();
                    }
                }
                else
                {
                  //  throw new InvalidOperationException("Record not found.");
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
                var query = _dbcontext.TblContacts.Where(x => x.Id == id).FirstOrDefault();
                if (query != null)
                {
                    if (status == true)
                    {
                        var Tcquery = _dbcontext.TblTranLeadContacts.Where(x => x.ContactId == id).Count();
                        if (Tcquery == 0)
                        {
                            query.Status = status;
                            _dbcontext.SaveChanges();
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

        #region ContactDelete
        [HttpDelete]
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

                var query = _dbcontext.TblContacts.Where(c => c.Id == id).FirstOrDefault();
                if (query != null)
                {
                    var associateContact = _dbcontext.TblTranLeadContacts.Where(e => e.ContactId == id).Count();
                    if (associateContact == 0)
                    {
                        _dbcontext.TblContacts.Remove(query);
                        _dbcontext.SaveChanges();

                        var addresscontact = (from a in _dbcontext.TblAddressContacts where a.ContactId == id select a).FirstOrDefault();
                        if (addresscontact != null)
                        {
                            var addressQuery = (from q in _dbcontext.TblAddresses where q.Id == addresscontact.AddressId select q).FirstOrDefault();
                            _dbcontext.TblAddresses.Remove(addressQuery);
                            _dbcontext.TblAddressContacts.Remove(addresscontact);
                            _dbcontext.SaveChanges();
                        }
                        var TransContact = (from t in _dbcontext.TblTransContacts where t.ContactId == id select t).FirstOrDefault();
                        if (TransContact != null)
                        {
                            _dbcontext.TblTransContacts.Remove(TransContact);
                            _dbcontext.SaveChanges();
                        }

                        var TransLeadContact = (from t in _dbcontext.TblTranLeadContacts where t.ContactId == id select t).ToList();
                        if (TransLeadContact != null)
                        {
                            foreach (var item in TransLeadContact)
                            {
                                var temp = (from t in _dbcontext.TblTranLeadContacts where t.ContactId == item.ContactId select t).FirstOrDefault();
                                _dbcontext.TblTranLeadContacts.Remove(temp);
                                _dbcontext.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                      //  throw new InvalidOperationException("Record is associated.");
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
        #endregion
    }
}
