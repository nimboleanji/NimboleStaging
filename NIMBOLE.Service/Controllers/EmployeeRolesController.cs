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
    [RoutePrefix("api/EmployeeRoles")]
    public class EmployeeRolesController : ApiController
    {
        private static Object thisLock = new Object();
        DTO ObjNIMBOLEMapper = new DTO();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();

        #region Get
		
		       [HttpGet]
        [Route("GetAllEmpRoles")]
        public IHttpActionResult GetAllEmpRoles(Guid Tid)
        {
            try
            {
                var empRoles = (from r in _dbcontext.TblEmployeeRoles where r.Status == true && r.TenantId == Tid select r).ToList();
                
                if (empRoles != null)
                    return Ok(empRoles);
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
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                var empRoles = (from r in _dbcontext.TblEmployeeRoles where r.Status == true && r.TenantId == Tid && r.RoleOrder != 0 select new { r.RoleOrder, r.Id, r.Description }).ToList();

                List<EmployeeRoleModel> lstEmployeeRoleModel = new List<EmployeeRoleModel>();
                foreach (var item in empRoles)
                {
                    //EmployeeModel objEmployeeModel = new EmployeeModel();
                    //foreach (var emp in item.TblEmployees)
                    //{
                    //    objEmployeeModel.Comments = emp.Comments;
                    //    objEmployeeModel.EmpCode = emp.Code;
                    //    objEmployeeModel.EmpRoleId = emp.EmpRoleId;
                    //    objEmployeeModel.FirstName = emp.FirstName;
                    //    objEmployeeModel.LastName = emp.LastName;
                    //    objEmployeeModel.Location = emp.Location;
                    //}
                    EmployeeRoleModel objEmployeeRoleModel = new EmployeeRoleModel();
                    //objEmployeeRoleModel.CreatedDate = item.CreatedDate ?? DateTime.Now;
                    //objEmployeeRoleModel.ModifiedDate = item.ModifiedDate ?? DateTime.Now;
                    //objEmployeeRoleModel.ERoleCode = item.Code;
                    objEmployeeRoleModel.Description = item.Description;
                    objEmployeeRoleModel.Id = item.Id;
                    objEmployeeRoleModel.RoleOrder = item.RoleOrder ?? 0;

                    lstEmployeeRoleModel.Add(objEmployeeRoleModel);
                }
                if (lstEmployeeRoleModel != null)
                    return Ok(lstEmployeeRoleModel);
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
        [Route("GetAllNew")]
        public IHttpActionResult GetAllNew(Guid Tid)
        {
            try
            {
                var empRoles = (from r in _dbcontext.TblEmployeeRoles where r.Status == true && r.TenantId == Tid && (r.RoleOrder == null || r.RoleOrder == 0) select new { r.Id, r.Description }).ToList();

                List<EmployeeRoleModel> lstEmployeeRoleModel = new List<EmployeeRoleModel>();
                foreach (var item in empRoles)
                {
                    //EmployeeModel objEmployeeModel = new EmployeeModel(); 
                    //foreach (var emp in item.TblEmployees)
                    //{
                    //    objEmployeeModel.Comments = emp.Comments;
                    //    objEmployeeModel.EmpCode = emp.Code;
                    //    objEmployeeModel.EmpRoleId = emp.EmpRoleId;
                    //    objEmployeeModel.FirstName = emp.FirstName;
                    //    objEmployeeModel.LastName = emp.LastName;
                    //    objEmployeeModel.Location = emp.Location;
                    //}
                    EmployeeRoleModel objEmployeeRoleModel = new EmployeeRoleModel();
                    //objEmployeeRoleModel.CreatedDate = item.CreatedDate ?? DateTime.Now;
                    //objEmployeeRoleModel.ModifiedDate = item.ModifiedDate ?? DateTime.Now;
                    //objEmployeeRoleModel.ERoleCode = item.Code;
                    objEmployeeRoleModel.Description = item.Description;
                    objEmployeeRoleModel.Id = item.Id;

                    lstEmployeeRoleModel.Add(objEmployeeRoleModel);
                }

                if (lstEmployeeRoleModel != null)
                    return Ok(lstEmployeeRoleModel);
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
        [Route("GetAllEmployeeRoles")]
        public IHttpActionResult GetAllEmployeeRoles(Guid Tid)
        {
            try
            {
                // var data = (from x in _dbcontext.TblEmployeeRoles where x.Status == true && x.TenantId==Tid orderby x.Description select new { Id = x.Id, Description = x.Description }).ToList();
                var data = (from x in _dbcontext.TblEmployeeRoles where x.Status == true && x.Code != "Admin" && x.TenantId == Tid orderby x.Id select new { Id = x.Id, Name = x.Description }).ToList();
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
        [Route("AllEmployeeRole")]
        public IHttpActionResult AllEmployeeRole(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblEmployeeRoles where x.TenantId == Tid && x.Id != 1 select x).ToList();
                List<EmployeeRoleModel> objEmployeeRoles = new List<EmployeeRoleModel>();
                EmployeeRoleModel obj_EmployeeRoleModel = new EmployeeRoleModel();

                foreach (var item in data)
                {
                    objEmployeeRoles.Add(new EmployeeRoleModel {Id = item.Id, ERoleCode = item.Code, Description = item.Description , Status = Convert.ToBoolean(item.Status)});
                }
                return Ok(objEmployeeRoles);
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
                var query = _dbcontext.TblEmployeeRoles.SingleOrDefault(r => r.Id == id && r.TenantId == Tid);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    EmployeeRoleModel ObjEmployeeRoleModel = ObjNIMBOLEMapper.MapTable2Model(query);
                    return Ok(ObjEmployeeRoleModel);
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

        [HttpGet]
        [Route("GetModulesList")]
        public IHttpActionResult GetModulesList(Guid Tid)
        {
            try
            {
                var lstModules = (from mdl in _dbcontext.TblModules
                                  where mdl.TenantId == Tid
                                  select new NIMBOLE.Models.Models.EmployeeRoleModel
                                      {
                                          ERoleCode = mdl.Code,
                                          Description = mdl.Description
                                      }).ToList();
                if (lstModules == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    return Json(lstModules);
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

        #region Insert
        [HttpPost]
        [Route("InsertEmployeeRole")]
        // [ModelValidator]
        public IHttpActionResult Post([FromBody] EmployeeRoleModel ObjEmployeeRoleModel)
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
                    TblEmployeeRole ObjTblEmployeeRole = ObjNIMBOLEMapper.MapModel2Table(ObjEmployeeRoleModel);

                    if (!_dbcontext.TblEmployeeRoles.Any(u => u.Description == ObjEmployeeRoleModel.Description && u.TenantId==ObjEmployeeRoleModel.TenantId))
                    {
                        _dbcontext.TblEmployeeRoles.Add(ObjTblEmployeeRole);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        throw new InvalidOperationException("Record already exists.");
                    }
                    ObjEmployeeRoleModel = ObjNIMBOLEMapper.MapTable2Model(ObjTblEmployeeRole);

                    return Ok(ObjEmployeeRoleModel);
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
        public IHttpActionResult Edit([FromBody] EmployeeRoleModel objEmployeeRoleModel)
        {
            try
            {
                if (objEmployeeRoleModel.Description != null)
                {
                    TblEmployeeRole record = (from x in _dbcontext.TblEmployeeRoles where x.Id == objEmployeeRoleModel.Id && x.TenantId == objEmployeeRoleModel.TenantId select x).FirstOrDefault();
                    if (record == null)
                        throw new InvalidOperationException("Record not Found.");
                    else
                        if (record.Description == objEmployeeRoleModel.Description)
                        {
                            record.Code = objEmployeeRoleModel.ERoleCode;
                            record.SelectedModules = objEmployeeRoleModel.SelectedModules;
                            record.ModifiedDate = objEmployeeRoleModel.ModifiedDate.ToDefaultDateIfTooEarly();
                            record.Status = objEmployeeRoleModel.Status;
                            _dbcontext.SaveChanges();
                            return Ok(objEmployeeRoleModel);
                        }
                        else
                        {
                            List<TblEmployeeRole> _objEmployeeRole = (from e in _dbcontext.TblEmployeeRoles where e.Description == objEmployeeRoleModel.Description && e.TenantId == objEmployeeRoleModel.TenantId select e).ToList();
                            if (_objEmployeeRole.Count == 0)
                            {
                                record.Code = objEmployeeRoleModel.ERoleCode;
                                record.Description = objEmployeeRoleModel.Description;
                                record.SelectedModules = objEmployeeRoleModel.SelectedModules;
                                record.ModifiedDate = objEmployeeRoleModel.ModifiedDate.ToDefaultDateIfTooEarly();
                                record.Status = objEmployeeRoleModel.Status;
                                _dbcontext.SaveChanges();
                                return Ok(objEmployeeRoleModel);
                            }
                            else
                            {
                                throw new InvalidOperationException("Record already exists.");
                            }
                        }
                }
                return Ok(objEmployeeRoleModel);
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
        [Route("DeleteEmployeeRole")]
        public IHttpActionResult DeleteEmployeeRole(int id, bool status)
        {
            try
            {
                var query = _dbcontext.TblEmployeeRoles.Where(x => x.Id == id).FirstOrDefault();
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
        #endregion Delete
    }
}