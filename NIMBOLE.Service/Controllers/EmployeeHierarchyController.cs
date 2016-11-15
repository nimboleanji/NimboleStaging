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
    [RoutePrefix("api/EmployeeHierarchy")]
    public class EmployeeHierarchyController : ApiController
    {
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        #region GET
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var data = (from x in _dbcontext.TblEmpHierarchies select x).ToList();
                if (data != null)
                    return Ok(data);
                else
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

        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var query=_dbcontext.TblEmpHierarchies.SingleOrDefault(x => x.Id == id);
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

        [HttpGet]
        [Route("GetData")]
        public IHttpActionResult GetData(Guid Tid)
        {
            try
            {
                List<EmpHierarchyModel> ObjEmpHierarchyModel = new List<EmpHierarchyModel>();
                var query = (from q in _dbcontext.TblEmployeeRoles where q.ParentId != null && q.TenantId==Tid select q ).ToList();
                foreach (var items in query)
                {
                    ObjEmpHierarchyModel.Add(new EmpHierarchyModel()
                    {
                        Id = items.Id,
                        ParentId = items.ParentId ?? 0,
                        EDescription = items.Description
                    });
                }

                if (ObjEmpHierarchyModel != null)
                    return Ok(ObjEmpHierarchyModel);
                else
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

        [HttpGet]
        [Route("GetDataNew")]
        public IHttpActionResult GetDataNew(Guid Tid)
        {
            try
            {
                List<EmpHierarchyModel> ObjEmpHierarchyModel = new List<EmpHierarchyModel>();
                var query = (from q in _dbcontext.TblEmployeeRoles where q.TenantId == Tid && (q.ParentId == null || q.ParentId == 0) select q).ToList();
                foreach (var items in query)
                {
                    ObjEmpHierarchyModel.Add(new EmpHierarchyModel()
                    {
                        Id = items.Id,
                        ParentId = items.ParentId??0,
                        EDescription = items.Description 
                    });
                }
                if (ObjEmpHierarchyModel != null)
                    return Ok(ObjEmpHierarchyModel);
                else
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


        [HttpGet]
        [Route("SelectAllEmpHierarchy")]
        public IHttpActionResult SelectAllEmpHierarchy()
        {
            try
            {
                var data = (from x in _dbcontext.TblEmpHierarchies select new { Id = x.ParentId ?? 0, EDescription = x.EDescription }).ToList();
                if (data != null)
                    return Ok(data);
                else
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

        #region Post
        [HttpPost]
        [Route("AddNode")]
        public IHttpActionResult AddNode(EmpHierarchyModel objEmpHierarchyModel)
        {
            try
            {
                TblEmpHierarchyNew _objEmpHierarchy = new TblEmpHierarchyNew();

                _objEmpHierarchy.TenantId = objEmpHierarchyModel.TenantId;
                _objEmpHierarchy.EDescription = objEmpHierarchyModel.EDescription;
                _objEmpHierarchy.ParentId = objEmpHierarchyModel.ParentId;
                _dbcontext.TblEmpHierarchyNews.Add(_objEmpHierarchy);
                _dbcontext.SaveChanges();

                return SelectAllEmpHierarchy();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }            
        }

        [HttpPost]
        [Route("UpdateNode")]
        public IHttpActionResult UpdateNode(EmpHierarchyModel objEmpHierarchyModel)
        {
            try
            {
                var node = (from e in _dbcontext.TblEmployeeRoles where e.Description == objEmpHierarchyModel.TxtOldName && e.TenantId==objEmpHierarchyModel.TenantId  select e).FirstOrDefault();
                //2. change student name in disconnected mode (out of ctx scope)
                if (node != null)
                {
                    node.Description = objEmpHierarchyModel.Data;
                }
                _dbcontext.Entry(node).State = System.Data.Entity.EntityState.Modified;
                _dbcontext.SaveChanges();
                return SelectAllEmpHierarchy();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }            
        }

        [HttpPost]
        [Route("DeleteNode")]
        public IHttpActionResult DeleteNode(EmpHierarchyModel objEmpHierarchyModel)
        {
            try
            {
                var node = (from e in _dbcontext.TblEmpHierarchyNews where e.EDescription == objEmpHierarchyModel.TxtNode && e.TenantId==objEmpHierarchyModel.TenantId select e).FirstOrDefault();
                _dbcontext.Entry(node).State = System.Data.Entity.EntityState.Deleted;
                _dbcontext.SaveChanges();
                return SelectAllEmpHierarchy();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }           
        }

        [HttpPost]
        [Route("ChangeParent")]
        public IHttpActionResult ChageParent(EmpHierarchyModel objEmpHierarchyModel)
        {
            try
            {
                var node = (from e in _dbcontext.TblEmployeeRoles where e.Id == objEmpHierarchyModel.Id && e.TenantId==objEmpHierarchyModel.TenantId select e).FirstOrDefault();
                node.ParentId = objEmpHierarchyModel.ParentId;
                node.RoleOrder = objEmpHierarchyModel.ParentId;
                _dbcontext.Entry(node).State = System.Data.Entity.EntityState.Modified;
                _dbcontext.SaveChanges();
                return SelectAllEmpHierarchy();
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
    }
}