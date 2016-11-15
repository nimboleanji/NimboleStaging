using System;
using System.Collections.Generic;
using NIMBOLE.Entities.Core;

namespace NIMBOLE.Models.Models
{
    public class EmployeeTaskModel //: BaseEntity
    {
        #region Properties
        public long Id { get; set; }
        public Guid TenantId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set;}
        public string EmpId { get; set; }
        public string Type_Task { set; get;}
        public DateTime TaskDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Int64 TaskOwnerId { set; get;}
        public Int64 TaskGivenToId { set; get;}
        public string Comments { set; get; }
        public string TaskGiven { set; get; }
        public string Active { get; set; }
        public string Change_status { get; set; }
        public bool IsActivity { set; get; }
        public string Type { set; get; }
        public bool IsSelected { get; set; }
        public List<EmpTaskKeyValueModel1> SelectedValue { get; set; }
        public List<EmpTaskKeyValueModel1> ReferenceIds { get; set; }

        private Nullable<DateTime> _CreatedDate;
        private Nullable<DateTime> _ModifiedDate;
        public DateTime CreatedDate
        {
            get
            {
                if (_CreatedDate != null)
                    return _CreatedDate.Value.Year == 1 ? DateTime.Now : _CreatedDate.Value;
                else
                    return DateTime.Now;
            }
            set
            {
                _CreatedDate = value;
            }
        }
        public DateTime ModifiedDate
        {
            get
            {
                if (_ModifiedDate != null)
                    return _ModifiedDate.Value.Year == 1 ? DateTime.Now : _ModifiedDate.Value;
                else
                    return DateTime.Now;
            }
            set
            {
                _ModifiedDate = value;
            }
        }
        public bool Status { get; set; }
        #endregion
    }

    public class EmpTaskKeyValueModel1
    {
        #region Properties
        public Int64 Id { get; set; }
        public string Name { get; set; }
        #endregion
    }
}
