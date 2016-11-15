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

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/ParentAccount")]
    public class ParentAccountController : ApiController
    {
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        #region POST
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Post([FromBody] ParentAccountModel objParentAccountModel)
        {
            try
            {
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);

                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion
                TblParentAccount objTblParentAccount = objNIMBOLEMapper.MapModel2Table(objParentAccountModel);
                _dbcontext.TblParentAccounts.Add(objTblParentAccount);
                _dbcontext.SaveChanges();
                objParentAccountModel = objNIMBOLEMapper.MapTable2Model(objTblParentAccount);
                return Ok(objParentAccountModel);
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
    }
}
