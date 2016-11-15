using AutoMapper;
using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;
using NIMBOLE.Entities;
namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Languages")]
    public class LanguagesController : ApiController
    {
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        #region GET
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                var languages = (from l in _dbcontext.TblLanguageNews where l.TenantId==Tid orderby l.LanguageName select new { Id = l.Id, Name = l.LanguageName }).ToList();
                return Ok(languages);
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
                var query=_dbcontext.TblLanguageNews.SingleOrDefault(l => l.Id == id && l.TenantId==Tid );
                if(query==null)
                    throw new InvalidOperationException("Record not Found.");
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
        #endregion

        #region POST
        [HttpPost]
        [Route("Insert")]
        [ModelValidator]
        public IHttpActionResult Post([FromBody] TblLanguageNew language)
        {
            try
            {
                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion
                
                _dbcontext.TblLanguageNews.Add(language);
                _dbcontext.SaveChanges();
                //return Created("InsertRole",role.RoleId);
                return Ok(language.Id);
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
                        // raise a new exception nesting  
                        // the current instance as InnerException  
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

        #region Put
        [HttpPut]
        [Route("Edit")]
        public IHttpActionResult Edit([FromBody] NIMBOLE.Models.Models.LanguageModel language)
        {
            try
            {
                TblLanguageNew query = _dbcontext.TblLanguageNews.Find(language.Id , language.TenantId);
                query.Code = language.Code;
                query.TenantId = language.TenantId;
                query.LanguageName= language.LanguageName;
                _dbcontext.SaveChanges();
                return Ok(language);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }           
        }
        #endregion Put

        #region Delete
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var languageEntity = _dbcontext.TblLanguages.Find(id);
                if (languageEntity != null)
                {
                    _dbcontext.TblLanguages.Remove(languageEntity);
                    _dbcontext.SaveChanges();
                    return Ok(languageEntity);
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
        #endregion Delete
    }
}