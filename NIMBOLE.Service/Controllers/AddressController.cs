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
    [RoutePrefix("api/Address")]
    public class AddressController : ApiController
    {
        DTO objNIMBOLEMapper = new DTO();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();

        #region GET
               
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                var data = (from a in _dbcontext.TblAddresses where a.TenantId==Tid select a).ToList();
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
                var query = _dbcontext.TblAddresses.SingleOrDefault(x => x.Id == id);
                if (query == null)
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
        public IHttpActionResult Post([FromBody] AddressModel objAddressModel)
        {
            try
            {
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                TblAddress objTblAddress = new TblAddress();
                objTblAddress = objNIMBOLEMapper.MapModel2Table(objAddressModel);

                _dbcontext.TblAddresses.Add(objTblAddress);
                _dbcontext.SaveChanges();
                objAddressModel.Id = objTblAddress.Id;

                return Json(objAddressModel);
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
                throw new InvalidOperationException(dbEx.Message);
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
        public IHttpActionResult Edit([FromBody] TblAddress entity)
        {
            try
            {
                var queries = (from x in _dbcontext.TblAddresses select x).ToList();
                if (queries != null)
                {
                    TblAddress query = new TblAddress();
                    query = queries.Where(x => x.Id== entity.Id).FirstOrDefault();
                    query.StreetName = entity.StreetName;
                    query.CityId = entity.CityId;
                    query.ZipCode = entity.ZipCode;
                    query.Phone = entity.Phone;
                    query.Mobile = entity.Mobile;
                    query.HomePhone = entity.HomePhone;
                    query.Fax = entity.Fax;
                    query.SkypeName = entity.SkypeName;                    
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
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var query = _dbcontext.TblAddresses.Where(x => x.Id == id).FirstOrDefault();
                if (query != null)
                {
                    _dbcontext.TblAddresses.Remove(query);
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
