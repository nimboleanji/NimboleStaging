using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.Models.Models;
using NIMBOLE.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;

namespace NIMBOLE.UI.Controllers
{
    public class AccountTypeController : BaseController
    {
        Guid TenentId = new Guid();


        public AccountTypeController()
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


        #region CREATE
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, AccountTypeModel objAccountTypeModel, string item)
        {
            try
            {
                objAccountTypeModel.TenantId = TenentId;
                objAccountTypeModel.Description = item;
                response = client.PostAsJsonAsync("api/NimboleAccounts/InsertAccountType", objAccountTypeModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var accountType = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result;
                    ViewData["AccountType"] = accountType;
                    return Json(accountType.ToDataSourceResult(request));
                }
                else
                {
                    return Json(new[] { objAccountTypeModel }.ToDataSourceResult(request, ModelState));
                }
            }
            catch
            {
                return Content("");
            }                       
        }              
        #endregion
              
    }
}