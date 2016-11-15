using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.Entities.Models;
using NIMBOLE.Entities;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;

namespace NIMBOLE.UI.Controllers
{
    public class ParentAccountController : BaseController
    {
        public ParentAccountController()
        {
        }

    
        #region CREATE
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ParentAccountModel objParentAccountModel, string item)
        {

            try
            {
                objParentAccountModel.Description = item;
                response = client.PostAsJsonAsync("api/ParentAccount/Insert", objParentAccountModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var parentaccounts = response.Content.ReadAsAsync<NIMBOLE.Entities.Models.ParentAccountModel>().Result;                  
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
                       
        }
              
        #endregion
              
    }
}