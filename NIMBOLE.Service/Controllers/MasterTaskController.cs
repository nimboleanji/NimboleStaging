using NIMBOLE.Entities;
using NIMBOLE.Models.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/MasterTask")]
    public class MasterTaskController : ApiController
    {
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();
        TblMasterTask objTblMasterTask = new TblMasterTask();

        #region GetMethods
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                List<EmployeeTaskModel> listEmployeeTaskModel = new List<EmployeeTaskModel>();
                var masterTaskList = (from t in _dbcontext.TblMasterTasks where t.TenantId == Tid  select t).ToList();
                foreach (var item in masterTaskList)
                {
                    listEmployeeTaskModel.Add(new EmployeeTaskModel() { Id = item.Id, Type = item.Type, Title = item.Title, Comments = item.Comments, Status = Convert.ToBoolean(item.Status)});
                }
                return Ok(listEmployeeTaskModel);
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
                var query = _dbcontext.TblMasterTasks.SingleOrDefault(x => x.Id == id && x.TenantId == Tid);
                if (query == null)
                    throw new InvalidOperationException("Record not Found.");
                else
                {
                    EmployeeTaskModel ObjEmployeeTaskModel = new EmployeeTaskModel();
                    ObjEmployeeTaskModel.Id = query.Id;
                    ObjEmployeeTaskModel.Title = query.Title;
                    ObjEmployeeTaskModel.Type = query.Type;
                    ObjEmployeeTaskModel.Comments = query.Comments;
                    ObjEmployeeTaskModel.Status = Convert.ToBoolean(query.Status);
                    return Ok(ObjEmployeeTaskModel);
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
      
        #region PostMethods
        [HttpPost]
        [Route("Insert")]
        //[ModelValidator]
        public IHttpActionResult Post([FromBody] EmployeeTaskModel objEmployeeTaskModel)
        {
            try
            {
                objEmployeeTaskModel.CreatedDate = DateTime.Now;
                objEmployeeTaskModel.ModifiedDate = DateTime.Now;
                objEmployeeTaskModel.Status = true;
                TblMasterTask ObjTblMasterTask = new TblMasterTask();
                ObjTblMasterTask.TenantId = objEmployeeTaskModel.TenantId;
                ObjTblMasterTask.Title = objEmployeeTaskModel.Title;
                ObjTblMasterTask.Type = objEmployeeTaskModel.Type;
                ObjTblMasterTask.Comments = objEmployeeTaskModel.Comments;
                ObjTblMasterTask.CreatedDate = objEmployeeTaskModel.CreatedDate;
                ObjTblMasterTask.ModifiedDate = objEmployeeTaskModel.ModifiedDate;
                ObjTblMasterTask.Status = objEmployeeTaskModel.Status;
                _dbcontext.TblMasterTasks.Add(ObjTblMasterTask);
                _dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        ReasonPhrase = ex.Message
                    });
            }
            return Ok(objEmployeeTaskModel);
        }

        [HttpPut]
        [Route("Edit")]
        public IHttpActionResult Edit(EmployeeTaskModel objEmployeeTaskModel)
        {
            var query = (from t in _dbcontext.TblMasterTasks where t.Id == objEmployeeTaskModel.Id && t.TenantId == objEmployeeTaskModel.TenantId select t).FirstOrDefault();
            query.Title = objEmployeeTaskModel.Title;
            query.Type = objEmployeeTaskModel.Type;
            query.Comments = objEmployeeTaskModel.Comments;
            query.Status = objEmployeeTaskModel.Status;
            _dbcontext.SaveChanges();
            return Ok(objEmployeeTaskModel);
        }
        #endregion

        #region DELETE
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(int id, bool status)
        {
            try
            {
                var query = _dbcontext.TblMasterTasks.Where(x => x.Id == id).FirstOrDefault();
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
