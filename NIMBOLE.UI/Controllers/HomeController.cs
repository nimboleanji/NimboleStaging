using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

using NIMBOLE.Models.Models;

namespace NIMBOLE.UI.Controllers
{
    public class HomeController : BaseController
    {
        Guid TenentId = new Guid();


        public HomeController()
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
        public ActionResult Index()
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                if (System.Web.HttpContext.Current.Session["EmployeeId"] != null)
                {
                    string strEmpId = System.Web.HttpContext.Current.Session["EmployeeId"].ToString();
                    response = client.GetAsync("api/Dashboard/GetAllCountsByEmpId?iEmpId=" + strEmpId + "&Tid="+TenentId).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var dicCounts = response.Content.ReadAsAsync<List<string>>().Result;
                        ViewBag.NoOfLeads = dicCounts[0];
                        ViewBag.NoOfTasks = dicCounts[2]; 
                        ViewBag.NoOfOpportunities = dicCounts[1];
                    }
                }
                return View();
            }
            return View();
        }

        public ActionResult GetLeadChartData()
        {
            response = client.GetAsync("api/Dashboard/GetLeadCountsForWeek").Result;
            if (response.IsSuccessStatusCode)
            {
                var dicCounts = response.Content.ReadAsAsync <IEnumerable<DashboardModel>>().Result;
                return Json(dicCounts);
            }
            return View();
        }

        public ActionResult GetOppDealChartData(string Type)
        {
            response = client.GetAsync("api/Dashboard/GetOppDealCountsForWeek?Type=" + Type).Result;
            if (response.IsSuccessStatusCode)
            {
                var dicCounts = response.Content.ReadAsAsync<IEnumerable<DashboardModel>>().Result;
                return Json(dicCounts);
            }
            return View();
        }

        public List<string> IndexForHub()
        {
            if (System.Web.HttpContext.Current.Session["EmployeeId"] != null)
            {
                string strEmpId = System.Web.HttpContext.Current.Session["EmployeeId"].ToString();
                response = client.GetAsync("api/Dashboard/GetAllCountsByEmpId?iEmpId=" + strEmpId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var dicCounts = response.Content.ReadAsAsync<List<string>>().Result;
                    return dicCounts;
                }
            }
            return null;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        #region Menu

        [ChildActionOnly]
        public ActionResult Menu()
        {
            var _menu = new MenuModel();
            return PartialView("_Menu", _menu);
        }
        #endregion
    }
}
