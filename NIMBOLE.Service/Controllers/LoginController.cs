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
using NIMBOLE.Entities;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        DTO objNIMBOLEMapper = new DTO();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();

        #region GET
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var data = (from x in _dbcontext.TblLogins select x).ToList();
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
        [Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var query=_dbcontext.TblLogins.SingleOrDefault(x => x.Id == id);
                if(query==null)
                    throw new InvalidOperationException("Record not Found.");
                else
                    return Ok(query);
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
        public IHttpActionResult Post([FromBody]LoginModel objLoginModel)
        {
            try
            {
                TblLogin objTblLogin = new TblLogin();
                objTblLogin = objNIMBOLEMapper.MapModel2Table(objLoginModel);
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Json<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion
                _dbcontext.TblLogins.Add(objTblLogin);
                _dbcontext.SaveChanges();
                objLoginModel = objNIMBOLEMapper.MapTable2Model(objTblLogin);
                return Json(objLoginModel);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
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
        public IHttpActionResult Edit([FromBody] TblLogin entity)
        {
            try
            {
                var queries= (from x in _dbcontext.TblLogins select x).ToList();
                if (queries == null)
                    throw new InvalidOperationException("Record not Found.");
                TblLogin query = new TblLogin();
                query = queries.Where(x => x.TenantId==entity.TenantId).FirstOrDefault();
                query.EmailAddress = entity.EmailAddress;
                query.Password = entity.Password;
                query.ModifiedDate = entity.ModifiedDate;
                query.Status = entity.Status;
                _dbcontext.SaveChanges();
                return Ok(entity);
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
        public IHttpActionResult Delete(Guid TenantId)
        {
            try
            {
                var query = _dbcontext.TblLogins.Where(x => x.TenantId== TenantId).FirstOrDefault();
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                _dbcontext.TblLogins.Remove(query);
                _dbcontext.SaveChanges();
                return Ok(query);
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