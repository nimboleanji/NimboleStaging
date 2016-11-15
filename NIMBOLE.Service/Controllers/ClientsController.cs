using AutoMapper;
using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;
using NIMBOLE.Entities;
namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Clients")]
    public class ClientsController : ApiController
    {
        private NIMBOLEContext _dbcontext = new NIMBOLEContext();

        #region GET
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var data = (from x in _dbcontext.TblSettings select x).ToList();
                if (data != null)
                    return Ok(data);
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
        [Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var query=_dbcontext.TblSettings.SingleOrDefault(x => x.Id == id);
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
        public IHttpActionResult Post([FromBody] TblSetting data)
        {
            try
            {
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion
                
                _dbcontext.TblSettings.Add(data);
                _dbcontext.SaveChanges();
                return Ok(data.Id);
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
        public IHttpActionResult Edit([FromBody] TblSetting entity)
        {
            try
            {
                var queries = (from x in _dbcontext.TblSettings select x).ToList();
                if (queries != null)
                {
                    TblSetting query = new TblSetting();
                    query = queries.Where(x => x.TenantId==entity.TenantId).FirstOrDefault();
                    query.FullName= entity.FullName;
                    query.PhoneNo= entity.PhoneNo;
                    query.URL= entity.URL;
                    query.DateFormat= entity.DateFormat;
                    query.TimeFormat= entity.TimeFormat;
                    query.CurrencyCode= entity.CurrencyCode;
                    query.NoOfLicenses= entity.NoOfLicenses;
                    query.LanguageCode = entity.LanguageCode;
                    query.CreatedDate = entity.CreatedDate;
                    query.ModifiedDate = entity.ModifiedDate;
                    query.Status = entity.Status;
                    _dbcontext.SaveChanges();
                    return Ok(entity);
                }
                else
                {
                    throw new InvalidOperationException("Record not Found.");
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
        public IHttpActionResult Delete(Guid TenantId)
        {
            try
            {
                var query = _dbcontext.TblSettings.Where(x => x.TenantId == TenantId).FirstOrDefault();
                if (query != null)
                {
                    _dbcontext.TblSettings.Remove(query);
                    _dbcontext.SaveChanges();
                    return Ok(query);
                }
                else
                {
                    throw new InvalidOperationException("Record not Found.");
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
    }
}