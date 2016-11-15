using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.Models.Models;
using System.Web.Security;
using System.Configuration;
using NIMBOLE.Models;
using AutoMapper;
using System.Net.Http;
using System.Net.Http.Headers;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Diagnostics;
using NIMBOLE.Common;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR;

namespace NIMBOLE.UI.Controllers
{
    public class ActivityController : BaseController
    {
        Guid TenentId = new Guid();
        public ActivityController()
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                    TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }

        #region GET
        [Route("GetReferenceIds")]
        public ActionResult GetReferenceIds([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Employees/GetReferenceIds?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var references = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.KeyValueModel>>().Result;
                return Json(references, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult Index()
        {
            return View("~/Views/Shared/Activity/_Create.cshtml");
        }

        public ActionResult ActivityByMilestone_Read([DataSourceRequest] DataSourceRequest request, int id = 0, long leadId = 0)
        {
            try
            {
                if (leadId > 0)
                {
                    // string strLeadId = Session["CurrentLeadId"].ToString();
                    response = client.GetAsync("api/Activity/GetActivityByMilestone?id=" + id + "&iLeadId=" + leadId + "&Tid=" + TenentId).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var lstData = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.ActivityByMilestone>>().Result;

                        return Json(lstData.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(null);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult Activity_Read([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Activity/GetAll?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var activities = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.ActivityModel>>().Result;
                return Json(activities.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            else
                return RedirectToAction("Create");
        }

        [HttpGet]
        public ActionResult GetAllMileStones()
        {
            try
            {
                response = client.GetAsync("api/MileStones/SelectAllMileStone?Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objMileStoneModel = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.MilestoneModel>>().Result;
                    return Json(objMileStoneModel, JsonRequestBehavior.AllowGet);
                }
                return Json("Error1: " + response.RequestMessage.Content);
            }
            catch (Exception ex)
            {
                return Json("Error2: " + ex.Message);
            }
        }

        [HttpPost]
        public JsonResult GetNotifyActivityByEmpId([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeId"] != null)
                {
                    string strEmpId = System.Web.HttpContext.Current.Session["EmployeeId"].ToString();
                    response = client.GetAsync("api/Activity/GetNotifyActivityByEmpId?iEmpId=" + strEmpId + "&Tid=" + TenentId).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var lstNotifyActivity = response.Content.ReadAsAsync<List<NotifyActivityModel>>().Result;
                        return Json(lstNotifyActivity.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    }
                    return Json("Error1: " + response.RequestMessage.Content);
                }
                else
                    return Json("Error: Not Loggin Properly");
            }
            catch (Exception ex)
            {
                return Json("Error2: " + ex.Message);
            }
        }
        [HttpGet]
        public JsonResult GetNotifications([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Activity/GetNotifications?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objActivityModel = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.ActivityModel>>().Result;
                return Json(objActivityModel, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        #endregion

        public ActionResult LaunchPartialWindow()
        {
            return PartialView("~/Views/Shared/Activity/_Create.cshtml");
        }

        #region CREATE
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("~/Views/Shared/Activity/_Create.cshtml");
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, LeadModel objLeadModel)
        {
            try
            {
                string page = "Lead";
                long iLeadId = Convert.ToInt64(Session["CurrentLeadId"].ToString());
                string[] ReferenceIds = null;
                if (Request.Form["objActivityModel.ReferenceIds"] != null)
                    ReferenceIds = Request.Form["objActivityModel.ReferenceIds"].Split(',');
                List<string> urls = (List<string>)Session["ActivityDocUrls"];

                MyActivityModel _objMyActivity = new MyActivityModel();
                _objMyActivity.objLeadModel = objLeadModel;
                _objMyActivity.TenantId = TenentId;
                _objMyActivity.refIds = ReferenceIds;
                _objMyActivity.lstURLs = urls;
                _objMyActivity.currentUser = Session["User"].ToString();

                //string json = Newtonsoft.Json.JsonConvert.SerializeObject(_objMyActivity);

                response = client.PostAsJsonAsync("api/Activity/Insert", _objMyActivity).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = response.Content.ReadAsAsync<NIMBOLE.Models.Models.LeadModel>();
                    ViewData["objResultValue"] = objResultValue.Id;

                    //var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.NimboleHub>();
                    //HomeController _objHomeController = new HomeController();
                    //hubContext.Clients.All.update(_objHomeController.IndexForHub());
                }
                Session["ActivityDocUrls"] = null;
                Session["LeadDocUrls"] = null;
                Session["TransactionDocUrls"] = null;

                if (page != "Lead")
                    return RedirectToAction("Index");
                else
                {
                    objLeadModel.objActivityModel = new ActivityModel();
                    //base.UpdateModel(objLeadModel);
                    //return RedirectToAction("MileStone_Read", "Leads");
                    return View("~/Views/Shared/Activity/_Create.cshtml");
                }
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Delete
        public ActionResult Delete(int id)
        {
            try
            {
                response = client.DeleteAsync(strAPIURL + "api/Activity/DeleteById?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return Json(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Create");
        }
        #endregion Delete

        public ActionResult Deleteurlactivity([DataSourceRequest] DataSourceRequest request, int id)
        {
            try
            {
                response = client.DeleteAsync(strAPIURL + "api/Activity/Deleteurlactivity?id=" + id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objDocumentModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.DocumentModel>().Result;
                    return Json(new[] { objDocumentModel }.ToDataSourceResult(request, ModelState));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}