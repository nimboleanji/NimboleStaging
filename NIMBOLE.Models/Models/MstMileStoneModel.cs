using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace NIMBOLE.Models.Models
{
    public class MstMileStoneModel 
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        #endregion

        #region Constructor
        public MstMileStoneModel()
        {

        }
        #endregion
    }
}