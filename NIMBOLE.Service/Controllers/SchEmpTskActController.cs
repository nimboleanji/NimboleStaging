using NIMBOLE.Entities;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using NIMBOLE.Common;
using System.Net.Http;
using System.Net;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/SchEmpTskAct")]
    public class SchEmpTskActController : ApiController
    {
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        [HttpGet]
        [Route("GetActivityEmpTaskByEmpId")]
        public IHttpActionResult GetActivityEmpTaskByEmpId(long iEmpId)
        {
            try
            {
                List<EmployeeTaskModel> lstEmployeeTaskModel = new List<EmployeeTaskModel>();
                #region Read Activities
                var data = (from ACT in _dbcontext.TblActivities
                            join ACTNTFY in _dbcontext.TblActivityNotifies on ACT.Id equals ACTNTFY.ActivityId
                            join EMP in _dbcontext.TblEmployees on ACTNTFY.EmployeeId equals EMP.Id
                            where ACT.TenantId == ACTNTFY.TenantId
                            && ACTNTFY.TenantId == EMP.TenantId
                            && EMP.Id == iEmpId
                            && EMP.Status == true
                            && ACT.Status == true
                            select ACT).ToList();
                foreach (var item in data)
                {
                    lstEmployeeTaskModel.Add(new EmployeeTaskModel
                    {
                        Id = item.Id,
                        Title = item.Title + item.Id.ToString(),
                        TaskDate = item.ActivityDate.Value,
                        StartDate = item.ActivityDate.Value,
                        EndDate = item.ActivityDate.Value.AddMinutes(20),
                        IsActivity = true
                    });
                }
                #endregion

                #region EmployeeTaskModel
                var lstTblEmployeeTask = _dbcontext
                                        .TblEmpTasks
                                        .Where(et => et.Status.Value == true)
                                        .Select(et => et).AsEnumerable();

                foreach (var item in lstTblEmployeeTask)
                {
                    lstEmployeeTaskModel.Add(new EmployeeTaskModel
                    {
                        Id = item.Id,
                        Title = item.Title + item.Id.ToString(),
                        TaskDate = item.TaskDate.Value,
                        StartDate = item.StartDate.Value,
                        EndDate = item.EndDate.Value.AddMinutes(20),
                        IsActivity = false
                    });
                }
                #endregion
                return Json(lstEmployeeTaskModel);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }            
        }

        #region post
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult create(SchedulerServiceModel objSchedulerServiceModel)
        {
            TblEmpTask objTblEmpTask = new TblEmpTask();
            //Guid tenantId = new Guid();
            //objTblEmpTask.TenantId = tenantId.ToDefaultTenantId();
            objTblEmpTask.TenantId=objSchedulerServiceModel.TenantId;
            objTblEmpTask.TaskOwnerId = Convert.ToInt64(objSchedulerServiceModel.EmpId);
            objTblEmpTask.Title = objSchedulerServiceModel.Title;
            objTblEmpTask.StartDate = objSchedulerServiceModel.Start;
            objTblEmpTask.EndDate = objSchedulerServiceModel.End;
            objTblEmpTask.TaskDate = DateTime.Now;
            objTblEmpTask.Comments = objSchedulerServiceModel.Description;
            objTblEmpTask.CreatedDate = DateTime.Now;
            objTblEmpTask.ModifiedDate = DateTime.Now;
            objTblEmpTask.Status = true;
            _dbcontext.TblEmpTasks.Add(objTblEmpTask);
            _dbcontext.SaveChanges();
            return Ok(objTblEmpTask);
        }
        #endregion 

        #region DELETE
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                
                var query = _dbcontext.TblEmpTasks.Where(m => m.Id == id ).FirstOrDefault();
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                query.Status = false;
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