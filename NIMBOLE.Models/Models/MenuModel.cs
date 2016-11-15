using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class MenuModel
    {
        public MenuModel()
        {
            Items = new List<MenuItemModel>();
        }
        public List<MenuItemModel> Items;
    }
}
