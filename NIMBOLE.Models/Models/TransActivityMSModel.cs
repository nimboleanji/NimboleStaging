using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NIMBOLE.Models.Models
{
    public class TransActivityMSModel
    {
        public virtual ActivityModel objActivityModel { get; set; }
        public virtual MilestoneModel objMileStoneModel { get; set; }
    }
}
