using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.Models.Models;
using NIMBOLE.UI.Helpers;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using NIMBOLE.Common;
using Microsoft.AspNet.SignalR;
using Microsoft.Reporting.WebForms;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Net.Http.Headers;

namespace NIMBOLE.UI.Controllers
{
    public class TestController : BaseController
    {
        Guid TenentId = new Guid();

        public TestController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }

        [HttpGet]
        public ActionResult Index(int id=579)
        {
            GetAllDropdownValues();
            #region Load Lead/Edit Page Main Panel Details
            response = client.GetAsync("api/Leads/GetById?id=" + id + "&Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objLeadModel = response.Content.ReadAsAsync<LeadModel>().Result;
                if (objLeadModel.Id > 0)
                {
                    Session["CurrentLeadId"] = id;
                    response = client.GetAsync("api/LeadPriceDiscounts/GetRecent?leadId=" + id + "&Tid=" + TenentId).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var objTransLeadPriceDescountModel = response.Content.ReadAsAsync<LeadPriceDiscountModel>().Result;
                        objLeadModel.objLeadPriceDiscountModel = objTransLeadPriceDescountModel;
                    }
                    else
                        objLeadModel.objLeadPriceDiscountModel = new LeadPriceDiscountModel();

                    objLeadModel.objActivityModel = new ActivityModel();
                    ViewData["LeadBudget"] = objLeadModel.Budget;
                    return View(objLeadModel);
                }
            }
            #endregion
            return View("Record Not Found");
        }

        private void GetAllDropdownValues()
        {
            //Accounts
            response = client.GetAsync("api/NimboleAccounts/SelectAllAccount?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objAccountModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                ViewData["Accounts"] = objAccountModel;
            }
            //LeadSources
            response = client.GetAsync("api/Leadsource/SelectAllLeadSource?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objLeadSourceModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                ViewData["LeadSources"] = objLeadSourceModel;
            }
            //Milestones
            response = client.GetAsync("api/MileStones/SelectAllMileStone?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objMileStoneModel = response.Content.ReadAsAsync<IEnumerable<MilestoneModel>>().Result.ToList();
                ViewData["SelectMilestones"] = new SelectList(objMileStoneModel, "Id", "Description", new object { }, "MSOrder");
            }
            //Employees
            response = client.GetAsync("api/Employees/GetReferenceIds?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objEmployees = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.KeyValueModel>>().Result;
                ViewData["SelectEmployees"] = new SelectList(objEmployees, "Id", "Name", new object { });
            }
        }
    }
}