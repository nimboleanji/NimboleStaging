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
using NIMBOLE.Models.Mappers;
using NIMBOLE.Common;

namespace NIMBOLE.UI.Controllers
{
     
    public class LeadPriceDiscountsController : BaseController
    {
        Guid TenentId = new Guid();
        public LeadPriceDiscountsController()
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
        //public bool IsNewRecord(long iLeadId)
        //{
        //    bool blnResult=false;
        //    blnResult= dbcontext.TblTransLeadPriceDiscounts.Where(tl => tl.LeadId == iLeadId).Count() > 0;
        //    return (blnResult);
        //}
        #region View
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LeadPriceDiscounts_Read([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/LeadPriceDiscounts/GetAll?Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var documents = response.Content.ReadAsAsync<IEnumerable<DocumentModel>>().Result;

                return Json(documents.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        public ActionResult Read_RecentPriceDiscountByLead([DataSourceRequest] DataSourceRequest request)
        {
            string strLeadId = Session["CurrentLeadId"].ToString();
            response = client.GetAsync("api/LeadPriceDiscounts/GetRecent?leadId=" + strLeadId +"&Tid="+ TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objLeadPriceDiscountModel = response.Content.ReadAsAsync<IEnumerable<LeadPriceDiscountModel>>().Result;

                return Json(objLeadPriceDiscountModel.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        public ActionResult LaunchPartialWindow()
        {
            return PartialView("~/Views/Shared/LeadPriceDiscounts/_Create.cshtml");
        }

        public String IsNewRecord()
        {
            long LeadId = Convert.ToInt64(Session["CurrentLeadId"]);
            response = client.GetAsync("api/LeadPriceDiscounts/IsNewRecord?LeadId=" + LeadId ).Result;
            if (response.IsSuccessStatusCode)
            {
                long count = response.Content.ReadAsAsync<long>().Result;
                return count.ToString();
            }
            return "0";
        }

        #endregion
        
        #region Create
        public ActionResult _Create(long leadId, string page)
        {
            try
            {
                response = client.GetAsync("api/LeadPriceDiscounts/GetRecent?leadId=" + leadId.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objLeadPriceDiscountModel = response.Content.ReadAsAsync<LeadPriceDiscountModel>().Result;
                    //return Json(objLeadPriceDiscountModel, JsonRequestBehavior.AllowGet);
                    return PartialView(objLeadPriceDiscountModel);
                }
            }
            catch
            {
                return View();
            }
            return View();
        }
        public ActionResult Create(long leadId, string page)
        {
            try
            {
                response = client.GetAsync("api/LeadPriceDiscounts/GetRecent?leadId=" + leadId.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objLeadPriceDiscountModel = response.Content.ReadAsAsync<LeadPriceDiscountModel>().Result;
                    //return Json(objLeadPriceDiscountModel, JsonRequestBehavior.AllowGet);
                    return View(objLeadPriceDiscountModel);
                }
            }
            catch
            {
                return View();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request,LeadModel objLeadModel)
        {
            try
            {
                string page = "Lead";
                LeadPriceDiscountModel objLeadPriceDiscountModel = new LeadPriceDiscountModel();
                objLeadPriceDiscountModel = objLeadModel.objLeadPriceDiscountModel;

                objLeadPriceDiscountModel.CreatedDate = DateTime.Now;
                objLeadPriceDiscountModel.ModifiedDate = DateTime.Now;
                objLeadPriceDiscountModel.Status = true;
                objLeadPriceDiscountModel.LeadId = Convert.ToInt64(Session["CurrentLeadId"]);
                objLeadPriceDiscountModel.TenantId = objLeadPriceDiscountModel.TenantId.ToDefaultTenantId();

                response = client.PostAsJsonAsync("api/LeadPriceDiscounts/Insert", objLeadPriceDiscountModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    objLeadPriceDiscountModel = response.Content.ReadAsAsync<LeadPriceDiscountModel>().Result;
                }

                if (page != "Lead")
                    return RedirectToAction("Index");
                else
                {
                    //return PartialView("~/Views/shared/LeadPriceDiscounts/_Create.cshtml", objLeadModel);
                    //return PartialView("~/Views/shared/LeadPriceDiscounts/_Create.cshtml", objLeadPriceDiscountModel);
                    return Json(new { url = Url.Action("Edit/" + objLeadModel.Id, "Leads") });
                }
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Edit
        [HttpGet]

        public ActionResult Edit(Int64 leadId)
        {
            response = client.GetAsync("api/LeadPriceDiscounts/GetRecent?leadId=" + leadId).Result;
            if (response.IsSuccessStatusCode)
            {
                LeadPriceDiscountModel objLeadPriceDiscountModel = response.Content.ReadAsAsync<LeadPriceDiscountModel>().Result;
                //LeadPriceDiscountModel objLeadPriceDiscountModel = objNIMBOLEMapper.MapTable2Model(objTblTransLeadPriceDiscount);
                ViewData["FirstName"] = objLeadPriceDiscountModel.EmployeeId;
                return View(objLeadPriceDiscountModel);
            }
            return View("Record Not Found");
        }

        public async System.Threading.Tasks.Task<ActionResult> Edit(LeadPriceDiscountModel objLeadPriceDiscountModel)
        {

            try
            {
                objLeadPriceDiscountModel.TenantId = new Guid(Session["CurrentTenentId"].ToString());
                var response = await client.PutAsJsonAsync<LeadPriceDiscountModel>("api/LeadPriceDiscounts/Edit", objLeadPriceDiscountModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<LeadPriceDiscountModel>();
                    ViewData["objResultValue"] = objResultValue;
                    return Redirect("~/LeadPriceDiscounts/Index");
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
        public async System.Threading.Tasks.Task<ActionResult> Delete(Int64 id)
        {
            response = await client.DeleteAsync("api/LeadPriceDiscounts/Delete?id=" + id);
            response = client.GetAsync("api/LeadPriceDiscounts/GetById?id=" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                LeadPriceDiscountModel objLeadPriceDiscountModel = response.Content.ReadAsAsync<LeadPriceDiscountModel>().Result;
                //LeadPriceDiscountModel objLeadPriceDiscountModel = Mapper.Map<TblTransLeadPriceDiscount, LeadPriceDiscountModel>(objTblTransLeadPriceDiscounts);
                return View(objLeadPriceDiscountModel);
            }
            return View("Record Not Found");
        }
        #endregion

    }
}