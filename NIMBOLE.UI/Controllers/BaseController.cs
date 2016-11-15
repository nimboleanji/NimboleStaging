using MyResources;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NIMBOLE.UI.Models;


namespace NIMBOLE.UI.Controllers
{
    [ValidateInput(false)]
    public class BaseController : Controller
    {
        Guid TenentId = new Guid();

        protected DTO objNIMBOLEMapper;
        protected HttpClient client;
        protected HttpResponseMessage response;
        protected Uri contactUri = null;
        protected string strAPIURL = string.Empty;

        public string CultureName { get; set; }
        public string CurrentUser { get; set; }
        public long CurrentUserId { get; set; }
        public string EmployeeName { get; set; }
        public long EmployeeId { get; set; }
        public string Role { get; set; }
        public long Id { get; set; }
        public long EmpRoleId { get; set; }
        public string DefaultMilestone { get; set; }
        public List<string> Modules { get; set; }
        public List<string> Roles { get; set; }

        public BaseController()
        {
            try
            {
                strAPIURL = ConfigurationManager.AppSettings["ServiceUrl"].ToString();
                contactUri = new Uri(strAPIURL);
                objNIMBOLEMapper = new DTO();

                client = new HttpClient();
                response = new HttpResponseMessage();
                //client.Timeout = new TimeSpan(100000*5);
                client.BaseAddress = contactUri;
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session.Count > 0)
                {
                    this.CurrentUser = System.Web.HttpContext.Current.Session["User"].ToString();
                    this.CurrentUserId = Convert.ToInt64(System.Web.HttpContext.Current.Session["UserId"]);
                    this.Id = Convert.ToInt64(System.Web.HttpContext.Current.Session["Id"]);

                    TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());

                    response = client.GetAsync("api/Employees/GetLoggedEmployee?id=" + Id + "&email=" + CurrentUser + "&Tid=" + TenentId).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var EmployerSettings = response.Content.ReadAsAsync<NIMBOLE.Models.Models.EmployeeModel>().Result;
                        this.EmployeeName = EmployerSettings.FirstName + " " + EmployerSettings.LastName;
                        this.EmployeeId = EmployerSettings.Id;
                        this.Role = EmployerSettings.RoleCode;
                        this.EmpRoleId = EmployerSettings.EmpRoleId;
                        this.Modules = EmployerSettings.objEmployeeRoleModel.SelectedModules.TrimEnd(',').Split(',').Select(x=>x.ToString()).ToList();

                        Roles = new List<string>(Modules.Count);

                        System.Web.HttpContext.Current.Session["EmployeeRole"] = this.Role;
                        System.Web.HttpContext.Current.Session["EmployeeRoleId"] = this.EmpRoleId;
                        System.Web.HttpContext.Current.Session["EmployeeName"] = this.EmployeeName;
                        System.Web.HttpContext.Current.Session["EmployeeId"] = this.EmployeeId;
                        System.Web.HttpContext.Current.Session["Modules"] = this.Modules;

                        foreach (var item in Modules)
                        {
                            switch (item)
                            { 
                                case "00":
                                    this.Roles.Add("Home");
                                    break;
                                case "01":
                                    this.Roles.Add("Milestone");
                                    this.Roles.Add("Department");
                                    this.Roles.Add("LeadSource");
                                    this.Roles.Add("ContactRoles");
                                    this.Roles.Add("EmployeeRoles");
                                    this.Roles.Add("FinancialYear");
                                    this.Roles.Add("EmployeeTarget");
                                    this.Roles.Add("Products");
                                    this.Roles.Add("OrganisationHierarchy");
                                    this.Roles.Add("EmployeeHyrarchy");
                                    this.Roles.Add("Incentive");
                                    this.Roles.Add("Location");
                                    break;
                                case "02":
                                    this.Roles.Add("NimboleAccounts");
                                    break;
                                case "03":
                                    this.Roles.Add("Contacts");
                                    break;
                                case "04":
                                    this.Roles.Add("Leads");
                                    break;
                                case "05":
                                    this.Roles.Add("Settings");
                                    break;
                                case "06":
                                    this.Roles.Add("Employees");
                                    break;
                                case "07":
                                    this.Roles.Add("Reports");
                                    break;
                            }
                        }

                        switch (EmployerSettings.objSettingModel.LanguageCode)
                        {
                            case "Chinese":
                                this.CultureName = "zh-Hans";
                                break;
                            case "English":
                                this.CultureName = "en-US";
                                break;
                            default:
                                this.CultureName = "en-US";
                                break;
                        }
                        this.ModifyCurrentCulture(this.CultureName);
                        System.Web.HttpContext.Current.Session["DateFormat"] = EmployerSettings.objSettingModel.DateFormat;
                        System.Web.HttpContext.Current.Session["TimeFormat"] = EmployerSettings.objSettingModel.TimeFormat;
                        System.Web.HttpContext.Current.Session["DefaultMilestone"] = EmployerSettings.objSettingModel.DefaultMilestone;
                        string curSymbol = string.Empty;
                        //curSymbol = GetCurrencySymbolByCode(EmployerSettings.objSettingModel.CurrencyCode);
                        curSymbol = EmployerSettings.objSettingModel.CurrencyCode;
                        if (string.IsNullOrEmpty(curSymbol))
                            curSymbol = "₹";
                        System.Web.HttpContext.Current.Session["Currency"] = curSymbol;
                        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(this.CultureName);
                        System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;


                    }

                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }

        }

        private string GetCurrencySymbolByCode(string curCode)
        {
            string symbol = string.Empty;
            switch (curCode)
            {
                /*case "ALL": symbol = "Lek";
                    break;
                case "AFN": symbol = "؋";
                    break;
                case "ARS": symbol = "$";
                    break;
                case "AWG": symbol = "ƒ";
                    break;
                case "AUD": symbol = "$";
                    break;
                case "AZN": symbol = "ман";
                    break;
                case "BSD": symbol = "$";
                    break;
                case "BBD": symbol = "$";
                    break;
                case "BYR": symbol = "p.";
                    break;
                case "BZD": symbol = "BZ$";
                    break;
                case "BMD": symbol = "$";
                    break;
                case "BOB": symbol = "$b";
                    break;
                case "BAM": symbol = "KM";
                    break;
                case "BWP": symbol = "P";
                    break;
                case "BGN": symbol = "лв";
                    break;
                case "BRL": symbol = "R$";
                    break;
                case "BND": symbol = "$";
                    break;
                case "KHR": symbol = "៛";
                    break;
                case "CAD": symbol = "$";
                    break;
                case "KYD": symbol = "$";
                    break;
                case "CLP": symbol = "$";
                    break;
                case "CNY": symbol = "¥";
                    break;
                case "COP": symbol = "$";
                    break;
                case "CRC": symbol = "₡";
                    break;
                case "HRK": symbol = "kn";
                    break;
                case "CUP": symbol = "₱";
                    break;*/
                case "USD": symbol = "$";
                    break;
                case "EUR": symbol = "€";
                    break;
                case "GBP": symbol = "£";
                    break;
                case "INR": symbol = "₹";
                    break;
                case "AUD": symbol = "$";
                    break;
                case "CAD": symbol = "$";
                    break;
                case "SGD": symbol = "$";
                    break;
                case "CHF": symbol = "CHF";
                    break;
                case "MYR": symbol = "RM";
                    break;
                case "JPY": symbol = "¥";
                    break;
                case "CNY": symbol = "¥";
                    break;
            }
            return symbol;
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session.IsNewSession || Session["UserId"] == null)
            {
                //response = client.PostAsync("api/Account/UpdateLoginStatus?Id=" + Convert.ToInt64(Session["UserId"]) + "&status=" + false, null).Result;
                filterContext.Result = new RedirectResult("~/Account/Login?ReturnUrl=" + Request.Url.PathAndQuery.ToString());
            }
            //else
            //{
            //    response = client.PostAsync("api/Account/UpdateLoginStatus?Id=" + Convert.ToInt64(Session["UserId"]) + "&status=" + true, null).Result;
            //}
        }
        public void ModifyCurrentCulture(string _CultureName = "en")
        {
            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(_CultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
    }
}
