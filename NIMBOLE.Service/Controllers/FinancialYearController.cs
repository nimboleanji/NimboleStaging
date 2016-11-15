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
using NIMBOLE.Entities;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/FinancialYear")]
    public class FinancialYearController : ApiController
    {
        private static Object thisLock = new Object();
        private DTO objNIMBOLEMapper;
        private TblFinancialYear objTblFinancialYear;
        private FinancialYearModel objFinancialYearModel;
        private NimboleStagingEntities _dbcontext;

        FinancialYearModel obj_FinancialYearModel = new FinancialYearModel();
        List<FinancialYearModel> IstFinancialYearModel = new List<FinancialYearModel>();

        public FinancialYearController()
        {
            _dbcontext = new NimboleStagingEntities();
            objNIMBOLEMapper = new DTO();
            objTblFinancialYear = new TblFinancialYear();
            objFinancialYearModel = new FinancialYearModel();
        }

        #region Get
        // GET api/FinancialYear
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                var data = (from r in _dbcontext.TblFinancialYears where r.TenantId==Tid select r).ToList();
                foreach (var item in data)
                {
                    IstFinancialYearModel.Add(new FinancialYearModel { });
                
                }
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
        [Route("GetAllFinancialYears")]
        public IHttpActionResult GetAllFinancialYears(Guid Tid)
        {
            try
            {
                var roles = (from r in _dbcontext.TblFinancialYears where r.Status == true && r.TenantId==Tid orderby r.FinancialYear descending  select r).ToList();
                List<FinancialYearModel> lstFinancialYearModel = new List<FinancialYearModel>();
                lstFinancialYearModel.Add(new FinancialYearModel
                {
                    Id = -1,
                    FinancialYear = "Select"
                });
                foreach (var item in roles)
                {
                    lstFinancialYearModel.Add(objNIMBOLEMapper.MapTable2Model(item));
                }
                return Ok(lstFinancialYearModel);
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
        [Route("GetFinancialYearsForReports")]
        public IHttpActionResult GetFinancialYearsForReports(Guid Tid)
        {
            try
            {
                var fy = (from r in _dbcontext.TblFinancialYears where r.Status == true && r.TenantId== Tid select r).ToList();
                return Ok(fy);
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
        public IHttpActionResult GetById(int id,Guid Tid)
        {
            try
            {
                var query = _dbcontext.TblFinancialYears.SingleOrDefault(x => x.Id == id && x.TenantId==Tid );
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    FinancialYearModel objFinancialYearModel = objNIMBOLEMapper.MapTable2Model(query);
                    return Ok(objFinancialYearModel);
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
        [ModelValidator]
        public IHttpActionResult Post([FromBody] FinancialYearModel objFinancialYearModel)
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
                    TblFinancialYear objTblFinancialYear = objNIMBOLEMapper.MapModel2Table(objFinancialYearModel);
                    if (objTblFinancialYear.FinancialYear != null)
                    {
                        if (!_dbcontext.TblFinancialYears.Any(u => u.FinancialYear == objFinancialYearModel.FinancialYear && u.TenantId == objFinancialYearModel.TenantId))
                        {
                            _dbcontext.TblFinancialYears.Add(objTblFinancialYear);
                            _dbcontext.SaveChanges();
                        }
                        else
                        {
                            throw new InvalidOperationException("Record already exists.");
                        }
                    }
                    objFinancialYearModel = objNIMBOLEMapper.MapTable2Model(objTblFinancialYear);
                    return Ok(objFinancialYearModel);
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
        public IHttpActionResult Edit([FromBody] FinancialYearModel objFinancialYearModel)
        {
            try
            {
                if (objFinancialYearModel.FinancialYear != null)
                {
                    TblFinancialYear record = (from x in _dbcontext.TblFinancialYears where x.Id == objFinancialYearModel.Id && x.TenantId == objFinancialYearModel.TenantId select x).FirstOrDefault();
                    if (record == null)
                        throw new InvalidOperationException("Record not Found.");
                    if (record.FinancialYear == objFinancialYearModel.FinancialYear)
                    {
                        record.Description = objFinancialYearModel.Description;
                        record.Status = objFinancialYearModel.Status;
                        record.ModifiedDate = DateTime.Now;
                        _dbcontext.SaveChanges();
                        return Ok(objFinancialYearModel);
                    }
                    else
                    {
                        List<TblFinancialYear> _objFinancialYear = (from e in _dbcontext.TblFinancialYears where e.FinancialYear == objFinancialYearModel.FinancialYear && e.TenantId == objFinancialYearModel.TenantId select e).ToList();

                        if (_objFinancialYear.Count == 0)
                        {
                            record.TenantId = objFinancialYearModel.TenantId;
                            record.FinancialYear = objFinancialYearModel.FinancialYear;
                            record.Description = objFinancialYearModel.Description;
                            record.Status = objFinancialYearModel.Status;
                            record.ModifiedDate = DateTime.Now;
                            _dbcontext.SaveChanges();
                            return Ok(objFinancialYearModel);
                        }
                        else 
                        {
                            throw new InvalidOperationException("Record already exists.");
                        }
                      }
                   }   
                        return Ok(objFinancialYearModel);
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
                var query = _dbcontext.TblFinancialYears.Where(x => x.Id == id).FirstOrDefault();
                if (query == null)
                    throw new InvalidOperationException("Not Found Record");
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