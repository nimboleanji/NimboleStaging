using AutoMapper;
using Newtonsoft.Json;
using NIMBOLE.Entities;
using NIMBOLE.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Competitor")]
    public class CompetitorController : ApiController
    {
        private NIMBOLEContext dbcontext = new NIMBOLEContext();
        #region GET
        [HttpGet]
        [Route("GetAll")]
        // GetAll api/Competitor/GetAll
        public IHttpActionResult GetAll()
        {
            try
            {
                var data = (from x in dbcontext.TblCompetitors select x).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest("NIMBOLE.Error: " + ex.Message);
            }
        }
        //  api/Competitor/GetById/5
        [HttpGet]
        [Route("GetById")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                var query = dbcontext.TblCompetitors.SingleOrDefault(x => x.Id == id);
                if (query == null)
                    return BadRequest("Record Not Found");
                else
                    return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet]
        //[Route("GetCompetitorPrice")]
        //public IHttpActionResult GetCompetitorPrice(long ProdId, long CompId, long LeadId)
        //{
        //    try
        //    {
        //        var price = dbcontext.TblCompetitors.Where(c => c.ProductId == ProdId && c.Id == CompId && c.Status == true).Select(q => q.Price).SingleOrDefault();
        //        if (price == null)
        //            return BadRequest("Record Not Found");
        //        else
        //            return Ok(Convert.ToDouble(price));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet]
        [Route("GetCompetitorPrice")]
        public IHttpActionResult GetCompetitorPrice(long ProdId, long LeadId)
        {
            try
            {
                //var query = (from tlc in dbcontext.TblTransLeadCompetitors
                //             join comp in dbcontext.TblProducts on tlc.ProductId equals comp.Id
                //             where tlc.LeadId == LeadId && comp.Id == ProdId
                //             select comp.Price).FirstOrDefault();
                var query = (from p in dbcontext.TblProducts where p.Id == ProdId select p.Price).FirstOrDefault();
                decimal price = query;
                if (price == null)
                    return BadRequest("Record Not Found");
                else
                    return Ok(Convert.ToDouble(price));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region POST
        public HttpResponseMessage Post(CompetitorModel objCompetitorModel)
        {
            HttpResponseMessage response = CommanMethods.CurrentObject.IsModelStateValid(ModelState, Request);
            #region If ModelState is not Valid
                if (response.StatusCode != HttpStatusCode.OK)
                    return Request.CreateResponse<ApiMessageError>(HttpStatusCode.Conflict, CommanMethods.CurrentObject.error);
            #endregion

            #region SaveChanges and Return HttpStatusCode
                response = Request.CreateResponse(HttpStatusCode.OK, string.Empty);
                try
                {
                    TblCompetitor objTblCompetitor = Mapper.Map<TblCompetitor>(objCompetitorModel);
                    dbcontext.TblCompetitors.Add(objTblCompetitor);
                    int iResult = dbcontext.SaveChanges();
                    if (iResult > 0)
                        response.Content = new StringContent(JsonConvert.SerializeObject(new { id = objTblCompetitor.Id.ToString() }), Encoding.UTF8, "application/json");
                    response.StatusCode = HttpStatusCode.OK;
                }
                catch (Exception ex)
                {
                    response.Content = new StringContent(JsonConvert.SerializeObject("Exception: " + ex.Message), Encoding.UTF8, "application/json");
                    response.StatusCode = HttpStatusCode.ExpectationFailed;
                }
            #endregion
            
            return response;
        }
        #endregion
        #region PUT
        // PUT api/Competitor/5
        public void Put(int id, [FromBody]CompetitorModel objCompetitorModel)
        {

        }
        #endregion
        #region DELETE
        // DELETE api/Competitor/5
        public void Delete(int id)
        {
        }
        #endregion
    }
}
