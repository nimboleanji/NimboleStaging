using AutoMapper;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Common;
using NIMBOLE.Service.Models;
using NIMBOLE.Entities;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/EmployeeTarget")]
    public class EmployeeTargetController : ApiController
    {
        private static Object thisLock = new Object();
        private NimboleStagingEntities _dbcontext;
        DTO ObjNIMBOLEMapper;
        public EmployeeTargetController()
        {
            ObjNIMBOLEMapper = new DTO();
            _dbcontext = new NimboleStagingEntities();
        }

        #region GET
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                var lstTblEmployeeTargets = (from r in _dbcontext.TblEmployeeTargets where r.Status == true && r.TenantId==Tid select r).ToList();
                List<EmployeeTargetModel> lstEmployeeTargetModel = new List<EmployeeTargetModel>();
                EmployeeTargetModel objEmployeeTargetModel = null;
                lstEmployeeTargetModel.Add(new EmployeeTargetModel { EmployeeRole = NIMBOLE.GlobalResources.Resources.Select, EmployeeRoleId = -1, FinancialYear = NIMBOLE.GlobalResources.Resources.Select, FinancialYearId = -1 });
                foreach (var objTblEmployeeTarget in lstTblEmployeeTargets)
                {
                    objEmployeeTargetModel = new EmployeeTargetModel();
                    objEmployeeTargetModel = ObjNIMBOLEMapper.MapTable2Model(objTblEmployeeTarget);
                    lstEmployeeTargetModel.Add(objEmployeeTargetModel);
                }
                return Ok(lstEmployeeTargetModel);
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
        [Route("GetAllEmployeeTargets")]
        public IHttpActionResult GetAllEmployeeTargets(Guid Tid)
        {
            try
            {
                //var lstTblEmployeeTarget = (from x in _dbcontext.TblEmployeeTargets select x).ToList();
                //List<EmployeeTargetModel> lstEmployeeTargetModel = new List<EmployeeTargetModel>();
                //EmployeeTargetModel objEmployeeTargetModel = null;

                //foreach (var item in lstTblEmployeeTarget)
                //{
                //    objEmployeeTargetModel = new EmployeeTargetModel();
                //    objEmployeeTargetModel = ObjNIMBOLEMapper.MapTable2Model(item);

                //    long id = item.FinancialYearId ?? 0;
                //    var financialYearQry = _dbcontext.TblFinancialYears.SingleOrDefault(x => x.Id == id);

                //    objEmployeeTargetModel.FinancialYear = financialYearQry.FinancialYear;

                //    id = item.EmployeeRoleId ?? 0;
                //    var empRoleQuery = _dbcontext.TblEmployeeRoles.SingleOrDefault(x => x.Id == id);

                //    objEmployeeTargetModel.EmployeeRole = empRoleQuery.Description;

                //    lstEmployeeTargetModel.Add(objEmployeeTargetModel);
                //}

                var lstTblEmployeeTarget = (from x in _dbcontext.VWEmployeeTargets where x.TenantId==Tid  select x).ToList();
                return Json(lstTblEmployeeTarget);
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
        [Route("AllEmployeeTarget")]
        public IHttpActionResult AllEmployeeTarget( Guid Tid)
        {
            try
            {
                var lstTblEmployeeTarget = (from x in _dbcontext.TblEmployeeTargets where x.Status == true && x.TenantId==Tid select x).ToList();
                //List<EmployeeTargetModel> _lstEmployeeTargetModel = new List<EmployeeTargetModel>();
                //foreach (var objTblEmployeeTarget in lstTblEmployeeTarget)
                //{
                //    _lstEmployeeTargetModel.Add(ObjNIMBOLEMapper.MapTable2Model(objTblEmployeeTarget));
                //}
                //return Json(_lstEmployeeTargetModel);
                return Json(lstTblEmployeeTarget);
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
                var objTblEmployeeTarget = _dbcontext.TblEmployeeTargets.SingleOrDefault(r => r.Id == id && r.TenantId==Tid);
                if (objTblEmployeeTarget == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    EmployeeTargetModel objEmployeeTargetModel = ObjNIMBOLEMapper.MapTable2Model(objTblEmployeeTarget);
                    return Ok(objEmployeeTargetModel);
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
        [Route("InsertEmployeeTarget")]
        public IHttpActionResult Post([FromBody] EmployeeTargetModel objEmployeeTargetModel)
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
                    long FinancialYear = Convert.ToInt64(objEmployeeTargetModel.FinancialYear);
                    long EmployeeRole = Convert.ToInt64(objEmployeeTargetModel.EmployeeRole);

                    if (!_dbcontext.TblEmployeeTargets.Any(u => u.TblFinancialYear.Id == FinancialYear && u.TblEmployeeRole.Id == EmployeeRole && u.TenantId == objEmployeeTargetModel.TenantId))
                    {
                        TblEmployeeTarget objTblEmployeeTarget = ObjNIMBOLEMapper.MapModel2Table(objEmployeeTargetModel);
                        _dbcontext.TblEmployeeTargets.Add(objTblEmployeeTarget);
                        _dbcontext.SaveChanges();
                        objEmployeeTargetModel = ObjNIMBOLEMapper.MapTable2Model(objTblEmployeeTarget);

                        objEmployeeTargetModel.FinancialYear = _dbcontext.TblFinancialYears.Where(fy => fy.Id == objTblEmployeeTarget.FinancialYearId).FirstOrDefault().FinancialYear;

                        objEmployeeTargetModel.Description = _dbcontext.TblEmployeeRoles.Where(er => er.Id == objTblEmployeeTarget.EmployeeRoleId).FirstOrDefault().Description;

                        return Ok(objEmployeeTargetModel);
                    }
                    else
                    {
                        throw new InvalidOperationException("Record already Exits.");
                    }
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
                        // raise a new exception nesting  
                        // the current instance as InnerException  
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
        public IHttpActionResult Edit([FromBody] EmployeeTargetModel objEmployeeTargetModel)
        {
            try
            {
                TblEmployeeTarget objTblEmployeeTarget = new TblEmployeeTarget();
                objTblEmployeeTarget = (from t in _dbcontext.TblEmployeeTargets where t.Id == objEmployeeTargetModel.Id  && t.TenantId==objEmployeeTargetModel.TenantId select t).FirstOrDefault();
                objTblEmployeeTarget.TenantId = objEmployeeTargetModel.TenantId;
                objTblEmployeeTarget.Budget = objEmployeeTargetModel.Budget;
                objTblEmployeeTarget.IsAutomatic = objEmployeeTargetModel.IsAutomatic;
                objTblEmployeeTarget.QuarterlyTarget = objEmployeeTargetModel.QuarterlyTarget;
                objTblEmployeeTarget.MonthlyTarget = objEmployeeTargetModel.MonthlyTarget;
                objTblEmployeeTarget.WeeklyTarget = objEmployeeTargetModel.WeeklyTarget;
                objTblEmployeeTarget.ModifiedDate = objEmployeeTargetModel.ModifiedDate;
                objTblEmployeeTarget.Status = objEmployeeTargetModel.Status;
                objTblEmployeeTarget.TargetHike = objEmployeeTargetModel.TargetHike;
                 _dbcontext.SaveChanges();
                 return Ok(objEmployeeTargetModel);                
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
        [Route("DeleteEmployeeTarget")]
        public IHttpActionResult DeleteEmployeeTarget(int id, bool status)
        {
            try
            {
                var query = _dbcontext.TblEmployeeTargets.Where(x => x.Id == id).FirstOrDefault();
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