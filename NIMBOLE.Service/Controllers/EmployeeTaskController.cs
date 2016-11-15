using NIMBOLE.Entities;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using NIMBOLE.Models;
using System.Web.Http;
using System.Net.Http;
using System.Web.WebPages.Html;
using NIMBOLE.Common;


namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/EmployeeTask")]

    public class EmployeeTaskController : ApiController
    {
        NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        #region GET
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid, int Id =0)
        {
            try
            {
                if (Id != 0)
                {

                    var query = @"select et.Id,et.TenantId,Title,TaskDate,et.TaskOwnerId,et.Comments,e.FirstName,e.LastName,case when e.LastName is not null then e.FirstName + ' ' +  e.LastName else e.FirstName end Name, 
                        Stuff((SELECT ', ' + case when e.LastName is not null then e.FirstName +' '+ e.LastName else e.FirstName end
                        FROM tblEmployee e
                        left join tbltransetask tet
                     on e.id = tet.taskgiventoid
                        where et.id = tet.emptaskid 
                        FOR XML PATH('')),1,1,'') TaskGiven,et.Status from tblEmpTask et 
                        Join tblEmployee e On e.Id = et.TaskOwnerId where e.Status = 1 and TaskOwnerId = " + Id.ToString() +" and et.TenantId='" + Tid.ToString() +"'";

                    List<EmployeeTaskModel> lstEmpTaskModel = _dbcontext.Database.SqlQuery<EmployeeTaskModel>(query).ToList<EmployeeTaskModel>();
                    return Ok(lstEmpTaskModel);
                }
                else
                {

                    var query = @"select et.Id,et.TenantId,Title,TaskDate,et.TaskOwnerId,et.Comments,e.FirstName,e.LastName,case when e.LastName is not null then e.FirstName + ' ' +  e.LastName else e.FirstName end Name,  
                        Stuff((SELECT ', ' + case when e.LastName is not null then e.FirstName +' '+ e.LastName else e.FirstName end
                        FROM tblEmployee e
                        left join tbltransetask tet
                        on e.id = tet.taskgiventoid
                        where et.id = tet.emptaskid 
                        FOR XML PATH('')),1,1,'') TaskGiven,et.Status from tblEmpTask et 
                        Join tblEmployee e On e.Id = et.TaskOwnerId where e.Status = 1 and et.TenantId= '" + Tid.ToString() +"'";

                    List<EmployeeTaskModel> lstEmpTaskModel = _dbcontext.Database.SqlQuery<EmployeeTaskModel>(query).ToList<EmployeeTaskModel>();

                    return Ok(lstEmpTaskModel);
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
        [Route("GetById")]
        public IHttpActionResult GetById(int id,Guid Tid)
        {
            try
            {
                var query = _dbcontext.TblEmpTasks.SingleOrDefault(x => x.Id == id && x.TenantId==Tid);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    EmployeeTaskModel ObjEmployeeTaskModel = objNIMBOLEMapper.MapTable2Model(query);
                    var transQuery = _dbcontext.TblTransETasks.Where(x => x.EmpTaskId == query.Id)
                        .Select(x => new EmpTaskKeyValueModel1 { Id = x.TaskGivenToId, Name = _dbcontext.TblEmployees.Where(e => e.Id == x.TaskGivenToId).Select(y => y.FirstName).FirstOrDefault() })
                        .ToList();
                    ObjEmployeeTaskModel.ReferenceIds = transQuery;
                    return Ok(ObjEmployeeTaskModel);
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

        #region post
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(EmployeeTaskModel ObjEmployeeTaskModel)
        {
            try
            {
                TblTransETask ObjTblTransETask = new TblTransETask();

                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion
                TblEmpTask ObjTblEmpTask = new TblEmpTask();
                ObjTblEmpTask = objNIMBOLEMapper.MapModel2Table(ObjEmployeeTaskModel);
                ObjTblEmpTask.TaskOwnerId = Convert.ToInt64(ObjEmployeeTaskModel.EmpId);
                _dbcontext.TblEmpTasks.Add(ObjTblEmpTask);
                _dbcontext.SaveChanges();
                ObjTblTransETask.EmpTaskId = ObjTblEmpTask.Id;

                foreach (var item in ObjEmployeeTaskModel.ReferenceIds)
                {
                    //ObjTblTransETask.TenantId = ObjEmployeeTaskModel.TenantId.ToDefaultTenantId();
                    ObjTblTransETask.TenantId = ObjEmployeeTaskModel.TenantId;
                    ObjTblTransETask.TaskGivenToId = Convert.ToInt64(item.Id);
                    _dbcontext.TblTransETasks.Add(ObjTblTransETask);
                    _dbcontext.SaveChanges();
                }
                ObjEmployeeTaskModel = objNIMBOLEMapper.MapTable2Model(ObjTblEmpTask);
                return Ok(ObjEmployeeTaskModel);
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
        public IHttpActionResult Edit([FromBody] EmployeeTaskModel ObjEmployeeTaskModel)
        {
            try
            {
                //if (ObjEmployeeTaskModel != null)
                //    //ObjEmployeeTaskModel.TenantId = ObjEmployeeTaskModel.TenantId.ToDefaultTenantId();

                var query = (from x in _dbcontext.TblEmpTasks select x).ToList();
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");

                TblEmpTask ObjTblEmpTask = new TblEmpTask();
                ObjTblEmpTask = query.Where(x => x.Id == ObjEmployeeTaskModel.Id && x.TenantId==ObjEmployeeTaskModel.TenantId ).FirstOrDefault();
                ObjTblEmpTask.Id = ObjEmployeeTaskModel.Id;
                ObjTblEmpTask.TenantId = ObjEmployeeTaskModel.TenantId;
                ObjTblEmpTask.Type_Task = ObjEmployeeTaskModel.Type_Task;
                ObjTblEmpTask.ModifiedDate = ObjEmployeeTaskModel.ModifiedDate;
                ObjTblEmpTask.TaskDate = ObjEmployeeTaskModel.TaskDate;
                ObjTblEmpTask.StartDate = ObjEmployeeTaskModel.StartDate;
                ObjTblEmpTask.EndDate = ObjEmployeeTaskModel.EndDate;
                ObjTblEmpTask.Comments = ObjEmployeeTaskModel.Comments;
                ObjTblEmpTask.Title = ObjEmployeeTaskModel.Title;
                ObjTblEmpTask.Status = true;
                _dbcontext.SaveChanges();

                TblTransETask ObjTblTransETask = new TblTransETask();
                var transQuery = ObjEmployeeTaskModel.ReferenceIds;
                var empTaskIds = _dbcontext.TblTransETasks.Where(x => x.EmpTaskId == ObjEmployeeTaskModel.Id).Select(x => x.Id).ToList();

                for (int i = 0; i < empTaskIds.Count; i++)
                {
                    long tempId = empTaskIds[i];
                    ObjTblTransETask = _dbcontext.TblTransETasks.Where(x => x.Id == tempId).FirstOrDefault();
                    _dbcontext.TblTransETasks.Remove(ObjTblTransETask);
                    _dbcontext.SaveChanges();
                }
                foreach (var item in ObjEmployeeTaskModel.ReferenceIds)
                {
                    ObjTblTransETask.TenantId = ObjEmployeeTaskModel.TenantId;
                    ObjTblTransETask.EmpTaskId = ObjEmployeeTaskModel.Id;
                    ObjTblTransETask.TaskGivenToId= Convert.ToInt64(item.Id);
                    _dbcontext.TblTransETasks.Add(ObjTblTransETask);
                    _dbcontext.SaveChanges();
                }

                ObjEmployeeTaskModel = objNIMBOLEMapper.MapTable2Model(ObjTblEmpTask);
                return Ok(ObjEmployeeTaskModel);
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
                var query = _dbcontext.TblEmpTasks.Where(m => m.Id == id).FirstOrDefault();
                if (query != null)
                {
                    query.Status = status;
                    _dbcontext.SaveChanges();
                    EmployeeTaskModel ObEmployeeTaskModel = objNIMBOLEMapper.MapTable2Model(query);

                    var assignedEmployees = _dbcontext.TblTransETasks.Where(m => m.EmpTaskId == id).ToList();
                    foreach (var item in assignedEmployees)
                    {
                        _dbcontext.TblTransETasks.Remove(item);
                        _dbcontext.SaveChanges();
                    }
                    return Ok(ObEmployeeTaskModel);
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