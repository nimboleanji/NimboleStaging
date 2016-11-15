using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NIMBOLE.Entities;

namespace NIMBOLE.Service.Controllers
{

    [RoutePrefix("api/GetUrl")]
    public class NimboleDashBoardTenantIdController : ApiController
    {
        [HttpGet]       
        [Route("GetUrlTenantId")]
        public HttpResponseMessage GetDashboardTenantid(string SubDomain)
        
        {
            HttpResponseMessage response = null;

            NimboleSuperadminDashboardEntities entities = new NimboleSuperadminDashboardEntities();

            var customer = entities.Customers.Where(c => c.URL.Equals(SubDomain)).Select(c => new { c.TenantID, c.URL, c.FirstName,c.TrialPeriod,c.CreatedDate,c.CID }).FirstOrDefault();

            if (customer != null && customer.TenantID != null)
            {


                if (customer.URL == SubDomain)
                {

                    if (customer.TrialPeriod > 0)
                    {
                      
                        var strQuery = string.Empty;

                        strQuery = "select datediff(day,createddate,getdate()) from customer where cid= " + customer.CID + " ";
                         var daysDiff = entities.Database.SqlQuery<int>(strQuery).FirstOrDefault<int>();

                         if ( daysDiff <= customer.TrialPeriod)
                         {
                             response = this.Request.CreateResponse(HttpStatusCode.OK);
                             response.Content = new StringContent("TenantID:" + customer.TenantID.ToString(), System.Text.Encoding.UTF8, "application/JSON");
                         }
                         else
                         {
                             //response = this.Request.CreateResponse(HttpStatusCode.OK);
                             //response.Content = new StringContent("Demo Expired", System.Text.Encoding.UTF8, "application/JSON");

                             //response = this.Request.CreateResponse(HttpStatusCode.OK);
                             //response.Content = new StringContent("Unauthorized access", System.Text.Encoding.UTF8, "application/JSON");

                             response = this.Request.CreateResponse(HttpStatusCode.Unauthorized);
                             response.Content = new StringContent("Unauthorized access", System.Text.Encoding.UTF8, "application/JSON");
                         
                         }
                       
                    }
                    else
                    {
                        response = this.Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = new StringContent("TenantID:" + customer.TenantID.ToString(), System.Text.Encoding.UTF8, "application/JSON");
                    }
                }
                else
                {
                    response = this.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    response.Content = new StringContent("Unauthorized access", System.Text.Encoding.UTF8, "application/JSON");
                }

            }
            else
            {
                response = this.Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent("Domain Not Found", System.Text.Encoding.UTF8, "application/JSON");
            }

            return response;
        }



        [HttpGet]
        [Route("GetMobAppCodeBasedTenantId")]
        public IHttpActionResult GetMobAppCodeBasedTenantId(int mobAppcode)
        {
           
            NimboleSuperadminDashboardEntities entities = new NimboleSuperadminDashboardEntities();

            var customer = entities.Customers.Where(c => c.CID.Equals(mobAppcode)).Select(c => new {c.TenantID,c.CID,Name=c.FirstName + " " + c.LastName, c.UserName, c.URL , c.Password  }).FirstOrDefault();

            if (customer != null && customer.TenantID != null)
            {
                if (customer.CID == mobAppcode)
                {
                    return Ok(customer);
                }
                else
                { 
                    return NotFound();
                }

            }
            else
            {
              
                return NotFound();
            }

           
        }

    }
}