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
using NIMBOLE.Entities;
using System.Threading.Tasks;
using System.Data.Entity;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/LeadSource")]
    public class LeadSourceController : ApiController
    {
        private static Object thisLock = new Object();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();
        List<LeadSourceModel> lstLeadSourceModel = new List<LeadSourceModel>();
        LeadSourceModel ObjLeadSourceModel = new LeadSourceModel();

        #region GET
		
		  [HttpGet]
        [Route("GetAllLeadSource")]
        public IHttpActionResult GetAllLeadSource(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblLeadSources where x.TenantId == Tid && x.Status==true select x).ToList();
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
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblLeadSources where x.TenantId == Tid orderby x.Id descending select x).ToList();
                foreach (var item in data)
                {
                    lstLeadSourceModel.Add(new LeadSourceModel { Code= item.Code, Description = item.Description , Status = Convert.ToBoolean(item.Status),Id= item.Id });
                }

                return Ok(lstLeadSourceModel);
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
        [Route("GetIdByLeadSource")]
        public IHttpActionResult GetIdByLeadSource(string LeadSource, Guid Tid)
        {
            try
            {
                
               var data = (from x in _dbcontext.TblLeadSources where x.Description==LeadSource &&  x.TenantId == Tid select x).FirstOrDefault();
               if (data == null)
                {
                    LeadSourceModel objLeadSourceModel = new LeadSourceModel();
                    objLeadSourceModel.Description = LeadSource;
                    objLeadSourceModel.Status = true;
                    TblLeadSource ObjTblLeadSource = objNIMBOLEMapper.MapModel2Table(objLeadSourceModel);
                    _dbcontext.TblLeadSources.Add(ObjTblLeadSource);
                    _dbcontext.SaveChanges();
                    return Ok(ObjTblLeadSource.Id);
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

        /// <summary>
        /// Modified by Ravi for Combobox
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectAllLeadSource")]
        public IHttpActionResult SelectAllLeadSource(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblLeadSources where x.Status == true && x.TenantId == Tid orderby x.Description select new { Id = x.Id, Name = x.Description }).ToList();
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
        [Route("SelectLeadSourceForCombobox")]
        public async Task<IHttpActionResult> SelectLeadSourceForCombobox(Guid Tid)
        {
            try
            {
                var data = await (from x in _dbcontext.TblLeadSources where x.Status == true && x.TenantId == Tid orderby x.Description select new { Id = x.Id, Name = x.Description }).ToListAsync();
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
        public IHttpActionResult GetById(int id, Guid Tid)
        {
            try
            {
                var query = _dbcontext.TblLeadSources.SingleOrDefault(x => x.Id == id && x.TenantId == Tid);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    LeadSourceModel ObjLeadSourceModel = objNIMBOLEMapper.MapTable2Model(query);
                    return Ok(ObjLeadSourceModel);
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

        #region POST
        [HttpPost]
        [Route("Insert")]
        [ModelValidator]
        public IHttpActionResult Post([FromBody] LeadSourceModel objLeadsourceModel)
        {
            try
            {
                lock (thisLock)
                {
                    objLeadsourceModel.CreatedDate = DateTime.Now;
                    objLeadsourceModel.ModifiedDate = DateTime.Now;
                    objLeadsourceModel.Status = true;
                    //objLeadsourceModel.TenantId = objLeadsourceModel.TenantId.ToDefaultTenantId();
                    //objLeadsourceModel.TenantId=

                    HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                    #region If ModelState is not Valid
                    if (response.StatusCode != HttpStatusCode.OK)
                        return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                    #endregion
                    TblLeadSource ObjTblLeadSource = objNIMBOLEMapper.MapModel2Table(objLeadsourceModel);
                    //if (ObjTblLeadSource.Description != null || ObjTblLeadSource.Code != null)
                    //{

                    if (!_dbcontext.TblLeadSources.Any(u => u.Description == objLeadsourceModel.Description && u.TenantId==objLeadsourceModel.TenantId ))
                    {
                        _dbcontext.TblLeadSources.Add(ObjTblLeadSource);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        throw new InvalidOperationException("Record already exists.");
                    }

                    objLeadsourceModel = objNIMBOLEMapper.MapTable2Model(ObjTblLeadSource);
                }
                return Ok(objLeadsourceModel);
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

        [HttpPost]
        [Route("InsertLeadSource")]
        public IHttpActionResult PostLeadSource([FromBody] LeadSourceModel objLeadSourceModel)
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

                    TblLeadSource objTblLeadSource = objNIMBOLEMapper.MapModel2Table(objLeadSourceModel);
                    if (!_dbcontext.TblLeadSources.Any(u => u.Description == objLeadSourceModel.Description && u.TenantId==objLeadSourceModel.TenantId))
                    {
                        _dbcontext.TblLeadSources.Add(objTblLeadSource);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        TblLeadSource _objTblLeadSource = _dbcontext.TblLeadSources.Where(u => u.Description == objLeadSourceModel.Description && u.TenantId== objLeadSourceModel.TenantId).FirstOrDefault();
                        if (_objTblLeadSource.Status == false)
                            throw new InvalidOperationException("Record already exist.");
                    }
                    objLeadSourceModel = objNIMBOLEMapper.MapTable2Model(objTblLeadSource);
                    objLeadSourceModel.Id = objTblLeadSource.Id;

                    var data = (from x in _dbcontext.TblLeadSources where x.Status == true && x.TenantId==objLeadSourceModel.TenantId  select new { Id = x.Id, Name = x.Description }).ToList();
                    return Ok(data);
                }
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
        public IHttpActionResult Edit([FromBody] LeadSourceModel objLeadSourceModel)
        {
            try
            {
                if (objLeadSourceModel.Description != null)
                {
                    TblLeadSource record = (from l in _dbcontext.TblLeadSources where l.Id == objLeadSourceModel.Id select l).FirstOrDefault();
                    if (record == null)
                        throw new InvalidOperationException("Record is not found");
                    if (record.Description == objLeadSourceModel.Description)
                    {
                        record.Code = objLeadSourceModel.Code;
                        record.ModifiedDate = DateTime.Now;
                        record.Status = objLeadSourceModel.Status;
                        _dbcontext.SaveChanges();
                        return Ok(objLeadSourceModel);
                    }
                    else
                    {
                        List<TblLeadSource> _objLeadSource = (from x in _dbcontext.TblLeadSources where x.Description == objLeadSourceModel.Description select x).ToList();
                        if (_objLeadSource.Count == 0)
                        {
                            record.Code = objLeadSourceModel.Code;
                            record.Description = objLeadSourceModel.Description;
                            record.ModifiedDate = DateTime.Now;
                            record.Status = objLeadSourceModel.Status;
                            _dbcontext.SaveChanges();
                            return Ok(objLeadSourceModel);
                        }
                        else
                        {
                            var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new StringContent("")
                            };
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
                var query = _dbcontext.TblLeadSources.Where(x => x.Id == id).FirstOrDefault();
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                query.Status = status;
                //_dbcontext.TblLeadSources.Remove(query);
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
