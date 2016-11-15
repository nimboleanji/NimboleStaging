using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using System.Threading.Tasks; 

namespace NIMBOLE.UI.Controllers
{
    public class MilestoneStagesController : BaseController
    {
        Guid TenentId = new Guid();

        public MilestoneStagesController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }
        // GET: MilestoneStages
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetMilestoneStageList([DataSourceRequest] DataSourceRequest request)
        {
            response = await client.GetAsync("api/MilestoneStages/GetMilestoneStageList?Tid=" + TenentId);
            if (response.IsSuccessStatusCode)
            {
                
                var objMilestoneStageModel = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.MilestoneStageModel>>().Result.ToList();
              
                ViewData["Modules"] = objMilestoneStageModel;
                return Json(objMilestoneStageModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        #region Post
        [HttpGet]
        public ActionResult Create()
        {
            GetEmployeeRoles();
            return View();
        }
        [HttpPost]
        public ActionResult Create(MilestoneStageModel ObjMilestoneStageModel)
        {
            try
            {
                ObjMilestoneStageModel.TenantId = TenentId;
                string lstroles = string.Empty;
                if (ObjMilestoneStageModel.Roles != null)
                {
                    lstroles = string.Join(",", ObjMilestoneStageModel.Roles);
                    ObjMilestoneStageModel.Roles.Add(lstroles);
                }
                response = client.PostAsJsonAsync("api/MilestoneStages/Insert", ObjMilestoneStageModel).Result;
               
                if (response.IsSuccessStatusCode)
                {
                    var mileStoneStages = response.Content.ReadAsAsync<MilestoneStageModel>().Result;
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Put
        [HttpGet]
        public ActionResult Edit(int id)
        {
            response = client.GetAsync("api/MilestoneStages/GetById?id=" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                MilestoneStageModel objMilestoneStageModel = response.Content.ReadAsAsync<MilestoneStageModel>().Result;

                return View(objMilestoneStageModel);
            }
            return View("Record Not Found");
        }
        //   HTTP:PUT  /MileStoneStage/Edit/
        public async System.Threading.Tasks.Task<ActionResult> Edit(MilestoneStageModel objMilestoneStageModel)
        {

            try
            {
                objMilestoneStageModel.TenantId = TenentId;
                var response = await client.PutAsJsonAsync<MilestoneStageModel>("api/MilestoneStages/Edit", objMilestoneStageModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<MilestoneStageModel>();
                    ViewData["objResultValue"] = objResultValue;
                }
                else
                {
                    // add something here to tell the user hey, something went wrong
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region DELETE
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, MilestoneStageModel objMilestoneStageModel, int id)
        {
            try
            {
                response = await client.DeleteAsync(strAPIURL + "api/MilestoneStages/Delete?id=" + id);
                if (response.IsSuccessStatusCode)
                {
                    var MilestoneStageId = response.Content.ReadAsAsync<MilestoneStageModel>().Result;
                    return Json(new[] { objMilestoneStageModel }.ToDataSourceResult(request, ModelState));

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("index");
        }
        #endregion
        private void GetEmployeeRoles()
        {
            response = client.GetAsync("api/EmployeeRoles/GetAllEmployeeRoles?Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objEmployeeRoleModel = response.Content.ReadAsAsync<IEnumerable<KeyValueRoleModel>>().Result;
                ViewData["EmpRoles"] = new SelectList(objEmployeeRoleModel, "Id", "Description", new object { });
            }
          
        }
    }
}