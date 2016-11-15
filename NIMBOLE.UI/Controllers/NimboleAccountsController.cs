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

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Web.UI;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Common;
using NIMBOLE.UI.Models;
using NIMBOLE.UI.Filters;
using System.Threading.Tasks;

namespace NIMBOLE.UI.Controllers
{
    public class NimboleAccountsController : BaseController
    {
        AccountExcelImport excelImport = new AccountExcelImport();
        Guid TenentId = new Guid();
        public NimboleAccountsController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }

        #region GET
        [HttpPost]
        public JsonResult GetAllAccounts()
        {
            response = client.GetAsync("api/NimboleAccounts/SelectAllAccount?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objAccountModel = response.Content.ReadAsAsync<IEnumerable<NimboleAccountModel>>().Result.ToList();
                return Json(objAccountModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpPost]
        public JsonResult GetAllOwnerships()
        {
            response = client.GetAsync("api/NimboleAccounts/GetAllOwnerships?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var _objOwnershipModel = response.Content.ReadAsAsync<IEnumerable<OwnershipModel>>().Result.ToList();
                return Json(_objOwnershipModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public JsonResult GetAllOwnershipsForCombo()
        {
            response = client.GetAsync("api/NimboleAccounts/GetAllOwnerships?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var _objOwnershipModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                return Json(_objOwnershipModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public JsonResult GetAllParentAccount()
        {
            response = client.GetAsync("api/NimboleAccounts/SelectAllParentAccounts?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var _objParentAccountModel = response.Content.ReadAsAsync<IEnumerable<ParentAccountNewModel>>().Result.ToList();
                return Json(_objParentAccountModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        //public JsonResult GetAllParentAccountForCombo()
        //{
        //    response = client.GetAsync("api/NimboleAccounts/SelectAllParentAccounts?Tid=" + TenentId).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var _objParentAccountModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
        //        return Json(_objParentAccountModel, JsonRequestBehavior.AllowGet);
        //    }
        //    return null;
        //}
        public JsonResult GetAllIndustry()
        {
            response = client.GetAsync("api/NimboleAccounts/SelectAllIndustries?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var _objIndustryModel = response.Content.ReadAsAsync<IEnumerable<IndustryModel>>().Result.ToList();
                return Json(_objIndustryModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public JsonResult GetAllIndustryForCombo()
        {
            response = client.GetAsync("api/NimboleAccounts/SelectAllIndustries?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var _objIndustryModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                return Json(_objIndustryModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public JsonResult GetAllDistributors()
        {
            response = client.GetAsync("api/NimboleAccounts/SelectAllDistributorNames?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var _objDistributorModel = response.Content.ReadAsAsync<IEnumerable<DistributorModel>>().Result.ToList();
                return Json(_objDistributorModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public JsonResult GetAllDistributorsForCombo()
        {
            response = client.GetAsync("api/NimboleAccounts/SelectAllDistributorNames?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var _objDistributorModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                return Json(_objDistributorModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        //  public static List<ListItem> SelectAllAccountType()
        public ActionResult SelectAllAccountType()
        {
            response = client.GetAsync("api/NimboleAccounts/SelectAllAccountType?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objAccountType = response.Content.ReadAsAsync<IEnumerable<AccountTypeModel>>().Result.ToList();
                return Json(objAccountType, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpGet]
        public ActionResult SelectAllAccountTypeForCombo()
        {
            response = client.GetAsync("api/NimboleAccounts/SelectAllAccountType?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objAccountType = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                return Json(objAccountType, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpGet]
        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/NimboleAccounts/GetAll?Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objAccountViewModel = response.Content.ReadAsAsync<IEnumerable<AccountViewModel>>().Result;
                    ViewData["NimboleAccounts"] = objAccountViewModel;
                    return View(objAccountViewModel);
                }
            }
            return RedirectToAction("AccessDenied", "Error");
        }
        public async Task<ActionResult> GetAccountwithFilter([DataSourceRequest] DataSourceRequest request)
        {
            response = await client.GetAsync("api/NimboleAccounts/SelectAllAccount?Tid=" + TenentId);
            if (response.IsSuccessStatusCode)
            {
                var objAccountModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                return Json(objAccountModel, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        public JsonResult Account_Read([DataSourceRequest] DataSourceRequest request, string acctname)
        {
            response = client.GetAsync("api/NimboleAccounts/GetAll?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objTblAccounts = response.Content.ReadAsAsync<List<AccountViewModel>>().Result;
                ViewData["AccountDetails"] = objTblAccounts;
                if (string.IsNullOrEmpty(acctname))
                {
                    return Json(objTblAccounts.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    acctname = acctname.ToLower();
                    return Json(objTblAccounts.AsQueryable().Where(a => a.AccountName.ToLower().Contains(acctname)).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                }
            }
            ViewData["AccountDetails"] = null;
            return Json(null);

        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                GetAllDropdownValues();
                return View();
            }
            return RedirectToAction("AccessDenied", "Error");
        }
        [HttpPost]
        public ActionResult Create(NimboleAccountModel objNimboleAccountModel)
        {
            try
            {
                #region New
                if (!string.IsNullOrEmpty(Request["hdnOwnership"]))
                {
                    objNimboleAccountModel.OwnerShip = Request["hdnOwnership"].ToString();
                }

                if (!string.IsNullOrEmpty(Request["hdnAccountType"]))
                {
                    objNimboleAccountModel.AccountType = Request["hdnAccountType"].ToString();
                }

                if (!string.IsNullOrEmpty(Request["hdnIndustry"]))
                {
                    objNimboleAccountModel.Industry = Request["hdnIndustry"].ToString();
                }

                if (!string.IsNullOrEmpty(Request["hdnDistributor"]))
                {
                    objNimboleAccountModel.DistributerName = Request["hdnDistributor"].ToString();
                }

                if (!string.IsNullOrEmpty(Request["hdnParentAccount"]))
                {
                    objNimboleAccountModel.ParentAccount = Request["hdnParentAccount"].ToString();
                }
                #endregion

                objNimboleAccountModel.TenantId = TenentId;
                response = client.PostAsJsonAsync("api/NimboleAccounts/Insert", objNimboleAccountModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var accounts = response.Content.ReadAsAsync<string>().Result;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Account Created Successfully."
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
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.SetAlert(new AlertMessageViewModel()
                {
                    MessageType = MessageType.Error,
                    MessageHeading = "Failure",
                    MessageString = ex.Message
                });
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// Modified by Ravi for Combobox
        [HttpPost]
        public ActionResult CreateAccount([DataSourceRequest] DataSourceRequest request, NimboleAccountModel objNimboleAccountModel, string item)
        {
            try
            {
                objNimboleAccountModel.TenantId = TenentId;
                objNimboleAccountModel.AccountName = item;
                response = client.PostAsJsonAsync("api/NimboleAccounts/InsertAccount", objNimboleAccountModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var account = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result;
                    ViewData["Accounts"] = account;
                    return Json(account.ToDataSourceResult(request));
                }
                else
                {
                    return Json(new[] { objNimboleAccountModel }.ToDataSourceResult(request, ModelState));
                }
            }
            catch
            {
                return Content("");
            }
        }
        [HttpPost]
        public ActionResult ParentCreate([DataSourceRequest] DataSourceRequest request, NimboleAccountModel objNimboleAccountModel, string item)
        {
            try
            {
                objNimboleAccountModel.TenantId = TenentId;
                objNimboleAccountModel.AccountName = item;
                objNimboleAccountModel.IsParentAccount = true;
                objNimboleAccountModel.Status = true;
                response = client.PostAsJsonAsync("api/NimboleAccounts/InsertAccount", objNimboleAccountModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var parentaccount = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result;
                    ViewData["ParentAccount"] = parentaccount;
                    return Json(parentaccount.ToDataSourceResult(request));
                }
                else
                {
                    return Json(new[] { objNimboleAccountModel }.ToDataSourceResult(request, ModelState));
                }
            }
            catch
            {
                return Content("");
            }
        }
        #endregion

        #region PUT
        [HttpGet]
        [EncryptedActionParameter]
        public ActionResult Edit(int Id, string Count)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                GetAllDropdownValues();
                response = client.GetAsync("api/NimboleAccounts/GetById?id=" + Id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    string errVal = response.Content.ReadAsStringAsync().Result;
                    if (!errVal.Contains("Failure"))
                    {
                        NimboleAccountModel objNimboleAccountModel = response.Content.ReadAsAsync<NimboleAccountModel>().Result;
                        objNimboleAccountModel.ContactsCount = Convert.ToInt32(Count);
                        return View(objNimboleAccountModel);
                    }
                    else
                    {
                        this.SetAlert(new AlertMessageViewModel()
                        {
                            MessageType = MessageType.Error,
                            MessageHeading = "Failure",
                            MessageString = "Record Not Found"
                        });
                        return RedirectToAction("Index");
                    }
                }
                //return View("Record Not Found");
                return RedirectToAction("Index");
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(NimboleAccountModel objNimboleAccountModel)
        {
            try
            {
                #region New
                if (!string.IsNullOrEmpty(Request["hdnOwnership"]))
                {
                    objNimboleAccountModel.OwnerShip = Request["hdnOwnership"].ToString();
                }

                if (!string.IsNullOrEmpty(Request["hdnAccountType"]))
                {
                    objNimboleAccountModel.AccountType = Request["hdnAccountType"].ToString();
                }

                if (!string.IsNullOrEmpty(Request["hdnIndustry"]))
                {
                    objNimboleAccountModel.Industry = Request["hdnIndustry"].ToString();
                }

                if (!string.IsNullOrEmpty(Request["hdnDistributor"]))
                {
                    objNimboleAccountModel.DistributerName = Request["hdnDistributor"].ToString();
                }

                if (!string.IsNullOrEmpty(Request["hdnParentAccount"]))
                {
                    objNimboleAccountModel.ParentAccount = Request["hdnParentAccount"].ToString();
                }
                #endregion

                objNimboleAccountModel.TenantId = TenentId;

                var response = client.PutAsJsonAsync<NimboleAccountModel>("api/NimboleAccounts/Edit", objNimboleAccountModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<NimboleAccountModel>();
                    ViewData["objResultValue"] = objResultValue;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Account Updated Successfully."
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
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.SetAlert(new AlertMessageViewModel()
                {
                    MessageType = MessageType.Error,
                    MessageHeading = "Failure",
                    MessageString = ex.Message
                });
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region DELETE
        [HttpDelete]
        public async Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, NimboleAccountModel objNimboleAccountModel, string [] selectedId, bool status)
        {
            try
            {
                var strselectedId = string.Join(",", selectedId);
                response = await client.DeleteAsync(strAPIURL + "api/NimboleAccounts/Delete?selectedId=" + strselectedId + "&status=" + status);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new[] { objNimboleAccountModel }.ToDataSourceResult(request, ModelState));
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
        
        #region Account Delete
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> AccountDelete([DataSourceRequest] DataSourceRequest request, string [] selectedId)
        {
            try
            {
                string strselectedId = string.Join(",", selectedId);
                response = await client.DeleteAsync(strAPIURL + "api/NimboleAccounts/DeleteById?selectedId=" + strselectedId);

                if (response.IsSuccessStatusCode)
                {
                    var objAccountModel = response.Content.ReadAsAsync<NimboleAccountModel>().Result;
                    return Json(new[] { objAccountModel }.ToDataSourceResult(request));
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

        #region Associated
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> DeleteRead([DataSourceRequest] DataSourceRequest request, string id, bool status)
        {
            try
            {
                response = await client.DeleteAsync(strAPIURL + "api/NimboleAccounts/DeleteRead?id=" + id + "&status=" + status);
                if (response.IsSuccessStatusCode)
                {
                    var objaccountmodel = response.Content.ReadAsAsync<NimboleAccountModel>().Result;
                    return Json(new[] {objaccountmodel}.ToDataSourceResult(request));
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
        public ActionResult ExcelImport(AccountExcelImport AExcelImport, string btnSubmit, int[] selectedId)
        {

            string corruptedDataError = string.Empty;
            try
            {
                string fileExtension = string.Empty;               
                int id = 0;
                string strAccountCode;
                string strAccountName;
                string strAccountOwner;
                string strAccountClassification;
                string strParentAccount;
                int strNoofEmployees;
                string strOwnership;
                string strIndustry;
                string strDistributer;
                string strAccountType;
                string strPhone;
                string strFax;
                string strEmail;
                int strRating;
                string strSICCode;
                decimal strAnnualRevenue;
                string strRegion;
                string strWebsite;

                string strAddress1 = string.Empty;
                string strCountry = string.Empty;
                string strCity = string.Empty;

                string strAddress2 = string.Empty;
                string strState = string.Empty;
                string strZipCode = string.Empty;

                int countryid = 0;
                int stateid = 0;
                
                Guid TenentId = Guid.Empty;

                TenentId = (new Guid(Session["CurrentTenentId"].ToString()));

                switch (btnSubmit)
                {
                    case "Upload":
                        if (AExcelImport.ImportFile != null)
                        {
                            fileExtension = System.IO.Path.GetExtension(AExcelImport.ImportFile.FileName);
                            if (fileExtension == ".xlsx" || fileExtension == ".csv")
                            {
                                using (MemoryStream excelDocumentMemoryStream = new MemoryStream())
                                {
                                    AExcelImport.ImportFile.InputStream.CopyTo(excelDocumentMemoryStream);

                                    SpreadsheetDocument spreadsheetdocument = SpreadsheetDocument.Open(excelDocumentMemoryStream, true);
                                    WorkbookPart wbPart = spreadsheetdocument.WorkbookPart;
                                    Sheet sheet = wbPart.Workbook.Descendants<Sheet>().FirstOrDefault();
                                    WorksheetPart wsPart = (WorksheetPart)(wbPart.GetPartById(sheet.Id));
                                    Worksheet wsSheet = wsPart.Worksheet;


                                    Dictionary<string, int> labels = new Dictionary<string, int>();

                                    // Account Code	Account Name	Account Owner	Account Classification	Parent Account	No of Employees	Ownership	Industry	
                                    // Distributer Account Type	Account Number	Account Hierarchy	Phone	Fax	Email	Rating	SIC Code	Annual Revenue	Region Website
                                    

                                    labels.Add("Account Name", 0);
                                    labels.Add("Account Code", 0);
                                    labels.Add("Account Owner", 0);
                                    labels.Add("Account Classification", 0);
                                    labels.Add("Parent Account", 0);
                                    labels.Add("No of Employees", 0);
                                    labels.Add("Ownership", 0);
                                    labels.Add("Industry", 0);
                                    labels.Add("Distributer", 0);
                                    labels.Add("Account Type", 0);

                                    labels.Add("Address1", 0);
                                    labels.Add("Address2", 0);
                                    labels.Add("Country", 0);
                                    labels.Add("State", 0);
                                    labels.Add("City", 0);
                                    labels.Add("Zip Code", 0);

                                    labels.Add("Phone", 0);
                                    labels.Add("Fax", 0);
                                    labels.Add("Email", 0);
                                    labels.Add("Rating", 0);
                                    labels.Add("SIC Code", 0);
                                    labels.Add("Annual Revenue", 0);
                                    labels.Add("Region", 0);
                                    labels.Add("Website", 0);


                                    labels = SetSheetLabels(wbPart, wsSheet, 1, labels);

                                    if (labels.Count > 0 && (labels.ContainsKey("Invalid") == true))
                                    {
                                        //excelImport.InvalidHeaders = "Invalid excel headers,for sample please click on 'Download Excel Template'.";
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


                                        strAccountName = values["Account Name"];
                                        strAccountCode = values["Account Code"];
                                        strAccountOwner = values["Account Owner"];
                                        strAccountClassification = values["Account Classification"];
                                        strParentAccount = values["Parent Account"];
                                        if (!string.IsNullOrEmpty(values["No of Employees"]))
                                        {
                                            strNoofEmployees = Convert.ToInt32(values["No of Employees"]);
                                        }
                                        else
                                        {
                                            strNoofEmployees = 0;
                                        }
                                        strOwnership = values["Ownership"];
                                        strIndustry = values["Industry"];
                                        strDistributer = values["Distributer"];
                                        strAccountType = values["Account Type"];

                                        strAddress1 = values["Address1"];
                                        strAddress2 = values["Address2"];
                                        strCountry = values["Country"];
                                        strState = values["State"];
                                        strCity = values["City"];
                                        strZipCode = values["Zip Code"];

                                        strPhone = values["Phone"];
                                        strFax = values["Fax"];
                                        strEmail = values["Email"];

                                        if (!string.IsNullOrEmpty(values["Rating"]))
                                        {
                                            strRating = Convert.ToInt32(values["Rating"]);
                                        }
                                        else
                                        {
                                            strRating = 0;
                                        }
                                        strSICCode = values["SIC Code"];
                                        if (!string.IsNullOrEmpty(values["Annual Revenue"]))
                                        {
                                            strAnnualRevenue = Convert.ToDecimal(values["Annual Revenue"]);
                                        }
                                        else
                                        {
                                            strAnnualRevenue = 0;
                                        }
                                        strRegion = values["Region"];
                                        strWebsite = values["Website"];
                                        

                                        if (!string.IsNullOrEmpty(strAccountName))
                                            strAccountName = strAccountName.Trim();
                                        
                                        if (((!string.IsNullOrEmpty(strAccountName)) && (!string.IsNullOrWhiteSpace(strAccountName)))) 
                                        {
                                            excelImport.AExcelImport.Add(new AExcelImport()
                                            {
                                                Id = id,                                                
                                                AccountName = strAccountName,
                                                AccountCode = strAccountCode,
                                                AccountOwner = strAccountOwner,
                                                AccountClassification = strAccountClassification,
                                                ParentAccount = strParentAccount,
                                                NoofEmployees = strNoofEmployees,
                                                Ownership = strOwnership,
                                                Industry = strIndustry,
                                                Distributer=strDistributer,
                                                AccountType = strAccountType,                                                

                                                Address1 = strAddress1,
                                                Address2 = strAddress2,

                                                Country = strCountry,
                                                State = strState,
                                                City = strCity,
                                                ZipCode = strZipCode,

                                                Phone = strPhone,
                                                Fax = strFax,
                                                Email = strEmail,
                                                Rating = strRating,
                                                SICCode = strSICCode,
                                                AnnualRevenue = strAnnualRevenue,
                                                Region=strRegion,
                                                Website = strWebsite

                                            });
                                        }
                                        id++;                                       
                                        rowIndex++;
                                    }

                                    Session["ExcelImport"] = excelImport.AExcelImport;
                                }
                            }
                        }

                        break;

                    case "ExcelTemplate":

                        break;

                    case "Import":
                       
                        if ((Session["ExcelImport"] != null))
                        {
                            List<AExcelImport> ExcelImportAccounts = (List<AExcelImport>)Session["ExcelImport"];

                            NimboleAccountModel objNimboleAccountModel = new NimboleAccountModel();

                            var selectedItems = (from ci in ExcelImportAccounts where selectedId.Contains(ci.Id) select ci).ToList();//where selectedContactId.Contains(ci.Id) select ci).ToList();
                            if (selectedItems.Count > 0)
                            {
                                foreach (var items in selectedItems)
                                {

                                    strAccountName = items.AccountName;
                                    strAccountCode = items.AccountCode;
                                    strAccountOwner = items.AccountOwner;
                                    strAccountClassification = items.AccountClassification;
                                    strParentAccount = items.ParentAccount;
                                    strNoofEmployees = items.NoofEmployees;
                                    strOwnership = items.Ownership;
                                    strIndustry = items.Industry;
                                    strDistributer = items.Distributer;
                                    strAccountType = items.AccountType;

                                    strAddress1 = items.Address1;
                                    strAddress2 = items.Address2;
                                    strCountry = items.Country;
                                    strState = items.State;
                                    strCity = items.City;
                                    strZipCode = items.ZipCode;

                                    strPhone = items.Phone;
                                    strFax = items.Fax;
                                    strEmail = items.Email;
                                    strRating = items.Rating;
                                    strSICCode = items.SICCode;
                                    strAnnualRevenue = items.AnnualRevenue;
                                    strRegion = items.Region;
                                    strWebsite = items.Website;


                                    response = client.GetAsync("api/NimboleAccounts/GetByAccName?AccName=" + strAccountName + "&Tid=" + TenentId).Result;
                                    if (response.IsSuccessStatusCode)
                                    {
                                        objNimboleAccountModel = response.Content.ReadAsAsync<NimboleAccountModel>().Result;
                                    }
                                    if (response.IsSuccessStatusCode == false)
                                    {

                                        NimboleAccountModel account = new NimboleAccountModel();

                                        account.TenantId = TenentId;
                                        account.AccountName = strAccountName;
                                        account.AccountCode = strAccountCode;
                                        account.AccountOwner = strAccountOwner;

                                        if (!string.IsNullOrEmpty(strAccountClassification))
                                        {
                                            if (strAccountClassification == "Customer")
                                            {
                                                account.AccountClassification = "1";
                                            }
                                            if (strAccountClassification == "Prospect")
                                            {
                                                account.AccountClassification = "2";
                                            }
                                        }

                                       // account.ParentAccount = strParentAccount;

                                        if (!string.IsNullOrEmpty(strParentAccount))
                                        {

                                            response = client.GetAsync("api/NimboleAccounts/GetByAccName?AccName=" + strParentAccount + "&Tid=" + TenentId).Result;
                                            if (response.IsSuccessStatusCode)
                                            {
                                                var parentAcc = response.Content.ReadAsAsync<string>().Result;
                                                account.ParentAccount = parentAcc;
                                            }
                                            if (response.IsSuccessStatusCode==false)
                                            {
                                                NimboleAccountModel parentAccount = new NimboleAccountModel();

                                                parentAccount.AccountName = strParentAccount;
                                                parentAccount.TenantId = TenentId;
                                                    parentAccount.Status = true;
                                                    response = client.PostAsJsonAsync("api/NimboleAccounts/Insert", parentAccount).Result;
                                                    if (response.IsSuccessStatusCode)
                                                    {
                                                        var parentAcc = response.Content.ReadAsAsync<string>().Result;
                                                        account.ParentAccount = parentAcc;
                                                    }
                                            }

                                        }

                                       // account.Distributer = strDistributer;

                                        if (!string.IsNullOrEmpty(strDistributer))
                                        {

                                            response = client.GetAsync("api/NimboleAccounts/GetByAccName?AccName=" + strDistributer + "&Tid=" + TenentId).Result;
                                            if (response.IsSuccessStatusCode)
                                            {
                                                var distributertAcc = response.Content.ReadAsAsync<string>().Result;
                                                account.DistributerName = distributertAcc;
                                            }
                                            if (response.IsSuccessStatusCode == false)
                                            {
                                                NimboleAccountModel distributerAccount = new NimboleAccountModel();

                                                distributerAccount.TenantId = TenentId;
                                                distributerAccount.AccountName = strDistributer;
                                                distributerAccount.Status = true;
                                                response = client.PostAsJsonAsync("api/NimboleAccounts/Insert", distributerAccount).Result;
                                                if (response.IsSuccessStatusCode)
                                                {
                                                    var distributertAcc = response.Content.ReadAsAsync<string>().Result;
                                                    account.DistributerName = distributertAcc;
                                                }
                                            }

                                        }

                                        
                                        account.Employees = Convert.ToInt32(strNoofEmployees);

                                      //  account.OwnerShip = strOwnership;

                                        if (!string.IsNullOrEmpty(strOwnership))
                                        {

                                            response = client.GetAsync("api/Ownership/GetByOwner?OwnerName=" + strOwnership + "&Tid=" + TenentId).Result;
                                            if (response.IsSuccessStatusCode)
                                            {
                                                var ownerName = response.Content.ReadAsAsync<string>().Result;
                                                account.OwnerShip = ownerName;
                                            }
                                            //if (response.IsSuccessStatusCode == false)
                                            //{
                                            //    OwnershipModel objOwnershipModel = new OwnershipModel();

                                            //    objOwnershipModel.Description = strOwnership;
                                            //    objOwnershipModel.Status = true;
                                            //    response = client.PostAsJsonAsync("api/Ownership/InsertOwner", objOwnershipModel).Result;
                                            //    if (response.IsSuccessStatusCode)
                                            //    {
                                            //        var ownerName = response.Content.ReadAsAsync<string>().Result;
                                            //        account.OwnerShip = ownerName;
                                            //    }
                                            //}

                                        }

                                      //  account.Industry = strIndustry;

                                        if (!string.IsNullOrEmpty(strIndustry))
                                        {

                                            response = client.GetAsync("api/Industry/GetByIndustry?IndustryName=" + strIndustry + "&Tid=" + TenentId).Result;
                                            if (response.IsSuccessStatusCode)
                                            {
                                                var industryName = response.Content.ReadAsAsync<string>().Result;
                                                account.Industry = industryName;
                                            }
                                            //if (response.IsSuccessStatusCode == false)
                                            //{
                                            //    IndustryModel objIndustryModel = new IndustryModel();

                                            //    objIndustryModel.Description = strIndustry;
                                            //    objIndustryModel.Status = true;
                                            //    response = client.PostAsJsonAsync("api/Industry/InsertIndustry", objIndustryModel).Result;
                                            //    if (response.IsSuccessStatusCode)
                                            //    {
                                            //        var industryName = response.Content.ReadAsAsync<string>().Result;
                                            //        account.Industry = industryName;
                                            //    }
                                            //}

                                        }

                                       // account.AccountType = strAccountType;

                                        if (!string.IsNullOrEmpty(strAccountType))
                                        {

                                            response = client.GetAsync("api/NimboleAccounts/GetByAccType?AccType=" + strAccountType + "&Tid=" + TenentId).Result;
                                            if (response.IsSuccessStatusCode)
                                            {
                                                var accType = response.Content.ReadAsAsync<string>().Result;
                                                account.AccountType = accType;
                                            }
                                            //if (response.IsSuccessStatusCode == false)
                                            //{
                                            //    IndustryModel objIndustryModel = new IndustryModel();

                                            //    objIndustryModel.Description = strAccountType;
                                            //    objIndustryModel.Status = true;
                                            //    response = client.PostAsJsonAsync("api/Industry/InsertIndustry", objIndustryModel).Result;
                                            //    if (response.IsSuccessStatusCode)
                                            //    {
                                            //        var industryName = response.Content.ReadAsAsync<string>().Result;
                                            //        account.Industry = industryName;
                                            //    }
                                            //}

                                        }

                                        account.objAddressModel.TenantId = TenentId;
                                        account.objAddressModel.HouseNo = strAddress1;
                                        account.objAddressModel.StreetName = strAddress2;

                                        if (!string.IsNullOrEmpty(strCountry))
                                        {

                                            response = client.GetAsync("api/AddressAutoComplete/GetIdByCountry?Country=" + strCountry +"&Tid=" + TenentId).Result;
                                            if (response.IsSuccessStatusCode)
                                            {
                                                account.objAddressModel.CountryId = response.Content.ReadAsAsync<long>().Result;
                                                countryid = Convert.ToInt32(account.objAddressModel.CountryId);

                                            }
                                        }

                                        if (!string.IsNullOrEmpty(strState))
                                        {

                                            response = client.GetAsync("api/AddressAutoComplete/GetIdByState?State=" + strState + "&Countryid=" + countryid +"&Tid="+TenentId).Result;
                                            if (response.IsSuccessStatusCode)
                                            {
                                                account.objAddressModel.StateId = response.Content.ReadAsAsync<long>().Result;
                                                stateid = Convert.ToInt32(account.objAddressModel.StateId);
                                            }
                                        }
                                        
                                        if (!string.IsNullOrEmpty(strCity))
                                        {
                                            response = client.GetAsync("api/AddressAutoComplete/GetIdByCity?City=" + strCity + "&Stateid=" + stateid+"&Tid="+TenentId).Result;
                                            if (response.IsSuccessStatusCode)
                                            {
                                                account.objAddressModel.CityId = response.Content.ReadAsAsync<long>().Result;
                                            }
                                        }

                                        account.Phone = strPhone;
                                        account.Fax = strFax;
                                        account.Email = strEmail;
                                        account.Rating = strRating.ToString();
                                        account.SICCode = strSICCode;


                                        account.AnnualRevenue = Convert.ToInt64(strAnnualRevenue);
                                        account.Region = strRegion;
                                        account.Website = strWebsite;
                                        account.objAddressModel.ZipCode = strZipCode;
                                        account.TenantId = TenentId;
                                        account.CreatedDate = DateTime.UtcNow;
                                        account.ModifiedDate = DateTime.UtcNow;
                                        account.Status = true;

                                        response = client.PostAsJsonAsync("api/NimboleAccounts/Insert", account).Result;
                                        if (response.IsSuccessStatusCode)
                                        {
                                            var accounts = response.Content.ReadAsAsync<string>().Result;
                                            //  return RedirectToAction("Index");

                                            //var accounts = response.Content.ReadAsAsync<NimboleAccountModel>().Result;

                                            //importContactId = Convert.ToInt32(accounts.Id);
                                        }
                                    }
                                    else
                                    {

                                    }

                                }
                                return Content("Account(s) Successfully imported!");
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
                    excelImport.AInvalidHeaders = corruptedDataError;

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
            repeat:
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
                    //colLabelIndex++;
                    //if (colLabelIndex < colLabels.Length)
                    //{
                    //    goto repeat;
                    //}
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

        #region Excelexport

        [HttpGet]
        public ActionResult Excel_Export()
        {

            List<NimboleAccountModel> AccountReports = new List<NimboleAccountModel>();

            List<NimboleAccountModel> objNimboleAccountModel = new List<NimboleAccountModel>();

            response = client.GetAsync("api/NimboleAccounts/GetAllForExport?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
                objNimboleAccountModel = response.Content.ReadAsAsync<IEnumerable<NimboleAccountModel>>().Result.ToList();

            foreach (var item in objNimboleAccountModel)
            {

                AccountReports.Add(new NimboleAccountModel()
                {

                    AccountName = item.AccountName,
                    AccountCode = item.AccountCode,
                    AccountOwner = item.AccountOwner,
                    AccountClassification = item.AccountClassification,
                    ParentAccount = item.ParentAccount,
                    ParentAccountName = item.ParentAccountName,
                    Employees = item.Employees,
                    HouseNo = !string.IsNullOrEmpty(item.HouseNo) ? item.HouseNo.Replace(',', ' ') : item.HouseNo,
                    StreetName = !string.IsNullOrEmpty(item.StreetName) ? item.StreetName.Replace(',', ' ') : item.StreetName,
                    CountryName = item.CountryName,
                    StateName = item.StateName,
                    CityName = item.CityName,
                    ZipCode = item.ZipCode,
                    OwnerShip=item.OwnerShip,
                    OwnerShipName = item.OwnerShipName,
                    Industry=item.Industry,
                    IndustryName = item.IndustryName,
                    AccountType=item.AccountType,
                    AccountTypeDescription = item.AccountTypeDescription,
                    IsParentAccount = Convert.ToBoolean(item.IsParentAccount),
                    DistributerName = item.DistributerName,
                    // Subsidiary = item.Subsidiary,
                    Region = item.Region,
                    Phone = item.Phone,
                    Fax = item.Fax,
                    Email = item.Email,
                    Rating = item.Rating,
                    SICCode = item.SICCode,
                    AnnualRevenue = item.AnnualRevenue,
                    Website = item.Website
                });
            }


            Response.AddHeader("content-disposition", "attachment; filename=AccountExport.csv");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";

            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            htw.WriteLine("Account Name" + "," + "Account Code" + "," + "Account Owner" + "," + "Account Classification" + "," + "Parent Account" + "," + "No of Employees" + "," + "Ownership" + "," + "Industry" + "," + "Account Type" + "," + "Is Parent Account" + "," + "Distributor Name" + "," + "Region" + "," + "Address1" + "," + "Address2" + "," + "Country" + "," + "State" + "," + "City" + "," + "Zip Code" + "," + "Phone" + "," + "Fax" + "," + "Email" + "," + "Rating" + "," + "SIC Code" + "," + "Annual Revenue" + "," + "Website");


            if (AccountReports != null)
            {
                foreach (var data in AccountReports)
                {

                    if (string.IsNullOrEmpty(data.AccountCode))
                    {
                        data.AccountCode = "Null";
                    }

                    if (string.IsNullOrEmpty(data.AccountName))
                    {
                        data.AccountName = "Null";
                    }

                    if (string.IsNullOrEmpty(data.AccountOwner))
                    {
                        data.AccountOwner = "Null";
                    }

                    if (string.IsNullOrEmpty(data.AccountClassification))
                    {
                        data.AccountClassification = "Null";
                    }

                    if (string.IsNullOrEmpty(data.ParentAccountName))
                    {
                        data.ParentAccountName = "Null";
                    }

                    if (data.Employees == 0)
                    {
                        data.Employees = 0;
                    }

                    if (string.IsNullOrEmpty(data.OwnerShipName))
                    {
                        data.OwnerShipName = "Null";
                    }

                    if (string.IsNullOrEmpty(data.IndustryName))
                    {
                        data.IndustryName = "Null";
                    }

                    if (string.IsNullOrEmpty(data.AccountTypeDescription))
                    {
                        data.AccountTypeDescription = "Null";
                    }

                    if (data.IsParentAccount)
                    {
                        data.IsParentAccount = true;
                    }

                    if (string.IsNullOrEmpty(data.DistributerName))
                    {
                        data.DistributerName = "Null";
                    }

                    //if (string.IsNullOrEmpty(data.Subsidiary))
                    //{
                    //    data.Subsidiary = "Null";
                    //}
                    if (string.IsNullOrEmpty(data.Region))
                    {
                        data.Region = "Null";
                    }
                    if (string.IsNullOrEmpty(data.Phone))
                    {
                        data.Phone = "Null";
                    }
                    if (string.IsNullOrEmpty(data.Fax))
                    {
                        data.Fax = "Null";
                    }
                    if (string.IsNullOrEmpty(data.Email))
                    {
                        data.Email = "Null";
                    }
                    if (data.Rating == null)
                    {
                        data.Rating = "0";
                    }
                    if (string.IsNullOrEmpty(data.SICCode))
                    {
                        data.SICCode = "Null";
                    }

                    if (data.AnnualRevenue == 0)
                    {
                        data.AnnualRevenue = 0;
                    }
                    if (string.IsNullOrEmpty(data.Website))
                    {
                        data.Website = "Null";
                    }
                    if (string.IsNullOrEmpty(data.HouseNo))
                    {
                        data.HouseNo = "Null";
                    }
                    if (string.IsNullOrEmpty(data.StreetName))
                    {
                        data.StreetName = "Null";
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

                    if (!string.IsNullOrEmpty(data.AccountName))
                    {
                        htw.WriteLine(data.AccountName.ToString() + "," + data.AccountCode.ToString() + "," + data.AccountOwner.ToString() +
                                        "," + data.AccountClassification.ToString() +
                                        "," + data.ParentAccountName.ToString() + "," + data.Employees.ToString() + "," + data.OwnerShipName.ToString() +
                                        "," + data.IndustryName.ToString() + "," + data.AccountTypeDescription.ToString() + "," + data.IsParentAccount.ToString() +
                                        "," + data.DistributerName.ToString() +
                                        "," + data.Region.ToString() + "," + data.HouseNo.ToString() + "," + data.StreetName.ToString() + "," + data.CountryName.ToString() + "," + data.StateName.ToString() + "," + data.CityName.ToString() + "," + data.ZipCode.ToString() + "," + data.Phone.ToString() + "," + data.Fax.ToString() +
                                        "," + data.Email.ToString() + "," + data.Rating.ToString() + "," + data.SICCode.ToString() +
                                        "," + data.AnnualRevenue.ToString() + "," + data.Website.ToString());
                    }

                }
            }

            Response.Output.Write(sw.ToString());

            Response.Flush();
            Response.End();

            return Content("");

        }
        #endregion

        private void GetAllDropdownValues()
        {
            /*//Accounts
            response = client.GetAsync("api/NimboleAccounts/SelectAllAccount?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objAccountModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                ViewData["Accounts"] = objAccountModel;
            }

            //Parent Account
            response = client.GetAsync("api/NimboleAccounts/SelectAllParentAccountsForCombobox?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objAccountModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                ViewData["ParentAccount"] = objAccountModel;
            }


            //Account Type
            response = client.GetAsync("api/NimboleAccounts/SelectAllAccountTypesForCombobox?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objAccountModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                ViewData["AccountType"] = objAccountModel;
            }


            //Industry
            response = client.GetAsync("api/NimboleAccounts/SelectAllIndustriesForCombobox?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objAccountModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                ViewData["Industry"] = objAccountModel;
            }

            //Distributor
            response = client.GetAsync("api/NimboleAccounts/SelectAllDistributorNamesForCombobox?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objAccountModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList().OrderBy(k=>k.Name);
                ViewData["Distributor"] = objAccountModel;
            }

            //Ownership
            response = client.GetAsync("api/NimboleAccounts/GetAllOwnerships?Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objAccountModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                ViewData["Ownership"] = objAccountModel;
            }*/
            //Countries
            response = client.GetAsync("api/AddressAutoComplete/Countries").Result;
            if (response.IsSuccessStatusCode)
            {
                var Countries = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.CountryModel>>().Result.ToList().OrderBy(x => x.CountryName);
                ViewData["Countries"] = new SelectList(Countries, "CountryId", "CountryName", new object { });
            }

        }
    }
}