using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using System.Net.Http;
using Kendo.Mvc.Extensions;
using NIMBOLE.UI.Filters;


namespace NIMBOLE.UI.Controllers
{
    public class NotificationsController : BaseController
    {
         Guid TenentId = new Guid();
         public NotificationsController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }


        #region Get
        public ActionResult Index()
        {
            response = client.GetAsync("api/Notify/GetNotifications?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objActivityModel = response.Content.ReadAsAsync<IEnumerable<NotificationModel>>().Result;              
                return View(objActivityModel);
            }
            return Json(null);

        }

        [HttpGet]
        public JsonResult GetNotifications([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Notify/GetNotifications?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objActivityModel = response.Content.ReadAsAsync<IEnumerable<NotificationModel>>().Result;
                return Json(objActivityModel, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        [HttpGet]
        public JsonResult GetTasks([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Notify/GetTasks?Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objEmployeeTaskModel = response.Content.ReadAsAsync<IEnumerable<EmployeeTaskModel>>().Result;
                return Json(objEmployeeTaskModel, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        public JsonResult GetNotificationsForGrid([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Notify/GetNotifications?Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objActivityModel = response.Content.ReadAsAsync<IEnumerable<NotificationModel>>().Result;
                return Json(objActivityModel.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        [HttpGet]
        [EncryptedActionParameter]
        public ActionResult Display(int Id)
        {
            response = client.GetAsync("api/Notify/GetById?id=" + Id +"&Tid=" +TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var ObjActivityModel = response.Content.ReadAsAsync<NotificationModel>().Result;
                return View(ObjActivityModel);
            }
            return View("Record Not Found");
        }
        #endregion

    }
}