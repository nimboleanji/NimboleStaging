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
using NIMBOLE.Models.Models;
using AutoMapper;
using System.Web.UI;
using System.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using NIMBOLE.Models.Mappers;
using System.Dynamic;
using NIMBOLE.Models.Models.Transactions.TransUser;
using NIMBOLE.Models.Models.Transactions;
using System.Threading.Tasks;

namespace NIMBOLE.UI.Controllers
{
    public class TranExtContactController : BaseController
    {
        Guid TenentId = new Guid();

        public TranExtContactController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }


        #region GET
        public async Task<JsonResult> TranExtContact_Read([DataSourceRequest] DataSourceRequest request,long leadId)
        {
           // long leadId = Convert.ToInt64(Session["CurrentLeadId"]);
            if (leadId > 0)
            {
                response = await client.GetAsync("api/TranExtContact/GetByLeadId?iLeadId=" + leadId.ToString() +"&Tid=" + TenentId);
                if (response.IsSuccessStatusCode)
                {
                    var lstExtContactModel = response.Content.ReadAsAsync<IEnumerable<ExtContactModel>>().Result;
                    return Json(lstExtContactModel.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }
        #endregion

        [HttpPost]
        public JsonResult Create([DataSourceRequest] DataSourceRequest request, ExtContactModel objExtContactModel,long leadId)
        {
            try
            {
                //if (objExtContactModel.CreatedDate.Date == DateTime.Now.Date)
                //{
                    //objExtContactModel.LeadId = Convert.ToInt64(Session["CurrentLeadId"].ToString());
                objExtContactModel.TenantId = TenentId;
                if (leadId > 0)
                {
                    objExtContactModel.LeadId = leadId;
                    response = client.PostAsJsonAsync("api/TranExtContact/Insert", objExtContactModel).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsAsync<ExtContactModel>().Result;
                        return Json(new[] { result }.ToDataSourceResult(request));
                    }
                    //}
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Ex: " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult TempCreate([DataSourceRequest] DataSourceRequest request, ExtContactModel objExtContactModel)
        {
            try
            {
                objExtContactModel.TenantId = TenentId;
                if (objExtContactModel.ExtContactId != null && objExtContactModel.ExtContactRoleId != null)
                {
                    objExtContactModel.WorkEmail = objExtContactModel.WorkEmail.Trim();
                    objExtContactModel.ContactEmail = objExtContactModel.ContactEmail.Trim();
                    return Json(new[] { objExtContactModel }.ToDataSourceResult(request, ModelState));
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }


        #region Edit
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(ExtContactModel objExtContactModel)
        {
            try
            {
                objExtContactModel.TenantId = TenentId;
                objExtContactModel.WorkEmail = objExtContactModel.WorkEmail.Trim();
                var response = await client.PutAsJsonAsync<ExtContactModel>("api/TranExtContact/Edit", objExtContactModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<ExtContactModel>();
                    ViewData["objResultValue"] = objResultValue;
                    return Json(objResultValue);
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

        #region Delete
        //[HttpPost]
        //[Route("Delete")]
        //public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, ExtContactModel objExtContactModel)
        //{
        //    try
        //    {
        //        response = client.DeleteAsync("api/TranExtContact/Delete?Id=" + objExtContactModel.Id).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return TranExtContact_Read(request);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new InvalidOperationException();
        //    }

        //    return RedirectToAction("Create");
        //}

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, ExtContactModel objExtContactModel, int Id)
        {
            try
            {
                if (Id > 0)
                {
                    response = await client.DeleteAsync("api/TranExtContact/Delete?Id=" + objExtContactModel.Id);
                    if (response.IsSuccessStatusCode)
                    {   
                        var objExtContactModels = response.Content.ReadAsAsync<ExtContactModel>().Result;
                        return Json(new[] { objExtContactModel }.ToDataSourceResult(request, ModelState));
                    }
                }
                return Content("");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException();
            }
        }

        #endregion
    }
}