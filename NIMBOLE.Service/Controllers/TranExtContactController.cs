using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;
using NIMBOLE.Models;
using AutoMapper;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Service.Controllers;
using NIMBOLE.Models.Models.Transactions;
using NIMBOLE.Entities;
using NIMBOLE.Common;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/TranExtContact")]
    public class TranExtContactController : ApiController
    {
        private static Object thisLock = new Object();
        DTO objNIMBOLEMapper = new DTO();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();

        #region GET

        [HttpGet]
        [Route("GetByLeadId")]
        public async Task<IHttpActionResult> GetByLeadId(int iLeadId,Guid Tid)
        {
            try
            {
                var lstTblTranExtContacts = await (from c in _dbcontext.TblTranLeadContacts where c.LeadId == iLeadId && c.TenantId==Tid select c).ToListAsync();
                if (lstTblTranExtContacts != null && lstTblTranExtContacts.Count > 0)
                {
                    ExtContactModel objExtContactModel = null;
                    List<ExtContactModel> lstExtContactModel = new List<ExtContactModel>();
                    foreach (var objTblTranExtContact in lstTblTranExtContacts)
                    {
                        objExtContactModel = new ExtContactModel();
                        objExtContactModel.ExtContactRoleId = objTblTranExtContact.ContactRoleId ?? 0;
                        string strExtContactRoleName = (from rn in _dbcontext.TblContactRoles where rn.Id == objTblTranExtContact.ContactRoleId && rn.TenantId==Tid select rn.Description).FirstOrDefault();
                        objExtContactModel.ExtContactRole = strExtContactRoleName;
                        var strExtContactFullName = (from rn in _dbcontext.TblContacts where rn.TenantId==Tid && rn.Id == (objTblTranExtContact.ContactId ?? 0) select new { FullName = rn.FirstName + " " + rn.LastName, rn.ContactEmail, rn.WorkEmail }).FirstOrDefault();
                        objExtContactModel.FullName = strExtContactFullName.FullName;
                        objExtContactModel.ContactEmail = strExtContactFullName.ContactEmail;
                        objExtContactModel.WorkEmail = strExtContactFullName.WorkEmail; // sreedhar added on 04-jan-2016
                        objExtContactModel.LeadId = iLeadId;
                        objExtContactModel.TenantId = objTblTranExtContact.TenantId;
                        objExtContactModel.Id = objTblTranExtContact.Id;
                        lstExtContactModel.Add(objExtContactModel);
                    }
                    return Ok(lstExtContactModel);
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
        #endregion

        #region POST
        [HttpPost]
        [Route("Insert")]
        [ModelValidator]
        public IHttpActionResult Post([FromBody] ExtContactModel objExtContactModel)
        {
            try
            {
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                try
                {
                    TranLeadContactModel objTranExtContactModel = new TranLeadContactModel();
                    TblTranLeadContact objTblTranExtContact = new TblTranLeadContact();
                    TblContact objTblContact = new TblContact();
                    //long ContactId = Convert.ToInt64(objExtContactModel.FullName);
                    long ContactId = objExtContactModel.ExtContactId;
                    //save contact
                    objTblContact = (from con in _dbcontext.TblContacts where con.Id == ContactId && con.TenantId==objExtContactModel.TenantId select con).FirstOrDefault();
                    _dbcontext.SaveChanges();
                    //Save Contacts and lead details
                    objTblTranExtContact.ContactId = ContactId;
                    objTblTranExtContact.TenantId = objExtContactModel.TenantId;
                    objTblTranExtContact.ContactRoleId = objExtContactModel.ExtContactRoleId;
                    objTblTranExtContact.LeadId = objExtContactModel.LeadId;
                    //objTblTranExtContact.TenantId = objExtContactModel.TenantId.ToDefaultTenantId();
                    _dbcontext.TblTranLeadContacts.Add(objTblTranExtContact);
                    _dbcontext.SaveChanges();

                    objTranExtContactModel = objNIMBOLEMapper.MapTable2Model(objTblTranExtContact);
                    objExtContactModel.Id = objTblTranExtContact.Id;
                    objExtContactModel.LeadId = Convert.ToInt64(objTblTranExtContact.LeadId);
                    objExtContactModel.TenantId = objTblTranExtContact.TenantId;
                    objExtContactModel.ExtContactRoleId = Convert.ToInt64(objTblTranExtContact.ContactRoleId);
                    var strRole = (from ec in _dbcontext.TblContactRoles where ec.Id == objExtContactModel.ExtContactRoleId && ec.TenantId== objExtContactModel.TenantId select ec.Description).FirstOrDefault();
                    objExtContactModel.ExtContactRole = strRole;
                    var strFullName = (from ec in _dbcontext.TblContacts where ec.Id == ContactId && ec.TenantId == objExtContactModel.TenantId select ec.FirstName + "" + ec.LastName).FirstOrDefault();
                    var strContactEmail = (from ec in _dbcontext.TblContacts where ec.Id == ContactId && ec.TenantId == objExtContactModel.TenantId select ec.ContactEmail).FirstOrDefault();
                    var strWorkEmail = (from ec in _dbcontext.TblContacts where ec.Id == ContactId && ec.TenantId== objExtContactModel.TenantId select ec.WorkEmail).FirstOrDefault(); //sreedhar added on 04-jan-2016

                    objExtContactModel.FullName = strFullName;
                    objExtContactModel.ContactEmail = strContactEmail;
                    objExtContactModel.WorkEmail = strWorkEmail;
                    return Json(objExtContactModel);
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    return BadRequest(ex.InnerException.InnerException.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
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
        [Route("InsertLeadCreateTime")]
        [ModelValidator]
        public IHttpActionResult PostLeadCreateTime([FromBody] ExtContactModel objExtContactModel)
        {
            try
            {
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion
                try
                {
                    lock (thisLock)
                    {
                        TranLeadContactModel objTranExtContactModel = new TranLeadContactModel();
                        TblTranLeadContact objTblTranExtContact = new TblTranLeadContact();
                        TblContact objTblContact = new TblContact();
                        long ContactId = objExtContactModel.ExtContactId;

                        //save contact
                        /*objTblContact = (from con in _dbcontext.TblContacts where con.Id == ContactId select con).FirstOrDefault();
                        objTblContact.ContactEmail = objExtContactModel.ContactEmail;
                        objTblContact.WorkEmail = objExtContactModel.WorkEmail; // sreedhar added on 04-jan-2016
                        _dbcontext.SaveChanges();*/

                        //Save Contacts and lead details
                        objTblTranExtContact.ContactId = ContactId;
                        objTblTranExtContact.ContactRoleId = objExtContactModel.ExtContactRoleId;
                        objTblTranExtContact.LeadId = objExtContactModel.LeadId;
                        objTblTranExtContact.TenantId = objExtContactModel.TenantId;
                       // objTblTranExtContact.TenantId = objExtContactModel.TenantId.ToDefaultTenantId();
                        _dbcontext.TblTranLeadContacts.Add(objTblTranExtContact);
                        _dbcontext.SaveChanges();

                        objTranExtContactModel = objNIMBOLEMapper.MapTable2Model(objTblTranExtContact);
                        objExtContactModel.Id = objTblTranExtContact.Id;
                        objExtContactModel.LeadId = Convert.ToInt64(objTblTranExtContact.LeadId);
                        objExtContactModel.TenantId = objTblTranExtContact.TenantId;
                        objExtContactModel.ExtContactRoleId = Convert.ToInt64(objTblTranExtContact.ContactRoleId);
                        var strRole = (from ec in _dbcontext.TblContactRoles where ec.Id == objExtContactModel.ExtContactRoleId && ec.TenantId== objExtContactModel.TenantId select ec.Description).FirstOrDefault();
                        objExtContactModel.ExtContactRole = strRole;
                        var strFullName = (from ec in _dbcontext.TblContacts where ec.Id == ContactId && ec.TenantId==objExtContactModel.TenantId  select ec.FirstName + "" + ec.LastName).FirstOrDefault();
                        var strContactEmail = (from ec in _dbcontext.TblContacts where ec.Id == ContactId select ec.ContactEmail).FirstOrDefault();
                        var strWorkEmail = (from ec in _dbcontext.TblContacts where ec.Id == ContactId && ec.TenantId==objExtContactModel.TenantId  select ec.WorkEmail).FirstOrDefault(); //sreedhar added on 04-jan-2016


                        objExtContactModel.FullName = strFullName;
                        objExtContactModel.ContactEmail = strContactEmail;
                        objExtContactModel.WorkEmail = strWorkEmail; //sreedhar added on 04-jan-2016
                        return Json(objExtContactModel);
                    }
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        ReasonPhrase = ex.Message
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
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
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

        #endregion

        #region PUT
        public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(val, NumberStyle,
          System.Globalization.CultureInfo.CurrentCulture, out result);
        }

        [HttpPut]
        [Route("Edit")]
        public IHttpActionResult Edit([FromBody] ExtContactModel objExtContactModel)
        {
            try
            {
                long contactid, contactrolid;

                if (isNumeric(objExtContactModel.ExtContactRole, System.Globalization.NumberStyles.Integer))
                {
                    contactrolid = Convert.ToInt64(objExtContactModel.ExtContactRole);
                }
                else
                {
                    contactrolid = objExtContactModel.ExtContactRoleId;
                }

                if (isNumeric(objExtContactModel.FullName, System.Globalization.NumberStyles.Integer))
                {
                    contactid = Convert.ToInt64(objExtContactModel.FullName);
                }
                else
                {
                    contactid = objExtContactModel.ExtContactId;
                }

                var objTblExtContacts = (from x in _dbcontext.TblTranLeadContacts where x.Id == objExtContactModel.Id && x.TenantId== objExtContactModel.TenantId  select x).FirstOrDefault();
                objTblExtContacts.ContactRoleId = contactrolid;
                if (objExtContactModel.ExtContactId != 0)
                {
                    objTblExtContacts.ContactId = contactid;

                    var objTblContact = (from x in _dbcontext.TblContacts where x.Id == contactid && x.TenantId==objExtContactModel.TenantId  select x).FirstOrDefault();
                    objTblContact.ContactEmail = objExtContactModel.ContactEmail;
                    objTblContact.WorkEmail = objExtContactModel.WorkEmail;// sreedhar added on 04-jan-2016
                }
                _dbcontext.SaveChanges();
                return Ok(objExtContactModel);
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

        #region Delete
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(int Id)
        {
            try
            {
                var qurey = _dbcontext.TblTranLeadContacts.Where(x => x.Id == Id).SingleOrDefault();
                if (qurey == null)
                    throw new InvalidOperationException("Record not Found.");
                _dbcontext.TblTranLeadContacts.Remove(qurey);
                _dbcontext.SaveChanges();
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
