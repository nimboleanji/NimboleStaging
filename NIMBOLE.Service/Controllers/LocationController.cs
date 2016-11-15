using AutoMapper;
using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.Entity;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Service.Models;
using NIMBOLE.Common;
using NIMBOLE.Entities;
using System.Threading.Tasks;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Location")]
    public class LocationController : ApiController
    {
        private static Object thisLock = new Object();
        DTO objNIMBOLEMapper = new DTO();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();

        LocationModel Obj_LocationModel = new LocationModel();
        List<LocationModel> list_Location = new List<LocationModel>();

        #region Get
        // GET api/location
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                var list_Location = (from x in _dbcontext.TblLocations where x.TenantId == Tid orderby x.Id descending select x).ToList();

                if (list_Location == null)
                {
                    throw new InvalidOperationException("Record not Found.");
                }
                return Ok(list_Location);
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
        [Route("AllLocation")]
        public async Task<IHttpActionResult> AllLocation(Guid Tid)
        {
            try
            {

                var lstLocationModel = await (from r in _dbcontext.TblLocations where r.Status == true && r.TenantId == Tid orderby r.Description select r).ToListAsync();
                return Ok(lstLocationModel);
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
                var query = _dbcontext.TblLocations.SingleOrDefault(x => x.Id == id && x.TenantId == Tid);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    LocationModel ObjLocationModel = objNIMBOLEMapper.MapTable2Model(query);
                    return Ok(ObjLocationModel);
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

        #endregion Get

        #region POST
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Post([FromBody] LocationModel objLocationModel)
        {
            try
            {
               // objLocationModel.TenantId = objLocationModel.TenantId.ToDefaultTenantId();
                
                objLocationModel.CreatedDate = DateTime.Now;
                objLocationModel.ModifiedDate = DateTime.Now;
                objLocationModel.Status = true;
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);

                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                TblLocation objTblLocation = objNIMBOLEMapper.MapModel2Table(objLocationModel);

                if (!_dbcontext.TblLocations.Any(u => u.Description == objLocationModel.Description && u.TenantId == objLocationModel.TenantId))
                {
                    _dbcontext.TblLocations.Add(objTblLocation);
                    _dbcontext.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Record already exists.");
                }
                objLocationModel = objNIMBOLEMapper.MapTable2Model(objTblLocation);
                objLocationModel.Id = objTblLocation.Id;
                if (objLocationModel != null)
                    return Ok(objLocationModel);
                else
                    throw new InvalidOperationException("Record not Found.");

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
        public IHttpActionResult Edit([FromBody] LocationModel ObjLocationModel)
        {
            try
            {
                if (ObjLocationModel.Description != null)
                {
                    TblLocation record = (from x in _dbcontext.TblLocations where x.Id == ObjLocationModel.Id && x.TenantId==ObjLocationModel.TenantId  select x).FirstOrDefault();
                    if (record == null)
                        throw new InvalidOperationException("Record Not Found");
                    if (record.Description == ObjLocationModel.Description)
                    {
                        record.Code = ObjLocationModel.Code;
                        record.ModifiedDate = DateTime.Now;
                        record.Status = ObjLocationModel.Status;
                        _dbcontext.SaveChanges();
                        return Ok(ObjLocationModel);
                    }
                    else
                    {
                        List<TblLocation> _ObjLocation = (from c in _dbcontext.TblLocations where c.Description == ObjLocationModel.Description && c.TenantId == ObjLocationModel.TenantId select c).ToList();

                        if (_ObjLocation.Count == 0)
                        {
                            record.Code = ObjLocationModel.Code;
                            record.Description = ObjLocationModel.Description;
                            record.ModifiedDate = DateTime.Now;
                            record.Status = ObjLocationModel.Status;
                            _dbcontext.SaveChanges();
                            return Ok(ObjLocationModel);
                        }
                        else
                        {
                            throw new InvalidOperationException("Record already exists.");
                        }
                    }
                }
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
        #endregion

        #region DELETE
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(int id, bool status)
        {
            try
            {
                var query = _dbcontext.TblLocations.Where(x => x.Id == id).FirstOrDefault();
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                query.Status = status;
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
