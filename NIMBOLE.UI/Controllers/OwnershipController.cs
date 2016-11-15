using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.Models.Models;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using NIMBOLE.Common;

namespace NIMBOLE.UI.Controllers
{
    public class OwnershipController : BaseController
    {
        Guid TenentId = new Guid();
        public OwnershipController()
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
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, OwnershipModel objOwnershipModel, string item)
        {
            try
            {
                objOwnershipModel.TenantId = TenentId;
                objOwnershipModel.Description = item;
                response = client.PostAsJsonAsync("api/Ownership/Insert", objOwnershipModel).Result;
                if (response.IsSuccessStatusCode)
                {                  
                    var ownership = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.OwnershipModel>>().Result;
                    return Json(ownership.ToDataSourceResult(request));                 
                }
                else
                {
                    return Json(new[] { objOwnershipModel }.ToDataSourceResult(request, ModelState));
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