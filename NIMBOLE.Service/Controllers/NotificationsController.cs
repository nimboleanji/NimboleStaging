using NIMBOLE.Models.Mappers;
using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NIMBOLE.Entities;

namespace NIMBOLE.Service.Controllers
{

    [RoutePrefix("api/Notify")]
    public class NotificationsController : ApiController
    {
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        #region GET
        [HttpGet]
        [Route("GetNotifications")]
        public IHttpActionResult GetNotifications(Guid Tid)
        {
            try
            {
                // sreedhar changed on 16/10/2015
                //var objActivity = (from a in _dbcontext.TblActivities
                //                   where a.Status == true && a.RequireNotify == true && a.ActivityDate >= DateTime.Now
                //                   join
                //                      m in _dbcontext.TblMileStones.Where(m => m.Status == true) on a.MileStoneId equals m.Id
                //                   select new
                //                   {
                //                       a.Id,
                //                       a.Title,
                //                       a.ActivityDate,
                //                       a.Comments,
                //                       a.CreatedDate,
                //                       m.Description,
                //                       a.ModifiedDate,
                //                       a.RequireNotify,
                //                       a.Status,
                //                   }).ToList();

                //List<NotificationModel> activityList = new List<NotificationModel>();
                //foreach (var item in objActivity)
                //{
                //    activityList.Add(new NotificationModel()
                //    {
                //        Id = item.Id,
                //        Title = item.Title,
                //        Descriptions = item.Description,
                //        ActivityComments = item.Comments,
                //        CreatedDate = item.CreatedDate ?? DateTime.Now,
                //        ActivityDate = item.ActivityDate ?? DateTime.Now
                //    });
                //}
                var activityList = (from AM in _dbcontext.VWActivityMileStones.Where(a=>a.TenantId==Tid).OrderByDescending(a=>a.Id)  select AM).ToList();
                
                return Json(activityList);
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
        [Route("GetTasks")]
        public IHttpActionResult GetTasks(Guid Tid)
        {
            try
            {
                var query = _dbcontext.TblEmpTasks.Where(a => a.Status == true && a.TenantId==Tid).OrderByDescending(a => a.TaskDate).Select(a => new EmployeeTaskModel { Id = a.Id, Title = a.Title, TaskDate = a.TaskDate ?? DateTime.Now, Comments = a.Comments }).ToList().Take(4);
                return Json(query);
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
                //sreedhar changed on 16/10/2015                
                //var query = (from a in _dbcontext.TblActivities.Where(a => a.Id == id)
                //             join
                //              m in _dbcontext.TblMileStones.Where(m => m.Status == true) on a.MileStoneId equals m.Id
                //             select new
                //             {
                //                 a.Id,
                //                 a.Title,
                //                 a.ActivityDate,
                //                 a.Comments,
                //                 a.CreatedDate,
                //                 a.MileStoneId,
                //                 a.Type_A_E_M,
                //                 m.Description,
                //                 a.ModifiedDate,
                //                 a.Status
                //             }).FirstOrDefault();

                //ActivityModel objActivityModel = new ActivityModel()
                //{
                //    Id = query.Id,
                //    Title = query.Title,
                //    MileStoneId = query.MileStoneId ?? 1,
                //    Type_A_E_M = query.Type_A_E_M,
                //    ActivityDate = query.ActivityDate ?? DateTime.Now,
                //    ActivityComments = query.Comments,
                //    CreatedDate = query.CreatedDate ?? DateTime.Now,
                //    ModifiedDate = query.ModifiedDate ?? DateTime.Now,
                //    Status = query.Status ?? true,
                //    Descriptions = query.Description
                //};

                var TblActivityModels = (from AM in _dbcontext.VWActivityMileStones where AM.Id == id && AM.TenantId==Tid  select AM).SingleOrDefault();
                return Ok(TblActivityModels);
                //if (TblActivityModels == null)
                //    throw new InvalidOperationException("Record Not Found");
                //else
                //{
                //    ActivityModel objActivityModel = new ActivityModel();
                //    objActivityModel.ActivityComments = TblActivityModels.ActivityComments;
                //    objActivityModel.Title = TblActivityModels.ActivityTitle;
                //    objActivityModel.Descriptions = TblActivityModels.Descriptions;
                //    objActivityModel.ActivityDate = TblActivityModels.ActivityDate ?? DateTime.Now;
                //    objActivityModel.MileStoneId = TblActivityModels.MileStoneId ?? 1;
                //    return Ok(objActivityModel);
                //}
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
