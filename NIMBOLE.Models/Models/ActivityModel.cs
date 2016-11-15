using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class ActivityModel  
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public Int64 MileStoneId { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }
        public string Type_A_E_M { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ActivityComments { get; set; }
        public bool RequireNotify { get; set; }
        public Int64 ReferenceId { get; set; }
        public List<SelectListItem> ReferenceIds { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
        //public virtual ICollection<TransAccConModel> lstTransAccConModel { get; set; }

        #endregion

        #region Constructor
        public ActivityModel()
        {
            ReferenceIds = new List<System.Web.Mvc.SelectListItem>();
        }
        #endregion
    }
    public class NotifyActivityModel//:Kendo.Mvc.UI.Fluent
    {
        public Int64 Id { get; set; }
        public string title { get; set; }
        //public DateTime ActivityDate { get; set; }
        public DateTime getTimezoneOffset { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public NotifyActivityModel()
        {
            getTimezoneOffset = new DateTime();
        }
    }
    public class MyActivityModel
    {
        public LeadModel objLeadModel { get; set; }
        public string[] refIds { get; set; }
        public List<string> lstURLs { get; set; }
        public string currentUser { get; set; }
        public Guid TenantId { get; set; }
    }
    public class MobileActivityModel
    {
        public Guid TenantId { get; set; }
        public Int64 LeadId { get; set; }
        public ActivityModel objActivityModel { get; set; }
        public DocumentModel objDocumentModel { get; set; }
        public string[] refIds { get; set; }
        public List<string> lstURLs { get; set; }
        public string currentUser { get; set; }

        public string Latitude {get;set;}
        public string Longitude {get;set;}
        public DateTime PostDateTime {get;set;}
        public Int64 ActivityId {get;set;}
        public Int64 EmpId {get;set;}
        public bool Status {get;set;}       
	
    }


    public class MobileLatLogModel
    {
        public Guid TenantId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Int64 EmpId { get; set; }
    }


    public class MapLatLogModel
    {
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Int64 EmpId { get; set; }
        public Int64 ActivityId { get; set; }
        public DateTime PostDateTime { get; set; }
        public Int64 LeadId { get; set; }
        public string LeadTitle { get; set; }
    }
}
