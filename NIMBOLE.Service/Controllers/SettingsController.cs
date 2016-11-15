using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Models;
using AutoMapper;
using NIMBOLE.Entities;
using System.Globalization;


namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Settings")]
    public class SettingsController : ApiController
    {
        private NimboleStagingEntities dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        #region Get

       // GetIdBasedonTid

        [HttpGet]
        [Route("GetIdBasedonTid")]
        public IHttpActionResult GetIdBasedonTid(Guid Tid)
        {
            try
            {
                // var query = dbcontext.TblSettings.FirstOrDefault(x => x.Id == id && x.TenantId==Tid);
                var query = dbcontext.TblSettings.FirstOrDefault(x => x.TenantId == Tid);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    SettingModel ObjClientModel = objNIMBOLEMapper.MapTable2Model(query);
                    return Ok(ObjClientModel);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id,Guid Tid)
        {
            try
            {
                var query = dbcontext.TblSettings.FirstOrDefault(x => x.Id == id && x.TenantId==Tid);
              //  var query = dbcontext.TblSettings.FirstOrDefault(x => x.TenantId == Tid);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    SettingModel ObjClientModel = objNIMBOLEMapper.MapTable2Model(query);
                    return Ok(ObjClientModel);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }            
        }
        #endregion

        #region PUT
        [HttpPut]
        [Route("MySettings")]
        public IHttpActionResult MySettings([FromBody] SettingModel ObjSettingModel)
        {
            try
            {
                var queries = (from x in dbcontext.TblSettings where x.Id == ObjSettingModel.Id && x.TenantId==ObjSettingModel.TenantId select x).FirstOrDefault();
                //var queries = (from x in dbcontext.TblSettings where x.TenantId == ObjSettingModel.TenantId select x).FirstOrDefault();

                if (queries != null)
                {
                    queries.ModifiedDate = DateTime.Now;
                    queries.FullName = ObjSettingModel.FullName;
                    queries.CurrencyCode = ObjSettingModel.CurrencyCode;
                    queries.ReportingCurrency = ObjSettingModel.ReportingCurrency;
                    queries.LanguageCode = ObjSettingModel.LanguageCode;
                    queries.PhoneNo = ObjSettingModel.PhoneNo;
                    queries.TimeFormat = ObjSettingModel.TimeFormat;
                    queries.NoOfLicenses = ObjSettingModel.NoOfLicenses;
                    queries.DateFormat = ObjSettingModel.DateFormat;
                    queries.Status = ObjSettingModel.Status;
                    queries.URL = ObjSettingModel.URL;
                    queries.DefaultEmail = ObjSettingModel.Email;
                    queries.DefaultMilestone = ObjSettingModel.DefaultMilestone;
                    dbcontext.SaveChanges();
                    return Ok(ObjSettingModel);
                }
                else
                {
                    throw new InvalidOperationException("Record not Found.");
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }            
        }
        #endregion

        #region ForDropDown

        [HttpGet]
        [Route("SelectTimeFormat")]
        public IHttpActionResult SelectTimeFormat()
        {
            try
            {
                List<TimeZoneInfo> allCultures = TimeZoneInfo.GetSystemTimeZones().ToList();
                var data = (from s in allCultures select new { Id = s.Id, Name = s.DisplayName}).ToList().Distinct();
                return Json(data);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        [HttpGet]
        [Route("SelectDateFormat")]
        public IHttpActionResult SelectDateFormat()
        {
            try
            {
                CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

                var data = (from s in allCultures select new { Id = s.DateTimeFormat.LongDatePattern, Name = s.DateTimeFormat.LongDatePattern }).Distinct().ToList();
                return Json(data);
            }

            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("SelectCurrencyFormat")]
        public IHttpActionResult SelectCurrencyFormat()
        {
            try
            {
                CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
                var data = (from s in allCultures select new { Id = new RegionInfo(s.Name).CurrencySymbol, Name = new RegionInfo(s.Name).CurrencySymbol }).Distinct().ToList();
                return Json(data);
            }

            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        //[HttpGet]
        //[Route("SelectNumberFormat")]
        //public IHttpActionResult SelectNumberFormat()
        //{
        //    try
        //    {

        //        CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

        //        var data = (from s in allCultures select new { Id = s.NumberFormat.CurrencyPositivePattern, Name = s.NumberFormat.CurrencyPositivePattern }).Distinct().ToList();

        //        return Ok(data);

        //    }

        //    catch (Exception ex)
        //    {
        //        throw new InvalidOperationException(ex.Message);
        //    }
        //}

        #endregion
    }
}
