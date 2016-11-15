using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Threading;
using System.Net.Http;
using NIMBOLE.Entities.Models;
using AutoMapper;
using NIMBOLE.Entities;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using NIMBOLE.Entities.Mappers;
using System.Data.Entity.Validation;

namespace NIMBOLE.UI.Controllers
{
    public class UsersController : BaseController
    {
        DTO objNIMBOLEMapper = new DTO();

        #region Local Variable Declarations
        NIMBOLE.Entities.NIMBOLEContext dbcontext;
        System.Net.Http.HttpClient client;
        System.Net.Http.HttpResponseMessage response;
        Uri contactUri = null;
        BaseController bc;
        string strAPIURL = string.Empty;
        #endregion
        
        #region Constructor
        public UsersController()
        {
            try
            {
                //strAPIURL = "http://localhost:6390/";
                strAPIURL = ConfigurationManager.AppSettings["ServiceUrl"].ToString();
                contactUri = new Uri(strAPIURL);
                dbcontext = new NIMBOLE.Entities.NIMBOLEContext();
                bc = new BaseController();
                client = new System.Net.Http.HttpClient();

                client.BaseAddress = contactUri;
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                
                response = new System.Net.Http.HttpResponseMessage();
                bc.CurrentUser = "abc@nimbole.com";
                var user = (from u in dbcontext.TblLogins where u.EmailAddress == bc.CurrentUser select u).FirstOrDefault();
                
                bc.CurrentUser = user.EmailAddress;
                bc.CultureName = (from c in dbcontext.TblSettings where c.TenantId == user.TenantId select c.LanguageCode).FirstOrDefault();
                bc.ModifyCurrentCulture();
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(bc.CultureName);
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            }
            catch (Exception ex)
            {
              //  System.Diagnostics.Debug.Print(ex.Message);
            }
            
        }
        #endregion
        public ActionResult User_Read([DataSourceRequest] DataSourceRequest request)
        {           
            response = client.GetAsync("api/Users/GetAll").Result;
            if (response.IsSuccessStatusCode)
            {
                var users = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Entities.Models.UserModel>>().Result;
                return Json(users.AsQueryable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        [HttpPost]
       // public ActionResult User_Create(NIMBOLE.Entities.Models.UserModel objUserModel)
        public ActionResult User_Create(UserModel objUserModel)
        {

            LoginModel objLoginModel = objUserModel.objLoginModel;

            objLoginModel.EmailAddress = objUserModel.UserEmail;
            objLoginModel.Password = "123";
            objLoginModel.TenantId = new Guid(Session["CurrentTenentId"].ToString());
            objLoginModel.CreatedDate = DateTime.Now;
            objLoginModel.ModifiedDate = DateTime.Now;
            objLoginModel.Status = true;

            response = client.PostAsJsonAsync("api/Login/Insert", objLoginModel).Result;
            if (response.IsSuccessStatusCode)
                objUserModel.objLoginModel = response.Content.ReadAsAsync<NIMBOLE.Entities.Models.LoginModel>().Result;
            

            //Entities.TblUser objTblUser = new Entities.TblUser();

            objUserModel.UserCode = objUserModel.UserCode;
            objUserModel.FirstName = objUserModel.FirstName;
            objUserModel.LastName = objUserModel.LastName;
            objUserModel.LoginId = 3;
            objUserModel.MobileNo = objUserModel.MobileNo;
          
            objUserModel.CreatedDate = DateTime.Now;
            objUserModel.ModifiedDate = DateTime.Now;
            objUserModel.Status = true;
            objUserModel.TenantId = new Guid(Session["CurrentTenentId"].ToString());

            try
            {

            
            TblUser objTblUser = new TblUser();

            TblLogin objTblLogin = new TblLogin();
            TblEmployeeRole objTblEmployeeRole = new TblEmployeeRole();

            objTblUser = objNIMBOLEMapper.MapModel2Table(objUserModel);
            dbcontext.TblUsers.Add(objTblUser);
            dbcontext.SaveChanges();
            objUserModel.Id = objTblUser.Id;

}
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


            AddressModel objAddressModel = objUserModel.objAddressModel;

            objAddressModel.HouseNo = objUserModel.objAddressModel.HouseNo;
            objAddressModel.StreetName = objUserModel.objAddressModel.StreetName;
            objAddressModel.CityId = objUserModel.objAddressModel.CityId;
            objAddressModel.Mobile = objUserModel.objAddressModel.Mobile;
            objAddressModel.Phone = objUserModel.objAddressModel.Phone;
            objAddressModel.HomePhone = objUserModel.objAddressModel.HomePhone;
            objAddressModel.ZipCode = objUserModel.objAddressModel.ZipCode;
            objAddressModel.Fax = objUserModel.objAddressModel.Fax;
            objAddressModel.SkypeName = objUserModel.objAddressModel.SkypeName;
            objAddressModel.Address_Type = "U";

            objAddressModel.TenantId = new Guid(Session["CurrentTenentId"].ToString());
            objAddressModel.CreatedDate = DateTime.Now;
            objAddressModel.ModifiedDate = DateTime.Now;
            objAddressModel.Status = true;


            response = client.PostAsJsonAsync("api/Address/Insert", objAddressModel).Result;
            if (response.IsSuccessStatusCode)
                objAddressModel = response.Content.ReadAsAsync<NIMBOLE.Entities.Models.AddressModel>().Result;
            

            UserAddressModel objUserAddressModel = new UserAddressModel();
            objUserAddressModel.AddressId = objAddressModel.Id;
            objUserAddressModel.UserId = objUserModel.Id;

            response = client.PostAsJsonAsync("api/AddressEmployee/Insert", objUserAddressModel).Result;
            if (response.IsSuccessStatusCode)
                objUserAddressModel = response.Content.ReadAsAsync<NIMBOLE.Entities.Models.UserAddressModel>().Result;
                      
           
            return RedirectToAction("Index");        
        }
        
        #region GET
        [HttpGet]
        public ActionResult GetAllUsers()
        {
            response = client.GetAsync("api/Users/SelectAllUsers").Result;
            if (response.IsSuccessStatusCode)
            {
                var objUserModel = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Entities.Models.UserModel>>().Result;
                return Json(objUserModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        
        // GET: /User/
        [HttpGet]
        public ActionResult Index()
        {
            //response = client.GetAsync("api/Users/GetAll").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var languages = response.Content.ReadAsAsync<IEnumerable<NIMBOLE.Entities.Models.UserModel>>().Result;
            //    return View(languages);
            //}
            //else
            //{
            //    return RedirectToAction("Create");
            //}
            return View();
        }
        #endregion
        public ActionResult Create()
        {
            return View();
        }

        #region POST
        [HttpPost]
        //   public ActionResult Create(UserModel objUserModel)
        public ActionResult Create(UserModel objUserModel)
        {

            LoginModel objLoginModel = objUserModel.objLoginModel;

            objLoginModel.EmailAddress = objUserModel.UserEmail;
            objLoginModel.Password = "123";
            objLoginModel.TenantId = new Guid(Session["CurrentTenentId"].ToString());
            objLoginModel.CreatedDate = DateTime.Now;
            objLoginModel.ModifiedDate = DateTime.Now;
            objLoginModel.Status = true;

            response = client.PostAsJsonAsync("api/Login/Insert", objLoginModel).Result;
            if (response.IsSuccessStatusCode)
                objLoginModel = response.Content.ReadAsAsync<LoginModel>().Result;


            AddressModel objAddressModel = objUserModel.objAddressModel;

            objAddressModel.HouseNo = objUserModel.objAddressModel.HouseNo;
            objAddressModel.StreetName = objUserModel.objAddressModel.StreetName;
            objAddressModel.CityId = objUserModel.objAddressModel.CityId;
            objAddressModel.Mobile = objUserModel.objAddressModel.Mobile;
            objAddressModel.Phone = objUserModel.objAddressModel.Phone;
            objAddressModel.HomePhone = objUserModel.objAddressModel.HomePhone;
            objAddressModel.ZipCode = objUserModel.objAddressModel.ZipCode;
            objAddressModel.Fax = objUserModel.objAddressModel.Fax;
            objAddressModel.SkypeName = objUserModel.objAddressModel.SkypeName;
            objAddressModel.Address_Type = "U";

            objAddressModel.TenantId = new Guid("7EA74E34-4F73-4609-A4F6-E0FA46E14D9E");
            objAddressModel.CreatedDate = DateTime.Now;
            objAddressModel.ModifiedDate = DateTime.Now;
            objAddressModel.Status = true;

            response = client.PostAsJsonAsync("api/Address/Insert", objAddressModel).Result;
            if (response.IsSuccessStatusCode)
                objAddressModel = response.Content.ReadAsAsync<NIMBOLE.Entities.Models.AddressModel>().Result;

            TblUser objTblUser = new TblUser();

            objTblUser.Code = objUserModel.UserCode;
            objTblUser.UserEmail = objUserModel.UserEmail;
            objTblUser.FirstName = objUserModel.FirstName;
            objTblUser.LastName = objUserModel.LastName;

            objTblUser.TenantId = new Guid("7EA74E34-4F73-4609-A4F6-E0FA46E14D9E");
            objTblUser.DOB = DateTime.Now;
            objTblUser.CreatedDate = DateTime.Now;
            objTblUser.ModifiedDate = DateTime.Now;
            objTblUser.MobileNo = objAddressModel.Mobile;
            objTblUser.LoginId = objLoginModel.Id;
            objTblUser.Status = true;

            dbcontext.TblUsers.Add(objTblUser);
            // dbcontext.SaveChanges();

            TblUserAddress objTblUserAddress = new TblUserAddress();
            objTblUserAddress.AddressId = objAddressModel.Id;
            objTblUserAddress.UserId = objTblUser.Id;
            objTblUserAddress.TenantId = new Guid("7EA74E34-4F73-4609-A4F6-E0FA46E14D9E");

            dbcontext.TblUserAddresses.Add(objTblUserAddress);
            dbcontext.SaveChanges();


            return RedirectToAction("Index");

        }
        #endregion
                
        //#region PUT
        //[HttpGet]
        //// HTTP:GET  /Users/Edit/1
        //public ActionResult Edit(int id)
        //{
        //    response = client.GetAsync("api/Users/GetById?id=" + id).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var objTblUser = response.Content.ReadAsAsync<NIMBOLE.Entities.TblUser>().Result;
        //        //UserModel objUserModel = Mapper.Map<TblUser, UserModel>(objTblUser);
        //        UserModel objUserModel = objNIMBOLEMapper.MapTable2Model(objTblUser);
        //        return View(objUserModel);
        //    }
        //    return View("Record Not Found");
        //}
        //// HTTP:PUT  /Users/Edit/
        //public async System.Threading.Tasks.Task<ActionResult> Edit(UserModel objUserModel)
        //{
        //    //Call Insert WEBAPI using          http://localhost:6390/api/Clients/Edit
        //    try
        //    {
        //        //response = await client.PutAsJsonAsync(gizmoUrl, gizmo);
        //        //TblUser objTblUser = Mapper.Map<UserModel, TblUser>(objUserModel);
        //        TblUser objTblUser = objNIMBOLEMapper.MapModel2Table(objUserModel);
        //        objTblUser.TenantId = new Guid(Session["CurrentTenentId"].ToString());
        //        var response = await client.PutAsJsonAsync<TblUser>("api/Users/Edit", objTblUser);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var objResultValue = await response.Content.ReadAsAsync<TblUser>();
        //            ViewData["objResultValue"] = objResultValue;
        //            return Redirect("~/Users/Index");
        //        }
        //        else
        //        {
        //            // add something here to tell the user hey, something went wrong
        //            return null;
        //        }
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        //#endregion

        #region Edit
        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            TblUser objTblUser = (from c in dbcontext.TblUsers where c.Id == id select c).FirstOrDefault();
            UserModel objUserModel = new UserModel();

            objUserModel.UserCode = objTblUser.Code;
            objUserModel.FirstName = objTblUser.FirstName;
            objUserModel.LastName = objTblUser.LastName;
            //objUserModel.LoginId = objTblUser.LoginId;
            objUserModel.UserEmail = objTblUser.UserEmail;

            TblUserAddress objTblUserAddress=(from ua in dbcontext.TblUserAddresses where ua.UserId==id select ua).FirstOrDefault();

            TblAddress objTblAddress = (from a in dbcontext.TblAddresses where a.Id == objTblUserAddress.AddressId select a).FirstOrDefault();

            objUserModel.objAddressModel.HouseNo = objTblAddress.HouseNo;
            objUserModel.objAddressModel.StreetName = objTblAddress.StreetName;
            objUserModel.objAddressModel.ZipCode = objTblAddress.ZipCode;
            objUserModel.objAddressModel.Phone = objTblAddress.Phone;
            objUserModel.objAddressModel.HomePhone = objTblAddress.HomePhone;
            objUserModel.objAddressModel.Mobile = objTblAddress.Mobile;
            objUserModel.objAddressModel.Fax = objTblAddress.Fax;
            objUserModel.objAddressModel.SkypeName = objTblAddress.SkypeName;
           

            return View(objUserModel);
        }

        // POST: users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, UserModel objUserModel)
        {
            try
            {
                TblUser query = (from x in dbcontext.TblUsers where x.Id == id select x).FirstOrDefault();

                query.FirstName = objUserModel.FirstName;
                query.LastName = objUserModel.LastName;
                query.Code = objUserModel.UserCode;

                query.UserEmail = objUserModel.UserEmail;
                query.ModifiedDate = DateTime.Now;


                TblUserAddress objTblUserAddress = (from ua in dbcontext.TblUserAddresses where ua.UserId == id select ua).FirstOrDefault();

                TblAddress objTblAddress = (from a in dbcontext.TblAddresses where a.Id == objTblUserAddress.AddressId select a).FirstOrDefault();



                objTblAddress.HouseNo = objUserModel.objAddressModel.HouseNo;
                objTblAddress.StreetName = objUserModel.objAddressModel.StreetName;
                objTblAddress.ZipCode = objUserModel.objAddressModel.ZipCode;
                objTblAddress.Phone = objUserModel.objAddressModel.Phone;
                objTblAddress.HomePhone = objUserModel.objAddressModel.HomePhone;
                objTblAddress.Mobile = objUserModel.objAddressModel.Mobile;
                objTblAddress.Fax = objUserModel.objAddressModel.Fax;
                objTblAddress.SkypeName = objUserModel.objAddressModel.SkypeName;
                
                dbcontext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region DELETE
        [HttpDelete]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            

            //TblUserAddress objTblUserAddress = (from ua in dbcontext.TblUserAddresses where ua.UserId == id select ua).FirstOrDefault();

            //TblAddress objTblAddress = (from a in dbcontext.TblAddresses where a.Id == objTblUserAddress.AddressId select a).FirstOrDefault();

            //dbcontext.TblUserAddresses.Remove(objTblUserAddress);

            //dbcontext.TblAddresses.Remove(objTblAddress);

            //dbcontext.SaveChanges();

                    //http://localhost:6390/api/Users/Delete?id=1
                    response = await client.DeleteAsync(strAPIURL + "api/Users/Delete?Id=" + id);

                    if (response.IsSuccessStatusCode)
                    {
                        var objTblUser = response.Content.ReadAsAsync<NIMBOLE.Entities.TblUser>().Result;
                        return View(objTblUser.Id);
                    }

                    return View();
        }
        #endregion
    }
}