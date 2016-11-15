
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using System.IO;
using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using System.Web.UI;
using NIMBOLE.Models.Mappers;
using System.Web;
using NIMBOLE.Common;
using NIMBOLE.UI.Helpers;
using NIMBOLE.UI.Models;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace NIMBOLE.UI.Controllers
{
    public class EmployeeHyrarchyController : BaseController
    {
        Guid TenentId = new Guid();
        public EmployeeHyrarchyController()
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["CurrentTenentId"] != null)
                    TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            response = client.GetAsync("api/EmployeeHierarchy/GetData?Tid="+TenentId).Result;
                        
            if (response.IsSuccessStatusCode)
            {
                List<EmpHierarchyModel> objEmpHierarchyModel = response.Content.ReadAsAsync<List<EmpHierarchyModel>>().Result;
                ResultEHierarchy objResultEHierarchy = new ResultEHierarchy();
                if (objEmpHierarchyModel.Count == 0)
                {
                    EmpHierarchyModel _objEmpHierarchyModel = new EmpHierarchyModel();
                    _objEmpHierarchyModel.Id = 0;
                    _objEmpHierarchyModel.EDescription = "No Data";
                    _objEmpHierarchyModel.Data = "No Data";
                    _objEmpHierarchyModel.ParentId = 0;
                    objEmpHierarchyModel.Add(_objEmpHierarchyModel);
                }
                var hdata = objResultEHierarchy.FlatToHierarchy(objEmpHierarchyModel);
                ViewBag.inlineDefault = (IEnumerable<EmpHierarchyModel>)hdata;
                ViewData["EmpHierarchy"] = (IEnumerable<EmpHierarchyModel>)objEmpHierarchyModel;
                
            }


                var responsenew = client.GetAsync("api/EmployeeHierarchy/GetDataNew?Tid=" + TenentId).Result;
                if (responsenew.IsSuccessStatusCode)
                {
                    List<EmpHierarchyModel> objEmpHierarchyModelNew = responsenew.Content.ReadAsAsync<List<EmpHierarchyModel>>().Result;
                    ResultEHierarchy objResultEHierarchyNew = new ResultEHierarchy();
                    if (objEmpHierarchyModelNew.Count == 0)
                    {
                        EmpHierarchyModel _objEmpHierarchyModel = new EmpHierarchyModel();
                        _objEmpHierarchyModel.Id = 0;
                        _objEmpHierarchyModel.EDescription = "No Data";
                        _objEmpHierarchyModel.Data = "No Data";
                        _objEmpHierarchyModel.ParentId = 0;
                        objEmpHierarchyModelNew.Add(_objEmpHierarchyModel);
                    }
                    var hdatanew = objResultEHierarchyNew.FlatToHierarchy(objEmpHierarchyModelNew);
                    // ViewBag.inlineDefaultVal = (IEnumerable<EmpHierarchyModel>)hdatanew;
                    ViewBag.inlineDefaultVal = (IEnumerable<EmpHierarchyModel>)objEmpHierarchyModelNew;
                    ViewData["EmpHierarchyNew"] = (IEnumerable<EmpHierarchyModel>)objEmpHierarchyModelNew;
                    return View();
                }                                         
                        
            return null;
        }
         [HttpGet]
        public JsonResult ReadList([DataSourceRequest] DataSourceRequest request)
        {
             response = client.GetAsync("api/EmployeeHierarchy/GetDataNew?Tid="+TenentId).Result;


            if (response.IsSuccessStatusCode)
            {
                List<EmpHierarchyModel> objEmpHierarchyModel = response.Content.ReadAsAsync<List<EmpHierarchyModel>>().Result;
                ResultEHierarchy objResultEHierarchy = new ResultEHierarchy();
                if (objEmpHierarchyModel.Count == 0)
                {
                    EmpHierarchyModel _objEmpHierarchyModel = new EmpHierarchyModel();
                    _objEmpHierarchyModel.Id = 0;
                    _objEmpHierarchyModel.EDescription = "No Data";
                    _objEmpHierarchyModel.Data = "No Data";
                    _objEmpHierarchyModel.ParentId = 0;
                    objEmpHierarchyModel.Add(_objEmpHierarchyModel);
                }
                var hdata = objResultEHierarchy.FlatToHierarchy(objEmpHierarchyModel);
              //  ViewBag.inlineDefaultVal = (IEnumerable<EmpHierarchyModel>)hdata;
                ViewBag.inlineDefaultVal = (IEnumerable<EmpHierarchyModel>)objEmpHierarchyModel;
                ViewData["EmpHierarchy"] = (IEnumerable<EmpHierarchyModel>)objEmpHierarchyModel;

           //     var result = hdata.ToTreeDataSourceResult(request,
           //    e => e.Id,
           //    e => e.ParentId,
           //    //e => e.EDescription,
           //    e => e.ToEmpHierarchyModel()
              
           //);

           //     return Json(result, JsonRequestBehavior.AllowGet);

                return Json(objEmpHierarchyModel.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        

        [HttpPost]
        public ActionResult AddNode(string txtNode, int parentId, EmpHierarchyModel objEmpHierarchyModel)
        {
            objEmpHierarchyModel.TenantId = TenentId;
            objEmpHierarchyModel.ParentId = parentId;
            objEmpHierarchyModel.EDescription = txtNode;
              response = client.PostAsJsonAsync("api/EmployeeHierarchy/AddNode", objEmpHierarchyModel).Result;
            if (response.IsSuccessStatusCode)
            {
                var objOrgHierarchyModel1 = response.Content.ReadAsAsync<IEnumerable<EmpHierarchyModel>>().Result;
                return Content("Success");
            }
            return null;
        }
        [HttpPost]
        public ActionResult ChangeParentNode(int nodeId, int targetNodeId,EmpHierarchyModel objEmpHierarchyModel)
        {
            objEmpHierarchyModel.TenantId = TenentId;
            objEmpHierarchyModel.Id = nodeId;
            objEmpHierarchyModel.ParentId = targetNodeId;
            response = client.PostAsJsonAsync("api/EmployeeHierarchy/ChangeParent", objEmpHierarchyModel).Result;
            if (response.IsSuccessStatusCode)
            {
               // var resultModel = response.Content.ReadAsAsync<EmpHierarchyModel>().Result;
               // var resultModel = JsonConvert.DeserializeObject<List<RetrieveMultipleResponse>>(JsonStr);
                return Content("Success");
            }
            return null;
        }
        [HttpPost]
        public ActionResult UpdateNode(string txtOldName, string txtNewName, EmpHierarchyModel objEmpHierarchyModel)
        {
            objEmpHierarchyModel.TenantId = TenentId;
            objEmpHierarchyModel.TxtOldName = txtOldName;
            objEmpHierarchyModel.Data = txtNewName;

            response = client.PostAsJsonAsync("api/EmployeeHierarchy/UpdateNode", objEmpHierarchyModel).Result;
            if (response.IsSuccessStatusCode)
            {
                var objEmpHierarchyModelResult = response.Content.ReadAsAsync<IEnumerable<EmpHierarchyModel>>().Result;
                return Content("Success");
            }
            return null;
        }
        
        [HttpPost]
        public ActionResult DeleteNode(string txtNode, EmpHierarchyModel objEmpHierarchyModel)
        {
            objEmpHierarchyModel.TenantId = TenentId;
            objEmpHierarchyModel.TxtNode = txtNode;
            response = client.PostAsJsonAsync("api/EmployeeHierarchy/DeleteNode", objEmpHierarchyModel).Result;
            if (response.IsSuccessStatusCode)
            {
                var objHierarchyModelResult = response.Content.ReadAsAsync<IEnumerable<EmpHierarchyModel>>().Result;
                return Content("Success");
            }
            return null;
        }
        public ActionResult AcHierarchyToFlat(string id)
        {
            (new NIMBOLE.Models.Models.ResultEHierarchy()).HierarchyToFlat(id);
            return Content("Welcome");
        }
        private IEnumerable<EmpHierarchyModel> GetDefaultInlineData()
        {
            List<EmpHierarchyModel> inlineDefault = new List<EmpHierarchyModel>
                {
                    new EmpHierarchyModel
                    {
                        EDescription = "Furniture",
                        lstEHierarchyModel = new List<EmpHierarchyModel>
                        {
                            new EmpHierarchyModel()
                            {
                                EDescription= "Tables & Chairs"                                
                            },
                            new EmpHierarchyModel
                            {
                                 EDescription = "Sofas"
                            },
                            new EmpHierarchyModel
                            {
                                 EDescription= "Occasional Furniture"
                            }
                        }
                    },
                    new EmpHierarchyModel
                    {
                        EDescription= "Decor",
                        lstEHierarchyModel = new List<EmpHierarchyModel>
                        {
                            new EmpHierarchyModel()
                            {
                                EDescription = "Bed Linen"                                
                            },
                            new EmpHierarchyModel
                            {
                                 EDescription= "Curtains & Blinds"
                            },
                            new EmpHierarchyModel
                            {
                                 EDescription= "Carpets"
                            }
                        }
                    }
                };

            return inlineDefault;
        }
        public ActionResult Remote_Data_Binding()
        {
            return View();
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