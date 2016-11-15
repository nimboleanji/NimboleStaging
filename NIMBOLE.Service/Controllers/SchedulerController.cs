using NIMBOLE.Entities;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Net;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Scheduler")]
    public class SchedulerController : ApiController
    {
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        [HttpGet]
        [Route("GetActivityEmpTaskByEmpId")]
        public IHttpActionResult GetActivityEmpTaskByEmpId(long iEmpId,Guid Tid)
        {
            try
            {
                List<SchedulerServiceModel> lstEmployeeTaskModel = new List<SchedulerServiceModel>();
                #region Read Activities
                var data = (from ACT in _dbcontext.TblActivities
                            join ACTNTFY in _dbcontext.TblActivityNotifies on ACT.Id equals ACTNTFY.ActivityId
                            join EMP in _dbcontext.TblEmployees on ACTNTFY.EmployeeId equals EMP.Id
                            where ACT.TenantId == ACTNTFY.TenantId
                            && ACT.TenantId==Tid                            
                            && ACTNTFY.TenantId == EMP.TenantId
                            && EMP.Id == iEmpId
                            && EMP.TenantId==Tid
                            && EMP.Status == true
                            && ACT.Status == true
                            select ACT).ToList();

                foreach (var item in data)
                {
                    lstEmployeeTaskModel.Add(new SchedulerServiceModel
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Start = item.ActivityDate.Value,
                        End = item.ActivityDate.Value.AddMinutes(20),
                        Description = item.Comments,
                        EmpId = "0",
                        mstart = item.ActivityDate.Value.ToString(),
                        mend = Convert.ToString(item.ActivityDate.Value),
                        IsActivity = true
                    });
                }
                #endregion

                #region EmployeeTaskModel
                var lstTblEmployeeTask = _dbcontext
                                        .TblEmpTasks
                                        .Where(et => et.Status.Value == true && et.TenantId==Tid && et.TaskOwnerId==iEmpId)
                                        .Select(et => et).AsEnumerable();

                foreach (var item in lstTblEmployeeTask)
                {
                    lstEmployeeTaskModel.Add(new SchedulerServiceModel
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Start = item.StartDate.Value,
                        End = item.EndDate.Value.AddMinutes(20),
                        Description = item.Comments,
                        EmpId = item.TaskOwnerId.ToString(),
                        mstart = item.StartDate.Value.ToString(),
                        mend = Convert.ToString(item.EndDate.Value),

                        IsActivity = false
                    });
                }
                #endregion
                return Json(lstEmployeeTaskModel.AsQueryable());
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("UpdateTask")]
        public IHttpActionResult UpdateTask([FromBody] SchedulerServiceModel ObjSchedulerServiceModel)
        {
            try
            {
                var query = (from x in _dbcontext.TblEmpTasks select x).ToList();
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");

                TblEmpTask ObjTblEmpTask = new TblEmpTask();
                ObjTblEmpTask = query.Where(x => x.Id == ObjSchedulerServiceModel.Id && x.TenantId==ObjSchedulerServiceModel.TenantId).FirstOrDefault();

                ObjTblEmpTask.Id = ObjSchedulerServiceModel.Id;
                ObjTblEmpTask.TenantId = ObjSchedulerServiceModel.TenantId;
                ObjTblEmpTask.Title = ObjSchedulerServiceModel.Title;
                ObjTblEmpTask.TaskDate = ObjSchedulerServiceModel.Start.Date;
                ObjTblEmpTask.StartDate = ObjSchedulerServiceModel.Start.Date;
                ObjTblEmpTask.EndDate = ObjSchedulerServiceModel.End.Date.AddMinutes(20);
                ObjTblEmpTask.Comments = ObjSchedulerServiceModel.Description;
                _dbcontext.SaveChanges();

                return Ok(ObjSchedulerServiceModel);

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {

            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return origin.AddSeconds(timestamp);

        }



    }
}