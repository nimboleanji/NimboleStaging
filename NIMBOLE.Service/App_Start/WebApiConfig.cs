using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;
using System.Net.Http.Formatting;
using System.Web.Http.ExceptionHandling;
using NIMBOLE.Service.ExceptionHandling;


namespace NIMBOLE.Service
{
    public static class WebApiConfig
    {      
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);



            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            //globaly Access From Different Server(Web API)
            //var cors = new EnableCorsAttribute("http://localhost:13869/", "*", "*");
            //config.EnableCors(cors);
            //config.EnableCors();          using Newtonsoft.Json.Serialization;        System.Net.Http.Formatting

            // There can be multiple exception loggers. (By default, no exception loggers are registered.)
            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
            // There must be exactly one exception handler. (There is a default one that may be replaced.)
            // To make this sample easier to run in a browser, replace the default exception handler with one that sends
            // back text/plain content for all errors.
            config.Services.Replace(typeof(IExceptionHandler), new GenericTextExceptionHandler());

            config.Formatters.Add(new JsonMediaTypeFormatter());
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            jsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
            jsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

                // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
