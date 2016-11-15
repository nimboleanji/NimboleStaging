using NIMBOLE.Entities;
using NIMBOLE.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NIMBOLE.WebAPI.Controllers
{
    public class ApiHomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
