using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Entities;
namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/EmployeeMasterTask")]
    public class EmployeeMasterTaskController : ApiController
    {
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();

        #region GetMethods
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                List<EmployeeTaskModel> listEmployeeTaskModel = new List<EmployeeTaskModel>();
                var masterTaskList = (from t in _dbcontext.TblMasterTasks where t.TenantId == Tid select t).ToList();
                foreach (var item in masterTaskList)
                {
                    listEmployeeTaskModel.Add(new EmployeeTaskModel() { Id = item.Id, Type = item.Type, Title = item.Title, Comments = item.Comments, Status = Convert.ToBoolean(item.Status) });
                }
                return Ok(listEmployeeTaskModel);
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
                
              Guid TenantId =  ObjEmployeeTaskModel.TenantId;

                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion
                

                foreach (var item in ObjEmployeeTaskModel.SelectedValue)
                {
                    TblEmpTask ObjTblEmpTask = new TblEmpTask();
                    TblTransETask ObjTblTransETask = new TblTransETask();

                    ObjTblEmpTask.TaskOwnerId = Convert.ToInt64(ObjEmployeeTaskModel.EmpId);
                    var data = (from t in _dbcontext.TblMasterTasks where t.Id == item.Id select t).FirstOrDefault();
                    ObjEmployeeTaskModel.Title = data.Title;
                    ObjEmployeeTaskModel.Type = data.Type;
                    ObjEmployeeTaskModel.Comments = data.Comments;
                    ObjTblEmpTask.TenantId = TenantId;
                    ObjTblEmpTask.TaskDate = DateTime.Now;
                    ObjTblEmpTask.CreatedDate = DateTime.Now;
                    ObjTblEmpTask.ModifiedDate = DateTime.Now;
                    ObjTblEmpTask.Status = true;
                    ObjTblEmpTask.Title = ObjEmployeeTaskModel.Title;
                    ObjTblEmpTask.Type_Task = ObjEmployeeTaskModel.Type;
                    ObjTblEmpTask.Comments = ObjEmployeeTaskModel.Comments;
                    ObjTblEmpTask.StartDate = ObjEmployeeTaskModel.StartDate;
                    ObjTblEmpTask.EndDate = ObjEmployeeTaskModel.EndDate;
                    _dbcontext.TblEmpTasks.Add(ObjTblEmpTask);
                    _dbcontext.SaveChanges();
                    ObjTblTransETask.EmpTaskId = ObjTblEmpTask.Id;
                    foreach (var itemva in ObjEmployeeTaskModel.ReferenceIds)
                    {
                        ObjTblTransETask.TenantId = ObjEmployeeTaskModel.TenantId;
                        ObjTblTransETask.TaskGivenToId = Convert.ToInt64(itemva.Id);
                        _dbcontext.TblTransETasks.Add(ObjTblTransETask);
                        _dbcontext.SaveChanges();
                    }
                }
               
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
    }
}
