using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using NIMBOLE.Models.Models;
using NIMBOLE.Models;
using System.Configuration;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Common;
using NIMBOLE.UI.Models;
using NIMBOLE.UI.Filters;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Web.UI.WebControls;
using NIMBOLE.UI.Helpers;


namespace NIMBOLE.UI.Controllers
{
    public class EmployeesController : BaseController
    {
        Guid TenentId = new Guid();
        public EmployeesController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }

        #region GET

        [HttpGet]
        public ActionResult GetAllEmployees()
        {
            response = client.GetAsync("api/Employees/SelectAllEmployees?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objEmployeeModel = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.EmployeeModel>>().Result;
                return Json(objEmployeeModel, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        [HttpGet]
        public ActionResult GetAllReportsEmployees(int EmpRoleId)
        {
            response = client.GetAsync("api/Employees/GetAllReportsToEmployees?EmpRoleId=" + EmpRoleId + "&Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                if (response.IsSuccessStatusCode)
                {
                    var objEmployeeModel = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.KeyValueModel>>().Result;
                    if (objEmployeeModel.Count() > 0)
                        return Json(objEmployeeModel, JsonRequestBehavior.AllowGet);
                    else
                        return Json(Content(""));
                }
                else
                    return Json(Content(""));
            }
            else
                return Json(Content(""));
        }
        [HttpGet]
        public ActionResult GetLoggedinEmployee()
        {
            response = client.GetAsync("api/Employees/GetLoggedEmployee?id=" + CurrentUserId + "&email=" + CurrentUser + "&Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var loggedEmployee = response.Content.ReadAsAsync<NIMBOLE.Models.Models.EmployeeModel>().Result;
                return Json(loggedEmployee, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        [HttpGet]
        public JsonResult GetAllEmployeeRoles()
        {
            response = client.GetAsync("api/EmployeeRoles/GetAllEmployeeRoles?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objEmployeeRoleModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result;
                //ViewData["EmpRoles"] = new SelectList(objEmployeeRoleModel, "Id", "Description", new object { });
                return Json(objEmployeeRoleModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpGet]
        public JsonResult GetAllLocation()
        {
            response = client.GetAsync("api/Employees/GetAllLocation?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objGetAllLocation = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result;
                //ViewData["EmpRoles"] = new SelectList(objEmployeeRoleModel, "Id", "Description", new object { });
                return Json(objGetAllLocation, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        //       [HttpGet]
        //public ActionResult GetAllStates()
        //   {
        //       response = client.GetAsync("api/Employees/GetAllStates").Result;
        //       if (response.IsSuccessStatusCode)
        //       {
        //           var objStates = response.Content.ReadAsAsync<IEnumerable<StateModel>>().Result;
        //           return Json(objStates, JsonRequestBehavior.AllowGet);
        //       }
        //       return null;
        //   }

        [HttpGet]
        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/Employees/GetAll?Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objTblEmployee = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.EmployeeModel>>().Result;
                    ViewData["EmployeeDetails"] = objTblEmployee;
                    return View(objTblEmployee);
                }
            }
            return RedirectToAction("AccessDenied", "Error");
        }
        public JsonResult GetAllEmployeeWithFilter([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/Employees/SelectAllEmployees?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objtblemployees = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.EmployeeModel>>().Result.ToList();
                ViewData["EmployeeDetails"] = objtblemployees;
                return Json(objtblemployees, JsonRequestBehavior.AllowGet);
            }
            ViewData["EmployeeDetails"] = null;
            return Json(null);
        }
        public async Task<JsonResult> Employee_Read([DataSourceRequest] DataSourceRequest request)
        {
            response = await client.GetAsync("api/Employees/GetAll?Tid=" + TenentId);
            if (response.IsSuccessStatusCode)
            {
                var objTblEmployee = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.EmployeeModel>>().Result;
                ViewData["EmployeeDetails"] = objTblEmployee;
                return Json(objTblEmployee.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        #endregion

        #region Create
        // GET: Employee/Create
        public ActionResult Create()
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                GetAllDropdownValues(0);
                return View();
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(EmployeeModel objEmployeeModel, HttpPostedFileBase uploadFile)
        {
            try
            {
                #region New
                if (!string.IsNullOrEmpty(Request["hdnDesignation"]))
                {
                    objEmployeeModel.EmpRoleId = Convert.ToInt64(Request["hdnDesignation"]);
                }
                if (!string.IsNullOrEmpty(Request["hdnReportsToEmployee"]))
                {
                    objEmployeeModel.ReportingTo = Convert.ToInt64(Request["hdnReportsToEmployee"]);
                }
                #endregion
                objEmployeeModel.TenantId = TenentId;
                response = client.PostAsJsonAsync("api/Employees/Insert", objEmployeeModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var employees = response.Content.ReadAsAsync<EmployeeModel>().Result;

                    var empid = employees.Id;
                    int id = Convert.ToInt32(employees.Id);

                    objEmployeeModel.Id = id;

                    if (uploadFile != null && uploadFile.ContentLength > 0)
                    {
                        PhotoCloudStorage pcs;
                        pcs = new PhotoCloudStorage();
                        //Guid tenantId = objEmployeeModel.TenantId.ToDefaultTenantId();
                        objEmployeeModel.Id = id;
                        objEmployeeModel.EmployeeImageURL = pcs.StoreInBlob(uploadFile, TenentId, id, "E");
                        objEmployeeModel.TenantId = TenentId;
                    }

                    var response1 = client.PostAsJsonAsync("api/Employees/InsertImage", objEmployeeModel).Result;
                    if (response1.IsSuccessStatusCode)
                    {
                        objEmployeeModel = response1.Content.ReadAsAsync<EmployeeModel>().Result;
                    }


                    string emailB = string.Empty;
                    string decPassword = employees.Password;

                    emailB = "Dear User," + "<br/>";
                    emailB = emailB + "Mr./ Ms. " + objEmployeeModel.FirstName + " " + objEmployeeModel.LastName + "<br/>";
                    emailB = emailB + "Your User Account is Created Successfully. <br/>";
                    emailB = emailB + "Site URL : http://nproduction.azurewebsites.net/ <br/>";
                    emailB = emailB + "Your User Name is : " + objEmployeeModel.EmployeeEmail + " <br/>";
                    emailB = emailB + "Your Password is : " + decPassword + " <br/>";
                    emailB = emailB + "<br/> <br/>  Regards, <br/> Nimbole Technologies <br/>";

                    EmailSender.SendEmailToNewUser("Employee Created Successfully", emailB, objEmployeeModel.EmployeeEmail);

                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Employee Created Successfully."
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

        #region Put
        // GET: Employees/Edit/5
        [EncryptedActionParameter]
        public ActionResult Edit(int id)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                EmployeeModel _objEmployeeModel = new EmployeeModel();
                long empid = 0;
                response = client.GetAsync("api/Employees/GetByIdAndAddressDetails?id=" + id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    string errVal = response.Content.ReadAsStringAsync().Result;
                    if (!errVal.Contains("Failure"))
                    {
                        _objEmployeeModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.EmployeeModel>().Result;
                        empid = _objEmployeeModel.EmpRoleId;
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
                return View(_objEmployeeModel);
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Edit(EmployeeModel objEmployeeModel, HttpPostedFileBase uploadFile)
        {
            objEmployeeModel.TenantId = TenentId;
            if (!string.IsNullOrEmpty(Request["hdnDesignation"]))
            {
                objEmployeeModel.EmpRoleId = Convert.ToInt64(Request["hdnDesignation"]);
            }

            int id = Convert.ToInt32(objEmployeeModel.Id);
            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                PhotoCloudStorage pcs;
                pcs = new PhotoCloudStorage();
                //Guid tenantId = objEmployeeModel.TenantId.ToDefaultTenantId();
                objEmployeeModel.EmployeeImageURL = pcs.StoreInBlob(uploadFile, TenentId, id, "E");
                objEmployeeModel.TenantId = TenentId;
            }

            var response = await client.PutAsJsonAsync<EmployeeModel>("api/Employees/Edit", objEmployeeModel);
            if (response.IsSuccessStatusCode)
            {
                var objResultValue = await response.Content.ReadAsAsync<string>();
                ViewData["objResultValue"] = objResultValue;
                this.SetAlert(new AlertMessageViewModel()
                {
                    MessageType = MessageType.Success,
                    MessageHeading = "Success",
                    MessageString = "Employee Updated Successfully"
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
        #endregion

        #region DELETE
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, EmployeeModel objEmployeeModel, string[] selectedId, bool status)
        {
            string strselectedId = string.Join(",", selectedId);
            response = await client.DeleteAsync(strAPIURL + "api/Employees/Delete?selectedId=" + strselectedId + "&status=" + status);
            if (response.IsSuccessStatusCode)
            {
                //  var objEmployeeModels = response.Content.ReadAsAsync<EmployeeModel>().Result;
                return Json(new[] { objEmployeeModel }.ToDataSourceResult(request, ModelState));
            }
            else
            {
                return Json(response.ReasonPhrase);
            }
        }

        //DeleteRec

        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> DeleteRec([DataSourceRequest] DataSourceRequest request, string[] selectedId)
        {
            string strselectedId = string.Join(",", selectedId);
            response = await client.DeleteAsync(strAPIURL + "api/Employees/DeleteRec?selectedId=" + strselectedId);
            if (response.IsSuccessStatusCode)
            {
                var objEmployeeModels = response.Content.ReadAsAsync<EmployeeModel>().Result;
                return Json(new[] { objEmployeeModels }.ToDataSourceResult(request));
            }
            else
            {
                return Json(response.ReasonPhrase);
            }
        }


        #endregion

        #region Employeetarget
        public ActionResult GetAllEmployeeTargets([DataSourceRequest] DataSourceRequest request ,int empid, int roleId)
        {
            response = client.GetAsync("api/Employees/GetAllEmployeeTargets?Tid=" + TenentId +"&EmpId=" + empid.ToString() + "&RoleId=" + roleId.ToString()).Result;
            if (response.IsSuccessStatusCode)
            {
                var lstEmployeeTargetModel = response.Content.ReadAsAsync<EmployeeTargetModel>().Result;
                return Json(new[] { lstEmployeeTargetModel }.ToDataSourceResult(request));
            }
            return null;
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> EditTarget([DataSourceRequest] DataSourceRequest request, EmployeeTargetModel objEmployeeTargetModel, string EmployeeType, int empid)
        {
            try
            {
                objEmployeeTargetModel.Type = EmployeeType;
                objEmployeeTargetModel.TenantId = TenentId;
                objEmployeeTargetModel.EmployeeId = empid;
                var response = await client.PutAsJsonAsync<EmployeeTargetModel>("api/Employees/EditTarget", objEmployeeTargetModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<EmployeeTargetModel>();
                    return Json(new[] { objResultValue }.ToDataSourceResult(request));
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
        private void GetAllDropdownValues(long empid)
        {
            //EmploRoles
            response = client.GetAsync("api/Employees/GetAllLocation?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objEmployeeRoleModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result;
                ViewData["EmpLocation"] = new SelectList(objEmployeeRoleModel, "Id", "Name", new object { });
            }
            //Location
            response = client.GetAsync("api/OrgHierarchy/GetAllOrgHierarchy?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                // var objOrgHierarchyModel = response.Content.ReadAsAsync<IEnumerable<OrgHierarchyModel>>().Result;
                var objOrgHierarchyModel = response.Content.ReadAsAsync<IEnumerable<EmployeeRoleModel>>().Result;
                ViewData["Location"] = new SelectList(objOrgHierarchyModel, "Id", "Description", new object { });
            }
            //Reports TO 
            response = client.GetAsync("api/Employees/GetAllReportsToEmployees?EmpRoleId=" + empid +"&Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objEmployeeModel = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.KeyValueModel>>().Result;
                ViewData["ReportsToEmployee"] = new SelectList(objEmployeeModel, "Id", "Name", new object { });
            }
            //Countries
            response = client.GetAsync("api/AddressAutoComplete/Countries?Tid="+ TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var Countries = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.CountryModel>>().Result.ToList().OrderBy(x => x.CountryName);
                ViewData["Countries"] = new SelectList(Countries, "CountryId", "CountryName", new object { });
            }
        }

    }
}
