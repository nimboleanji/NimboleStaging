using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Models.Models
{
    public class LanguageModel
    {
        #region Properties
        public Int64 Id { get; set; }
        public Guid TenantId { get; set; }
        public string Code { get; set; }
        public string LanguageName { get; set; }
        #endregion

        #region Constructor
        public LanguageModel()
        {

        }
        #endregion
    }
}
