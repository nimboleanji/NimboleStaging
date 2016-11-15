using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.Entities.Models;
using NIMBOLE.Entities;
using System.Net.Http;
using System.Net.Http.Headers;


namespace NIMBOLE.UI.Controllers
{
    public class LoginController : Controller
    {
       
        NIMBOLE.Entities.NIMBOLEContext dbcontext;        
        HttpClient client;
        HttpResponseMessage response;
        Uri contactUri = null;
        BaseController bc;

        public LoginController()
        {
            try
            {
                string strAPIURL = string.Empty;
                strAPIURL = "http://localhost:6390/";
                contactUri = new Uri(strAPIURL);
                dbcontext = new NIMBOLE.Entities.NIMBOLEContext();
                bc = new BaseController();
                client = new HttpClient();

                client.BaseAddress = contactUri;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                response = new HttpResponseMessage();
                //bc.CurrentUser = "abc@nimbole.com";
                var user = (from u in dbcontext.TblLogins where u.EmailAddress == bc.CurrentUser select u).FirstOrDefault();
                bc.CurrentUser = user.EmailAddress;
                bc.CultureName = (from c in dbcontext.TblSettings where c.TenantId == user.TenantId select c.LanguageCode).FirstOrDefault();
                bc.ModifyCurrentCulture();
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(bc.CultureName);
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }

        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }
	}
}