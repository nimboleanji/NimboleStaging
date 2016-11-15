using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Mvc;
using NIMBOLE.UI.Models;
using NIMBOLE.Models.Models;
using Kendo.Mvc.Extensions;
using System.Web.Script.Serialization;

namespace NIMBOLE.UI.Controllers
{
    public class SchEmpTskActController : BaseController
    {
        Guid TenentId = new Guid();
        #region Constructor
        public SchEmpTskActController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }
        #endregion

        #region Read
        [HttpPost]
        public JsonResult Scheduler_Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["EmployeeId"] != null)
                {
                    string strEmpId = System.Web.HttpContext.Current.Session["EmployeeId"].ToString();
                    response = client.GetAsync("api/Scheduler/GetActivityEmpTaskByEmpId?iEmpId=" + strEmpId +"&Tid="+TenentId).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var lstEmployeeTaskModel = response.Content.ReadAsAsync<IEnumerable<SchedulerServiceModel>>().Result;
                        var result = Json(lstEmployeeTaskModel.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                        return result;

                    }
                }
                return Json("Error: Not Loggin Properly");
            }
            catch (Exception ex)
            {
                return Json("Error2: " + ex.Message);
            }
        }
        #endregion

        #region Edit
        [HttpPost]
        public virtual JsonResult Scheduler_Edit([DataSourceRequest] DataSourceRequest request, SchedulerModel schedulerTask)
        {
            try
            {
                SchedulerServiceModel _objServiceModel = new SchedulerServiceModel();
                _objServiceModel.TenantId = TenentId;
                _objServiceModel.Id = schedulerTask.Id;
                _objServiceModel.Title = schedulerTask.Title;
                _objServiceModel.Start = schedulerTask.Start;
                _objServiceModel.End = schedulerTask.End;
                _objServiceModel.Description = schedulerTask.Description;

                var response = client.PutAsJsonAsync<SchedulerServiceModel>("api/Scheduler/UpdateTask", _objServiceModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = response.Content.ReadAsAsync<SchedulerModel>();
                    return Json(new[] { schedulerTask }.ToDataSourceResult(request, ModelState));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            //return Json(new[] { schedulerTask }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Create
        [HttpPost]
        public ActionResult Scheduler_Create([DataSourceRequest] DataSourceRequest request, SchedulerModel schedulerTask)
        {
            try 
            {
                schedulerTask.TenantId = TenentId;
                string strEmpId = Session["EmployeeId"].ToString();
                SchedulerServiceModel _objServiceModel = new SchedulerServiceModel();
                _objServiceModel.Title = schedulerTask.Title;
                _objServiceModel.Start = schedulerTask.Start;
                _objServiceModel.End = schedulerTask.End;
                _objServiceModel.EmpId = strEmpId;
                _objServiceModel.Description = schedulerTask.Description;
                response = client.PostAsJsonAsync<SchedulerServiceModel>("api/SchEmpTskAct/Insert", _objServiceModel).Result;
                if(response.IsSuccessStatusCode)
                {
                   var resultvalue = response.Content.ReadAsAsync<SchedulerModel>();
                   schedulerTask.Id = resultvalue.Result.Id;
                   return Json(new[] { schedulerTask }.ToDataSourceResult(request, ModelState));
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
              return View();
            }

          
        }

        #endregion

        #region DELETE
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Scheduler_Destroy([DataSourceRequest] DataSourceRequest request, SchedulerModel objSchedulerModel)
        {
            try
            {
                long id= objSchedulerModel.Id;              
                response = await client.DeleteAsync(strAPIURL + "api/SchEmpTskAct/Delete?id=" + id);
                if (response.IsSuccessStatusCode)
                {                   
                        var objScheduler = response.Content.ReadAsAsync<SchedulerModel>().Result;
                        objSchedulerModel.Id = objScheduler.Id;
                        return Json(new[] { objSchedulerModel }.ToDataSourceResult(request, ModelState));                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Index");
        }
        #endregion

    }
}



