using NIMBOLE.Entities;
using NIMBOLE.Models.Models;
using NIMBOLE.Models.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace NIMBOLE.Service.Controllers
{
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : ApiController
    {
        private NimboleStagingEntities _dbcontext;
        DTO objNIMBOLEMapper ;
        public DashboardController()
        {
            _dbcontext = new NimboleStagingEntities();
            objNIMBOLEMapper = new DTO();
        }

        #region GET's
        [HttpGet]
        [Route("GetAllCountsByEmpId")]
        public IHttpActionResult GetAllCountsByEmpId(long iEmpId,Guid Tid)
        {
            try
            {
                var strQuery = string.Empty;
                //sreedhar changed on 13th Jan 2016 from sql to Entity (EDMX)

                strQuery = "select count (id) from tbllead where status = 1 and TenantId='"+ Tid +"' and convert(varchar(10),CreatedDate, 112) = convert(varchar(10), getdate(), 112)";
                var _lstLeadIds = _dbcontext.Database.SqlQuery<int>(strQuery).ToList<int>();

                //var lstLeadIds = (from l in dbcontext.TblLeads where l.Status == true && l.CreatedDate.Value.Year == DateTime.Now.Year && l.CreatedDate.Value.Month == DateTime.Now.Month && l.CreatedDate.Value.Day == DateTime.Now.Day group l by l.Id into gp select gp.Count()).ToList();

                //query to get Opportunities count
                //strQuery = "select count(a.id) from tblactivity a join tbltranslead tl on tl.activityid = a.id where a.status = 1 and milestoneid = (select id from tblmilestone where code = 'opcode' and status = 1) and convert(varchar(10),a.CreatedDate, 112) = convert(varchar(10), getdate(), 112) and tl.leadId in (select id from tbllead)";
                //strQuery = "select count(l.id) from tblLead l left outer join tblMilestone m on m.Id = L.LeadStatus where l.status = 1 and m.code = 'opcode' and m.status = 1 and convert(varchar(10),l.CreatedDate, 112) = convert(varchar(10), getdate(), 112)";

                strQuery = "select count(l.id) from tblLead l left outer join tblMilestone m on m.Id = L.LeadStatus where l.status = 1 and l.leadstage = 1  and m.status = 1 and l.TenantId='" + Tid + "'  and m.TenantId='" + Tid + "'  and convert(varchar(10),l.CreatedDate, 112) = convert(varchar(10), getdate(), 112)";

                var _lstOportunity = _dbcontext.Database.SqlQuery<int>(strQuery).ToList<int>();


                //var lid=(from l in _dbcontext.TblLeads where l.Status==true select l.Id).ToString();
                //var msid=(from m in _dbcontext.TblMileStones where m.Code=="opcode" && m.Status==true select m.Id ).FirstOrDefault();
                //var lstOportunity = (from a in dbcontext.TblActivities join tl in _dbcontext.TblTransLeads on a.Id equals tl.Id where a.Status == true && a.MileStoneId == msid  && a.CreatedDate.Value.Year == DateTime.Now.Year && a.CreatedDate.Value.Month == DateTime.Now.Month && a.CreatedDate.Value.Day == DateTime.Now.Day && tl.LeadId.ToString().Contains(lid) group a by a.Id into gp select gp.Count()).ToList<int>();

                //query to get Deal count
                //strQuery = "select count(a.id) from tblactivity a join tbltranslead tl on tl.activityid = a.id where a.status = 1 and milestoneid = (select id from tblmilestone where code = 'deal' and status = 1) and convert(varchar(10),a.CreatedDate, 112) = convert(varchar(10), getdate(), 112) and tl.leadId in (select id from tbllead)";
                //strQuery = "select count(l.id) from tblLead l left outer join tblMilestone m on m.Id = L.LeadStatus where l.status = 1 and m.code = 'Deal' and m.status = 1 and convert(varchar(10),l.CreatedDate, 112) = convert(varchar(10), getdate(), 112)";

                strQuery = "select count(l.id) from tblLead l left outer join tblMilestone m on m.Id = L.LeadStatus where l.status = 1 and l.leadstage = 2 and m.status = 1  and l.TenantId='" + Tid + "'  and m.TenantId='" + Tid + "'  and convert(varchar(10),l.CreatedDate, 112) = convert(varchar(10), getdate(), 112)";
                var _lstEmpTasks = _dbcontext.Database.SqlQuery<int>(strQuery).ToList<int>();

                //var deal = (from m in _dbcontext.TblMileStones where m.Code == "deal" && m.Status == true select m.Id).FirstOrDefault();
                //var lstEmpTasks = (from a in dbcontext.TblActivities join tl in _dbcontext.TblTransLeads on a.Id equals tl.Id where a.Status == true && a.MileStoneId == deal && a.CreatedDate.Value.Year == DateTime.Now.Year && a.CreatedDate.Value.Month == DateTime.Now.Month && a.CreatedDate.Value.Day == DateTime.Now.Day && tl.LeadId.ToString().Contains(lid) group a by a.Id into gp select gp.Count()).ToList<int>();


                List<string> strCounts = new List<string>();
                //strCounts.Add(_lstLeadIds.Count.ToString());
                //strCounts.Add(_lstOportunity.Count.ToString());
                //strCounts.Add(_lstEmpTasks.Count.ToString());                
                strCounts.Add(_lstLeadIds[0].ToString());
                strCounts.Add(_lstOportunity[0].ToString());
                strCounts.Add(_lstEmpTasks[0].ToString());
                return Json(strCounts);
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
        [Route("GetLeadCountsForWeek")]
        public IHttpActionResult GetLeadCountsForWeek()
        {
            try
            {
                //var strQuery = "select RIGHT('0' + CAST(DAY(CreatedDate) AS varchar(2)), 2)[Date], coalesce(count(Id),0)[count] from tbllead WHERE CreatedDate >= dateadd(day,datediff(day,0,GetDate())- 8,0) GROUP BY RIGHT('0' + CAST(DAY(CreatedDate) AS varchar(2)), 2)";
                var strQuery = ";with aweek(day) as (select getdate() as day union all select day - 1 from aweek where day > dateadd(day,datediff(day,0,GetDate())- 5,0))select RIGHT('0' + CAST(DAY(aweek.day) AS varchar(2)), 2)[Date], count(Id)[Count] from aweek left join tbllead on convert(varchar(10), tbllead.CreatedDate, 112) = convert(varchar(10), aweek.day, 112) and status = 1 group by RIGHT('0' + CAST(DAY(aweek.day) AS varchar(2)), 2),RIGHT('0' + CAST(MONTH(day) AS varchar(2)), 2)";
                var query = _dbcontext.Database.SqlQuery<DashboardModel>(strQuery).ToList<DashboardModel>();

                if (query != null)
                    return Json(query);
                else
                    throw new InvalidOperationException("Record not Found.");
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
        [Route("GetOppDealCountsForWeek")]
        public IHttpActionResult GetOppDealCountsForWeek(string Type)
        {
            try
            {
                /*var strQuery = ";with aweek(day) as (select getdate() as day union all select day - 1 from aweek where day > dateadd(day,datediff(day,0,GetDate())- 5,0))";
                strQuery += "select RIGHT('0' + CAST(DAY(a.day) AS varchar(2)), 2)[Date], count(Id)[Count] from aweek a left join (";
                strQuery += "select id, (select createddate from tblactivity where milestoneId = (select id from tblmilestone where code = '" + Type + "'))CreatedDate, status from tbllead L where id in(select distinct leadid from tbltranslead where activityid in(select id from tblactivity where milestoneId = (select id from tblmilestone where code = '" + Type + "')))) as b	on convert(varchar(10), b.CreatedDate, 112) = convert(varchar(10), a.day, 112) and b.status = 1 group by RIGHT('0' + CAST(DAY(a.day) AS varchar(2)), 2)";*/
                var strQuery = ";with aweek(day) as (select getdate() as day union all select day - 1 from aweek where day > dateadd(day,datediff(day,0,GetDate())- 5,0))";
                strQuery += "select RIGHT('0' + CAST(DAY(a.day) AS varchar(2)), 2)[Date], count(Id)[Count] from aweek a left join (";
                strQuery += "select id, CreatedDate, status from tbllead L where id in(select distinct leadid from tbltranslead where activityid in(select id from tblactivity where milestoneId = (select id from tblmilestone where code = '" + Type + "' and status = 1)))) as b	on convert(varchar(10), b.CreatedDate, 112) = convert(varchar(10), a.day, 112) and b.status = 1 group by RIGHT('0' + CAST(DAY(a.day) AS varchar(2)), 2),RIGHT('0' + CAST(MONTH(day) AS varchar(2)), 2)";
                var query = _dbcontext.Database.SqlQuery<DashboardModel>(strQuery).ToList<DashboardModel>();

                if (query != null)
                    return Json(query);
                else
                    throw new InvalidOperationException("Record not Found.");
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
