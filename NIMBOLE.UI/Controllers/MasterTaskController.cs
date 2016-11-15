using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.Models.Models;
using System.Net.Http;
using NIMBOLE.UI.Models;
using NIMBOLE.UI.Filters;//ReadAsAsync 
using System.Linq;

namespace NIMBOLE.UI.Controllers
{
    public class MasterTaskController : BaseController
    {
        Guid TenentId = new Guid();
        public MasterTaskController()
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
        // GET: MasterTask
        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            response = client.GetAsync("api/MasterTask/GetAll?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var taskList = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.EmployeeTaskModel>>().Result;
                return View(taskList);
            }
            return RedirectToAction("AccessDenied", "Error");
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(EmployeeTaskModel objEmployeeTaskModel)
        {
            try
            {
                objEmployeeTaskModel.TenantId = TenentId;
                response = client.PostAsJsonAsync("api/MasterTask/Insert", objEmployeeTaskModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    var masterTask = response.Content.ReadAsAsync<EmployeeTaskModel>().Result;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Task Created Successfully."
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

        #region Edit
        [HttpGet]
        [EncryptedActionParameter]
        public ActionResult Edit(int id)
        {
            try
            {
                response = client.GetAsync("api/MasterTask/GetById?id=" + id + "&Tid= " + TenentId).Result;
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var taskList = response.Content.ReadAsAsync<EmployeeTaskModel>().Result;
                        return View(taskList);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                throw null;
            }
        }
        public async System.Threading.Tasks.Task<ActionResult> Edit(EmployeeTaskModel objEmployeeTaskModel)
        {
            try
            {
                objEmployeeTaskModel.TenantId = TenentId;
                response = await client.PutAsJsonAsync<EmployeeTaskModel>("api/MasterTask/Edit", objEmployeeTaskModel);
                if (response.IsSuccessStatusCode)
                {
                    var taskList = response.Content.ReadAsAsync<EmployeeTaskModel>().Result;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Task Created Successfully."
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
        public async System.Threading.Tasks.Task<ActionResult> Delete([DataSourceRequest] DataSourceRequest request, EmployeeTaskModel objEmployeeTaskModel, int id, bool status)
        {
            try
            {
                response = await client.DeleteAsync("api/MasterTask/Delete?id=" + id + "&status=" + status);
                if (response.IsSuccessStatusCode)
                {
                    var objTask = response.Content.ReadAsAsync<NIMBOLE.Models.Models.EmployeeTaskModel>().Result;
                    return Json(new[] { objEmployeeTaskModel });
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