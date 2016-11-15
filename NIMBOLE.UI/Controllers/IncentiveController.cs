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
    public class IncentiveController : BaseController
    {
        Guid TenentId = new Guid();
        public IncentiveController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());

        }

        // GET: Incentive
        public ActionResult Index()
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/Incentive/GetAll?Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var incentive = response.Content.ReadAsAsync<IEnumerable<IncentiveModel>>().Result;
                    ViewData["ContactRoles"] = incentive;
                    return View(incentive);
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
        public ActionResult Create(IncentiveModel objIncentiveModel)
        {
            objIncentiveModel.TenantId = TenentId;
            objIncentiveModel.Status = true;
            response = client.PostAsJsonAsync("api/Incentive/Insert", objIncentiveModel).Result;
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
                response = client.GetAsync("api/Incentive/GetById?id=" + Id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    IncentiveModel objIncentiveModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.IncentiveModel>().Result;
                    return View(objIncentiveModel);
                }
                return View("Record Not Found");
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        public async System.Threading.Tasks.Task<ActionResult> Edit(IncentiveModel objIncentiveModel)
        {
            try
            {
                objIncentiveModel.TenantId = TenentId;
                var response = await client.PutAsJsonAsync<IncentiveModel>("api/Incentive/Edit", objIncentiveModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<IncentiveModel>();
                    ViewData["objResultValue"] = objResultValue;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Incentive Updated Successfully."
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
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, IncentiveModel objIncentiveModel, int id, bool status)
        {
            try
            {
                response = await client.DeleteAsync(strAPIURL + "api/Incentive/Delete?id=" + id + "&status=" + status);
                if (response.IsSuccessStatusCode)
                {
                    var IncentiveId = response.Content.ReadAsAsync<NIMBOLE.Models.Models.IncentiveModel>().Result;
                    return Json(new[] { objIncentiveModel }.ToDataSourceResult(request, ModelState));
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