using MyResources;
using NIMBOLE.Entities;
using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace NIMBOLE.Service.Controllers
{
    public class CommanMethods:ApiController
    {
        public ApiMessageError error = new ApiMessageError() { message = "Model Validated No Error Found" };
        private static CommanMethods _CurrentObject;
        public static CommanMethods CurrentObject
        {
            get
            {
                if (_CurrentObject == null)
                    _CurrentObject = new CommanMethods();
                return _CurrentObject;
            }
        }

        public void setCultureByUser(string strCultureCode)
        {
            if (CultureHelper.CultureName == null)
            {
                CultureHelper.CultureName = strCultureCode;//Set Default Language
            }
            if (!strCultureCode.Equals(""))
            {
                CultureHelper.CultureName = strCultureCode;
            }
            else
            {
                    CultureHelper.CultureName = "en";
            }
            
            //ReadCultureFromCookies();
            CultureHelper.ModifyCurrentCulture(CultureHelper.CultureName);
        }

        public HttpResponseMessage IsModelStateValid(System.Web.Http.ModelBinding.ModelStateDictionary ModelState, HttpRequestMessage Request)
        {
            if (!ModelState.IsValid)
            {
                // add errors into our client error model for client
                foreach (var prop in ModelState.Values)
                {
                    var modelError = prop.Errors.FirstOrDefault();
                    if (!string.IsNullOrEmpty(modelError.ErrorMessage))
                        error.errors.Add(modelError.ErrorMessage);
                    else
                        error.errors.Add(modelError.Exception.Message);
                }
                return Request.CreateResponse<ApiMessageError>(HttpStatusCode.Conflict, error);
            }
            else
                return Request.CreateResponse<ApiMessageError>(HttpStatusCode.OK, error);
        }

        public static DataTable GetDistinctRecords(DataTable dt, string[] Columns)
        {
            DataTable dtUniqRecords = new DataTable();
            dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
            return dtUniqRecords;
        }

        //public GeneralValuesModel GetCountryStateCity(string strFindWhat,long iPrevId)
        //{
        //    GeneralValuesModel objGeneralValuesModel=new GeneralValuesModel();
        //    using (NIMBOLEContext ctx=new NIMBOLEContext())
        //    {
        //        if(strFindWhat.Equals("City"))
        //        {
        //            var objData=ctx.TblCities.Where(s => s.Id==iPrevId).SingleOrDefault();
        //            objGeneralValuesModel=new GeneralValuesModel{
        //                Id=objData.Id,
        //                Code=objData.Code,
        //                Description=objData.CityName
        //            };
        //        }
        //        if (strFindWhat.Equals("State"))
        //        {
        //            var objData = ctx.TblStates.Where(s => s.TblCities.Where(x=>x.Id== iPrevId).FirstOrDefault().Status==true).FirstOrDefault();
        //            objGeneralValuesModel=new GeneralValuesModel{
        //                Id=objData.StateId,
        //                Code=objData.Code,
        //                Description=objData.StateName
        //            };
        //        }
        //        if (strFindWhat.Equals("Country"))
        //        {
        //            var objData = ctx.TblCountries.Where(s => s.TblStates.Where(x => x.StateId == iPrevId).FirstOrDefault().Status==true).FirstOrDefault();
        //            objGeneralValuesModel=new GeneralValuesModel{
        //                Id=objData.CountryId,
        //                Code=objData.Code,
        //                Description=objData.CountryName
        //            };
        //        }
        //    }
        //    return (objGeneralValuesModel);
        //}

        public GeneralValuesModel GetCountryStateCity(string strFindWhat, long iPrevId)
        {
            GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
            using (NimboleStagingEntities ctx = new NimboleStagingEntities())
            {
                if (strFindWhat.Equals("City"))
                {
                    //var objData = ctx.TblCities.Where(s => s.Id == iPrevId && s.Status==true).SingleOrDefault();
                    var objData = ctx.TblCities.SingleOrDefault(s => s.Id == iPrevId && s.Status == true);
                    objGeneralValuesModel = new GeneralValuesModel
                    {
                        Id = objData.Id,
                        Code = objData.Code,
                        Description = objData.CityName
                    };
                }
                if (strFindWhat.Equals("State"))
                {
                    var objData = ctx.TblStates.Where(s => s.StateId == iPrevId && s.Status==true).SingleOrDefault();
                   
                    objGeneralValuesModel = new GeneralValuesModel
                    {
                        Id = objData.StateId,
                        Code = objData.Code,
                        Description = objData.StateName
                    };
                }
                if (strFindWhat.Equals("Country"))
                {
                    var objData = ctx.TblCountries.Where(s => s.CountryId==iPrevId && s.Status == true).FirstOrDefault();
                    objGeneralValuesModel = new GeneralValuesModel
                    {
                        Id = objData.CountryId,
                        Code = objData.Code,
                        Description = objData.CountryName
                    };
                }
            }
            return (objGeneralValuesModel);
        }
    }
}