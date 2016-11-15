using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NIMBOLE.Models.Models;
using System.Web.Security;
using System.Configuration;
using NIMBOLE.Models;
using AutoMapper;
using System.Net.Http;
using System.Net.Http.Headers;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Web.UI;
using NIMBOLE.Models.Mappers;
using System.Threading.Tasks;

namespace NIMBOLE.UI.Controllers
{
    //[CustomAuthorize]
    //[ValidateInput(false)]
    public class AccountController : Controller
    {
        AccountExcelImport excelImport = new AccountExcelImport();
        string strAPIURL = string.Empty;
        HttpClient client;
        HttpResponseMessage response;
        Uri contactUri = null;
        SettingModel objClientModel = new SettingModel();
        DTO objNIMBOLEMapper = new DTO();

        Guid TenentId = new Guid();


        #region Login
        public AccountController()
        {
            try
            {
                strAPIURL = ConfigurationManager.AppSettings["ServiceUrl"].ToString();
                contactUri = new Uri(strAPIURL);
                client = new HttpClient();
                client.BaseAddress = contactUri;
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                response = new HttpResponseMessage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message.ToString());
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {


            //string domainname = Request.Url.Authority;
            // string domainname = "test.Nimbole.net";
            //response = client.GetAsync("api/GetUrl/GetUrlTenantId?SubDomain=" + domainname).Result;
            //Task<string> t = response.Content.ReadAsStringAsync();


            //if (response.ReasonPhrase == "OK")
            //{

              //  System.Web.HttpContext.Current.Session["CurrentTenentId"] = new Guid(t.Result.Split(':')[1].ToString());

                //if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                //    returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

                //if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
                //{
                //    ViewBag.ReturnURL = returnUrl;
                //}

                //return View();
            //}
            //else
            //{
            //    return RedirectToAction("AccessDenied", "Error");

            //}

            //return RedirectToAction("NotFound", "Error");

            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnURL = returnUrl;
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TransLoginModel login, string ReturnUrl)
        {
            try
            {
                string password = string.Empty, username = string.Empty;
                username = login.objLoginModel.EmailAddress;
                password = login.objLoginModel.Password;
                System.Web.HttpContext.Current.Session["CurrentTenentId"] = login.objLoginModel.TenantId;
                TenentId = new Guid(System.Web.HttpContext.Current.Session["CurrentTenentId"].ToString());


                    if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
                    {
                       
                        response = client.GetAsync("api/Account/ValidateUser?username=" + username + "&password=" + password + "&Tid=" + TenentId).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var CurrentUser = response.Content.ReadAsAsync<NIMBOLE.Models.Models.LoginModel>().Result;
                            if (CurrentUser != null)
                            {
                                CurrentUser.Password = password;
                                FormsAuthentication.SetAuthCookie(login.objLoginModel.EmailAddress, true);
                                Session["User"] = CurrentUser.EmailAddress;
                                Session["UserId"] = CurrentUser.LoginId;
                                Session["Id"] = CurrentUser.Id;

                                if (!string.IsNullOrEmpty(ReturnUrl))
                                {
                                    var url = ReturnUrl.Split('/');
                                    var empty = url[0];
                                    var controllerName = url[1];
                                    var actionName = url[2];
                                    if (actionName == "Edit")
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }
                                    else { return RedirectToLocal(ReturnUrl); }
                                }
                                else
                                {
                                    return RedirectToLocal(ReturnUrl);
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                                login.objLoginModel = CurrentUser;
                                return View(login);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Both Username and Password are Mandatory.");
                        return View(login);
                    }


                //}
                //else
                //{
                //    return View(login);
                //}
               
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message.ToString());
                return View(login);
            }
            ModelState.Clear();
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(login);

        }

        #endregion

        #region Change Password
        //[Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //[Authorize]
        //[HttpPost]
        //public ActionResult ChangePassword(Changepassword changePassword)
        //{
        //    try
        //    {
        //        string currentPassword = string.Empty;
        //        string newPassword = string.Empty;
        //        string confirmPassword = string.Empty;
        //        string fullName = string.Empty;
        //        string email = string.Empty;
        //        string subject = string.Empty;
        //        string emailBody = string.Empty;
        //        string encryptNewPassword = string.Empty;
        //        string logedinUserPassword = string.Empty;


        //        currentPassword = changePassword.CurrentPassword;
        //        newPassword = changePassword.NewPassword;
        //        confirmPassword = changePassword.ConfirmPassword;

        //        email =User.Identity.Name;

        //        //var getMember = (from m in dbcontext.TblContacts where m.ContactName == email select m).FirstOrDefault();
        //        //if (getMember != null)
        //        //{
        //        //    //logedinUserPassword = Common.Decrypt(getMember.MemPwd);
        //        //    //encryptNewPassword = Common.Encrypt(newPassword);


        //        //    if (logedinUserPassword == currentPassword)
        //        //    {
        //        //        //getMember.MemPwd = encryptNewPassword;

        //        //        //getMember.CurrentPwd = currentPassword;
        //        //        //getMember.NewPwd = newPassword;
        //        //        //getMember.ConfirmPwd = confirmPassword;

        //        //        //int status = BccDb.SaveChanges();

        //        //        //if (status > 0)
        //        //        //{
        //        //        //    fullName = getMember.FirstName + " " + getMember.LastName;

        //        //        //    subject =  " Password Change";
        //        //        //    emailBody = "Dear " + fullName + ",<br/><br/>";
        //        //        //    emailBody = emailBody + "Password successfully changed. <br/><br/>";
        //        //        //    emailBody = emailBody + "Your new  password for the account " + email + " has been set.<br/><br/><br/>";
        //        //        //    emailBody = emailBody + "Regards,<br/>";
        //        //        //    emailBody = emailBody + "Intercity Transit";
        //        //        //    EmailSender.SendEmailToNewUser(subject, emailBody, getMember.EmailAdd);


        //        //        //    return RedirectToAction("Index", "Home");
        //        //        //}
        //        //        //else
        //        //        //    ViewData["VerificationError"] = "New password should not be same as current password.";                        
        //        //    }
        //        //    else
        //        //        ViewData["VerificationError"] = "The current password is incorrect.";
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return View();

        //}


        #endregion

        #region Forgot Password

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgotPassword(AccountModel forgotPassword)
        {
            try
            {
                //var getMember = (from m in dbcontext.TblContacts where m.ContactName == forgotPassword.UserName select m).FirstOrDefault();

                //if (getMember != null)
                //{
                //    string decryptPassword = string.Empty;
                //    string fullName = string.Empty;
                //    string subject = string.Empty;
                //    string emailBody = string.Empty;

                //decryptPassword = Common.Decrypt(getMember.MemPwd);

                //decryptPassword = getMember.MemPwd;

                // fullName = getMember.FirstName + " " + getMember.LastName ;

                // subject = "Forgot password ";

                // emailBody = "Dear " + fullName + ",<br/><br/>";

                // emailBody = emailBody + "We received a request for Forgot password for <a style='text-decoration: inherit;'> " + forgotPassword.UserName + "</a>.<br/><br/>";

                // emailBody = emailBody + "Your Current Password :  " + decryptPassword + "<br/><br/><br/>";

                // emailBody = emailBody + "Regards,<br/>";

                // emailBody = emailBody + "Intersity Transit";

                // EmailSender.SendEmailToNewUser(subject, emailBody, getMember.EmailAdd);

                // ViewData["forgotPwdSuccess"] = "Your password has been sent to your email successfully!"; // ___________, your password has been emailed to you

                // ViewData["forgotPwdSuccess"] = "Hi " + fullName + ", Your password has been emailed to you.";

                // ModelState.Clear();                                

                //ViewData["forgotPwdSuccess"] = "Hai " + fullName + " Your password is : " + decryptPassword ;

                //ModelState.Clear();
                //}
                //else

                ViewData["UserNotFound"] = "Invalid User Name.";
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }

        #endregion

        #region Logout
        public ActionResult LogoutMethod()
        {
            Session.Abandon();

            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();

            // Clear authentication cookie.
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            //HttpCookie aCookie;
            //string cookieName;
            //int limit = Request.Cookies.Count;
            //for (int i = 0; i < limit; i++)
            //{

            //    cookieName = Request.Cookies[i].Name;
            //    aCookie = new HttpCookie(cookieName);
            //    aCookie.Expires = DateTime.Now.AddDays(-1);
            //    Response.Cookies.Add(aCookie);
            //}
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion Helpers
    }
}

