using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI;
using NIMBOLE.Models.Models;

namespace NIMBOLE.UI.Models
{
    public class HandleErrorInfoModel
    {
        //public HandleErrorInfoModel(Exception exception, string controllerName, string actionName);

        public string ActionName { set; get; }

        public string ControllerName { set; get; }

        public Exception Exception { set; get; }
    }
}