using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;
using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using AutoMapper;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.Configuration;
using NIMBOLE.Common;
using NIMBOLE.UI.Filters;
using NIMBOLE.UI.Models;

namespace NIMBOLE.UI.Controllers
{
    public class FinancialYearController  : BaseController
    {
          Guid TenentId = new Guid();

        public FinancialYearController()
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
        //public JsonResult FinancialYear_Read([DataSourceRequest] DataSourceRequest request)
        //{
        //    response = client.GetAsync("api/FinancialYear/GetAll?Tid="+TenentId).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var lstObjFinancialYear = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.FinancialYearModel>>().Result.ToList().OrderByDescending(x=>x.FinancialYear);
        //        ViewData["FinancialYearDetails"] = lstObjFinancialYear;
        //        return Json(lstObjFinancialYear.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //    }
        //    ViewData["FinancialYearDetails"] = null;
        //    return Json(null);
        //}

        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/FinancialYear/GetAll?Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var lstObjFinancialYear = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.FinancialYearModel>>().Result.ToList().OrderByDescending(x => x.FinancialYear);
                    ViewData["FinancialYearDetails"] = lstObjFinancialYear;
                    return View(lstObjFinancialYear);
                }
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        #region GET
        [HttpGet]
        public ActionResult GetAllFinancialYears()
        {
            response = client.GetAsync("api/FinancialYear/GetAllFinancialYears?Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var lstObjFinancialYearModel = response.Content.ReadAsAsync<IEnumerable<FinancialYearModel>>().Result;
                return Json(lstObjFinancialYearModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpGet]
        public ActionResult GetFinancialYearsForReports()
        {
            response = client.GetAsync("api/FinancialYear/GetFinancialYearsForReports?Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var lstObjFinancialYearModel = response.Content.ReadAsAsync<IEnumerable<FinancialYearModel>>().Result;
                return Json(lstObjFinancialYearModel, JsonRequestBehavior.AllowGet);
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
        public ActionResult Create(FinancialYearModel objFinancialYearModel)
        {
            try
            {
                objFinancialYearModel.TenantId = TenentId;
                objFinancialYearModel.Status = true;
                response = client.PostAsJsonAsync("api/FinancialYear/Insert", objFinancialYearModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    var _objFinancialYearModel = response.Content.ReadAsAsync<FinancialYearModel>().Result;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Financial Year Created Successfully."
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
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region PUT
        [HttpGet]
        [EncryptedActionParameter]
        // HTTP:GET  /FinancialYear/Edit/1
        public ActionResult Edit(int Id)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/FinancialYear/GetById?id=" + Id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    FinancialYearModel objFinancialYearModel = response.Content.ReadAsAsync<FinancialYearModel>().Result;
                    return View(objFinancialYearModel);
                }
                return View("Record Not Found");
            }
            return RedirectToAction("AccessDenied", "Error");
        }
        // HTTP:PUT  /FinancialYear/Edit/
        public async System.Threading.Tasks.Task<ActionResult> Edit(FinancialYearModel objFinancialYearModel)
        {
            try
            {
                var response = await client.PutAsJsonAsync<FinancialYearModel>("api/FinancialYear/Edit", objFinancialYearModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<FinancialYearModel>();
                    ViewData["objResultValue"] = objResultValue;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Financial Year Updated Successfully."
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
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region DELETE
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, FinancialYearModel objFinancialYearModel, int id, bool status)
        {
            try
            {
                response = await client.DeleteAsync(strAPIURL + "api/FinancialYear/Delete?Id=" + id + "&status=" + status);
                if (response.IsSuccessStatusCode)
                {
                    var objFinancialYearModels = response.Content.ReadAsAsync<FinancialYearModel>().Result;
                    return Json(new[] { objFinancialYearModel }.ToDataSourceResult(request, ModelState));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Index");
        }
        #endregion
	}
}