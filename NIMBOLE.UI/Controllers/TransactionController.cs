using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.Models.Models;
using System.Web.Security;
using System.Configuration;
using NIMBOLE.Models;
using AutoMapper;
using System.Net.Http;
using System.Net.Http.Headers;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Diagnostics;
using NIMBOLE.Common;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR;
using NIMBOLE.UI.Models;

namespace NIMBOLE.UI.Controllers
{
    public class TransactionController : BaseController
    {

        Guid TenentId = new Guid();

        public TransactionController()
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                    TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        // GET: Transaction
        #region GET

        public ActionResult Index()
        {
            return View("~/Views/Shared/Transaction/_Create.cshtml");
        }

        public ActionResult Transaction_Read([DataSourceRequest] DataSourceRequest request, long leadId = 0)
        {
            try
            {
                if (leadId > 0)
                {
                    // string strLeadId = Session["CurrentLeadId"].ToString();
                    response = client.GetAsync("api/Transaction/GetTransactionByLeadId?iLeadId=" + leadId).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var lstData = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.LeadTransactionInfoModel>>().Result;

                        return Json(lstData.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(null);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        // TransDocument_Read

        public ActionResult TransDocument_Read([DataSourceRequest] DataSourceRequest request, long leadId = 0, long TransId = 0)
        {
            try
            {
                if (leadId > 0)
                {

                    response = client.GetAsync("api/Transaction/GetDocumentsByLeadId?iLeadId=" + leadId + "&TransId=" + TransId + "&Tid=" +TenentId).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var lstData = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.DocumentModel>>().Result;
                        return Json(lstData.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(null);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        public ActionResult LaunchPartialWindow()
        {
            return PartialView("~/Views/Shared/Transaction/_Create.cshtml");
        }

        #region CREATE
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("~/Views/Shared/Transaction/_Create.cshtml");
        }

        [HttpPost]
        //public ActionResult Create([DataSourceRequest] DataSourceRequest request, string strName,string plateNo,string bpkbNo,string address,string objId,string tenorSch)
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, LeadModel objLeadModel )
        {
            try
            {
                string page = "Lead";
                long iLeadId = Convert.ToInt64(Session["CurrentLeadId"].ToString());
               
                List<string> urls = (List<string>)Session["TransactionDocUrls"];

                LeadTransactionInfoModel _objLeadTransactionInfoModel = new LeadTransactionInfoModel();
                _objLeadTransactionInfoModel.TenantId = TenentId;
                _objLeadTransactionInfoModel.TransName = objLeadModel.objLeadTransactionInfoModel.TransName;
                _objLeadTransactionInfoModel.BPKBNumber = objLeadModel.objLeadTransactionInfoModel.BPKBNumber;
                _objLeadTransactionInfoModel.Address  =  objLeadModel.objLeadTransactionInfoModel.Address;
                _objLeadTransactionInfoModel.TenorScheme  =  objLeadModel.objLeadTransactionInfoModel.TenorScheme;
                _objLeadTransactionInfoModel.ProductId  =  objLeadModel.objLeadTransactionInfoModel.ProductId;
                _objLeadTransactionInfoModel.PlateNumber = objLeadModel.objLeadTransactionInfoModel.PlateNumber;
                _objLeadTransactionInfoModel.LeadId = iLeadId;
                _objLeadTransactionInfoModel.currentUser = Session["User"].ToString();
                _objLeadTransactionInfoModel.lstURLs = urls;

                response = client.PostAsJsonAsync("api/Transaction/InsertEdit", _objLeadTransactionInfoModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = response.Content.ReadAsAsync<NIMBOLE.Models.Models.LeadModel>();
                    ViewData["objResultValue"] = objResultValue;
                }
                Session["ActivityDocUrls"] = null;
                Session["LeadDocUrls"] = null;
                Session["TransactionDocUrls"] = null;

                if (page != "Lead")
                    return RedirectToAction("Index");
                else
                {   
                    objLeadModel.objLeadTransactionInfoModel= new LeadTransactionInfoModel();
                    return View("~/Views/Shared/Transaction/_Create.cshtml");
                    //return View(ViewData["objResultValue"]);
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
        public ActionResult Edit(int Id)
        {
            response = client.GetAsync("api/Transaction/GetById?id=" + Id + "&Tid=" + TenentId).Result;

            if (response.IsSuccessStatusCode)
            {
                var transaction = response.Content.ReadAsAsync<NIMBOLE.Models.Models.LeadTransactionInfoModel>().Result;
                //return Json(transaction.ToDataSourceResult(request));
                return PartialView("~/Views/Shared/Transaction/_Edit.cshtml", transaction);
            }
            else
            {
                return PartialView("~/Views/Shared/Transaction/_Edit.cshtml");
            }
        }
        [HttpPost]
        public ActionResult Edit(LeadTransactionInfoModel objLeadTransactionInfoModel)
        {
            try
            {

                if (objLeadTransactionInfoModel.Id != 0)
                {

                    Session["CurrentLeadId"] = objLeadTransactionInfoModel.LeadId.ToString();

                    List<string> urls = (List<string>)Session["TransactionDocUrls"];
                    objLeadTransactionInfoModel.lstURLs = urls;
                    objLeadTransactionInfoModel.currentUser = Session["User"].ToString();
                    objLeadTransactionInfoModel.TenantId = TenentId;
                    response = client.PutAsJsonAsync<LeadTransactionInfoModel>("api/Transaction/Edit", objLeadTransactionInfoModel).Result;
                   
                    if (response.IsSuccessStatusCode)
                    {
                        var objResultValue = response.Content.ReadAsAsync<LeadTransactionInfoModel>();
                        ViewData["objResultValue"] = objResultValue;
                        Session["ActivityDocUrls"] = null;
                        Session["LeadDocUrls"] = null;
                        Session["TransactionDocUrls"] = null;

                        return View("~/Views/Shared/Transaction/_Edit.cshtml");
                      
                    }
                }
                return View("~/Views/Shared/Transaction/_Edit.cshtml");
                
            }
            catch (Exception ex)
            {
                this.SetAlert(new AlertMessageViewModel()
                {
                    MessageType = MessageType.Error,
                    MessageHeading = "Failure",
                    MessageString = ex.Message
                });
                return RedirectToAction("_Edit");
            }
        }

        #endregion

        #region Delete
        public ActionResult Delete(int id)
        {
            try
            {
                response = client.DeleteAsync(strAPIURL + "api/Transaction/DeleteById?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return Json(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Create");
        }
        #endregion Delete
    }
}