using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.UI.Models;

namespace NIMBOLE.UI.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            /*this.SetAlert(new AlertMessageViewModel()
            {
                MessageType = MessageType.Error,
                MessageHeading = "Failure",
                MessageString = "Record Not Found"
            });*/
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("NotFound");
            //return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}