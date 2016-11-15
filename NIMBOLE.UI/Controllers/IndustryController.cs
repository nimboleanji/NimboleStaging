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
    public class IndustryController : BaseController
    {
        Guid TenentId = new Guid();
        public IndustryController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }

    
        #region CREATE
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, IndustryModel objIndustryModel, string item)
        {
            try
            {
                objIndustryModel.TenantId = TenentId;
                objIndustryModel.Description = item;
                response = client.PostAsJsonAsync("api/Industry/Insert", objIndustryModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var industry = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result;
                    ViewData["Industry"] = industry;
                    return Json(industry.ToDataSourceResult(request));
                }
                else
                {
                    return Json(new[] { objIndustryModel }.ToDataSourceResult(request, ModelState));
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