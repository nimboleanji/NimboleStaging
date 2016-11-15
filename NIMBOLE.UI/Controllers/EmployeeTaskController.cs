using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.Models.Models;
using Kendo.Mvc.Extensions;
using System.Net.Http;
using Kendo.Mvc.UI;
using NIMBOLE.UI.Models;
using Microsoft.AspNet.SignalR;
using NIMBOLE.UI.Filters;

namespace NIMBOLE.UI.Controllers
{
    public class EmployeeTaskController : BaseController
    {
        Guid TenentId = new Guid();
        public EmployeeTaskController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }
        public JsonResult EmpTask_Read([DataSourceRequest] DataSourceRequest request)
        {
            if (Session["EmployeeName"].ToString().Trim() == "Admin")
                response = client.GetAsync("api/EmployeeTask/GetAll?Tid=" + TenentId).Result;
            else
            {
                var empId = Convert.ToInt32(Session["EmployeeId"]);
                response = client.GetAsync("api/EmployeeTask/GetAll?Tid=" + TenentId + "&id=" + empId).Result;
            }
            if (response.IsSuccessStatusCode)
            {
                var EmployeeTaskList = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.EmployeeTaskModel>>().Result;
                ViewData["EmpTaskDetails"] = EmployeeTaskList;
                return Json(EmployeeTaskList.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            //bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            //if (_objAuthorized)
            //{
                GetDropdownValues("Index");
                response = client.GetAsync("api/EmployeeTask/GetAll?Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsAsync<IEnumerable<EmployeeTaskModel>>().Result;

                    return View(result);
                }

            //}
            //return RedirectToAction("AccessDenied", "Error");
                return View();

        }

        private void GetDropdownValues(string type)
        {
            long EmpId = Session["EmployeeId"] != null ? Convert.ToInt64(Session["EmployeeId"]) : 0;

            response = client.GetAsync("api/Employees/SelectAllEmployeesForReport?EmpId=" + EmpId + "&Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                EmpRoleModel _objItem = new EmpRoleModel();
                var objEmployeeModel = response.Content.ReadAsAsync<IEnumerable<EmpRoleModel>>().Result.ToList();
                _objItem.Id = EmpId;
                _objItem.Name = "Me";
                _objItem.FirstName = "Me";
                _objItem.LastName = "Me";
                if (type == "Index")
                {
                    objEmployeeModel.Insert(0, _objItem);// .Add(_objItem);
                    ViewData["Employees"] = new SelectList(objEmployeeModel, "Id", "Name");
                }
                else
                    ViewData["Employees"] = new SelectList(objEmployeeModel, "Id", "FirstName");
            }
        }

        #region Create
        public ActionResult Create()
        {
            GetDropdownValues("Create");
            return View();
        }
        [HttpPost]
        public ActionResult Create(EmployeeTaskModel objEmployeeTaskModel)
        {
            try
            {
                string strEmpId = Session["EmployeeId"].ToString();
                objEmployeeTaskModel.EmpId = strEmpId;
                objEmployeeTaskModel.TenantId = TenentId;
                List<EmpTaskKeyValueModel1> lstSelectListItem = new List<EmpTaskKeyValueModel1>();
                if (Request["ReferenceIds"] != null)
                {
                    var strRefIds = Request["ReferenceIds"].ToString().Split(',');
                    foreach (var item in strRefIds)
                    {
                        lstSelectListItem.Add(new EmpTaskKeyValueModel1 { Id = Convert.ToInt64(item), Name = item });
                    }
                }
                objEmployeeTaskModel.ReferenceIds = lstSelectListItem;
                objEmployeeTaskModel.Status = true;

                objEmployeeTaskModel = InsertEmpTaskModel(objEmployeeTaskModel);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private EmployeeTaskModel InsertEmpTaskModel(EmployeeTaskModel objEmployeeTaskModel)
        {
            objEmployeeTaskModel.TenantId = TenentId;
            response = client.PostAsJsonAsync("api/EmployeeTask/Insert", objEmployeeTaskModel).Result;
            if (response.IsSuccessStatusCode)
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.NimboleHub>();
                HomeController _objHomeController = new HomeController();
                hubContext.Clients.All.update(_objHomeController.IndexForHub());
                objEmployeeTaskModel = response.Content.ReadAsAsync<EmployeeTaskModel>().Result;
            }
            return objEmployeeTaskModel;
        }

        [HttpPost]
        public virtual JsonResult EmployeeTask_Create([DataSourceRequest] DataSourceRequest request, object ContestIdGridRead, SchedulerModel sm, List<EmpTaskKeyValueModel1> lstReferenceIds)
        {
            if (sm.Id > 0)
            {
                lstReferenceIds = new List<EmpTaskKeyValueModel1> { 
             new EmpTaskKeyValueModel1{Id=1,Name="AAA"}
            };
                EmployeeTaskModel objEmployeeTaskModel = InsertEmpTaskModel(new EmployeeTaskModel
                {
                    Title = sm.Title,
                    IsActivity = false,
                    EmpId = Session["EmployeeId"].ToString(),
                    ReferenceIds = lstReferenceIds,
                    TaskDate = sm.Start,
                    StartDate = sm.Start,
                    EndDate = sm.End,
                    Comments = sm.Description
                });
                return Json(new[] { sm }.ToDataSourceResult(request, ModelState));
            }
            else
                return null;
        }
        #endregion

        [HttpGet]
        [EncryptedActionParameter]
        public ActionResult ViewTask(int Id)
        {
            return Edit(Id);
        }

        #region Edit
        [HttpGet]
        [EncryptedActionParameter]
        // HTTP:GET  /EmployeeTask/Edit/1
        public ActionResult Edit(int Id)
        {
            GetDropdownValues("Edit");
            response = client.GetAsync("api/EmployeeTask/GetById?Id=" + Id + "&Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                EmployeeTaskModel objEmployeeTaskModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.EmployeeTaskModel>().Result;
                return View(objEmployeeTaskModel);
            }
            return View("Record not found");
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(EmployeeTaskModel objEmployeeTaskModel)
        {
            try
            {
                List<EmpTaskKeyValueModel1> lstSelectListItem = new List<EmpTaskKeyValueModel1>();
                if (Request["ReferenceIds"] != null)
                {
                    var strRefIds = Request["ReferenceIds"].ToString().Split(',');
                    foreach (var item in strRefIds)
                    {
                        lstSelectListItem.Add(new EmpTaskKeyValueModel1 { Id = Convert.ToInt64(item), Name = item });
                    }
                }
                objEmployeeTaskModel.ReferenceIds = lstSelectListItem;
                objEmployeeTaskModel.TenantId = TenentId;
                var response = await client.PutAsJsonAsync<EmployeeTaskModel>("api/EmployeeTask/Edit", objEmployeeTaskModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<EmployeeTaskModel>();
                    ViewData["objResultValue"] = objResultValue;
                    return RedirectToAction("Index");
                }
                else
                {
                    // add something here to tell the user hey, something went wrong
                    return null;
                }
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public virtual JsonResult Edit_Scheduler([DataSourceRequest] DataSourceRequest request, SchedulerModel objSchedulerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = client.PutAsJsonAsync<SchedulerModel>("api/EmployeeTask/Edit_Scheduler", objSchedulerModel).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var objResultValue = response.Content.ReadAsAsync<SchedulerModel>();
                        ViewData["objResultValue"] = objResultValue;
                        return Json(new[] { objSchedulerModel }.ToDataSourceResult(request, ModelState));
                    }
                    else
                    {
                        // add something here to tell the user hey, something went wrong
                        return null;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region DELETE
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, EmployeeTaskModel objEmployeeTaskModel, int id, bool status)
        {
            try
            {
                response = await client.DeleteAsync(strAPIURL + "api/EmployeeTask/Delete?Id=" + id + "&status=" + status);
                if (response.IsSuccessStatusCode)
                {
                    var objEmployeeTask = response.Content.ReadAsAsync<NIMBOLE.Models.Models.EmployeeTaskModel>().Result;
                    //return View(objEmployeeTask);
                    return Json(new[] { objEmployeeTaskModel }.ToDataSourceResult(request, ModelState));
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}