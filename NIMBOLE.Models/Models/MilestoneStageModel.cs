using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class MilestoneStageModel
    {
        public Int64 Id { get; set; }
        public string MileStoneStage { get; set; }
        public string EmployeeRoles { get; set; }
        public List<string> Roles { get; set; }

        public Guid TenantId { get; set; }

        public MilestoneStageModel()
        {

        }
    }
    
}
