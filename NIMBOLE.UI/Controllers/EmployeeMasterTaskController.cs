using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.Models.Models;
using NIMBOLE.UI.Filters;
using System.Net.Http;
using Microsoft.AspNet.SignalR;

namespace NIMBOLE.UI.Controllers
{
    public class EmployeeMasterTaskController : BaseController
    {
        Guid TenentId = new Guid();
        public EmployeeMasterTaskController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }
        [HttpGet]
        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        { 
            response = client.GetAsync("api/EmployeeMasterTask/GetAll?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objTblEmployeetask = response.Content.ReadAsAsync<IEnumerable<EmployeeTaskModel>>().Result;
                ViewData["EmployeeCosting"] = objTblEmployeetask;
                return View(objTblEmployeetask);
            }
            return View();
        }
        [HttpPost]
        public ActionResult UpdateRecord(EmployeeTaskModel objEmployeeTaskModel, DateTime std, DateTime end, string[] selectedId, string[] refernsId) 
        {
              string strEmpId = Session["EmployeeId"].ToString();
                objEmployeeTaskModel.EmpId = strEmpId;
                objEmployeeTaskModel.TenantId = TenentId;
                objEmployeeTaskModel.StartDate = std;
                objEmployeeTaskModel.EndDate = end;

                List<EmpTaskKeyValueModel1> lstSelectListItem = new List<EmpTaskKeyValueModel1>();
                List<EmpTaskKeyValueModel1> lstSelectListItemselected = new List<EmpTaskKeyValueModel1>();
                if (refernsId != null)
                {
                    foreach (var item in refernsId)
                    {
                        lstSelectListItem.Add(new EmpTaskKeyValueModel1 { Id = Convert.ToInt64(item), Name = item });
                    }
                }
                if (selectedId != null)
                {
                    foreach (var item in selectedId)
                    {
                        lstSelectListItemselected.Add(new EmpTaskKeyValueModel1 { Id = Convert.ToInt64(item), Name = item });
                    }
                }
                objEmployeeTaskModel.ReferenceIds = lstSelectListItem;
                objEmployeeTaskModel.Status = true;
                objEmployeeTaskModel.SelectedValue = lstSelectListItemselected;
                objEmployeeTaskModel = InsertEmpTaskModel(objEmployeeTaskModel);
                return RedirectToAction("Index");
        }
        private EmployeeTaskModel InsertEmpTaskModel(EmployeeTaskModel objEmployeeTaskModel)
        {
            objEmployeeTaskModel.TenantId = TenentId;
            response = client.PostAsJsonAsync("api/EmployeeMasterTask/Insert", objEmployeeTaskModel).Result;
            if (response.IsSuccessStatusCode)
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.NimboleHub>();
                HomeController _objHomeController = new HomeController();
                hubContext.Clients.All.update(_objHomeController.IndexForHub());
                objEmployeeTaskModel = response.Content.ReadAsAsync<EmployeeTaskModel>().Result;
            }
            return objEmployeeTaskModel;
        }
    }
}