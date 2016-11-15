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
using System.Configuration;

namespace NIMBOLE.UI.Controllers
{
    public class LanguageController : BaseController
    {
        Guid TenentId = new Guid();


        public LanguageController()
        {

            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }
        public ActionResult Language_Read([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Languages/Getnames?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var languages = response.Content.ReadAsAsync<IEnumerable<LanguageModel>>();
                return Json(languages, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
       
 
        [HttpGet]
        public ActionResult Index()
        {
            response = client.GetAsync("api/Languages/GetAll?Tid="+ TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var languages = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result;
                return Json(languages, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            } 
        }
        
        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(LanguageModel objLanguageModel)
        {
            try
            {
                objLanguageModel.TenantId = TenentId;
                var response = await client.PostAsJsonAsync<LanguageModel>("api/Languages/Insert", objLanguageModel);
                
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<long>();
                    ViewData["objResultValue"] = objResultValue;
                    if(Convert.ToInt64(objResultValue)>0)
                        return RedirectToAction("Index");
                    return RedirectToAction("Create");
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

        #region PUT
        [HttpGet]
        public ActionResult Edit(int id)
        {
                response = client.GetAsync("api/Languages/GetById?id=" + id + "&Tid=" +TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objLanguageModel = response.Content.ReadAsAsync<LanguageModel>().Result;
                    return View(objLanguageModel);
                }
             return View("Record Not Found");
        }
        public async System.Threading.Tasks.Task<ActionResult> Edit(LanguageModel objLanguageModel)
        {
            try
            {
                objLanguageModel.TenantId = TenentId;
                var response = await client.PutAsJsonAsync<LanguageModel>("api/Languages/Edit", objLanguageModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<LanguageModel>();
                    ViewData["objResultValue"] = objResultValue;
                    return RedirectToAction("Index");
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
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
                response = await client.DeleteAsync("api/Languages/Delete?id="+ id);
                if (response.IsSuccessStatusCode)
                {
                    var objLanguageModel = response.Content.ReadAsAsync<LanguageModel>().Result;
                    return View(objLanguageModel);
                }
              return View("Record Not Found");
        }
        #endregion
    }
}