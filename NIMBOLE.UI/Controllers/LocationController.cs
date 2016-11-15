using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using System.Web.UI;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using NIMBOLE.Models.Mappers;
using System.Web;
using NIMBOLE.Common;
using NIMBOLE.UI.Helpers;
using NIMBOLE.UI.Models;
using NIMBOLE.UI.Filters;

namespace NIMBOLE.UI.Controllers
{
    public class LocationController : BaseController
    {

        Guid TenentId = new Guid();
        public LocationController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());

        }

        // GET: Location
        [HttpGet]
        public ActionResult Index()
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/Location/GetAll?Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var location = response.Content.ReadAsAsync<IEnumerable<LocationModel>>().Result;                   
                    return View(location);
                }
            }
            return RedirectToAction("AccessDenied", "Error");
        }


        [HttpGet]
        public ActionResult SelectLocation()
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/Location/AllLocation?Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var location = response.Content.ReadAsAsync<IEnumerable<LocationModel>>().Result;

                    return Json(location, JsonRequestBehavior.AllowGet);
                }
            }
            return RedirectToAction("AccessDenied", "Error");
        }


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
        public ActionResult Create(LocationModel objLocationModel)
        {
            objLocationModel.TenantId = TenentId;
            objLocationModel.Status = true;
            response = client.PostAsJsonAsync("api/Location/Insert", objLocationModel).Result;
            return RedirectToAction("Index");
        }
        #endregion

        #region PUT
        [HttpGet]
        [EncryptedActionParameter]
        public ActionResult Edit(int Id)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/Location/GetById?id=" + Id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    LocationModel objLocationModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.LocationModel>().Result;
                    return View(objLocationModel);
                }
                return View("Record Not Found");
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        public async System.Threading.Tasks.Task<ActionResult> Edit(LocationModel objLocationModel)
        {
            try
            {
                objLocationModel.TenantId = TenentId;
                var response = await client.PutAsJsonAsync<LocationModel>("api/Location/Edit", objLocationModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<LocationModel>();
                    ViewData["objResultValue"] = objResultValue;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Location Updated Successfully."
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
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.SetAlert(new AlertMessageViewModel()
                {
                    MessageType = MessageType.Error,
                    MessageHeading = "Failure",
                    MessageString = ex.Message
                });
                return View();
            }
        }

        #endregion

        #region DELETE
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, LocationModel objLocationModel, int id, bool status)
        {
            try
            {
                response = await client.DeleteAsync(strAPIURL + "api/Location/Delete?id=" + id + "&status=" + status);
                if (response.IsSuccessStatusCode)
                {
                    var LocationId = response.Content.ReadAsAsync<NIMBOLE.Models.Models.LocationModel>().Result;
                    return Json(new[] { objLocationModel }.ToDataSourceResult(request, ModelState));
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