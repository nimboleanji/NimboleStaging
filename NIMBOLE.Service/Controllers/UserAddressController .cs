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
    [RoutePrefix("api/UserAddress")]
    public class UserAddressController : ApiController
    {
        DTO objNIMBOLEMapper = new DTO();
        // GET api/AddressEmployees
        private NIMBOLEContext _dbcontext = new NIMBOLEContext();

        #region POST
        [HttpPost]
        [Route("Insert")]
        [ModelValidator]
        public IHttpActionResult Post([FromBody] UserAddressModel objUserAddressModel)
        {
            try
            {
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                TblUserAddress objTblUserAddress = new TblUserAddress();
                objTblUserAddress = objNIMBOLEMapper.MapModel2Table(objUserAddressModel);
                _dbcontext.TblUserAddresses.Add(objTblUserAddress);
                _dbcontext.SaveChanges();
                objUserAddressModel = objNIMBOLEMapper.MapTable2Model(objTblUserAddress);
                return Ok(objUserAddressModel);
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
                throw new InvalidOperationException(ex.Message);
            }
            
        }
        #endregion

    }
}
