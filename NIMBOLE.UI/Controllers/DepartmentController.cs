using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;
using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using AutoMapper;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.Configuration;
using NIMBOLE.Common;
using NIMBOLE.UI.Filters;
using NIMBOLE.UI.Models;

namespace NIMBOLE.UI.Controllers
{
    public class DepartmentController : BaseController
    {
        Guid TenentId = new Guid();
        public DepartmentController()
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                    TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
                //TenentId = TenentId.ToDefaultTenantId();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }

        //public JsonResult Department_Read([DataSourceRequest] DataSourceRequest request)
        //{
        //    response = client.GetAsync("api/Departments/GetAll?Tid=" + TenentId).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var departmentList = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.DepartmentModel>>().Result;
        //        ViewData["Departments"] = departmentList;
        //        return Json(departmentList.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(null);
        //}
        [HttpGet]
        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Departments/SelectAllDepartments?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var departmentList = response.Content.ReadAsAsync<IEnumerable<DepartmentModel>>().Result;
                ViewData["Departments"] = departmentList;
                return View(departmentList);
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        #region GET
        [HttpGet]
        public ActionResult GetAllDepartments()
        {
            response = client.GetAsync("api/Departments/SelectAllDepartments?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var departmentList = response.Content.ReadAsAsync<IEnumerable<DepartmentModel>>().Result;
                ViewData["Departments"] = departmentList;
                return View(departmentList);
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        [HttpGet]
        public ActionResult GetAllDepartmentsForCombo()
        {
            //response = client.GetAsync("api/Departments/SelectDepartmentsForDropdown?Tid=" + TenentId).Result;
            response = client.GetAsync("api/Departments/SelectDepartmentsForCombobox?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objDepartmentModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                return Json(objDepartmentModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        #endregion

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                return View();
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        [HttpPost]
        public ActionResult Create(DepartmentModel objDepartmentModel)
        {
            try
            {
                objDepartmentModel.TenantId = TenentId;
                response = client.PostAsJsonAsync("api/Departments/Insert", objDepartmentModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    var departmens = response.Content.ReadAsAsync<NIMBOLE.Models.Models.DepartmentModel>().Result;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Department Created Successfully."
                    });
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
                return View();
            }
        }

        //CreateDepartment

        [HttpPost]
        public ActionResult CreateDepartment([DataSourceRequest] DataSourceRequest request, DepartmentModel objDepartmentModel, string item)
        {
            try
            {
                objDepartmentModel.TenantId = TenentId;
                objDepartmentModel.DepartmentName = item;
                objDepartmentModel.Status = true;

                response = client.PostAsJsonAsync("api/Departments/InsertDepartment", objDepartmentModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var department = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.KeyValueModel>>().Result;
                    ViewData["Departments"] = department;
                    return Json(department.ToDataSourceResult(request));
                }
                else
                {
                    return Json(new[] { objDepartmentModel }.ToDataSourceResult(request, ModelState));
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
        // HTTP:GET  /Department/Edit/1
        public ActionResult Edit(int Id)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/Departments/GetById?id=" + Id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    DepartmentModel ObjDepartmentModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.DepartmentModel>().Result;
                    return View(ObjDepartmentModel);
                }
                return View("Record Not Found");
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        // HTTP:PUT  /Departments/Edit/
        public async System.Threading.Tasks.Task<ActionResult> Edit(DepartmentModel ObjDepartmentModel)
        {
            try
            {
                ObjDepartmentModel.TenantId = TenentId;
                var response = await client.PutAsJsonAsync<DepartmentModel>("api/Departments/Edit", ObjDepartmentModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<DepartmentModel>();
                    ViewData["objResultValue"] = objResultValue;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Department Updated Successfully."
                    });
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
                return View();
            }
        }
        #endregion

        #region DELETE
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, DepartmentModel objDepartmentModel, int id, bool status)
        {
            try
            {
                response = await client.DeleteAsync(strAPIURL + "api/Departments/Delete?DeptId=" + id + "&status=" + status);
                if (response.IsSuccessStatusCode)
                {
                    var objDepartment = response.Content.ReadAsAsync<NIMBOLE.Models.Models.DepartmentModel>().Result;
                    return Json(new[] { objDepartmentModel }.ToDataSourceResult(request, ModelState));
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