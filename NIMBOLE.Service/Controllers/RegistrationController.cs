using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using NIMBOLE.Common;
using NIMBOLE.Entities;
using System.Web.Http.Cors;
namespace NIMBOLE.Service.Controllers
{       // GET: Registration

      [EnableCors(origins: "*", headers: "*", methods: "*")]

    //  [EnableCors(origins: "http://mywebclient.azurewebsites.net", headers: "*", methods: "*")]
    public class RegistrationController : ApiController
    {


          NIMBOLE.Entities.NIMBOLEContext _dbcontext = new NIMBOLE.Entities.NIMBOLEContext();
          NIMBOLE.Entities.NimboleSuperadminDashboardEntities Obj_dbcontextSuperAdmin = new NIMBOLE.Entities.NimboleSuperadminDashboardEntities();
        public partial class Regobj
        {  
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string siteurl { get; set; }
            public string password { get; set; }
            public int Numberoflicenses { get; set; }
            public int TrialPeriod { get; set; }
        }

          [EnableCors(origins: "*", headers: "*", methods: "*")] 
          public HttpResponseMessage PostReistration(Regobj obj)
        {
             HttpResponseMessage response = null;
           //WebRole1.edmx.NaberlyDashboardEntities entities1 = new WebRole1.edmx.NaberlyDashboardEntities();
           string encryptedPassword = string.Empty;
            try
            {
                if (obj != null)
                {
                    Customer _objCustomer = new Customer();
                        _objCustomer.LastName = obj.LastName;
                        _objCustomer.FirstName = obj.FirstName;

                        _objCustomer.TenantID = Guid.NewGuid();
                        _objCustomer.URL = obj.siteurl;
                        _objCustomer.UserName = obj.Email;
                        _objCustomer.Password = Encrypt(obj.password);
                        _objCustomer.Numberoflicenses = obj.Numberoflicenses;
                        _objCustomer.TrialPeriod = obj.TrialPeriod;
                        _objCustomer.CreatedDate = DateTime.UtcNow;
                        _objCustomer.ModifiedDate = DateTime.UtcNow;
                        _objCustomer.Status = true;
                        //Obj_dbcontextSuperAdmin.Customers.Add(_objCustomer);
                        Obj_dbcontextSuperAdmin.Customers.Add(_objCustomer);
                        Obj_dbcontextSuperAdmin.SaveChanges();
                        string subject = string.Empty;
                        string emailBody = string.Empty;
                        TblSetting objTblSetting = new TblSetting();
                        objTblSetting.FullName = obj.FirstName + " "+ obj.LastName;
                        objTblSetting.DefaultEmail = obj.Email;
                        objTblSetting.Status = true;
                        objTblSetting.URL = obj.siteurl;
                        objTblSetting.CreatedDate = DateTime.UtcNow.Date;
                        encryptedPassword = Helper.Encrypt(obj.password);
                        objTblSetting.Password = encryptedPassword;
                        objTblSetting.ModifiedDate = DateTime.UtcNow.Date;
                        objTblSetting.TenantId = _objCustomer.TenantID;
                        _dbcontext.TblSettings.Add(objTblSetting);
                        _dbcontext.SaveChanges();
                        subject = "Welcome to Nimbole Dashboard";
                        emailBody = "Dear Customer,<br/><br/>";
                        emailBody = emailBody + "Welcome to Nimbole Dashboard. You can login with your email  " + obj.Email + "<br/>";
                        emailBody = emailBody + "Password:  " + Helper.Decrypt(encryptedPassword) + "<br/>";
                        emailBody = emailBody + "Website URL: http://" + obj.siteurl + "<br/>";
                        emailBody = emailBody + "Regards,<br/>";
                        emailBody = emailBody + "The Nimbole Team";
                        EmailSender.SendEmailToNewUser("Welcome to Nimbole Dashboard", emailBody, obj.Email);
                        response = this.Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent("Registration successfully completed", System.Text.Encoding.UTF8, "application/JSON");
                }
            }
            catch (Exception ex)
            {
                response = this.Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Registration is not completed " +ex.InnerException, System.Text.Encoding.UTF8, "application/JSON");
          
            }
            return response;
        }
        public static string Encrypt(string password)
        {
            string encryptedPassword = string.Empty;
            try
            {
                byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(password);
                encryptedPassword = Convert.ToBase64String(b);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return encryptedPassword;
        }

    }
}