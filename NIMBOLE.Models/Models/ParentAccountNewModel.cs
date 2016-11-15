using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class ParentAccountNewModel //: NIMBOLE.Entities.TblEmployeeRole
    {
        #region Properties
        public Int64 Id { get; set; }

        public string AccountName { get; set; }
       
        #endregion

        #region Constructor
        public ParentAccountNewModel()
        {

        }
        #endregion
    }
}
