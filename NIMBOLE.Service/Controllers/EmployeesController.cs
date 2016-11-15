using NIMBOLE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Models.Models;
using NIMBOLE.Models.Mappers;
using NIMBOLE.Entities;
using NIMBOLE.Common;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Employees")]
    public class EmployeesController : ApiController
    {
        private static Object thisLock = new Object();
        DTO objNIMBOLEMapper = new DTO();
        // GET api/employees
        private NimboleStagingEntities _dbcontext = new NimboleStagingEntities();

        #region GET

        [HttpGet]
        [Route("GetAllEmpLogin")]
        public IHttpActionResult GetAllEmpLogin()
        {
            try
            {
                //sreedhar changed on 16/10/2015

                //var emplogin =
                //(
                //   from e in _dbcontext.TblEmployees
                //   join l in _dbcontext.TblLogins on e.LoginId equals l.Id into el                   
                //   select new { e, el }
                //).ToList();

                //var emplogin = (from EMP in _dbcontext.VWLgnEmployees select EMP).ToList();
                var emplogin = _dbcontext.VWLgnEmployees;
                if (emplogin != null)
                    return Ok(emplogin);
                else
                    throw new InvalidOperationException("Record not found.");
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll(Guid Tid)
        {
            try
            {
                //sreedhar changed on 16/10/2015
                var query = (from x in _dbcontext.TblEmployees where x.TenantId == Tid && x.Id != 1 && x.Id != 145 orderby x.Id descending select x).ToList();
                List<EmployeeModel> emplist_data = new List<EmployeeModel>();
                foreach (var item in query)
                {
                    emplist_data.Add(new EmployeeModel() { Id = item.Id, FirstName = item.FirstName, LastName = item.LastName, EmployeeEmail = item.EmployeeEmail, Status = Convert.ToBoolean(item.Status) });
                }
                if (query != null)
                    return Ok(emplist_data);
                else
                    throw new InvalidOperationException("Record not found.");
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("GetByIdAndAddressDetails")]
        public IHttpActionResult GetByIdAndAddressDetails(int id, Guid Tid)
        {
            try
            {
                //var objTblEmployee = (from c in _dbcontext.TblEmployees where c.Id == id && c.TenantId == Tid select c).FirstOrDefault();
                var objTblEmployee = _dbcontext.TblEmployees.FirstOrDefault(c => c.Id == id && c.TenantId == Tid);

                var EmpDesignationId = (from e in _dbcontext.TblEmployees where e.Id == objTblEmployee.ReportToId && e.TenantId == Tid select e.EmpRoleId).FirstOrDefault();

                if (objTblEmployee != null)
                {
                    EmployeeModel objEmployeeModel = new EmployeeModel();
                    objEmployeeModel = new EmployeeModel();
                    objEmployeeModel = objNIMBOLEMapper.MapTable2Model(objTblEmployee);
                    objEmployeeModel.EmpDesiganationRoleId = EmpDesignationId;
                    //TblAddressEmployee objTblAddressEmployee = (from adEmp in _dbcontext.TblAddressEmployees where adEmp.EmployeeId == id && adEmp.TenantId == Tid select adEmp).FirstOrDefault();
                    TblAddressEmployee objTblAddressEmployee = _dbcontext.TblAddressEmployees.FirstOrDefault(adEmp => adEmp.EmployeeId == id && adEmp.TenantId == Tid);

                    if (objTblAddressEmployee != null)
                    {
                        //var objTblAddress = (from adModel in _dbcontext.TblAddresses where adModel.Id == objTblAddressEmployee.AddressId && adModel.TenantId == Tid select adModel).FirstOrDefault();
                        var objTblAddress = _dbcontext.TblAddresses.FirstOrDefault(adModel => adModel.Id == objTblAddressEmployee.AddressId && adModel.TenantId == Tid);

                        if (objTblAddress != null)
                        {
                            objEmployeeModel.objAddressModel = new AddressModel();
                            objEmployeeModel.objAddressModel = objNIMBOLEMapper.MapTable2Model(objTblAddress);

                            var iCityId = objEmployeeModel.objAddressModel.CityId;
                            var iStateId = objEmployeeModel.objAddressModel.StateId;
                            var iCountryId = objEmployeeModel.objAddressModel.CountryId;

                            if (iCityId > 0)
                            {
                                CommanMethods objCommanMethods = new CommanMethods();
                                GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                                objGeneralValuesModel = objCommanMethods.GetCountryStateCity("City", objEmployeeModel.objAddressModel.CityId);
                                objEmployeeModel.objAddressModel.City = objGeneralValuesModel.Description;
                            }
                            if (iStateId > 0)
                            {
                                CommanMethods objCommanMethods = new CommanMethods();
                                GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                                objGeneralValuesModel = objCommanMethods.GetCountryStateCity("State", objEmployeeModel.objAddressModel.StateId);
                                objEmployeeModel.objAddressModel.State = objGeneralValuesModel.Description;
                                objEmployeeModel.objAddressModel.StateId = objGeneralValuesModel.Id;
                            }
                            if (iCountryId > 0)
                            {
                                CommanMethods objCommanMethods = new CommanMethods();
                                GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                                objGeneralValuesModel = objCommanMethods.GetCountryStateCity("Country", objEmployeeModel.objAddressModel.CountryId);
                                objEmployeeModel.objAddressModel.Country = objGeneralValuesModel.Description;
                                objEmployeeModel.objAddressModel.CountryId = objGeneralValuesModel.Id;
                                //objTransAccConModel.objAddressModel.Country = objGeneralValuesModel.Description;
                            }
                        }
                        //if (objTblAddress != null)
                        //{
                        //    objEmployeeModel.objAddressModel = new AddressModel();
                        //    objEmployeeModel.objAddressModel = objNIMBOLEMapper.MapTable2Model(objTblAddress);

                        //    var iCityId = objEmployeeModel.objAddressModel.CityId;
                        //    if (iCityId > 0)
                        //    {
                        //        CommanMethods objCommanMethods = new CommanMethods();
                        //        GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                        //        objGeneralValuesModel = objCommanMethods.GetCountryStateCity("City", objEmployeeModel.objAddressModel.CityId);
                        //        objEmployeeModel.objAddressModel.City = objGeneralValuesModel.Description;
                        //        objGeneralValuesModel = new GeneralValuesModel();
                        //        objGeneralValuesModel = objCommanMethods.GetCountryStateCity("State", objEmployeeModel.objAddressModel.CityId);
                        //        objEmployeeModel.objAddressModel.StateId = objGeneralValuesModel.Id;
                        //        objEmployeeModel.objAddressModel.State = objGeneralValuesModel.Description;
                        //        objGeneralValuesModel = objCommanMethods.GetCountryStateCity("Country", objGeneralValuesModel.Id);
                        //        objEmployeeModel.objAddressModel.CountryId = objGeneralValuesModel.Id;
                        //        objEmployeeModel.objAddressModel.Country = objGeneralValuesModel.Description;
                        //    }
                        //}
                    }
                    return Ok(objEmployeeModel);
                }
                return Json("Failure");
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                //var objTblEmployee = (from c in _dbcontext.TblEmployees where c.Id == id select c).FirstOrDefault();
                var objTblEmployee = _dbcontext.TblEmployees.FirstOrDefault(c => c.Id == id);

                if (objTblEmployee != null)
                {
                    EmployeeModel objEmployeeModel = new EmployeeModel();

                    objEmployeeModel = new EmployeeModel();
                    objEmployeeModel = objNIMBOLEMapper.MapTable2Model(objTblEmployee);

                    //TblAddressEmployee objTblAddressEmployee = (from adEmp in _dbcontext.TblAddressEmployees where adEmp.EmployeeId == id select adEmp).FirstOrDefault();
                    TblAddressEmployee objTblAddressEmployee = _dbcontext.TblAddressEmployees.FirstOrDefault(adEmp => adEmp.EmployeeId == id);
                    if (objTblAddressEmployee != null)
                    {
                        //var objTblAddress = (from adModel in _dbcontext.TblAddresses where adModel.Id == objTblAddressEmployee.AddressId select adModel).FirstOrDefault();
                        var objTblAddress = _dbcontext.TblAddresses.FirstOrDefault(adModel => adModel.Id == objTblAddressEmployee.AddressId);
                        if (objTblAddress != null)
                        {
                            objEmployeeModel.objAddressModel = new AddressModel();
                            objEmployeeModel.objAddressModel = objNIMBOLEMapper.MapTable2Model(objTblAddress);


                            var iCityId = objEmployeeModel.objAddressModel.CityId;
                            var iStateId = objEmployeeModel.objAddressModel.StateId;
                            var iCountryId = objEmployeeModel.objAddressModel.CountryId;

                            if (iCityId > 0)
                            {
                                CommanMethods objCommanMethods = new CommanMethods();
                                GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                                objGeneralValuesModel = objCommanMethods.GetCountryStateCity("City", objEmployeeModel.objAddressModel.CityId);
                                objEmployeeModel.objAddressModel.City = objGeneralValuesModel.Description;
                            }
                            if (iStateId > 0)
                            {
                                CommanMethods objCommanMethods = new CommanMethods();
                                GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                                objGeneralValuesModel = objCommanMethods.GetCountryStateCity("State", objEmployeeModel.objAddressModel.StateId);
                                objEmployeeModel.objAddressModel.State = objGeneralValuesModel.Description;
                                objEmployeeModel.objAddressModel.StateId = objGeneralValuesModel.Id;
                            }
                            if (iCountryId > 0)
                            {
                                CommanMethods objCommanMethods = new CommanMethods();
                                GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                                objGeneralValuesModel = objCommanMethods.GetCountryStateCity("Country", objEmployeeModel.objAddressModel.CountryId);
                                objEmployeeModel.objAddressModel.Country = objGeneralValuesModel.Description;
                                objEmployeeModel.objAddressModel.CountryId = objGeneralValuesModel.Id;
                                //objTransAccConModel.objAddressModel.Country = objGeneralValuesModel.Description;
                            }
                            //var iCityId = objEmployeeModel.objAddressModel.CityId;
                            //if (iCityId > 0)
                            //{
                            //    CommanMethods objCommanMethods = new CommanMethods();
                            //    GeneralValuesModel objGeneralValuesModel = new GeneralValuesModel();
                            //    objGeneralValuesModel = objCommanMethods.GetCountryStateCity("City", objEmployeeModel.objAddressModel.CityId);
                            //    objEmployeeModel.objAddressModel.City = objGeneralValuesModel.Description;
                            //    objGeneralValuesModel = new GeneralValuesModel();
                            //    objGeneralValuesModel = objCommanMethods.GetCountryStateCity("State", objEmployeeModel.objAddressModel.CityId);
                            //    objEmployeeModel.objAddressModel.State = objGeneralValuesModel.Description;
                            //    objGeneralValuesModel = objCommanMethods.GetCountryStateCity("Country", objGeneralValuesModel.Id);
                            //    objEmployeeModel.objAddressModel.Country = objGeneralValuesModel.Description;
                            //}


                        }
                    }
                    return Ok(objEmployeeModel);
                }
                throw new InvalidOperationException("Record not found.");
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("GetReferenceIds")]
        public IHttpActionResult GetReferenceIds(Guid Tid)
        {
            try
            {
                var data = (from emp in _dbcontext.TblEmployees where emp.Status == true && emp.TenantId == Tid orderby emp.FirstName ascending select new { Name = emp.FirstName, Id = emp.Id.ToString() }).Distinct().ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("GetAllLocation")]
        public IHttpActionResult GetAllLocation(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblLocations where x.Status == true && x.TenantId == Tid orderby x.Description select new { Id = x.Id, Name = x.Description }).ToList();
                return Ok(data);
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

        #region SELECT
        [HttpGet]
        [Route("SelectAllEmployees")]
        public IHttpActionResult SelectAllEmployees(Guid Tid)
        {
            try
            {
                var data = (from x in _dbcontext.TblEmployees where x.Status == true && x.TenantId == Tid orderby x.FirstName, x.LastName select new { Id = x.Id, FirstName = x.FirstName, LastName = x.LastName, FullName = x.FirstName + " " + x.LastName }).ToList();
                return Json(data);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("GetAllReportsToEmployees")]
        public IHttpActionResult GetAllReportsToEmployees(int EmpRoleId, Guid Tid)
        {
            try
            {
                var roleOrderId = (from e in _dbcontext.TblEmployeeRoles where e.Id == EmpRoleId && e.TenantId == Tid select e.RoleOrder).FirstOrDefault();

                var data = (from x in _dbcontext.TblEmployees where x.TenantId == Tid && x.Status == true && x.EmpRoleId != 2 && x.EmpRoleId == EmpRoleId orderby x.FirstName, x.LastName select new { Id = x.Id, FirstName = x.FirstName, LastName = x.LastName, FullName = x.FirstName + " " + x.LastName }).ToList();
                var queries = (data.Select(s => new KeyValueModel { Id = s.Id, Name = s.FullName }).OrderBy(s => s.Name)).ToList();
                return Ok(queries);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("SelectAllEmployeesForReport")]
        public IHttpActionResult SelectAllEmployeesForReport(long EmpId, Guid Tid)
        {
            try
            {
                string query = string.Empty;
                query = "exec sp_SubordinatesByRoleOrder " + EmpId + ", '" + Tid + "'";

                /*if (EmpId == 1)
                {
                    query = @"
                    SELECT Id, FullName Name, Role FROM (
                    SELECT 
                     E.Id
                     ,E.Code
                     ,E.FirstName
                     ,E.LastName
                     ,(COALESCE(E.FirstName,'') + ' ' + COALESCE(E.LastName,'')) FullName
                     ,E.EmpRoleId
                     ,E.ReportToId
                     ,(SELECT Code FROM TblEmployeeRole WHERE Id = E.EmpRoleId AND Code NOT IN ('Admin')) Role
                    FROM TblEmployee E 
                     JOIN TblEmployee RE ON RE.Id = E.ReportToId 
                    WHERE E.Status = 1 AND RE.Status = 1 
                    )X WHERE Role IS NOT NULL";
                }
                else
                {
                    query = @"WITH tblChild AS
                    (
                        SELECT
		                    E.Id
		                    ,E.Code
		                    ,E.FirstName
		                    ,E.LastName
		                    ,(COALESCE(E.FirstName,'') + ' ' + COALESCE(E.LastName,'')) FullName
		                    ,E.EmpRoleId
		                    ,E.ReportToId
		                    ,(SELECT Code FROM TblEmployeeRole WHERE Id = E.EmpRoleId AND Code NOT IN ('Admin')) Role
	                    FROM TblEmployee E 
	                    WHERE E.Status = 1 AND ReportToId = " + EmpId;
                    query += @" 
                        UNION ALL

                        SELECT 
		                    E.Id
		                    ,E.Code
		                    ,E.FirstName
		                    ,E.LastName
		                    ,(COALESCE(E.FirstName,'') + ' ' + COALESCE(E.LastName,'')) FullName
		                    ,E.EmpRoleId
		                    ,E.ReportToId
		                    ,(SELECT Code FROM TblEmployeeRole WHERE Id = E.EmpRoleId AND Code NOT IN ('Admin')) Role 
	                    FROM TblEmployee E  JOIN tblChild  ON E.ReportToId = tblChild.Id
                    )
                    SELECT Id, FullName Name, Role FROM tblChild WHERE Role IS NOT NULL
                    OPTION(MAXRECURSION 10)";
                }*/

                //                query = @"SELECT E.Id,E.TenantId,E.Code,E.FirstName,E.LastName,(COALESCE(E.FirstName,'') + ' ' + COALESCE (E.LastName,'')) Name ,E.EmpRoleId 	,E.ReportToId ,ER.Code EmployeeRole ,ER.RoleOrder FROM TblEmployee E JOIN TblEmployeeRole ER ON ER.Id = E.EmpRoleId AND ER.TenantId=E.TenantId _ 	WHERE EmpRoleId
                //	                IN(
                //		            SELECT Id FROM TblEmployeeRole WHERE RoleOrder >= 
                //		            (
                //			        SELECT RoleOrder FROM TblEmployeeRole WHERE  Id = 
                //			        (SELECT EmpRoleId FROM TblEmployee WHERE Id = + EmpId + AND Status = 1 ) AND Status = 1 AND TenantId=E.TenantId )
                //
                //	                )	
                //	                AND E.Status = 1 AND E.TenantId=''+TId+'' ORDER BY E.FirstName";

                var data = _dbcontext.Database.SqlQuery<EmpRoleModel>(query).ToList();
                // data.Where(x => x.TenantId == Tid);
                return Json(data);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("GetLoggedEmployee")]
        public IHttpActionResult GetLoggedEmployee(long id, string email, Guid Tid)
        {
            try
            {
                //var data = (from L in _dbcontext.TblLogins
                //            join E in _dbcontext.TblEmployees on L.Id equals E.LoginId
                //            join R in _dbcontext.TblEmployeeRoles on E.EmpRoleId equals R.Id
                //            where L.Status == true && L.Id == id && L.EmailAddress == email
                //            select new { Id = E.Id, Code = E.Code, FristName = E.FirstName, LastName = E.LastName, Email = E.EmployeeEmail, Location = E.Location, RoleCode = R.Code }).FirstOrDefault();
                ////return Json(data);

                var data = (from EMP in _dbcontext.VWLgnEmployees where EMP.Id == id && EMP.EmailAddress == email && EMP.TenantId == Tid select EMP).FirstOrDefault();
                if (data != null)
                {
                    EmployeeModel _objEmpModel = new EmployeeModel();
                    _objEmpModel.Id = data.Id;
                    _objEmpModel.EmpCode = data.Code;
                    _objEmpModel.FirstName = data.FirstName;
                    _objEmpModel.LastName = data.LastName;
                    _objEmpModel.EmployeeEmail = data.EmailAddress;
                    _objEmpModel.Location = data.Location;
                    _objEmpModel.RoleCode = data.RoleCode;
                    _objEmpModel.EmpRoleId = data.EmpRoleId;

                    TblEmployeeRole empRoles = _dbcontext.TblEmployeeRoles.SingleOrDefault(x => x.Id == data.EmpRoleId);// && x.TenantId==Tid);
                    if (empRoles != null)
                    {
                        EmployeeRoleModel _objEmpRoleModel = new EmployeeRoleModel();
                        _objEmpRoleModel.TenantId = Tid;
                        _objEmpRoleModel = objNIMBOLEMapper.MapTable2Model(empRoles);
                        _objEmpModel.objEmployeeRoleModel = _objEmpRoleModel;
                    }
                    else
                    {
                        throw new InvalidOperationException("Record not found.");
                    }

                    TblSetting query = _dbcontext.TblSettings.SingleOrDefault(x => x.TenantId == Tid);
                    if (query == null)
                        throw new InvalidOperationException("Record not found.");
                    else
                        _objEmpModel.TenantId = Tid;
                    _objEmpModel.objSettingModel = objNIMBOLEMapper.MapTable2Model(query);

                    return Json(_objEmpModel);
                }
                throw new InvalidOperationException("Record not found.");
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("SelectAllEmpCbo")]
        public IHttpActionResult SelectAllEmpCbo()
        {
            try
            {
                var data = (from x in _dbcontext.TblEmployees where x.Status == true select new { Id = x.Id, FirstName = x.FirstName }).ToList();
                return Ok(data);
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

        #region POST

        [HttpPost]
        [Route("InsertImage")]
        public IHttpActionResult InsertImage([FromBody] EmployeeModel objEmployeeModel)
        {
            try
            {
                int id = Convert.ToInt32(objEmployeeModel.Id);

                var query = (from e in _dbcontext.TblEmployees where e.Id == id && e.TenantId == objEmployeeModel.TenantId select e).FirstOrDefault();
                if (query != null)
                {
                    query.EmployeeImageURL = objEmployeeModel.EmployeeImageURL;
                    _dbcontext.SaveChanges();
                }
                return Ok(objEmployeeModel);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("Insert")]
        //   [ModelValidator]
        public IHttpActionResult Post([FromBody] EmployeeModel objEmployeeModel)
        {

            NimboleSuperadminDashboardEntities entities = new NimboleSuperadminDashboardEntities();
            DbContextTransaction dbTran = _dbcontext.Database.BeginTransaction();
            try
            {
                var customer = entities.Customers.Where(c => c.TenantID.Equals(objEmployeeModel.TenantId)).Select(c => new { c.TenantID, c.URL, c.FirstName, c.TrialPeriod, c.CreatedDate, c.Numberoflicenses }).FirstOrDefault();

                var curEmp = _dbcontext.TblEmployees.Where(e => e.TenantId.Equals(objEmployeeModel.TenantId)).Count();
                if (curEmp < customer.Numberoflicenses)
                {
                    HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
                    #region If ModelState is not Valid
                    if (response.StatusCode != HttpStatusCode.OK)
                        return Ok<ApiMessageError>(CommanMethods.CurrentObject.error);
                    #endregion
                    using (dbTran)
                    {
                        lock (thisLock)
                        {
                            #region Login
                            objEmployeeModel.objLoginModel.EmailAddress = objEmployeeModel.EmployeeEmail;
                            objEmployeeModel.objLoginModel.TenantId = objEmployeeModel.TenantId;
                            TblLogin objTblLogin = objNIMBOLEMapper.MapModel2Table(objEmployeeModel.objLoginModel);
                            if (!_dbcontext.TblLogins.Any(l => l.EmailAddress == objEmployeeModel.EmployeeEmail && l.TenantId == objEmployeeModel.TenantId))
                            {
                                _dbcontext.TblLogins.Add(objTblLogin);
                                objTblLogin.EmailAddress = objEmployeeModel.EmployeeEmail;
                                _dbcontext.SaveChanges();
                                objEmployeeModel.LoginId = objTblLogin.Id;
                                objEmployeeModel.Password = objTblLogin.Password;
                            }
                            else
                            {
                                dbTran.Rollback();
                                throw new InvalidOperationException("Employee email already exist.");
                            }
                            #endregion

                            #region Employee
                            TblEmployee objTblEmployee = objNIMBOLEMapper.MapModel2Table(objEmployeeModel);
                            if (!_dbcontext.TblEmployees.Any(u => u.FirstName == objEmployeeModel.FirstName && u.EmployeeEmail == objEmployeeModel.EmployeeEmail && u.EmpRoleId == objEmployeeModel.EmpRoleId && u.Location == objEmployeeModel.Location && u.TenantId == objEmployeeModel.TenantId))
                            {
                                _dbcontext.TblEmployees.Add(objTblEmployee);
                                _dbcontext.SaveChanges();
                                objEmployeeModel.Id = objTblEmployee.Id;
                            }
                            else
                            {
                                dbTran.Rollback();
                                throw new InvalidOperationException("Record already exist/inactive.");
                            }
                            #endregion

                            objEmployeeModel.objEmployeeRoleModel = new EmployeeRoleModel();
                            if (objEmployeeModel.objAddressModel != null && (objEmployeeModel.objAddressModel.CityId != 0 || objEmployeeModel.objAddressModel.CountryId != 0 || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.Mobile) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.Phone) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.HouseNo) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.Fax) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.HomePhone) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.StreetName) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.ZipCode) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.SkypeName)))
                            {
                                #region Address
                                objEmployeeModel.objAddressModel.Address_Type = "E";
                                objEmployeeModel.objAddressModel.TenantId = objEmployeeModel.TenantId;
                                TblAddress objTblAddress = objNIMBOLEMapper.MapModel2Table(objEmployeeModel.objAddressModel);
                                _dbcontext.TblAddresses.Add(objTblAddress);
                                _dbcontext.SaveChanges();
                                objEmployeeModel.objAddressModel.Id = objTblAddress.Id;

                                #endregion

                                #region AddressEmployee

                                TblAddressEmployee objTblAddressEmployee = new TblAddressEmployee();

                                //objTblAddressEmployee.TenantId = objEmployeeModel.TenantId.ToDefaultTenantId();
                                //objTblAddressEmployee.AddressId = objEmployeeModel.objAddressModel.Id;
                                //objTblAddressEmployee.EmployeeId = objEmployeeModel.Id;
                                objTblAddressEmployee.TenantId = objEmployeeModel.TenantId;
                                objTblAddressEmployee.AddressId = objTblAddress.Id;
                                objTblAddressEmployee.EmployeeId = objTblEmployee.Id;

                                _dbcontext.TblAddressEmployees.Add(objTblAddressEmployee);
                                _dbcontext.SaveChanges();
                                #endregion
                            }
                            //commit transaction
                            dbTran.Commit();
                            return Ok(objEmployeeModel);
                        }
                    }//Close Using Block
                }
                else
                {
                    throw new InvalidOperationException("Maximum number of users already exist.");
                    // return null;
                }
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                if (dbTran.UnderlyingTransaction.Connection != null)
                    dbTran.Rollback();
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                if (dbTran.UnderlyingTransaction.Connection != null)
                    dbTran.Rollback();
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
        }
        #endregion

        #region PUT
        [HttpPut]
        [Route("Edit")]
        public IHttpActionResult Edit([FromBody] EmployeeModel objEmployeeModel)
        {
            DbContextTransaction dbTran = _dbcontext.Database.BeginTransaction();
            try
            {
                using (dbTran)
                {
                    var queries = (from x in _dbcontext.TblEmployees where x.Id == objEmployeeModel.Id && x.TenantId == objEmployeeModel.TenantId select x).FirstOrDefault();
                    if (queries != null)
                    {
                        queries.EmployeeEmail = objEmployeeModel.EmployeeEmail;
                        queries.FirstName = objEmployeeModel.FirstName;
                        queries.LastName = objEmployeeModel.LastName;
                        queries.EmpRoleId = objEmployeeModel.EmpRoleId;
                        queries.Location = objEmployeeModel.Location;
                        queries.Comments = objEmployeeModel.Comments;
                        queries.ReportToId = objEmployeeModel.ReportingTo;
                        queries.BankDetails = objEmployeeModel.BankDetails;
                        queries.BornPlace = objEmployeeModel.BornPlace;
                        queries.JoinDate = objEmployeeModel.JoinDate;
                        queries.BankId = objEmployeeModel.BankId;
                        queries.BankNumber = objEmployeeModel.BankNumber;
                        queries.ResignDate = objEmployeeModel.ResignDate;
                        queries.BornDate = objEmployeeModel.BornDate;
                        queries.ModifiedDate = DateTime.Now;
                        queries.Status = objEmployeeModel.Status;
                        if (!string.IsNullOrEmpty(objEmployeeModel.EmployeeImageURL))
                            queries.EmployeeImageURL = objEmployeeModel.EmployeeImageURL;
                        _dbcontext.SaveChanges();
                        TblAddressEmployee objTblAddressEmployee = (from adEmp in _dbcontext.TblAddressEmployees where adEmp.EmployeeId == objEmployeeModel.Id && adEmp.TenantId == objEmployeeModel.TenantId select adEmp).FirstOrDefault();
                        if (objTblAddressEmployee != null)
                        {
                            var objTblAddress = (from adModel in _dbcontext.TblAddresses where adModel.Id == objTblAddressEmployee.AddressId && adModel.TenantId == objEmployeeModel.TenantId select adModel).FirstOrDefault();
                            if (objTblAddress != null && (objEmployeeModel.objAddressModel.CityId != 0 || objEmployeeModel.objAddressModel.CountryId != 0 || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.Mobile) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.Phone) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.HouseNo) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.Fax) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.HomePhone) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.StreetName) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.ZipCode) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.SkypeName)))
                            {
                                objEmployeeModel.objAddressModel.Id = objTblAddress.Id;
                                objEmployeeModel.objAddressModel.Address_Type = "E";
                                objEmployeeModel.objAddressModel.TenantId = objEmployeeModel.TenantId;
                                objTblAddress.StreetName = objEmployeeModel.objAddressModel.StreetName;
                                objTblAddress.HouseNo = objEmployeeModel.objAddressModel.HouseNo;
                                objTblAddress.CityId = objEmployeeModel.objAddressModel.CityId;
                                objTblAddress.StateId = objEmployeeModel.objAddressModel.StateId;
                                objTblAddress.CountryId = objEmployeeModel.objAddressModel.CountryId;
                                objTblAddress.ZipCode = objEmployeeModel.objAddressModel.ZipCode;
                                objTblAddress.Phone = objEmployeeModel.objAddressModel.Phone;
                                objTblAddress.Mobile = objEmployeeModel.objAddressModel.Mobile;
                                objTblAddress.HomePhone = objEmployeeModel.objAddressModel.HomePhone;
                                objTblAddress.Fax = objEmployeeModel.objAddressModel.Fax;
                                objTblAddress.SkypeName = objEmployeeModel.objAddressModel.SkypeName;
                                objTblAddress.Address_Type = objEmployeeModel.objAddressModel.Address_Type;
                                objTblAddress.ModifiedDate = DateTime.Now;
                                _dbcontext.SaveChanges();
                            }
                        }
                        else if (objEmployeeModel.objAddressModel != null && (objEmployeeModel.objAddressModel.CityId != 0 || objEmployeeModel.objAddressModel.CountryId != 0 || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.Mobile) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.Phone) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.HouseNo) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.Fax) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.HomePhone) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.StreetName) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.ZipCode) || !string.IsNullOrEmpty(objEmployeeModel.objAddressModel.SkypeName)))
                        {
                            #region Address
                            objEmployeeModel.objAddressModel.Address_Type = "E";
                            objEmployeeModel.objAddressModel.TenantId = objEmployeeModel.TenantId;
                            TblAddress objTblAddress = objNIMBOLEMapper.MapModel2Table(objEmployeeModel.objAddressModel);
                            _dbcontext.TblAddresses.Add(objTblAddress);
                            _dbcontext.SaveChanges();
                            objEmployeeModel.objAddressModel.Id = objTblAddress.Id;

                            #endregion

                            #region AddressEmployee

                            TblAddressEmployee objTblAddressEmployee1 = new TblAddressEmployee();
                            objEmployeeModel.objAddressModel.TenantId = objEmployeeModel.TenantId;
                            objTblAddressEmployee1.AddressId = objTblAddress.Id;
                            objTblAddressEmployee1.EmployeeId = objEmployeeModel.Id;

                            _dbcontext.TblAddressEmployees.Add(objTblAddressEmployee1);
                            _dbcontext.SaveChanges();
                            #endregion
                        }
                        dbTran.Commit();
                    }
                    else
                    {
                        dbTran.Rollback();
                        throw new InvalidOperationException("Record not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (dbTran.UnderlyingTransaction.Connection != null)
                    dbTran.Rollback();
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    ReasonPhrase = ex.Message
                });
            }
            return Ok(objEmployeeModel.Id.ToString());
        }

        [HttpPut]
        [Route("EditTarget")]
        public IHttpActionResult Edit([FromBody] EmployeeTargetModel objEmployeeTargetModel)
        {
            try
            {
                TblTransEmployeeTargetNew objTblTransEmployeetarget = new TblTransEmployeeTargetNew();
                var count = (from t in _dbcontext.TblTransEmployeeTargetNews where t.EmployeeRoleId == objEmployeeTargetModel.EmployeeRoleId && t.EmpId == objEmployeeTargetModel.EmpId && t.Status == true && t.TenantId == objEmployeeTargetModel.TenantId select t).Count();
                if (count > 0)
                {
                    objTblTransEmployeetarget = (from t in _dbcontext.TblTransEmployeeTargetNews where t.EmployeeRoleId == objEmployeeTargetModel.EmployeeRoleId && t.EmpId == objEmployeeTargetModel.EmpId && t.TenantId == objEmployeeTargetModel.TenantId select t).FirstOrDefault();
                    objTblTransEmployeetarget.Budget = objEmployeeTargetModel.Budget;
                    objTblTransEmployeetarget.IsAutomatic = objEmployeeTargetModel.IsAutomatic;
                    objTblTransEmployeetarget.QuarterlyTarget = objEmployeeTargetModel.QuarterlyTarget;
                    objTblTransEmployeetarget.MonthlyTarget = objEmployeeTargetModel.MonthlyTarget;
                    objTblTransEmployeetarget.WeeklyTarget = objEmployeeTargetModel.WeeklyTarget;
                    objTblTransEmployeetarget.Status = objEmployeeTargetModel.Status;
                    objTblTransEmployeetarget.TargetHike = objEmployeeTargetModel.TargetHike;
                    _dbcontext.SaveChanges();
                    return Ok(objEmployeeTargetModel);
                }
                else
                {
                    objTblTransEmployeetarget.TenantId = objEmployeeTargetModel.TenantId;
                    objTblTransEmployeetarget.EmpId = objEmployeeTargetModel.EmployeeId;
                    objTblTransEmployeetarget.EmployeeRoleId = objEmployeeTargetModel.EmployeeRoleId;
                    objTblTransEmployeetarget.FinancialYearId = objEmployeeTargetModel.FinancialYearId;
                    objTblTransEmployeetarget.IsAutomatic = objEmployeeTargetModel.IsAutomatic;
                    objTblTransEmployeetarget.TargetHike = objEmployeeTargetModel.TargetHike;
                    objTblTransEmployeetarget.Budget = objEmployeeTargetModel.Budget;
                    objTblTransEmployeetarget.QuarterlyTarget = objEmployeeTargetModel.QuarterlyTarget;
                    objTblTransEmployeetarget.MonthlyTarget = objEmployeeTargetModel.MonthlyTarget;
                    objTblTransEmployeetarget.WeeklyTarget = objEmployeeTargetModel.WeeklyTarget;
                    objTblTransEmployeetarget.CreatedDate = DateTime.Now;
                    objTblTransEmployeetarget.ModifiedDate = DateTime.Now;
                    objTblTransEmployeetarget.Status = true;
                    _dbcontext.TblTransEmployeeTargetNews.Add(objTblTransEmployeetarget);
                    _dbcontext.SaveChanges();
                    return Ok(objEmployeeTargetModel);
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

        #region DELETE
        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(string selectedId, bool status)
        {
            try
            {
                List<string> StrTempIds = new List<string>();
                string[] ids = { selectedId };
                if (!string.IsNullOrEmpty(selectedId))
                {
                    ids = selectedId.Split(',');
                }

                foreach (string tempId in ids)
                {
                    int id = Convert.ToInt32(tempId);

                    var query = _dbcontext.TblEmployees.Where(x => x.Id == id).FirstOrDefault();
                    if (query != null)
                    {
                        if (!status)
                        {
                            var LeadQuery = _dbcontext.TblLeads.Where(x => x.LeadOwnerId == id && x.Status == true).Count();
                            if (LeadQuery == 0)
                            {
                                query.Status = status;
                                _dbcontext.SaveChanges();
                            }
                            else
                            {
                                // throw new InvalidOperationException("Record is associated.");
                                StrTempIds.Add(tempId);
                            }
                        }
                        else
                        {
                            query.Status = status;
                            _dbcontext.SaveChanges();
                        }
                    }
                    else
                    {
                        //  throw new InvalidOperationException("Record not found.");
                    }
                }
                if (StrTempIds.Count > 0)
                {
                    throw new InvalidOperationException("Record is associated.");
                }
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

        [HttpDelete]
        [Route("DeleteRec")]
        public IHttpActionResult DeleteRec(string selectedId)
        {
            try
            {
                //employee
                string[] ids = { selectedId };
                List<string> StrTempIds = new List<string>();

                if (!string.IsNullOrEmpty(selectedId))
                {
                    ids = selectedId.Split(',');
                }

                foreach (string tempId in ids)
                {
                    int id = Convert.ToInt32(tempId);
                    var query = _dbcontext.TblEmployees.Where(x => x.Id == id).FirstOrDefault();
                    if (query != null)
                    {
                        var LeadQuery = _dbcontext.TblLeads.Where(x => x.LeadOwnerId == id && x.Status == true).Count();
                        if (LeadQuery == 0)
                        {
                            //employeelogin
                            var loginrec = (from l in _dbcontext.TblLogins where l.Id == query.LoginId select l).FirstOrDefault();
                            if (loginrec != null)
                            {
                                _dbcontext.TblLogins.Remove(loginrec);
                            }

                            //address & addressemployee

                            var addresstrn = (from a in _dbcontext.TblAddressEmployees where a.EmployeeId == id select a).FirstOrDefault();
                            if (addresstrn != null)
                            {
                                var addressrec = (from ad in _dbcontext.TblAddresses where ad.Id == addresstrn.AddressId select ad).FirstOrDefault();
                                if (addressrec != null)
                                {
                                    _dbcontext.TblAddresses.Remove(addressrec);
                                }
                                _dbcontext.TblAddressEmployees.Remove(addresstrn);
                            }

                            //employeetasks

                            //var emptask = (from t in _dbcontext.TblEmpTasks where t.TaskOwnerId == id select t).ToList();
                            //if (emptask != null)
                            //{
                            //    foreach(var item in emptask)
                            //    {
                            //        _dbcontext.TblEmpTasks.Remove(item);
                            //    }
                            //}

                            //leads

                            //var emplead = (from ld in _dbcontext.TblLeads where ld.LeadOwnerId == id select ld).ToList();
                            //if (emplead != null)
                            //{
                            //    foreach (var item in emplead)
                            //    {
                            //        _dbcontext.TblLeads.Remove(item);
                            //    }
                            //}

                            //tblactivitynotify and activity

                            //var empactivitynotify = (from an in _dbcontext.TblActivityNotifies where an.EmployeeId == id select an).ToList();
                            //if (empactivitynotify != null)
                            //{
                            //    foreach (var item in empactivitynotify)
                            //    {
                            //        var activity = (from ad in _dbcontext.TblActivities where ad.Id == item.ActivityId select ad).FirstOrDefault();
                            //        if (activity != null)
                            //        {
                            //            _dbcontext.TblActivities.Remove(activity);
                            //        }
                            //        _dbcontext.TblActivityNotifies.Remove(item);
                            //    }
                            //}

                            _dbcontext.TblEmployees.Remove(query);

                            _dbcontext.SaveChanges();
                            return Ok(query);
                        }
                        else
                        {
                            //  throw new InvalidOperationException("Record is associated.");
                            StrTempIds.Add(tempId);
                        }
                    }
                    else
                    {
                        //  throw new InvalidOperationException("Record not found.");
                    }

                }
                if (StrTempIds.Count > 0)
                {
                    throw new InvalidOperationException("Record is associated.");
                }
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

        #region EmployeeTarget
        [HttpGet]
        [Route("GetAllEmployeeTargets")]
        public IHttpActionResult GetAllEmployeeTargets(Guid Tid, string EmpId, string RoleId)
        {
            try
            {
                Int64 EmployeeId = Convert.ToInt64(EmpId);
                Int64 EmproleId = Convert.ToInt64(RoleId);
                var currentyear = DateTime.Now.Date.Year.ToString() + '-' + DateTime.Now.AddYears(1).ToString();
                var FinancialyearId = (from f in _dbcontext.TblFinancialYears where f.FinancialYear == "2016-2017" && f.TenantId == Tid select f.Id).FirstOrDefault();

                var EmpdataChecking = (from e in _dbcontext.TblTransEmployeeTargetNews where e.EmployeeRoleId == EmproleId && e.EmpId == EmployeeId && e.Status == true && e.TenantId == Tid select e).Count();
                if (EmpdataChecking > 0)
                {
                    //var query = "SELECT ET.*, FY.FinancialYear,ER.Code EmployeeRole,ER.Description FROM TblTransEmployeetarget ET JOIN TblFinancialYear FY ON FY.Id = '" + FinancialyearId + "' JOIN TblEmployeeRole ER ON ER.Id = '" + EmproleId + "' WHERE   ET.Status = 1 AND FY.Status = 1 AND ER.Status = 1 and ET.FinancialYearId = '" + FinancialyearId + "' and ET.EmployeeRoleId = '" + EmproleId + "' and ET.EmpId = '" + EmployeeId + "'";

                    var query = "SELECT ET.*, FY.FinancialYear,ER.Code EmployeeRole,ER.Description FROM TblTransEmployeeTargetNew ET JOIN TblFinancialYear FY ON FY.Id = '" + FinancialyearId + "' JOIN TblEmployeeRole ER ON ER.Id = '" + EmproleId + "' WHERE   ET.Status = 1 AND FY.Status = 1 AND ER.Status = 1 and ET.FinancialYearId = '" + FinancialyearId + "' and ET.EmployeeRoleId = '" + EmproleId + "' and ET.EmpId = '" + EmployeeId + "' and ET.TenantId= '" + Tid + "'";
                    var data = _dbcontext.Database.SqlQuery<EmployeeTargetModel>(query).FirstOrDefault();
                    return Json(data);
                }
                else
                {
                    var query = "SELECT ET.*, FY.FinancialYear,ER.Code EmployeeRole,ER.Description FROM TblEmployeeTarget ET JOIN TblFinancialYear FY ON FY.Id = '" + FinancialyearId + "' JOIN TblEmployeeRole ER ON ER.Id = '" + RoleId + "' WHERE   ET.Status = 1 AND FY.Status = 1 AND ER.Status = 1 and ET.FinancialYearId = '" + FinancialyearId + "' and ET.EmployeeRoleId = '" + RoleId + "' and ET.TenantId= '" + Tid + "' ";
                    var data = _dbcontext.Database.SqlQuery<EmployeeTargetModel>(query).FirstOrDefault();
                    return Json(data);
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
    }
}
