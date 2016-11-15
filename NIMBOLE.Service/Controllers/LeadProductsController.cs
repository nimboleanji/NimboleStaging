using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;
using NIMBOLE.Models;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Common;
using NIMBOLE.Entities;
using System.Threading.Tasks;
using System.Data.Entity;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/LeadProducts")]
    public class LeadProductsController : ApiController
    {
        DTO objNIMBOLEMapper = new DTO();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        private static  Object thislock = new Object();
        int value;
        public LeadProductsController()
        {


        }
        #region Get
        [HttpGet]
        [Route("GetAllByLeadId")]
        public async Task<IHttpActionResult> GetAllByLeadId(long leadId)
        {
            try
            {
                List<TranLeadProdCompModel> lstTranLeadProdCompModel = new List<TranLeadProdCompModel>();
                List<CompetitorModel> lstCompetitorModel = new List<CompetitorModel>();

                if (leadId != 0)
                {
                    #region Create Pivot Table using LINQ

                    var result = await _dbcontext.TblProducts
                                .Join(_dbcontext.TblTransLeadCompetitors, Products => Products.Id, TransLeadCompetitors => TransLeadCompetitors.ProductId, (Products, TransLeadCompetitors) => new { Products, TransLeadCompetitors }).DefaultIfEmpty()
                                .Join(_dbcontext.TblProducts, LeadProductCompetitors => LeadProductCompetitors.TransLeadCompetitors.CompetitorId, Competitor => Competitor.Id, (LeadProductCompetitors, Competitor) => new { LeadProductCompetitors, Competitor })
                                .Select(all => new
                                {
                                    ProductId = all.LeadProductCompetitors.Products.Id,
                                    ProductName = all.LeadProductCompetitors.Products.ProductName,
                                    ProductType = all.LeadProductCompetitors.Products.ProductType,
                                    ProductPrice = all.LeadProductCompetitors.TransLeadCompetitors.Price ?? 0,
                                    Quantity = all.LeadProductCompetitors.TransLeadCompetitors.Quantity ?? 0,
                                    Amount = all.LeadProductCompetitors.TransLeadCompetitors.Amount ?? 0,
                                    LeadId = all.LeadProductCompetitors.TransLeadCompetitors.LeadId,
                                    Status = all.LeadProductCompetitors.TransLeadCompetitors.Status,
                                    Discount = all.LeadProductCompetitors.TransLeadCompetitors.Discount ?? 0,
                                    DiscountType = ((all.LeadProductCompetitors.TransLeadCompetitors.DiscountType == "Select") ? string.Empty : (all.LeadProductCompetitors.TransLeadCompetitors.DiscountType == "A") ? "Amount" : (all.LeadProductCompetitors.TransLeadCompetitors.DiscountType == "P") ? "Percentage" : ""),
                                    RowId = all.LeadProductCompetitors.TransLeadCompetitors.Id,
                                    CompPrice = all.LeadProductCompetitors.TransLeadCompetitors.Price ?? 0,
                                    CompQuantity = all.LeadProductCompetitors.TransLeadCompetitors.Quantity ?? 0,
                                    CompName = all.Competitor.ProductName,
                                    Competitors = all.Competitor,
                                }).Where(l => l.Status == true && l.LeadId == leadId).ToListAsync();
                    var dynamicColumnResult = (from r in result
                                               group r by new
                                               {
                                                   //r.RowId,
                                                   r.ProductId,
                                                   r.ProductName
                                                   //,r.Quantity,
                                                   //r.Amount,
                                                   //r.ProductPrice,
                                                   //r.Discount,
                                                   //r.DiscountType
                                               }
                                                   into dynColResGroup
                                                   where dynColResGroup.Count() > 0
                                                   select new
                                                   {
                                                       //dynColResGroup.Key.RowId,
                                                       dynColResGroup.Key.ProductId,
                                                       dynColResGroup.Key.ProductName,
                                                       //dynColResGroup.Key.Quantity,
                                                       //dynColResGroup.Key.Amount,
                                                       //dynColResGroup.Key.ProductPrice,
                                                       //dynColResGroup.Key.Discount,
                                                       //dynColResGroup.Key.DiscountType,
                                                       AllCompetitors = dynColResGroup.GroupBy(c => c.Competitors).Select
                                                       (m => new { CId = m.Key.Id, CName = m.Key.ProductName, CPrice = m.Key.Price }),
                                                   }).ToList();
                    #endregion
                    #region Preparing objTranLeadProdCompModel to Send
                    foreach (var LPCModel in dynamicColumnResult)
                    {
                        TranLeadProdCompModel objTranLeadProdCompModel = new TranLeadProdCompModel();
                        objTranLeadProdCompModel.Id = LPCModel.ProductId;
                        objTranLeadProdCompModel.ProdId = LPCModel.ProductId;
                        objTranLeadProdCompModel.ProductName = LPCModel.ProductName;

                        objTranLeadProdCompModel.Pro1RowId = Convert.ToInt64((from r in result where r.ProductId == objTranLeadProdCompModel.Id && r.CompName == objTranLeadProdCompModel.ProductName select r.RowId).FirstOrDefault());
                        //objTranLeadProdCompModel.Pro1RowId = LPCModel.RowId;
                        objTranLeadProdCompModel.Price = Convert.ToDecimal((from r in result where r.ProductId == objTranLeadProdCompModel.Id && r.CompName == objTranLeadProdCompModel.ProductName select r.ProductPrice).FirstOrDefault());
                        //objTranLeadProdCompModel.Price = LPCModel.ProductPrice;
                        objTranLeadProdCompModel.Discount = Convert.ToDecimal((from r in result where r.ProductId == objTranLeadProdCompModel.Id && r.CompName == objTranLeadProdCompModel.ProductName select r.Discount).FirstOrDefault());
                        //objTranLeadProdCompModel.Discount = LPCModel.Discount;
                        objTranLeadProdCompModel.DiscountType = ((from r in result where r.ProductId == objTranLeadProdCompModel.Id && r.CompName == objTranLeadProdCompModel.ProductName select r.DiscountType).FirstOrDefault());

                        var productType = ((from r in result where r.ProductId == objTranLeadProdCompModel.Id && r.CompName == objTranLeadProdCompModel.ProductName select r.ProductType).FirstOrDefault());
                        if (int.TryParse(productType, out value))
                        {
                            //Console.WriteLine("String is numeric");
                             var producttypeid = Convert.ToInt64(productType);
                             var Producttype = ((from p in _dbcontext.TblProductTypeNews where p.Id == producttypeid select p.ProductType).FirstOrDefault());
                             objTranLeadProdCompModel.ProductType = Producttype;
                        }
                        else
                        {
                            objTranLeadProdCompModel.ProductType = productType;
                        }
                        //objTranLeadProdCompModel.DiscountType = LPCModel.DiscountType;
                        objTranLeadProdCompModel.Quantity = Convert.ToInt64((from r in result where r.ProductId == objTranLeadProdCompModel.Id && r.CompName == objTranLeadProdCompModel.ProductName select r.Quantity).FirstOrDefault()); //Convert.ToInt64(LPCModel.Quantity);
                        //objTranLeadProdCompModel.Quantity = Convert.ToInt64(LPCModel.Quantity);
                        objTranLeadProdCompModel.Amount = Convert.ToDecimal((from r in result where r.ProductId == objTranLeadProdCompModel.Id && r.CompName == objTranLeadProdCompModel.ProductName select r.Amount).FirstOrDefault());
                        //objTranLeadProdCompModel.Amount = Convert.ToDecimal(LPCModel.Amount);


                        #region Prepare Competitors
                        var lstComp = LPCModel.AllCompetitors.ToList();
                        if (lstComp.Count > 0)
                        {
                            for (int i = 1; i < lstComp.Count(); i++)
                            {
                                var comp = lstComp[i];
                                CompetitorModel objCompetitorModel = new CompetitorModel();
                                objCompetitorModel.Id = comp.CId;

                                objCompetitorModel.Name = comp.CName;
                                objCompetitorModel.Price = Convert.ToDecimal(comp.CPrice);
                                switch (i)
                                {
                                    case 1:
                                        objTranLeadProdCompModel.Comp1Name = objCompetitorModel.Name;
                                        objTranLeadProdCompModel.Comp1Price = objCompetitorModel.Price;
                                        objTranLeadProdCompModel.Comp1ProdId = objCompetitorModel.Id;
                                        objTranLeadProdCompModel.Com1RowId = Convert.ToInt64((from r in result where r.ProductId == objTranLeadProdCompModel.Id && r.CompName == objTranLeadProdCompModel.Comp1Name select r.RowId).FirstOrDefault());
                                        objTranLeadProdCompModel.Comp1 = objCompetitorModel;
                                        break;
                                }
                                objTranLeadProdCompModel.lstCompetitorModel.Add(objCompetitorModel);
                            }
                        }
                        else
                        {
                            CompetitorModel objCompetitorModel = new CompetitorModel();
                            objTranLeadProdCompModel.lstCompetitorModel.Add(objCompetitorModel);
                        }
                        #endregion
                        ProductModel objProductModel = new ProductModel();
                        TblProduct objTblProduct = _dbcontext.TblProducts.Where(p => p.Id == LPCModel.ProductId).FirstOrDefault();
                        objProductModel = objNIMBOLEMapper.MapTable2Model(objTblProduct);
                        objTranLeadProdCompModel.Prod = objProductModel;
                        lstTranLeadProdCompModel.Add(objTranLeadProdCompModel);
                    }
                    #endregion
                    return Ok(lstTranLeadProdCompModel);
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
        #endregion Get

        #region POST
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Post([FromBody]TranLeadProdCompModel objTranLeadProdCompModel)
        {
            try
            {
                List<long> compIds = new List<long>();
                TblProduct objTblProduct = new TblProduct();
                TblTransLeadCompetitor objTblTransLeadCompetitor = new TblTransLeadCompetitor();

                var productPrice = _dbcontext.TblProducts.Where(p => p.Id.Equals(objTranLeadProdCompModel.ProdId) && p.Price == 0).FirstOrDefault();
                if (productPrice != null)
                {
                    productPrice.Price = objTranLeadProdCompModel.Price;
                    _dbcontext.SaveChanges();
                }
                if (objTranLeadProdCompModel.Comp1ProdId != 0)
                {
                    long comp1id = Convert.ToInt64(objTranLeadProdCompModel.Comp1ProdId);
                    objTblProduct = _dbcontext.TblProducts.Where(c => c.Id.Equals(comp1id)).FirstOrDefault();
                    compIds.Add(objTblProduct.Id);
                    objTblProduct.Price = objTranLeadProdCompModel.Comp1Price;
                    _dbcontext.SaveChanges();
                    objTranLeadProdCompModel.Comp1Name = objTranLeadProdCompModel.Comp1Name;
                }
                long proid = Convert.ToInt64(objTranLeadProdCompModel.ProdId);
                objTblProduct = _dbcontext.TblProducts.Where(p => p.Id.Equals(proid)).FirstOrDefault();
                lock (thislock)
                {
                    if (!_dbcontext.TblTransLeadCompetitors.Any(t => t.ProductId == objTranLeadProdCompModel.ProdId && t.LeadId == objTranLeadProdCompModel.LeadId && t.CompetitorId == objTblProduct.Id && t.Status == true))
                    {
                        objTblTransLeadCompetitor.CompetitorId = objTblProduct.Id;
                        objTblTransLeadCompetitor.LeadId = objTranLeadProdCompModel.LeadId;
                        objTblTransLeadCompetitor.ProductId = objTblProduct.Id;
                        objTblTransLeadCompetitor.Discount = objTranLeadProdCompModel.Discount;
                        objTblTransLeadCompetitor.DiscountType = objTranLeadProdCompModel.DiscountType == "Select" ? string.Empty : objTranLeadProdCompModel.DiscountType;
                        if (objTblTransLeadCompetitor.DiscountType == "Amount")
                        {
                            objTblTransLeadCompetitor.DiscountType = "A";
                        }
                        if (objTblTransLeadCompetitor.DiscountType == "Percentage")
                        {
                            objTblTransLeadCompetitor.DiscountType = "P";
                        }
                        objTblTransLeadCompetitor.Amount = objTranLeadProdCompModel.Amount;
                        objTblTransLeadCompetitor.Price = objTranLeadProdCompModel.Price;
                        objTblTransLeadCompetitor.Quantity = objTranLeadProdCompModel.Quantity;
                        objTblTransLeadCompetitor.CreatedDate = DateTime.Now;
                        objTblTransLeadCompetitor.ModifiedDate = DateTime.Now;
                        objTblTransLeadCompetitor.Status = true;
                        objTblTransLeadCompetitor.TenantId = objTranLeadProdCompModel.TenantId.ToDefaultTenantId();
                        _dbcontext.TblTransLeadCompetitors.Add(objTblTransLeadCompetitor);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        throw new InvalidOperationException("Record already exists.");
                    }
                 for (int i = 1; i <= compIds.Count; i++)
                {
                    objTblTransLeadCompetitor = new TblTransLeadCompetitor();
                    #region Updating...objTblTransLeadCompetitor
                    objTblTransLeadCompetitor.CompetitorId = compIds[i - 1];
                    objTblTransLeadCompetitor.LeadId = objTranLeadProdCompModel.LeadId;
                    objTblTransLeadCompetitor.ProductId = objTblProduct.Id;
                    if (!_dbcontext.TblTransLeadCompetitors.Any(t => t.ProductId == objTblProduct.Id && t.CompetitorId == objTblTransLeadCompetitor.CompetitorId && t.LeadId == objTranLeadProdCompModel.LeadId && t.Status == true))
                    {
                        switch (i)
                        {
                            case 1:
                                objTblTransLeadCompetitor.Price = objTranLeadProdCompModel.Comp1Price;
                                break;
                        }
                        objTblTransLeadCompetitor.Quantity = 1;
                        objTblTransLeadCompetitor.CreatedDate = DateTime.Now;
                        objTblTransLeadCompetitor.ModifiedDate = DateTime.Now;
                        objTblTransLeadCompetitor.Status = true;
                        objTblTransLeadCompetitor.TenantId = objTranLeadProdCompModel.TenantId.ToDefaultTenantId();
                        _dbcontext.TblTransLeadCompetitors.Add(objTblTransLeadCompetitor);
                        _dbcontext.SaveChanges();
                    }
                }
                    #endregion
                }
                objTranLeadProdCompModel.Id = objTblTransLeadCompetitor.Id;
                var queries = _dbcontext.TblLeads.Where(ld => ld.Id == objTblTransLeadCompetitor.LeadId).FirstOrDefault();
                queries.Size = queries.Size + Convert.ToInt64(objTranLeadProdCompModel.Price * objTranLeadProdCompModel.Quantity);
                _dbcontext.SaveChanges();
                return Ok(objTranLeadProdCompModel);
            }
            //catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            //{
            //    Exception raise = dbEx;
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            string message = string.Format("{0}:{1}",
            //                validationErrors.Entry.Entity.ToString(),
            //                validationError.ErrorMessage);
            //            raise = new InvalidOperationException(message, raise);
            //        }
            //    }
            //    throw raise;
            //}
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        #endregion

        #region Edit
        [HttpPut]
        [Route("Edit")]
        public IHttpActionResult Edit([FromBody] TranLeadProdCompModel objTranLeadProdCompModel)
        {
            try
            {
                var objTblProduct = _dbcontext.TblProducts.Where(p => p.Id.Equals(objTranLeadProdCompModel.ProdId) && p.Price == 0).FirstOrDefault();
                if (objTblProduct != null)
                {
                    objTblProduct.Price = objTranLeadProdCompModel.Price;
                    objTblProduct.ModifiedDate = DateTime.Now;
                    _dbcontext.SaveChanges();
                }
                if (objTranLeadProdCompModel.Comp1ProdId != 0)
                {
                    long comp1id = Convert.ToInt64(objTranLeadProdCompModel.Comp1ProdId);
                    objTblProduct = _dbcontext.TblProducts.Where(c => c.Id.Equals(comp1id)).FirstOrDefault();
                    objTblProduct.Price = objTranLeadProdCompModel.Comp1Price;
                    _dbcontext.SaveChanges();
                }
                var objTranleadcomp = (from tc in _dbcontext.TblTransLeadCompetitors where tc.Id == objTranLeadProdCompModel.Pro1RowId select tc).FirstOrDefault();
                if (objTranleadcomp != null)
                {
                    objTranleadcomp.ProductId = objTranLeadProdCompModel.ProdId;
                    objTranleadcomp.CompetitorId = objTranLeadProdCompModel.ProdId;
                    objTranleadcomp.Price = objTranLeadProdCompModel.Price;
                    objTranleadcomp.Quantity = objTranLeadProdCompModel.Quantity;
                    objTranleadcomp.Amount = objTranLeadProdCompModel.Amount;
                    objTranleadcomp.Discount = objTranLeadProdCompModel.Discount;
                    objTranleadcomp.ModifiedDate = DateTime.Now;
                    objTranleadcomp.Status = true;
                    objTranleadcomp.DiscountType = objTranLeadProdCompModel.DiscountType == "Select" ? string.Empty : objTranLeadProdCompModel.DiscountType;
                    if (objTranleadcomp.DiscountType == "Amount")
                    {
                        objTranleadcomp.DiscountType = "A";
                    }
                    if (objTranleadcomp.DiscountType == "Percentage")
                    {
                        objTranleadcomp.DiscountType = "P";
                    }
                    _dbcontext.SaveChanges();
                }

                TblTransLeadCompetitor objTranleadcompdet1 = new TblTransLeadCompetitor();
                objTranleadcompdet1 = (from tc in _dbcontext.TblTransLeadCompetitors where tc.Id == objTranLeadProdCompModel.Com1RowId select tc).FirstOrDefault();
                if (objTranleadcompdet1 != null)
                {
                    if (objTranLeadProdCompModel.Comp1Price != 0)
                    {
                        objTranleadcompdet1.ProductId = objTranLeadProdCompModel.ProdId;
                        objTranleadcompdet1.CompetitorId = objTranLeadProdCompModel.Comp1ProdId;
                        objTranleadcompdet1.Price = objTranLeadProdCompModel.Comp1Price;
                        objTranleadcompdet1.ModifiedDate = DateTime.Now;
                        objTranleadcompdet1.Status = true;
                        objTranleadcompdet1.Quantity = 1;
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        _dbcontext.TblTransLeadCompetitors.Remove(objTranleadcompdet1);
                        _dbcontext.SaveChanges();
                    }
                }
                else
                {
                    if (objTranLeadProdCompModel.Comp1ProdId != 0)
                    {
                        TblTransLeadCompetitor _objTransLeadCompetitor = new TblTransLeadCompetitor();
                        _objTransLeadCompetitor.ProductId = objTranLeadProdCompModel.ProdId;
                        _objTransLeadCompetitor.CompetitorId = objTranLeadProdCompModel.Comp1ProdId;
                        _objTransLeadCompetitor.TenantId = objTranLeadProdCompModel.TenantId.ToDefaultTenantId();
                        _objTransLeadCompetitor.LeadId = objTranleadcomp.LeadId;
                        _objTransLeadCompetitor.Quantity = 1;
                        _objTransLeadCompetitor.Price = objTranLeadProdCompModel.Comp1Price;
                        _objTransLeadCompetitor.ModifiedDate = DateTime.Now;
                        _objTransLeadCompetitor.Status = true;
                        _dbcontext.TblTransLeadCompetitors.Add(_objTransLeadCompetitor);
                        _dbcontext.SaveChanges();
                    }
                }
                return Ok(objTranLeadProdCompModel);
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
        public IHttpActionResult Delete(string proids, long LeadId)
        {
            try
            {
                var numbers = proids.Split(',').Select(Int64.Parse).ToList();
                var leadSize = Int64.Parse("0");
                for (int i = 0; i < numbers.Count; i++)
                {
                    if (numbers[i] != 0)
                    {
                        var prodId = numbers[0];
                        if (i == 0)
                        {
                            var query = _dbcontext.TblTransLeadCompetitors.Where(x => x.LeadId == LeadId && x.ProductId == prodId && x.CompetitorId == prodId && x.Status == true).FirstOrDefault();
                            if (query != null)
                            {
                                leadSize = Convert.ToInt64(query.Price * query.Quantity);
                                query.Status = false;
                                _dbcontext.Entry(query).State = System.Data.Entity.EntityState.Modified;
                                _dbcontext.SaveChanges();
                            }
                        }
                        else
                        {
                            var compId = numbers[i];
                            var query = _dbcontext.TblTransLeadCompetitors.Where(x => x.LeadId == LeadId && x.ProductId == prodId && x.CompetitorId == compId && x.Status == true).FirstOrDefault();
                            if (query != null)
                            {
                                leadSize += Convert.ToInt64(query.Price * query.Quantity);
                                query.Status = false;
                                _dbcontext.Entry(query).State = System.Data.Entity.EntityState.Modified;
                                _dbcontext.SaveChanges();
                            }
                        }
                    }
                }
                var leadQuery = _dbcontext.TblLeads.Where(x => x.Id == LeadId && x.Status == true).FirstOrDefault();
                if (leadQuery != null)
                {
                    leadQuery.Size = leadQuery.Size - leadSize;
                    if (leadQuery.Size > 0)
                        leadQuery.Size = leadQuery.Size;
                    else
                        leadQuery.Size = 0;

                    _dbcontext.Entry(leadQuery).State = System.Data.Entity.EntityState.Modified;
                    _dbcontext.SaveChanges();
                }
                return Ok();
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
