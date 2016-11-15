using AutoMapper;
using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Service.Models;
using NIMBOLE.Common;
using NIMBOLE.Entities;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/AddressAutoComplete")]
    public class AddressAutoCompleteController : ApiController
    {
        private static Object thisLock = new Object();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        #region UsingMethods
        [HttpGet]
        [Route("Countries")]
        public IHttpActionResult Countries(Guid Tid)
        {
            try
            {
                var allCountryIdWithStates = _dbcontext.TblStates.Where(st => st.CountryId != null).ToDictionary(st => st.StateId, st => st.CountryId);
                var allCWS = allCountryIdWithStates.Select(a => a.Value ?? 0).ToList().Distinct();
                var query = _dbcontext
                            .TblCountries
                            .Where(ctry => allCWS.Contains(ctry.CountryId))
                            .ToList()
                            .Select(c => new {CountryId = c.CountryId, CountryName = c.CountryName }).OrderBy(c=>c.CountryName);
                return Ok(query);
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
        [Route("GetStateNamesByCountryId")]
        public IHttpActionResult GetStateNamesByCountryId(int? CountryId,Guid Tid)
        {
            try
            {
                var states = _dbcontext.TblStates.Where(s => s.Status == true).AsQueryable();
                if (CountryId != 0)
                {
                    states = states.Where(s => s.CountryId == CountryId);
                    var query = (states.Select(s => new KeyValueModel { Id = s.StateId, Name = s.StateName }).OrderBy(s => s.Name)).ToList();
                    //query.Insert(0, new KeyValueModel
                    //{
                    //    Id = 0,
                    //    Name = "Select"
                    //}); 
                    return Ok(query);
                }
                return null;
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
        [Route("GetCityNamesByStateId")]
        public IHttpActionResult GetCityNamesByStateId(int? stateId, Guid Tid)
        {
            try
            {
                var cities = _dbcontext.TblCities.Where(c => c.Status == true).AsQueryable();
                if (stateId != 0)
                {
                    cities = cities.Where(o => o.StateId == stateId);
                    var query = (cities.Select(c => new KeyValueModel { Id = c.Id, Name = c.CityName }).OrderBy(s => s.Name)).ToList();
                    //query.Insert(0, new KeyValueModel
                    //{
                    //    Id = 0,
                    //    Name = "Select"
                    //}); 
                    return Ok(query);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        #endregion UsingMethods

        #region NonUsingMethods
        [HttpGet]
        [Route("StatesByCountryName")]
        public IHttpActionResult StatesByCountryName(string countryName)
        {
            try
            {
                var CountryId = _dbcontext.TblCountries.Where(country => country.CountryName == countryName && country.Status == true).Select(ctry => ctry.CountryId).FirstOrDefault();
                var query = _dbcontext
                            .TblStates
                            .Where(st => st.CountryId == CountryId && st.Status == true)
                            .Select(st => new { StateId = st.StateId, StateName = st.StateName });
                return Ok(query);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        [HttpGet]
        [Route("StatesWithCountry")]
        public IHttpActionResult StatesWithCountry()
        {
            try
            {
                var states = _dbcontext.TblStates.Where(s => s.Status == true).AsQueryable();
                var query = (states.Select(s => new { StateId = s.StateId, Name = s.StateName, Id = s.CountryId }).OrderBy(s => s.Name)).ToList();
                return Ok(query);
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
        [Route("CitiesWithStates")]
        public IHttpActionResult CitiesWithStates()
        {
            try
            {
                var cities = _dbcontext.TblCities.Where(c => c.Status == true).AsQueryable();
                var query = (cities.Select(s => new { CityId = s.Id, Name = s.CityName, StateId = s.StateId }).OrderBy(s => s.Name)).ToList();
                return Ok(query);
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
        [Route("States")]
        public IHttpActionResult States(int? countries, string stateFilter)
        {
            try
            {
                var states = _dbcontext.TblStates.Where(s => s.Status == true).AsQueryable();
                if (countries != null)
                {
                    states = states.Where(s => s.CountryId == countries);
                }

                if (!string.IsNullOrEmpty(stateFilter))
                {
                    states = states.Where(s => s.StateName.Contains(stateFilter));
                }

                var query = (states.Select(s => new { Id = s.StateId, Name = s.StateName }).OrderBy(s => s.Name)).ToList();
                return Ok(query);
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
        [Route("Cities")]
        public IHttpActionResult Cities(int? states, string cityFilter)
        {
            try
            {

                var cities = _dbcontext.TblCities.Where(c => c.Status == true).AsQueryable();
                if (states != null)
                {
                    cities = cities.Where(c => c.StateId == states);
                }

                if (!string.IsNullOrEmpty(cityFilter))
                {
                    cities = cities.Where(c => c.CityName.Contains(cityFilter));
                }

                var query = (cities.Select(c => new { Id = c.Id, Name = c.CityName }).OrderBy(s => s.Name)).ToList();
                return Ok(query);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        #endregion NonUsingMethods

        #region Country State City

        [HttpPost]
        [Route("InsertCountry")]
        [ModelValidator]
        public IHttpActionResult PostCountry([FromBody] CountryModel objCountryModel)
        {
            try
            {

                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);

                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                lock (thisLock)
                {
                    TblCountry objTblCountry = objNIMBOLEMapper.MapModel2Table(objCountryModel);

                    if (!_dbcontext.TblCountries.Any(u => u.CountryName == objCountryModel.CountryName))
                    {
                        _dbcontext.TblCountries.Add(objTblCountry);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        throw new InvalidOperationException("Record already exists.");
                    }
                    objCountryModel = objNIMBOLEMapper.MapTable2Model(objTblCountry);
                    objCountryModel.CountryId = objTblCountry.CountryId;
                    if (objCountryModel != null)
                        return Ok(objCountryModel);
                    else
                        throw new InvalidOperationException("Record not found.");
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }


        [HttpPost]
        [Route("InsertState")]
        [ModelValidator]
        public IHttpActionResult PostState([FromBody] StateModel objStateModel)
        {
            try
            {

                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);

                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                lock (thisLock)
                {
                    TblState objTblState = objNIMBOLEMapper.MapModel2Table(objStateModel);

                    if (!_dbcontext.TblStateNews.Any(u => u.StateName == objStateModel.StateName))
                    {
                        _dbcontext.TblStates.Add(objTblState);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        throw new InvalidOperationException("Record already exists.");
                    }
                    objStateModel = objNIMBOLEMapper.MapTable2Model(objTblState);
                    objStateModel.StateId = objTblState.StateId;
                    if (objStateModel != null)
                        return Ok(objStateModel);
                    else
                        throw new InvalidOperationException("Record not found.");
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }


        [HttpPost]
        [Route("InsertCity")]
        [ModelValidator]
        public IHttpActionResult PostCity([FromBody] CityModel objCityModel)
        {
            try
            {

                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);

                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                lock (thisLock)
                {
                    TblCity objTblCity = objNIMBOLEMapper.MapModel2Table(objCityModel);

                    if (!_dbcontext.TblCities.Any(u => u.CityName == objCityModel.CityName))
                    {
                        _dbcontext.TblCities.Add(objTblCity);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        throw new InvalidOperationException("Record already exists.");
                    }
                    objCityModel = objNIMBOLEMapper.MapTable2Model(objTblCity);
                    objCityModel.Id = objTblCity.Id;
                    if (objCityModel != null)
                        return Ok(objCityModel);
                    else
                        throw new InvalidOperationException("Record not found.");
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
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
        
        #region Country State City For Excel Import

  
        [HttpGet]
        [Route("GetIdByCountry")]
        public IHttpActionResult GetIdByCountry(string Country, Guid Tid)
        {
            try
            {
                var query = _dbcontext.TblCountries.SingleOrDefault(x => x.CountryName == Country );
                if (query == null)
                {
                    CountryModel objCountryModel = new CountryModel();
                    objCountryModel.CountryName = Country;
                    TblCountry ObjTblCountry = objNIMBOLEMapper.MapModel2Table(objCountryModel);
                    _dbcontext.TblCountries.Add(ObjTblCountry);
                    _dbcontext.SaveChanges();
                    return Ok(ObjTblCountry.CountryId);
                }
                else
                {
                    return Ok(query.CountryId);
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
        [Route("GetIdByState")]
        public IHttpActionResult GetIdByState(string State,int Countryid, Guid Tid)
        {
            try
            {
                var query = _dbcontext.TblStateNews.SingleOrDefault(x => x.StateName == State && x.TenantId==Tid);
                if (query == null)
                {
                    StateModel objStateModel = new StateModel();
                    objStateModel.StateName = State;
                    if(Countryid> 0)
                    objStateModel.CountryId = Countryid;
                    TblState ObjTblState = objNIMBOLEMapper.MapModel2Table(objStateModel);
                    _dbcontext.TblStates.Add(ObjTblState);
                    _dbcontext.SaveChanges();
                    return Ok(ObjTblState.StateId);
                }
                else
                {
                    return Ok(query.StateId);
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
        [Route("GetIdByCity")]
        public IHttpActionResult GetIdByCity(string City,int Stateid, Guid Tid)
        {
            try
            {
                var query = _dbcontext.TblCityNews.SingleOrDefault(x => x.CityName == City && x.TenantId==Tid);
                if (query == null)
                {
                    CityModel objCityModel = new CityModel();
                    objCityModel.CityName = City;
                    if(Stateid>0)
                    objCityModel.StateId = Stateid;
                    TblCity ObjTblCity = objNIMBOLEMapper.MapModel2Table(objCityModel);
                    _dbcontext.TblCities.Add(ObjTblCity);
                    _dbcontext.SaveChanges();
                    return Ok(ObjTblCity.Id);
                }
                else
                {
                    return Ok(query.Id);
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


    }
}
