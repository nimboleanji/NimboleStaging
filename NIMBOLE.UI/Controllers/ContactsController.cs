using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using System.Web.UI;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using NIMBOLE.Models.Mappers;
using System.Web;
using NIMBOLE.Common;
using NIMBOLE.UI.Helpers;
using NIMBOLE.UI.Models;
using NIMBOLE.UI.Filters;

using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NIMBOLE.UI.Controllers
{
    public class ContactsController : BaseController
    {
        ContactExcelImport excelImport = new ContactExcelImport();
        Guid TenentId = new Guid();
        System.Uri PreviousUrl;
        public ContactsController()
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

        #region GET
        public JsonResult Contact_Read([DataSourceRequest] DataSourceRequest request, int Id = 0, string str = "", string contId = "")
        {
            int controlCount = Request.Form.AllKeys.Count();
            int AccountId = 0;
            string ContactId = string.Empty;

            if (Request.Form.Count > 0)
            {
                if (!string.IsNullOrEmpty(Request.Form["str"]))
                {
                    AccountId = Convert.ToInt32(Request.Form["str"].ToString());
                }

                if (!string.IsNullOrEmpty(Request.Form["contId"]))
                {
                    ContactId = Request.Form["contId"].ToString();
                }
            }

            if (AccountId == 0 || ContactId == string.Empty)
            {
                if (Id > 0)
                {
                    AccountId = Id;
                }
            }

            response = client.GetAsync("api/Contacts/GetAll?Id=" + AccountId + "&Tid=" + TenentId).Result;
            //response = client.GetAsync("api/Contacts/GetAll?Id=" + AccountId.ToString() + "&contId=" + ContactId.ToString()).Result;

            if (response.IsSuccessStatusCode)
            {
                var objTblContacts = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.ContactViewModel>>().Result;
                if (string.IsNullOrEmpty(contId))
                {
                    ViewData["Contacts"] = objTblContacts;
                    return Json(objTblContacts.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    contId = contId.ToLower();
                    ViewData["Contacts"] = objTblContacts;
                    return Json(objTblContacts.AsQueryable().Where(f => f.FullName.ToLower().Contains(contId)).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null);
        }
        public JsonResult GetAllContact([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Contacts/GetAllContact?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objTblContacts = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.ContactViewModel>>().Result;
                return Json(objTblContacts, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        //sreedhar added on 09Dec2015 
        public async Task<JsonResult> GetAllContactbyAccountId([DataSourceRequest] DataSourceRequest request, int id)
        {
            response = await client.GetAsync("api/Contacts/GetAllContactbyAccountId?Id=" + id + "&Tid=" + TenentId);
            if (response.IsSuccessStatusCode)
            {
                var objTblContacts = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.ContactViewModel>>().Result;
                return Json(objTblContacts, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }


        public JsonResult GetAllContactWithFilter([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Contacts/GetAllContact?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objTblContacts = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.ContactViewModel>>().Result.ToList();
                return Json(objTblContacts, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        public string GetDepartmentById(long iDeptId)
        {
            string strDepartmentName = "";
            response = client.GetAsync("api/Departments/GetById?Id=" + iDeptId.ToString() + "&Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objDepartmentModel = response.Content.ReadAsAsync<DepartmentModel>().Result;
                strDepartmentName = objDepartmentModel.DepartmentName;
            }
            return strDepartmentName;
        }
        public long GetDepartmentIdByName(string strDepartmentName)
        {
            long iDepartmentId = 0;
            response = client.GetAsync("api/Departments/GetById?Id=" + strDepartmentName.ToString() + "&Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objDepartmentModel = response.Content.ReadAsAsync<DepartmentModel>().Result;
                iDepartmentId = objDepartmentModel.Id;
            }
            return iDepartmentId;
        }

        #endregion

        #region Index
        public ActionResult Index(string Id)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                GetAllDropdownValues();
                return View();
            }
            return RedirectToAction("AccessDenied", "Error");
        }
        #endregion

        #region ListviewIndex
        [EncryptedActionParameter]
        [HttpGet]
        public ActionResult ListviewIndex([DataSourceRequest] DataSourceRequest request)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            int AccountId = 0;
            if (_objAuthorized)
            {
                response = client.GetAsync("api/Contacts/GetAll?Id=" + AccountId + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objContactViewModel = response.Content.ReadAsAsync<IEnumerable<ContactViewModel>>().Result;

                    return View(objContactViewModel);
                }
            }
            return RedirectToAction("AccessDenied", "Error");
           
        }

        #endregion

        #region Create
        // GET: Contacts/Create
        public ActionResult Create()
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                //GetAllDropdownValues();
                return View();
            }
            return RedirectToAction("AccessDenied", "Error");
        }
        // POST: Contacts/Create
        [HttpPost]
        public ActionResult Create(TransAccConModel objTransAccConModel, HttpPostedFileBase uploadFile)
        {
            try
            {
                #region New

                if (!string.IsNullOrEmpty(Request["hdnAccount"]))
                {
                    objTransAccConModel.objTransContactModel.AccountId = Convert.ToInt64(Request["hdnAccount"].ToString());
                }

                if (!string.IsNullOrEmpty(Request["hdnSource"]))
                {
                    objTransAccConModel.objContactModel.LeadSourceId = Convert.ToInt64(Request["hdnSource"].ToString());
                }

                if (!string.IsNullOrEmpty(Request["hdnDepartment"]))
                {
                    objTransAccConModel.objContactModel.DepartmentId = Convert.ToInt64(Request["hdnDepartment"].ToString());
                }

                if (!string.IsNullOrEmpty(Request["hdnDesignation"]))
                {
                    objTransAccConModel.objTransContactModel.ContactRoleId = Convert.ToInt32(Request["hdnDesignation"].ToString());
                }
                #endregion
                objTransAccConModel.objContactModel.TenantId = TenentId;
                response = client.PostAsJsonAsync("api/Contacts/Insert", objTransAccConModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objTransContactModelRes = response.Content.ReadAsAsync<TransContactModel>().Result;
                    var conid = objTransAccConModel.objContactModel.Id;
                    int id = Convert.ToInt32(objTransContactModelRes.ContactId);

                    objTransAccConModel.objContactModel.Id = id;

                    if (uploadFile != null && uploadFile.ContentLength > 0)
                    {
                        PhotoCloudStorage pcs;
                        pcs = new PhotoCloudStorage();
                        //Guid tenantId = objTransContactModelRes.TenantId.ToDefaultTenantId();
                        objTransAccConModel.objContactModel.Id = id;
                        objTransAccConModel.objContactModel.ContactImageURL = pcs.StoreInBlob(uploadFile, TenentId, id, "C");
                        objTransAccConModel.objContactModel.TenantId = TenentId;
                    }

                    var response1 = client.PostAsJsonAsync("api/Contacts/InsertImage", objTransAccConModel).Result;
                    if (response1.IsSuccessStatusCode)
                    {
                        objTransContactModelRes = response1.Content.ReadAsAsync<TransContactModel>().Result;
                    }

                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Contact Created Successfully."
                    });
                }
                else
                {
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Error,
                        MessageHeading = "Failure",
                        MessageString = response.ReasonPhrase
                    });
                }
                return RedirectToAction("ListviewIndex");
            }
            catch (Exception ex)
            {
                this.SetAlert(new AlertMessageViewModel()
                {
                    MessageType = MessageType.Error,
                    MessageHeading = "Failure",
                    MessageString = ex.Message
                });
                return RedirectToAction("ListviewIndex");
            }
        }

        [HttpPost]
        public ActionResult CreateLeadContact([DataSourceRequest] DataSourceRequest request, TransAccConModel objTransAccConModel, string item, string accid, string email)
        {
            try
            {

                long accId = Convert.ToInt64(accid);

                objTransAccConModel.objContactModel.FirstName = item;
                objTransAccConModel.objTransContactModel.AccountId = accId;
                objTransAccConModel.objContactModel.WorkEmail = email;
                objTransAccConModel.objContactModel.TenantId = TenentId;

                response = client.PostAsJsonAsync("api/Contacts/LeadInsert", objTransAccConModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var contact = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.ContactViewModel>>().Result;
                    return Json(contact.ToDataSourceResult(request));
                }
                return new HttpStatusCodeResult(500, "Email already exist.");
            }
            catch (Exception Ex)
            {
                this.SetAlert(new AlertMessageViewModel()
                {
                    MessageType = MessageType.Error,
                    MessageHeading = "Failure",
                    MessageString = Ex.Message
                });
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region Edit
        [HttpGet]
        [EncryptedActionParameter]
        public ActionResult Edit(int Id)
        {

             //PreviousUrl = System.Web.HttpContext.Current.Request.UrlReferrer;


            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                GetAllDropdownValues();
                 response = client.GetAsync("api/Contacts/GetById?id=" + Id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    string errVal = response.Content.ReadAsStringAsync().Result;
                    if (!errVal.Contains("Failure"))
                    {
                        TransAccConModel _objTransAccConModel = response.Content.ReadAsAsync<TransAccConModel>().Result;
                        return View(_objTransAccConModel);
                    }
                    else
                    {
                        this.SetAlert(new AlertMessageViewModel()
                        {
                            MessageType = MessageType.Error,
                            MessageHeading = "Failure",
                            MessageString = "Record Not Found"
                        });
                        return RedirectToAction("ListviewIndex");
                    }
                }
                return View("Record Not Found");
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(TransAccConModel objTransAccConModel, HttpPostedFileBase uploadFile)
        {
            try
            {
                #region New
                if (!string.IsNullOrEmpty(Request["hdnOwnership"]))
                {
                    objTransAccConModel.objTransContactModel.ContactRoleId = Convert.ToInt64(Request["hdnDesignation"].ToString());
                }
                #endregion
                int id = Convert.ToInt32(objTransAccConModel.objContactModel.Id);
                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    PhotoCloudStorage pcs;
                    pcs = new PhotoCloudStorage();
                    //Guid tenantId = objTransAccConModel.objContactModel.TenantId.ToDefaultTenantId();
                    objTransAccConModel.objContactModel.ContactImageURL = pcs.StoreInBlob(uploadFile, TenentId, id, "C");
                   
                }
                objTransAccConModel.objContactModel.TenantId = TenentId;
                var response = client.PutAsJsonAsync<TransAccConModel>("api/Contacts/Edit", objTransAccConModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var _objTransAccConModel = await response.Content.ReadAsAsync<TransAccConModel>();
                    ViewData["objResultValue"] = _objTransAccConModel;

                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Contact Updated Successfully"
                    });
                }
                else
                {
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Error,
                        MessageHeading = "Failure",
                        MessageString = response.ReasonPhrase
                    });
                }
                return RedirectToAction("ListviewIndex");
            }
     

            //    string view = Session["viewtype"].ToString();
            //    if (view != null)
            //    {
            //        return RedirectToAction("ListviewIndex", "Contacts", new { q = UrlExtensions.EncodeString("ViewType=" + view) });
            //    }
            //    else
            //    {
            //        return RedirectToAction("ListviewIndex");
            //    }
            //}
            catch (Exception ex)
            {
                this.SetAlert(new AlertMessageViewModel()
                {
                    MessageType = MessageType.Error,
                    MessageHeading = "Failure",
                    MessageString = ex.Message
                });
                return RedirectToAction("ListviewIndex");
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> EditById([DataSourceRequest] DataSourceRequest request, long Id, string contactEmail)
        {
            try
            {
                ContactModel objContactModel = new ContactModel();
                objContactModel.Id = Id;
                objContactModel.ContactEmail = contactEmail;
                var response = await client.PutAsJsonAsync("api/Contacts/EditById?Tid=" + TenentId, objContactModel);
                if (response.IsSuccessStatusCode)
                {

                    var contact = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.ContactViewModel>>().Result;
                    return Json(contact.ToDataSourceResult(request));
                }
                else
                {
                    return Content("");
                }
            }
            catch
            {
                return Content("");
            }
        }
        #endregion

        #region Delete
        [HttpDelete]
        [Route("Delete")]
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, ContactModel objContactModel, string [] selectedId, bool status)
        {
            try
            {
                var strselectedId = string.Join(",", selectedId);
                response = await client.DeleteAsync(strAPIURL + "api/Contacts/Delete?selectedId=" + strselectedId + "&status=" + status);
                if (response.IsSuccessStatusCode)
                {
                    //objContactModel = response.Content.ReadAsAsync<ContactModel>().Result;
                    return Json(new[] { objContactModel }.ToDataSourceResult(request, ModelState));
                }
                else 
                {
                    return Json(response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region associated
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> DeleteRead([DataSourceRequest] DataSourceRequest request, string id, bool status)
        {
            try
            {
                response = await client.DeleteAsync(strAPIURL + "api/Contacts/DeleteRead?id=" + id + "&status=" + status);
                if (response.IsSuccessStatusCode)
                {
                    //var objaccountmodel = response.Content.ReadAsAsync<NimboleAccountModel>().Result;
                  var objContactModel = response.Content.ReadAsAsync<ContactModel>().Result;
                    return Json(new[] { objContactModel }.ToDataSourceResult(request));
                }
                else
                {
                    return Json(response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region CotactDelete
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> ContactDelete([DataSourceRequest] DataSourceRequest request, string [] selectedId)
        {
            try
            {
                var strselectedId = string.Join(",", selectedId);
                response = await client.DeleteAsync(strAPIURL + "api/Contacts/DeleteById?selectedId=" + strselectedId);
                if(response.IsSuccessStatusCode)
                {
                    var objContactmodel = response.Content.ReadAsAsync<TransAccConModel>().Result;
                    return Json(new[] { objContactmodel }.ToDataSourceResult(request));
                }
                else
                {
                    return Json(response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }
        #endregion

        #region ExcelImport
        //[Authorize]
        public ActionResult ExcelImport()
        {
            return View(excelImport);
        }
        //[Authorize]
        [HttpPost]
        public ActionResult ExcelImport(ContactExcelImport CExcelImport, string btnSubmit, int[] selectedId)
        {
            //  ContactExcelImport excelImport = new ContactExcelImport();

            string corruptedDataError = string.Empty;
            try
            {
                string fileExtension = string.Empty;
                int importContactId = 0;
                int id = 0;

                string strFirstName = string.Empty;
                string strContactEmail = string.Empty;
                string strDepartment = string.Empty;
                string strDesignation = string.Empty;
                string strAddress1 = string.Empty;
                string strCountry = string.Empty;
                string strCity = string.Empty;
                string strMobile = string.Empty;
                string strFax = string.Empty;
                string strSkypeName = string.Empty;


                string strMiddleName = string.Empty;
                string strLastName = string.Empty;
                string strWorkEmail = string.Empty;
                string strLeadSource = string.Empty;
                string strAccountName = string.Empty;
                string strAddress2 = string.Empty;
                string strState = string.Empty;
                string strZipCode = string.Empty;
                string strOfficePhone = string.Empty;
                string strHomePhone = string.Empty;
                string strComments = string.Empty;

                string strReportsTo = string.Empty;

                int countryid = 0;
                int stateid = 0;
                
               
                // int AccountId = 1;                // sreedhar added on 20/10/2015
                
                

                //Guid TenentId = Guid.Empty;

                //TenentId = (new Guid(Session["CurrentTenentId"].ToString()));

                switch (btnSubmit)
                {
                    case "Upload":
                        //if (ModelState.IsValid)
                        //{
                        if (CExcelImport.ImportFile != null)
                        {
                            fileExtension = System.IO.Path.GetExtension(CExcelImport.ImportFile.FileName);
                            if (fileExtension == ".xlsx" || fileExtension == ".csv")
                            {
                                using (MemoryStream excelDocumentMemoryStream = new MemoryStream())
                                {


                                    CExcelImport.ImportFile.InputStream.CopyTo(excelDocumentMemoryStream);

                                    SpreadsheetDocument spreadsheetdocument = SpreadsheetDocument.Open(excelDocumentMemoryStream, true);
                                    WorkbookPart wbPart = spreadsheetdocument.WorkbookPart;
                                    Sheet sheet = wbPart.Workbook.Descendants<Sheet>().FirstOrDefault();
                                    WorksheetPart wsPart = (WorksheetPart)(wbPart.GetPartById(sheet.Id));
                                    Worksheet wsSheet = wsPart.Worksheet;


                                    Dictionary<string, int> labels = new Dictionary<string, int>();

                                    labels.Add("First Name", 0);
                                    labels.Add("Last Name", 0);
                                    labels.Add("Contact Email", 0);
                                    labels.Add("Work Email", 0);
                                    labels.Add("Department", 0);
                                    labels.Add("Lead Source", 0);
                                    labels.Add("Designation", 0);
                                    labels.Add("Account Name", 0);
                                    labels.Add("Address1", 0);
                                    labels.Add("Address2", 0);
                                    labels.Add("Country", 0);
                                    labels.Add("State", 0);
                                    labels.Add("City", 0);
                                    labels.Add("Zip Code", 0);
                                    labels.Add("Mobile", 0);
                                    labels.Add("Office Phone", 0);
                                    labels.Add("Fax", 0);
                                    labels.Add("Home Phone", 0);
                                    labels.Add("Skype Name", 0);
                                    labels.Add("Comments", 0);
                                   

                                    labels = SetSheetLabels(wbPart, wsSheet, 1, labels);

                                    if (labels.Count > 0 && (labels.ContainsKey("Invalid") == true))
                                    {

                                        ViewData["InvalidHeaders"] = "Invalid excel headers,for sample please click on 'Download Excel Template'.";
                                        break;
                                    }
                                    int rowIndex = 2;

                                    while (true)
                                    {
                                        Dictionary<string, string> values = new Dictionary<string, string>();
                                        values = GetSheetValues(wbPart, wsSheet, rowIndex, labels);
                                        if (values.FirstOrDefault().Value.Length == 0)
                                            break;

                                        strFirstName = values["First Name"];
                                        strLastName = values["Last Name"];
                                        strContactEmail = values["Contact Email"];
                                        strWorkEmail = values["Work Email"];
                                        strDepartment = values["Department"];
                                        strLeadSource = values["Lead Source"];
                                        strDesignation = values["Designation"];
                                        strAccountName = values["Account Name"];
                                        strAddress1 = values["Address1"];
                                        strAddress2 = values["Address2"];
                                        strCountry = values["Country"];
                                        strState = values["State"];
                                        strCity = values["City"];
                                        strZipCode = values["Zip Code"];
                                        strMobile = values["Mobile"];
                                        strOfficePhone = values["Office Phone"];
                                        strFax = values["Fax"];
                                        strHomePhone = values["Home Phone"];
                                        strSkypeName = values["Skype Name"];
                                        strComments = values["Comments"];
                                        
                                        
                                        if (!string.IsNullOrEmpty(strFirstName))
                                            strFirstName = strFirstName.Trim();

                                        //&& ((!string.IsNullOrEmpty(strLastName)) && (!string.IsNullOrWhiteSpace(strLastName))) strAccountName

                                        if (((!string.IsNullOrEmpty(strFirstName)) && (!string.IsNullOrWhiteSpace(strFirstName))) && ((!string.IsNullOrEmpty(strWorkEmail)) && (!string.IsNullOrWhiteSpace(strWorkEmail))) && ((!string.IsNullOrEmpty(strAccountName)) && (!string.IsNullOrWhiteSpace(strAccountName))))
                                        {
                                            excelImport.ExcelImport.Add(new ExcelImport()
                                            {
                                                Id = id,
                                                FirstName       = strFirstName,
                                                LastName        = strLastName,
                                                ContactEmail    = strContactEmail, 
                                                WorkEmail       = strWorkEmail,
                                                DepartmentName      = strDepartment,
                                                LeadSource      = strLeadSource,
                                                Designation     = strDesignation,
                                                AccountName     = strAccountName,
                                                Address1        = strAddress1,
                                                Address2        = strAddress2,
                                                CountryName         = strCountry,
                                                StateName           = strState,
                                                CityName            = strCity,
                                                ZipCode         = strZipCode,
                                                Mobile          = strMobile,
                                                OfficePhone     = strOfficePhone,
                                                Fax             = strFax,
                                                HomePhone       = strHomePhone,
                                                SkypeName       = strSkypeName,
                                                Comments        = strComments
                                               
                                            });
                                        }
                                        id++;

                                        rowIndex++;
                                    }

                                    Session["ExcelImport"] = excelImport.ExcelImport;
                                }
                            }
                        }


                        //}
                        break;

                    case "ExcelTemplate":

                        var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("excelimports"));
                        //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("nimboleStorage"));
                        CloudBlobClient cloudBlobClient = account.CreateCloudBlobClient();
                        CloudBlobContainer BlobContainer = cloudBlobClient.GetContainerReference("nimboleexcelimports");
                        CloudBlob blob = BlobContainer.GetBlobReference("Contacts_Nimbole.xlsx");
                        MemoryStream memStream = new MemoryStream();
                        blob.DownloadToStream(memStream);

                        Response.ContentType = blob.Properties.ContentType;
                        Response.AddHeader("Content-Disposition", "Attachment; filename=Contacts_Nimbole.xlsx");
                        Response.AddHeader("Content-Length", blob.Properties.Length.ToString());
                        Response.BinaryWrite(memStream.ToArray());

                        break;

                    case "Import":

                        if ((Session["ExcelImport"] != null))
                        {
                            List<ExcelImport> ExcelImportContacts = (List<ExcelImport>)Session["ExcelImport"];


                            AddressContactModel objAddressContactModel = new AddressContactModel();

                            var selectedItems = (from ci in ExcelImportContacts where selectedId.Contains(ci.Id) select ci).ToList();


                            if (selectedItems.Count > 0)
                            {
                                foreach (var items in selectedItems)
                                {


                                    strFirstName        =   items.FirstName    ;
                                    strLastName         =   items.LastName     ;
                                    strContactEmail     =   items.ContactEmail ;
                                    strWorkEmail        =   items.WorkEmail    ;
                                    strDepartment       =   items.DepartmentName   ;
                                    strLeadSource       =   items.LeadSource   ;
                                    strDesignation      =   items.Designation  ;
                                    strAccountName      =   items.AccountName  ;
                                    strAddress1         =   items.Address1     ;
                                    strAddress2         =   items.Address2     ;
                                    strCountry          =   items.CountryName      ;
                                    strState            =   items.StateName        ;
                                    strCity             =   items.CityName         ;
                                    strZipCode          =   items.ZipCode      ;
                                    strMobile           =   items.Mobile       ;
                                    strOfficePhone      =   items.OfficePhone  ;
                                    strFax              =   items.Fax          ;
                                    strHomePhone        =   items.HomePhone    ;
                                    strSkypeName        =   items.SkypeName    ;
                                    strComments         =   items.Comments     ;  


                                    if (!string.IsNullOrEmpty(items.WorkEmail))
                                        if (emailIsValid(items.WorkEmail))
                                        {


                                            response = client.GetAsync("api/Contacts/GetByEmail?Email=" + strWorkEmail + "&Tid=" + TenentId).Result;

                                            ContactModel objContactModel = new ContactModel();

                                            if (response.IsSuccessStatusCode)
                                            {

                                                objContactModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.ContactModel>().Result;
                                            }

                                            //response = client.GetAsync("api/Contacts/GetByEmail?Email=" + strWorkEmail + "&Tid=" + TenentId).Result;


                                            // GetByAccName
                                           
                                            // if (objContactModel == null)

                                            if (response.IsSuccessStatusCode == false)
                                            {

                                                TransAccConModel objTransAccConModel = new TransAccConModel();
                                                objTransAccConModel.objContactModel.TenantId = TenentId;
                                                objTransAccConModel.objContactModel.FirstName = strFirstName;
                                                objTransAccConModel.objContactModel.LastName = strLastName;
                                                objTransAccConModel.objContactModel.ContactEmail = strContactEmail;
                                                objTransAccConModel.objContactModel.WorkEmail = strWorkEmail;
                                                objTransAccConModel.objContactModel.TenantId = TenentId;

                                                if (!string.IsNullOrEmpty(strLeadSource))
                                                {

                                                    response = client.GetAsync("api/LeadSource/GetIdByLeadSource?LeadSource=" + strLeadSource + "&Tid=" + TenentId).Result;
                                                    if (response.IsSuccessStatusCode)
                                                    {
                                                        objTransAccConModel.objContactModel.LeadSourceId  = response.Content.ReadAsAsync<long>().Result;
                                                    }
                                                }

                                                if (!string.IsNullOrEmpty(strDepartment))
                                                {

                                                    response = client.GetAsync("api/Departments/GetIdByDepartment?Department=" + strDepartment + "&Tid=" + TenentId).Result;
                                                    if (response.IsSuccessStatusCode)
                                                    {
                                                        objTransAccConModel.objContactModel.DepartmentId = response.Content.ReadAsAsync<long>().Result;
                                                    }
                                                }


                                                objTransAccConModel.objContactModel.CreatedDate = DateTime.Now;
                                                objTransAccConModel.objContactModel.ModifiedDate = DateTime.Now;
                                                objTransAccConModel.objContactModel.Status = true;
                                                objTransAccConModel.objContactModel.Comments = strComments;


                                                //objTransAccConModel.objAddressModel.Country = strCountry;
                                                //objTransAccConModel.objAddressModel.State = strState;
                                                //objTransAccConModel.objAddressModel.City = strCity;

                                                objTransAccConModel.objAddressModel.Mobile = strMobile;
                                                objTransAccConModel.objAddressModel.Phone = strOfficePhone;
                                                objTransAccConModel.objAddressModel.HomePhone = strHomePhone;
                                                objTransAccConModel.objAddressModel.Fax = strFax;
                                                objTransAccConModel.objAddressModel.ZipCode = strZipCode;
                                                objTransAccConModel.objAddressModel.HouseNo = strAddress1;
                                                objTransAccConModel.objAddressModel.StreetName = strAddress2;
                                                objTransAccConModel.objAddressModel.SkypeName = strSkypeName;


                                                if (!string.IsNullOrEmpty(strAccountName))
                                                {

                                                    response = client.GetAsync("api/NimboleAccounts/GetIdByAccName?AccName=" + strAccountName + "&Tid=" + TenentId).Result;
                                                    if (response.IsSuccessStatusCode)
                                                    {
                                                        objTransAccConModel.objTransContactModel.AccountId = response.Content.ReadAsAsync<long>().Result;
                                                    }
                                                }


                                                if (!string.IsNullOrEmpty(strDesignation))
                                                {

                                                    response = client.GetAsync("api/ContactRoles/GetIdByDesignation?Designation=" + strDesignation + "&Tid=" + TenentId).Result;
                                                    if (response.IsSuccessStatusCode)
                                                    {
                                                        objTransAccConModel.objTransContactModel.ContactRoleId = response.Content.ReadAsAsync<long>().Result;
                                                    }
                                                }


                                                if (!string.IsNullOrEmpty(strCountry))
                                                {

                                                    response = client.GetAsync("api/AddressAutoComplete/GetIdByCountry?Country=" + strCountry+"&Tid="+TenentId).Result;
                                                    if (response.IsSuccessStatusCode)
                                                    {
                                                        objTransAccConModel.objAddressModel.CountryId = response.Content.ReadAsAsync<long>().Result;
                                                        countryid = Convert.ToInt32(objTransAccConModel.objAddressModel.CountryId);
                                                        
                                                    }
                                                }


                                                if (!string.IsNullOrEmpty(strState))
                                                {

                                                    response = client.GetAsync("api/AddressAutoComplete/GetIdByState?State=" + strState + "&Countryid=" + countryid+"&Tid="+TenentId).Result;
                                                    if (response.IsSuccessStatusCode)
                                                    {
                                                        objTransAccConModel.objAddressModel.StateId = response.Content.ReadAsAsync<long>().Result;
                                                        stateid = Convert.ToInt32(objTransAccConModel.objAddressModel.StateId);
                                                    }
                                                }


                                                if (!string.IsNullOrEmpty(strCity))
                                                {

                                                    response = client.GetAsync("api/AddressAutoComplete/GetIdByCity?City=" + strCity + "&Stateid=" + stateid+"&Tid="+TenentId).Result;
                                                    if (response.IsSuccessStatusCode)
                                                    {
                                                        objTransAccConModel.objAddressModel.CityId = response.Content.ReadAsAsync<long>().Result;
                                                    }
                                                }



                                                response = client.PostAsJsonAsync("api/Contacts/Insert", objTransAccConModel).Result;
                                                if (response.IsSuccessStatusCode)
                                                {
                                                    objAddressContactModel = response.Content.ReadAsAsync<AddressContactModel>().Result;
                                                    importContactId = Convert.ToInt32(objTransAccConModel.objContactModel.Id);
                                                }


                                            }
                                            else
                                            {

                                            }

                                        }

                                        else
                                        {

                                        }

                                }
                                return Content("Contact(s) Successfully imported!");
                            }

                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "File contains corrupted data.")
                    corruptedDataError = "File contains corrupted data, Please upload proper excel again.";

                else if (ex.Message == "Invalid Hyperlink:Malformed URI is embedded as a hyperlink in the document.")
                    corruptedDataError = "File contains incorrect data, Please enter correctly.";

                else
                    throw ex;
            }
            finally
            {

                if (!string.IsNullOrEmpty(corruptedDataError))
                    excelImport.InvalidHeaders = corruptedDataError;

            }

            return View(excelImport);
        }
        private Dictionary<string, int> SetSheetLabels(WorkbookPart wbPart, Worksheet wsSheet, int rowIndex, Dictionary<string, int> labels)
        {
            Dictionary<string, int> results = new Dictionary<string, int>();
            string[] colLabels = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            int colLabelIndex = 0;
            foreach (string strFindLabel in labels.Keys)
            {
                //repeat:
                Cell cell = wsSheet.Descendants<Cell>().Where(c => c.CellReference == colLabels[colLabelIndex] + rowIndex.ToString().Trim()).FirstOrDefault();
                string value = "";
                if (cell != null)
                {
                    value = cell.InnerText;
                    if (cell.DataType != null)
                    {
                        if (cell.DataType == CellValues.SharedString)
                        {
                            value = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault()
                                .SharedStringTable
                            .ElementAt(int.Parse(value)).InnerText;
                        }

                    }

                }
                if (value.Equals(strFindLabel))
                {
                    results.Add(value, colLabelIndex);
                }
                else
                {
                    //Clearing the results dictionary and adding the value
                    results.Clear();
                    results.Add("Invalid", colLabelIndex);
                    break;

                }
                colLabelIndex++;

            }
            return results;
        }

        private Dictionary<string, string> GetSheetValues(WorkbookPart wbPart, Worksheet wsSheet, int rowIndex, Dictionary<string, int> labels)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            string[] colLabels = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            foreach (KeyValuePair<string, int> labelPair in labels)
            {
                Cell cell = wsSheet.Descendants<Cell>().Where(c => c.CellReference == colLabels[labelPair.Value] + rowIndex.ToString().Trim()).FirstOrDefault();
                string value = "";
                if (cell != null)
                {
                    value = cell.InnerText;
                    if (cell.DataType != null)
                    {
                        if (cell.DataType == CellValues.SharedString)
                        {
                            value = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault()
                                .SharedStringTable
                        .ElementAt(int.Parse(value)).InnerText;
                        }

                    }

                }
                values.Add(labelPair.Key, value);
            }
            return values;

        }

        #endregion

        public static bool emailIsValid(string email)
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
          
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #region Excelexport

        [HttpGet]
        public ActionResult Excel_Export()
        {

            List<ExcelImport> ContactReports = new List<ExcelImport>();

            List<ExcelImport> contact = new List<ExcelImport>();


            response = client.GetAsync("api/Contacts/GetAllContactExport?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)

                contact = response.Content.ReadAsAsync<IEnumerable<ExcelImport>>().Result.ToList();
            
            string strLeadSource = string.Empty;
            string strDepartment = string.Empty;

            foreach (var item in contact)
            {
                ContactReports.Add(new ExcelImport() { FirstName = item.FirstName,LastName=item.LastName,ContactEmail=item.ContactEmail,WorkEmail=item.WorkEmail,LeadSource=item.LeadSource,DepartmentName=item.DepartmentName,Designation=item.Designation,AccountName=item.AccountName,Address1=!string.IsNullOrEmpty(item.Address1)?item.Address1.Replace(',',' ') : item.Address1,Address2=!string.IsNullOrEmpty(item.Address2)?item.Address2.Replace(',',' ') : item.Address2,CountryName=item.CountryName,StateName=item.StateName,CityName=item.CityName,ZipCode=item.ZipCode,Mobile=item.Mobile,OfficePhone=item.OfficePhone,Fax=item.Fax,HomePhone=item.HomePhone,SkypeName=item.SkypeName,Comments=!string.IsNullOrEmpty(item.Comments)?item.Comments.Replace(',', ' ') : item.Comments });
            }


            Response.AddHeader("content-disposition", "attachment; filename=ContactExport.csv");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";

            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);


            htw.WriteLine("First Name" + "," + "Last Name" + "," + "Contact Email" + "," + "Work Email" + "," + "Department" + "," + "Lead Source" + "," + "Designation" + "," + "Account Name" + "," + "Address1" + "," + "Address2" + "," + "Country" + "," + "State" + "," + "City" + "," + "Zip Code" + "," + "Mobile" + "," + "Office Phone" + "," + "Fax" + "," + "Home Phone" + "," + "Skype Name" + "," + "Comments");

            if (ContactReports != null)
            {
                foreach (var data in ContactReports)
                {                    
                    if (string.IsNullOrEmpty(data.FirstName))
                    {
                        data.FirstName = "Null";
                    }
                    if (string.IsNullOrEmpty(data.LastName))
                    {
                        data.LastName = "Null";
                    }
                    if (string.IsNullOrEmpty(data.ContactEmail))
                    {
                        data.ContactEmail = "Null";
                    }
                    if (string.IsNullOrEmpty(data.WorkEmail)) 
                    {
                        data.WorkEmail = "Null";
                    }
                    if (string.IsNullOrEmpty(data.DepartmentName))
                    {
                        data.DepartmentName = "Null";
                    }
                    if (string.IsNullOrEmpty(data.LeadSource))
                    {
                        data.LeadSource = "Null";
                    }                    
                    if (string.IsNullOrEmpty(data.Designation))
                    {
                        data.Designation = "Null";
                    }
                    if (string.IsNullOrEmpty(data.AccountName))
                    {
                        data.AccountName = "Null";
                    }
                    if (string.IsNullOrEmpty(data.Address1))
                    {
                        data.Address1 = "Null";
                    }
                    if (string.IsNullOrEmpty(data.Address2))
                    {
                        data.Address2 = "Null";
                    }
                    if (string.IsNullOrEmpty(data.CountryName))
                    {
                        data.CountryName = "Null";
                    }
                    if (string.IsNullOrEmpty(data.StateName))
                    {
                        data.StateName = "Null";
                    }
                    if (string.IsNullOrEmpty(data.CityName))
                    {
                        data.CityName = "Null";
                    }
                    if (string.IsNullOrEmpty(data.ZipCode))
                    {
                        data.ZipCode = "Null";
                    }
                    if (string.IsNullOrEmpty(data.Mobile))
                    {
                        data.Mobile = "Null";
                    }
                    if (string.IsNullOrEmpty(data.OfficePhone))
                    {
                        data.OfficePhone = "Null";
                    }
                    if (string.IsNullOrEmpty(data.Fax))
                    {
                        data.Fax = "Null";
                    }
                    if (string.IsNullOrEmpty(data.HomePhone))
                    {
                        data.HomePhone = "Null";
                    }
                    if (string.IsNullOrEmpty(data.SkypeName))
                    {
                        data.SkypeName = "Null";
                    }
                    if (string.IsNullOrEmpty(data.Comments))
                    {
                        data.Comments = "Null";
                    }

                    
                    if (!string.IsNullOrEmpty(data.FirstName))
                    {
                        htw.WriteLine(data.FirstName.ToString() + "," + data.LastName.ToString() + "," + data.ContactEmail.ToString() + "," + data.WorkEmail.ToString() + "," + data.DepartmentName.ToString() + "," + data.LeadSource.ToString() + "," + data.Designation.ToString() + "," + data.AccountName.ToString() + "," + data.Address1.ToString() + "," + data.Address2.ToString() + "," + data.CountryName.ToString() + "," + data.StateName.ToString() + "," + data.CityName.ToString() + "," + data.ZipCode.ToString() + "," + data.Mobile.ToString() + "," + data.OfficePhone.ToString() + "," + data.Fax.ToString() + "," + data.HomePhone.ToString() + "," + data.SkypeName.ToString() + "," + data.Comments.ToString());
                    }

                }
            }

            Response.Output.Write(sw.ToString());

            Response.Flush();
            Response.End();

            // return RedirectToAction("ListviewIndex");
            return Content("");

        }
        #endregion

        private void GetAllDropdownValues()
        {
            //departments
            response = client.GetAsync("api/departments/selectdepartmentsfordropdown?tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objdepartmentmodel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                ViewData["departments"] = objdepartmentmodel;
            }
           // designations
            response = client.GetAsync("api/contactroles/selectcontactrolesfordropdown?tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var roles = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                ViewData["designations"] = roles;
            }
           // leadsources
            response = client.GetAsync("api/leadsource/selectallleadsource?tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objleadsourcemodel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                ViewData["leadsources"] = objleadsourcemodel;
            }
          //  Accounts
            response = client.GetAsync("api/NimboleAccounts/SelectAllAccount?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objAccountModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                ViewData["Accounts"] = objAccountModel;
            }
            //Countries
            //response = client.GetAsync("api/AddressAutoComplete/Countries").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var Countries = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.CountryModel>>().Result.ToList().OrderBy(x => x.CountryName);
            //    ViewData["Countries"] = new SelectList(Countries, "CountryId", "CountryName", new object { });
            //}
        }
    }
}
