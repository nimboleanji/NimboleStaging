using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class MenuItemModel
    {
        public MenuItemModel()
        {
            this.ChildMenuItems= new List<MenuItemModel>();
        }    
        public int MenuItemId {get;set;}
        public string MenuItemName { get; set; }
        public string MenuItemPath { get; set; }
        public Nullable<int> ParentItemId { get; set; }
        public virtual ICollection<MenuItemModel> ChildMenuItems { get; set; }
    }
}
