using AutoMapper;
using NIMBOLE.Entities;
using NIMBOLE.Entities.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Entities.Models;
using NIMBOLE.Common;
namespace NIMBOLE.Service.Controllers
{
     [RoutePrefix("api/ExtContactRole")]
    public class ExtContactRoleController : ApiController
    {
         DTO objNIMBOLEMapper = new DTO();
         private NIMBOLEContext _dbcontext = new NIMBOLEContext();
         public ExtContactRoleController()
        {

        }
         
        #region GET
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            var data = (from x in _dbcontext.TblExtContactRoles where x.Status==true select x).ToList();
            List<ExtContactRoleModel> lstExtContactRoleModel = new List<ExtContactRoleModel>();
            foreach (var item in data)
            {
                ExtContactRoleModel objExtContactRoleModel = new ExtContactRoleModel();
                objExtContactRoleModel = objNIMBOLEMapper.MapTable2Model(item);
                lstExtContactRoleModel.Add(objExtContactRoleModel);
            }
            return Json(lstExtContactRoleModel);
        }

        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var query = _dbcontext.TblExtContactRoles.SingleOrDefault(x => x.Id == id);
                if (query == null)
                    return BadRequest("Record Not Found");
                else
                    return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        [HttpGet]
        [Route("SelectAllExtContactRoles")]
        public IHttpActionResult SelectAllExtContactRoles()
        {

            var data = (from x in _dbcontext.TblExtContactRoles where x.Status==true select new { Id = x.Id, Description = x.Description }).ToList();
            return Ok(data);
        }

        #region POST
        [HttpPost]
        [Route("Insert")]
        //[ModelValidator]
        public IHttpActionResult Post([FromBody] ExtContactRoleModel objExtContactRoleModel)
        {
            try
            {
                if (objExtContactRoleModel.TenantId.Equals(new Guid("{00000000-0000-0000-0000-000000000000}")))
                    objExtContactRoleModel.TenantId = objExtContactRoleModel.TenantId.ToDefaultTenantId();
                objExtContactRoleModel.CreateDate = DateTime.Now;
                objExtContactRoleModel.ModifiedDate = DateTime.Now;
                objExtContactRoleModel.Status = true;
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion
                TblExtContactRole objTblExtContactRole = objNIMBOLEMapper.MapModel2Table(objExtContactRoleModel);
                _dbcontext.TblExtContactRoles.Add(objTblExtContactRole);
                _dbcontext.SaveChanges();
                objExtContactRoleModel = objNIMBOLEMapper.MapTable2Model(objTblExtContactRole);
                return Ok(objExtContactRoleModel);
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
                return new NIMBOLE.Service.Controllers.TextResult(ex.InnerException.Message, Request);
            }

        }
        #endregion

        //#region PUT
        //[HttpPut]
        //[Route("Edit")]
        //public IHttpActionResult Edit([FromBody] TblProduct entity)
        //{
        //    try
        //    {
        //        var queries = (from x in _dbcontext.TblProducts select x).ToList();
        //        if (queries != null)
        //        {
        //            TblProduct query = new TblProduct();
        //            query = queries.Where(x => x.TenantId == entity.TenantId).FirstOrDefault();
        //            query.Code = entity.Code;
        //            query.ProductName = entity.ProductName;

        //            query.Price = entity.Price;
        //            query.ExpireDate = entity.ExpireDate;
        //            query.ManifectureName = entity.ManifectureName;
        //            query.Comments = entity.Comments;

        //            query.CreatedDate = entity.CreatedDate;
        //            query.ModifiedDate = entity.ModifiedDate;
        //            query.Status = entity.Status;
        //            _dbcontext.SaveChanges();
        //            return Ok(entity);
        //        }
        //        else
        //        {
        //            return BadRequest("Not Found");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}
        //#endregion

        //#region DELETE
        //[HttpDelete]
        //[Route("Delete")]
        //public IHttpActionResult Delete(int id)
        //{
        //    try
        //    {
        //        var query = _dbcontext.TblProducts.Where(x => x.Id == id).FirstOrDefault();
        //        if (query != null)
        //        {
        //            query.Status = false;
        //           // _dbcontext.TblProducts.Remove(query);
        //            _dbcontext.SaveChanges();
        //            return Ok(query);
        //        }
        //        else
        //        {
        //            return BadRequest("Not Found Record");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}
        //#endregion

    }
}
