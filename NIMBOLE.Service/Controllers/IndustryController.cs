using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using NIMBOLE.Models.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Entities;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Industry")]
    public class IndustryController : ApiController
    {
        private static Object thisLock = new Object();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        #region POST
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Post([FromBody] IndustryModel objIndustryModel)
        {
            try
            {
                lock (thisLock)
                {
                    HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);

                    #region If ModelState is not Valid
                    if (response.StatusCode != HttpStatusCode.OK)
                        return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                    #endregion
                    TblIndustry objTblIndustry = objNIMBOLEMapper.MapModel2Table(objIndustryModel);
                    if (!_dbcontext.TblIndustries.Any(u => u.Description == objIndustryModel.Description && u.TenantId==objIndustryModel.TenantId))
                    {
                        _dbcontext.TblIndustries.Add(objTblIndustry);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        TblIndustry _objTblIndustry = _dbcontext.TblIndustries.Where(u => u.Description == objIndustryModel.Description && u.TenantId==objIndustryModel.TenantId).FirstOrDefault();
                        if (_objTblIndustry.Status == false)
                            throw new InvalidOperationException("Record already exist.");
                    }
                    objIndustryModel = objNIMBOLEMapper.MapTable2Model(objTblIndustry);

                    var data = (from x in _dbcontext.TblIndustries where x.Status == true && x.TenantId==objIndustryModel.TenantId select new { Id = x.Id, Name = x.Description }).ToList();
                    return Ok(data);
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
        [Route("InsertIndustry")]
        public IHttpActionResult PostIndustry([FromBody] IndustryModel objIndustryModel)
        {
            try
            {
                lock (thisLock)
                {
                    HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);

                    #region If ModelState is not Valid
                    if (response.StatusCode != HttpStatusCode.OK)
                        return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                    #endregion
                    TblIndustry objTblIndustry = objNIMBOLEMapper.MapModel2Table(objIndustryModel);
                    if (!_dbcontext.TblIndustries.Any(u => u.Description == objIndustryModel.Description && u.TenantId==objIndustryModel.TenantId))
                    {
                        _dbcontext.TblIndustries.Add(objTblIndustry);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        TblIndustry _objTblIndustry = _dbcontext.TblIndustries.Where(u => u.Description == objIndustryModel.Description && u.TenantId== objIndustryModel.TenantId).FirstOrDefault();
                        if (_objTblIndustry.Status == false)
                            throw new InvalidOperationException("Record already exist.");
                    }
                    objIndustryModel = objNIMBOLEMapper.MapTable2Model(objTblIndustry);

                    var data = (from x in _dbcontext.TblIndustries where x.Status == true && x.TenantId==objIndustryModel.TenantId select new { Id = x.Id, Name = x.Description }).ToList();
                    return Ok(data);
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


        [HttpGet]
        [Route("GetByIndustry")]
        public IHttpActionResult GetByIndustry(string IndustryName, Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblIndustries where x.Description == IndustryName && x.TenantId == Tid select x).FirstOrDefault();

                if (data == null)
                {
                    IndustryModel objIndustryModel = new IndustryModel();
                    objIndustryModel.TenantId = Tid;
                    objIndustryModel.Description = IndustryName;
                    objIndustryModel.Status = true;
                    TblIndustry objTblIndustry = objNIMBOLEMapper.MapModel2Table(objIndustryModel);

                    _dbcontext.TblIndustries.Add(objTblIndustry);
                    _dbcontext.SaveChanges();

                    objIndustryModel = objNIMBOLEMapper.MapTable2Model(objTblIndustry);
                    objIndustryModel.Id = objTblIndustry.Id;

                    return Ok(objIndustryModel.Id);
                }
                else
                {
                    return Ok(data.Id);
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
    }
}
