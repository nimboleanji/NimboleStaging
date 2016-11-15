using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using AutoMapper;
using NIMBOLE.Models;

namespace NIMBOLE.UI.Controllers
{
    public class OrganisationHierarchyController : BaseController
    {

        Guid TenentId = new Guid();

        public OrganisationHierarchyController()
        {
            if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
        }
        [HttpGet]
        public ActionResult SelectAllOrgHierarchy()
        {
            response = client.GetAsync("api/OrgHierarchy/SelectAllOrgHierarchy?Tid="+ TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objOrgHierarchyModel = response.Content.ReadAsAsync<IEnumerable<OrgHierarchyModel>>().Result;
                return Json(objOrgHierarchyModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpGet]
        public ActionResult GetAllOrgHierarchy()
        {
            response = client.GetAsync("api/OrgHierarchy/GetAllOrgHierarchy?Tid="+TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                var objOrgHierarchyModel = response.Content.ReadAsAsync<IEnumerable<OrgHierarchyModel>>().Result;
                return Json(objOrgHierarchyModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        [HttpGet]
        public ActionResult Index()
        {
            response = client.GetAsync("api/OrgHierarchy/GetData?Tid=" + TenentId).Result;
            if (response.IsSuccessStatusCode)
            {
                List<EmployeeDisplayModel> objEmployeeDisplayModel = response.Content.ReadAsAsync<List<EmployeeDisplayModel>>().Result;
                var hdata = objEmployeeDisplayModel;
                ViewBag.inlineDefault = (IEnumerable<EmployeeDisplayModel>)hdata;
                return View((IEnumerable<EmployeeDisplayModel>)hdata);
            }
            return null;
        }
        [HttpPost]
        public ActionResult AddNode(string txtNode, int parentId, OrgHierarchyModel objOrgHierarchyModel)
        {
            objOrgHierarchyModel.TenantId = TenentId;
            objOrgHierarchyModel.ParentId = parentId;
            objOrgHierarchyModel.TxtNewName = txtNode;
            objOrgHierarchyModel.TenantId = TenentId;
            response = client.PostAsJsonAsync("api/OrgHierarchy/AddNode", objOrgHierarchyModel).Result;
            if (response.IsSuccessStatusCode)
            {
                var objOrgHierarchyModel1 = response.Content.ReadAsAsync<IEnumerable<OrgHierarchyModel>>().Result;
                return Content("Success");
            }
            return null;
        }
        [HttpPost]
        public ActionResult ChangeParentNode(int nodeId, int targetNodeId, OrgHierarchyModel objOrgHierarchyModel)
        {
            objOrgHierarchyModel.TenantId = TenentId;
            objOrgHierarchyModel.Id = nodeId;
            objOrgHierarchyModel.ParentId = targetNodeId;
            objOrgHierarchyModel.LevelDepth = targetNodeId;

            response = client.PostAsJsonAsync("api/OrgHierarchy/ChangeParent", objOrgHierarchyModel).Result;
            if (response.IsSuccessStatusCode)
            {
                //var resultModel = response.Content.ReadAsAsync<OrgHierarchyModel>().Result;
                return Content("Success");
            }
            return null;
        }
        [HttpPost]
        public ActionResult UpdateNode(string txtOldName, string txtNewName, OrgHierarchyModel objOrgHierarchyModel)
        {
            objOrgHierarchyModel.TxtOldName = txtOldName;
            objOrgHierarchyModel.TxtNewName = txtNewName;

            response = client.PostAsJsonAsync("api/OrgHierarchy/UpdateNode", objOrgHierarchyModel).Result;
            if (response.IsSuccessStatusCode)
            {
                var objOrgHierarchyModelResult = response.Content.ReadAsAsync<IEnumerable<OrgHierarchyModel>>().Result;
                return Content("Success");
            }
            return null;
        }
        [HttpPost]
        public ActionResult DeleteNode(int nodeId, OrgHierarchyModel objOrgHierarchyModel)
        {
            objOrgHierarchyModel.Id = nodeId;
            response = client.PostAsJsonAsync("api/OrgHierarchy/DeleteNode", objOrgHierarchyModel).Result;
            if (response.IsSuccessStatusCode)
            {
                var objHierarchyModelResult = response.Content.ReadAsAsync<IEnumerable<OrgHierarchyModel>>().Result;
                return Content("Success");
            }
            return null;
        }
        public ActionResult AcHierarchyToFlat(string id)
        {
            (new ResultEHierarchy()).HierarchyToFlat(id);
            return Content("Welcome");
        }
        private IEnumerable<OrgHierarchyModel> GetDefaultInlineData()
        {
            List<OrgHierarchyModel> inlineDefault = new List<OrgHierarchyModel>
                {
                    new OrgHierarchyModel
                    {
                        OrgFullName = "Furniture",
                        lstOrgHierarchyModel = new List<OrgHierarchyModel>
                        {
                            new OrgHierarchyModel()
                            {
                                OrgFullName= "Tables & Chairs"                                
                            },
                            new OrgHierarchyModel
                            {
                                 OrgFullName = "Sofas"
                            },
                            new OrgHierarchyModel
                            {
                                 OrgFullName= "Occasional Furniture"
                            }
                        }
                    },
                    new OrgHierarchyModel
                    {
                        OrgFullName= "Decor",
                        lstOrgHierarchyModel = new List<OrgHierarchyModel>
                        {
                            new OrgHierarchyModel()
                            {
                                OrgFullName = "Bed Linen"                                
                            },
                            new OrgHierarchyModel
                            {
                                 OrgFullName= "Curtains & Blinds"
                            },
                            new OrgHierarchyModel
                            {
                                 OrgFullName= "Carpets"
                            }
                        }
                    }
                };

            return inlineDefault;
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
    }
}