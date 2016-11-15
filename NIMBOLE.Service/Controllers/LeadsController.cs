using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;
using NIMBOLE.Models;
using NIMBOLE.Models.Mappers;
using System.Data.Entity.Core;
using NIMBOLE.Entities;
using System.Data.Entity.Infrastructure;
using System.Web.Helpers;
using Newtonsoft.Json;
using NIMBOLE.Models.Models.Transactions;
using NIMBOLE.Models.Models.Transactions.TransUser;

using Newtonsoft.Json.Linq;

using System.Web.Script.Serialization;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Leads")]
    public class LeadsController : ApiController
    {
        private static Object thisLock = new Object();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        #region GET
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(int id, string Mstid, string resHome, long EmpId, Guid Tid)
        {
            try
            {
                dynamic lstLeadModels;
                // var strQuery1 = (from L in _dbcontext.VWLeadLsEmps where L.TenantId == Tid select L).ToList();
                string query = string.Empty;
                query = "exec sp_SubordinateIdsByEmpIdAndRoleOrder " + EmpId;
                var data = _dbcontext.Database.SqlQuery<long>(query).ToList();
                //var strQuery = (from L in _dbcontext.TblLeads join MSS in _dbcontext.TblMileStoneStages on L.LeadStage.ToString() equals MSS.Id.ToString() join M in _dbcontext.TblMileStones on L.LeadStatus equals M.Id.ToString() join E in _dbcontext.TblEmployees on L.LeadOwnerId equals E.Id join LS in _dbcontext.TblLeadSources on L.LeadSourceId equals LS.Id into t from rt in t.DefaultIfEmpty() where L.TenantId == Tid && data.Contains(L.LeadOwnerId ?? 0) select new { Id = L.Id, TenantId = L.TenantId, LeadTitle = L.LeadTitle, LeadDescription = L.LeadDescription, LeadOwnerId = L.LeadOwnerId, LeadSourceId = L.LeadSourceId, LeadType = L.LeadType, Budget = L.Budget, LeadStatus = L.LeadStatus, Location = L.Location, TimeFrame = L.TimeFrame, AccountId = L.AccountId, Size = L.Size, CreatedDate = L.CreatedDate, ModifiedDate = L.ModifiedDate, Status = L.Status, LeadStage = L.LeadStage, MasterMilestoneStage = MSS.MileStoneStage, MilestoneName = M.Description, LeadOwnerName = E.LastName == null || E.LastName == "" ? E.FirstName : E.FirstName + " " + E.LastName }).ToList();
                var strQuery = (from L in _dbcontext.VWLeadLsEmps where L.TenantId == Tid && data.Contains(L.LeadOwnerId ?? 0) orderby L.Id descending select new { Id = L.Id, TenantId = L.TenantId, LeadTitle = L.LeadTitle, LeadDescription = L.LeadDescription, LeadOwnerId = L.LeadOwnerId, LeadSourceId = L.LeadSourceId, LeadType = L.LeadType, Budget = L.Budget, LeadStatus = L.LeadStatus, Location = L.Location, TimeFrame = L.TimeFrame, AccountId = L.AccountId, Size = L.Size, CreatedDate = L.CreatedDate, ModifiedDate = L.ModifiedDate, Status = L.Status, LeadStage = L.LeadStage, MasterMilestoneStage = L.MilestoneStage, MilestoneName = L.Milestone, LeadOwnerName = L.FirstName + " " + L.LastName }).ToList();
                // var strQuery1 = "SELECT * FROM [VWLeadLsEmp] WHERE [TenantId] = '" + Tid + "'";
                if (Mstid != "0")
                {
                    //strQuery1 = strQuery1 + " AND [LeadStatus] = '" + Mstid + "'";                
                    //  lstLeadModels = _dbcontext.Database.SqlQuery<LeadModel>(strQuery1).ToList<LeadModel>();
                    lstLeadModels = (from M in strQuery where M.LeadStatus == Mstid select M).ToList();
                    return Json(lstLeadModels);
                }
                else if (id != 0)
                {
                    //strQuery1 = strQuery1 + " AND Id =" + id.ToString();
                    //lstLeadModels = _dbcontext.Database.SqlQuery<LeadModel>(strQuery1).ToList<LeadModel>();
                    lstLeadModels = (from I in strQuery where I.Id == id select I).ToList();
                    return Json(lstLeadModels);
                }
                else if (!string.IsNullOrEmpty(resHome))
                {
                    if (resHome == "2")
                    {
                        //strQuery1 = strQuery1 + " AND convert(varchar(10),createddate,112) = convert(varchar(10),getdate(),112)";
                        //lstLeadModels = _dbcontext.Database.SqlQuery<LeadModel>(strQuery1).ToList<LeadModel>();
                        lstLeadModels = (from HL in strQuery where DateTime.Compare(HL.CreatedDate.Value.Date, DateTime.Now.Date) == 0 select HL).ToList();
                        return Json(lstLeadModels);
                    }
                    if (resHome == "3")
                    {
                        //strQuery1 = strQuery1 + " AND convert(varchar(10),createddate,112) = convert(varchar(10),getdate(),112) AND LeadStage = 1";
                        //lstLeadModels = _dbcontext.Database.SqlQuery<LeadModel>(strQuery1).ToList<LeadModel>();
                        lstLeadModels = (from HL in strQuery where DateTime.Compare(HL.CreatedDate.Value.Date, DateTime.Now.Date) == 0 && HL.LeadStage == 1 select HL).ToList();
                        return Json(lstLeadModels);
                    }
                    if (resHome == "4")
                    {
                        //strQuery1 = strQuery1 + " AND convert(varchar(10),createddate,112) = convert(varchar(10),getdate(),112) AND LeadStage = 2";
                        //lstLeadModels = _dbcontext.Database.SqlQuery<LeadModel>(strQuery1).ToList<LeadModel>();
                        lstLeadModels = (from HL in strQuery where DateTime.Compare(HL.CreatedDate.Value.Date, DateTime.Now.Date) == 0 && HL.LeadStage == 2 select HL).ToList();
                        return Json(lstLeadModels);
                    }
                    return null;
                }
                else
                {
                    //lstLeadModels = _dbcontext.Database.SqlQuery<LeadModel>(strQuery1).ToList<LeadModel>();
                    lstLeadModels = (from L in strQuery select L).ToList();
                    return Json(lstLeadModels);
                }
                // sreedhar changed on 16/10/2015

                //  lstLeadModel=(from L in _dbcontext.vw);
                //if (id != 0)            
                //    data = (from L in _dbcontext.TblLeads where L.Id == id join LS in _dbcontext.TblLeadSources on L.LeadSourceId equals LS.Id join E in _dbcontext.TblEmployees on L.LeadOwnerId equals E.Id where L.Status == true select new { L, LS, E }).ToList();            
                //else
                //    data = (from L in _dbcontext.TblLeads join LS in _dbcontext.TblLeadSources on L.LeadSourceId equals LS.Id join E in _dbcontext.TblEmployees on L.LeadOwnerId equals E.Id where L.Status == true select new { L, LS, E }).ToList();
                //if (data != null)
                //{
                //    List<LeadModel> lstLeadModel = new List<LeadModel>();
                //    foreach (var item in data)
                //    {
                //        lstLeadModel.Add(new LeadModel()
                //        {
                //            Id = item.L.Id,
                //            LeadTitle = item.L.LeadTitle,
                //            LeadStatus = item.L.LeadStatus,
                //            LeadSourceId = item.L.LeadSourceId,
                //            LeadSourceName = item.LS.Description,
                //            LeadDescription = item.L.LeadDescription,
                //            LeadType = item.L.LeadType,
                //            LeadOwnerId = Convert.ToInt64(item.L.LeadOwnerId),
                //            LeadOwnerName = item.E.FirstName,
                //            TimeFrame = item.L.TimeFrame,
                //            Size = item.L.Size ?? 0,
                //            Budget = item.L.Budget ?? 0,
                //            AccountId = item.L.AccountId,
                //            Location = item.L.Location,
                //            TenantId = item.L.TenantId,
                //            CreatedDate = Convert.ToDateTime(item.L.CreatedDate),
                //            ModifiedDate = Convert.ToDateTime(item.L.ModifiedDate),
                //            Status = Convert.ToBoolean(item.L.Status)
                //        });
                //    }

                //    return Json(lstLeadModel);
                // }
                //   return null;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        //[HttpGet]
        //[Route("GetAllNew")]
        //public HttpResponseMessage GetAllNew(long EmpId,Guid Tid)
        //{
        //    string query = string.Empty;
        //    string procedureQuery = string.Empty;
        //    int recordsCount = 0;

        //    procedureQuery = string.Format("sp_GetLeads @PageSize = {0}, @PageNumber= {1},@RecordCount = {2}", 10, 1, recordsCount);
        //    var affectedRows = _dbcontext.Database.SqlQuery<LeadModel>(procedureQuery);            
        //    return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { affectedRows, recordsCount });

        //}

        [HttpGet]
        [Route("GetAllForReports")]
        public IHttpActionResult GetAllForReports(int id = 0)
        {
            try
            {
                dynamic lstLeadModel;
                lstLeadModel = (from L in _dbcontext.VWLeadLsEmps select L).ToList();
                if (id != 0)
                {
                    lstLeadModel = (from L in _dbcontext.VWLeadLsEmps where L.Id == id select L).ToList();
                }
                if (lstLeadModel != null)
                {
                    return Json(lstLeadModel);
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
        [Route("GetActualVsExpectedForReports")]
        public IHttpActionResult GetActualVsExpectedForReports(int EmpId = 0, string FinYear = "")
        {
            try
            {
                string query = string.Empty;
                query = "exec sp_TargetVsActual " + EmpId + ",'" + FinYear + "'";

                var data = _dbcontext.Database.SqlQuery<ActualVsExpectedModel>(query).ToList();
                return Json(data);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetSalesByMilestone")]
        //For SalesFunel report
        public IHttpActionResult GetSalesByMilestone()
        {
            try
            {
                string query = string.Empty;
                query = "exec sp_SalesFunnel";
                var data = _dbcontext.Database.SqlQuery<SalesFunelModel>(query).ToList();
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

        public IHttpActionResult GetLeadsByMilestone(string milestone)
        {
            try
            {
                string query = string.Empty;
                query = @"SELECT distinct
	                        L.Id
	                        ,L.TenantId
	                        ,L.LeadTitle
	                        ,L.LeadDescription
	                        ,L.LeadOwnerId
	                        ,L.LeadSourceId
	                        ,L.LeadType LeadTypeId
	                        ,L.Budget
	                        ,L.LeadStatus
	                        ,L.TimeFrame
	                        ,L.AccountId
	                        ,L.Size
	                        ,L.CreatedDate
	                        ,L.ModifiedDate
	                        ,L.LeadStage
	                        ,L.LeadEmployees
	                        --,A.Id ActivityId
	                        --,A.Title
	                        --,A.Type_A_E_M
	                        --,A.Comments
	                        --,A.MilestoneId
	                        --,A.ActivityDate
	                        --,A.CreatedDate
	                        --,A.ModifiedDate
	                        ,M.Code MilestoneCode
	                        ,M.Description MilestoneName
                        FROM TblLead L
	                        JOIN TblTransLead TL ON L.Id = TL.LeadId
	                        JOIN TblActivity A ON A.Id = TL.ActivityId
	                        JOIN TblMilestone M ON M.Id = A.MileStoneId
                        WHERE A.Status = 1 AND M.Status = 1 AND A.Type_A_E_M = 'Activity'";
                if (!string.IsNullOrEmpty(milestone))
                    query = query + " M.Description = " + milestone;
                var data = _dbcontext.Database.SqlQuery<SalesFunelModel>(query).ToList();
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
        [Route("GetAllOpportunity")]
        public IHttpActionResult GetAllOpportunity(string LeadOwner, Guid Tid)
        {
            try
            {
                Int64 leadowner = Convert.ToInt64(LeadOwner);
                var innerQuery = (from m in _dbcontext.TblMileStones where m.Code == "OpCode" && m.TenantId == Tid select m.Id.ToString()).ToList();
                var lstLeadModel = (from L in _dbcontext.VWLeadLsEmps where L.LeadOwnerId == leadowner && L.TenantId == Tid select L).ToList();
                if (lstLeadModel != null)
                {
                    return Json(lstLeadModel);
                }
                return null;

                //sreedhar changed on 16/10/2015
                //var data = (from L in _dbcontext.TblLeads join LS in _dbcontext.TblLeadSources on L.LeadSourceId equals LS.Id where L.Status == true && L.LeadOwnerId == leadowner && innerQuery.Contains(L.LeadStatus) select new { L,LS}).ToList();
                //if (data != null)
                //{
                //    List<LeadModel> lstLeadModel = new List<LeadModel>();
                //    foreach (var item in data)
                //    {
                //        lstLeadModel.Add(new LeadModel()
                //        {
                //            Id = item.L.Id,
                //            LeadTitle = item.L.LeadTitle,
                //            LeadStatus = item.L.LeadStatus,
                //            LeadSourceId = item.L.LeadSourceId,
                //            LeadDescription = item.L.LeadDescription,
                //            LeadType = item.L.LeadType,
                //            TimeFrame = item.L.TimeFrame,
                //            LeadSourceName = item.LS.Description,
                //            Size = item.L.Size ?? 0,
                //            Budget = item.L.Budget ?? 0,
                //            AccountId = item.L.AccountId,
                //            Location = item.L.Location,
                //            TenantId = item.L.TenantId,
                //            CreatedDate = Convert.ToDateTime(item.L.CreatedDate),
                //            ModifiedDate = Convert.ToDateTime(item.L.ModifiedDate),
                //            Status = Convert.ToBoolean(item.L.Status)
                //        });
                //    }
                //    return Json(lstLeadModel);
                //}
                //return null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        [HttpGet]
        [Route("SelectAllLeads")]
        public IHttpActionResult SelectAllLeads(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.VWLeadLsEmps where x.Status == true && x.TenantId == Tid orderby x.LeadTitle select new { Id = x.Id, LeadTitle = x.LeadTitle }).ToList();
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
        [Route("CheckModuleConfig")]
        public IHttpActionResult CheckModuleConfig(Guid Tid)
        {
            LeadModel objLeadModel = new LeadModel();
            var modulevalueStatus = (from m in _dbcontext.TblModuleConfigs where m.Status == true && m.ModuleName == "Transation" && m.TenantId == Tid select m.Status).FirstOrDefault();
            if (modulevalueStatus == true)
            {
                objLeadModel.ModuleStatus = true;
            }
            return Ok(objLeadModel);

        }
        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id, Guid Tid)
        {
            try
            {
                LeadNewModel objLeadModel = new LeadNewModel();
                var objTblLead = _dbcontext.TblLeads.SingleOrDefault(x => x.Id == id && x.TenantId == Tid);
                if (objTblLead == null)
                    return Json("Failure");
                else
                {
                    objLeadModel = objNIMBOLEMapper.MapTable2Model(objTblLead);
                    objLeadModel.Id = id;
                    //var objTblLPDiscount = _dbcontext.TblTransLeadPriceDiscounts.Where(lpd => lpd.LeadId == objLeadModel.Id && lpd.TenantId == Tid).OrderByDescending(lpd => lpd.Id).FirstOrDefault();
                    //if (objTblLPDiscount != null)
                    //{
                    //    var objLeadPriceDiscountModel = objNIMBOLEMapper.MapTable2Model(objTblLPDiscount);
                    //    objLeadModel.objLeadPriceDiscountModel = objLeadPriceDiscountModel;
                    //}

                    var lstTransactionByLeadId = (from TS in _dbcontext.TblTransactionInfoes
                                                  where TS.Status == true && TS.LeadId == objLeadModel.Id && TS.TenantId == Tid
                                                  select TS).FirstOrDefault();

                    LeadTransactionInfoModel objLeadTransactionInfoModel = new LeadTransactionInfoModel();
                    if (lstTransactionByLeadId != null)
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
                        objLeadModel.objLeadTransactionInfoModel = objLeadTransactionInfoModel;
                    }
                    else
                    {
                        objLeadModel.objLeadTransactionInfoModel = new LeadTransactionInfoModel();
                    }
                    return Ok(objLeadModel);
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

        #region Post
        [HttpPost]
        [Route("Insert")]
        // POST api/leads
        public IHttpActionResult Post([FromBody] LeadNewModel objLeadModel)
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
                    TblLead objTblLead = objNIMBOLEMapper.MapModel2Table(objLeadModel);
                    if (!_dbcontext.TblLeads.Any(u => u.LeadTitle == objLeadModel.LeadTitle && u.LeadSourceId == objLeadModel.LeadSourceId && u.AccountId == objLeadModel.AccountId && u.TenantId == objLeadModel.TenantId))
                    {
                        _dbcontext.TblLeads.Add(objTblLead);
                        _dbcontext.SaveChanges();
                        objLeadModel.Id = objTblLead.Id;
                    }
                    else
                    {
                        throw new InvalidOperationException("Record already exist/inactive.");
                    }
                    if (!string.IsNullOrEmpty(objLeadModel.ProductString))
                    {
                        List<ProductModel> listProducts = JsonConvert.DeserializeObject<List<ProductModel>>(objLeadModel.ProductString);

                        foreach (var item in listProducts)
                        {
                            TransLProductModel objTransLProductModel = new TransLProductModel();
                            objTransLProductModel.CompetitorId = item.Id;
                            objTransLProductModel.TenantId = objLeadModel.TenantId;
                            objTransLProductModel.LeadId = objLeadModel.Id;
                            objTransLProductModel.ProductId = item.Id;
                            objTransLProductModel.Price = item.Price;
                            objTransLProductModel.Code = item.ProductCode;
                            objTransLProductModel.Quantity = item.Quantity;
                            objTransLProductModel.CreatedDate = DateTime.Now;
                            objTransLProductModel.ModifiedDate = DateTime.Now;
                            // objTransLProductModel.TenantId = item.TenantId.ToDefaultTenantId();
                            objTransLProductModel.Status = true;
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
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(objLeadModel.ContactJsonArray))
                    {
                        List<ExtContactModel> listExtContacts = JsonConvert.DeserializeObject<List<ExtContactModel>>(objLeadModel.ContactJsonArray);
                        foreach (var item in listExtContacts)
                        {
                            ExtContactModel objExtContactModel = new ExtContactModel();
                            objExtContactModel.LeadId = objLeadModel.Id;
                            objExtContactModel.ExtContactId = item.ExtContactId;
                            objExtContactModel.FullName = item.FullName;
                            objExtContactModel.WorkEmail = item.WorkEmail;
                            objExtContactModel.ExtContactRoleId = item.ExtContactRoleId;
                            objExtContactModel.ExtContactRole = item.ExtContactRole;
                            objExtContactModel.TenantId = objLeadModel.TenantId;
                            //objExtContactModel.TenantId = item.TenantId.ToDefaultTenantId();
                            lock (thisLock)
                            {
                                TranLeadContactModel objTranExtContactModel = new TranLeadContactModel();
                                TblTranLeadContact objTblTranExtContact = new TblTranLeadContact();
                                TblContact objTblContact = new TblContact();
                                long ContactId = objExtContactModel.ExtContactId;
                                //Save Contacts and lead details
                                objTblTranExtContact.ContactId = ContactId;
                                objTblTranExtContact.ContactRoleId = objExtContactModel.ExtContactRoleId;
                                objTblTranExtContact.LeadId = objExtContactModel.LeadId;
                                objTblTranExtContact.TenantId = objExtContactModel.TenantId;
                                // objTblTranExtContact.TenantId = objExtContactModel.TenantId.ToDefaultTenantId();
                                _dbcontext.TblTranLeadContacts.Add(objTblTranExtContact);
                                _dbcontext.SaveChanges();

                                //objTranExtContactModel = objNIMBOLEMapper.MapTable2Model(objTblTranExtContact);
                                //objExtContactModel.Id = objTblTranExtContact.Id;
                                //objExtContactModel.LeadId = Convert.ToInt64(objTblTranExtContact.LeadId);
                                //objExtContactModel.TenantId = objTblTranExtContact.TenantId;
                                //objExtContactModel.ExtContactRoleId = Convert.ToInt64(objTblTranExtContact.ContactRoleId);
                                //var strRole = (from ec in _dbcontext.TblContactRoles where ec.Id == objExtContactModel.ExtContactRoleId && ec.TenantId == objExtContactModel.TenantId select ec.Description).FirstOrDefault();
                                //objExtContactModel.ExtContactRole = strRole;
                                //var strFullName = (from ec in _dbcontext.TblContacts where ec.Id == ContactId && ec.TenantId == objExtContactModel.TenantId select ec.FirstName + "" + ec.LastName).FirstOrDefault();
                                //var strContactEmail = (from ec in _dbcontext.TblContacts where ec.Id == ContactId select ec.ContactEmail).FirstOrDefault();
                                //var strWorkEmail = (from ec in _dbcontext.TblContacts where ec.Id == ContactId && ec.TenantId == objExtContactModel.TenantId select ec.WorkEmail).FirstOrDefault(); //sreedhar added on 04-jan-2016


                                //objExtContactModel.FullName = strFullName;
                                //objExtContactModel.ContactEmail = strContactEmail;
                                //objExtContactModel.WorkEmail = strWorkEmail; //sreedhar added on 04-jan-2016
                                //return Json(objExtContactModel);
                            }
                        }
                    }

                    if (objLeadModel.objLeadTransactionInfoModel != null)
                    {
                        if (!string.IsNullOrEmpty(objLeadModel.objLeadTransactionInfoModel.TransName))
                        {
                            List<string> urls = objLeadModel.objLeadTransactionInfoModel.lstURLs;

                            #region If ModelState is not Valid
                            if (response.StatusCode != HttpStatusCode.OK)
                                return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                            #endregion

                            TblTransactionInfo objTblTransactionInfo = new TblTransactionInfo();
                            objTblTransactionInfo.TenantId = objLeadModel.objLeadTransactionInfoModel.TenantId;
                            objTblTransactionInfo.LeadId = objLeadModel.Id;
                            objTblTransactionInfo.TransactionName = objLeadModel.objLeadTransactionInfoModel.TransName;
                            objTblTransactionInfo.Address = objLeadModel.objLeadTransactionInfoModel.Address;
                            objTblTransactionInfo.PlateNo = objLeadModel.objLeadTransactionInfoModel.PlateNumber;
                            objTblTransactionInfo.ObjectId = objLeadModel.objLeadTransactionInfoModel.ProductId;
                            objTblTransactionInfo.BPKBNumber = objLeadModel.objLeadTransactionInfoModel.BPKBNumber;
                            objTblTransactionInfo.TenorScheme = Convert.ToInt32(objLeadModel.objLeadTransactionInfoModel.TenorScheme);
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
                                        string CurrentUser = objLeadModel.objLeadTransactionInfoModel.currentUser;
                                        var CurrentUserId = _dbcontext.TblEmployees.Where(a => a.EmployeeEmail == CurrentUser && a.TenantId == objLeadModel.objLeadTransactionInfoModel.TenantId).Select(a => a.Id).FirstOrDefault();
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
                                        objTblTransInfoDocumet.LeadId = objLeadModel.Id;
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
                        }
                    }
                    return Ok(objLeadModel);
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
        #endregion Post

        #region Post
        [HttpPost]
        [Route("InsertNew")]
        // POST api/leads
        public IHttpActionResult PostNew([FromBody]LeadNewModel objLeadModel)
        {
            try
            {
                //HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                //#region If ModelState is not Valid
                //if (response.StatusCode != HttpStatusCode.OK)
                //    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                //#endregion

                lock (thisLock)
                {
                    TblLead objTblLead = objNIMBOLEMapper.MapModel2Table(objLeadModel);
                    if (!_dbcontext.TblLeads.Any(u => u.LeadTitle == objLeadModel.LeadTitle && u.LeadSourceId == objLeadModel.LeadSourceId && u.AccountId == objLeadModel.AccountId && u.TenantId == objLeadModel.TenantId))
                    {
                        _dbcontext.TblLeads.Add(objTblLead);
                        _dbcontext.SaveChanges();
                        objLeadModel.Id = objTblLead.Id;
                    }
                    else
                    {
                        throw new InvalidOperationException("Record already exist/inactive.");
                    }

                    if (objTblLead.Id != null)
                    {

                        if (Convert.ToInt64(objTblLead.Id) > 0)
                        {

                            foreach (var item in objLeadModel.objProductModel)
                            {
                                TblTransLeadCompetitor objTblTransLeadCompetitor = new TblTransLeadCompetitor();

                                objTblTransLeadCompetitor.CompetitorId = item.Id;
                                objTblTransLeadCompetitor.LeadId = objTblLead.Id;
                                objTblTransLeadCompetitor.ProductId = item.Id;
                                objTblTransLeadCompetitor.Price = item.Price;
                                objTblTransLeadCompetitor.Quantity = item.Quantity;
                                objTblTransLeadCompetitor.CreatedDate = DateTime.Now;
                                objTblTransLeadCompetitor.ModifiedDate = DateTime.Now;
                                objTblTransLeadCompetitor.TenantId = objTblLead.TenantId;
                                objTblTransLeadCompetitor.Status = true;

                                _dbcontext.TblTransLeadCompetitors.Add(objTblTransLeadCompetitor);
                                _dbcontext.SaveChanges();

                            }
                        }
                    }

                    if (objTblLead.Id != null)
                    {
                        if (Convert.ToInt64(objTblLead.Id) > 0)
                        {
                            foreach (var item in objLeadModel.objExtContactModel)
                            {
                                if (item.ExtContactId > 0)
                                {
                                    TblTranLeadContact objTblTranLeadContact = new TblTranLeadContact();
                                    objTblTranLeadContact.LeadId = objTblLead.Id;
                                    objTblTranLeadContact.ContactId = item.ExtContactId;
                                    objTblTranLeadContact.ContactRoleId = item.ExtContactRoleId;
                                    objTblTranLeadContact.TenantId = objTblLead.TenantId;

                                    _dbcontext.TblTranLeadContacts.Add(objTblTranLeadContact);
                                    _dbcontext.SaveChanges();

                                }
                            }

                        }
                    }
                    return Ok(objLeadModel);

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

        #endregion Post

        #region PUT
        // PUT api/leads/5
        [HttpPut]
        [Route("Edit")]
        public IHttpActionResult Edit([FromBody]LeadNewModel objLeadModel)
        {
            try
            {
                TblLead objTblLead = new TblLead();
                string lstemployee = string.Empty;
                if (objLeadModel.LeadEmployees != null)
                    lstemployee = string.Join(",", objLeadModel.LeadEmployees);

                //  objLeadModel.LeadEmployees.Add(lstemployee);
                objTblLead = objNIMBOLEMapper.MapModel2Table(objLeadModel);

                var queries = (from x in _dbcontext.TblLeads select x).ToList();
                if (queries == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    TblLead query = new TblLead();
                    query = queries.Where(x => x.Id == objTblLead.Id).FirstOrDefault();

                    if (query != null)
                    {
                        if (!_dbcontext.TblLeads.Any(u => u.LeadTitle == objLeadModel.LeadTitle && u.LeadSourceId == objLeadModel.LeadSourceId && u.AccountId == objLeadModel.AccountId))
                        {
                            query.LeadTitle = objTblLead.LeadTitle;
                            query.LeadSourceId = objTblLead.LeadSourceId;
                            query.AccountId = objTblLead.AccountId;
                        }


                        objLeadModel.TenantId = query.TenantId;                       

                        var json = new JavaScriptSerializer().Serialize(objLeadModel);

                        LeadNewModel objLeadModelold = new LeadNewModel();
                       
                        objLeadModelold = objNIMBOLEMapper.MapTable2Model(query);

                        //StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                        var jsonold = new JavaScriptSerializer().Serialize(objLeadModelold);

                        string sourceJsonString = jsonold;
                        string targetJsonString = json;

                        string changesinlead = string.Empty;

                        JObject sourceJObject = JsonConvert.DeserializeObject<JObject>(sourceJsonString);
                        JObject targetJObject = JsonConvert.DeserializeObject<JObject>(targetJsonString);

                        if (!JToken.DeepEquals(sourceJObject, targetJObject))
                        {
                            foreach (KeyValuePair<string, JToken> sourceProperty in sourceJObject)
                            {
                                JProperty targetProp = targetJObject.Property(sourceProperty.Key);
                                                                
                                if (!JToken.DeepEquals(sourceProperty.Value, targetProp.Value))
                                {
                                    if (sourceProperty.Key != ("CreatedDate"))
                                        if (sourceProperty.Key != ("ModifiedDate"))
                                    //Console.WriteLine(string.Format("{0} property value is changed", sourceProperty.Key));
                                    changesinlead = changesinlead + ',' + sourceProperty.Key + ':' + targetProp.Value;
                                }

                            }
                        }
                        else
                        {
                            //Console.WriteLine("Objects are same");
                            changesinlead = "Objects are same";
                        }



                        query.LeadDescription = objTblLead.LeadDescription;
                        query.LeadType = objTblLead.LeadType;
                        query.Budget = objTblLead.Budget;
                        query.LeadStatus = objTblLead.LeadStatus;
                        query.Location = objTblLead.Location;
                        query.TimeFrame = objTblLead.TimeFrame;
                        query.Size = objTblLead.Size;
                        query.LeadStage = objTblLead.LeadStage;
                        query.LeadOwnerId = objTblLead.LeadOwnerId;
                        query.LeadEmployees = lstemployee;
                        query.ModifiedDate = objTblLead.ModifiedDate;
                        query.Status = objTblLead.Status;
                        //_dbcontext.SaveChanges();


                        TblEditLogTrack objTblEditLogTrack = new TblEditLogTrack();

                        //var json = new JavaScriptSerializer().Serialize(objLeadModel);

                        objTblEditLogTrack.Module = "Lead " + query.Id;
                        objTblEditLogTrack.ModuleDescription = changesinlead;
                        objTblEditLogTrack.TenantId = query.TenantId;
                        objTblEditLogTrack.EmpId = objLeadModel.ModifiedLeadOwnerId;
                        objTblEditLogTrack.CreatedDate = DateTime.Now;

                        _dbcontext.TblEditLogTracks.Add(objTblEditLogTrack);
                        _dbcontext.SaveChanges();



                    }
                }
                //objLeadModel = objNIMBOLEMapper.MapTable2Model(objTblLead);
                return Ok(objLeadModel);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

        }


        //public partial class Entities
        //{
        //    partial void OnContextCreated()
        //    {
        //        SavingChanges += OnSavingChanges;
        //    }

        //    void OnSavingChanges(object sender, EventArgs e)
        //    {

        //        var modifiedEntities = ObjectStateManager.GetObjectStateEntries(EntityState.Modified);
        //        foreach (var entry in modifiedEntities)
        //        {
        //            var modifiedProps = ObjectStateManager.GetObjectStateEntry(entry.EntityKey).GetModifiedProperties();
        //            var currentValues = ObjectStateManager.GetObjectStateEntry(entry.EntityKey).CurrentValues;
        //            foreach (var propName in modifiedProps)
        //            {
        //                var newValue = currentValues[propName];
        //                //log changes
        //            }
        //        }
        //    }
        //}

        [HttpPut]
        [Route("UpdateValue")]
        public IHttpActionResult UpdateValue([FromBody]string leadData)
        {
            try
            {
                long leadId = Convert.ToInt64(leadData.Split(',')[0].ToString());
                long leadValue = Convert.ToInt64(leadData.Split(',')[1].ToString());
                var query = _dbcontext.TblLeads.Where(ld => ld.Id == leadId).FirstOrDefault();
                query.Size = leadValue;
                _dbcontext.SaveChanges();

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

        //[HttpGet]
        //[Route("GetAllComboValues")]
        //public IHttpActionResult GetAllComboValues(Guid Tid)
        //{
        //    //List<TransLeadModel> objTransLeadModel = new List<TransLeadModel>();
        //    TransLeadModel objTransLeadModelItem = new TransLeadModel();
        //    KeyValueModel ObjKeyValueModel = new KeyValueModel();

        //    TblAccount ObjTblAccount = new TblAccount();
        //    try
        //    {
        //        var ObjTblAccounts = (from x in _dbcontext.TblAccounts where x.Status == true && x.TenantId == Tid orderby x.AccountName select new { Id = x.Id, Name = x.AccountName }).Distinct();
        //        ObjKeyValueModel = objNIMBOLEMapper.MapTabletoModel(ObjTblAccount);

        //        return Ok(ObjKeyValueModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
        //        {
        //            ReasonPhrase = ex.Message
        //        });
        //    }
        //    return null;
        //}

        #region DELETE
        // DELETE api/leads/5
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(string selectedId, bool status)
        {
            try
            {
                List<string> StrTempIds = new List<string>();
                string[] ids = { selectedId };
                if (!string.IsNullOrEmpty(selectedId))
                {
                    ids = selectedId.Split(',');
                }

                foreach (string tempId in ids)
                {
                    int id = Convert.ToInt32(tempId);

                    var objTblLead = _dbcontext.TblLeads.Where(x => x.Id == id).FirstOrDefault();
                    if (objTblLead == null)
                        throw new InvalidOperationException("Record not Found.");

                    objTblLead.Status = status;
                    // _dbcontext.TblLeads.Remove(objTblLead);
                    _dbcontext.SaveChanges();
                    //LeadModel objLeadModel = objNIMBOLEMapper.MapTable2Model(objTblLead);
                    //return Ok(objLeadModel);
                }
                if (StrTempIds.Count > 0)
                {
                    throw new InvalidOperationException("Record is associated.");
                }
                return Ok();
            }
            catch (EntityException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        [HttpDelete]
        [Route("DeleteRec")]
        public IHttpActionResult DeleteRec(string selectedId)
        {
            try
            {
                string[] ids = { selectedId };
                List<string> StrTempIds = new List<string>();

                if (!string.IsNullOrEmpty(selectedId))
                {
                    ids = selectedId.Split(',');
                }

                foreach (string tempId in ids)
                {
                    int id = Convert.ToInt32(tempId);

                    var objTblLead = _dbcontext.TblLeads.Where(x => x.Id == id).FirstOrDefault();
                    if (objTblLead != null)
                    {
                        //select * from tbltransleadcompetitor where leadid=458
                        var trnsleadcomp = (from tlc in _dbcontext.TblTransLeadCompetitors where tlc.LeadId == id select tlc).ToList();
                        if (trnsleadcomp != null)
                        {
                            foreach (var item in trnsleadcomp)
                            {
                                _dbcontext.TblTransLeadCompetitors.Remove(item);
                            }
                        }

                        // select * from tbltranleadcontact where leadid=458

                        var trnsleadcont = (from tc in _dbcontext.TblTranLeadContacts where tc.LeadId == id select tc).ToList();
                        if (trnsleadcont != null)
                        {
                            foreach (var item in trnsleadcont)
                            {
                                _dbcontext.TblTranLeadContacts.Remove(item);
                            }
                        }


                        // select * from tbldocument where id in ( select documentid from tbltransdocument where leadid=458)

                        var trnsdocid = (from td in _dbcontext.TblTransDocuments where td.LeadId == id select td.DocumentId).ToList();

                        if (trnsdocid != null)
                        {
                            var tbldoc = _dbcontext.TblDocuments.Where(d => trnsdocid.Contains(d.Id)).ToList();

                            foreach (var item in tbldoc)
                            {
                                _dbcontext.TblDocuments.Remove(item);
                            }

                            var trndoc = (from td in _dbcontext.TblTransDocuments where td.LeadId == id select td).ToList();

                            foreach (var item in trndoc)
                            {
                                _dbcontext.TblTransDocuments.Remove(item);
                            }
                        }

                        //select * from tblactivity where id in (select activityid from tbltranslead where leadid=458)

                        var trnsleadid = (from tl in _dbcontext.TblTransLeads where tl.LeadId == id select tl.ActivityId).ToList();

                        if (trnsleadid != null)
                        {
                            var tblact = _dbcontext.TblActivities.Where(d => trnsleadid.Contains(d.Id)).ToList();

                            foreach (var item in tblact)
                            {
                                _dbcontext.TblActivities.Remove(item);
                            }

                            var trnslead = (from tl in _dbcontext.TblTransLeads where tl.LeadId == id select tl).ToList();

                            foreach (var item in trnslead)
                            {
                                _dbcontext.TblTransLeads.Remove(item);
                            }
                        }


                        _dbcontext.TblLeads.Remove(objTblLead);
                        _dbcontext.SaveChanges();
                        //LeadModel objLeadModel = objNIMBOLEMapper.MapTable2Model(objTblLead);
                        //return Ok(objLeadModel);
                    }
                    else
                    {
                        // throw new InvalidOperationException("Record not found.");
                    }
                }
                if (StrTempIds.Count > 0)
                {
                    throw new InvalidOperationException("Record is associated.");
                }
                return Ok();
            }
            catch (EntityException ex)
            {
                throw new InvalidOperationException(ex.Message);
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
