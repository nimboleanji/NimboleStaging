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
using NIMBOLE.Models.Mappers;
using System.Threading.Tasks;

namespace NIMBOLE.UI.Controllers
{
    public class LeadProductsController : BaseController
    {

        ProductsController _objProducts = new ProductsController();
        Guid TenentId = new Guid();

        public LeadProductsController()
        {
            
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());            
        }

        //
        // GET: /LeadProducts/

        #region Read
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string Product_Read([DataSourceRequest] DataSourceRequest request)
        {
            string products = _objProducts.Product_Read(request).ToString();
            return products;
        }
        public async Task<JsonResult> LeadProducts_Read([DataSourceRequest] DataSourceRequest request,long leadId)
        {            
            //long leadId = Convert.ToInt64(Session["CurrentLeadId"]);
            if (leadId > 0)
            {
                response = await client.GetAsync("api/LeadProducts/GetAllByLeadId?leadId=" + leadId);
                if (response.IsSuccessStatusCode)
                {
                    var lstData = response.Content.ReadAsAsync<IEnumerable<TranLeadProdCompModel>>().Result.ToList().OrderByDescending(p=>p.Pro1RowId);
                    ViewData["lstTranLeadProdCompModel"] = lstData;
                    return Json(lstData.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }

        #endregion
        public ActionResult LaunchPartialWindow()
        {
            return PartialView("~/Views/Shared/LeadProducts/_Create.cshtml");
        }

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, TranLeadProdCompModel objTranLeadProdCompModel,long leadId)
        {
            string strMessage = string.Empty;
            try
            {
               // long leadId = Convert.ToInt64(Session["CurrentLeadId"]);
                objTranLeadProdCompModel.LeadId = leadId;
                objTranLeadProdCompModel.TenantId = TenentId;
                string strLeadValue = "0";
                if (Request["Size"] != null)
                {
                    strLeadValue = Request["Size"].ToString();
                }
                objTranLeadProdCompModel.LeadValue = Convert.ToInt64(strLeadValue.ToString());
                response = client.PostAsJsonAsync("api/LeadProducts/Insert", objTranLeadProdCompModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var compProducts = response.Content.ReadAsAsync<TranLeadProdCompModel>().Result;
                    return Json(new[] { compProducts }.ToDataSourceResult(request));
                }
                strMessage = "OK";
            }
            catch(UnsupportedMediaTypeException ex)
            {
                ModelState.AddModelError("", ex.Message.ToString());
                strMessage = "UnsupportedMediaTypeException";
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message.ToString());
                strMessage = "General";
            }
            return null;
        }
        #endregion

        #region Edit
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit([DataSourceRequest] DataSourceRequest request, TranLeadProdCompModel objTranLeadProdCompModel)
        {
            try
            {
                if (objTranLeadProdCompModel.TempTransactionType != "Prevented")
                {
                    objTranLeadProdCompModel.TenantId = TenentId;
                    var response = await client.PutAsJsonAsync("api/LeadProducts/Edit", objTranLeadProdCompModel);
                    if (response.IsSuccessStatusCode)
                    {
                        var objResultValue = response.Content.ReadAsAsync<TranLeadProdCompModel>();
                        ViewData["objResultValue"] = objResultValue;
                        return Json(objResultValue);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                    return Json(objTranLeadProdCompModel);
            }
            catch
            {
                return View();
            }
        }

        #endregion
        
        #region DELETE
        //[HttpPost]
        //[Route("Delete")]
        //public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, TranLeadProdCompModel objTranLeadProdCompModel)
        //{
        //    if (objTranLeadProdCompModel.TempTransactionType == "Delete")
        //    {
        //        Int64[] proids = new Int64[4];

        //        proids[0] = objTranLeadProdCompModel.Pro1RowId;
        //        proids[1] = objTranLeadProdCompModel.Com1RowId;
        //        //proids[2] = objTranLeadProdCompModel.Com2RowId;
        //        //proids[3] = objTranLeadProdCompModel.Com3RowId;

        //        string strProductId = string.Join(",", proids);
        //        response = await client.DeleteAsync("api/LeadProducts/Delete?proids=" + strProductId);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return LeadProducts_Read(request);
        //        }
        //    }
        //    return Content("");
        //}

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, TranLeadProdCompModel objTranLeadProdCompModel,int ProdId, int Comp1Id)
        {
            try
            {
                long leadId = Convert.ToInt64(Session["CurrentLeadId"]);
                Int64[] proids = new Int64[2];

                proids[0] = ProdId;
                proids[1] = Comp1Id;

                string strProductId = string.Join(",", proids);
                response = await client.DeleteAsync("api/LeadProducts/Delete?proids=" + strProductId + "&LeadId=" + leadId);
                if (response.IsSuccessStatusCode)
                {
                    var objTranLeadProdCompModels = response.Content.ReadAsAsync<TranLeadProdCompModel>().Result;
                    return Json(new[] { objTranLeadProdCompModel }.ToDataSourceResult(request, ModelState));
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