using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;

namespace NIMBOLE.Service.App_Start
{
    public class JsonContentNegotiator : IContentNegotiator
    {
        private readonly JsonMediaTypeFormatter _jsonFormatter;
        private System.Net.Http.Formatting.JsonMediaTypeFormatter jsonFormatter;
        public JsonContentNegotiator(System.Net.Http.Formatting.JsonMediaTypeFormatter jsonFormatter)
        {
            // TODO: Complete member initialization
            this.jsonFormatter = jsonFormatter;
        }

        public ContentNegotiationResult Negotiate(
                Type type,
                HttpRequestMessage request,
                IEnumerable<MediaTypeFormatter> formatters)
        {
            return new ContentNegotiationResult(
                _jsonFormatter,
                new MediaTypeHeaderValue("application/json"));
        }
    }

}
