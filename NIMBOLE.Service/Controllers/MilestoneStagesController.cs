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
    [RoutePrefix("api/MilestoneStages")]
    public class MilestoneStagesController : ApiController
    {
        DTO objNIMBOLEMapper = new DTO();
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();

        #region Get

        [HttpGet]
        [Route("GetMilestoneStageList")]
        public async Task<IHttpActionResult> GetMilestoneStageList(Guid Tid)
        {
            try
            {
                var lstMilestoneStages = await (from mss in _dbcontext.TblMileStoneStages  select mss).ToListAsync();

                if (lstMilestoneStages == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    List<MilestoneStageModel> objMilestoneStageModel = new List<MilestoneStageModel>();
                    foreach (var item in lstMilestoneStages)
                    {
                        objMilestoneStageModel.Add(new MilestoneStageModel()
                            {
                                Id = item.Id,
                                MileStoneStage = item.MileStoneStage,
                                EmployeeRoles = item.Roles
                            });
                    }
                    return Json(objMilestoneStageModel);
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
        public IHttpActionResult Post([FromBody] MilestoneStageModel ObjMilestoneStageModel)
        {
            try
            {
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion
                TblMileStoneStage ObjTblMileStoneStage = objNIMBOLEMapper.MapModel2Table(ObjMilestoneStageModel);

                if (ObjTblMileStoneStage.MileStoneStage != null)
                {
                    _dbcontext.TblMileStoneStages.Add(ObjTblMileStoneStage);
                    _dbcontext.SaveChanges();
                }

                ObjMilestoneStageModel = objNIMBOLEMapper.MapTable2Model(ObjTblMileStoneStage);
                return Ok(ObjMilestoneStageModel);
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
        public IHttpActionResult Edit([FromBody] MilestoneStageModel ObjMilestoneStageModel)
        {
            try
            {
                if (ObjMilestoneStageModel != null && !string.IsNullOrEmpty(ObjMilestoneStageModel.MileStoneStage))
                {

                    string lstroles = string.Empty;
                    if (ObjMilestoneStageModel.Roles != null)
                        lstroles = string.Join(",", ObjMilestoneStageModel.Roles);

                    var queries = (from ms in _dbcontext.TblMileStoneStages select ms).ToList();
                    if (queries == null)
                        throw new InvalidOperationException("Record not Found.");

                    TblMileStoneStage ObjTblMileStoneStage = new TblMileStoneStage();
                    ObjTblMileStoneStage = queries.Where(m => m.MileStoneStage == ObjMilestoneStageModel.MileStoneStage && m.Id == ObjMilestoneStageModel.Id).FirstOrDefault();
                    ObjTblMileStoneStage.Id = ObjMilestoneStageModel.Id;
                    ObjTblMileStoneStage.MileStoneStage = ObjMilestoneStageModel.MileStoneStage;
                    ObjTblMileStoneStage.Roles = lstroles;

                    _dbcontext.SaveChanges();
                    return Ok(ObjMilestoneStageModel);
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
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var query = _dbcontext.TblMileStoneStages.Where(m => m.Id == id).FirstOrDefault();
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                // has to delete   
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