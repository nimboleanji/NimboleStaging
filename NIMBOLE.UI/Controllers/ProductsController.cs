using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NIMBOLE.Models.Models;
using System.Web.UI;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using NIMBOLE.Common;
using NIMBOLE.UI.Filters;
using Kendo.Mvc.UI;
using NIMBOLE.UI.Models;
using System.Threading.Tasks;
using System.Globalization;

namespace NIMBOLE.UI.Controllers
{
    public class ProductsController : BaseController
    {
        ProductExcelImport excelImport = new ProductExcelImport();
        Guid TenentId = new Guid();
        public ProductsController()
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

        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/Products/GetAll?Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var products = response.Content.ReadAsAsync<IEnumerable<ProductModel>>().Result;
                    ViewData["ProductDetails"] = products;
                    return View(products);
                }
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        #region GET
        public JsonResult Product_Read([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Products/GetAll?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var products = response.Content.ReadAsAsync<IEnumerable<ProductModel>>().Result;
                ViewData["ProductDetails"] = products;
                return Json(products.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            ViewData["ProductDetails"] = null;
            return Json(null);
        }
        [HttpGet]
        public async Task<JsonResult> GetProductNamesByType([DataSourceRequest] DataSourceRequest request, string type)
        {
            response = await client.GetAsync("api/Products/SelectAllProductsByType?Type=" + type + "&Tid=" + TenentId);
            if (response.IsSuccessStatusCode)
            {
                var lstProductNamesModel = response.Content.ReadAsAsync<IEnumerable<ProductNamesModel>>().Result.ToList();

                //for (int i = 0; i < lstProductNamesModel.Count; i++)
                //{
                //    lstProductNamesModel[0].ProductNamesId = 0;
                //    lstProductNamesModel[0].ProductName = "Select";
                //    break;
                //}
                return Json(lstProductNamesModel, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        private void GetAllDropdownValues()
        {
            //ProductTypes
            response = client.GetAsync("api/Products/SelectAllProductTypes?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objProductNamesModel = response.Content.ReadAsAsync<IEnumerable<ProductTypeModel>>().Result.ToList();
                ViewData["ProductType"] = objProductNamesModel;
            }
        }
        public JsonResult GetProductNames([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Products/SelectAllProducts?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var lstProductNamesModel = response.Content.ReadAsAsync<IEnumerable<ProductNamesModel>>().Result;
                var result = Json(lstProductNamesModel, JsonRequestBehavior.AllowGet);
                return result;
            }
            return Json(null);
        }
        [HttpGet]
        public async Task<JsonResult> GetAllProductTypes()
        {
            response = await client.GetAsync("api/Products/SelectAllProductTypes?Tid=" + TenentId);
            if (response.IsSuccessStatusCode)
            {
                var _objProductModel = response.Content.ReadAsAsync<IEnumerable<ProductTypeModel>>().Result;
                return Json(_objProductModel, JsonRequestBehavior.AllowGet);

            }
            return null;
        }

        public ActionResult GetCompProdPrice([DataSourceRequest] DataSourceRequest request, long ProdId, long CompId)
        {
            try
            {
                long leadId = Convert.ToInt64(Session["CurrentLeadId"]);
                response = client.GetAsync("api/Products/GetCompetitorPrice?ProdId=" + CompId + "&LeadId=" + leadId + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var price = response.Content.ReadAsAsync<double>().Result;

                    return Json(price, JsonRequestBehavior.AllowGet);
                }
                return Json(null);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult GetCompProdCode([DataSourceRequest] DataSourceRequest request, long ProdId)
        {
            try
            {
                response = client.GetAsync("api/Products/GetProductCode?ProdId=" + ProdId + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var productCode = response.Content.ReadAsAsync<string>().Result;

                    return Json(productCode, JsonRequestBehavior.AllowGet);
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public ActionResult GetAllProductsCodePrice(long ProdId)
        {
            response = client.GetAsync("api/Products/SelectAllProductsCodePrice?ProdId=" + ProdId + "&Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var _objProductModel = response.Content.ReadAsAsync<IEnumerable<ProductModel>>().Result;
                return Json(_objProductModel, JsonRequestBehavior.AllowGet);

            }
            return null;
        }
        #endregion

        #region Create
        // GET: Products/Create
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

        // POST: Products/Create
        [HttpPost]
        public ActionResult Create(ProductModel objProductModel)
        {
            try
            {
                #region New

                if (!string.IsNullOrEmpty(Request["hdnProduct"]) && string.IsNullOrEmpty(objProductModel.ProductTypeId))
                {
                    objProductModel.ProductType = Request["hdnProduct"].ToString();
                }
                #endregion
                objProductModel.TenantId = TenentId;
                objProductModel.Status = true;
                response = client.PostAsJsonAsync("api/Products/Insert", objProductModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var products = response.Content.ReadAsAsync<ProductModel>().Result;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Product Created Successfully."
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

        [HttpPost]
        public ActionResult TempCreate([DataSourceRequest] DataSourceRequest request, ProductModel objProductModel)
        {
            try
            {
                //if (objProductModel.ProductType != null && objProductModel.ProductName != null)
                if (objProductModel.ProductName != null)
                {
                    objProductModel.ProductTypeId = objProductModel.ProductTypeId == null ? "1" : objProductModel.ProductTypeId;
                    objProductModel.ProductCode = objProductModel.ProductCode.Trim();
                    objProductModel.TenantId = TenentId;
                    objProductModel.ExpiryDate = DateTime.Now.AddYears(1);
                    response = client.PostAsJsonAsync("api/Products/UpdatePrice", objProductModel).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var products = response.Content.ReadAsAsync<ProductModel>().Result;
                    }
                    return Json(new[] { objProductModel }.ToDataSourceResult(request, ModelState));
                }
                return Json(new[] { objProductModel }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult CreatePoductType([DataSourceRequest] DataSourceRequest request, ProductTypeModel objProductTypeModel, string item)
        {
            try
            {
                objProductTypeModel.ProductType = item;
                objProductTypeModel.TenantId = TenentId;
                objProductTypeModel.CreatedDate = DateTime.Now.Date;
                objProductTypeModel.ModifiedDate = DateTime.Now.Date;
                objProductTypeModel.Status = true;

                response = client.PostAsJsonAsync("api/Products/InsertProductType", objProductTypeModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var productType = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.ProductTypeModel>>().Result;
                    return Json(productType.ToDataSourceResult(request));
                }
                else
                {
                    return Json(new[] { objProductTypeModel }.ToDataSourceResult(request, ModelState));
                }
            }
            catch
            {
                return Content("");
            }
        }

        [HttpPost]
        public ActionResult CreateLeadProductWithType([DataSourceRequest] DataSourceRequest request, ProductModel objProductModel, string item, string type, decimal? price)
        {
            try
            {
                objProductModel.ProductCode = item.Substring(0, 2);
                objProductModel.Price = 0;
                objProductModel.ProductName = item;
                objProductModel.ExpiryDate = DateTime.Now.AddYears(1);
                //objProductModel.TenantId = new Guid(Session["CurrentTenentId"].ToString());
                objProductModel.TenantId = TenentId;
                objProductModel.Status = true;
                if (!string.IsNullOrEmpty(type.ToString()))
                {
                    objProductModel.ProductType = type.ToString();
                }
                else
                    objProductModel.ProductType = "1";
                if (price.HasValue)
                    objProductModel.Price = Convert.ToDecimal(price);

                response = client.PostAsJsonAsync("api/Products/LeadInsert", objProductModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var product = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.ProductModel>>().Result;
                    return Json(product.ToDataSourceResult(request));
                }
                else
                {
                    return Json(new[] { objProductModel }.ToDataSourceResult(request, ModelState));
                }
            }
            catch
            {
                return Content("");
            }
        }
        [HttpPost]
        public ActionResult CreateLeadProduct([DataSourceRequest] DataSourceRequest request, ProductModel objProductModel, string item, string type)
        {
            try
            {
                if (!string.IsNullOrEmpty(type))
                {
                    objProductModel.ProductType = type;
                }
                objProductModel.ProductCode = item.Substring(0, 2);
                objProductModel.Price = 0;
                objProductModel.ProductName = item;
                //objProductModel.TenantId = new Guid(Session["CurrentTenentId"].ToString());
                objProductModel.TenantId = TenentId;
                objProductModel.Status = true;
                objProductModel.ExpiryDate = DateTime.Now.AddYears(1);

                response = client.PostAsJsonAsync("api/Products/LeadInsert", objProductModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var product = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.ProductModel>>().Result;
                    return Json(product.ToDataSourceResult(request));

                }
                else
                {
                    return Json(new[] { objProductModel }.ToDataSourceResult(request, ModelState));
                }

            }
            catch
            {
                return Content("");
            }
        }
        #endregion

        #region Edit
        [EncryptedActionParameter]
        // GET: Products/Edit/5
        public ActionResult Edit(int Id)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                GetAllDropdownValues();
                response = client.GetAsync("api/Products/GetById?id=" + Id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    ProductModel ObjProductModel = response.Content.ReadAsAsync<ProductModel>().Result;
                    return View(ObjProductModel);

                }
                return View("Record Not Found");
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        // POST: Products/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(ProductModel objProductModel)
        {
            try
            {
                #region New
                if (!string.IsNullOrEmpty(Request["hdnProduct"]) && string.IsNullOrEmpty(objProductModel.ProductTypeId))
                {
                    objProductModel.ProductType = Request["hdnProduct"].ToString();
                }
                #endregion

                objProductModel.TenantId = TenentId;
                if (objProductModel.ProductCode != null)
                {
                    objProductModel.ProductCode = objProductModel.ProductCode.Trim();
                }
                var response = await client.PutAsJsonAsync<ProductModel>("api/Products/Edit", objProductModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<ProductModel>();
                    ViewData["objResultValue"] = objResultValue;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Product Updated Successfully."
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
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> EditById(long Id, decimal iPrice)
        {
            try
            {
                ProductModel objProductModel = new ProductModel();
                objProductModel.Id = Id;
                objProductModel.Price = iPrice;
                var response = await client.PutAsJsonAsync("api/Products/EditById", objProductModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<ProductModel>();
                    ViewData["objResultValue"] = objResultValue;
                    return RedirectToAction("Index");
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
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, ProductModel objProductModel, int id, bool status)
        {
            try
            {
                response = await client.DeleteAsync(strAPIURL + "api/Products/Delete?Id=" + id + "&status=" + status);
                if (response.IsSuccessStatusCode)
                {
                    var objProductModels = response.Content.ReadAsAsync<ProductModel>().Result;
                    return Json(new[] { objProductModel }.ToDataSourceResult(request, ModelState));
                }
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("index");
        }
        #endregion

        #region ExcelImportFromAzure

        public static DateTime ConvertToDateTime(double excelDate)
        {
            if (excelDate < 1)
            {
                throw new ArgumentException("Excel dates cannot be smaller than 0.");
            }
            DateTime dateOfReference = new DateTime(1900, 1, 1);
            if (excelDate > 60d)
            {
                excelDate = excelDate - 2;
            }
            else
            {
                excelDate = excelDate - 1;
            }
            return dateOfReference.AddDays(excelDate);
        }

        public ActionResult PExcelImport()
        {
            ProductExcelImport excelImport = new ProductExcelImport();
            return View(excelImport);
        }

        [HttpPost]
        public ActionResult PExcelImport(ProductExcelImport ExcelImport, string btnSubmit, int[] selectedId)
        {
            //  ContactExcelImport excelImport = new ContactExcelImport();

            string corruptedDataError = string.Empty;
            try
            {
                string fileExtension = string.Empty;
                int importProductId = 0;
                int id = 0;

                string strProductName = string.Empty;
                string strProductCode = string.Empty;
                string strProductType = string.Empty;
                decimal dPrice = 0;

                DateTime dtExpireDate = DateTime.Now.Date;

                string strComments = string.Empty;
                string strManufactureName = string.Empty;

                Guid TenentId = Guid.Empty;

                TenentId = (new Guid(Session["CurrentTenentId"].ToString()));
                //TenentId = TenentId == null ? new Guid("7EA74E34-4F73-4609-A4F6-E0FA46E14D9E") : TenentId;

                switch (btnSubmit)
                {
                    case "Upload":
                        //if (ModelState.IsValid)
                        //{
                        if (ExcelImport.ImportFile != null)
                        {
                            fileExtension = System.IO.Path.GetExtension(ExcelImport.ImportFile.FileName);
                            if (fileExtension == ".xlsx")
                            {
                                using (MemoryStream excelDocumentMemoryStream = new MemoryStream())
                                {
                                    ExcelImport.ImportFile.InputStream.CopyTo(excelDocumentMemoryStream);

                                    SpreadsheetDocument spreadsheetdocument = SpreadsheetDocument.Open(excelDocumentMemoryStream, true);
                                    WorkbookPart wbPart = spreadsheetdocument.WorkbookPart;
                                    // Sheet sheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == "Sheet1").FirstOrDefault();
                                    Sheet sheet = wbPart.Workbook.Descendants<Sheet>().FirstOrDefault();
                                    WorksheetPart wsPart = (WorksheetPart)(wbPart.GetPartById(sheet.Id));
                                    Worksheet wsSheet = wsPart.Worksheet;


                                    Dictionary<string, int> labels = new Dictionary<string, int>();


                                    labels.Add("Product Name", 0);
                                    labels.Add("Product Code", 0);
                                    labels.Add("Price", 0);
                                    labels.Add("Product Type", 0);
                                    labels.Add("Manufacturer Name", 0);
                                    labels.Add("Expiry Date", 0);
                                    labels.Add("Comments", 0);


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

                                        strProductName = values["Product Name"];
                                        strProductCode = values["Product Code"];
                                        dPrice = Convert.ToDecimal(values["Price"]);
                                        if (!string.IsNullOrEmpty(values["Expiry Date"]))
                                        {

                                            double d = double.Parse(values["Expiry Date"]);
                                            dtExpireDate = ConvertToDateTime(d);
                                            //dtExpireDate = DateTime.FromOADate(values["Expiry Date"]).ToString("MM-dd-yyyy"); 

                                            // dtExpireDate = Convert.ToDateTime(values["Expiry Date"]);
                                            // dtExpireDate = DateTime.Parse(values["Expiry Date"], new CultureInfo("en-US", true));
                                            // dtExpireDate = DateTime.ParseExact(values["Expiry Date"], "dd/MM/yyyy", null);
                                            //dtExpireDate = DateTime.ParseExact(values["Expiry Date"].ToString(), "dd/MM/yyyy",
                                            //  CultureInfo.InvariantCulture);
                                            //dtExpireDate=DateTime.ParseExact(values["Expiry Date"], "MM/dd/yyyy", null);
                                            // dtExpireDate = Convert.ToDateTime(values["Expiry Date"].ToString("yyyy-MM-dd"));

                                        }
                                        else
                                        {
                                            dtExpireDate = DateTime.Now.AddYears(1);
                                        }

                                        strProductType = values["Product Type"];
                                        strManufactureName = values["Manufacturer Name"];
                                        strComments = values["Comments"];

                                        if (!string.IsNullOrEmpty(strProductCode))
                                            strProductCode = strProductCode.Trim();
                                        //To check if email is valid or not
                                        //System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                        //System.Text.RegularExpressions.Match match = regex.Match(strEmail);
                                        //if (match.Success)
                                        //{
                                        if (((!string.IsNullOrEmpty(strProductName)) && (!string.IsNullOrWhiteSpace(strProductName))) && ((!string.IsNullOrEmpty(strProductType)) && (!string.IsNullOrWhiteSpace(strProductType)))) //&& ((!string.IsNullOrEmpty(strPhoneNumber)) && (!string.IsNullOrWhiteSpace(strPhoneNumber))) && ((!string.IsNullOrEmpty(strAddressLine1)) && (!string.IsNullOrWhiteSpace(strAddressLine1))) && ((!string.IsNullOrEmpty(strAddressLine2)) && (!string.IsNullOrWhiteSpace(strAddressLine2))) && ((!string.IsNullOrEmpty(strTitle)) && (!string.IsNullOrWhiteSpace(strTitle))) &&
                                        //((!string.IsNullOrEmpty(strOrganization)) && (!string.IsNullOrWhiteSpace(strOrganization))) && ((!string.IsNullOrEmpty(strCity)) && (!string.IsNullOrWhiteSpace(strCity))) && ((!string.IsNullOrEmpty(strCountry)) && (!string.IsNullOrWhiteSpace(strCountry))) && ((!string.IsNullOrEmpty(strState)) && (!string.IsNullOrWhiteSpace(strState))) && ((!string.IsNullOrEmpty(strZip)) && (!string.IsNullOrWhiteSpace(strZip))) && ((!string.IsNullOrEmpty(strNotes)) && (!string.IsNullOrWhiteSpace(strNotes))))
                                        {
                                            excelImport.PExcelImport.Add(new PExcelImport()
                                            {
                                                Id = id,
                                                ProductName = strProductName,
                                                ProductCode = strProductCode,
                                                Price = dPrice,
                                                ExpireDate = dtExpireDate,
                                                ProductType = strProductType,
                                                ManufacturerName = strManufactureName,
                                                Comments = strComments

                                            });
                                        }
                                        id++;
                                        //}
                                        rowIndex++;
                                    }

                                    Session["ExcelImport"] = excelImport.PExcelImport;
                                }
                            }
                        }


                        //}
                        break;

                    //case "ExcelTemplate":

                    //    var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("naberlyStorage"));
                    //    //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("naberlyStorage"));
                    //    CloudBlobClient cloudBlobClient = account.CreateCloudBlobClient();
                    //    CloudBlobContainer BlobContainer = cloudBlobClient.GetContainerReference("naberlyexcelfiles");
                    //    CloudBlob blob = BlobContainer.GetBlobReference("ExcelTemplate.xlsx");
                    //    MemoryStream memStream = new MemoryStream();
                    //    blob.DownloadToStream(memStream);

                    //    Response.ContentType = blob.Properties.ContentType;
                    //    Response.AddHeader("Content-Disposition", "Attachment; filename=ExcelTemplate.xlsx");
                    //    Response.AddHeader("Content-Length", blob.Properties.Length.ToString());
                    //    Response.BinaryWrite(memStream.ToArray());

                    //    break;

                    case "Import":
                        //  if ((selectedContactId != null) && (Session["ExcelImport"] != null))
                        if ((Session["ExcelImport"] != null))
                        {
                            List<PExcelImport> ExcelImportProducts = (List<PExcelImport>)Session["ExcelImport"];

                            var selectedItems = (from ci in ExcelImportProducts where selectedId.Contains(ci.Id) select ci).ToList();//where selectedContactId.Contains(ci.Id) select ci).ToList();
                            if (selectedItems.Count > 0)
                            {
                                foreach (var items in selectedItems)
                                {

                                    strProductName = items.ProductName;
                                    strProductCode = items.ProductCode;
                                    dPrice = items.Price;
                                    if (items.ExpireDate != null)
                                    {
                                        //dtExpireDate = DateTime.Parse(items.ExpireDate);
                                        dtExpireDate = items.ExpireDate;
                                    }
                                    strProductType = items.ProductType;
                                    strComments = items.Comments;
                                    strManufactureName = items.ManufacturerName;


                                    //response = client.GetAsync("api/Products/GetByCode?Code=" + strProductCode + "&Tid=" + TenentId).Result;
                                    //ProductModel ObjProductModel = new ProductModel();

                                    //if (response.IsSuccessStatusCode == false)
                                    //{
                                    //  ObjProductModel = response.Content.ReadAsAsync<ProductModel>().Result;


                                    //  var productExists = (from p in dbcontext.TblProducts where p.Code == strProductCode select p).FirstOrDefault();// where e.Email == strEmail select e).FirstOrDefault();


                                    //if (ObjProductModel == null)
                                    //{

                                    //  TblProduct product = new TblProduct();

                                    ProductModel product = new ProductModel();

                                    product.ProductName = strProductName;
                                    product.ProductCode = strProductCode;
                                    product.Price = dPrice;
                                    product.ExpiryDate = dtExpireDate;

                                    //product.ProductType = strProductType;

                                    if (!string.IsNullOrEmpty(strProductType))
                                    {
                                        response = client.GetAsync("api/Products/GetByType?prodType=" + strProductType + "&Tid=" + TenentId).Result;
                                        if (response.IsSuccessStatusCode)
                                        {
                                            var prodType = response.Content.ReadAsAsync<string>().Result;
                                            product.ProductType = prodType;
                                        }
                                    }

                                    product.ManufacturerName = strManufactureName;
                                    product.Comments = strComments;
                                    product.TenantId = TenentId;
                                    product.CreatedDate = DateTime.UtcNow;
                                    product.ModifiedDate = DateTime.UtcNow;
                                    product.Status = true;

                                    response = client.PostAsJsonAsync("api/Products/Insert", product).Result;

                                    if (response.IsSuccessStatusCode)
                                    {
                                        product = response.Content.ReadAsAsync<ProductModel>().Result;
                                        importProductId = Convert.ToInt32(product.Id);
                                    }
                                    //if (importContactId > 0)
                                    //    SendMail(strFirstName, strLastName, strEncryptPassword, strEmail, tenantGuid);

                                    //}
                                    //else
                                    //{


                                    //productExists.Code = strProductCode;
                                    //productExists.ProductName = strProductName;
                                    //productExists.Price = dPrice;
                                    //productExists.ExpiryDate = dtExpireDate;
                                    //productExists.Comments = strComments;
                                    //productExists.ManufactureName = strManufactureName; 

                                    //dbcontext.SaveChanges();
                                    //importProductId = Convert.ToInt32(productExists.Id);

                                    //}

                                    //}
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

            //  string ReadCSV = File.ReadAllText(CSVFilePath);
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

        public ActionResult Excel_Export()
        {
            List<ProductModel> ProductReports = new List<ProductModel>();
            List<ProductModel> product = new List<ProductModel>();


            response = client.GetAsync("api/Products/GetAllExport?Tid=" + TenentId).Result;

            if (response.IsSuccessStatusCode)
            {
                product = response.Content.ReadAsAsync<IEnumerable<ProductModel>>().Result.ToList();
            }
            foreach (var item in product)
            {
                ProductReports.Add(new ProductModel() { ProductCode = item.ProductCode, ProductName = item.ProductName, Price = item.Price, ProductTypeDes = item.ProductTypeDes, ManufacturerName = item.ManufacturerName, ExpiryDate = item.ExpiryDate, Comments = item.Comments });
            }

            Response.AddHeader("content-disposition", "attachment; filename=ProductExport.csv");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            htw.WriteLine("Product Name" + "," + "Product Code" + "," + "Price" + "," + "Product Type" + "," + "Manufacturer Name" + "," + "Expiry Date" + "," + "Comments");

            if (ProductReports != null)
            {
                foreach (var data in ProductReports)
                {
                    if (string.IsNullOrEmpty(data.ProductCode))
                    {
                        data.ProductCode = "Null";
                    }

                    if (string.IsNullOrEmpty(data.ProductName))
                    {
                        data.ProductName = "Null";
                    }

                    if (string.IsNullOrEmpty(data.ProductTypeDes))
                    {
                        data.ProductTypeDes = "Null";
                    }

                    if (data.Price == 0)
                    {
                        data.Price = 0;
                    }

                    if (data.ExpiryDate == null)
                    {
                        data.ExpiryDate = DateTime.Now.AddYears(1);
                    }


                    if (string.IsNullOrEmpty(data.Comments))
                    {
                        data.Comments = "Null";
                    }

                    if (string.IsNullOrEmpty(data.ManufacturerName))
                    {
                        data.ManufacturerName = "Null";
                    }


                    if (!string.IsNullOrEmpty(data.ProductName))
                    {
                        htw.WriteLine(data.ProductName.ToString() + "," + data.ProductCode.ToString() + "," + data.Price.ToString() + "," + data.ProductTypeDes.ToString() + "," + data.ManufacturerName.ToString() + "," + data.ExpiryDate.ToString() + "," + data.Comments.ToString());
                    }
                }
            }

            Response.Output.Write(sw.ToString());

            Response.Flush();
            Response.End();

            return Content("");

        }
        #endregion

        //#region Edit
        //[HttpPost]
        //public async System.Threading.Tasks.Task<ActionResult> ProductEdit(ProductModel objProductModel)
        //{
        //    try
        //    {
        //        var response = await client.PutAsJsonAsync<ProductModel>("api/Products/Edit", objProductModel);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var objResultValue = await response.Content.ReadAsAsync<ExtContactModel>();
        //            ViewData["objResultValue"] = objResultValue;
        //            return Json(objResultValue);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //#endregion

        #region LeadproductDelete
        [HttpPost]
        public ActionResult TempDelete([DataSourceRequest] DataSourceRequest request, ProductModel objProductModel)
        {
            try
            {
                return Json(new[] { objProductModel }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException();
            }
        }

        #endregion
    }
}
