using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Net.Http.Headers;
using System.Diagnostics;
using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using AutoMapper;
using System.Collections;
using System.Configuration;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Common;
using NIMBOLE.UI.Filters;
using NIMBOLE.UI.Models;
using System.Threading.Tasks;

namespace NIMBOLE.UI.Controllers
{
    public class LeadSourceController : BaseController
    {
        Guid TenentId = new Guid();
        public LeadSourceController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }

        public JsonResult Leadsource_Read([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/LeadSource/GetAll?Tid="+ TenentId).Result;
            if(response.IsSuccessStatusCode)
            {
                var leadsourceList = response.Content.ReadAsAsync<IEnumerable<LeadSourceModel>>().Result;
                ViewData["LeadSources"] = leadsourceList;
                return Json(leadsourceList.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            ViewData["LeadSources"] = null;
             return Json(null);
        }
         
        [HttpGet]
        public JsonResult GetAllLeadSources()
        {
            response = client.GetAsync("api/Leadsource/SelectAllLeadSource?Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objLeadSourceModel = response.Content.ReadAsAsync<IEnumerable<LeadSourceModel>>().Result.ToList();
                return Json(objLeadSourceModel, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        [HttpGet]
        public async Task<JsonResult> GetAllLeadSourcesForCombo()
        {
            //response = client.GetAsync("api/Leadsource/SelectAllLeadSource?Tid=" + TenentId).Result;
            response = await client.GetAsync("api/Leadsource/SelectLeadSourceForCombobox?Tid=" + TenentId);
            if (response.IsSuccessStatusCode)
            {
                var objLeadSourceModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                return Json(objLeadSourceModel, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        [HttpGet]
        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
             response = client.GetAsync("api/LeadSource/GetAll?Tid="+ TenentId).Result;
            if(response.IsSuccessStatusCode)
            {
                var leadsourceList = response.Content.ReadAsAsync<IEnumerable<LeadSourceModel>>().Result;
                ViewData["LeadSources"] = leadsourceList;
                return View(leadsourceList);
            }
            return RedirectToAction("AccessDenied", "Error");
        }

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

        [ HttpPost]
        public ActionResult Create(LeadSourceModel objLeadsourceModel)
        {
            //Call Insert WEBAPI using    http://localhost:6390/api/Languages/Insert
            try
            {
                objLeadsourceModel.TenantId = TenentId;
                var response = client.PostAsJsonAsync("api/Leadsource/Insert", objLeadsourceModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Leadsources = response.Content.ReadAsAsync<LeadSourceModel>().Result;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Lead Source Created Successfully."
                    });
                    return RedirectToAction("Index");
                }
                else
                {
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Error,
                        MessageHeading = "Failure",
                        MessageString = response.ReasonPhrase
                    });
                    return RedirectToAction("Index");
                }
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

        [HttpPost]
        public ActionResult CreateLeadSource([DataSourceRequest] DataSourceRequest request, LeadSourceModel objLeadSourceModel, string item)
        {
            try
            {
                objLeadSourceModel.Description = item;
                objLeadSourceModel.TenantId = TenentId;
                response = client.PostAsJsonAsync("api/LeadSource/InsertLeadSource", objLeadSourceModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var leadsource = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.KeyValueModel>>().Result;
                    ViewData["LeadSources"] = leadsource;
                    return Json(leadsource.ToDataSourceResult(request));
                }
                else
                {
                    return Json(new[] { objLeadSourceModel }.ToDataSourceResult(request, ModelState));
                }
            }
            catch
            {
                return Content("");
            }
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
                response = client.GetAsync("api/LeadSource/GetById?id=" + Id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    LeadSourceModel objLeadSourceModel = response.Content.ReadAsAsync<LeadSourceModel>().Result;
                    return View(objLeadSourceModel);
                }
                return View("Record Not Found");
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        public async System.Threading.Tasks.Task<ActionResult> Edit(LeadSourceModel objLeadSourceModel)
        {
            try
            {
                objLeadSourceModel.TenantId = TenentId;
                var response = await client.PutAsJsonAsync<LeadSourceModel>("api/LeadSource/Edit", objLeadSourceModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<LeadSourceModel>();
                    ViewData["objResultValue"] = objResultValue;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Lead Source Updated Successfully."
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
            catch(Exception ex)
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
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, LeadSourceModel objLeadSourceModel, int id, bool status)
        {
            try
            {
                response = await client.DeleteAsync(strAPIURL + "api/LeadSource/Delete?id=" + id + "&status=" + status);

                if (response.IsSuccessStatusCode)
                {
                    var objTblLeadsource = response.Content.ReadAsAsync<LeadSourceModel>().Result;
                    return Json(new[] { objLeadSourceModel }.ToDataSourceResult(request, ModelState));
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
