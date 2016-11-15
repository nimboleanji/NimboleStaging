using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.Models.Models;
using System.Web.Security;
using System.Configuration;
using NIMBOLE.Models;
using AutoMapper;
using System.Net.Http;
using System.Net.Http.Headers;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Diagnostics;
using NIMBOLE.Models.Mappers;
using System.Globalization;
using NIMBOLE.UI.Models;

namespace NIMBOLE.UI.Controllers
{
    public class SettingsController : BaseController
    {
        DTO objNIMBOLEMapper = new DTO();
        Guid TenentId = new Guid();

        public SettingsController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }

        [HttpGet]
        public ActionResult MySettings()
        {
            try
            {
                bool _objAuthorized = NimboleCommon.IsAuthorized(Request.RequestContext.RouteData.Values["controller"].ToString(), Roles);
                if (_objAuthorized)
                {
                    //SelectAllCultures();

                   response = client.GetAsync("api/Settings/GetIdBasedonTid?Tid=" + TenentId).Result;
                    long clntId=0;
                    if (response.IsSuccessStatusCode)
                    {
                        var objClntModel = response.Content.ReadAsAsync<SettingModel>().Result;
                        clntId = objClntModel.Id;
                    }
                    
                    Session["CurrentClientId"] = clntId;
                    response = client.GetAsync("api/Settings/GetById?id=" + Session["CurrentClientId"] +"&Tid="+TenentId).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var objClientModel = response.Content.ReadAsAsync<SettingModel>().Result;
                        return View(objClientModel);
                    }
                    return View("Record Not Found");
                }
                return RedirectToAction("AccessDenied", "Error");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> MySettings(SettingModel ObjClientModel)
        {
            try
            {
                #region New
                if (!string.IsNullOrEmpty(Request["hdnDateName"]))
                {
                    ObjClientModel.DateFormat = Request["hdnDateName"].ToString();
                }

                if (!string.IsNullOrEmpty(Request["hdnTimeZoneName"]))
                {
                    ObjClientModel.TimeFormat = Request["hdnTimeZoneName"].ToString();
                }

                if (!string.IsNullOrEmpty(Request["hdnCurrencyName"]))
                {
                    ObjClientModel.CurrencyCode = Request["hdnCurrencyName"].ToString();
                }
                if (!string.IsNullOrEmpty(Request["hdnReportingCurrencyName"]))
                {
                    ObjClientModel.ReportingCurrency = Request["hdnReportingCurrencyName"].ToString();
                }
                if (!string.IsNullOrEmpty(Request["hdnMilestoneName"]))
                {
                    ObjClientModel.DefaultMilestone = Request["hdnMilestoneName"].ToString();

                }
                if (!string.IsNullOrEmpty(Request["hdnLanguageName"]))
                {
                    ObjClientModel.LanguageCode = Request["hdnLanguageName"].ToString();
                }
                #endregion
                //SelectAllCultures();
                ObjClientModel.TenantId = TenentId;
                ObjClientModel.Id = Convert.ToInt64(Session["CurrentClientId"]);
                var response = await client.PutAsJsonAsync<SettingModel>("api/Settings/MySettings", ObjClientModel);
                if (response.IsSuccessStatusCode)
                {
                    var objResultValue = await response.Content.ReadAsAsync<SettingModel>();
                    new BaseController();
                    ViewData["objResultValue"] = objResultValue;

                    return View(objResultValue);
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
        public JsonResult SelectTimeFormat()
        {
            response = client.GetAsync("api/Settings/SelectTimeFormat?Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var _objTimeformat = response.Content.ReadAsAsync<IEnumerable<KeyValueFormatModel>>().Result.ToList();
                return Json(_objTimeformat, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        public JsonResult SelectDateFormat()
        {
            response = client.GetAsync("api/Settings/SelectDateFormat?Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var _objDateformat = response.Content.ReadAsAsync<IEnumerable<KeyValueFormatModel>>().Result;
                return Json(_objDateformat, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public JsonResult SelectCurrencyFormat()
        {
            response = client.GetAsync("api/Settings/SelectCurrencyFormat?Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objSettingsCurrency = response.Content.ReadAsAsync<IEnumerable<KeyValueFormatModel>>().Result;
                ViewData["SettingsCurrency"] = new SelectList(objSettingsCurrency, "Id", "Name", new object { });
                return Json(objSettingsCurrency, JsonRequestBehavior.AllowGet);
            }
            return null;

        }

        public JsonResult SelectAllMileStone()
        {
            response = client.GetAsync("api/MileStones/SelectAllMileStone?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objMileStoneModel = response.Content.ReadAsAsync<IEnumerable<MilestoneModel>>().Result.ToList();
                //  ViewData["SelectMilestones"] = new SelectList(objMileStoneModel, "Id", "Description", new object { }, "MSOrder");
                // return Json(objMileStoneModel, JsonRequestBehavior.AllowGet);
                return Json(objMileStoneModel, JsonRequestBehavior.AllowGet); ;
            }
            return null;
        }

        [HttpPost]
        public ActionResult Create(MySettingModel objMySettingModel)
        {
            try
            {
                objMySettingModel.TenantId = TenentId;
                var response = client.PostAsJsonAsync("api/Settings/Insert", objMySettingModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    var Leadsources = response.Content.ReadAsAsync<MySettingModel>().Result;
                    this.SetAlert(new AlertMessageViewModel()
                    {
                        MessageType = MessageType.Success,
                        MessageHeading = "Success",
                        MessageString = "Settings Created Successfully."
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


        //private void SelectAllCultures()
        //{
        //    response = client.GetAsync("api/Settings/SelectTimeFormat").Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var objSettings = response.Content.ReadAsAsync<IEnumerable<KeyValueFormatModel>>().Result;
        //        ViewData["SettingsTimeFormat"] = new SelectList(objSettings, "Id", "Name", new object { });
        //        return Json(objSettings, JsonRequestBehavior.AllowGet);
        //    }

        //    response = client.GetAsync("api/Settings/SelectDateFormat").Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var objSettingsDateFormat = response.Content.ReadAsAsync<IEnumerable<KeyValueFormatModel>>().Result;
        //        ViewData["SettingsDateFormat"] = new SelectList(objSettingsDateFormat, "Id", "Name", new object { });
        //    }

        //    response = client.GetAsync("api/Settings/SelectCurrencyFormat").Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var objSettingsCurrency = response.Content.ReadAsAsync<IEnumerable<KeyValueFormatModel>>().Result;
        //        ViewData["SettingsCurrency"] = new SelectList(objSettingsCurrency, "Id", "Name", new object { });

        //    }

        //    //Milestones
        //    response = client.GetAsync("api/MileStones/SelectAllMileStone?Tid=" + TenentId).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var objMileStoneModel = response.Content.ReadAsAsync<IEnumerable<MilestoneModel>>().Result.ToList();
        //        ViewData["SelectMilestones"] = new SelectList(objMileStoneModel, "Id", "Description", new object { }, "MSOrder");
        //    }

        //    //response = client.GetAsync("api/Settings/SelectNumberFormat").Result;
        //    //if (response.IsSuccessStatusCode)
        //    //{
        //    //    var objSettingsNumberFormat = response.Content.ReadAsAsync<IEnumerable<KeyValueFormatModel>>().Result.ToList();
        //    //    ViewData["SettingsNumberFormat"] = objSettingsNumberFormat;

        //    //}
        //}
    }
}
