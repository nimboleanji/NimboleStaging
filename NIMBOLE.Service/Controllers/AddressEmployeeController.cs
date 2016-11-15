using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Entities;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/AddressEmployee")]
   // public class AddressController : ApiController
    public class AddressEmployeeController : ApiController
    {
        DTO objNIMBOLEMapper = new DTO();
        // GET api/AddressEmployees
        private NIMBOLEContext _dbcontext = new NIMBOLEContext();

        #region GET
               
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var data = (from a in _dbcontext.TblAddressEmployees select a).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var query = _dbcontext.TblAddressEmployees.SingleOrDefault(x => x.Id == id);
                if (query == null)
                    return BadRequest("Record Not Found");
                else
                    return Ok(query);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

        }
        #endregion
               
        //#region POST
        //[HttpPost]
        //[Route("Insert")]
        //[ModelValidator]
        //public IHttpActionResult Post([FromBody] EmployeeAddressModel objEmployeeAddressModel)
        //{
        //    try
        //    {
        //        HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
        //        #region If ModelState is not Valid
        //        if (response.StatusCode != HttpStatusCode.OK)
        //            return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
        //        #endregion

        //        TblAddressEmployee objTblAddressEmployee = new TblAddressEmployee();

        //        objTblAddressEmployee = objNIMBOLEMapper.MapModel2Table(objEmployeeAddressModel);

        //        _dbcontext.TblAddressEmployees.Add(objTblAddressEmployee);
        //        _dbcontext.SaveChanges();

        //        objEmployeeAddressModel = objNIMBOLEMapper.MapTable2Model(objTblAddressEmployee);
        //        return Ok(objEmployeeAddressModel);
        //    }
        //    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
        //    {
        //        Exception raise = dbEx;
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                string message = string.Format("{0}:{1}",
        //                    validationErrors.Entry.Entity.ToString(),
        //                    validationError.ErrorMessage);
        //                raise = new InvalidOperationException(message, raise);
        //            }
        //        }
        //        throw raise;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new NIMBOLE.Service.Controllers.TextResult(ex.Message, Request);
        //    }

        //}
        //#endregion

        #region PUT
        [HttpPut]
        [Route("Edit")]
        public IHttpActionResult Edit([FromBody] TblAddressEmployee entity)
        {
            try
            {
                var queries = (from x in _dbcontext.TblAddressEmployees select x).ToList();
                if (queries != null)
                {
                    TblAddressEmployee query = new TblAddressEmployee();
                    query = queries.Where(x => x.Id== entity.Id).FirstOrDefault();
                    query.AddressId = entity.AddressId;
                    query.EmployeeId = entity.EmployeeId;                    
                    _dbcontext.SaveChanges();
                    return Ok(entity);
                }
                else
                {
                    throw new InvalidOperationException("No Record found.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
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
                var query = _dbcontext.TblAddressEmployees.Where(x => x.Id == id).FirstOrDefault();
                if (query != null)
                {
                    _dbcontext.TblAddressEmployees.Remove(query);
                    _dbcontext.SaveChanges();
                    return Ok(query);
                }
                else
                {
                    throw new InvalidOperationException("No Record found.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
          
        }
        #endregion

    }
}
