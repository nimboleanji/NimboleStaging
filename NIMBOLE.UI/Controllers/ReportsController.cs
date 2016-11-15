using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using System.Web;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using NIMBOLE.UI;
using NIMBOLE.UI.Reporting;
using NIMBOLE.Models.Models;
using NIMBOLE.UI.Filters;

namespace NIMBOLE.UI.Controllers
{
    public class ReportsController : BaseController
    {
        private NimboleReportingDataSet tds = new NimboleReportingDataSet();
        Guid TenentId = new Guid();
        int employeeId = 0;
        string financialYear = string.Empty;

        public ReportsController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }

        /// <summary>
        /// Creates a ReportViewer control and stores it on the ViewBag
        /// </summary>
        /// <returns></returns>
        [EncryptedActionParameter]
        public ActionResult ShowReport(string rptName, string onsubmit = "")
        {
            try
            {
                bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
                if (_objAuthorized)
                {
                    if (rptName.Contains('='))
                    {
                        rptName = EncryptedActionParameterAttribute.DecryptStringAES(rptName);
                    }
                    string finYear = DateTime.Now.Month <= 3 ? DateTime.Now.AddYears(-1).Year.ToString() + "-" + DateTime.Now.Year.ToString() : DateTime.Now.Year.ToString() + "-" + DateTime.Now.AddYears(1).Year.ToString();
                    employeeId = Convert.ToInt32(Request.Form["hdnEmployee"] == "" || Request.Form["hdnEmployee"] == "Select" || Request.Form["hdnEmployee"] == null ? Session["EmployeeId"] : Request.Form["hdnEmployee"]);
                    financialYear = Request.Form["hdnFinYear"] == "" || Request.Form["hdnFinYear"] == null ? finYear : Request.Form["hdnFinYear"];
                    if (employeeId > 0)
                    {
                        Session["EmployeeId"] = employeeId;
                    }
                    Session["FinYear"] = financialYear;
                    ViewData["ReportName"] = rptName;
                    SetSalesFunnelReport(rptName);
                    return View();
                }
                return RedirectToAction("AccessDenied", "Error");
            }
            catch (Exception ex)
            {
                var excepiton = ex;
                return RedirectToAction("Index", "Error");
            }
        }


       //[HttpGet]
        public ActionResult ShowMapReport(string rptName, string onsubmit = "")
        
        {
            try
            {
                //bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
                //if (_objAuthorized)
                //{
                    //if (rptName.Contains('='))
                    //{
                    //    rptName = EncryptedActionParameterAttribute.DecryptStringAES(rptName);
                    //}
                    //string finYear = DateTime.Now.Month <= 3 ? DateTime.Now.AddYears(-1).Year.ToString() + "-" + DateTime.Now.Year.ToString() : DateTime.Now.Year.ToString() + "-" + DateTime.Now.AddYears(1).Year.ToString();
                    //employeeId = Convert.ToInt32(Request.Form["hdnEmployee"] == "" || Request.Form["hdnEmployee"] == "Select" || Request.Form["hdnEmployee"] == null ? Session["EmployeeId"] : Request.Form["hdnEmployee"]);
                    //financialYear = Request.Form["hdnFinYear"] == "" || Request.Form["hdnFinYear"] == null ? finYear : Request.Form["hdnFinYear"];
                    //if (employeeId > 0)
                    //{
                    //    Session["EmployeeId"] = employeeId;
                    //}
                    //Session["FinYear"] = financialYear;
                    //ViewData["ReportName"] = rptName;
                    //SetSalesFunnelReport(rptName);

                    //  employeeId = Convert.ToInt32(Request.Form["hdnEmployee"] == "" || Request.Form["hdnEmployee"] == "Select" || Request.Form["hdnEmployee"] == null ? Session["EmployeeId"] : Request.Form["hdnEmployee"]);

                //response = client.GetAsync("api/Activity/GetLatLogActivityByEmpId?iEmpId=152&Tid=2218DBD6-719E-4BFD-B615-39EAD3682AC8").Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    var lstObjMapLatLogModel = response.Content.ReadAsAsync<IEnumerable<MapLatLogModel>>().Result.ToList().OrderByDescending(x => x.Id);
                //    return  Json(lstObjMapLatLogModel);
                //}

                    return View();
                //}
                //return RedirectToAction("AccessDenied", "Error");
            }
            catch (Exception ex)
            {
                var excepiton = ex;
                return RedirectToAction("Index", "Error");
            }
        }



        [HttpGet]
        public ActionResult ShowMapLatLogReport(long  empId=0, string  forDt="")
        {
            try
            {

                response = client.GetAsync("api/Activity/GetLatLogActivityByEmpId?iEmpId=" + empId + "&FromDt=" + forDt +"&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var lstObjMapLatLogModel = response.Content.ReadAsAsync<IEnumerable<MapLatLogModel>>().Result.ToList().OrderByDescending(x => x.Id);
                    return Json(lstObjMapLatLogModel, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch (Exception ex)
            {
                var excepiton = ex;
                return RedirectToAction("Index", "Error");
            }
        }

        private void SetSalesFunnelReport(string rptName)
        {
            try
            {
                ReportViewer reportViewer = new ReportViewer();
                if (!string.IsNullOrEmpty(rptName))
                {
                    reportViewer.ProcessingMode = ProcessingMode.Local;
                    reportViewer.SizeToReportContent = true;
                    reportViewer.Width = Unit.Percentage(100);
                    reportViewer.Height = Unit.Percentage(100);
                    reportViewer.PageCountMode = PageCountMode.Actual;
                    reportViewer.InteractivityPostBackMode = InteractivityPostBackMode.AlwaysAsynchronous;
                    reportViewer.PageCountMode = PageCountMode.Actual;
                    FillDataSet(rptName);
                    reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reporting\Rpt" + rptName + ".rdlc";

                    reportViewer.LocalReport.DataSources.ToList();

                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", tds.Tables["Tbl" + rptName]));
                  
                    reportViewer.LocalReport.SetParameters(GetParameters(rptName));

                }
                ViewBag.ReportViewer = reportViewer;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
       // var reportDataSource = new ReportDataSource("DataSet1", resultSet);
        private void FillDataSet(string rptName)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlDataAdapter sqlDataAapter = new SqlDataAdapter();
                SqlCommand sqlCommand = new SqlCommand();
                try
                {
                    switch (rptName)
                    {
                        case "SalesFunnel": 
                            sqlCommand = new SqlCommand("sp_SalesFunnel", sqlConnection);
                            sqlCommand.Parameters.Add(new SqlParameter("@TenantId", TenentId));
                            sqlCommand.CommandType = CommandType.StoredProcedure;                            
                            sqlDataAapter.SelectCommand = sqlCommand;
                            tds.TblSalesFunnel.Clear();
                            tds.EnforceConstraints = false;
                            sqlDataAapter.Fill(tds , tds.TblSalesFunnel.TableName);
                            break;
                        case "ActualVsExpected":
                            BindDropDownValues();
                            sqlCommand = new SqlCommand("sp_TargetVsActual", sqlConnection);
                            sqlCommand.Parameters.Add(new SqlParameter("@EmployeeId", employeeId.ToString()));
                            sqlCommand.Parameters.Add(new SqlParameter("@FinYear", financialYear));
                            sqlCommand.Parameters.Add(new SqlParameter("@TenantId", TenentId));
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlDataAapter.SelectCommand = sqlCommand;
                            tds.TblActualVsExpected.Clear();
                            tds.EnforceConstraints = false;
                            var data =  tds.TblActualVsExpected.TableName;
                            sqlDataAapter.Fill(tds, tds.TblActualVsExpected.TableName);
                            break;
                    }                    
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(ex.Message);
                }
                finally
                {
                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
            }
        }

        private string GetConnectionString()
        {
            string nimboelConnectionString = ConfigurationManager.ConnectionStrings["NImboleDBCSRpt"].ConnectionString;
            return nimboelConnectionString;
        }

        private string GetQueryString()
        {
            return "SELECT d.name as Dept, s.Name as Shift, e.BusinessEntityID as EmployeeID"
                + " FROM (HumanResources.Department d INNER JOIN HumanResources.EmployeeDepartmentHistory e ON d.DepartmentID = e.DepartmentID)"
                + " INNER JOIN HumanResources.Shift s ON e.ShiftID = s.ShiftID";
        }

        private ReportParameter[] GetParameters(string rptName)
        {
            switch (rptName)
            {
                case "SalesFunnel":
                    return new ReportParameter[] { };
                case "ActualVsExpected":
                    ReportParameter p1 = new ReportParameter("EmployeeId", employeeId.ToString());
                    ReportParameter p2 = new ReportParameter("FinYear", financialYear);
                    return new ReportParameter[] { p1, p2 };
            }
            return new ReportParameter[] { };
        }

        private void BindDropDownValues()
        {
            //Employees
            /*string EmpRole = Session["EmployeeRole"] != null ? Session["EmployeeRole"].ToString() : "";
            long EmpId = Session["EmployeeId"] != null ? Convert.ToInt64(Session["EmployeeId"]) : 0;
            long EmpRoleId = Session["EmployeeRoleId"] != null ? Convert.ToInt64(Session["EmployeeRoleId"]) : 0;

            if (EmpId == 1)
                response = client.GetAsync("api/Employees/SelectAllEmployeesForReport?EmpId=" + EmpRoleId).Result;
            else
                response = client.GetAsync("api/Employees/SelectAllEmployeesForReport?EmpId=" + EmpId).Result;

            if (response.IsSuccessStatusCode)
            {
                EmpRoleModel _objItem = new EmpRoleModel();
                var objEmployeeModel = response.Content.ReadAsAsync<IEnumerable<EmpRoleModel>>().Result.ToList();
                _objItem.Id = EmpId;
                _objItem.Name = "Me";
                objEmployeeModel.Insert(0, _objItem);
                ViewData["Employees"] = objEmployeeModel;
            }*/

            //FinancialYear
            response = client.GetAsync("api/FinancialYear/GetFinancialYearsForReports?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var lstObjFinancialYearModel = response.Content.ReadAsAsync<IEnumerable<FinancialYearModel>>().Result.ToList().OrderByDescending(x => x.FinancialYear);
                ViewData["FinYears"] = new SelectList(lstObjFinancialYearModel, "FinancialYear", "FinancialYear", new object { });
            }
        }
    }
}