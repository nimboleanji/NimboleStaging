using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;
using NIMBOLE.Models;
using AutoMapper;
using NIMBOLE.Common;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Entities;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/ContactRoles")]
    public class ContactRolesController : ApiController
    {
        private static Object thisLock = new Object();
        DTO objNIMBOLEMapper = new DTO();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();

        ContactRoleModel Obj_ContactRoleModel = new ContactRoleModel();
        List<ContactRoleModel> list_ContactRole = new List<ContactRoleModel>();

        #region Get
        // GET api/contactroles
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblContactRoles where x.TenantId == Tid orderby x.Id descending select x).ToList();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list_ContactRole.Add(new ContactRoleModel { Code = item.Code, Description = item.Description, Status = Convert.ToBoolean(item.Status), Id = item.Id });
                    }
                }

                else
                {
                    throw new InvalidOperationException("Record not Found.");
                }
                return Ok(list_ContactRole);
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
        [Route("GetAllContactRoles")]
        public IHttpActionResult GetAllContactRoles(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblContactRoles where x.TenantId == Tid && x.Status == true select x).ToList();
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


        /// <summary>
        /// Modified by Ravi for Combobox
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("AllContactRole")]
        public async Task<IHttpActionResult> AllContactRole(Guid Tid)
        {
            try
            {
                //var data = (from x in _dbcontext.TblContactRoles where x.Status == true && x.TenantId==Tid orderby x.Description select new { Id = x.Id, Name = x.Description }).ToList();
                var lstContactRoleModel = await (from r in _dbcontext.TblContactRoles where r.Status == true && r.TenantId == Tid orderby r.Description select new { RoleCode = r.Code, Description = r.Description, Status = r.Status, TenantId = r.TenantId, Id = r.Id }).ToListAsync();
                return Ok(lstContactRoleModel);
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
                var query = _dbcontext.TblContactRoles.SingleOrDefault(x => x.Id == id && x.TenantId == Tid);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    ContactRoleModel ObjContactRoleModel = objNIMBOLEMapper.MapTable2Model(query);
                    return Ok(ObjContactRoleModel);
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
        [Route("SelectContactRolesForDropdown")]
        public IHttpActionResult SelectContactRolesForDropdown(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblContactRoles where x.Status == true && x.TenantId == Tid orderby x.Description select new { Id = x.Id, Name = x.Description }).ToList();
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
        [Route("SelectContactRolesForCombobox")]
        public async Task<IHttpActionResult> SelectContactRolesForCombobox(Guid Tid)
        {
            try
            {
                _dbcontext.Configuration.AutoDetectChangesEnabled = false;
                var data = await (from x in _dbcontext.TblContactRoles where x.Status == true && x.TenantId == Tid orderby x.Description select new { Id = x.Id, Name = x.Description }).ToListAsync();
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
        [Route("GetIdByDesignation")]
        public IHttpActionResult GetIdByDesignation(string Designation, Guid Tid)
        {
            try
            {
                var query = _dbcontext.TblContactRoles.SingleOrDefault(x => x.Description == Designation && x.TenantId == Tid);
                if (query == null)
                {
                    ContactRoleModel objContactRoleModel = new ContactRoleModel();
                    objContactRoleModel.Description = Designation;
                    objContactRoleModel.Status = true;
                    TblContactRole ObjTblContactRole = objNIMBOLEMapper.MapModel2Table(objContactRoleModel);
                    _dbcontext.TblContactRoles.Add(ObjTblContactRole);
                    _dbcontext.SaveChanges();
                    return Ok(ObjTblContactRole.Id);
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


        #endregion Get

        #region POST
        /// <summary>
        /// Modified by Ravi for Combobox
        /// </summary>
        /// <param name="ObjContactRoleModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertLeadContact")]
        [ModelValidator]
        public IHttpActionResult Post([FromBody] ContactRoleModel ObjContactRoleModel)
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
                    TblContactRole ObjTblContactRole = objNIMBOLEMapper.MapModel2Table(ObjContactRoleModel);
                    if (!_dbcontext.TblContactRoles.Any(u => u.Code == ObjContactRoleModel.RoleCode && u.Description == ObjContactRoleModel.Description))
                    {
                        _dbcontext.TblContactRoles.Add(ObjTblContactRole);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        TblContactRole _objTblContactRole = _dbcontext.TblContactRoles.Where(u => u.Code == ObjContactRoleModel.RoleCode && u.Description == ObjContactRoleModel.Description && u.TenantId == ObjContactRoleModel.TenantId).FirstOrDefault();
                        if (_objTblContactRole.Status == false)
                            throw new InvalidOperationException("Record already exist.");
                    }
                    ObjContactRoleModel = objNIMBOLEMapper.MapTable2Model(ObjTblContactRole);
                    ObjContactRoleModel.Id = ObjTblContactRole.Id;

                    var data = (from x in _dbcontext.TblContactRoles where x.Status == true && x.TenantId == ObjContactRoleModel.TenantId select new { Id = x.Id, Name = x.Description }).ToList();
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

        [HttpPost]
        [Route("InsertContactRole")]
        [ModelValidator]
        public IHttpActionResult InsertContactRole([FromBody] ContactRoleModel ObjContactRoleModel)
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
                    TblContactRole ObjTblContactRole = objNIMBOLEMapper.MapModel2Table(ObjContactRoleModel);
                    if (!_dbcontext.TblContactRoles.Any(u => u.Description == ObjContactRoleModel.Description && u.TenantId == ObjContactRoleModel.TenantId))
                    {
                        _dbcontext.TblContactRoles.Add(ObjTblContactRole);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        TblContactRole _objTblContactRole = _dbcontext.TblContactRoles.Where(u => u.Description == ObjContactRoleModel.Description && u.TenantId == ObjContactRoleModel.TenantId).FirstOrDefault();
                        if (_objTblContactRole.Status == false)
                            throw new InvalidOperationException("Record already exist.");
                    }
                    ObjContactRoleModel = objNIMBOLEMapper.MapTable2Model(ObjTblContactRole);
                    ObjContactRoleModel.Id = ObjTblContactRole.Id;

                    var data = (from x in _dbcontext.TblContactRoles where x.Status == true && x.TenantId == ObjContactRoleModel.TenantId select new { Id = x.Id, Name = x.Description }).ToList();
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

        [HttpPost]
        [Route("Insert")]
        [ModelValidator]
        public IHttpActionResult Create([FromBody] ContactRoleModel ObjContactRoleModel)
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
                    TblContactRole ObjTblContactRole = objNIMBOLEMapper.MapModel2Table(ObjContactRoleModel);
                    if (!string.IsNullOrEmpty(ObjTblContactRole.Description) || !string.IsNullOrEmpty(ObjTblContactRole.Code))
                    {
                        if (!_dbcontext.TblContactRoles.Any(u => u.Description == ObjContactRoleModel.Description && u.TenantId == ObjContactRoleModel.TenantId))
                        {
                            _dbcontext.TblContactRoles.Add(ObjTblContactRole);
                            _dbcontext.SaveChanges();
                        }
                        else
                        {
                            TblContactRole _objTblContactRole = _dbcontext.TblContactRoles.Where(u => u.Description == ObjContactRoleModel.Description && u.TenantId == ObjContactRoleModel.TenantId).FirstOrDefault();
                            if (_objTblContactRole.Status == false)
                                throw new InvalidOperationException("Record already exist.");
                        }
                    }
                    ObjContactRoleModel = objNIMBOLEMapper.MapTable2Model(ObjTblContactRole);
                    ObjContactRoleModel.Id = ObjTblContactRole.Id;
                    return Ok(ObjContactRoleModel);
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
        public IHttpActionResult Edit([FromBody] ContactRoleModel ObjContactRoleModel)
        {
            try
            {
                if (ObjContactRoleModel.Description != null)
                {
                    TblContactRole record = (from x in _dbcontext.TblContactRoles where x.Id == ObjContactRoleModel.Id select x).FirstOrDefault();
                    if (record == null)
                        throw new InvalidOperationException("Record Not Found");
                    if (record.Description == ObjContactRoleModel.Description)
                    {
                        record.Code = ObjContactRoleModel.RoleCode;
                        record.ModifiedDate = DateTime.Now;
                        record.Status = ObjContactRoleModel.Status;
                        _dbcontext.SaveChanges();
                        return Ok(ObjContactRoleModel);
                    }
                    else
                    {
                        List<TblContactRole> _objContactRole = (from c in _dbcontext.TblContactRoles where c.Description == ObjContactRoleModel.Description && c.TenantId == ObjContactRoleModel.TenantId select c).ToList();

                        if (_objContactRole.Count == 0)
                        {
                            record.Description = ObjContactRoleModel.Description;
                            record.Code = ObjContactRoleModel.RoleCode;
                            record.ModifiedDate = DateTime.Now;
                            record.Status = ObjContactRoleModel.Status;
                            _dbcontext.SaveChanges();
                            return Ok(ObjContactRoleModel);
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
                var query = _dbcontext.TblContactRoles.Where(x => x.Id == id).FirstOrDefault();
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
