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
using System.Threading.Tasks;


namespace NIMBOLE.UI.Controllers
{
    public class DocumentsController : BaseController
    {
        Guid TenentId = new Guid();
        public DocumentsController()
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

        //
        // GET: /Documents/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Document_Read([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Documents/GetAll?Tid="+ TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var documents = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.DocumentModel>>().Result;

                return Json(documents.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        public async Task<ActionResult> ReadDistinctDocuments([DataSourceRequest] DataSourceRequest request,long leadId)
        {

           // long iLeadId =Convert.ToInt64(Session["CurrentLeadId"]);
            if (leadId > 0)
            {
                response = await client.GetAsync("api/Documents/GetAllDocumentByLeadId?iLeadId=" + leadId +"&Tid=" + TenentId);
                if (response.IsSuccessStatusCode)
                {
                    var documents = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.DocumentWithMultipleUrlsModel>>().Result.ToArray();
                    for (int i = 0; i < documents.Count(); i++)
                    {
                        documents[i].Id = documents[i].lstUrlBasedId[0];
                    }

                    return Json(documents.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null);
        }

        [HttpGet]
        public async Task<ActionResult> SelectDocumentsByActivityId(int iActivityId = 0)
        {
            try
            {
                response = await client.GetAsync("api/Documents/GetDocumentsByActivityId?iActivityId=" + iActivityId.ToString() +"&Tid="+TenentId);
                if (response.IsSuccessStatusCode)
                {
                    var objDocuments = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.DocumentModel>>().Result;
                    return Json(objDocuments, JsonRequestBehavior.AllowGet);
                }
                return Json(null);
            }
            catch
            {
                return Json(null);
            }
        }


        [HttpGet]
        public async Task<ActionResult> GetDocumentsByTransactionId(int iTransactionId = 0)
        {
            try
            {
                response = await client.GetAsync("api/Documents/GetDocumentsByTransactionId?iTransactionId=" + iTransactionId.ToString() + "&Tid=" + TenentId);
                if (response.IsSuccessStatusCode)
                {
                    var objDocuments = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.DocumentModel>>().Result;
                    return Json(objDocuments, JsonRequestBehavior.AllowGet);
                }
                return Json(null);
            }
            catch
            {
                return Json(null);
            }
        }

        [HttpGet]
        public ActionResult DocumentUrl_Read([DataSourceRequest] DataSourceRequest request, string docName)
        {
            long iLeadId = Convert.ToInt64(Session["CurrentLeadId"]);
            response = client.GetAsync("api/Documents/GetUrlByName?docName=" + docName + "&leadId=" + iLeadId + "&Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var documents = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.DocumentModel>>().Result;

                return Json(documents, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        public ActionResult Document_ReadByLead([DataSourceRequest] DataSourceRequest request, int id)
        {
            response = client.GetAsync("api/Documents/GetDocumentByLead?id=" + id +"&Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var documents = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.DocumentModel>>().Result;

                return Json(documents.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        public ActionResult Document_ReadByGroup([DataSourceRequest] DataSourceRequest request, string grpName)
        {
            response = client.GetAsync("api/Documents/GetDocumentByGroup?name=" + grpName +"&Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var documents = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.DocumentModel>>().Result;

                return Json(documents.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        public ActionResult LaunchPartialWindow()
        {
            return PartialView("~/Views/Shared/Documents/_Create.cshtml");
        }

        public ActionResult AttachDocument()
        {
            return View();
        }
	
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, DocumentModel objDocumentModel)
        {
            try
            {
                objDocumentModel.TenantId = TenentId;

                if (objDocumentModel.CreatedDate.Date == DateTime.Now.Date)
                {
                    long iLeadId = Convert.ToInt64(Session["CurrentLeadId"]);
                    // long iActivityId = Convert.ToInt64(Session["CurrentActivityId"]);

                    long iActivityId = Convert.ToInt64(Session["CurrentActivityId"]);
                    List<string> _leadDocUrls = (List<string>)Session["LeadDocUrls"];
                    List<string> _activityDocUrls = (List<string>)Session["ActivityDocUrls"];

                    if (iActivityId == 0)
                    {
                        GenerateDocumentParameters(objDocumentModel, _leadDocUrls);
                        Session["LeadDocUrls"] = null;
                    }
                    else
                    {
                        GenerateDocumentParameters(objDocumentModel, _activityDocUrls);
                        Session["ActivityDocUrls"] = null;
                    }

                    response = client.PostAsJsonAsync("api/Documents/Insert?LeadId=" + iLeadId + " & ActivityId=" + iActivityId, objDocumentModel).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        objDocumentModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.DocumentModel>().Result;
                    }

                    return Json(new[] { objDocumentModel }.ToDataSourceResult(request));
                }
                else
                    return Content("");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult LeadDocumentCreate([DataSourceRequest] DataSourceRequest request, DocumentWithMultipleUrlsModel objDocumentWithMultipleUrlsModel,long leadId)
        {
            try
            {
                DocumentModel objDocumentModel = new DocumentModel();
                objDocumentModel.Id = 0;
                objDocumentModel.DocumentName = objDocumentWithMultipleUrlsModel.DocumentName;
                objDocumentModel.CreatedDate = objDocumentWithMultipleUrlsModel.CreatedDate;

                if (objDocumentWithMultipleUrlsModel.lstDocumentUrl.Count == 0)
                {
                  //  long iLeadId = Convert.ToInt64(Session["CurrentLeadId"]);
                    long iActivityId = Convert.ToInt64(Session["CurrentActivityId"]);

                    List<string> _leadDocUrls = (List<string>)Session["LeadDocUrls"];

                    objDocumentModel.TenantId = new Guid(Session["CurrentTenentId"].ToString());
                    objDocumentModel.URLs = _leadDocUrls;
                    objDocumentModel.Status = true;
                    objDocumentModel.UploadedById = Convert.ToInt64(Session["UserId"]);

                    response = client.PostAsJsonAsync("api/Documents/Insert?LeadId=" + leadId + " & ActivityId=" + iActivityId, objDocumentModel).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        objDocumentModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.DocumentModel>().Result;
                        objDocumentWithMultipleUrlsModel.DocumentName = objDocumentModel.DocumentName;
                        objDocumentWithMultipleUrlsModel.Id = objDocumentModel.Id;
                    }
                    else
                    {
                        objDocumentWithMultipleUrlsModel.DocumentName = null;
                    }
                  
                    Session["LeadDocUrls"] = null;
                    return Json(new[] { objDocumentWithMultipleUrlsModel }.ToDataSourceResult(request));
                    
                }
                else
                    //Session["LeadDocUrls"] = null;
                    return Content("");
            }
            catch
            {
                return View();
            }
        }

        private void GenerateDocumentParameters(DocumentModel objDocumentModel, List<string> urls)
        {
            long iLeadId = Convert.ToInt64(Session["CurrentLeadId"]);
            long iActivityId = Convert.ToInt64(Session["CurrentActivityId"]);

            foreach (var item in urls)
            {
                //Documents
                objDocumentModel.CreatedDate = DateTime.Now;
                objDocumentModel.ModifiedDate = DateTime.Now;
                objDocumentModel.URL = item;
                objDocumentModel.Status = true;
                objDocumentModel.UploadedById = Convert.ToInt64(Session["UserId"]);

                if (iActivityId == 0 && iLeadId != 0)
                    objDocumentModel.DocumentType = "Lead";
                else if (iActivityId != 0)
                    objDocumentModel.DocumentType = "Activity";

                objDocumentModel.TenantId = new Guid(Session["CurrentTenentId"].ToString());

                //response = client.PostAsJsonAsync("api/Documents/Insert?LeadId=" + iLeadId + " & ActivityId=" + iActivityId, objDocumentModel).Result;

                //if (response.IsSuccessStatusCode)
                //{
                //    objDocumentModel = response.Content.ReadAsAsync<NIMBOLE.Entities.Models.DocumentModel>().Result;
                //}
            }
        }

        #region Edit
        [HttpGet]
        // HTTP:GET  /Documents/Edit/1
        public ActionResult Edit(Int64 id)
        {
            response = client.GetAsync("api/Documents/GetById?id=" + id +"&Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objDocumentModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.DocumentModel>().Result;
                return View(objDocumentModel);
            }
            return View("Record Not Found");
        }
        // HTTP:PUT  /Documents/Edit/
        public async System.Threading.Tasks.Task<ActionResult> Edit(DocumentModel objDocumentModel)
        {

            try
            {
                objDocumentModel.TenantId = new Guid(Session["CurrentTenentId"].ToString());
                var response = await client.PutAsJsonAsync<DocumentModel>("api/Documents/Edit", objDocumentModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<DocumentModel>();
                    ViewData["objResultValue"] = objResultValue;
                    return Redirect("~/Documents/Index");
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
      //  [HttpDelete]
      //  public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, int id)
      //  {
      //      response = await client.DeleteAsync("api/Documents/Delete?id=" + id);
      //      if (response.IsSuccessStatusCode)
      //      {
      //          var objDocumentModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.DocumentModel>().Result;
      //          return Json(new[] { objDocumentModel }.ToDataSourceResult(request, ModelState));
      //      }
      //      return View("Record Not Found");
      //  }

        [HttpDelete]
        public async Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, int id)
        {
            try
            {
                response = await client.DeleteAsync("api/Documents/Delete?id=" + id);
                if (response.IsSuccessStatusCode)
                {
                    var objDocumentModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.DocumentModel>().Result;
                    return Json(new[] { objDocumentModel }.ToDataSourceResult(request, ModelState));
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