using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using NIMBOLE.Models.Models.Core;

namespace NIMBOLE.Service.Controllers
{
    public class RelayController : ApiController
    {
        public override Task<HttpResponseMessage> ExecuteAsync(
            HttpControllerContext controllerContext,
            CancellationToken cancellationToken)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["Redirect"] ?? "false"))
            {
                var url = controllerContext.Request.RequestUri;
                url = new Uri(url.AbsoluteUri.Replace(
                  ConfigurationManager.AppSettings["OriginalUriFragment"],
                  ConfigurationManager.AppSettings["ReplacemenUriFragment"]));

                var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                foreach (var httpRequestHeader in controllerContext.Request.Headers)
                {
                    client.DefaultRequestHeaders.Add(httpRequestHeader.Key, httpRequestHeader.Value);
                }
                if (controllerContext.Request.Method == HttpMethod.Get)
                {
                    return client.GetAsync(url, cancellationToken);
                }
                if (controllerContext.Request.Method == HttpMethod.Post)
                {
                    return client.PostAsync(url, controllerContext.Request.Content, cancellationToken);
                }
                if (controllerContext.Request.Method == HttpMethod.Delete)
                {
                    return client.DeleteAsync(url, cancellationToken);
                }
                if (controllerContext.Request.Method == HttpMethod.Put)
                {
                    return client.PutAsync(url, controllerContext.Request.Content, cancellationToken);
                }
                throw new NotSupportedException("Unknown method passed to Relay Controller");
            }

            return base.ExecuteAsync(controllerContext, cancellationToken);
        }

        protected ApiResult<T> Execute<T>(Func<T> executeFunc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = executeFunc();
                    return new ApiResult<T>(result);
                    
                }
                else
                {
                    return new ApiResult<T>(default(T), false, GetModelStateErrors());
                }
            }
            catch (Exception exception)
            {
                //TODO: log errors
                return new ApiResult<T>(default(T), false, exception.Message);
            }
        }

        protected virtual string GetModelStateErrors()
        {

            if (ModelState.IsValid)
            {
                return string.Empty;
            }
            var errors = new StringBuilder();
            foreach (var modelKey in ModelState.Keys)
            {
                foreach (var error in ModelState[modelKey].Errors)
                {
                    errors.Append(error.ErrorMessage);
                    errors.Append(Environment.NewLine);
                }
            }
            return errors.ToString();
        }

    }
}
