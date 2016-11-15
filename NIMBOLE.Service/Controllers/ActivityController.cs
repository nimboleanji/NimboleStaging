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
using System.Globalization;
using System.Text;
using System.IO;
using System.Net.Security;

using System.Security.Cryptography.X509Certificates;



namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Activity")]
    public class ActivityController : ApiController
    {
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        #region GET's
        // GetAll api/Activity
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {

                //sreedhar changed on 15/10/2015



                //                select TL.ActivityId as Id,TL.LeadId as LeadId,LD.LeadTitle,LD.LeadDescription,LD.LeadOwnerId,LD.AccountId, ACT.Title as ActivityTitle,COALESCE(ACT.ActivityDate,getdate())as ActivityDate ,ACT.Comments as ActivityComments,ACT.MileStoneId,MS.Description as Descriptions
                //from TblMileStone MS with (nolock)
                //join TblActivity ACT with (nolock) on MS.Id=ACT.MilestoneId
                //join TblTransLead TL with (nolock) on ACT.Id=TL.ActivityId
                //join TblLead LD with (nolock) on TL.LeadId=LD.Id
                //where TL.status=1 and MS.status=1 and ACT.status=1 and LD.status=1


                var query = (from ACT in _dbcontext.TblActivities.Where(ACT => ACT.Status == true && ACT.TenantId == Tid)
                             join
                              MS in _dbcontext.TblMileStones.Where(MS => MS.Status == true && MS.TenantId == Tid) on ACT.MileStoneId equals MS.Id
                             join
                             TL in _dbcontext.TblTransLeads.Where(TL => TL.Status == true && TL.TenantId == Tid) on ACT.Id equals TL.ActivityId
                             join
                             LD in _dbcontext.TblLeads.Where(LD => LD.Status == true && LD.TenantId == Tid) on TL.LeadId equals LD.Id

                             select new
                             {
                                 Id = TL.ActivityId,
                                 LeadId = TL.LeadId,
                                 LD.LeadTitle,
                                 LD.LeadDescription,
                                 LD.LeadOwnerId,
                                 LD.AccountId,
                                 ActivityTitle = ACT.Title,
                                 ActivityDate = ACT.ActivityDate,
                                 ActivityComments = ACT.Comments,
                                 ACT.MileStoneId,
                                 Descriptions = MS.Description
                             }).ToList();

                //List<ActivityModel> objActivityModel = new List<ActivityModel>();
                //foreach (var item in query)
                //{
                //    objActivityModel.Add(new ActivityModel() { Id = item.Id, Title = item.Title, ActivityDate = item.ActivityDate ?? DateTime.Now, ActivityComments = item.Comments, Descriptions = item.Description });
                //}
                //return Json(objActivityModel);

                // var query=(from a in _dbcontext.VWActivityMileStones select a).ToList();

                if (query != null)
                    return Json(query);
                else
                    throw new InvalidOperationException("Record not found.");
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }


        // GetAMSAll api/activity
        [HttpGet]
        [Route("GetActivityByMilestone")]
        public IHttpActionResult GetActivityByMilestone(int id, long iLeadId, Guid Tid)
        {
            try
            {
                //sreedhar changed on 15/10/2015

                //List<ActivityByMilestone> lstActivityByMilestone  = (from MS in _dbcontext.TblMileStones
                //             join TA in _dbcontext.TblActivities on MS.Id equals TA.MileStoneId
                //             join TL in _dbcontext.TblTransLeads on TA.Id equals TL.ActivityId
                //             join LD in _dbcontext.TblLeads on TL.LeadId equals LD.Id
                //             where MS.Id == id && TL.LeadId == iLeadId
                //              select new ActivityByMilestone { Id = TL.ActivityId??0, ActivityTitle = TA.Title, ActivityDate = TA.ActivityDate??DateTime.Now}).ToList();

                var lstActivityByMilestone = (from MS in _dbcontext.VWActivityMileStones
                                              join TL in _dbcontext.TblTransLeads on MS.Id equals TL.ActivityId
                                              where MS.MileStoneId == id && MS.LeadId == iLeadId && TL.TenantId == Tid
                                              select new { Id = TL.ActivityId, MS.ActivityTitle, MS.ActivityDate }).ToList();

                if (lstActivityByMilestone == null)
                    throw new InvalidOperationException("Record not found.");
                else
                {
                    return Json(lstActivityByMilestone);
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
        [Route("GetActivityByMilestoneId")]
        public IHttpActionResult GetActivityByMilestoneId(int msId)
        {
            try
            {
                //sreedhar changed on 15/10/2015

                // List<ActivityByMilestone> lstActivityByMilestone =null;
                dynamic lstActivityByMilestone = null;

                if (msId == -1)
                {
                    //lstActivityByMilestone = (from MS in _dbcontext.TblMileStones
                    //                          join ACT in _dbcontext.TblActivities on MS.Id equals ACT.MileStoneId
                    //                          join TL in _dbcontext.TblTransLeads on ACT.Id equals TL.ActivityId
                    //                          join LD in _dbcontext.TblLeads on TL.LeadId equals LD.Id
                    //                          select new ActivityByMilestone { Id = TL.ActivityId ?? 0, ActivityTitle = ACT.Title, ActivityDate = ACT.ActivityDate ??   DateTime.Now }).ToList();

                    //lstActivityByMilestone = (from MS in _dbcontext.VWActivityMileStones  select new { MS.Id, MS.ActivityTitle, MS.ActivityDate }).ToList();

                    lstActivityByMilestone = (from MS in _dbcontext.VWActivityMileStones
                                              join TL in _dbcontext.TblTransLeads on MS.Id equals TL.ActivityId
                                              where MS.MileStoneId == msId
                                              select new { Id = TL.ActivityId, MS.ActivityTitle, MS.ActivityDate }).ToList();

                }
                else
                {
                    //lstActivityByMilestone = (from MS in _dbcontext.TblMileStones
                    //            join ACT in _dbcontext.TblActivities on MS.Id equals ACT.MileStoneId
                    //            join TL in _dbcontext.TblTransLeads on ACT.Id equals TL.ActivityId
                    //            join LD in _dbcontext.TblLeads on TL.LeadId equals LD.Id
                    //            where MS.Id == msId  
                    //            select new ActivityByMilestone { Id = TL.ActivityId ?? 0, ActivityTitle = ACT.Title, ActivityDate = ACT.ActivityDate ?? DateTime.Now }).ToList();

                    //lstActivityByMilestone = (from MS in _dbcontext.VWActivityMileStones where MS.MileStoneId == msId select new { MS.Id, MS.ActivityTitle, MS.ActivityDate }).ToList();

                    lstActivityByMilestone = (from MS in _dbcontext.VWActivityMileStones
                                              join TL in _dbcontext.TblTransLeads on MS.Id equals TL.ActivityId
                                              where MS.MileStoneId == msId
                                              select new { Id = TL.ActivityId, MS.ActivityTitle, MS.ActivityDate }).ToList();
                }
                if (lstActivityByMilestone == null)
                    throw new InvalidOperationException("Record not found.");
                else
                {
                    return Json(lstActivityByMilestone);
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
        [Route("GetNotifyActivityByEmpId")]
        public IHttpActionResult GetNotifyActivityByEmpId(long iEmpId, Guid Tid)
        {
            try
            {
                //sreedhar changed on 15/10/2015

                //var data = (from ACT in _dbcontext.TblActivities 
                //                                        join ACTNTFY in _dbcontext.TblActivityNotifies on ACT.Id equals                                     ACTNTFY.ActivityId
                //                                         join EMP in _dbcontext.TblEmployees on ACTNTFY.EmployeeId equals EMP.Id
                //                                         where ACT.TenantId == ACTNTFY.TenantId 
                //                                         && ACTNTFY.TenantId == EMP.TenantId
                //                                         && EMP.Id == iEmpId
                //                                         //&& ACT.RequireNotify == true
                //                                         && EMP.Status == true  
                //                                         && ACT.Status == true
                //                                         select  ACT
                //                                        ).ToList();


                var _lstNotifyActivityModel = (from ACT in _dbcontext.VWEmpActivityNotifies
                                               where ACT.EmpId == iEmpId && ACT.TenantId == Tid
                                               select new
                                               {
                                                   Id = ACT.Id,
                                                   title = ACT.Title + ACT.Id.ToString(),
                                                   getTimezoneOffset = DateTime.Now,
                                                   start = ACT.ActivityDate.Value.ToLongDateString(),
                                                   end = ACT.ActivityDate.Value.AddMinutes(20).ToLongDateString()
                                               }).ToList();

                //List<NotifyActivityModel> _lstNotifyActivityModel = new List<NotifyActivityModel>();
                //foreach (var item in data)
                //{
                //    _lstNotifyActivityModel.Add(new NotifyActivityModel
                //    {
                //        Id = item.Id,
                //        title = item.Title + item.Id.ToString(),
                //        getTimezoneOffset = DateTime.Now,
                //        start = item.ActivityDate.Value.ToLongDateString(),
                //        end = item.ActivityDate.Value.AddMinutes(20).ToLongDateString()
                //    });
                //}

                if (_lstNotifyActivityModel == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    return Json(_lstNotifyActivityModel);
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
        [Route("SelectAllActivity")]
        public IHttpActionResult SelectAllActivity()
        {
            try
            {
                var query = (from x in _dbcontext.TblActivities select new { Id = x.Id, Name = x.Comments }).ToList();
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
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

        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var query = _dbcontext.TblActivities.SingleOrDefault(x => x.Id == id);
                //var result=AutoMapper.Mapper.Map<TblActivity, NIMBOLE.Entities.Models.ActivityModel>(query);
                var result = new ActivityModel
                {
                    Id = query.Id,
                    TenantId = query.TenantId,
                    MileStoneId = query.MileStoneId ?? 1,
                    Title = query.Title,
                    Type_A_E_M = query.Type_A_E_M,
                    ActivityDate = query.ActivityDate ?? DateTime.Now,
                    ActivityComments = query.Comments,
                    RequireNotify = query.RequireNotify ?? false,
                    ReferenceId = query.ReferenceId,
                    CreatedDate = query.CreatedDate ?? DateTime.Now,
                    ModifiedDate = query.ModifiedDate ?? DateTime.Now,
                    Status = query.Status ?? true
                };
                if (query == null)
                    throw new InvalidOperationException("Record Not Found.");
                else
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

        [HttpGet]
        [Route("GetNotifications")]
        public IHttpActionResult GetNotifications(Guid Tid)
        {
            try
            {
                var objActivity = (from x in _dbcontext.TblActivities where x.Status == true && x.TenantId == Tid && x.RequireNotify == true && x.ActivityDate >= DateTime.Now select x).ToList();

                List<ActivityModel> activityList = new List<ActivityModel>();
                foreach (var item in objActivity)
                {
                    activityList.Add(new ActivityModel() { Title = item.Title, ActivityComments = item.Comments, CreatedDate = Convert.ToDateTime(item.CreatedDate) });
                }
                if (activityList.Count > 0)
                    return Json(activityList);
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
        [Route("GetLatLogActivityByEmpId")]
        public IHttpActionResult GetLatLogActivityByEmpId(int iEmpId, string FromDt, Guid Tid)
        {
            try
            {


                var strQuerywithLead = string.Empty;
                var strQuerywithoutLead = string.Empty;


                if (iEmpId > 0)
                {

                    strQuerywithLead = "select t.*,l.LeadTitle from tbllatlog t left outer join  tbllead l on t.LeadId=l.Id where t.TenantId='" + Tid + "' and convert(varchar(10),t.postdatetime, 112) = convert(varchar(10),convert(datetime, '" + FromDt + "'), 112) and t.empid=" + iEmpId + " and t.LeadId > 0";

                    strQuerywithoutLead = "select t.*,l.LeadTitle from tbllatlog t left outer join  tbllead l on t.LeadId=l.Id where t.TenantId='" + Tid + "' and convert(varchar(10),t.postdatetime, 112) = convert(varchar(10),convert(datetime, '" + FromDt + "'), 112) and t.empid=" + iEmpId + " and t.LeadId = 0";

                }


                var lstObjMapLatLogModel = _dbcontext.Database.SqlQuery<MapLatLogModel>(strQuerywithLead).ToList<MapLatLogModel>();

                var lstObjMapLatLogModelTravel = _dbcontext.Database.SqlQuery<MapLatLogModel>(strQuerywithoutLead).ToList<MapLatLogModel>();


                List<MapLatLogModel> lstMapLatLogModel = new List<MapLatLogModel>();

                int i, j, n = 0;


                if (lstObjMapLatLogModel != null)
                {

                    foreach (var item in lstObjMapLatLogModel)
                    {
                        lstMapLatLogModel.Add(new MapLatLogModel() { Id = item.Id, TenantId = item.TenantId, Latitude = item.Latitude, Longitude = item.Longitude, EmpId = item.EmpId, ActivityId = item.ActivityId, LeadId = item.LeadId, LeadTitle = item.LeadTitle });
                    }
                }

                if (lstObjMapLatLogModelTravel != null)
                {
                    double lat1 = Convert.ToDouble(lstObjMapLatLogModelTravel[0].Latitude.ToString());
                    double lng1 = Convert.ToDouble(lstObjMapLatLogModelTravel[0].Longitude.ToString());

                    double lat2 = Convert.ToDouble(lstObjMapLatLogModelTravel[0].Latitude.ToString());
                    double lng2 = Convert.ToDouble(lstObjMapLatLogModelTravel[0].Longitude.ToString());

                    lstMapLatLogModel.Add(new MapLatLogModel() { Id = lstObjMapLatLogModelTravel[0].Id, TenantId = lstObjMapLatLogModelTravel[0].TenantId, Latitude = lstObjMapLatLogModelTravel[0].Latitude, Longitude = lstObjMapLatLogModelTravel[0].Longitude, EmpId = lstObjMapLatLogModelTravel[0].EmpId, ActivityId = lstObjMapLatLogModelTravel[0].ActivityId, LeadId = lstObjMapLatLogModelTravel[0].LeadId, LeadTitle = lstObjMapLatLogModelTravel[0].LeadTitle });


                    int cnt = lstObjMapLatLogModelTravel.Count - 1;

                    for (i = n; i < lstObjMapLatLogModelTravel.Count - 1; i++)
                    {

                        lat1 = Convert.ToDouble(lstObjMapLatLogModelTravel[i].Latitude.ToString());
                        lng1 = Convert.ToDouble(lstObjMapLatLogModelTravel[i].Longitude.ToString());

                        for (j = i + 1; j < lstObjMapLatLogModelTravel.Count - 1; j++)
                        {
                            lat2 = Convert.ToDouble(lstObjMapLatLogModelTravel[j].Latitude.ToString());
                            lng2 = Convert.ToDouble(lstObjMapLatLogModelTravel[j].Longitude.ToString());

                            if (distance(lat1, lng1, lat2, lng2, 'M') > 0.05)
                            {
                                lstMapLatLogModel.Add(new MapLatLogModel() { Id = lstObjMapLatLogModelTravel[i].Id, TenantId = lstObjMapLatLogModelTravel[i].TenantId, Latitude = lstObjMapLatLogModelTravel[i].Latitude, Longitude = lstObjMapLatLogModelTravel[i].Longitude, EmpId = lstObjMapLatLogModelTravel[i].EmpId, ActivityId = lstObjMapLatLogModelTravel[i].ActivityId, LeadId = lstObjMapLatLogModelTravel[i].LeadId, LeadTitle = lstObjMapLatLogModelTravel[i].LeadTitle });

                                break;
                            }
                            else
                            {
                                n = n + 1;
                            }
                        }
                    }

                    lstMapLatLogModel.Add(new MapLatLogModel() { Id = lstObjMapLatLogModelTravel[cnt].Id, TenantId = lstObjMapLatLogModelTravel[cnt].TenantId, Latitude = lstObjMapLatLogModelTravel[cnt].Latitude, Longitude = lstObjMapLatLogModelTravel[cnt].Longitude, EmpId = lstObjMapLatLogModelTravel[cnt].EmpId, ActivityId = lstObjMapLatLogModelTravel[cnt].ActivityId, LeadId = lstObjMapLatLogModelTravel[cnt].LeadId, LeadTitle = lstObjMapLatLogModelTravel[cnt].LeadTitle });
                }





                if (lstObjMapLatLogModel == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    //return Json(lstObjMapLatLogModel);
                    return Json(lstMapLatLogModel.OrderBy(l => l.Id));
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


        private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) +
                          Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
                          Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'M')
            {
                dist = dist * 1.609344;
                dist = dist / 1000;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }

            return (dist);
        }

        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }


        private DateTime ConvertToDateTime(string strDateTime)
        {
            DateTime dtFinaldate; string sDateTime;
            try { dtFinaldate = Convert.ToDateTime(strDateTime); }
            catch (Exception e)
            {
                string[] sDate = strDateTime.Split('/');
                sDateTime = sDate[1] + '/' + sDate[0] + '/' + sDate[2];
                dtFinaldate = Convert.ToDateTime(sDateTime);
            }
            return dtFinaldate;
        }




        #endregion

        #region POST
        [HttpPost]
        [Route("MobileInsert")]
        [ModelValidator]
        public IHttpActionResult MobileInsert([FromBody] MobileActivityModel _objMyActivity)
        {
            try
            {

                ActivityModel _objActivityModel = _objMyActivity.objActivityModel;
                string[] ReferenceIds = _objMyActivity.refIds;
                List<string> urls = _objMyActivity.lstURLs;

                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                TblActivity objTblActivity = objNIMBOLEMapper.MapModel2Table(_objActivityModel);


                _dbcontext.TblActivities.Add(objTblActivity);
                _dbcontext.SaveChanges();

                if (objTblActivity.Id > 0)
                {
                    TblTransLead objTblTransLead = new TblTransLead();
                    objTblTransLead.ActivityId = objTblActivity.Id;
                    objTblTransLead.LeadId = _objMyActivity.LeadId;
                    objTblTransLead.TenantId = objTblActivity.TenantId;
                    objTblTransLead.CreatedDate = DateTime.Now;
                    objTblTransLead.ModifiedDate = DateTime.Now;
                    objTblTransLead.Status = true;
                    _dbcontext.TblTransLeads.Add(objTblTransLead);
                    _dbcontext.SaveChanges();

                    if (ReferenceIds != null && ReferenceIds.Count() > 0)
                    {
                        foreach (string item in ReferenceIds)
                        {
                            TblActivityNotify objTblActivityNotify = new TblActivityNotify();
                            objTblActivityNotify.TenantId = objTblActivity.TenantId;
                            objTblActivityNotify.ActivityId = objTblActivity.Id;
                            objTblActivityNotify.EmployeeId = Convert.ToInt64(item);
                            _dbcontext.TblActivityNotifies.Add(objTblActivityNotify);
                            _dbcontext.SaveChanges();
                        }
                    }

                    if (urls != null)
                    {
                        foreach (var item in urls)
                        {
                            TblDocument objTblDocument = new TblDocument();
                            objTblDocument.TenantId = objTblActivity.TenantId;
                            objTblDocument.URL = item;
                            objTblDocument.DocumentName = objTblActivity.Title;
                            string CurrentUser = _objMyActivity.currentUser;
                            var CurrentUserId = _dbcontext.TblEmployees.Where(a => a.EmployeeEmail == CurrentUser).Select(a => a.Id).FirstOrDefault();
                            objTblDocument.UploadedById = CurrentUserId;
                            objTblDocument.UploadDateTime = DateTime.Now;
                            objTblDocument.DocumentType = objTblDocument.DocumentType;
                            objTblDocument.SizeOfDocument = 0;
                            objTblDocument.CreatedDate = DateTime.Now;
                            objTblDocument.ModifiedDate = DateTime.Now;
                            objTblDocument.Status = true;
                            _dbcontext.TblDocuments.Add(objTblDocument);
                            _dbcontext.SaveChanges();

                            TblTransDocument objTblTransDocument = new TblTransDocument();
                            objTblTransDocument.TenantId = objTblActivity.TenantId;
                            objTblTransDocument.DocumentId = objTblDocument.Id;
                            objTblTransDocument.ActivityId = objTblActivity.Id;
                            objTblTransDocument.LeadId = _objMyActivity.LeadId;
                            objTblTransDocument.CreatedDate = DateTime.Now;
                            objTblTransDocument.ModifiedDate = DateTime.Now;
                            objTblTransDocument.Status = true;
                            _dbcontext.TblTransDocuments.Add(objTblTransDocument);
                            _dbcontext.SaveChanges();
                        }
                    }
                }

                return Ok(objTblActivity.Id);
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
        [Route("MobileInsertLatLog")]
        [ModelValidator]
        public IHttpActionResult MobileInsertLatLog([FromBody] MobileActivityModel _objMyActivity)
        {
            try
            {

                ActivityModel _objActivityModel = _objMyActivity.objActivityModel;
                string[] ReferenceIds = _objMyActivity.refIds;
                List<string> urls = _objMyActivity.lstURLs;

                //HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                //#region If ModelState is not Valid
                //if (response.StatusCode != HttpStatusCode.OK)
                //    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                //#endregion

                TblActivity objTblActivity = objNIMBOLEMapper.MapModel2Table(_objActivityModel);


                _dbcontext.TblActivities.Add(objTblActivity);
                _dbcontext.SaveChanges();

                if (objTblActivity.Id > 0)
                {
                    TblTransLead objTblTransLead = new TblTransLead();
                    objTblTransLead.ActivityId = objTblActivity.Id;
                    objTblTransLead.LeadId = _objMyActivity.LeadId;
                    objTblTransLead.TenantId = objTblActivity.TenantId;
                    objTblTransLead.CreatedDate = DateTime.Now;
                    objTblTransLead.ModifiedDate = DateTime.Now;
                    objTblTransLead.Status = true;
                    _dbcontext.TblTransLeads.Add(objTblTransLead);
                    _dbcontext.SaveChanges();

                    if (ReferenceIds != null && ReferenceIds.Count() > 0)
                    {
                        foreach (string item in ReferenceIds)
                        {
                            TblActivityNotify objTblActivityNotify = new TblActivityNotify();
                            objTblActivityNotify.TenantId = objTblActivity.TenantId;
                            objTblActivityNotify.ActivityId = objTblActivity.Id;
                            objTblActivityNotify.EmployeeId = Convert.ToInt64(item);
                            _dbcontext.TblActivityNotifies.Add(objTblActivityNotify);
                            _dbcontext.SaveChanges();
                        }
                    }

                    if (urls != null)
                    {
                        foreach (var item in urls)
                        {
                            TblDocument objTblDocument = new TblDocument();
                            objTblDocument.TenantId = objTblActivity.TenantId;
                            objTblDocument.URL = item;
                            objTblDocument.DocumentName = objTblActivity.Title;
                            string CurrentUser = _objMyActivity.currentUser;
                            var CurrentUserId = _dbcontext.TblEmployees.Where(a => a.EmployeeEmail == CurrentUser).Select(a => a.Id).FirstOrDefault();
                            objTblDocument.UploadedById = CurrentUserId;
                            objTblDocument.UploadDateTime = DateTime.Now;
                            objTblDocument.DocumentType = objTblDocument.DocumentType;
                            objTblDocument.SizeOfDocument = 0;
                            objTblDocument.CreatedDate = DateTime.Now;
                            objTblDocument.ModifiedDate = DateTime.Now;
                            objTblDocument.Status = true;
                            _dbcontext.TblDocuments.Add(objTblDocument);
                            _dbcontext.SaveChanges();

                            TblTransDocument objTblTransDocument = new TblTransDocument();
                            objTblTransDocument.TenantId = objTblActivity.TenantId;
                            objTblTransDocument.DocumentId = objTblDocument.Id;
                            objTblTransDocument.ActivityId = objTblActivity.Id;
                            objTblTransDocument.LeadId = _objMyActivity.LeadId;
                            objTblTransDocument.CreatedDate = DateTime.Now;
                            objTblTransDocument.ModifiedDate = DateTime.Now;
                            objTblTransDocument.Status = true;
                            _dbcontext.TblTransDocuments.Add(objTblTransDocument);
                            _dbcontext.SaveChanges();
                        }
                    }

                    TblLatLog objTblLatLog = new TblLatLog();

                    objTblLatLog.ActivityId = objTblActivity.Id;
                    objTblLatLog.TenantId = objTblActivity.TenantId;
                    objTblLatLog.Latitude = _objMyActivity.Latitude;
                    objTblLatLog.Longitude = _objMyActivity.Longitude;
                    objTblLatLog.LeadId = _objMyActivity.LeadId;
                    objTblLatLog.PostDateTime = DateTime.Now;
                    objTblLatLog.EmpId = _objMyActivity.EmpId;

                    _dbcontext.TblLatLogs.Add(objTblLatLog);
                    _dbcontext.SaveChanges();

                }

                return Ok(objTblActivity.Id);
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
        [Route("LatLogInsert")]
        [ModelValidator]
        public IHttpActionResult LatLogInsert([FromBody] MobileLatLogModel _MobileLatLogModel)
        {
            try
            {


                TblLatLog objTblLatLog = new TblLatLog();

                objTblLatLog.TenantId = _MobileLatLogModel.TenantId;
                objTblLatLog.Latitude = _MobileLatLogModel.Latitude;
                objTblLatLog.Longitude = _MobileLatLogModel.Longitude;
                objTblLatLog.ActivityId = 0;
                objTblLatLog.LeadId = 0;
                objTblLatLog.PostDateTime = DateTime.Now;
                objTblLatLog.EmpId = _MobileLatLogModel.EmpId;

                _dbcontext.TblLatLogs.Add(objTblLatLog);
                _dbcontext.SaveChanges();

                return Ok(objTblLatLog.Id);
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
        public IHttpActionResult Insert([FromBody]MyActivityModel _objMyActivity)
        {
            try
            {
                LeadModel _objLeadModel = _objMyActivity.objLeadModel;
                ActivityModel _objActivityModel = _objLeadModel.objActivityModel;
                _objActivityModel.TenantId = _objMyActivity.TenantId;
                string[] ReferenceIds = _objMyActivity.refIds;
                List<string> urls = _objMyActivity.lstURLs;

                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                TblActivity objTblActivity = objNIMBOLEMapper.MapModel2Table(_objActivityModel);

                if (!_dbcontext.TblActivities.Any(u => u.Title == objTblActivity.Title && u.MileStoneId == objTblActivity.MileStoneId && u.Status == true && u.TenantId == _objMyActivity.TenantId))
                {
                    _dbcontext.TblActivities.Add(objTblActivity);
                    _dbcontext.SaveChanges();

                    if (objTblActivity.Id > 0)
                    {
                        TblTransLead objTblTransLead = new TblTransLead();
                        objTblTransLead.ActivityId = objTblActivity.Id;
                        objTblTransLead.LeadId = _objLeadModel.Id;
                        objTblTransLead.TenantId = objTblActivity.TenantId;
                        objTblTransLead.CreatedDate = DateTime.Now;
                        objTblTransLead.ModifiedDate = DateTime.Now;
                        objTblTransLead.Status = true;
                        _dbcontext.TblTransLeads.Add(objTblTransLead);
                        _dbcontext.SaveChanges();

                        if (ReferenceIds != null && ReferenceIds.Count() > 0)
                        {
                            foreach (string item in ReferenceIds)
                            {
                                TblActivityNotify objTblActivityNotify = new TblActivityNotify();
                                objTblActivityNotify.TenantId = objTblActivity.TenantId;
                                objTblActivityNotify.ActivityId = objTblActivity.Id;
                                objTblActivityNotify.EmployeeId = Convert.ToInt64(item);
                                _dbcontext.TblActivityNotifies.Add(objTblActivityNotify);
                                _dbcontext.SaveChanges();
                            }
                        }

                        if (urls != null)
                        {
                            foreach (var item in urls)
                            {
                                TblDocument objTblDocument = new TblDocument();
                                objTblDocument.TenantId = objTblActivity.TenantId;
                                objTblDocument.URL = item;
                                objTblDocument.DocumentName = objTblActivity.Title;
                                string CurrentUser = _objMyActivity.currentUser;
                                var CurrentUserId = _dbcontext.TblEmployees.Where(a => a.EmployeeEmail == CurrentUser && a.TenantId == _objMyActivity.TenantId).Select(a => a.Id).FirstOrDefault();
                                objTblDocument.UploadedById = CurrentUserId;
                                objTblDocument.UploadDateTime = DateTime.Now;
                                objTblDocument.DocumentType = objTblDocument.DocumentType;
                                objTblDocument.SizeOfDocument = 0;
                                objTblDocument.CreatedDate = DateTime.Now;
                                objTblDocument.ModifiedDate = DateTime.Now;
                                objTblDocument.Status = true;
                                _dbcontext.TblDocuments.Add(objTblDocument);
                                _dbcontext.SaveChanges();

                                TblTransDocument objTblTransDocument = new TblTransDocument();
                                objTblTransDocument.TenantId = objTblActivity.TenantId;
                                objTblTransDocument.DocumentId = objTblDocument.Id;
                                objTblTransDocument.ActivityId = objTblActivity.Id;
                                objTblTransDocument.LeadId = _objLeadModel.Id;
                                objTblTransDocument.CreatedDate = DateTime.Now;
                                objTblTransDocument.ModifiedDate = DateTime.Now;
                                objTblTransDocument.Status = true;
                                _dbcontext.TblTransDocuments.Add(objTblTransDocument);
                                _dbcontext.SaveChanges();
                            }
                        }
                    }
                    string message = "Push Notification Checking";

                    List<string> GcmIds = new List<string>();
                    if (ReferenceIds != null && ReferenceIds.Count() > 0)
                    {
                        foreach (string item in ReferenceIds)
                        {
                            var EmpId = Convert.ToInt64(item);
                            var gcmid = _dbcontext.TblEmployees.Where(e => e.Id.Equals(EmpId)).Select(k => k.GcmId).FirstOrDefault();
                            GcmIds.Add(gcmid);
                        }
                        SendNotification(message, GcmIds);
                    }
                }
                return Ok(objTblActivity.Id);
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
        
        public List<string> SendNotification(string message, List<string> GcmIds)
        {
            List<string> ObjActivitys = new List<string>();
            foreach (var GCMRegId in GcmIds)
            {
                if (GCMRegId != null)
                {
                    string SERVER_API_KEY = "AIzaSyBlCATVBrtWUaC8OQrjlhSeQs1FTgeDn_k";
                    var SENDER_ID = "824171086081";
                    var value = message;
                    WebRequest tRequest;
                    tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
                    tRequest.Method = "post";
                    tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));
                    tRequest.ContentType = "application/json";
                    var data = new
                    {
                        to = GCMRegId,
                        notification = new
                        {
                            body =  message,
                            title = "This is the title",
                            icon = "myicon"
                        }
                    };
                    //var serializer = new javascriptserializer();
                    var json = JsonConvert.SerializeObject(data);
                    byte[] bytearray = Encoding.UTF8.GetBytes(json);

                    tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

                    string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + GcmIds + "";
                    Console.WriteLine(postData);
                    Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    tRequest.ContentLength = byteArray.Length;

                    Stream dataStream = tRequest.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    WebResponse tResponse = tRequest.GetResponse();

                    dataStream = tResponse.GetResponseStream();

                    StreamReader tReader = new StreamReader(dataStream);

                    String sResponseFromServer = tReader.ReadToEnd();
                    tReader.Close();
                    dataStream.Close();
                    tResponse.Close();
                    ObjActivitys.Add(sResponseFromServer);
                    //return sResponseFromServer;
                }
            }
            return ObjActivitys;
        }
        #region PUT
        [HttpPut]
        [Route("Edit")]
        public IHttpActionResult Edit([FromBody] TblActivity entity)
        {
            try
            {
                var queries = (from x in _dbcontext.TblActivities select x).ToList();
                if (queries != null)
                {
                    TblActivity query = new TblActivity();
                    query = queries.Where(x => x.TenantId == entity.TenantId && x.Id == entity.Id).FirstOrDefault();
                    query.Title = entity.Title;
                    query.Comments = entity.Comments;
                    query.ActivityDate = entity.ActivityDate;
                    query.Comments = entity.Comments;
                    query.RequireNotify = entity.RequireNotify;
                    query.ReferenceId = entity.ReferenceId;
                    query.Type_A_E_M = entity.Type_A_E_M;
                    query.CreatedDate = entity.CreatedDate;
                    query.ModifiedDate = entity.ModifiedDate;
                    query.Status = entity.Status;
                    _dbcontext.SaveChanges();
                    return Ok(entity);
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

        [HttpPut]
        [Route("UpdateStatus")]
        public IHttpActionResult UpdateStatus([FromBody] TblActivity entity, long Id, bool Status)
        {
            try
            {
                var record = (from x in _dbcontext.TblActivities where x.Id == Id && x.TenantId == entity.TenantId select x).FirstOrDefault();
                if (record != null)
                {
                    record.Status = Status;
                    _dbcontext.SaveChanges();
                    return Ok(entity);
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

        #region DELETE
        [HttpDelete]
        [Route("DeleteById")]
        public IHttpActionResult DeleteById(long id)
        {
            try
            {
                var query = _dbcontext.TblActivities.Where(x => x.Id == id).FirstOrDefault();
                if (query != null)
                {
                    _dbcontext.TblActivities.Remove(query);
                    _dbcontext.SaveChanges();
                    return Ok(query);
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

        #region ActivityUrlDelaete
        [HttpDelete]
        [Route("Deleteurlactivity")]
        public IHttpActionResult Deleteurlactivity(int id, Guid Tid)
        {
            try
            {

                var query = (from d in _dbcontext.TblDocuments where d.Id == id && d.TenantId == Tid select d).FirstOrDefault();
                if (query != null)
                {
                    _dbcontext.TblDocuments.Remove(query);
                    _dbcontext.SaveChanges();
                    return Ok(query);
                }
                else
                {
                    throw new InvalidOperationException("Record not Found");
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
