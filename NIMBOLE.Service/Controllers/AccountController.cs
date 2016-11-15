using AutoMapper;
using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Service.Models;
using NIMBOLE.Common;
using NIMBOLE.Entities;
using System.Data.Entity.Validation;
using System.Data;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();
        HttpResponseMessage response = null;
        string encryptedPassword = string.Empty;

        #region GET
        [HttpGet]
        [Route("ValidateUser")]
        public IHttpActionResult ValidateUser(string username, string password, Guid Tid)
        {
            try
            {
                //var CurrentUser = (from cu in _dbcontext.VWLoginEmployees where cu.EmailAddress == username && cu.Password == password select cu).FirstOrDefault();
                var CurrentUser = _dbcontext.VWLoginEmployees.FirstOrDefault(cu => cu.EmailAddress == username && cu.Password == password && cu.TenantId == Tid);
                if (CurrentUser != null)
                    return Json(CurrentUser);
                else
                    throw new InvalidOperationException("No User found.");
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        #endregion

        #region Push Notification
        [HttpPost]
        [Route("LoginFormobile")]
        public IHttpActionResult LoginFormobile(string username, string password, Guid Tid, string Gcmid)
        {
            try
            {   
                EmployeeModel ObjEmployeeModel = new EmployeeModel();
                var CurrentUser = _dbcontext.VWLoginEmployees.FirstOrDefault(cu => cu.EmailAddress == username && cu.Password == password && cu.TenantId == Tid);
                if (CurrentUser != null)
                {
                    ObjEmployeeModel.GcmId = Gcmid;
                    var ObjTblEmployee = _dbcontext.TblEmployees.Where(x => x.Id == CurrentUser.Id).FirstOrDefault();
                    ObjTblEmployee.GcmId = ObjEmployeeModel.GcmId;
                    _dbcontext.SaveChanges();
                    return Json(CurrentUser);
                }
                else
                {
                    throw new InvalidOperationException("No User found.");
                }
            }
            catch (Exception ex)
            {

                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }

        }
        #endregion

        #region GET
        [HttpPost]
        [Route("InsertCustomer")]
        public IHttpActionResult Post(RegistrationModel _objRegistrationModel)
        {
            try
            {
                string subject = string.Empty;
                string emailBody = string.Empty;
                TblLogin _objTblLogin = new TblLogin();
                _objTblLogin.TenantId = _objRegistrationModel.TenantID;
                _objTblLogin.EmailAddress = _objRegistrationModel.UserEmail;
                _objTblLogin.Password = Helper.Encrypt(_objRegistrationModel.Password);
                _objTblLogin.CreatedDate = DateTime.UtcNow.Date;
                _objTblLogin.ModifiedDate = DateTime.UtcNow.Date;
                _objTblLogin.Status = true;
                _dbcontext.TblLogins.Add(_objTblLogin);
                _dbcontext.SaveChanges();
                //_objTblLogin.Id 
                TblEmployee _objTblEmployee = new TblEmployee();
                _objTblEmployee.TenantId = _objRegistrationModel.TenantID;
                _objTblEmployee.FirstName = _objRegistrationModel.FirstName;
                _objTblEmployee.LastName = _objRegistrationModel.LastName;
                _objTblEmployee.EmployeeEmail = _objRegistrationModel.UserEmail;
                _objTblEmployee.EmployeeImageURL = _objRegistrationModel.Siteurl;
                _objTblEmployee.LoginId = _objTblLogin.Id;
                _objTblEmployee.EmpRoleId = 2;
                _objTblEmployee.Location = "3";
                _objTblEmployee.CreatedDate = DateTime.UtcNow.Date;
                _objTblEmployee.ModifiedDate = DateTime.UtcNow.Date;
                _objTblEmployee.Status = true;
                _dbcontext.TblEmployees.Add(_objTblEmployee);
                _dbcontext.SaveChanges();

                TblSetting objTblSetting = new TblSetting();
                objTblSetting.TenantId = _objRegistrationModel.TenantID;
                objTblSetting.FullName = _objRegistrationModel.FirstName + " " + _objRegistrationModel.LastName;
                objTblSetting.DefaultEmail = _objRegistrationModel.UserEmail;
                objTblSetting.URL = _objRegistrationModel.Siteurl;
                objTblSetting.NoOfLicenses = _objRegistrationModel.Numberoflicenses;
                objTblSetting.CreatedDate = DateTime.UtcNow.Date;
                objTblSetting.ModifiedDate = DateTime.UtcNow.Date;
                objTblSetting.Status = true;
                encryptedPassword = Helper.Encrypt(_objRegistrationModel.Password);
                objTblSetting.Password = encryptedPassword;
                _dbcontext.TblSettings.Add(objTblSetting);
                _dbcontext.SaveChanges();

                TblModule _objTblModule = new TblModule();
                TblModule _objTblModule1 = new TblModule();
                TblModule _objTblModule2 = new TblModule();
                TblModule _objTblModule3 = new TblModule();
                TblModule _objTblModule4 = new TblModule();
                TblModule _objTblModule5 = new TblModule();
                TblModule _objTblModule6 = new TblModule();
                TblModule _objTblModule7 = new TblModule();

                _objTblModule.TenantId = _objRegistrationModel.TenantID;
                _objTblModule.Code = "00";
                _objTblModule.Description = "Home";
                _objTblModule.CreatedDate = DateTime.UtcNow.Date;
                _objTblModule.ModifiedDate = DateTime.UtcNow.Date;
                _objTblModule.Status = true;

                _objTblModule1.TenantId = _objRegistrationModel.TenantID;
                _objTblModule1.Code = "01";
                _objTblModule1.Description = "Masters";
                _objTblModule1.CreatedDate = DateTime.UtcNow.Date;
                _objTblModule1.ModifiedDate = DateTime.UtcNow.Date;
                _objTblModule1.Status = true;

                _objTblModule2.TenantId = _objRegistrationModel.TenantID;
                _objTblModule2.Code = "02";
                _objTblModule2.Description = "Account";
                _objTblModule2.CreatedDate = DateTime.UtcNow.Date;
                _objTblModule2.ModifiedDate = DateTime.UtcNow.Date;
                _objTblModule2.Status = true;

                _objTblModule3.TenantId = _objRegistrationModel.TenantID;
                _objTblModule3.Code = "03";
                _objTblModule3.Description = "Contact";
                _objTblModule3.CreatedDate = DateTime.UtcNow.Date;
                _objTblModule3.ModifiedDate = DateTime.UtcNow.Date;
                _objTblModule3.Status = true;

                _objTblModule4.TenantId = _objRegistrationModel.TenantID;
                _objTblModule4.Code = "04";
                _objTblModule4.Description = "Lead";
                _objTblModule4.CreatedDate = DateTime.UtcNow.Date;
                _objTblModule4.ModifiedDate = DateTime.UtcNow.Date;
                _objTblModule4.Status = true;

                _objTblModule5.TenantId = _objRegistrationModel.TenantID;
                _objTblModule5.Code = "05";
                _objTblModule5.Description = "Settings";
                _objTblModule5.CreatedDate = DateTime.UtcNow.Date;
                _objTblModule5.ModifiedDate = DateTime.UtcNow.Date;
                _objTblModule5.Status = true;

                _objTblModule6.TenantId = _objRegistrationModel.TenantID;
                _objTblModule6.Code = "06";
                _objTblModule6.Description = "Employee";
                _objTblModule6.CreatedDate = DateTime.UtcNow.Date;
                _objTblModule6.ModifiedDate = DateTime.UtcNow.Date;
                _objTblModule6.Status = true;

                _objTblModule7.TenantId = _objRegistrationModel.TenantID;
                _objTblModule7.Code = "07";
                _objTblModule7.Description = "Report";
                _objTblModule7.CreatedDate = DateTime.UtcNow.Date;
                _objTblModule7.ModifiedDate = DateTime.UtcNow.Date;
                _objTblModule7.Status = true;

                _dbcontext.TblModules.Add(_objTblModule);
                _dbcontext.TblModules.Add(_objTblModule1);
                _dbcontext.TblModules.Add(_objTblModule2);
                _dbcontext.TblModules.Add(_objTblModule3);
                _dbcontext.TblModules.Add(_objTblModule4);
                _dbcontext.TblModules.Add(_objTblModule5);
                _dbcontext.TblModules.Add(_objTblModule6);
                _dbcontext.TblModules.Add(_objTblModule7);
                _dbcontext.SaveChanges();

                subject = "Welcome to Nimbole Dashboard";
                emailBody = "Dear Customer,<br/><br/>";
                emailBody = emailBody + "Welcome to Nimbole Dashboard. You can login with your email  " + _objRegistrationModel.UserEmail + "<br/>";
                emailBody = emailBody + "Password:  " + encryptedPassword + "<br/>";
                emailBody = emailBody + "Website URL: http://" + _objRegistrationModel.Siteurl + "<br/>";
                emailBody = emailBody + "Regards,<br/>";
                emailBody = emailBody + "The Nimbole Team";
                EmailSender.SendEmailToNewUser("Welcome to Nimbole Dashboard", emailBody, _objRegistrationModel.UserEmail);
                response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent("Registration successfully completed", System.Text.Encoding.UTF8, "application/JSON");
                return Ok();
            }

            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        #endregion
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
