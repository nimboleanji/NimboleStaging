using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Entities;
using NIMBOLE.Common;
using System.Threading.Tasks;
using System.Data.Entity;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Documents")]
    public class DocumentsController : ApiController
    {
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO _objNimboleMapper = new DTO();

        #region GET
        // GET api/document
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("GetDocumentsByActivityId")]
        public async Task<IHttpActionResult> GetDocumentsByActivityId(int iActivityId,Guid Tid)
        {
            try
            {
                //sreedhar changed on 16/10/2015

                //var query = _dbcontext.TblTransDocuments.Where(Tdoc => Tdoc.ActivityId == iActivityId).DefaultIfEmpty()
                //            .Join(_dbcontext.TblDocuments, Tdoc => Tdoc.DocumentId, Doc => Doc.Id, (Tdoc, Doc) => Doc);

                //List<DocumentModel> _objTbldocuments = new List<DocumentModel>();
                //foreach (var item in query)
                //{
                //    _objTbldocuments.Add(new DocumentModel
                //    {
                //        Id = item.Id,
                //        DocumentName = item.DocumentName,
                //        DocumentType = item.DocumentType,
                //        SizeOfDocument = (long)item.SizeOfDocument,
                //        Status = (bool)item.Status,
                //        TenantId = item.TenantId,
                //        URL = item.URL,
                //        UploadedById = (long)item.UploadedById,
                //        UploadDateTime = (DateTime)item.UploadDateTime,
                //        CreatedDate = (DateTime)item.CreatedDate,
                //        ModifiedDate = (DateTime)item.ModifiedDate
                //    });
                //} 

                var _objTbldocuments = await (from DOC in _dbcontext.VWTranDocuments where DOC.ActivityId == iActivityId && DOC.TenantId==Tid select DOC).ToListAsync();
                if (_objTbldocuments != null)
                    return Json(_objTbldocuments);
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

        [HttpGet]
        [Route("GetDocumentsByTransactionId")]
        public async Task<IHttpActionResult> GetDocumentsByTransactionId(int iTransactionId, Guid Tid)
        {
            try
            {


                var _objTbldocuments = await (from DOC in _dbcontext.TblDocuments 
                                              join TDOC in _dbcontext.TblTransInfoDocumets on DOC.Id equals TDOC.DocumentId
                                              where DOC.Id == iTransactionId && DOC.TenantId == Tid select DOC).ToListAsync();
                if (_objTbldocuments != null)
                    return Json(_objTbldocuments);
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



        // GET api/document/5
        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id,Guid Tid)
        {
            try
            {
                var query = _dbcontext.TblDocuments.SingleOrDefault(x => x.Id == id && x.TenantId==Tid);
                if (query == null)
                    throw new InvalidOperationException("Record Not Found");
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
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblDocuments where x.TenantId==Tid select x).ToList();
                if (data != null)
                    return Json(data);
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

        [HttpGet]
        [Route("GetUrlByName")]
        public IHttpActionResult GetUrlByName(string docName, long leadId,Guid Tid)
        {
            try
            {

                //sreedhar changed on 16/10/2015

                //var data = (from x in _dbcontext.TblDocuments join y in _dbcontext.TblTransDocuments on x.Id equals y.DocumentId where x.DocumentName == docName && y.LeadId == leadId select new { x, y }).ToList();

                ////var data = (from x in _dbcontext.TblDocuments where x.DocumentName == docName select x).ToList();
                //List<DocumentModel> _objTbldocuments = new List<DocumentModel>();
                //List<DocumentWithMultipleUrlsModel> _lstTblDocumentWithUrls = new List<DocumentWithMultipleUrlsModel>();
                //List<string> _lstUrl = new List<string>();
                //string strDocumentName = "";
                //foreach (var item in data)
                //{
                //    strDocumentName = (item.x.DocumentName == strDocumentName) ? "" : item.x.DocumentName;

                //    _objTbldocuments.Add(new DocumentModel
                //    {
                //        Id = item.x.Id,
                //        DocumentName = strDocumentName,
                //        DocumentType = item.x.DocumentType,
                //        SizeOfDocument = (long)item.x.SizeOfDocument,
                //        Status = (bool)item.x.Status,
                //        TenantId = item.x.TenantId,
                //        URL = item.x.URL,
                //        UploadedById = (long)item.x.UploadedById,
                //        UploadDateTime = (DateTime)item.x.UploadDateTime,
                //        CreatedDate = (DateTime)item.x.CreatedDate,
                //        ModifiedDate = (DateTime)item.x.ModifiedDate
                //    });

                //    _lstUrl.Add(item.x.URL);
                //    _lstTblDocumentWithUrls.Add(new DocumentWithMultipleUrlsModel
                //    {
                //        DocumentName = strDocumentName,
                //        lstDocumentUrl = _lstUrl
                //    });
                //}

                var _lstTblDocumentWithUrls = (from DOC in _dbcontext.VWTranDocuments where DOC.DocumentName == docName && DOC.LeadId == leadId && DOC.TenantId== Tid select DOC).ToList();

                if (_lstTblDocumentWithUrls != null)
                    return Json(_lstTblDocumentWithUrls);
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

        public DocumentWithMultipleUrlsModel GetUrlByNameInternal(string docName, long leadId,Guid Tid)
        {
            try
            {
                //sreedhar changed on 16/10/2015
                var data = (from x in _dbcontext.TblDocuments join y in _dbcontext.TblTransDocuments on x.Id equals y.DocumentId where x.DocumentName == docName && y.LeadId == leadId && x.TenantId==Tid select new { x, y }).ToList();
                //var data = (from x in _dbcontext.TblDocuments where x.DocumentName == docName select x).ToList();
                List<DocumentModel> _objTbldocuments = new List<DocumentModel>();
                DocumentWithMultipleUrlsModel _objTblDocumentWithUrls = new DocumentWithMultipleUrlsModel();
                List<string> _lstUrl = new List<string>();
                List<long> _lstUrlBasedId = new List<long>();

                foreach (var item in data)
                {
                    _lstUrlBasedId.Add(item.x.Id);
                    _lstUrl.Add(item.x.URL);
                }
                _objTblDocumentWithUrls.lstUrlBasedId = _lstUrlBasedId;
                _objTblDocumentWithUrls.DocumentName = docName;
                _objTblDocumentWithUrls.lstDocumentUrl = _lstUrl;
                //DocumentWithMultipleUrlsModel _objTblDocumentWithUrls = new DocumentWithMultipleUrlsModel();
                // _objTblDocumentWithUrls = (from DOC in _dbcontext.VWTranDocuments where DOC.DocumentName == docName && DOC.LeadId == leadId select DOC).ToList();
                if (_objTblDocumentWithUrls != null)
                    return (_objTblDocumentWithUrls);
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

        [HttpGet]
        [Route("GetAllDocumentByLeadId")]
        public async Task<IHttpActionResult> GetAllDocumentByLeadId(Int64 iLeadId,Guid Tid)
        {
            try
            {
                //sreedhar changed on 16/10/2015
                //var data = (from x in _dbcontext.TblTransDocuments
                //            join d in _dbcontext.TblDocuments on x.DocumentId equals d.Id
                //            where x.LeadId == iLeadId && x.ActivityId == 0
                //            select new
                //            {
                //                d.DocumentName
                //            }).Distinct().ToList();

                var data = await (from DOC in _dbcontext.VWTranDocuments where DOC.LeadId == iLeadId && DOC.ActivityId == 0 && DOC.TenantId==Tid select DOC).ToListAsync();
                List<DocumentModel> lstDocumentModel = new List<DocumentModel>();
                List<DocumentWithMultipleUrlsModel> lstDocumentWithUrlsModel = new List<DocumentWithMultipleUrlsModel>();
                foreach (var objTblDocument in data)
                {
                    lstDocumentWithUrlsModel.Add(GetUrlByNameInternal(objTblDocument.DocumentName, iLeadId,Tid));
                }
                if (lstDocumentWithUrlsModel != null)
                    return Json(lstDocumentWithUrlsModel);
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

        [HttpGet]
        [Route("GetDistinctDocuments")]
        public IHttpActionResult GetDistinctDocuments()
        {
            try
            {
                //var data = _dbcontext.TblDocuments.GroupBy(x => x.DocumentName).Select(y => y.FirstOrDefault());               
                //List<DocumentModel> _objTbldocuments = new List<DocumentModel>();
                //foreach (var item in data)
                //{
                //    _objTbldocuments.Add(new DocumentModel
                //    {
                //        Id = item.Id,
                //        DocumentName = item.DocumentName,
                //        DocumentType = item.DocumentType,
                //        SizeOfDocument = (long)item.SizeOfDocument,
                //        Status = (bool)item.Status,
                //        TenantId = item.TenantId,
                //        URL = item.URL,
                //        UploadedById = (long)item.UploadedById,
                //        UploadDateTime = (DateTime)item.UploadDateTime,
                //        CreatedDate = (DateTime)item.CreatedDate,
                //        ModifiedDate = (DateTime)item.ModifiedDate
                //    });
                //}
                var _objTbldocuments = (from DOC in _dbcontext.VWTranDocuments select DOC).ToList();
                if (_objTbldocuments != null)
                    return Json(_objTbldocuments);
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
        [ModelValidator]
        public IHttpActionResult Post([FromBody] DocumentModel objDocumentModel, long LeadId, long ActivityId = 0)
        {
            try
            {
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                TblDocument objDocTable = new TblDocument();
                if (!_dbcontext.TblDocuments.Any(u => u.DocumentName == objDocumentModel.DocumentName && u.TenantId == objDocumentModel.TenantId))
                {
                    if (objDocumentModel.URLs != null)
                    {
                        foreach (var item in objDocumentModel.URLs)
                        {
                            TblDocument objTblDocument = new TblDocument();
                            objTblDocument.TenantId = objDocumentModel.TenantId;
                            objTblDocument.URL = item;
                            objTblDocument.DocumentName = objDocumentModel.DocumentName;
                            //string CurrentUser = objDocumentModel.UploadedById;
                            //var CurrentUserId = _dbcontext.TblEmployees.Where(a => a.EmployeeEmail == CurrentUser).Select(a => a.Id).FirstOrDefault();
                            objTblDocument.UploadedById = objDocumentModel.UploadedById;
                            objTblDocument.UploadDateTime = objDocumentModel.CreatedDate.ToDefaultDateIfTooEarly();
                            objTblDocument.DocumentType = objDocumentModel.DocumentType;
                            objTblDocument.SizeOfDocument = 0;
                            objTblDocument.CreatedDate = objDocumentModel.CreatedDate.ToDefaultDateIfTooEarly();
                            objTblDocument.ModifiedDate = objDocumentModel.CreatedDate.ToDefaultDateIfTooEarly();
                            objTblDocument.Status = true;
                            _dbcontext.TblDocuments.Add(objTblDocument);
                            _dbcontext.SaveChanges();

                            TblTransDocument objTblTransDocument = new TblTransDocument();
                            objTblTransDocument.TenantId = objDocumentModel.TenantId;
                            objTblTransDocument.DocumentId = objTblDocument.Id;
                            objTblTransDocument.ActivityId = ActivityId;
                            objTblTransDocument.LeadId = LeadId;
                            objTblTransDocument.CreatedDate = objDocumentModel.CreatedDate.ToDefaultDateIfTooEarly();
                            objTblTransDocument.ModifiedDate = objDocumentModel.CreatedDate.ToDefaultDateIfTooEarly();
                            objTblTransDocument.Status = true;
                            _dbcontext.TblTransDocuments.Add(objTblTransDocument);
                            _dbcontext.SaveChanges();
                        }
                    }
                    return Json(objDocumentModel);
                }
                else
                {
                    TblDocument _objTblDocument = _dbcontext.TblDocuments.Where(u => u.DocumentName == objDocumentModel.DocumentName).FirstOrDefault();
                    if (_objTblDocument.Status == false)
                        throw new InvalidOperationException("Record already exist.");
                }
                return null;
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
        public IHttpActionResult Edit([FromBody] TblDocument entity)
        {
            try
            {
               // var queries = (from x in _dbcontext.TblDocuments select x).ToList();
                var queries = (from x in _dbcontext.TblDocuments where x.Id == entity.Id select x).FirstOrDefault();

                if (queries != null)
                {
                   // TblDocument query = new TblDocument();
                    //query = queries.Where(x => x.Id == entity.Id).FirstOrDefault();                
                    queries.URL = entity.URL;
                    queries.DocumentName = entity.DocumentName;
                    queries.DocumentType = entity.DocumentType;
                    queries.SizeOfDocument = entity.SizeOfDocument;
                   // queries.CreatedDate = entity.CreatedDate;
                   // queries.ModifiedDate = entity.ModifiedDate;
                    queries.ModifiedDate = DateTime.Now;
                   // queries.Status = entity.Status;
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
        [Route("Delete")]
        public IHttpActionResult Delete(Int64 Id)
        {
            try
            {
                var query = _dbcontext.TblDocuments.Where(x => x.Id == Id).FirstOrDefault();
                if (query != null)
                {
                    _dbcontext.TblDocuments.Remove(query);
                    //query.Status = false;
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
