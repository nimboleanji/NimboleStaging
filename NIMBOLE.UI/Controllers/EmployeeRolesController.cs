using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using NIMBOLE.Common;
using NIMBOLE.UI.Filters;
using NIMBOLE.UI.Models;

namespace NIMBOLE.UI.Controllers
{
    public class EmployeeRolesController : BaseController
    {
        NIMBOLE.Models.Models.ProductExcelImport excelImport = new NIMBOLE.Models.Models.ProductExcelImport();
        Guid TenentId = new Guid();
        public EmployeeRolesController()
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

        #region GET
        // GET: Roles
        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/EmployeeRoles/AllEmployeeRole?Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var roles = response.Content.ReadAsAsync<IEnumerable<EmployeeRoleModel>>().Result;
                    ViewData["EmployeeRoles"] = roles;
                    return View(roles);
                }
            }
            return RedirectToAction("AccessDenied", "Error");
        }
        public JsonResult EmployeeRole_Read([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/EmployeeRoles/AllEmployeeRole?Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var roles = response.Content.ReadAsAsync<IEnumerable<EmployeeRoleModel>>().Result;
                ViewData["EmployeeRoles"] = roles;
                return Json(roles.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            ViewData["EmployeeRoles"] = null;
            return Json(null);
        }
        [HttpGet]
        public ActionResult GetAllEmployeeRoles()
        {
            response = client.GetAsync("api/EmployeeRoles/GetAllEmployeeRoles?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objEmployeeRoleModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result;
                ViewData["EmpRoles"] = objEmployeeRoleModel;
                return Json(objEmployeeRoleModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpGet]
        public JsonResult GetModulesList()
        {
            response = client.GetAsync("api/EmployeeRoles/GetModulesList?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objEmployeeRoleModel = response.Content.ReadAsAsync<IEnumerable<EmployeeRoleModel>>().Result;
                ViewData["Modules"] = objEmployeeRoleModel;
                return Json(objEmployeeRoleModel, JsonRequestBehavior.AllowGet);
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
        public ActionResult Create(string SelectedModules, string rCode, string rDescription)
        {
            try
            {
                EmployeeRoleModel objEmployeeRoleModel = new EmployeeRoleModel { ERoleCode = rCode, Description = rDescription, SelectedModules = SelectedModules };
                objEmployeeRoleModel.Status = true;
                objEmployeeRoleModel.TenantId = TenentId;

                response = client.PostAsJsonAsync("api/EmployeeRoles/InsertEmployeeRole", objEmployeeRoleModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var employeeRole = response.Content.ReadAsAsync<EmployeeRoleModel>().Result;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Employee role Created Successfully."
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
            }
            catch (Exception ex)
            {
                this.SetAlert(new AlertMessageViewModel()
                {
                    MessageType = MessageType.Error,
                    MessageHeading = "Failure",
                    MessageString = ex.Message
                });
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        [HttpGet]
        [EncryptedActionParameter]
        // HTTP:GET  /Roles/Edit/1      
        public ActionResult Edit(int Id)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/EmployeeRoles/GetById?id=" + Id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objEmployeeRoleModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.EmployeeRoleModel>().Result;
                    return View(objEmployeeRoleModel);
                }
                return View("Record Not Found");
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(string rId, string SelectedModules, string rCode, string rDescription, string rStatus)
        {
            try
            {
                EmployeeRoleModel objEmployeeRoleModel = new EmployeeRoleModel
                {
                    Id = Convert.ToInt32(rId),
                    TenantId=TenentId,
                    ERoleCode = rCode,
                    Description = rDescription,
                    SelectedModules = SelectedModules,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Status = Convert.ToBoolean(rStatus)
                };
                var response = await client.PutAsJsonAsync<EmployeeRoleModel>("api/EmployeeRoles/Edit", objEmployeeRoleModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<EmployeeRoleModel>();
                    ViewData["objResultValue"] = objResultValue;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Employee role Updated Successfully."
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
        #endregion       

        #region DELETE
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, EmployeeRoleModel objEmployeeRoleModel, int Id, bool status)
        {
                try
                {
                    response = await client.DeleteAsync(strAPIURL + "api/EmployeeRoles/DeleteEmployeeRole?id=" + Id + "&status=" + status);
                    if (response.IsSuccessStatusCode)
                    {
                        var objTblAccount = response.Content.ReadAsAsync<NIMBOLE.Models.Models.EmployeeRoleModel>().Result;
                        return Json(new[] { objTblAccount }.ToDataSourceResult(request, ModelState));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message.ToString());
                }
            return RedirectToAction("index");
        }
        #endregion
    }
}



