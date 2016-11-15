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
using NIMBOLE.Common;
using System.Threading.Tasks;
using System.Data.Entity;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Milestones")]
    public class MilestonesController : ApiController
    {
        private static Object thisLock = new Object();
        DTO objNIMBOLEMapper = new DTO();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();

        #region GET
		
		     [HttpGet]
        [Route("GetAllMileStones")]
        public IHttpActionResult GetAllMileStones(Guid Tid)
        {
            try
            {                
                var data = (from m in _dbcontext.TblMileStones.Where(m => m.TenantId == Tid && m.Status==true) select m).ToList();
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
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                string objTid = Tid.ToString();

                List<MilestoneModel> lstMileStoneModel = new List<MilestoneModel>();
                MilestoneModel ObjMileStoneModel = new MilestoneModel();
                var data = (from m in _dbcontext.TblMileStones.Where(m => m.TenantId == Tid) from MS in _dbcontext.TblMileStoneStages.Where(details => m.MSOrder == details.Id && m.TenantId == Tid).DefaultIfEmpty() select new { Id = m.Id, TenantId = m.TenantId, Code = m.Code, Description = m.Description, Status = m.Status, MileStoneStage = MS.MileStoneStage }).ToList();
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
        [Route("GetAllByLeadId")]
        public async Task<IHttpActionResult> GetAllByLeadId(long iLeadId)
        {
            try
            {
                List<MilestoneModel> lstMileStoneModel = new List<MilestoneModel>();
                var activityId = await (from a in _dbcontext.TblTransLeads where a.LeadId == iLeadId && a.Status == true select a.ActivityId).ToListAsync();
                if (activityId != null)
                {                    
                    var milestoneId = await (from m in _dbcontext.TblActivities where activityId.Contains(m.Id) && m.Status == true select m.MileStoneId).ToListAsync();
                    if (milestoneId != null && milestoneId.Count > 0)
                    {
                        var data = await (from tm in _dbcontext.TblMileStones where milestoneId.Contains(tm.Id) && tm.Status == true select tm).ToListAsync();
                        if (data.Count() > 0)
                        {                            
                            foreach (var item in data)
                            {
                                MilestoneModel ObjMileStoneModel = objNIMBOLEMapper.MapTable2Model(item);
                                lstMileStoneModel.Add(ObjMileStoneModel);
                            }
                        }
                    }
                }
                return Ok(lstMileStoneModel);
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
        [Route("GetMileStoneByLeadActivity")]
        public IHttpActionResult GetMileStoneByLeadActivity(long iLeadId, long iActivityId,Guid Tid)
        {
            try
            {
                var mileStones = _dbcontext.TblActivities.Where(act => act.Id == iActivityId && act.Status == true && act.TenantId==Tid).ToList();
                List<long> lstMileStoneId = mileStones.Select(ms => ms.MileStoneId ?? 0).ToList<long>();

                List<MilestoneModel> lstMilestoneModel = new List<MilestoneModel>();
                TblMileStone objTblMileStone = null;
                lstMilestoneModel.Clear();
                foreach (var item in lstMileStoneId)
                {
                    objTblMileStone = new TblMileStone();
                    objTblMileStone = _dbcontext.TblMileStones.Where(ms => ms.Id == item && ms.Status == true && ms.TenantId==Tid).FirstOrDefault();
                    MilestoneModel ObjMileStoneModel = objNIMBOLEMapper.MapTable2Model(objTblMileStone);
                    lstMilestoneModel.Add(ObjMileStoneModel);
                }
                if (lstMilestoneModel == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    return Ok(lstMilestoneModel);
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

        //GetById api/MileStone/GetById/5
        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id, Guid Tid)
        {
            try
            {
                var query = _dbcontext.TblMileStones.SingleOrDefault(x => x.Id == id && x.TenantId==Tid);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    MilestoneModel ObjMileStoneModel = objNIMBOLEMapper.MapTable2Model(query);
                    return Ok(ObjMileStoneModel);
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
        [Route("SelectAllMileStone")]
        public IHttpActionResult SelectAllMileStone(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblMileStones orderby x.Description where x.Status == true && x.TenantId==Tid select new { Id = x.Id, Description = x.Description, MSOrder = x.MSOrder }).ToList();
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
        [ModelValidator]
        public IHttpActionResult Post([FromBody] MilestoneModel ObjMileStoneModel)
        {
            try
            {
                lock (thisLock)
                {
                  //  ObjMileStoneModel.TenantId = ObjMileStoneModel.TenantId.ToDefaultTenantId();
                    ObjMileStoneModel.CreatedDate = DateTime.Now;
                    ObjMileStoneModel.ModifiedDate = DateTime.Now;
                    ObjMileStoneModel.Status = true;
                    HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                    #region If ModelState is not Valid
                    if (response.StatusCode != HttpStatusCode.OK)
                        return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                    #endregion
                    TblMileStone ObjTblMstMileStone = objNIMBOLEMapper.MapModel2Table(ObjMileStoneModel);

                    if (ObjTblMstMileStone.Description != null || ObjTblMstMileStone.Code != null)
                    {
                        if (!_dbcontext.TblMileStones.Any(u => u.Description == ObjMileStoneModel.Description && u.TenantId==ObjMileStoneModel.TenantId))
                        {
                            _dbcontext.TblMileStones.Add(ObjTblMstMileStone);
                            _dbcontext.SaveChanges();
                        }
                        else
                        {
                            throw new InvalidOperationException("Record already exists.");
                        }
                    }
                    ObjMileStoneModel = objNIMBOLEMapper.MapTable2Model(ObjTblMstMileStone);
                }
                return Ok(ObjMileStoneModel);
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
        public IHttpActionResult Edit([FromBody] MilestoneModel ObjMileStoneModel)
        {
            try
            {
                if (ObjMileStoneModel.Description != null)
                {

                    TblMileStone record = (from m in _dbcontext.TblMileStones where m.Id == ObjMileStoneModel.Id && m.TenantId == ObjMileStoneModel.TenantId select m).FirstOrDefault();
                    if (record == null)
                        throw new InvalidOperationException("Record not Found.");

                    if (ObjMileStoneModel.Description == record.Description)
                    {
                        record.Status = ObjMileStoneModel.Status;
                        record.Code = ObjMileStoneModel.Code;
                        record.MSOrder = ObjMileStoneModel.MSOrder;
                        record.ModifiedDate = DateTime.Now;
                        _dbcontext.SaveChanges();
                        return Ok(ObjMileStoneModel);
                    }
                    else
                    {
                        List<TblMileStone> _objMilesones = (from x in _dbcontext.TblMileStones where x.Description == ObjMileStoneModel.Description && x.TenantId==ObjMileStoneModel.TenantId  select x).ToList();
                        if (_objMilesones.Count == 0)
                        {
                            record.Status = ObjMileStoneModel.Status;
                            record.Code = ObjMileStoneModel.Code;
                            record.MSOrder = ObjMileStoneModel.MSOrder;
                            record.Description = ObjMileStoneModel.Description;
                            record.ModifiedDate = DateTime.Now;
                            _dbcontext.SaveChanges();
                            return Ok(ObjMileStoneModel);
                        }
                        else
                        {
                            throw new InvalidOperationException("Record already exists.");
                        }
                    }
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

        #region DELETE
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(int id, bool status)
        {
            try
            {
                var query = _dbcontext.TblMileStones.Where(m => m.Id == id).FirstOrDefault();
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                query.Status = status;
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
