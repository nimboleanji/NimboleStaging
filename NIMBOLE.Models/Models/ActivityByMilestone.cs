using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace NIMBOLE.Models.Models
{
    public class ActivityByMilestone
    {
        #region Properties
        public Int64 Id { get; set; }
        public string ActivityTitle { get; set; }
        public DateTime ActivityDate { get; set; }
        #endregion

        #region Constructor
        public ActivityByMilestone()
        {

        }
        #endregion
    }

}

