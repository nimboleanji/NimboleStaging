using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Entities.Models;
using NIMBOLE.Entities;
using AutoMapper;
using NIMBOLE.Entities.Mappers;
using NIMBOLE.Service.Controllers;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/ExtContact")]
    public class ExtContactController : ApiController
    {
        DTO objNIMBOLEMapper = new DTO();
        private NIMBOLEContext _dbcontext = new NIMBOLEContext();
        #region GET
        //[HttpGet]
        //[Route("GetAll")]
        //public IHttpActionResult GetAll()
        //{
        //    var objTblExtContact = (from x in _dbcontext.TblExtContacts where x.Status == true select x).ToList();
        //    List<ExtContactModel> lstExtContactModel = new List<ExtContactModel>();
        //    foreach (var item in objTblExtContact)
        //    {

        //        var objRoleDes = (from r in _dbcontext.TblExtContactRoles where r.Id == item.ExtContactRoleId select r.Description).FirstOrDefault();

        //        lstExtContactModel.Add(new ExtContactModel() { Id = item.Id, TenantId = item.TenantId, LeadId = item.LeadId??0, ExtContactRoleId = item.ExtContactRoleId??0, ExtContactRole=objRoleDes, FullName = item.FullName, CreatedDate = item.CreatedDate??DateTime.Now, ModifiedDate = item.ModifiedDate??DateTime.Now, Status = item.Status??true });

        //        //lstExtContactModel.Add(new ExtContactModel() {FullName = item.FullName});
        //    }
        //    return Json(lstExtContactModel);
        //}

        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            var lstTblExtContact = (from x in _dbcontext.TblExtContacts where x.Status == true select x).ToList();
            List<ExtContactModel> lstExtContactModel = new List<ExtContactModel>();
            foreach (var item in lstTblExtContact)
            {
                ExtContactModel objExtContactModel = new ExtContactModel();
                
                objExtContactModel = objNIMBOLEMapper.MapTable2Model(item);
                lstExtContactModel.Add(objExtContactModel);
            }
            return Json(lstExtContactModel);
        }

        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var objTblExtContact = (from c in _dbcontext.TblExtContacts where c.Id == id && c.Status == true select c).FirstOrDefault();

                if (objTblExtContact != null)
                {
                    ExtContactModel objExtContactModel = new ExtContactModel();
                    #region ExtContactModel
                    objExtContactModel = objNIMBOLEMapper.MapTable2Model(objTblExtContact);
                    #endregion
                    return Ok(objExtContactModel);
                }
                return BadRequest("Record Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

                using (System.Data.Entity.DbContextTransaction dbTran = _dbcontext.Database.BeginTransaction())
                {
                    try
                    {
                        objExtContactModel.ExtContactRoleId = (from cr in _dbcontext.TblExtContactRoles where cr.Description == objExtContactModel.ExtContactRole select cr.Id).FirstOrDefault();
                        TblExtContact objTblExtContact=new TblExtContact();
                        objTblExtContact = objNIMBOLEMapper.MapModel2Table(objExtContactModel);
                        _dbcontext.TblExtContacts.Add(objTblExtContact);
                        _dbcontext.SaveChanges();
                        objExtContactModel = objNIMBOLEMapper.MapTable2Model(objTblExtContact);
                        dbTran.Commit();
                        return Ok(objExtContactModel);
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        //Rollback transaction if exception occurs
                        dbTran.Rollback();
                        return BadRequest(ex.InnerException.InnerException.Message);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }//Close Using Block
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
                return new NIMBOLE.Service.Controllers.TextResult(ex.Message, Request);
            }

        }
        #endregion

        #region PUT
        //[HttpPut]
        //[Route("Edit")]
        //public IHttpActionResult Edit([FromBody] TransAccConModel objTransAccConModel)
        //{
        //    var objContacts = (from x in _dbcontext.TblContacts where x.Id == objTransAccConModel.objContactModel.Id select x).FirstOrDefault();
        //    if (objContacts != null)
        //    {
        //        objContacts = objNIMBOLEMapper.MapModel2Table(objTransAccConModel.objContactModel);
        //        _dbcontext.SaveChanges();
        //        var objTblAddress = (from a in _dbcontext.TblAddresses where a.Id == objTransAccConModel.objAddressModel.Id select a).FirstOrDefault();
        //        if (objTblAddress != null)
        //        {
        //            objTblAddress = objNIMBOLEMapper.MapModel2Table(objTransAccConModel.objAddressModel);
        //        }
        //        //_dbcontext.Entry(objTblAddress).State = EntityState.Modified;
        //        _dbcontext.SaveChanges();
        //        return Ok(objTransAccConModel);
        //    }

        //    else
        //    {
        //        return BadRequest("Not Found");
        //    }
        //}
        //#endregion

        //#region DELETE
        //[HttpDelete]
        //[Route("Delete")]
        //public IHttpActionResult Delete(int id)
        //{
        //    try
        //    {
        //        var query = _dbcontext.TblContacts.Where(x => x.Id == id).FirstOrDefault();
        //        if (query == null)
        //            return BadRequest("Not Found Record");
        //        query.Status = false;
        //        // _dbcontext.TblContacts.Remove(query);
        //        _dbcontext.SaveChanges();
        //        return Ok(query);
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}
        #endregion
    }
}
