using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NIMBOLE.Models;
//using NIMBOLE.Service.Models;
using NIMBOLE.Models.Models;
using AutoMapper;
using System.Web.UI;
using System.Diagnostics;
using NIMBOLE.Common;
using NIMBOLE.UI.Filters;
using NIMBOLE.UI.Models;

namespace NIMBOLE.UI.Controllers
{
    public class MilestoneController : BaseController
    {
        Guid TenentId = new Guid();
        //MilestoneModel objMilestoneModel = new MilestoneModel();
        //List<MilestoneModel> lstMileStoneModel = new List<MilestoneModel>();
        public MilestoneController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }

        #region GET
        public ActionResult GetAllMileStone()
        {
            try
            {
                response = client.GetAsync("api/Milestones/SelectAllMilestone?Tid="+TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objMilestoneModel = response.Content.ReadAsAsync<IEnumerable<MilestoneModel>>().Result;
                    
                    return Json(objMilestoneModel, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult GetMileStoneByLeadActivity(long iLeadId, long iActivityId)
        {
            try
            {
                response = client.GetAsync("api/MileStones/GetMileStoneByLeadActivity?iLeadId=" + iLeadId + "&iActivityId=" + iActivityId + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var lstMileStoneModel = response.Content.ReadAsAsync<IEnumerable<MilestoneModel>>().Result;
                    return Json(lstMileStoneModel, JsonRequestBehavior.AllowGet);
                }
                return Json("Error1: " + response.RequestMessage.Content);
            }
            catch (Exception ex)
            {
                return Json("Error2: " + ex.Message);
            }
        }
        
        [HttpGet]
        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            //bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            //if (_objAuthorized)
            //{
            //    var result = Milestone_Read(request);
            //    return View();
            //}
            //return RedirectToAction("AccessDenied","Error");

            response = client.GetAsync("api/Milestones/GetAll?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var lstMileStoneModel = response.Content.ReadAsAsync<IEnumerable<MilestoneModel>>().Result;
                ViewData["Milestones"] = lstMileStoneModel;
                return View(lstMileStoneModel);
                // return Json(data.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("AccessDenied","Error");
        }
        public JsonResult Milestone_Read([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Milestones/GetAll?Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<IEnumerable<MilestoneModel>>().Result;
                ViewData["Milestones"] = data;
                return Json(data.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            ViewData["Milestones"] = null;
            return Json(null);
        }
        #endregion

        #region CREATE
        [HttpGet]
        public ActionResult Create()
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                return View();
            }
            return RedirectToAction("AccessDenied", "Error");
        }
        [HttpPost]
        public ActionResult Create(MilestoneModel ObjMilestoneModel)
        {
            try
            {
                ObjMilestoneModel.TenantId = TenentId;

                response = client.PostAsJsonAsync("api/MileStones/Insert", ObjMilestoneModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var mileStones = response.Content.ReadAsAsync<MilestoneModel>().Result;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Milestone Created Successfully."
                    });
                }
                else
                {
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Error,
                        MessageHeading = "Failure",
                        MessageString = response.ReasonPhrase
                    });
                }
            }
            catch
            {
            }
            return RedirectToAction("Index");

        }
          #endregion
      //   HTTP:GET  /MileStone/Edit/1
        #region Post
        [HttpGet]
        [EncryptedActionParameter]
        public ActionResult Edit(int Id)
            {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/Milestones/GetById?id=" + Id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    MilestoneModel objMilestoneModel = response.Content.ReadAsAsync<MilestoneModel>().Result;
                    return View(objMilestoneModel);
                }
                else
                {
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Error,
                        MessageHeading = "Failure",
                        MessageString = response.ReasonPhrase
                    });
                }
                return View("Record Not Found");
            }
            return RedirectToAction("AccessDenied", "Error");
        }
      //   HTTP:PUT  /MileStone/Edit/
        public async System.Threading.Tasks.Task<ActionResult> Edit(MilestoneModel objMilestoneModel)
        {          
            try
            {
                objMilestoneModel.TenantId = TenentId;
                var response = await client.PutAsJsonAsync<MilestoneModel>("api/MileStones/Edit", objMilestoneModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<MilestoneModel>();
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Milestone Updated Successfully."
                    });
                    ViewData["objResultValue"] = objResultValue;
                }
                else
                {
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Error,
                        MessageHeading = "Failure",
                        MessageString = response.ReasonPhrase
                    });
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region DELETE
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, MilestoneModel objMilestoneModel, int id, bool status)
        {
            try
            {
                response = await client.DeleteAsync(strAPIURL + "api/Milestones/Delete?id=" + id + "&status=" + status);
                if (response.IsSuccessStatusCode)
                {
                    var MilestoneId = response.Content.ReadAsAsync<MilestoneModel>().Result;
                    return Json(new[] { MilestoneId }.ToDataSourceResult(request, ModelState));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("index");
        }
        #endregion
    }
}