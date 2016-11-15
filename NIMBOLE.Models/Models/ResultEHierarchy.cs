using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using NIMBOLE.Entities;

namespace NIMBOLE.Models.Models
{
    public class ResultEHierarchy
    {
        public List<EmpHierarchyModel> FlatToHierarchy(List<EmpHierarchyModel> list)
        {
            // hashtable lookup that allows us to grab references to containers based on id
            var lookup = new Dictionary<long, EmpHierarchyModel>();
            // actual nested collection to return
            List<EmpHierarchyModel> nested = new List<EmpHierarchyModel>();

            foreach (EmpHierarchyModel item in list)
            {
                if (lookup.ContainsKey(item.ParentId))
                {
                    // add to the parent's child list 
                    lookup[item.ParentId].lstEHierarchyModel.Add(item);
                }
                else
                {
                    // no parent added yet (or this is the first time)
                    nested.Add(item);
                }
                lookup.Add(item.Id, item);
            }

            return nested;
        }

                  
        //List<EHierarchyModel>
        public void HierarchyToFlat(string strJSon)
        {
            dynamic data = Json.Decode(strJSon);

            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(EHierarchyModel));
            //MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(strJSon));
            //EHierarchyModel parent = (EHierarchyModel)ser.ReadObject(stream);

            //yield return parent;
            //foreach (EHierarchyModel child in parent.lstEHierarchyModel) // check null if you must
            //    foreach (EHierarchyModel relative in HierarchyToFlat(child))
            //        yield return relative;
        }

        public List<OrgHierarchyModel> FlatToHierarchy(List<OrgHierarchyModel> list)
        {
            // hashtable lookup that allows us to grab references to containers based on id
            var lookup = new Dictionary<long, OrgHierarchyModel>();
            // actual nested collection to return
            List<OrgHierarchyModel> nested = new List<OrgHierarchyModel>();

            foreach (OrgHierarchyModel item in list)
            {
                if (lookup.ContainsKey(item.ParentId))
                {
                    // add to the parent's child list 
                    lookup[item.ParentId].lstOrgHierarchyModel.Add(item);
                }
                else
                {
                    // no parent added yet (or this is the first time)
                    nested.Add(item);
                }
                lookup.Add(item.Id, item);
            }
            return nested;
        }




        public IList<TreeObject> FlatToHierarchy(IQueryable<TblEmpHierarchy> list, long? nullable)
        {
            var q = (from i in list
                     where i.ParentId == nullable
                     select new
                     {
                         id = i.Id,
                         parent_id = i.ParentId,
                         EDescription = i.EDescription
                     }).ToList();
            return q.Select(x => new TreeObject
            {
                Children = FlatToHierarchy(list, x.id)
            }).ToList();
        }
    }
}
