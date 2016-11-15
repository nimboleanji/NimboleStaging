using AutoMapper;
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
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        DTO objNIMBOLEMapper = new DTO();

        #region GET
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var data = (from x in _dbcontext.TblUsers where x.Status == true select x).ToList();
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
        [Route("SelectAllUsers")]
        public IHttpActionResult SelectAllUsers()
        {
            try
            {
                var data = (from usr in _dbcontext.TblUsers where usr.Status == true select new { Id = usr.Id, FirstName = usr.FirstName }).Union(from emp in _dbcontext.TblEmployees select new { Id = emp.Id, FirstName = emp.FirstName }).ToList();
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
        [Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var query = _dbcontext.TblUsers.SingleOrDefault(x => x.Id == id);
                if (query == null)
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
        public IHttpActionResult Post([FromBody] UserModel objUserModel)
        {
            try
            {
                #region If ModelState is not Valid

                #endregion

                HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                #endregion

                TblUser objTblUser = objNIMBOLEMapper.MapModel2Table(objUserModel);
                _dbcontext.TblUsers.Add(objTblUser);
                _dbcontext.SaveChanges();

                objUserModel = objNIMBOLEMapper.MapTable2Model(objTblUser);
                return Ok(objUserModel);
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
        public IHttpActionResult Edit([FromBody] TblUser user)
        {
            try
            {
                var users = (from x in _dbcontext.TblUsers select x).ToList();
                if (users != null)
                {
                    TblUser query = new TblUser();
                    query = users.Where(x => x.Code == user.Code && x.TenantId == user.TenantId).FirstOrDefault();
                    query.Id = user.Id;
                    query.Id = user.Id;
                    query.FirstName = user.FirstName;
                    query.LastName = user.LastName;
                    query.MobileNo = user.MobileNo;
                    query.DOB = user.DOB;
                    query.CreatedDate = user.CreatedDate;
                    query.ModifiedDate = user.ModifiedDate;
                    query.Status = user.Status;
                    _dbcontext.SaveChanges();
                    return Ok(user);
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
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var query = _dbcontext.TblUsers.Where(x => x.Id == id).FirstOrDefault();
                if (query != null)
                {
                    query.Status = false;
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