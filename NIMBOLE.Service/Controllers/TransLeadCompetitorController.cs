using AutoMapper;
using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;
using NIMBOLE.Entities;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Common;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/TransLeadCompetitor")]
    public class TransLeadCompetitorController : ApiController
    {
        private static Object thisLock = new Object();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        #region GET
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var data = (from x in _dbcontext.TblTransLeadCompetitors select x).ToList();
                List<TblTransLeadCompetitor> lstTblTransLeadCompetitor = new List<TblTransLeadCompetitor>();
                foreach (var item in data)
                {
                    TblTransLeadCompetitor objTblTransLeadCompetitor = StoreTransLeadCompetitor(item);
                    lstTblTransLeadCompetitor.Add(objTblTransLeadCompetitor);
                }
                return Ok(lstTblTransLeadCompetitor);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }           
        }

        public TblTransLeadCompetitor StoreTransLeadCompetitor(TblTransLeadCompetitor item)
        {
            try
            {
                List<TblTransLeadCompetitor> lstTblTransLeadCompetitor = new List<TblTransLeadCompetitor>();
                TblTransLeadCompetitor objTblTransLeadCompetitor = new TblTransLeadCompetitor();
                objTblTransLeadCompetitor.Id = item.Id;
                objTblTransLeadCompetitor.TenantId = item.TenantId.ToDefaultTenantId();

                objTblTransLeadCompetitor.CompetitorId = item.CompetitorId;
                objTblTransLeadCompetitor.LeadId = item.LeadId;
                objTblTransLeadCompetitor.Price = item.Price;
                objTblTransLeadCompetitor.ProductId = item.ProductId;

                objTblTransLeadCompetitor.CreatedDate = item.CreatedDate;
                objTblTransLeadCompetitor.ModifiedDate = item.ModifiedDate;
                objTblTransLeadCompetitor.Status = item.Status;
                objTblTransLeadCompetitor.TblLead = item.TblLead;
                objTblTransLeadCompetitor.TblProduct = item.TblProduct;

                return objTblTransLeadCompetitor;
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
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var query = _dbcontext.TblTransLeadCompetitors.SingleOrDefault(x => x.Id == id);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    TblTransLeadCompetitor objTblTransLeadCompetitor = StoreTransLeadCompetitor(query);
                    return Ok(objTblTransLeadCompetitor);
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

        #region POST
        [HttpPost]
        [Route("Insert")]
        [ModelValidator]
        public IHttpActionResult Post([FromBody] TransLProductModel objTransLProductModel)
        {
            try
            {
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion
                var size = 0;
                var totalValue = 0;
                lock (thisLock)
                {
                    TblProduct objTblProduct = new TblProduct();

                    objTblProduct = _dbcontext.TblProducts.Where(p => p.Id == objTransLProductModel.ProductId && p.TenantId == objTransLProductModel.TenantId && p.Price == 0).FirstOrDefault();
                    if (objTblProduct != null)
                    {
                        objTblProduct.Price = objTransLProductModel.Price;
                        objTblProduct.Code = objTransLProductModel.Code;
                        objTblProduct.ModifiedDate = DateTime.Now;
                        _dbcontext.SaveChanges();
                    }
                    TblTransLeadCompetitor objTblTransLeadCompetitor = objNIMBOLEMapper.MapModel2Table(objTransLProductModel);
                    _dbcontext.TblTransLeadCompetitors.Add(objTblTransLeadCompetitor);
                    _dbcontext.SaveChanges();

                    var Objdata = _dbcontext.TblTransLeadCompetitors.Where(c => c.TenantId == objTransLProductModel.TenantId &&  c.LeadId == objTransLProductModel.LeadId && c.Status == true).ToList();
                    foreach (var data in Objdata)
                    {
                            size = Convert.ToInt16(data.Amount);
                            totalValue = totalValue + size;
                    }
                    var LeadSizevalue = _dbcontext.TblLeads.Where(c => c.Id == objTransLProductModel.LeadId && c.TenantId == objTransLProductModel.TenantId).FirstOrDefault();
                    LeadSizevalue.Size = totalValue;
                    _dbcontext.SaveChanges();
                    return Ok(objTblTransLeadCompetitor.Id);
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                #region Model Validation Exception Handling
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
                #endregion
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

        #region DELETE
        [HttpDelete]
        [Route("DeleteById")]
        public IHttpActionResult DeleteById(int Id)
        {
            try
            {
                var query = _dbcontext.TblContacts.Where(x => x.Id == Id).FirstOrDefault();
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                _dbcontext.TblContacts.Remove(query);
                _dbcontext.SaveChanges();
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
        #endregion
    }
}