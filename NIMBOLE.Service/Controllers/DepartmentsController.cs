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
    [RoutePrefix("api/Departments")]
    public class DepartmentsController : ApiController
    {
        private static Object thisLock = new Object();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();
        #region GET
		   [HttpGet]
        [Route("GetAllDepartments")]
        public IHttpActionResult GetAllDepartments(Guid Tid)
        {
            try
            {
                var dep = (from d in _dbcontext.TblDepartments where d.TenantId == Tid && d.Status==true select d).ToList();
                if (dep != null)
                    return Ok(dep);
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
                var dep = (from d in _dbcontext.TblDepartments where d.TenantId == Tid select d).ToList();
                if (dep != null)
                    return Ok(dep);
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
        [Route("SelectAllDepartments")]
        public IHttpActionResult SelectAllDepartments(Guid Tid)
        {
            List<DepartmentModel> lstDepartmentModel = new List<DepartmentModel>();
            DepartmentModel ObjDepartmentModel = new DepartmentModel();
            try
            {
                var data = (from x in _dbcontext.TblDepartments where x.TenantId == Tid orderby x.Id descending select x).ToList();
                foreach (var item in data)
                {
                    lstDepartmentModel.Add(new DepartmentModel { Id = item.Id, DeptId = item.Id,Code = item.Code,DepartmentName =item.DepartmentName,Status = Convert.ToBoolean(item.Status)});
                }
                return Ok(lstDepartmentModel);
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
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectDepartmentsForDropdown")]
        public IHttpActionResult SelectDepartmentsForDropdown(Guid Tid)
        {
            try
            {
                _dbcontext.Configuration.AutoDetectChangesEnabled = false;
                var data = (from x in _dbcontext.TblDepartments where x.Status == true && x.TenantId == Tid orderby x.DepartmentName select new { Id = x.Id, Name = x.DepartmentName }).ToList();
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
            finally
            {
                _dbcontext.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        [HttpGet]
        [Route("SelectDepartmentsForCombobox")]
        public async Task<IHttpActionResult> SelectDepartmentsForCombobox(Guid Tid)
        {
            try
            {
                _dbcontext.Configuration.AutoDetectChangesEnabled = false;
                var data = await (from x in _dbcontext.TblDepartments where x.Status == true && x.TenantId == Tid orderby x.DepartmentName select new { Id = x.Id, Name = x.DepartmentName }).ToListAsync();

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
            finally
            {
                _dbcontext.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id, Guid Tid)
        {
            try
            {
                var query = _dbcontext.TblDepartments.SingleOrDefault(x => x.Id == id && x.TenantId == Tid);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    DepartmentModel ObjDepartmentModel = objNIMBOLEMapper.MapTable2Model(query);
                    return Ok(ObjDepartmentModel);
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
        [Route("GetIdByDepartment")]
        public IHttpActionResult GetIdByDepartment(string Department, Guid Tid)
        {
            try
            {
                var query = _dbcontext.TblDepartments.SingleOrDefault(x => x.DepartmentName == Department && x.TenantId == Tid);
                if (query == null)
                {
                    DepartmentModel objDepartmentModel = new DepartmentModel();
                    objDepartmentModel.DepartmentName = Department;
                    objDepartmentModel.Status = true;
                    TblDepartment ObjTblDepartment = objNIMBOLEMapper.MapModel2Table(objDepartmentModel);
                    _dbcontext.TblDepartments.Add(ObjTblDepartment);
                    _dbcontext.SaveChanges();
                    return Ok(ObjTblDepartment.Id);
                }
                else
                {
                    return Ok(query.Id);
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
        //[ModelValidator]
        public IHttpActionResult Post([FromBody] DepartmentModel objDepartmentModel)
        {
            try
            {
               // objDepartmentModel.TenantId = objDepartmentModel.TenantId.ToDefaultTenantId();
                objDepartmentModel.CreatedDate = DateTime.Now;
                objDepartmentModel.ModifiedDate = DateTime.Now;
                objDepartmentModel.Status = true;
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);

                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                lock (thisLock)
                {
                    TblDepartment objTblDepartment = objNIMBOLEMapper.MapModel2Table(objDepartmentModel);

                    if (!_dbcontext.TblDepartments.Any(u => u.DepartmentName == objDepartmentModel.DepartmentName && u.TenantId == objDepartmentModel.TenantId))
                    {
                        _dbcontext.TblDepartments.Add(objTblDepartment);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        throw new InvalidOperationException("Record already exists.");
                    }
                    objDepartmentModel = objNIMBOLEMapper.MapTable2Model(objTblDepartment);
                    objDepartmentModel.Id = objTblDepartment.Id;
                    if (objDepartmentModel != null)
                        return Ok(objDepartmentModel);
                    else
                        throw new InvalidOperationException("Record not Found.");
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

        /// <summary>
        /// Modified by Ravi for Combobox
        /// </summary>
        /// <param name="objDepartmentModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertDepartment")]
        public IHttpActionResult PostDepartment([FromBody] DepartmentModel objDepartmentModel)
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

                    TblDepartment objTblDepartment = objNIMBOLEMapper.MapModel2Table(objDepartmentModel);
                    if (!_dbcontext.TblDepartments.Any(u => u.DepartmentName == objDepartmentModel.DepartmentName && u.TenantId == objDepartmentModel.TenantId) && !string.IsNullOrEmpty(objDepartmentModel.DepartmentName))
                    {
                        _dbcontext.TblDepartments.Add(objTblDepartment);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        TblDepartment _objTblDepartment = _dbcontext.TblDepartments.Where(u => u.DepartmentName == objDepartmentModel.DepartmentName && u.TenantId == objDepartmentModel.TenantId).FirstOrDefault();
                        if (_objTblDepartment.Status == false)
                            throw new InvalidOperationException("Record already exist.");
                    }
                    objDepartmentModel = objNIMBOLEMapper.MapTable2Model(objTblDepartment);
                    objDepartmentModel.Id = objTblDepartment.Id;

                    var department = (from x in _dbcontext.TblDepartments where x.Status == true && x.TenantId == objDepartmentModel.TenantId select new { Id = x.Id, Name = x.DepartmentName }).ToList();
                    return Ok(department);
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
        public IHttpActionResult Edit([FromBody] DepartmentModel ObjDepartmentModel)
        {
            try
            {
                if (ObjDepartmentModel.DepartmentName != null)
                {
                    TblDepartment record = (from x in _dbcontext.TblDepartments where x.Id == ObjDepartmentModel.Id && x.TenantId== ObjDepartmentModel.TenantId select x).FirstOrDefault();
                    if (record == null)
                        throw new InvalidOperationException("Record is not found");

                    if (record.DepartmentName == ObjDepartmentModel.DepartmentName)
                    {
                        record.ModifiedDate = DateTime.Now;
                        record.Status = ObjDepartmentModel.Status;
                        record.Code = ObjDepartmentModel.Code;
                        _dbcontext.SaveChanges();
                        return Ok(ObjDepartmentModel);
                    }
                    else
                    {
                        List<TblDepartment> _objDepartment = (from d in _dbcontext.TblDepartments where d.DepartmentName == ObjDepartmentModel.DepartmentName && d.TenantId== ObjDepartmentModel.TenantId select d).ToList();
                        if (_objDepartment.Count == 0)
                        {
                            record.DepartmentName = ObjDepartmentModel.DepartmentName;
                            record.ModifiedDate = DateTime.Now;
                            record.Status = ObjDepartmentModel.Status;
                            record.Code = ObjDepartmentModel.Code;
                            _dbcontext.SaveChanges();
                            return Ok(ObjDepartmentModel);
                        }
                        else
                        {
                            throw new InvalidOperationException("Record already exists");
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
        public IHttpActionResult Delete(int DeptId, bool status)
        {
            try
            {
                var query = _dbcontext.TblDepartments.Where(x => x.Id == DeptId).FirstOrDefault();
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