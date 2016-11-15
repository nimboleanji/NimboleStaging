using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NIMBOLE.Models.Models
{
    public class TreeObject
    {
        public Int64 Id { get; set; }
        public string EDescription{get; set;}
        public Int64 ParentId { get; set; }
        public IList<TreeObject> Children { get; set; }
        public TreeObject()
        {
            Children = new List<TreeObject>();
        }
    }
}
