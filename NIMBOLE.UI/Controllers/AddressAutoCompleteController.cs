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

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Web.UI;

namespace NIMBOLE.UI.Controllers
{
    public class AddressAutoCompleteController : BaseController
    {
        Guid TenentId = new Guid();

        public AddressAutoCompleteController()
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

        // GET: /AddressAutoComplete/
        public ActionResult Index()
        {
            return View();
        }

        #region UsingMethods
        [HttpGet]
        public JsonResult GetCountryNames([DataSourceRequest] DataSourceRequest request)
        {
            response = client.GetAsync("api/AddressAutoComplete/Countries?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var Countries = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.CountryModel>>().Result;
                return Json(Countries, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpGet]
        public JsonResult GetStateNamesCreate(int? Countries)
        {
            if (Countries != null)
            {
                var response = client.GetAsync("api/AddressAutoComplete/GetStateNamesByCountryId?CountryId=" + Countries+"&Tid="+TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var statename = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.KeyValueModel>>().Result;
                    if (statename.Count() > 0)
                        return Json(statename, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetCityNamesCreate(int? states)
        {
            if (states != null)
            {
                var response = client.GetAsync("api/AddressAutoComplete/GetCityNamesByStateId?stateId=" + states +"&Tid=" + TenentId).Result;
                if (response.IsSuccessStatusCode)
                {
                    var cityname = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.KeyValueModel>>().Result;
                    if (cityname.Count() > 0)
                        return Json(cityname, JsonRequestBehavior.AllowGet);
                    else
                        return Json(Content(""));
                }
                else
                    return Json(Content(""));
            }
            else
                return Json(Content(""));
        }
        #endregion UsingMethods

        
        [HttpGet]
        public JsonResult GetStateNamesByCountryName(string countries)
        {
            try
            {
                if(countries!=null)
                {
                    if (!countries.Equals(""))
                    {
                        var response = client.GetAsync("api/AddressAutoComplete/StatesByCountryName?countryName=" + countries).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var states = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.StateModel>>().Result;
                            return Json(states, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public JsonResult GetStateNames(int? countries, string stateFilter)
        {
            try
            {
                var response = client.GetAsync("api/AddressAutoComplete/States?stateFilter=" + stateFilter + "&countries=" + countries).Result;
                if (response.IsSuccessStatusCode)
                {
                    var states = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.KeyValueModel>>().Result;
                    return Json(states, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpGet]
        public JsonResult GetCityNames(int? states, string cityFilter)
        {
            var response = client.GetAsync("api/AddressAutoComplete/cities?states=" + states + "&cityFilter=" + cityFilter).Result;
            if(response.IsSuccessStatusCode)
            {
                var cities = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.KeyValueModel>>().Result;
                return Json(cities, JsonRequestBehavior.AllowGet);
            }
           return null;
        }

        [HttpGet]
        public JsonResult StatesWithCountry()
        {
            var response = client.GetAsync("api/AddressAutoComplete/StatesWithCountry").Result;
            if (response.IsSuccessStatusCode)
            {
                var statename = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.KeyValueModel>>().Result;
                if (statename.Count() > 0)
                    return Json(statename, JsonRequestBehavior.AllowGet);
                else
                    return Json(Content(""));
            }
            return null;
        }

        [HttpGet]
        public JsonResult CitiesWithStates()
        {
            var response = client.GetAsync("api/AddressAutoComplete/CitiesWithStates").Result;
            if (response.IsSuccessStatusCode)
            {
                var statename = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Models.Models.KeyValueModel>>().Result;
                if (statename.Count() > 0)
                    return Json(statename, JsonRequestBehavior.AllowGet);
                else
                    return Json(Content(""));
            }
            return null;
        }


        [HttpPost]
        public ActionResult Create(CountryModel objCountryModel)
        {
            try
            {
                response = client.PostAsJsonAsync("api/Contacts/Insert", objCountryModel).Result;
                if (response.IsSuccessStatusCode)
                {
                    var objTransContactModelRes = response.Content.ReadAsAsync<CountryModel>().Result;   

                }
                else
                {
                    

                }
                return null;
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

    }
}