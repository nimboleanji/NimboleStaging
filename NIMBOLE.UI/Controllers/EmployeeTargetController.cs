using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using NIMBOLE.Common;
using NIMBOLE.UI.Filters;

namespace NIMBOLE.UI.Controllers
{
    public class EmployeeTargetController : BaseController
    {
        DTO objNIMBOLEMapper = null;
        ProductExcelImport excelImport;
        Guid TenentId = new Guid();
        public EmployeeTargetController()
        {
            try
            {
                objNIMBOLEMapper = new DTO();
                excelImport = new ProductExcelImport();
                if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                    TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }

        #region GET
        // GET: EmployeeTarget
        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                return View();
            }
            return RedirectToAction("AccessDenied", "Error");
        }
        public JsonResult EmployeeTarget_Read([DataSourceRequest] DataSourceRequest request)
        {

            response = client.GetAsync("api/EmployeeTarget/AllEmployeeTarget?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var targets = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.EmployeeTargetModel>>().Result;
                return Json(targets.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        public ActionResult GetAllEmployeeTargets([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/EmployeeTarget/GetAllEmployeeTargets?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var lstEmployeeTargetModel = response.Content.ReadAsAsync<IEnumerable<EmployeeTargetModel>>().Result;
                return Json(lstEmployeeTargetModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        #endregion

        #region Create
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
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, EmployeeTargetModel objEmployeeTargetModel, FormCollection fomr)
        {
            try
            {
                objEmployeeTargetModel.TenantId = TenentId;
                response = client.PostAsJsonAsync("api/EmployeeTarget/InsertEmployeeTarget", objEmployeeTargetModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var employeeTarget = response.Content.ReadAsAsync<EmployeeTargetModel>().Result;
                    return Json(new[] { employeeTarget }.ToDataSourceResult(request));
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Edit
        [HttpGet]
        [EncryptedActionParameter]
        // HTTP:GET  /EmployeeTarget/Edit/1
        public ActionResult Edit(int Id)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/EmployeeTarget/GetById?id=" + Id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objEmployeeTargetModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.EmployeeTargetModel>().Result;
                    return View(objEmployeeTargetModel);
                }
                return View("Record Not Found");
            }
            return RedirectToAction("AccessDenied", "Error");
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit([DataSourceRequest] DataSourceRequest request, EmployeeTargetModel objEmployeeTargetModel)
        {
            try
            {
                var response = await client.PutAsJsonAsync<EmployeeTargetModel>("api/EmployeeTarget/Edit", objEmployeeTargetModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<EmployeeTargetModel>();
                    return Json(new[] { objResultValue }.ToDataSourceResult(request));
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region DELETE
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, int id, bool status)
        {
            try
            {
                response = await client.DeleteAsync(strAPIURL + "api/EmployeeTarget/DeleteEmployeeTarget?id=" + id + "&status=" + status);
                if (response.IsSuccessStatusCode)
                {
                    var objEmployeeTargetModel = response.Content.ReadAsAsync<EmployeeTargetModel>().Result;
                    return Json(new[] { objEmployeeTargetModel }.ToDataSourceResult(request));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message.ToString());
            }
            return RedirectToAction("index");
        }
        #endregion
    }
}



