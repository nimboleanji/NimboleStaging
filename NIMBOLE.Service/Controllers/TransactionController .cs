using NIMBOLE.Models.Mappers;
using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NIMBOLE.Entities;
using System.Threading.Tasks;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Transaction")]
    public class TransactionController : ApiController
    {
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        #region GET's
        // GetAll api/Transaction
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {


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

        // GetById


        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(long Id,Guid Tid)
        {
            try
            {
                var lstTransactionByLeadId = (from TS in _dbcontext.TblTransactionInfoes
                                              where TS.Status == true && TS.Id == Id && TS.TenantId==Tid
                                              select TS).FirstOrDefault();
                LeadTransactionInfoModel objLeadTransactionInfoModel = new LeadTransactionInfoModel();

                if (lstTransactionByLeadId == null)
                    throw new InvalidOperationException("Record not found.");
                else
                {
                    objLeadTransactionInfoModel.Id = lstTransactionByLeadId.Id;
                    objLeadTransactionInfoModel.LeadId = lstTransactionByLeadId.LeadId;
                    objLeadTransactionInfoModel.TransactionId = lstTransactionByLeadId.Id;
                    objLeadTransactionInfoModel.PlateNumber = lstTransactionByLeadId.PlateNo;
                    objLeadTransactionInfoModel.ProductId = lstTransactionByLeadId.ObjectId;
                    objLeadTransactionInfoModel.TenorScheme = lstTransactionByLeadId.TenorScheme.ToString();
                    objLeadTransactionInfoModel.TransName = lstTransactionByLeadId.TransactionName;
                    objLeadTransactionInfoModel.BPKBNumber = lstTransactionByLeadId.BPKBNumber;
                    objLeadTransactionInfoModel.Address = lstTransactionByLeadId.Address;
                    objLeadTransactionInfoModel.Status = Convert.ToBoolean(lstTransactionByLeadId.Status);


                    return Json(objLeadTransactionInfoModel);
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
        [Route("GetTransactionByLeadId")]
        public IHttpActionResult GetTransactionByLeadId(long iLeadId, Guid Tid)
        {
            try
            {
                
                var lstTransactionByLeadId = (from TS in _dbcontext.TblTransactionInfoes
                                              where TS.Status == true && TS.LeadId == iLeadId && TS.TenantId == Tid
                                              select TS).FirstOrDefault();

                LeadTransactionInfoModel objLeadTransactionInfoModel = new LeadTransactionInfoModel();

                if (lstTransactionByLeadId == null)
                    throw new InvalidOperationException("Record not found.");
                else
                {

                    objLeadTransactionInfoModel.TenantId = lstTransactionByLeadId.TenantId;
                    objLeadTransactionInfoModel.Id = lstTransactionByLeadId.Id;
                    objLeadTransactionInfoModel.LeadId = lstTransactionByLeadId.LeadId;
                    objLeadTransactionInfoModel.TransactionId = lstTransactionByLeadId.Id;
                    objLeadTransactionInfoModel.PlateNumber = lstTransactionByLeadId.PlateNo;
                    objLeadTransactionInfoModel.ProductId = lstTransactionByLeadId.ObjectId;
                    objLeadTransactionInfoModel.TenorScheme = lstTransactionByLeadId.TenorScheme.ToString();
                    objLeadTransactionInfoModel.TransName = lstTransactionByLeadId.TransactionName;
                    objLeadTransactionInfoModel.BPKBNumber = lstTransactionByLeadId.BPKBNumber;
                    objLeadTransactionInfoModel.Address = lstTransactionByLeadId.Address;
                    objLeadTransactionInfoModel.Status = Convert.ToBoolean(lstTransactionByLeadId.Status);



                    //var lstDocIds = (from ltd in _dbcontext.TblTransInfoDocumets where ltd.TransactionId == objLeadTransactionInfoModel.Id select ltd.DocumentId).ToList();


                    //var lstDoc = (from d in _dbcontext.TblDocuments where lstDocIds.Contains(d.Id) select d).ToList();


                    //List<DocumentModel> _objTbldocuments = new List<DocumentModel>();
                    ////foreach (var item in lstDoc)
                    ////{
                    ////    _objTbldocuments.Add(new DocumentModel
                    ////    {
                    ////        Id = item.Id,
                    ////        DocumentName = item.DocumentName,
                    ////        DocumentType = item.DocumentType,
                    ////        SizeOfDocument = (long)item.SizeOfDocument,
                    ////        Status = (bool)item.Status,
                    ////        TenantId = item.TenantId,
                    ////        URL = item.URL,
                    ////        UploadedById = (long)item.UploadedById,
                    ////        UploadDateTime = (DateTime)item.UploadDateTime,
                    ////        CreatedDate = (DateTime)item.CreatedDate,
                    ////        ModifiedDate = (DateTime)item.ModifiedDate
                    ////    });
                    ////}


                   // return Json(objLeadTransactionInfoModel);

                    return Ok(objLeadTransactionInfoModel);
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
        [Route("GetDocumentsByLeadId")]
        public IHttpActionResult GetDocumentsByLeadId(int iLeadId, int TransId,Guid Tid)
        {
            try
            {

                var lstDocIds = (from ltd in _dbcontext.TblTransInfoDocumets where ltd.TransactionId == TransId && ltd.TenantId==Tid select ltd.DocumentId).ToList();


                var lstDoc = (from d in _dbcontext.TblDocuments where lstDocIds.Contains(d.Id) select d).ToList();

               // var grpDoc = lstDoc.GroupBy(g => g.DocumentName).ToList();

                //var results = from p in _dbcontext.TblDocuments
                //              group p.DocumentName by p.DocumentName into g
                //              select new { Id = g.Key, D = g.ToList() };

                if (lstDoc != null)
                {
                    return Json(lstDoc);
                }
                else
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

        #region POST

        [HttpPost]
        [Route("Insert")]
        // [ModelValidator]
        public IHttpActionResult Post([FromBody]LeadModel _objLeadModel)
        {
            try
            {

                List<string> urls = _objLeadModel.objLeadTransactionInfoModel.lstURLs;

                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                TblTransactionInfo objTblTransactionInfo = new TblTransactionInfo();

                objTblTransactionInfo.TenantId = _objLeadModel.objLeadTransactionInfoModel.TenantId;
                objTblTransactionInfo.LeadId = _objLeadModel.objLeadTransactionInfoModel.LeadId;
                objTblTransactionInfo.TransactionName = _objLeadModel.objLeadTransactionInfoModel.TransName;
                objTblTransactionInfo.Address = _objLeadModel.objLeadTransactionInfoModel.Address;
                objTblTransactionInfo.PlateNo = _objLeadModel.objLeadTransactionInfoModel.PlateNumber;
                objTblTransactionInfo.ObjectId = _objLeadModel.objLeadTransactionInfoModel.ProductId;
                objTblTransactionInfo.BPKBNumber = _objLeadModel.objLeadTransactionInfoModel.BPKBNumber;
                objTblTransactionInfo.TenorScheme = Convert.ToInt32(_objLeadModel.objLeadTransactionInfoModel.TenorScheme);
                objTblTransactionInfo.CreatedDate = DateTime.Now.Date;
                objTblTransactionInfo.ModifiedDate = DateTime.Now.Date;
                objTblTransactionInfo.Status = true;

                _dbcontext.TblTransactionInfoes.Add(objTblTransactionInfo);
                _dbcontext.SaveChanges();


                if (objTblTransactionInfo.Id > 0)
                {

                    if (urls != null)
                    {
                        foreach (var item in urls)
                        {
                            TblDocument objTblDocument = new TblDocument();
                            objTblDocument.TenantId = objTblTransactionInfo.TenantId;
                            objTblDocument.URL = item;
                            objTblDocument.DocumentName = objTblTransactionInfo.TransactionName;
                            string CurrentUser = _objLeadModel.objLeadTransactionInfoModel.currentUser;
                            var CurrentUserId = _dbcontext.TblEmployees.Where(a => a.EmployeeEmail == CurrentUser && a.TenantId == _objLeadModel.objLeadTransactionInfoModel.TenantId ).Select(a => a.Id).FirstOrDefault();
                            objTblDocument.UploadedById = CurrentUserId;
                            objTblDocument.UploadDateTime = DateTime.Now;
                            objTblDocument.DocumentType = objTblDocument.DocumentType;
                            objTblDocument.SizeOfDocument = 0;
                            objTblDocument.CreatedDate = DateTime.Now;
                            objTblDocument.ModifiedDate = DateTime.Now;
                            objTblDocument.Status = true;
                            _dbcontext.TblDocuments.Add(objTblDocument);
                            _dbcontext.SaveChanges();

                            TblTransInfoDocumet objTblTransInfoDocumet = new TblTransInfoDocumet();

                            objTblTransInfoDocumet.TransactionId = objTblTransactionInfo.Id;                           
                            objTblTransInfoDocumet.LeadId = _objLeadModel.objLeadTransactionInfoModel.LeadId;
                            objTblTransInfoDocumet.DocumentId = objTblDocument.Id;
                            objTblTransInfoDocumet.TenantId = objTblTransactionInfo.TenantId;
                            objTblTransInfoDocumet.CreatedDate = DateTime.Now;
                            objTblTransInfoDocumet.ModifiedDate = DateTime.Now;
                            objTblTransInfoDocumet.Status = true;
                            _dbcontext.TblTransInfoDocumets.Add(objTblTransInfoDocumet);
                            _dbcontext.SaveChanges();

                        }
                    }
                }
                _objLeadModel.objLeadTransactionInfoModel.Id = objTblTransactionInfo.Id;

                return Ok(_objLeadModel);
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
        [Route("InsertEdit")]
        // [ModelValidator]
        public IHttpActionResult InsertEdit([FromBody]LeadTransactionInfoModel _objLeadTransactionInfoModel)
        {
            try
            {

                List<string> urls = _objLeadTransactionInfoModel.lstURLs;

                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                TblTransactionInfo objTblTransactionInfo = new TblTransactionInfo();

                objTblTransactionInfo.TenantId = _objLeadTransactionInfoModel.TenantId;
                objTblTransactionInfo.LeadId = _objLeadTransactionInfoModel.LeadId;
                objTblTransactionInfo.TransactionName = _objLeadTransactionInfoModel.TransName;
                objTblTransactionInfo.Address = _objLeadTransactionInfoModel.Address;
                objTblTransactionInfo.PlateNo = _objLeadTransactionInfoModel.PlateNumber;
                objTblTransactionInfo.ObjectId = _objLeadTransactionInfoModel.ProductId;
                objTblTransactionInfo.BPKBNumber = _objLeadTransactionInfoModel.BPKBNumber;
                objTblTransactionInfo.TenorScheme = Convert.ToInt32(_objLeadTransactionInfoModel.TenorScheme);
                objTblTransactionInfo.CreatedDate = DateTime.Now.Date;
                objTblTransactionInfo.ModifiedDate = DateTime.Now.Date;
                objTblTransactionInfo.Status = true;

                _dbcontext.TblTransactionInfoes.Add(objTblTransactionInfo);
                _dbcontext.SaveChanges();


                if (objTblTransactionInfo.Id > 0)
                {

                    if (urls != null)
                    {
                        foreach (var item in urls)
                        {
                            TblDocument objTblDocument = new TblDocument();
                            objTblDocument.TenantId = objTblTransactionInfo.TenantId;
                            objTblDocument.URL = item;
                            objTblDocument.DocumentName = objTblTransactionInfo.TransactionName;
                            string CurrentUser = _objLeadTransactionInfoModel .currentUser;
                            var CurrentUserId = _dbcontext.TblEmployees.Where(a => a.EmployeeEmail == CurrentUser && a.TenantId == _objLeadTransactionInfoModel.TenantId ).Select(a => a.Id).FirstOrDefault();
                            objTblDocument.UploadedById = CurrentUserId;
                            objTblDocument.UploadDateTime = DateTime.Now;
                            objTblDocument.DocumentType = objTblDocument.DocumentType;
                            objTblDocument.SizeOfDocument = 0;
                            objTblDocument.CreatedDate = DateTime.Now;
                            objTblDocument.ModifiedDate = DateTime.Now;
                            objTblDocument.Status = true;
                            _dbcontext.TblDocuments.Add(objTblDocument);
                            _dbcontext.SaveChanges();

                            TblTransInfoDocumet objTblTransInfoDocumet = new TblTransInfoDocumet();

                            objTblTransInfoDocumet.TransactionId = objTblTransactionInfo.Id;                           
                            objTblTransInfoDocumet.LeadId = _objLeadTransactionInfoModel.LeadId;
                            objTblTransInfoDocumet.DocumentId = objTblDocument.Id;
                            objTblTransInfoDocumet.TenantId = objTblTransactionInfo.TenantId;
                            objTblTransInfoDocumet.CreatedDate = DateTime.Now;
                            objTblTransInfoDocumet.ModifiedDate = DateTime.Now;
                            objTblTransInfoDocumet.Status = true;
                            _dbcontext.TblTransInfoDocumets.Add(objTblTransInfoDocumet);
                            _dbcontext.SaveChanges();

                        }
                    }
                }
                _objLeadTransactionInfoModel.Id = objTblTransactionInfo.Id;

                return Ok(_objLeadTransactionInfoModel);
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
        public IHttpActionResult Edit([FromBody] LeadTransactionInfoModel objLeadTransactionInfoModel)
        {
            try
            {
                List<string> urls = objLeadTransactionInfoModel.lstURLs;

                var queries = (from x in _dbcontext.TblTransactionInfoes where x.LeadId==objLeadTransactionInfoModel.LeadId && x.TenantId== objLeadTransactionInfoModel.TenantId select x ).FirstOrDefault();
                if (queries != null)
                {
                    queries.TenantId = objLeadTransactionInfoModel.TenantId;
                    queries.TransactionName = objLeadTransactionInfoModel.TransName;
                    queries.ObjectId = objLeadTransactionInfoModel.ProductId;
                    queries.Address = objLeadTransactionInfoModel.Address;
                    queries.BPKBNumber = objLeadTransactionInfoModel.BPKBNumber;
                    queries.PlateNo = objLeadTransactionInfoModel.PlateNumber;
                    queries.TenorScheme = Convert.ToInt32(objLeadTransactionInfoModel.TenorScheme);
                    queries.ModifiedDate = DateTime.Now.Date;
                    queries.Status = true;
                    _dbcontext.SaveChanges();

                    if (urls != null)
                    {
                        foreach (var item in urls)
                        {
                            TblDocument objTblDocument = new TblDocument();
                            objTblDocument.TenantId = queries.TenantId;
                            objTblDocument.URL = item;
                            objTblDocument.DocumentName = objLeadTransactionInfoModel.TransName;
                            string CurrentUser = objLeadTransactionInfoModel.currentUser;
                            var CurrentUserId = _dbcontext.TblEmployees.Where(a => a.EmployeeEmail == CurrentUser).Select(a => a.Id).FirstOrDefault();
                            objTblDocument.UploadedById = CurrentUserId;
                            objTblDocument.UploadDateTime = DateTime.Now;
                            objTblDocument.DocumentType = objTblDocument.DocumentType;
                            objTblDocument.SizeOfDocument = 0;
                            objTblDocument.CreatedDate = DateTime.Now;
                            objTblDocument.ModifiedDate = DateTime.Now;
                            objTblDocument.Status = true;
                            _dbcontext.TblDocuments.Add(objTblDocument);
                            _dbcontext.SaveChanges();

                            TblTransInfoDocumet objTblTransInfoDocumet = new TblTransInfoDocumet();
                            objTblTransInfoDocumet.TenantId = objLeadTransactionInfoModel.TenantId;
                            objTblTransInfoDocumet.TransactionId = queries.Id;
                            objTblTransInfoDocumet.LeadId = queries.LeadId;
                            objTblTransInfoDocumet.DocumentId = objTblDocument.Id;
                            objTblTransInfoDocumet.TenantId = queries.TenantId;
                            objTblTransInfoDocumet.CreatedDate = DateTime.Now;
                            objTblTransInfoDocumet.ModifiedDate = DateTime.Now;
                            objTblTransInfoDocumet.Status = true;
                            _dbcontext.TblTransInfoDocumets.Add(objTblTransInfoDocumet);
                            _dbcontext.SaveChanges();

                        }
                    }


                    return Ok(objLeadTransactionInfoModel);
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

        [HttpPut]
        [Route("UpdateStatus")]
        public IHttpActionResult UpdateStatus([FromBody] TblActivity entity, long Id, bool Status)
        {
            try
            {
                var record = (from x in _dbcontext.TblActivities where x.Id == Id select x).FirstOrDefault();
                if (record != null)
                {
                    record.Status = Status;
                    _dbcontext.SaveChanges();
                    return Ok(entity);
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




        #region DELETE
        [HttpDelete]
        [Route("DeleteById")]
        public IHttpActionResult DeleteById(long id)
        {
            try
            {
                var query = _dbcontext.TblActivities.Where(x => x.Id == id).FirstOrDefault();
                if (query != null)
                {
                    _dbcontext.TblActivities.Remove(query);
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
