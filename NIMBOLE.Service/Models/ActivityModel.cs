using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.UI;

namespace NIMBOLE.Entities.Models
{
    public class NotifyActivityModel : ISchedulerEvent
    {
        public Int64 Id { get; set; }
        public string Title { get; set; }
        //public DateTime ActivityDate { get; set; }
        public string ActivityStartDate { get; set; }
        public string ActivityEndDate { get; set; }
    }
    public class MyActivityModel
    {
        public LeadModel objLeadModel { get; set; }
        public string[] refIds { get; set; }
        public List<string> lstURLs { get; set; }
        public string currentUser { get; set; }
    }

}
