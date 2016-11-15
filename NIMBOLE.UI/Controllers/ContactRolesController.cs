using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.Models.Models;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using AutoMapper;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Web.UI.WebControls;
using NIMBOLE.Common;
using NIMBOLE.UI.Filters;
using NIMBOLE.UI.Models;
using System.Threading.Tasks;

namespace NIMBOLE.UI.Controllers
{
    public class ContactRolesController : BaseController
    {

        Guid TenentId = new Guid();
        public ContactRolesController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());          
        }

        #region GET
        //
        // GET: /ContactRoles/
            [HttpGet]
        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/ContactRoles/GetAll?Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var roles = response.Content.ReadAsAsync<IEnumerable<ContactRoleModel>>().Result;
                    ViewData["ContactRoles"] = roles;
                    return View(roles);
                }
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        //public JsonResult ContactRole_Read([DataSourceRequest] DataSourceRequest request)
        //{
        //    response = client.GetAsync("api/ContactRoles/GetAll?Tid="+TenentId).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var roles = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.ContactRoleModel>>().Result;
        //        ViewData["ContactRoles"] = roles;
        //        return Json(roles.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //    }
        //    ViewData["ContactRoles"] = null;
        //    return Json(null);
        //}

        public JsonResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/ContactRoles/GetAll?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var roles = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.ContactRoleModel>>().Result;
                return Json(roles, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }
        [HttpGet]
        public async Task<ActionResult> AllContactRole()
        {
            response = await client.GetAsync("api/ContactRoles/AllContactRole?Tid=" + TenentId);
            if (response.IsSuccessStatusCode)
            {
                var objContactRoleModel = response.Content.ReadAsAsync<IEnumerable<ContactRoleModel>>().Result;
                return Json(objContactRoleModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpGet]
        public ActionResult AllContactRoleForCombo()
        {
            //response = client.GetAsync("api/ContactRoles/SelectContactRolesForDropdown?Tid=" + TenentId).Result;
            response = client.GetAsync("api/ContactRoles/SelectContactRolesForCombobox?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objContactRoleModel = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result.ToList();
                return Json(objContactRoleModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        #endregion

        #region CREATE
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
        public ActionResult Create(ContactRoleModel objContactRoleModel)
        {
            try
            {
                objContactRoleModel.TenantId = TenentId;
                objContactRoleModel.Status = true;
                response = client.PostAsJsonAsync("api/ContactRoles/Insert", objContactRoleModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var contactRoles = response.Content.ReadAsAsync<NIMBOLE.Models.Models.ContactRoleModel>().Result;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Contact role Created Successfully."
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

        [HttpPost]
        public ActionResult CreateLeadContactRole([DataSourceRequest] DataSourceRequest request, ContactRoleModel objContactRoleModel, string item)
        {
            try
            {
                objContactRoleModel.TenantId = TenentId;
                objContactRoleModel.Description = item;
                objContactRoleModel.Status = true;

                response = client.PostAsJsonAsync("api/ContactRoles/InsertLeadContact", objContactRoleModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var contactRoles = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result;
                    ViewData["Designations"] = contactRoles;
                    return Json(contactRoles.ToDataSourceResult(request));
                }
                else
                {
                    return Json(new[] { objContactRoleModel }.ToDataSourceResult(request, ModelState));
                }
            }
            catch
            {
                return Content("");
            }
        }

        [HttpPost]
        public ActionResult CreateContactRole([DataSourceRequest] DataSourceRequest request, ContactRoleModel objContactRoleModel, string item)
        {
            try
            {
                objContactRoleModel.TenantId = TenentId;
                objContactRoleModel.Description = item;
                objContactRoleModel.Status = true;
                response = client.PostAsJsonAsync("api/ContactRoles/InsertContactRole", objContactRoleModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var contactRoles = response.Content.ReadAsAsync<IEnumerable<KeyValueModel>>().Result;
                    ViewData["Designations"] = contactRoles;
                    return Json(contactRoles.ToDataSourceResult(request));
                }
                else
                {
                    return Json(new[] { objContactRoleModel }.ToDataSourceResult(request, ModelState));
                }
            }
            catch (Exception ex)
            {
                return Content("");
            }
        }
          #endregion

        #region PUT
        [HttpGet]
        [EncryptedActionParameter]
        // HTTP:GET  /ContactRoles/Edit/1
        public ActionResult Edit(int Id)
        {
            bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
            if (_objAuthorized)
            {
                response = client.GetAsync("api/ContactRoles/GetById?id=" + Id + "&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    ContactRoleModel objContactRoleModel = response.Content.ReadAsAsync<NIMBOLE.Models.Models.ContactRoleModel>().Result;
                    return View(objContactRoleModel);
                }
                return View("Record Not Found");
            }
            return RedirectToAction("AccessDenied", "Error");
        }

        public async System.Threading.Tasks.Task<ActionResult> Edit(ContactRoleModel objContactRoleModel)
        {
            try
            {
                var response = await client.PutAsJsonAsync<ContactRoleModel>("api/ContactRoles/Edit", objContactRoleModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<ContactRoleModel>();
                    ViewData["objResultValue"] = objResultValue;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Contact role Updated Successfully."
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
            catch(Exception ex)
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
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, ContactRoleModel objContactRoleModel, int id, bool status)
        {
                try
                {
                    response = await client.DeleteAsync(strAPIURL + "api/ContactRoles/Delete?id=" + id + "&status=" + status);
                    if (response.IsSuccessStatusCode)
                    {
                        var ContactRolesId = response.Content.ReadAsAsync<NIMBOLE.Models.Models.ContactRoleModel>().Result;
                        return Json(new[] { objContactRoleModel }.ToDataSourceResult(request, ModelState));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            return RedirectToAction("index");
        }
        #endregion
    }
}