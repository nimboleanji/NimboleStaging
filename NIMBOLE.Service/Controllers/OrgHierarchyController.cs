//using NIMBOLE.Models.Models;
//using NIMBOLE.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using NIMBOLE.Entities;

//namespace NIMBOLE.Service.Controllers
//{
//    [RoutePrefix("api/OrgHierarchy")]
//    public class OrgHierarchyController : ApiController
//    {
//        private NIMBOLEContext dbcontext = new NIMBOLEContext();

//        #region Get

//        public IEnumerable<NIMBOLE.Models.Models.OrgHierarchyModel> GetModel()
//        {
//            try
//            {
//                var query = (from x in dbcontext.TblOrgHierarchies select x).ToList();
//                List<OrgHierarchyModel> lstOrgHierarchyModel = new List<OrgHierarchyModel>();
//                foreach (var item in query)
//                {
//                    AutoMapper.Mapper.CreateMap<TblOrgHierarchy, OrgHierarchyModel>();
//                    var result = AutoMapper.Mapper.Map<TblOrgHierarchy, OrgHierarchyModel>(item);
//                    lstOrgHierarchyModel.Add(result);
//                }
//                ResultEHierarchy objResultEHierarchy = new ResultEHierarchy();
//                var hdata = objResultEHierarchy.FlatToHierarchy(lstOrgHierarchyModel);
//                return hdata;
//            }
//            catch (Exception ex)
//            {
//                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
//                {
//                    ReasonPhrase = ex.Message
//                });
//            }           
//        }

//        [HttpGet]
//        [Route("SelectAllOrgHierarchy")]
//        public IHttpActionResult SelectAllOrgHierarchy(Guid Tid)
//        {
//            try
//            {
//                var data = (from x in dbcontext.TblOrgHierarchies where x.TenantId==Tid select new { Id = x.ParentId ?? 0, OrgFullName = x.OrgFullName }).ToList();
//                return Ok(data);
//            }
//            catch (Exception ex)
//            {
//                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
//                {
//                    ReasonPhrase = ex.Message
//                });
//            }           
//        }

//        [HttpGet]
//        [Route("GetAllOrgHierarchy")]
//        public IHttpActionResult GetAllOrgHierarchy(Guid Tid)
//        {
//            try
//            {
//                //var data = (from x in dbcontext.TblOrgHierarchies where x.TenantId==Tid orderby x.OrgFullName select new { Id = x.Id, OrgFullName = x.OrgFullName }).ToList();
//                var data = (from x in dbcontext.TblEmployeeRoles where x.TenantId == Tid orderby x.Description select new { Id = x.Id, Desription = x.Description }).ToList();
//                return Ok(data);
//            }
//            catch (Exception ex)
//            {
//                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
//                {
//                    ReasonPhrase = ex.Message
//                });
//            }           
//        }

//        [HttpGet]
//        [Route("GetData")]
//        public IHttpActionResult GetData(Guid Tid)
//        {
//            try
//            {
//                List<OrgHierarchyModel> ObjOrgHierarchyModel = new List<OrgHierarchyModel>();
//                var query = (from q in dbcontext.TblOrgHierarchies where q.TenantId==Tid select q).ToList();

//                foreach (var items in query)
//                {
//                    ObjOrgHierarchyModel.Add(new OrgHierarchyModel()
//                    {
//                        Id = items.Id,
//                        ParentId = items.ParentId ?? 0,
//                        OrgFullName = items.OrgFullName
//                    });
//                }
//                return Ok(ObjOrgHierarchyModel);
//            }
//            catch (Exception ex)
//            {
//                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
//                {
//                    ReasonPhrase = ex.Message
//                });
//            }            
//        }



//        [HttpGet]
//        [Route("GetAll")]
//        public IHttpActionResult GetAll(Guid Tid)
//        {
//            try
//            {
//                var empRoles = (from r in dbcontext.TblOrgHierarchies where  r.TenantId == Tid && r.ParentId != 0 select new { r.ParentId, r.Id, r.OrgFullName }).ToList();

//                List<OrgHierarchyModel> lstOrgHierarchyModel = new List<OrgHierarchyModel>();
//                foreach (var item in empRoles)
//                {

//                    OrgHierarchyModel objOrgHierarchyModel = new OrgHierarchyModel();
//                    //objEmployeeRoleModel.CreatedDate = item.CreatedDate ?? DateTime.Now;
//                    //objEmployeeRoleModel.ModifiedDate = item.ModifiedDate ?? DateTime.Now;
//                    //objEmployeeRoleModel.ERoleCode = item.Code;
//                    objOrgHierarchyModel.OrgFullName = item.OrgFullName;
//                    objOrgHierarchyModel.Id = item.Id;
//                    objOrgHierarchyModel.ParentId = item.ParentId ?? 0;

//                    lstOrgHierarchyModel.Add(objOrgHierarchyModel);
//                }
//                if (lstOrgHierarchyModel != null)
//                    return Ok(lstOrgHierarchyModel);
//                else
//                    throw new InvalidOperationException("Record not Found.");
//            }

//            catch (Exception ex)
//            {
//                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
//                {
//                    ReasonPhrase = ex.Message
//                });
//            }
//        }

//        [HttpGet]
//        [Route("GetAllNew")]
//        public IHttpActionResult GetAllNew(Guid Tid)
//        {
//            try
//            {
//                var empRoles = (from r in dbcontext.TblOrgHierarchies where r.TenantId == Tid && (r.ParentId == null || r.ParentId == 0) select new { r.Id, r.OrgFullName }).ToList();

//                List<OrgHierarchyModel> lstOrgHierarchyModel = new List<OrgHierarchyModel>();
//                foreach (var item in empRoles)
//                {

//                    OrgHierarchyModel objOrgHierarchyModel = new OrgHierarchyModel();

//                    objOrgHierarchyModel.OrgFullName = item.OrgFullName;
//                    objOrgHierarchyModel.Id = item.Id;

//                    lstOrgHierarchyModel.Add(objOrgHierarchyModel);
//                }

//                if (lstOrgHierarchyModel != null)
//                    return Ok(lstOrgHierarchyModel);
//                else
//                    throw new InvalidOperationException("Record not Found.");
//            }
//            catch (Exception ex)
//            {
//                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
//                {
//                    ReasonPhrase = ex.Message
//                });
//            }
//        }


//        #endregion

//        #region Post
//        [HttpPost]
//        [Route("AddNode")]
//        public IHttpActionResult AddNode(OrgHierarchyModel objOrgHierarchyModel)
//        {
//            try
//            {
//                TblOrgHierarchy _objOrgHierarchy = new TblOrgHierarchy();
//                _objOrgHierarchy.OrgFullName = objOrgHierarchyModel.TxtNode;
//                _objOrgHierarchy.ParentId = objOrgHierarchyModel.ParentId;
//                dbcontext.TblOrgHierarchies.Add(_objOrgHierarchy);
//                dbcontext.SaveChanges();

//                return SelectAllOrgHierarchy(_objOrgHierarchy.TenantId);
//            }
//            catch (Exception ex)
//            {
//                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
//                {
//                    ReasonPhrase = ex.Message
//                });
//            }            
//        }

//        //[HttpPost]
//        //[Route("UpdateNode")]
//        //public IHttpActionResult UpdateNode(OrgHierarchyModel objOrgHierarchyModel)
//        //{
//        //    try
//        //    {
//        //        var node = (from e in dbcontext.TblOrgHierarchies where e.OrgFullName == objOrgHierarchyModel.TxtOldName select e).FirstOrDefault();
//        //        if (node != null)
//        //        {
//        //            node.OrgFullName = objOrgHierarchyModel.TxtNewName;
//        //        }
//        //        dbcontext.Entry(node).State = System.Data.Entity.EntityState.Modified;
//        //        dbcontext.SaveChanges();
//        //        return SelectAllOrgHierarchy();
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        throw new InvalidOperationException(ex.Message);
//        //    }
//        //}


//        [HttpPost]
//        [Route("UpdateNode")]
//        public IHttpActionResult UpdateNode(EmployeeRoleModel objEmployeeRoleModel)
//        {
//            try
//            {
//                var node = (from e in dbcontext.TblEmployeeRoles where e.Description == objEmployeeRoleModel.TxtOldName select e).FirstOrDefault();
//                if (node != null)
//                {
//                    node.Description = objEmployeeRoleModel.TxtNewName;
//                }
//                dbcontext.Entry(node).State = System.Data.Entity.EntityState.Modified;
//                dbcontext.SaveChanges();
//                return SelectAllOrgHierarchy(objEmployeeRoleModel.TenantId);
//            }
//            catch (Exception ex)
//            {
//                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
//                {
//                    ReasonPhrase = ex.Message
//                });
//            }            
//        }

//        [HttpPost]
//        [Route("DeleteNode")]
//        public IHttpActionResult DeleteNode(OrgHierarchyModel objOrgHierarchyModel)
//        {
//            try
//            {
//                var node = (from e in dbcontext.TblOrgHierarchies where e.OrgFullName == objOrgHierarchyModel.TxtNode select e).FirstOrDefault();
//                dbcontext.Entry(node).State = System.Data.Entity.EntityState.Deleted;
//                dbcontext.SaveChanges();
//                return SelectAllOrgHierarchy(objOrgHierarchyModel.TenantId);
//            }
//            catch (Exception ex)
//            {
//                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
//                {
//                    ReasonPhrase = ex.Message
//                });
//            }           
//        }

//        //[HttpPost]
//        //[Route("ChangeParent")]
//        //public IHttpActionResult ChageParent(OrgHierarchyModel objOrgHierarchyModel)
//        //{
//        //    try
//        //    {
//        //        var node = (from e in dbcontext.TblOrgHierarchies where e.Id == objOrgHierarchyModel.Id select e).FirstOrDefault();
//        //        node.ParentId = objOrgHierarchyModel.ParentId;
//        //        dbcontext.Entry(node).State = System.Data.Entity.EntityState.Modified;
//        //        dbcontext.SaveChanges();
//        //        return SelectAllOrgHierarchy();
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        throw new InvalidOperationException(ex.Message);
//        //    }
//        //}


//        [HttpPost]
//        [Route("ChangeParent")]
//        public IHttpActionResult ChageParent(EmployeeRoleModel objEmployeeRoleModel)
//        {
//            try
//            {
//                var node = (from e in dbcontext.TblEmployeeRoles where e.Id == objEmployeeRoleModel.Id select e).FirstOrDefault();
//                node.RoleOrder = objEmployeeRoleModel.RoleOrder;
//                dbcontext.Entry(node).State = System.Data.Entity.EntityState.Modified;
//                dbcontext.SaveChanges();
//                return SelectAllOrgHierarchy(objEmployeeRoleModel.TenantId);
//            }
//            catch (Exception ex)
//            {
//                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
//                {
//                    ReasonPhrase = ex.Message
//                });
//            }            
//        }
//        #endregion
//    }
//}


using NIMBOLE.Models.Models;
using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Entities;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/OrgHierarchy")]
    public class OrgHierarchyController : ApiController
    {
        private NimboleStagingEntities dbcontext = new NimboleStagingEntities();

        #region Get

        public IEnumerable<NIMBOLE.Models.Models.OrgHierarchyModel> GetModel()
        {
            try
            {
                var query = (from x in dbcontext.TblOrgHierarchies select x).ToList();
                List<OrgHierarchyModel> lstOrgHierarchyModel = new List<OrgHierarchyModel>();
                foreach (var item in query)
                {
                    AutoMapper.Mapper.CreateMap<TblOrgHierarchy, OrgHierarchyModel>();
                    var result = AutoMapper.Mapper.Map<TblOrgHierarchy, OrgHierarchyModel>(item);
                    lstOrgHierarchyModel.Add(result);
                }
                ResultEHierarchy objResultEHierarchy = new ResultEHierarchy();
                var hdata = objResultEHierarchy.FlatToHierarchy(lstOrgHierarchyModel);
                return hdata;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        [HttpGet]
        [Route("SelectAllOrgHierarchy")]
        public IHttpActionResult SelectAllOrgHierarchy(Guid Tid)
        {
            try
            {
                //var data = (from x in dbcontext.TblOrgHierarchies select new { Id = x.ParentId ?? 0, OrgFullName = x.OrgFullName }).ToList();
                var data = (from x in dbcontext.TblOrgHierarchyNews where x.TenantId==Tid select new { Id = x.Id, OrgFullName = x.Description, NodeId = x.Node_Id, LevelDepth = x.Level_Depth }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllOrgHierarchy")]
        public IHttpActionResult GetAllOrgHierarchy(Guid Tid)
        {
            try
            {
                //var data = (from x in dbcontext.TblOrgHierarchies orderby x.OrgFullName select new { Id = x.Id, OrgFullName = x.OrgFullName }).ToList();
                var data = (from x in dbcontext.TblOrgHierarchyNews where x.TenantId==Tid select new { Id = x.Under_Parent ?? 0, OrgFullName = x.Description, NodeId = x.Node_Id, LevelDepth = x.Level_Depth }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetData")]
        public IHttpActionResult GetData(Guid Tid)
        {
            try
            {
                //List<OrgHierarchyModel> ObjOrgHierarchyModel = new List<OrgHierarchyModel>();
                //var query = (from q in dbcontext.TblOrgHierarchies select q).ToList();
                // var query = (from q in dbcontext.TblOrgHierarchyNews select q).ToList();
                //                select  Description,count(under_parent)as NoofBranches,under_parent from tblorghierarchynew 
                //group by under_parent, Description order by under_parent 

                //var pl = from r in info
                //         orderby r.metric
                //         group r by r.metric into grp
                //         select new { key = grp.Key, cnt = grp.Count() };

                //            List<ResultLine> result = Lines
                //.GroupBy(l => l.ProductCode)
                //.Select(cl => new ResultLine
                //{
                //    ProductName = cl.First().Name,
                //    Quantity = cl.Count().ToString(),
                //    Price = cl.Sum(c => c.Price).ToString(),
                //}).ToList();

                //var queryres = (from q in dbcontext.TblOrgHierarchyNews  select q); 
                ////orderby q.Under_Parent group q by q.Under_Parent into grp select new {key=grp.Key, cnt=grp.Count() }).ToList(); 
                //var query = queryres.GroupBy(g => g.Under_Parent).Select(s => new OrgHierarchyModel {Id=s.FirstOrDefault().Id, OrgFullName = s.FirstOrDefault().Description, LevelDepth = s.Count(), ParentId = s.FirstOrDefault().Under_Parent ?? 0 }).ToList();
                //foreach (var items in query)
                //{
                //    ObjOrgHierarchyModel.Add(new OrgHierarchyModel()
                //    {
                //        //Id = items.Id,
                //        //ParentId = items.Under_Parent ?? 0,
                //        //OrgFullName = items.Description,
                //        //LevelDepth = items.Level_Depth ?? 0



                //    });
                //}
                //return Ok(ObjOrgHierarchyModel);

                //             var query =
                //from post in database.Posts
                //join meta in database.Post_Metas on post.ID equals meta.Post_ID
                //where post.ID == id
                //select new { Post = post, Meta = meta };

                //                select e.Id,e.ReportToId as ParentId,e.FirstName + '' +e.LastName as Name ,e.EmpRoleId,er.Description as Title,er.RoleOrder,e.EmployeeEmail as Mail, 
                //e.EmployeeImageUrl as Image
                //from tblemployee e 
                //join tblemployeerole er on e.emproleid=er.id
                //order by er.roleorder
                //id: 6, parentId: 1, name: "Rebecca Randall", title: "Optometrist", phone: "801-920-9842", mail: "JasonWGoodman@armyspy.com", image: "images/f-27.jpg"  

                List<EmployeeDisplayModel> ObjEmployeeDisplayModel = new List<EmployeeDisplayModel>();

                var query = (from e in dbcontext.TblEmployees
                             join er in dbcontext.TblEmployeeRoles on e.EmpRoleId equals er.Id
                             where e.TenantId == Tid && er.TenantId == Tid
                             select new { Id = e.Id, ParentId = e.ReportToId, Name = e.FirstName + " " + e.LastName, Title = er.Description, Phone = "123", Mail = e.EmployeeEmail, Address = "123", Image = e.EmployeeImageURL!=null?e.EmployeeImageURL:"noimage" , RoleOrder = er.RoleOrder, EmpRoleId = e.EmpRoleId }).OrderBy(e => e.ParentId).ToList();


                //var query = (from e in dbcontext.TblEmployees
                //             join er in dbcontext.TblEmployeeRoles on e.EmpRoleId equals er.Id
                //             where e.TenantId == Tid && er.TenantId == Tid
                //             select new { Id = e.Id, ParentId = e.ReportToId, Name = e.FirstName + " " + e.LastName, Title = er.Description, Phone = "123", Mail = e.EmployeeEmail, Address = "123", Image = e.EmployeeImageURL!=null?e.EmployeeImageURL:"No Image", RoleOrder = er.RoleOrder, EmpRoleId = e.EmpRoleId }).OrderBy(e => e.RoleOrder).ToList();

                return Json(query);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        #endregion

        #region Post
        [HttpPost]
        [Route("AddNode")]
        public IHttpActionResult AddNode(OrgHierarchyModel objOrgHierarchyModel)
        {
            try
            {
                TblOrgHierarchyNew _objOrgHierarchy = new TblOrgHierarchyNew();
                _objOrgHierarchy.TenantId = objOrgHierarchyModel.TenantId;
                _objOrgHierarchy.Description = objOrgHierarchyModel.TxtNode;
                _objOrgHierarchy.TenantId = objOrgHierarchyModel.TenantId;
                _objOrgHierarchy.Node_Id = objOrgHierarchyModel.ParentId;
                _objOrgHierarchy.Under_Parent = objOrgHierarchyModel.ParentId;
                _objOrgHierarchy.Level_Depth = objOrgHierarchyModel.ParentId;
                dbcontext.TblOrgHierarchyNews.Add(_objOrgHierarchy);
                dbcontext.SaveChanges();

                return SelectAllOrgHierarchy(_objOrgHierarchy.TenantId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateNode")]
        public IHttpActionResult UpdateNode(OrgHierarchyModel objOrgHierarchyModel)
        {
            try
            {
                var node = (from e in dbcontext.TblOrgHierarchyNews where  e.Description == objOrgHierarchyModel.TxtOldName select e).FirstOrDefault();
                if (node != null)
                {
                    node.Description = objOrgHierarchyModel.TxtNewName;
                }
                dbcontext.Entry(node).State = System.Data.Entity.EntityState.Modified;
                dbcontext.SaveChanges();
                return SelectAllOrgHierarchy(node.TenantId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteNode")]
        public IHttpActionResult DeleteNode(OrgHierarchyModel objOrgHierarchyModel)
        {
            try
            {
                var node = (from e in dbcontext.TblOrgHierarchyNews where e.Id == objOrgHierarchyModel.Id select e).FirstOrDefault();
                dbcontext.Entry(node).State = System.Data.Entity.EntityState.Deleted;
                dbcontext.SaveChanges();
                return SelectAllOrgHierarchy(node.TenantId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        [HttpPost]
        [Route("ChangeParent")]
        public IHttpActionResult ChageParent(OrgHierarchyModel objOrgHierarchyModel)
        {
            try
            {
                var node = (from e in dbcontext.TblOrgHierarchyNews where e.Id == objOrgHierarchyModel.Id select e).FirstOrDefault();
                node.Under_Parent = objOrgHierarchyModel.ParentId;
                node.Node_Id = objOrgHierarchyModel.ParentId;
                node.Level_Depth = objOrgHierarchyModel.ParentId;
                dbcontext.Entry(node).State = System.Data.Entity.EntityState.Modified;
                dbcontext.SaveChanges();
                return SelectAllOrgHierarchy(node.TenantId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        #endregion
    }
}
