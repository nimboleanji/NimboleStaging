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
using NIMBOLE.UI.Models;
using NIMBOLE.UI.Filters;
using System.Threading.Tasks;

namespace NIMBOLE.UI.Controllers
{
    public class LeadsController : BaseController
    {
        Guid TenentId = new Guid();
        long lgnEmpId;

        //protected HttpClient client;
        //protected HttpResponseMessage response;
        //protected Uri contactUri = null;
        //protected string strAPIURL = string.Empty;
        public LeadsController()
        {
            //BindCommonThings();
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
            lgnEmpId = System.Web.HttpContext.Current.Session["EmployeeId"] != null ? Convert.ToInt64(System.Web.HttpContext.Current.Session["EmployeeId"]) : 0;
        }

        #region Index
        //[EncryptedActionParameter]
        //public ActionResult Index([DataSourceRequest] DataSourceRequest request, string MilestoneId = "", string resHome = "")
        //{
        //    bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
        //    if (_objAuthorized)
        //    {
        //        int LeadId = 0;
        //        int MstID = 0;

        //        GetAllDropdownValues();

        //        if (!string.IsNullOrEmpty(MilestoneId) || MilestoneId != "Index")
        //        {
        //            if (int.TryParse(MilestoneId, out LeadId))
        //            {
        //                MstID = Convert.ToInt32(MilestoneId);
        //            }
        //        }
        //        var result = Json(null, JsonRequestBehavior.AllowGet);
        //        result = Leads_Read(request, LeadId, MstID, resHome, "", "");
        //        ViewData["resHome"] = resHome;
        //        return View();
        //    }
        //    return RedirectToAction("AccessDenied", "Error");
        //}
        [HttpGet]
        public ActionResult Index([DataSourceRequest] DataSourceRequest request, int LeadId = 0, int MstID = 0, string resHome = "", string str = "", string MilestoneId = "")
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                GetAllDropdownValues();
                if (!string.IsNullOrEmpty(MilestoneId) || MilestoneId != "Index")
                {
                    if (int.TryParse(MilestoneId, out LeadId))
                    {
                        MstID = Convert.ToInt32(MilestoneId);
                    }
                }
                response = client.GetAsync("api/Leads/GetAll?Id=" + LeadId.ToString() + "&Mstid=" + MstID.ToString() + "&resHome=" + resHome + "&EmpId=" + lgnEmpId + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objLeadModel = response.Content.ReadAsAsync<IEnumerable<LeadModel>>().Result;
                    return View(objLeadModel);
                }
            }
            return RedirectToAction("AccessDenied", "Error");
        }
        #endregion
        public void BindCommonThings()
        {
            strAPIURL = ConfigurationManager.AppSettings["ServiceUrl"].ToString();
            contactUri = new Uri(strAPIURL);
            client = new HttpClient();
            response = new HttpResponseMessage();
            strAPIURL = ConfigurationManager.AppSettings["ServiceUrl"].ToString();
            client.BaseAddress = contactUri;
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public JsonResult Leads_Read([DataSourceRequest] DataSourceRequest request, int LeadId = 0, int MstID = 0, string resHome = "", string str = "", string MilestoneId = "")
        {
            response = client.GetAsync("api/Leads/GetAll?Id=" + LeadId.ToString() + "&Mstid=" + MstID.ToString() + "&resHome=" + resHome + "&EmpId=" + lgnEmpId + "&Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objLeadModel = response.Content.ReadAsAsync<List<LeadModel>>().Result;

                if (string.IsNullOrEmpty(str))
                {
                    var result = objLeadModel.AsQueryable().ToDataSourceResult(request);
                    ViewData["LeadDetails"] = objLeadModel;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    str = str.ToLower();
                    var result = objLeadModel.AsQueryable().Where(l => l.LeadTitle.ToLower().StartsWith(str)).ToDataSourceResult(request);
                    ViewData["LeadDetails"] = objLeadModel;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null);
        }

        #region GetLeads
        public JsonResult LeadsWithFilter([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Leads/SelectAllLeads?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objLeadModel = response.Content.ReadAsAsync<IEnumerable<LeadModel>>().Result.ToList();
                return Json(objLeadModel, JsonRequestBehavior.AllowGet);
            }
            return Json(null);

        }
        public JsonResult GetAllLeads([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                response = client.GetAsync("api/Leads/SelectAllLeads?Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objLeadModel = response.Content.ReadAsAsync<IEnumerable<LeadModel>>().Result;
                    return Json(objLeadModel, JsonRequestBehavior.AllowGet);
                }
                return Json("Error1: " + response.RequestMessage.Content);
            }
            catch (Exception ex)
            {
                return Json("Error2: " + ex.Message);
            }
        }

        #endregion

        #region Milestones
        [HttpGet]
        public JsonResult GetAllMileStones([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                response = client.GetAsync("api/MileStones/SelectAllMileStone?Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objMileStoneModel = response.Content.ReadAsAsync<IEnumerable<MilestoneModel>>().Result;
                    return Json(objMileStoneModel, JsonRequestBehavior.AllowGet);
                }
                return Json("Error1: " + response.RequestMessage.Content);
            }
            catch (Exception ex)
            {
                return Json("Error2: " + ex.Message);
            }
        }
        public async Task<JsonResult> MileStone_Read([DataSourceRequest] DataSourceRequest request, long leadId)
        {
            try
            {
                // long iLeadId = Convert.ToInt64(Session["CurrentLeadId"]);
                if (leadId > 0)
                {
                    response = await client.GetAsync("api/MileStones/GetAllByLeadId?iLeadId=" + leadId + "&Tid=" + TenentId);
                    if (response.IsSuccessStatusCode)
                    {
                        var lstData = response.Content.ReadAsAsync<IEnumerable<MilestoneModel>>().Result;

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

        #region CREATE
        [HttpPost]
        public ActionResult CreateMilestone(MilestoneModel ObjMilestoneModel)
        {
            try
            {
                ObjMilestoneModel.TenantId = TenentId;
                response = client.PostAsJsonAsync("api/MileStones/Insert", ObjMilestoneModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var mileStones = response.Content.ReadAsAsync<MilestoneModel>().Result;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Milestone Created Successfully."
                    });
                    return RedirectToAction("Index", "Milestone");
                }
                else
                {
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Error,
                        MessageHeading = "Failure",
                        MessageString = response.ReasonPhrase
                    });
                    return RedirectToAction("Index", "Milestone");
                }
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Post
        [HttpGet]
        public ActionResult EditMilestone(int id)
        {
            response = client.GetAsync("api/Milestones/GetById?id=" + id + "&Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                MilestoneModel objMilestoneModel = response.Content.ReadAsAsync<MilestoneModel>().Result;

                return View(objMilestoneModel);
            }
            return View("Record Not Found");
        }

        public async System.Threading.Tasks.Task<ActionResult> EditMilestone(MilestoneModel objMilestoneModel)
        {
            try
            {
                var response = await client.PutAsJsonAsync<MilestoneModel>("api/MileStones/Edit", objMilestoneModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<MilestoneModel>();
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
        public async System.Threading.Tasks.Task<ActionResult> DeleteMilestone(int id)
        {
            try
            {
                response = await client.DeleteAsync(strAPIURL + "api/Milestones/Delete?id=" + id + "&Tid=" + TenentId);
                if (response.IsSuccessStatusCode)
                {
                    var MilestoneId = response.Content.ReadAsAsync<MilestoneModel>().Result;
                    return View(MilestoneId);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message.ToString());
            }
            return RedirectToAction("index");
        }
        #endregion
        #endregion

        #region Reporting

        #region AllLeads Actions and GetAllLeads
        [HttpPost]
        public ActionResult AllLeads(long iAccountId)
        {
            IEnumerable<LeadModel> lstLeadModel = GetAllLeadsForReports();
            lstLeadModel = lstLeadModel.Where(c => c.AccountId == iAccountId).ToList();
            Session["ReportType"] = "AllLeads";
            Session["DataToBind"] = lstLeadModel;
            return View();
        }
        [HttpGet]
        public ActionResult AllLeads()
        {
            IEnumerable<LeadModel> lstLeadModel = GetAllLeadsForReports();
            Session["ReportType"] = "AllLeads";
            Session["DataToBind"] = lstLeadModel;
            return View();
        }
        private IEnumerable<LeadModel> GetAllLeadsForReports()
        {
            IEnumerable<LeadModel> cm = new List<LeadModel>();
            response = client.GetAsync("api/Leads/GetAllForReports").Result;
            if (response.IsSuccessStatusCode)
            {
                cm = response.Content.ReadAsAsync<IEnumerable<LeadModel>>().Result;
            }
            return cm;
        }
        #endregion

        #region ActualVsExpected

        [HttpGet]
        public ActionResult ActualVsExpected()
        {
            int employeeId = Session["EmployeeId"] != null ? Convert.ToInt32(Session["EmployeeId"]) : 0;
            string finYear = DateTime.Now.Month < 3 ? DateTime.Now.AddYears(-1).Year.ToString() + "-" + DateTime.Now.Year.ToString() : DateTime.Now.Year.ToString() + "-" + DateTime.Now.AddYears(1).Year.ToString();

            Session["ReportType"] = "ActualVsExpected";
            Session["EmployeeId"] = employeeId;
            Session["FinYear"] = finYear;
            GetActualVsExpectedLeadsForReports(employeeId, finYear);
            BindDropDownValues();
            return View();
        }

        public ActionResult GetEmployeesForDropdown([DataSourceRequest] DataSourceRequest request)
        {
            string EmpRole = Session["EmployeeRole"] != null ? Session["EmployeeRole"].ToString() : "";
            long EmpId = Session["EmployeeId"] != null ? Convert.ToInt64(Session["EmployeeId"]) : 0;
            long EmpRoleId = Session["EmployeeRoleId"] != null ? Convert.ToInt64(Session["EmployeeRoleId"]) : 0;

            response = client.GetAsync("api/Employees/SelectAllEmployeesForReport?EmpId=" + EmpId + "&Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var employees = response.Content.ReadAsAsync<IEnumerable<EmpRoleModel>>().Result.ToList();
                EmpRoleModel _objItem = new EmpRoleModel();
                _objItem.Id = EmpId;
                _objItem.Name = "Me";
                _objItem.FirstName = "Me";
                _objItem.LastName = "Me";
                employees.Insert(0, _objItem);// .Add(_objItem);
                return Json(employees, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        public ActionResult GetEmployeesForReports([DataSourceRequest] DataSourceRequest request)
        {
            string EmpRole = Session["EmployeeRole"] != null ? Session["EmployeeRole"].ToString() : "";
            long EmpId = Session["EmployeeId"] != null ? Convert.ToInt64(Session["EmployeeId"]) : 0;
            long EmpRoleId = Session["EmployeeRoleId"] != null ? Convert.ToInt64(Session["EmployeeRoleId"]) : 0;

            response = client.GetAsync("api/Employees/SelectAllEmployeesForReport?EmpId=" + EmpId + "&Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                //EmpRoleModel _objItem = new EmpRoleModel();
                var objEmployeeModel = response.Content.ReadAsAsync<IEnumerable<EmpRoleModel>>().Result.ToList();
                for (int i = 0; i < objEmployeeModel.Count; i++)
                {
                    if (objEmployeeModel[i].Id == EmpId)
                    {
                        objEmployeeModel[i].Name = "Me";
                        objEmployeeModel[i].FirstName = "Me";
                        objEmployeeModel[i].LastName = "Me";
                        break;
                    }
                }
                //_objItem.Id = EmpId;
                //_objItem.Name = "Me";
                //_objItem.FirstName = "Me";
                //_objItem.LastName = "Me";
                //objEmployeeModel.Insert(0, _objItem);// .Add(_objItem);
                ViewData["Employees"] = new SelectList(objEmployeeModel, "Id", "FirstName");
                return Json(objEmployeeModel, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        private void BindDropDownValues()
        {
            //Employees
            string EmpRole = Session["EmployeeRole"] != null ? Session["EmployeeRole"].ToString() : "";
            long EmpId = Session["EmployeeId"] != null ? Convert.ToInt64(Session["EmployeeId"]) : 0;
            long EmpRoleId = Session["EmployeeRoleId"] != null ? Convert.ToInt64(Session["EmployeeRoleId"]) : 0;

            if (EmpId == 2)
                response = client.GetAsync("api/Employees/SelectAllEmployeesForReport?EmpId=" + EmpRoleId + "&Tid=" + TenentId).Result;
            else
                response = client.GetAsync("api/Employees/SelectAllEmployeesForReport?EmpId=" + EmpId + "&Tid=" + TenentId).Result;

            if (response.IsSuccessStatusCode)
            {
                EmpRoleModel _objItem = new EmpRoleModel();
                var objEmployeeModel = response.Content.ReadAsAsync<IEnumerable<EmpRoleModel>>().Result.ToList();
                _objItem.Id = 0;
                //_objItem.Name = Session["EmployeeName"] != null ? Session["EmployeeName"].ToString() : "";
                _objItem.Name = "Me";
                objEmployeeModel.Insert(0, _objItem);// .Add(_objItem);
                ViewData["Employees"] = objEmployeeModel;
                //if (EmpRole == "SM")
                //{
                //    //ViewData["Employees"] = new SelectList(objEmployeeModel, "Name", "Name", new object { });
                //    ViewData["Employees"] = objEmployeeModel;
                //}
                //else
                //{
                //    //ViewData["Employees"] = new SelectList(objEmployeeModel, "Name", "Name", new object { });
                //    ViewData["Employees"] = objEmployeeModel;
                //}                
            }

            //FinancialYear
            response = client.GetAsync("api/FinancialYear/GetFinancialYearsForReports?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var lstObjFinancialYearModel = response.Content.ReadAsAsync<IEnumerable<FinancialYearModel>>().Result;
                ViewData["FinYears"] = new SelectList(lstObjFinancialYearModel, "FinancialYear", "FinancialYear", new object { });
            }
        }

        [HttpPost]
        public ActionResult ActualVsExpected(string onsubmit = "")
        {
            BindDropDownValues();
            int employeeId = Convert.ToInt32(Request.Form["hdnEmployee"] == "" || Request.Form["hdnEmployee"] == "Select" ? Session["EmployeeId"] : Request.Form["hdnEmployee"]);
            string financialYear = Request.Form["hdnFinYear"];
            if (employeeId > 0)
            {
                Session["EmployeeId"] = employeeId;
                Session["FinYear"] = financialYear;
                GetActualVsExpectedLeadsForReports(employeeId, financialYear);
                return View();
            }
            return View();
        }

        private void GetActualVsExpectedLeadsForReports(int EmployeeId, string FinYear)
        {
            IEnumerable<ActualVsExpectedModel> _data = new List<ActualVsExpectedModel>();
            //string url = "api/Leads/GetActualVsExpectedForReports?EmpId=" + (EmployeeId == 2 ? 0 : EmployeeId) + "&FinYear=" + FinYear;
            string url = "api/Leads/GetActualVsExpectedForReports?EmpId=" + EmployeeId + "&FinYear=" + FinYear;
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                _data = response.Content.ReadAsAsync<IEnumerable<ActualVsExpectedModel>>().Result;
                Session["EmployeeTargets"] = _data;
            }
        }
        #endregion

        #region SalesFunnel
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult SalesFunnel()
        {
            Session["ReportType"] = "SalesFunnel";
            IEnumerable<SalesFunelModel> _data = new List<SalesFunelModel>();
            string url = "api/Leads/GetSalesByMilestone";
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                _data = response.Content.ReadAsAsync<IEnumerable<SalesFunelModel>>().Result;
                Session["SalesFunnel"] = _data;
            }
            return View();
        }

        public void LeadsByMilestones(string milestone)
        {
            //Session["ReportType"] = "LeadsByMilestone";
            IEnumerable<LeadModel> _data = new List<LeadModel>();
            string url = "api/Leads/GetLeadsByMilestone?milestone=" + milestone;
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                _data = response.Content.ReadAsAsync<IEnumerable<LeadModel>>().Result;
                Session["LeadsByMilestone"] = _data;
            }
        }

        public ActionResult LeadsByMilestone(string milestone)
        {
            Session["ReportType"] = "LeadsByMilestone";
            IEnumerable<LeadModel> _data = new List<LeadModel>();
            string url = "api/Leads/GetLeadsByMilestone?milestone=" + milestone;
            response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                _data = response.Content.ReadAsAsync<IEnumerable<LeadModel>>().Result;
                Session["LeadsByMilestone"] = _data;
            }
            return View();
        }
        #endregion SalesFunnel

        #region ActivityByMSId
        [HttpGet]
        public ActionResult ActivityByMSId()
        {
            Session["MileStoneId"] = "-1";
            GetActivityByMSId("-1");
            return View();
            //return PartialView("RptIndex");
        }
        [HttpPost]
        public ActionResult ActivityByMSId(string strDdlMileStones)
        {
            if (strDdlMileStones != null)
            {
                GetActivityByMSId(strDdlMileStones);
                Session["MileStoneId"] = strDdlMileStones;
                return PartialView("RptIndex");
            }
            return View();
        }
        private void GetActivityByMSId(string strMSId)
        {
            IEnumerable<ActivityByMilestone> lstActivityByMilestone = new List<ActivityByMilestone>();
            response = client.GetAsync("api/Activity/GetActivityByMilestoneId?msId=" + strMSId).Result;
            if (response.IsSuccessStatusCode)
            {
                lstActivityByMilestone = response.Content.ReadAsAsync<IEnumerable<ActivityByMilestone>>().Result;
                Session["DataToBind"] = lstActivityByMilestone;
            }
        }
        #endregion

        [HttpGet]
        public ActionResult LeadRptByParameter()
        {
            IEnumerable<LeadModel> cm = GetAllLeadsForReports();
            Session["cm"] = cm;
            ReportViewer rptVwr = new ReportViewer();
            rptVwr.LocalReport.ReportPath = Server.MapPath("~/Reporting/LeadRptByParameter.rdlc");
            ReportDataSource rdc = new ReportDataSource("DSet1", cm);
            //ReportViewer1.LocalReport.DataSources.Clear();
            //ReportViewer1.LocalReport.DataSources.Remove(rdc);
            rptVwr.LocalReport.DataSources.Add(rdc);
            rptVwr.DataBind();
            rptVwr.LocalReport.Refresh();
            ReportParameter rptParam = new ReportParameter();
            //ReportDataSource rds = new ReportDataSource("UnitPerformanceDataSet_sp_RptUnitPerformance", ds.Tables[0]);
            return View();
        }
        //LeadRptByAccountId?printForamt="PDF"&iAccountId=2
        public ActionResult LeadRptByAccountId(string printForamt = "Excel", long iAccountId = 2)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reporting"), "rptLeadAccountById.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            IEnumerable<LeadModel> cm = new List<LeadModel>();
            response = client.GetAsync("api/Leads/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                cm = response.Content.ReadAsAsync<IEnumerable<LeadModel>>().Result;
            }
            cm = cm.Where(c => c.AccountId == iAccountId).ToList();


            ReportDataSource rd = new ReportDataSource("DSet1", cm);
            lr.DataSources.Add(rd);
            string reportType = printForamt;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + printForamt + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            ReportParameter rp = new ReportParameter("rptParamAccId", iAccountId.ToString());

            lr.SetParameters(new ReportParameter[] { rp });
            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }
        //Report?id=PDF,Excel,Word,Image
        public ActionResult Report(string id)
        {
            id = "Excel";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reporting"), "LeadAccountById.rdlc");
            if (System.IO.File.Exists(path))
                lr.ReportPath = path;
            else
                return View("Index");

            IEnumerable<LeadModel> cm = new List<LeadModel>();
            response = client.GetAsync("api/Leads/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                cm = response.Content.ReadAsAsync<IEnumerable<LeadModel>>().Result;
            }


            ReportDataSource rd = new ReportDataSource("DSet1", cm);
            lr.DataSources.Add(rd);
            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);


            return File(renderedBytes, mimeType);
        }
        #endregion

        #region Create
        #region New Leads Product Create
        public ActionResult EditingInline_Read([DataSourceRequest] DataSourceRequest request)
        {
            List<ProductModel> lstProductModel = new List<ProductModel>();
            response = client.GetAsync("api/Leads/ProductRead").Result;
            if (response.IsSuccessStatusCode)
            {
                var objLeadModel = response.Content.ReadAsAsync<IEnumerable<LeadModel>>().Result;
                return Json(lstProductModel.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return Json(lstProductModel.ToDataSourceResult(request));
        }
        #endregion

        [HttpGet]
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
        public ActionResult Create(LeadModel objLeadModel)
        {
            try
            {
                #region New
                string strData = Request["hdnGridData"].ToString();
                string strExtCntData = Request["hdnContactGrid"].ToString();
                objLeadModel.TenantId = TenentId;
                objLeadModel.ProductString = strData;
                objLeadModel.ContactJsonArray = strExtCntData;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                if (!string.IsNullOrEmpty(strData))
                {
                    List<ProductModel> listProducts = serializer.Deserialize<List<ProductModel>>(strData);
                    objLeadModel.Size = 0;
                    foreach (var item in listProducts)
                    {
                        objLeadModel.Size = objLeadModel.Size + Convert.ToInt64(item.Quantity * item.Price);
                    }
                }
                if (!string.IsNullOrEmpty(Request["hdnAccount"]))
                {
                    objLeadModel.AccountId = Convert.ToInt64(Request["hdnAccount"].ToString());
                }
                if (!string.IsNullOrEmpty(Request["hdnSource"]))
                {
                    objLeadModel.LeadSourceId = Convert.ToInt64(Request["hdnSource"].ToString());
                }
                #endregion
                objLeadModel.TenantId = TenentId;
                if (Session["EmployeeId"] != null)
                {
                    string strLeadOwnerId = Session["EmployeeId"].ToString();
                    objLeadModel.LeadOwnerId = Convert.ToInt64(strLeadOwnerId);
                }
                string lstemployee = string.Empty;
                if (objLeadModel.LeadEmployees != null)
                {
                    lstemployee = string.Join(",", objLeadModel.LeadEmployees);
                    objLeadModel.LeadEmployees.Add(lstemployee);
                }
                // Transaction 
                if (objLeadModel.objLeadTransactionInfoModel != null)
                {
                    if (!string.IsNullOrEmpty(objLeadModel.objLeadTransactionInfoModel.TransName))
                    {
                        List<string> urls = (List<string>)Session["TransactionDocUrls"];
                        objLeadModel.objLeadTransactionInfoModel.TenantId = TenentId;
                        objLeadModel.objLeadTransactionInfoModel.currentUser = Session["User"].ToString();
                        objLeadModel.objLeadTransactionInfoModel.lstURLs = urls;
                        Session["TransactionDocUrls"] = null;
                    }
                }
                response = client.PostAsJsonAsync("api/Leads/Insert", objLeadModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = response.Content.ReadAsAsync<LeadModel>();
                    ViewData["objResultValue"] = objResultValue.Result.Id;

                    if (Convert.ToInt64(objResultValue.Result.Id) > 0)
                    {
                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.NimboleHub>();
                        HomeController _objHomeController = new HomeController();
                        hubContext.Clients.All.update(_objHomeController.IndexForHub());
                        this.SetAlert(new AlertMessageViewModel()
                        {
                            MessageType = MessageType.Success,
                            MessageHeading = "Success",
                            MessageString = "Lead Created Successfully."
                        });
                        return RedirectToAction("Index");
                    }
                    else
                        return null;
                }
                else
                {
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Error,
                        MessageHeading = "Failure",
                        MessageString = response.ReasonPhrase
                    });
                    return RedirectToAction("Index");
                }
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
        public ActionResult CreateNew(LeadNewModel objLeadNewModel)
        {
            try
            {
                #region New

                string strData = Request["hdnGridData"].ToString();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                if (!string.IsNullOrEmpty(strData))
                {
                    List<ProductModel> listProducts = serializer.Deserialize<List<ProductModel>>(strData);
                    objLeadNewModel.Size = 0;
                    foreach (var item in listProducts)
                    {
                        objLeadNewModel.Size = objLeadNewModel.Size + Convert.ToInt64(item.Quantity * item.Price);
                    }
                }

                if (!string.IsNullOrEmpty(Request["hdnAccount"]))
                {
                    objLeadNewModel.AccountId = Convert.ToInt64(Request["hdnAccount"].ToString());
                }
                if (!string.IsNullOrEmpty(Request["hdnSource"]))
                {
                    objLeadNewModel.LeadSourceId = Convert.ToInt64(Request["hdnSource"].ToString());
                }

                #endregion

                objLeadNewModel.TenantId = TenentId;
                if (Session["EmployeeId"] != null)
                {
                    string strLeadOwnerId = Session["EmployeeId"].ToString();
                    objLeadNewModel.LeadOwnerId = Convert.ToInt64(strLeadOwnerId);
                }

                string lstemployee = string.Empty;
                if (objLeadNewModel.LeadEmployees != null)
                {
                    lstemployee = string.Join(",", objLeadNewModel.LeadEmployees);
                    objLeadNewModel.LeadEmployees.Add(lstemployee);
                }

                objLeadNewModel.TenantId = TenentId;
                response = client.PostAsJsonAsync("api/Leads/InsertNew", objLeadNewModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = response.Content.ReadAsAsync<LeadNewModel>();
                    ViewData["objResultValue"] = objResultValue.Result.Id;

                    if (Convert.ToInt64(objResultValue.Result.Id) > 0)
                    {
                        if (!string.IsNullOrEmpty(strData))
                        {
                            List<ProductModel> listProducts = serializer.Deserialize<List<ProductModel>>(strData);

                            foreach (var item in listProducts)
                            {
                                TransLProductModel objTransLProductModel = new TransLProductModel();
                                objTransLProductModel.TenantId = TenentId;
                                objTransLProductModel.CompetitorId = item.Id;
                                objTransLProductModel.LeadId = objResultValue.Result.Id;
                                objTransLProductModel.ProductId = item.Id;
                                objTransLProductModel.Price = item.Price;
                                objTransLProductModel.Code = item.ProductCode;
                                objTransLProductModel.Quantity = item.Quantity;
                                objTransLProductModel.CreatedDate = DateTime.Now;
                                objTransLProductModel.ModifiedDate = DateTime.Now;
                                //objTransLProductModel.TenantId = item.TenantId.ToDefaultTenantId();
                                objTransLProductModel.Status = true;

                                response = client.PostAsJsonAsync("api/TransLeadCompetitor/Insert", objTransLProductModel).Result;
                                if (response.IsSuccessStatusCode)
                                {
                                }
                            }
                        }

                        //ExtContact 
                        string strExtCntData = Request["hdnContactGrid"].ToString();
                        JavaScriptSerializer serializercontact = new JavaScriptSerializer();

                        if (!string.IsNullOrEmpty(strExtCntData))
                        {
                            List<ExtContactModel> listExtContacts = serializercontact.Deserialize<List<ExtContactModel>>(strExtCntData);

                            foreach (var item in listExtContacts)
                            {
                                if (!string.IsNullOrEmpty(item.ExtContactRole))
                                {
                                    ExtContactModel objExtContactModel = new ExtContactModel();
                                    objExtContactModel.TenantId = TenentId;
                                    objExtContactModel.LeadId = objResultValue.Result.Id;
                                    objExtContactModel.ExtContactId = item.ExtContactId;
                                    objExtContactModel.FullName = item.FullName;
                                    objExtContactModel.WorkEmail = item.WorkEmail;
                                    objExtContactModel.ExtContactRoleId = item.ExtContactRoleId;
                                    objExtContactModel.ExtContactRole = item.ExtContactRole;
                                    // objExtContactModel.TenantId = item.TenantId.ToDefaultTenantId();

                                    response = client.PostAsJsonAsync("api/TranExtContact/InsertLeadCreateTime", objExtContactModel).Result;
                                    if (response.IsSuccessStatusCode)
                                    {
                                    }
                                }
                            }
                        }
                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.NimboleHub>();
                        HomeController _objHomeController = new HomeController();
                        hubContext.Clients.All.update(_objHomeController.IndexForHub());
                        this.SetAlert(new AlertMessageViewModel()
                        {
                            MessageType = MessageType.Success,
                            MessageHeading = "Success",
                            MessageString = "Lead Created Successfully."
                        });

                        //return RedirectToAction("Edit/" + objResultValue.Result.Id, "Leads");
                        return RedirectToAction("Index");
                    }
                    else
                        return null;
                }
                else
                {
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Error,
                        MessageHeading = "Failure",
                        MessageString = response.ReasonPhrase
                    });
                    return RedirectToAction("Index");
                }
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

        #region Edit
        [HttpGet]
        [EncryptedActionParameter]
        public ActionResult Edit(int Id)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                GetAllDropdownValues();
                #region Load Lead/Edit Page Main Panel Details
                response = client.GetAsync("api/Leads/GetById?id=" + Id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    string errVal = response.Content.ReadAsStringAsync().Result;
                    if (!errVal.Contains("Failure"))
                    {
                        var objLeadModel = response.Content.ReadAsAsync<LeadNewModel>().Result;
                        if (objLeadModel.Id > 0)
                        {
                            Session["CurrentLeadId"] = Id;
                            //response = client.GetAsync("api/LeadPriceDiscounts/GetRecent?leadId=" + Id + "&Tid=" + TenentId).Result;
                            //if (response.IsSuccessStatusCode)
                            //{
                            //    var objTransLeadPriceDescountModel = response.Content.ReadAsAsync<LeadPriceDiscountModel>().Result;
                            //    objLeadModel.objLeadPriceDiscountModel = objTransLeadPriceDescountModel;
                            //}
                            //else
                            //    objLeadModel.objLeadPriceDiscountModel = new LeadPriceDiscountModel();


                            //response = client.GetAsync("api/Transaction/GetTransactionByLeadId?iLeadId=" + Id + "&Tid=" + TenentId).Result;
                            //if (response.IsSuccessStatusCode)
                            //{
                            //    var objLeadTransactionInfoModel = response.Content.ReadAsAsync<LeadTransactionInfoModel>().Result;
                            //    objLeadModel.objLeadTransactionInfoModel = objLeadTransactionInfoModel;
                            //}
                            //else
                            //{
                            //    objLeadModel.objLeadTransactionInfoModel = new LeadTransactionInfoModel();
                            //}

                            //objLeadModel.objActivityModel = new ActivityModel();
                            ViewData["LeadBudget"] = objLeadModel.Budget;
                            return View(objLeadModel);
                        }
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
                    return RedirectToAction("Index");
                }
                else
                {
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Error,
                        MessageHeading = "Failure",
                        MessageString = response.ReasonPhrase
                    });
                    return View();
                }
                #endregion
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        [HttpPost]
        public ActionResult Edit(LeadNewModel objLeadModel)
        {
            try
            {
                long strSize = 0;
                string type = Request.Form["hdnType"].ToString();
                if (objLeadModel.Id != 0)
                {
                    if (Request["Size"] != null)
                    {
                        //strSize = Convert.ToInt64(Request["Size"].Trim(Convert.ToChar(Session["Currency"])).Trim(' '));
                        strSize = Convert.ToInt64(Request["Size"]);
                    }
                    Session["CurrentLeadId"] = objLeadModel.Id.ToString();
                    objLeadModel.Size = strSize;

                    if (Session["EmployeeId"] != null)
                    {
                        string strModifiedLeadOwnerId = Session["EmployeeId"].ToString();
                        objLeadModel.ModifiedLeadOwnerId = Convert.ToInt64(strModifiedLeadOwnerId);
                    }

                    response = client.PutAsJsonAsync<LeadNewModel>("api/Leads/Edit", objLeadModel).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var objResultValue = response.Content.ReadAsAsync<LeadModel>();
                        ViewData["objResultValue"] = objResultValue;
                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.NimboleHub>();
                        HomeController _objHomeController = new HomeController();
                        hubContext.Clients.All.update(_objHomeController.IndexForHub());
                        this.SetAlert(new AlertMessageViewModel()
                        {
                            MessageType = MessageType.Success,
                            MessageHeading = "Success",
                            MessageString = "Lead Updated Successfully"
                        });
                        if (type == "btnSubmit1")
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            //long LeadId = Convert.ToInt64(Session["CurrentLeadId"].ToString());
                            //return RedirectToAction("Edit/" + objLeadModel.Id, "Leads");
                            return Redirect(Request.UrlReferrer.OriginalString);
                        }
                    }
                    else
                    {
                        this.SetAlert(new AlertMessageViewModel()
                        {
                            MessageType = MessageType.Error,
                            MessageHeading = "Failure",
                            MessageString = response.ReasonPhrase
                        });
                        if (type == "btnSubmit1")
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return Redirect(Request.UrlReferrer.OriginalString);
                        }
                    }
                }
                else
                {
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Error,
                        MessageHeading = "Failure",
                        MessageString = "Invalid Operation Done"
                    });
                    return RedirectToAction("Index");
                }
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
        public ActionResult UpdateValue(long leadValue)
        {
            try
            {
                long leadId = Convert.ToInt64(Session["CurrentLeadId"]);
                string leadData = (leadId + "," + leadValue).ToString();
                response = client.PutAsJsonAsync("api/Leads/UpdateValue", leadData).Result;
                if (response.IsSuccessStatusCode)
                {
                    return Content("Success");
                }
                return Content("");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message.ToString());
                return Content("");
            }
        }
        #endregion

        #region DocumentCreate

        BlobStorageService _objBlobStorageService = new BlobStorageService();

        public ActionResult DocumentCreate()
        {
            CloudBlobContainer blobContainer = _objBlobStorageService.GetCloudBlobContainer();

            List<string> blobs = new List<string>();
            foreach (var blobItem in blobContainer.ListBlobs())
            {
                blobs.Add(blobItem.Uri.ToString());
            }

            return View(blobs);
        }

        [HttpPost]
        public ActionResult CreateDocuments(DocumentModel model, IEnumerable<HttpPostedFileBase> files, string DocType)
        {
            List<string> urls = new List<string>();
            try
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var destinationPath = Path.Combine(Server.MapPath("~/Images/"), file.FileName);
                        file.SaveAs(destinationPath);
                        Stream fileStream = System.IO.File.OpenRead(destinationPath);
                        string docURL = LeadDocumentsCloudStorage.StoreInBlob(fileStream, TenentId, file.FileName, DocType);
                        urls.Add(docURL);

                        if (System.IO.File.Exists(destinationPath))
                        {
                            fileStream.Flush();
                            fileStream.Dispose();
                            System.IO.File.Delete(destinationPath);
                        }
                    }
                    if (DocType == "Activity")
                    {
                        if (Session["ActivityDocUrls"] != null)
                        {
                            List<string> _tempList = (List<string>)Session["ActivityDocUrls"];
                            for (int i = 0; i < _tempList.Count; i++)
                            {
                                urls.Add(_tempList[i].ToString());
                            }
                        }
                        Session["ActivityDocUrls"] = urls;
                    }
                    else if (DocType == "Transaction")
                    {
                        if (Session["TransactionDocUrls"] != null)
                        {
                            List<string> _tempList = (List<string>)Session["TransactionDocUrls"];
                            for (int i = 0; i < _tempList.Count; i++)
                            {
                                urls.Add(_tempList[i].ToString());
                            }
                        }
                        Session["TransactionDocUrls"] = urls;
                    }
                    else if (DocType == "Lead")
                        Session["LeadDocUrls"] = urls;
                }
                return Content("");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        #endregion

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
            //ProductTypes
            response = client.GetAsync("api/Products/SelectAllProductTypes?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objProductTypes = response.Content.ReadAsAsync<IEnumerable<ProductTypeModel>>().Result;
                ViewData["ProductTypes"] = objProductTypes.ToList();
            }
            //ProductNames
            response = client.GetAsync("api/Products/SelectAllProducts?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objProducts = response.Content.ReadAsAsync<IEnumerable<ProductNamesModel>>().Result;
                ViewData["ProductNames"] = objProducts.ToList();
            }
            //ContactRoles
            response = client.GetAsync("api/ContactRoles/AllContactRole?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objProducts = response.Content.ReadAsAsync<IEnumerable<ContactRoleModel>>().Result;
                ViewData["ContactRoles"] = objProducts.ToList();
            }

        }

        #region Delete
        [HttpDelete]
        [Route("Delete")]
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, LeadModel objLeadModel, string[] selectedId, bool status)
        {
            try
            {
                string strselectedId = string.Join(",", selectedId);
                response = await client.DeleteAsync(strAPIURL + "api/Leads/Delete?selectedId=" + strselectedId + "&status=" + status);
                if (response.IsSuccessStatusCode)
                {
                    // objLeadModel = response.Content.ReadAsAsync<LeadModel>().Result;
                    //var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.NimboleHub>();
                    //HomeController _objHomeController = new HomeController();
                    //hubContext.Clients.All.update(_objHomeController.IndexForHub());
                    return Json(new[] { objLeadModel }.ToDataSourceResult(request, ModelState));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message.ToString());
            }
            return RedirectToAction("Index");
        }


        [HttpDelete]
        [Route("DeleteRec")]
        public async System.Threading.Tasks.Task<ActionResult> DeleteRec([DataSourceRequest] DataSourceRequest request, string[] selectedId)
        {
            try
            {
                string strselectedId = string.Join(",", selectedId);
                response = await client.DeleteAsync(strAPIURL + "api/Leads/DeleteRec?selectedId=" + strselectedId);
                if (response.IsSuccessStatusCode)
                {
                    var objLeadModel = response.Content.ReadAsAsync<LeadModel>().Result;
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.NimboleHub>();
                    HomeController _objHomeController = new HomeController();
                    hubContext.Clients.All.update(_objHomeController.IndexForHub());
                    return Json(new[] { objLeadModel }.ToDataSourceResult(request));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message.ToString());
            }
            return RedirectToAction("Index");
        }
        #endregion

    }
}
