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
    [RoutePrefix("api/Ownership")]
    public class OwnershipController : ApiController
    {
        private static Object thisLock = new Object();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        #region POST
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Post([FromBody] OwnershipModel objOwnershipModel)
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
                    TblOwnership objTblOwnership = objNIMBOLEMapper.MapModel2Table(objOwnershipModel);
                    if (!_dbcontext.TblOwnerships.Any(u => u.Description == objOwnershipModel.Description && u.TenantId == objOwnershipModel.TenantId ))
                    {
                        _dbcontext.TblOwnerships.Add(objTblOwnership);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        TblOwnership _objTblOwnership = _dbcontext.TblOwnerships.Where(u => u.Description == objOwnershipModel.Description && u.TenantId == objOwnershipModel.TenantId ).FirstOrDefault();
                        if (_objTblOwnership.Status == false)
                            throw new InvalidOperationException("Record already exist.");
                    }
                    objOwnershipModel = objNIMBOLEMapper.MapTable2Model(objTblOwnership);

                    List<TblOwnership> lstTblOwnership = _dbcontext.TblOwnerships.Where(cr => cr.Status == true && cr.TenantId == objOwnershipModel.TenantId ).ToList();
                    var ownership = (from o in _dbcontext.TblOwnerships where o.Status == true && o.TenantId == objOwnershipModel.TenantId  select o).ToList();
                    List<OwnershipModel> lstOwnershipModel = new List<OwnershipModel>();
                    foreach (var item in ownership)
                    {
                        lstOwnershipModel.Add(new OwnershipModel() { Id = item.Id, Description = item.Description });
                    }
                    return Ok(lstOwnershipModel);
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
        [Route("InsertOwner")]
        public IHttpActionResult PostOwner([FromBody] OwnershipModel objOwnershipModel)
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
                    TblOwnership objTblOwnership = objNIMBOLEMapper.MapModel2Table(objOwnershipModel);
                    if (!_dbcontext.TblOwnerships.Any(u => u.Description == objOwnershipModel.Description && u.TenantId == objOwnershipModel.TenantId ))
                    {
                        _dbcontext.TblOwnerships.Add(objTblOwnership);
                        _dbcontext.SaveChanges();
                    }
                   
                    objOwnershipModel = objNIMBOLEMapper.MapTable2Model(objTblOwnership);
                   
                    return Ok(objOwnershipModel.Id);
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
        [Route("GetByOwner")]
        public IHttpActionResult GetByOwner(string OwnerName, Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblOwnerships where x.Description == OwnerName && x.TenantId == Tid select x).FirstOrDefault();
                
                if (data == null)
                {
                    OwnershipModel objOwnershipModel = new OwnershipModel();
                    objOwnershipModel.TenantId = Tid;
                    objOwnershipModel.Description = OwnerName;
                    objOwnershipModel.Status = true;
                    TblOwnership objTblOwnership = objNIMBOLEMapper.MapModel2Table(objOwnershipModel);

                    _dbcontext.TblOwnerships.Add(objTblOwnership);
                    _dbcontext.SaveChanges();

                    objOwnershipModel = objNIMBOLEMapper.MapTable2Model(objTblOwnership);
                    objOwnershipModel.Id = objTblOwnership.Id;

                    return Ok(objOwnershipModel.Id);
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
