
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI;
using NIMBOLE.Models.Models;

namespace NIMBOLE.UI.Models
{
    public class SchedulerModel : ISchedulerEvent
    {
        #region ISchedulerEvent Members
        
        public string Description { get; set; }

        public DateTime End { get; set; }

        public string EndTimezone { get; set; }

        public bool IsAllDay { get; set; }

        public string RecurrenceException { get; set; }

        public string RecurrenceRule { get; set; }

        public DateTime Start { get; set; }

        public string StartTimezone { get; set; }
         
        public string Title { get; set; }

        //Other Fields
        public Guid TenantId { get; set; }
        public string EmpId { get; set; }
        public bool IsActivity { get; set; }
        public long Id { get; set; }
        #endregion
    }
}