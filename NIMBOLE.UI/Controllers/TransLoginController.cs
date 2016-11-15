using NIMBOLE.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;

namespace NIMBOLE.UI.Controllers
{
    public class TransLoginController : BaseController
    {
        public TransLoginController()
        {
            try
            {
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }
        //
        // GET: /TransLogin/
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(TransLoginModel login)
        {
            try
            {
                string password = string.Empty, username = string.Empty;
                // password =  login.Password;
                username = login.objLoginModel.EmailAddress;
                password = login.objLoginModel.Password;

                var CurrentUser = (from cu in dbcontext.TblLogins where cu.EmailAddress == username && cu.Password == password select cu).FirstOrDefault();
                if (CurrentUser != null)
                {
                    FormsAuthentication.SetAuthCookie(login.objLoginModel.EmailAddress, true);
                    Session["User"] = login.objLoginModel.EmailAddress;

                    //CurrentUser.TenantId=new Guid("7ea74e34-4f73-4609-a4f6-e0fa46e14d9e");

                    Session["CurrentTenentId"] = CurrentUser.TenantId;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            ModelState.Clear();
            return View(login);

        }
        


	}
}