using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace NIMBOLE.Models.Models
{
   public  class NotificationModel
    {
        #region Properties
        public string Descriptions { get; set; }
        public Int64 Id { get; set; }
        public Int64 LeadId { get; set; }
        public Int64 AccountId { get; set; }
        public Int64 MileStoneId { get; set; }
        public DateTime ActivityDate { get; set; }
        public string Title { get; set; }
        public string LeadTitle { get; set; }
        public string AccountTitle { get; set; }
        public string LeadDescription { get; set; }
        public Int64 LeadOwnerId { get; set; }
        public string ActivityComments { get; set; }

        public string ActivityTitle { get; set; }
        public DateTime CreatedDate { get; set; }

        #endregion

    }
}
