using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace NIMBOLE.Models.Models
{
    public class MilestoneModel 
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string MileStoneStage { get; set; }
        public int MSOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        public String Active { get; set; }
        public string Change_status { get; set; }


        #endregion

        #region Constructor
        public MilestoneModel()
        {
           
        }
        #endregion
    }
}