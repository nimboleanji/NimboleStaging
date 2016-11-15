
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class SchedulerServiceModel
    {
        #region ISchedulerEvent Members

        public long Id { get; set; }
        public Guid TenantId { get; set; }
        public string Title { get; set; }
        public string EmpId { get; set; }
        public string Type_Task { set; get; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public bool IsActivity { get; set; }
        public string EndTimezone { get; set; }
        public bool IsAllDay { get; set; }
        public string RecurrenceException { get; set; }
        public string RecurrenceRule { get; set; }
        public string StartTimezone { get; set; }

        public string mstart { get; set; }
        public string mend { get; set; }
        #endregion

        #region Constructor
        public SchedulerServiceModel()
        {

        }

        #endregion
    }
}