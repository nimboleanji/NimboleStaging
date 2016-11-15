
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using NIMBOLE.Models.Models.ReportModel;
using AutoMapper;
using NIMBOLE.Models.Models;

namespace NIMBOLE.Service.Controllers
{
    public class ReportController : RelayController
    {
        private readonly IReportsRepository _reportsRepository;

        public ReportController(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        [HttpGet]
        public ApiResult<IEnumerable<ReportModel>> Reports()
        {
            return Execute(() => _reportsRepository.GetReports().Select(Mapper.Map<ReportModel>));
        }

        [HttpGet]
        public ApiResult<ReportRequestModel> Report(int reportId)
        {
            return Execute(() =>
            {
                var request = Mapper.Map<ReportRequestModel>(_reportsRepository.GetReport(reportId));
                request.ReportRequestParameters.ForEach(one =>
                {
                    if (one.ParameterViewName == "ActiveFlagDropDown")
                    {
                        one.ParameterValue = "1";
                    }
                });
                return request;
            });
        }


        [HttpPost]
        public ApiResult<string> CreateRequest(ReportRequestModel reportRequest)
        {
            return Execute(() =>
            {
                var result = Guid.NewGuid();
                reportRequest.UniqueId = result;
                _reportsRepository.CreateRequest(Mapper.Map<ReportRequestModel>(reportRequest));
                return result.ToString();
            });
        }
    }
}
