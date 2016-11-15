using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
  public  class ProductTypeModel
    {
        #region Properties
      public Guid TenantId { get; set; }
      public Int64 ProductTypeId { get; set; }
        public string ProductType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
       
        #endregion

        #region Constructor
        public ProductTypeModel()
        {

        }

        #endregion
    }
}
