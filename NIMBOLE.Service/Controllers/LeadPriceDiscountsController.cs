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
    [RoutePrefix("api/LeadPriceDiscounts")]
    public class LeadPriceDiscountsController : ApiController
    {
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();        
        DTO objNIMBOLEMapper = new DTO();


        #region GET
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblTransLeadPriceDiscounts where x.TenantId==Tid select x).ToList();
                return Ok(data);
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
        [Route("GetRecent")]
        public IHttpActionResult GetRecent(int leadId, Guid Tid)
        {
            var objTblLPDiscount = _dbcontext.TblTransLeadPriceDiscounts.Where(lpd => lpd.LeadId == leadId && lpd.TenantId==Tid).OrderByDescending(lpd =>lpd.Id).FirstOrDefault();
            var objLeadPriceDiscountModel = objNIMBOLEMapper.MapTable2Model(objTblLPDiscount);
            return Ok(objLeadPriceDiscountModel);
        }
        
        [HttpGet]
        [Route("SelectAll")]
        public IHttpActionResult SelectAll(Guid Tid)
        {
            var data = (from x in _dbcontext.TblTransLeadPriceDiscounts where x.TenantId==Tid  select new { Id = x.Id, Description = x.DiscountedPrice }).ToList();
            return Ok(data);
        }
        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var query = _dbcontext.TblTransLeadPriceDiscounts.SingleOrDefault(x => x.Id == id);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
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
        [Route("IsNewRecord")]
        public IHttpActionResult IsNewRecord(long LeadId)
        {
            try
            {
                var result = _dbcontext.TblTransLeadPriceDiscounts.Where(tl => tl.LeadId == LeadId).Count();
                return Ok(result);
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
       // [ModelValidator]
        public IHttpActionResult Insert([FromBody] LeadPriceDiscountModel objLeadPriceDiscountModel)
        {
            try
            {
                objLeadPriceDiscountModel.CreatedDate = DateTime.Now;
                objLeadPriceDiscountModel.ModifiedDate = DateTime.Now;
                objLeadPriceDiscountModel.Status = true;           

                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion
                //TblTransLeadPriceDiscount objTblTransLeadPriceDiscount = objNIMBOLEMapper.MapModel2Table(objLeadPriceDiscountModel);
                //_dbcontext.TblTransLeadPriceDiscounts.Add(objTblTransLeadPriceDiscount);
                //_dbcontext.SaveChanges();
                //objLeadPriceDiscountModel = objNIMBOLEMapper.MapTable2Model(objTblTransLeadPriceDiscount);
                //return Ok(objLeadPriceDiscountModel);
                TblTransLeadPriceDiscount objTblTransLeadPriceDiscount = objNIMBOLEMapper.MapModel2Table(objLeadPriceDiscountModel);
                if (objTblTransLeadPriceDiscount.DiscountedDate.Value.Year == 1)
                    objTblTransLeadPriceDiscount.DiscountedDate = DateTime.Now;
                _dbcontext.TblTransLeadPriceDiscounts.Add(objTblTransLeadPriceDiscount);
                _dbcontext.SaveChanges();
                objLeadPriceDiscountModel = objNIMBOLEMapper.MapTable2Model(objTblTransLeadPriceDiscount);
                return Ok(objLeadPriceDiscountModel);
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

        #region PUT
        [HttpPut]
        [Route("Edit")]
        public IHttpActionResult Edit([FromBody]LeadPriceDiscountModel objLeadPriceDiscountModel)
        {
            try
            {
                TblTransLeadPriceDiscount objTblTransLeadPriceDiscount = objNIMBOLEMapper.MapModel2Table(objLeadPriceDiscountModel);
                var queries = (from x in _dbcontext.TblTransLeadPriceDiscounts select x).ToList();
                if (queries == null)
                    throw new InvalidOperationException("Record not Found.");
                TblTransLeadPriceDiscount query = new TblTransLeadPriceDiscount();
                query = queries.Where(x => x.Id == objTblTransLeadPriceDiscount.Id).FirstOrDefault();
                query.DiscountedDate = objTblTransLeadPriceDiscount.DiscountedDate ?? DateTime.Now;
                query.DiscountedDate = objTblTransLeadPriceDiscount.DiscountedDate;
                query.EmployeeId = objTblTransLeadPriceDiscount.EmployeeId;
                query.ApprovalStatus = objTblTransLeadPriceDiscount.ApprovalStatus;
                query.ApprovedBy = objTblTransLeadPriceDiscount.ApprovedBy;
                query.ApprovedDate = objTblTransLeadPriceDiscount.ApprovedDate;
                query.Comments = objTblTransLeadPriceDiscount.Comments;
                //query.CreatedDate = entity.CreatedDate;
                query.ModifiedDate = DateTime.Now;
                //query.Status = entity.Status;
                _dbcontext.SaveChanges();
                return Ok(objTblTransLeadPriceDiscount);
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
        [Route("Delete")]
        public IHttpActionResult Delete(Int64 Id)
        {
            try
            {
                var query = _dbcontext.TblTransLeadPriceDiscounts.Where(x => x.Id == Id).FirstOrDefault();
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                //_dbcontext.TblTransLeadPriceDiscounts.Remove(query);
                query.Status = false;
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
