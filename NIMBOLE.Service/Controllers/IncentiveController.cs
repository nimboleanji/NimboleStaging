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
    [RoutePrefix("api/Incentive")]
    public class IncentiveController : ApiController
    {
        private static Object thisLock = new Object();
        DTO objNIMBOLEMapper = new DTO();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();

        IncentiveModel Obj_IncentiveModel = new IncentiveModel();
        List<IncentiveModel> list_Incentive = new List<IncentiveModel>();

        #region Get
        // GET api/incentive
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                var list_Incentive = (from x in _dbcontext.TblIncentives where x.TenantId == Tid orderby x.Id descending select x).ToList();

                if (list_Incentive == null)
                {
                    throw new InvalidOperationException("Record not Found.");
                }
                return Ok(list_Incentive);
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
        [Route("AllIncentive")]
        public async Task<IHttpActionResult> AllIncentive(Guid Tid)
        {
            try
            {

                var lstIncentiveModel = await (from r in _dbcontext.TblIncentives where r.Status == true && r.TenantId == Tid orderby r.IncFrom select r).ToListAsync();
                return Ok(lstIncentiveModel);
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
                var query = _dbcontext.TblIncentives.SingleOrDefault(x => x.Id == id && x.TenantId == Tid);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    IncentiveModel ObjIncentiveModel = objNIMBOLEMapper.MapTable2Model(query);
                    return Ok(ObjIncentiveModel);
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
        public IHttpActionResult Post([FromBody] IncentiveModel objIncentiveModel)
        {
            try
            {
                //objIncentiveModel.TenantId = objIncentiveModel.TenantId.ToDefaultTenantId();

                objIncentiveModel.CreatedDate = DateTime.Now;
                objIncentiveModel.ModifiedDate = DateTime.Now;
                objIncentiveModel.Status = true;
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);

                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                TblIncentive objTblIncentive = objNIMBOLEMapper.MapModel2Table(objIncentiveModel);

                if (!_dbcontext.TblIncentives.Any(u => u.IncFrom == objIncentiveModel.IncFrom && u.TenantId==objIncentiveModel.TenantId))
                {
                    _dbcontext.TblIncentives.Add(objTblIncentive);
                    _dbcontext.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Record already exists.");
                }
                objIncentiveModel = objNIMBOLEMapper.MapTable2Model(objTblIncentive);
                objIncentiveModel.Id = objTblIncentive.Id;
                if (objIncentiveModel != null)
                    return Ok(objIncentiveModel);
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
        public IHttpActionResult Edit([FromBody] IncentiveModel ObjIncentiveModel)
        {
            try
            {
                if (ObjIncentiveModel.IncFrom != null)
                {
                    TblIncentive record = (from x in _dbcontext.TblIncentives where x.Id == ObjIncentiveModel.Id && x.TenantId== ObjIncentiveModel.TenantId select x).FirstOrDefault();
                    if (record == null)
                        throw new InvalidOperationException("Record Not Found");
                    if (record.IncFrom == ObjIncentiveModel.IncFrom)
                    {
                        record.IncTo = ObjIncentiveModel.IncTo;
                        record.Percentage = ObjIncentiveModel.Percentage;
                        record.Comments = ObjIncentiveModel.Comments;
                        record.ModifiedDate = DateTime.Now;
                        record.Status = ObjIncentiveModel.Status;
                        _dbcontext.SaveChanges();
                        return Ok(ObjIncentiveModel);
                    }
                    else
                    {
                        List<TblIncentive> _ObjIncentive = (from c in _dbcontext.TblIncentives where c.TenantId== ObjIncentiveModel.TenantId && c.IncFrom == ObjIncentiveModel.IncFrom select c).ToList();

                        if (_ObjIncentive.Count == 0)
                        {
                            record.IncTo = ObjIncentiveModel.IncTo;
                            record.Percentage = ObjIncentiveModel.Percentage;
                            record.Comments = ObjIncentiveModel.Comments;
                            record.ModifiedDate = DateTime.Now;
                            record.Status = ObjIncentiveModel.Status;
                            _dbcontext.SaveChanges();
                            return Ok(ObjIncentiveModel);
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
                var query = _dbcontext.TblIncentives.Where(x => x.Id == id).FirstOrDefault();
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
