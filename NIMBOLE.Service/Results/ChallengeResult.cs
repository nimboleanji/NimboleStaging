using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Text;

namespace NIMBOLE.Service.Results
{
    public class ChallengeResult : IHttpActionResult
    {
        #region Properties
        public string LoginProvider { get; set; }

        public string Content { get; private set; }

        public Encoding Encoding { get; private set; }

        public HttpRequestMessage Request { get; private set; }
        #endregion Properties

        public ChallengeResult(string loginProvider, ApiController controller)
        {
            LoginProvider = loginProvider;
            Request = controller.Request;
        }

        public ChallengeResult(string content, Encoding encoding, HttpRequestMessage request)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            Content = content;
            Encoding = encoding;
            Request = request;
        }


        //public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        //{
        //    Request.GetOwinContext().Authentication.Challenge(LoginProvider);

        //    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        //    response.RequestMessage = Request;
        //    return Task.FromResult(response);
        //}

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            response.RequestMessage = Request;
            response.Content = new StringContent(Content, Encoding);
            return response;
        }
    }
}
