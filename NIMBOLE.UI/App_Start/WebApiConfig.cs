using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;

namespace NIMBOLE.UI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.MapHttpAttributeRoutes();
            //config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
            config.Formatters.Add(new XmlMediaTypeFormatter());
            config.Formatters.Add(new FormUrlEncodedMediaTypeFormatter());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                
                defaults: new { id = RouteParameter.Optional }
            );
            //config.Routes.MapHttpRoute(
            //    name: "DefaultMVC",
            //    routeTemplate: "api/{controller}/{action}/{id}",                
            //    defaults: new { id = RouteParameter.Optional }
            //);
            
        }
    }
}
