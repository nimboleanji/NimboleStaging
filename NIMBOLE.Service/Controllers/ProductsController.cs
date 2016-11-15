using AutoMapper;
using NIMBOLE.Models;
using NIMBOLE.Models.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;
using NIMBOLE.Service.Models;
using NIMBOLE.Common;
using NIMBOLE.Entities;
using System.Data.Entity;
using System.Threading.Tasks;


namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Products")]
    public class ProductsController : ApiController
    {
        private static Object thisLock = new Object();
        DTO objNIMBOLEMapper = new DTO();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        public ProductsController()
        {

        }

        #region GET
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblProducts where x.TenantId == Tid select x).ToList();
                List<ProductModel> lstProductModel = new List<ProductModel>();
                ProductModel objProductModel = new ProductModel();
                foreach (var item in data)
                {
                    objProductModel = objNIMBOLEMapper.MapTable2Model(item);

                    lstProductModel.Add(objProductModel);
                }
                return Json(lstProductModel);
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
        [Route("GetAllExport")]
        public IHttpActionResult GetAllExport(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.VWProductExports where x.TenantId == Tid select x).ToList();
                //List<ProductModel> lstProductModel = new List<ProductModel>();
                //ProductModel objProductModel = null;
                ////foreach (var item in data)
                ////{
                ////    objProductModel = new ProductModel();

                ////    objProductModel = objNIMBOLEMapper.MapTable2Model(item);

                ////    lstProductModel.Add(objProductModel);
                ////}
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
        [Route("GetById")]
        public IHttpActionResult GetById(int id, Guid Tid)
        {
            try
            {
                var query = _dbcontext.TblProducts.SingleOrDefault(x => x.Id == id && x.TenantId == Tid);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    int result;
                    string productType = query.ProductType;

                    if (int.TryParse(productType, out result))
                    {
                        int ptypeid = Convert.ToInt32(query.ProductType);
                        var ptype = (from pt in _dbcontext.TblProductTypeNews where pt.Id == ptypeid select pt.ProductType).FirstOrDefault();
                        query.ProductType = ptype;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(query.ProductType))
                        {
                            string ptypeid = productType;
                            var ptype = (from pt in _dbcontext.TblProductTypeNews where pt.ProductType == ptypeid select pt.ProductType).FirstOrDefault();
                            query.ProductType = ptype;
                        }
                    }
                    ProductModel objProductModel = objNIMBOLEMapper.MapTable2Model(query);
                    return Ok(objProductModel);
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
        [Route("GetByCode")]
        public IHttpActionResult GetByCode(string Code, Guid Tid)
        {
            try
            {
                var query = _dbcontext.TblProducts.SingleOrDefault(x => x.Code == Code && x.TenantId == Tid);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    ProductModel objProductModel = objNIMBOLEMapper.MapTable2Model(query);
                    return Ok(objProductModel);
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
        [Route("GetCompetitorPrice")]
        public IHttpActionResult GetCompetitorPrice(long ProdId, long LeadId, Guid Tid)
        {
            try
            {
                var query = (from p in _dbcontext.TblProducts where p.Id == ProdId && p.TenantId == Tid select p.Price).FirstOrDefault();
                decimal price = Convert.ToDecimal(query);
                return Ok(Convert.ToDouble(price));
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
        [Route("GetProductCode")]
        public IHttpActionResult GetProductCode(long ProdId, Guid Tid)
        {
            try
            {
                var query = (from p in _dbcontext.TblProducts where p.Id == ProdId && p.TenantId == Tid select p.Code).FirstOrDefault();
                string productCode = query.ToString();
                if (string.IsNullOrEmpty(productCode))
                    throw new InvalidOperationException("Record not Found.");
                else
                    return Ok(productCode);
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
        [Route("SelectAllProductsByType")]
        public async Task<IHttpActionResult> SelectAllProductsByType(string type, Guid Tid)
        {
            try
            {
                if (!string.IsNullOrEmpty(type))
                {
                    //var data = await (from x in _dbcontext.TblProducts where x.Status == true && x.ProductType == type && x.TenantId==Tid orderby x.ProductName select new { ProductNamesId = x.Id, ProductName = x.ProductName }).ToListAsync();

                    //List<ProductNamesModel> lstProductNamesModel = new List<ProductNamesModel>();

                    //foreach (var item in data)
                    //{
                    //    lstProductNamesModel.Add(new ProductNamesModel() { ProductNamesId = item.ProductNamesId, ProductName = item.ProductName });
                    //}

                    var lstProductNamesModel = await (from x in _dbcontext.TblProducts where x.Status == true && x.ProductType == type && x.TenantId == Tid orderby x.ProductName select new { ProductNamesId = x.Id, ProductName = x.ProductName }).ToListAsync();

                    return Json(lstProductNamesModel);
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
        [Route("SelectAllProducts")]
        public IHttpActionResult SelectAllProducts(Guid Tid)
        {
            try
            {

                //var data = (from x in _dbcontext.TblProducts where x.Status == true && x.TenantId==Tid orderby x.ProductName select new { ProductNamesId = x.Id, ProductName = x.ProductName }).ToList();
                //List<ProductNamesModel> lstProductNamesModel = new List<ProductNamesModel>();
                //foreach (var item in data)
                //{
                //    lstProductNamesModel.Add(new ProductNamesModel() { ProductNamesId = item.ProductNamesId, ProductName = item.ProductName });
                //}

                var lstProductNamesModel = (from x in _dbcontext.TblProducts where x.Status == true && x.TenantId == Tid orderby x.ProductName select new { ProductNamesId = x.Id, ProductName = x.ProductName }).ToList();

                return Json(lstProductNamesModel);
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
        [Route("SelectAllProductTypes")]
        public async Task<IHttpActionResult> SelectAllProductTypes(Guid Tid)
        {
            try
            {
                var data = await (from p in _dbcontext.TblProductTypeNews where p.TenantId==Tid orderby p.ProductType select new { ProductTypeId = p.Id, ProductType = p.ProductType }).ToListAsync();
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
        [Route("SelectProductsForDropdown")]
        public IHttpActionResult SelectProductsForDropdown(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblProducts where x.Status == true && x.TenantId == Tid orderby x.ProductName select new { Id = x.Id, Name = x.ProductName }).ToList();
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
        [Route("GetByType")]
        public IHttpActionResult GetByType(string prodType, Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblProductTypeNews where x.ProductType == prodType && x.TenantId==Tid select x).FirstOrDefault();

                if (data == null)
                {
                    ProductTypeModel objProductTypeModel = new ProductTypeModel();
                    objProductTypeModel.ProductType = prodType;
                    // objOwnershipModel.Status = true;
                    TblProductTypeNew objTblProductType = new TblProductTypeNew();
                    //objTblProductType.ProductType = objProductTypeModel.ProductType = prodType;
                    objTblProductType.ProductType = prodType;
                    objTblProductType.TenantId = Tid;
                    objTblProductType.CreatedDate = DateTime.Now.Date;
                    objTblProductType.ModifiedDate = DateTime.Now.Date;
                    objTblProductType.Status = true;

                    _dbcontext.TblProductTypeNews.Add(objTblProductType);
                    _dbcontext.SaveChanges();

                    objProductTypeModel.ProductTypeId = objTblProductType.Id;

                    return Ok(objProductTypeModel.ProductTypeId);
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

        // SelectAllProductsCodePrice

        [HttpGet]
        [Route("SelectAllProductsCodePrice")]
        public IHttpActionResult SelectAllProductsCodePrice(long ProdId, Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblProducts where x.Id == ProdId && x.TenantId == Tid && x.Status == true select new { Id = x.Id, Name = x.ProductName, ProductCode = x.Code, Price = x.Price}).ToList();
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

        #region For Mobile
        [HttpGet]
        [Route("GetAllProducts")]
        public IHttpActionResult GetAllProducts(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblProducts where x.TenantId == Tid && x.Status == true select x).ToList();
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

        #endregion

        [HttpGet]
        [Route("GetAllProductTypes")]
        public IHttpActionResult GetAllProductTypes(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblProductTypeNews where x.TenantId== Tid select x).ToList();

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

        #endregion

        #region POST
        [HttpPost]
        [Route("Insert")]
        //[ModelValidator]
        public IHttpActionResult Post([FromBody] ProductModel objProductModel)
        {
            DbContextTransaction dbTran = _dbcontext.Database.BeginTransaction();
            using (dbTran)
            {
                try
                {
                      int result;
                    string productType = objProductModel.ProductType;

                    HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                    #region If ModelState is not Valid
                    if (response.StatusCode != HttpStatusCode.OK)
                        return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                    #endregion

                    lock (thisLock)
                    {

                        

                        if (!string.IsNullOrEmpty(productType))
                        {
                             bool isIntString = productType.All(char.IsDigit);
                             if (isIntString)
                             {
                                 objProductModel.ProductType = Convert.ToString(productType);
                             }
                             else
                             {
                                 string ptypename = productType;
                                 var ptype = (from pt in _dbcontext.TblProductTypeNews where pt.ProductType == ptypename && pt.TenantId == objProductModel.TenantId select pt.Id).FirstOrDefault();
                                 objProductModel.ProductType = Convert.ToString(ptype);
                             }
                        }
                        TblProduct objTblProduct = objNIMBOLEMapper.MapModel2Table(objProductModel);
                        _dbcontext.Entry(objTblProduct).State = EntityState.Added;
                        //_dbcontext.TblProducts.Add(objTblProduct);
                        //if (!_dbcontext.TblProducts.Any(u => u.ProductName == objProductModel.ProductName && u.ProductType == objProductModel.ProductType && u.Price == objProductModel.Price))
                        if (!_dbcontext.TblProducts.Any(u => u.ProductName == objProductModel.ProductName && u.ProductType == objProductModel.ProductType && u.TenantId==objProductModel.TenantId))
                        {
                            _dbcontext.SaveChanges();
                            dbTran.Commit();
                        }
                        else
                        {
                            dbTran.Rollback();
                            throw new InvalidOperationException("Record already exists.");
                        }
                        objProductModel = objNIMBOLEMapper.MapTable2Model(objTblProduct);
                    }
                    return Ok(objProductModel);
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
                    dbTran.Rollback();
                    throw raise;
                }
                catch (Exception ex)
                {
                    if (dbTran.UnderlyingTransaction.Connection != null)
                        dbTran.Rollback();
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        ReasonPhrase = ex.Message
                    });
                }
            }
        }

        // InsertProductType
        [HttpPost]
        [Route("InsertProductType")]
        public IHttpActionResult PostInsertProductType([FromBody] ProductTypeModel objProductTypeModel)
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
                    TblProductTypeNew objTblProductType = new TblProductTypeNew();
                    objTblProductType.ProductType = objProductTypeModel.ProductType;
                    objTblProductType.TenantId = objProductTypeModel.TenantId;
                    objTblProductType.CreatedDate = objProductTypeModel.CreatedDate;
                    objTblProductType.ModifiedDate = objProductTypeModel.ModifiedDate;
                    objTblProductType.Status = objProductTypeModel.Status;

                    if (!_dbcontext.TblProductTypeNews.Any(u => u.ProductType == objProductTypeModel.ProductType && u.TenantId==objProductTypeModel.TenantId ))
                    {
                        _dbcontext.TblProductTypeNews.Add(objTblProductType);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        throw new InvalidOperationException("Record already exist.");
                    }
                    //List<ProductTypeModel> lstProductTypeModel = new List<ProductTypeModel>();
                    //var productType = (from p in _dbcontext.TblProductTypes select p).ToList();
                    //foreach (var item in productType)
                    //{
                    //    lstProductTypeModel.Add(new ProductTypeModel() { ProductTypeId = item.Id, ProductType = item.ProductType });
                    //}
                    //return Ok(lstProductTypeModel);

                    //List<ProductTypeModel> lstProductTypeModel = (from pt in _dbcontext.TblProductTypes select new { pt.Id, pt.ProductType })
                    //.Select(m => new ProductTypeModel()
                    //{
                    //    ProductTypeId = m.Id,
                    //    ProductType = m.ProductType
                    //}).ToList();

                    //List<ProductTypeModel> lstProductTypeModel = new List<ProductTypeModel>();
                    var lstProductTypeModel = (from pt in _dbcontext.TblProductTypeNews where pt.TenantId == objProductTypeModel.TenantId select new { ProductTypeId = pt.Id, ProductType = pt.ProductType }).ToList();

                    //.Select(m => new ProductTypeModel()
                    //{
                    //    ProductTypeId = m.Id,
                    //    ProductType = m.ProductType
                    //}).ToList();

                    return Ok(lstProductTypeModel);

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

        [HttpPost]
        [Route("LeadInsert")]
        //[ModelValidator]
        public IHttpActionResult PostLead([FromBody] ProductModel objProductModel)
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
                    TblProduct objTblProduct = objNIMBOLEMapper.MapModel2Table(objProductModel);
                    //if (!_dbcontext.TblProducts.Any(u => u.ProductName == objProductModel.ProductName && u.ProductType == objProductModel.ProductType && u.Price == objProductModel.Price))
                    if (!_dbcontext.TblProducts.Any(u => u.ProductName == objProductModel.ProductName && u.ProductType == objProductModel.ProductType && u.TenantId == objProductModel.TenantId))
                    {
                        _dbcontext.TblProducts.Add(objTblProduct);
                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        throw new InvalidOperationException("Record already exists.");
                    }
                    // objProductModel = objNIMBOLEMapper.MapTable2Model(objTblProduct);

                    //List<TblProduct> lstTblProduct = _dbcontext.TblProducts.Where(cr => cr.Status == true).ToList();
                    //var product = (from c in _dbcontext.TblProducts where c.Status == true select c).ToList();
                    //List<ProductModel> lstProductModel = new List<ProductModel>();
                    //foreach (var item in product)
                    //{
                    //    lstProductModel.Add(new ProductModel() { Id = item.Id, ProductName = item.ProductName, ProductType = item.ProductType, Price = item.Price ?? 0 });
                    //}
                    //return Ok(lstProductModel);


                    //List<ProductModel> lstProductModel = (from p in _dbcontext.TblProducts where p.Status == true select new  {p.Id,p.ProductName,p.ProductType,p.Price})
                    //  .Select(m => new ProductModel()
                    //  {
                    //      Id = m.Id,
                    //      ProductName = m.ProductName,
                    //      ProductType = m.ProductType,
                    //      Price = m.Price ?? 0
                    //  }).ToList();

                    var lstProductModel = (from p in _dbcontext.TblProducts where p.Status == true && p.TenantId == objProductModel.TenantId select new { Id = p.Id, ProductName = p.ProductName, ProductType = p.ProductType, Price = p.Price ?? 0 }).ToList();


                    return Ok(lstProductModel);

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
        [Route("UpdatePrice")]
        public IHttpActionResult UpdatePrice([FromBody] ProductModel objProductModel)
        {
            try
            {
                if (objProductModel != null)
                {
                    //objProductModel.TenantId = objProductModel.TenantId.ToDefaultTenantId();
                    objProductModel.TenantId = objProductModel.TenantId;
                    var queries = (from x in _dbcontext.TblProducts where x.TenantId== objProductModel.TenantId select x).ToList();
                    if (queries == null)
                        throw new InvalidOperationException("Record not Found.");
                    TblProduct query = new TblProduct();
                    query = queries.Where(x => x.TenantId == objProductModel.TenantId && x.Id == objProductModel.Id).FirstOrDefault();
                    query.Code = objProductModel.ProductCode;
                    query.ProductType = objProductModel.ProductTypeId;
                    query.Price = objProductModel.Price;
                    query.ModifiedDate = objProductModel.ModifiedDate.ToDefaultDateIfTooEarly();
                    query.Status = true;
                    _dbcontext.SaveChanges();
                    return Ok(objProductModel);
                }
                throw new InvalidOperationException("Record not Found.");
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
        public IHttpActionResult Edit([FromBody] ProductModel objProductModel)
        {
            DbContextTransaction dbTran = _dbcontext.Database.BeginTransaction();
            using (dbTran)
            {
                try
                {
                    decimal Val;
                    var productVal = decimal.TryParse(objProductModel.ProductType, out Val);
                    if (productVal == true)
                    {
                    long pvalue = Convert.ToInt64(objProductModel.ProductType);
                    var ptype = (from pt in _dbcontext.TblProductTypeNews where pt.Id == pvalue && pt.TenantId==objProductModel.TenantId select pt.Id).FirstOrDefault();
                        objProductModel.ProductType = Convert.ToString(ptype);
                    }
                    if (productVal == false)
                    {
                        string ptypename = objProductModel.ProductType;
                        var ptype = (from pt in _dbcontext.TblProductTypeNews where pt.ProductType == ptypename && pt.TenantId == objProductModel.TenantId select pt.Id).FirstOrDefault();
                        objProductModel.ProductType = Convert.ToString(ptype);
                    }
                    if (objProductModel != null)
                    {
                        TblProduct record = (from p in _dbcontext.TblProducts where p.Id == objProductModel.Id  && p.TenantId==objProductModel.TenantId  select p).FirstOrDefault();
                        if (record == null)
                            throw new InvalidOperationException("Record is not found");
                        if (record.ProductName == objProductModel.ProductName && record.ProductType == objProductModel.ProductType && record.TenantId== objProductModel.TenantId)
                        {

                            record.Code = objProductModel.ProductCode;
                            record.ModifiedDate = DateTime.Now;
                            record.Status = objProductModel.Status;
                            record.Price = objProductModel.Price;
                            record.Comments = objProductModel.Comments;
                            record.ManufacturerName = objProductModel.ManufacturerName;
                            record.ExpiryDate = objProductModel.ExpiryDate;
                            _dbcontext.SaveChanges();
                            dbTran.Commit();
                            return Ok(objProductModel);
                        }
                        else
                        {
                            List<TblProduct> _objProducts = (from p in _dbcontext.TblProducts where p.ProductName == objProductModel.ProductName && p.ProductType == objProductModel.ProductType && p.TenantId== objProductModel.TenantId  select p).ToList();
                            if (_objProducts.Count == 0)
                            {
                                record.ProductType = objProductModel.ProductType;
                                record.ProductName = objProductModel.ProductName;
                                record.Code = objProductModel.ProductCode;
                                record.ModifiedDate = DateTime.Now;
                                record.Status = objProductModel.Status;
                                record.Price = objProductModel.Price;
                                record.Comments = objProductModel.Comments;
                                record.ManufacturerName = objProductModel.ManufacturerName;
                                _dbcontext.SaveChanges();
                                dbTran.Commit();
                                return Ok(objProductModel);
                            }
                            else
                            {
                                var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new StringContent("")
                                };
                                dbTran.Rollback();
                                throw new InvalidOperationException("Record already exists.");
                            }
                        }
                    }
                    throw new InvalidOperationException("Record not Found.");
                }
                catch (Exception ex)
                {
                    dbTran.Rollback();
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        ReasonPhrase = ex.Message
                    });
                }
            }
        }

        [HttpPut]
        [Route("EditById")]
        public IHttpActionResult EditById([FromBody]ProductModel objProductModel, Guid Tid)
        {
            try
            {
                var queries = (from x in _dbcontext.TblProducts where x.TenantId == Tid select x).ToList();
                if (queries == null)
                    throw new InvalidOperationException("Record not Found.");
                var query = queries.Where(x => x.Id == objProductModel.Id && x.Status == true).FirstOrDefault();
                query.Price = objProductModel.Price;
                query.ModifiedDate = DateTime.Now;
                _dbcontext.SaveChanges();
                objProductModel = objNIMBOLEMapper.MapTable2Model(query);
                return Ok(objProductModel);
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
        public IHttpActionResult Delete(int id, bool status)
        {
            try
            {
                var query = _dbcontext.TblProducts.Where(x => x.Id == id).FirstOrDefault();
                if (query != null)
                {
                    query.Status = status;
                    _dbcontext.SaveChanges();
                    return Ok(query);
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

    }
}
