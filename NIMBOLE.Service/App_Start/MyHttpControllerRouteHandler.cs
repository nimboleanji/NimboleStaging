using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;

namespace NIMBOLE.Service.App_Start
{
    public class MyHttpControllerHandler
    : HttpControllerHandler, IRequiresSessionState
    {
        public MyHttpControllerHandler(RouteData routeData)
            : base(routeData)
        { }
    }
    public class MyHttpControllerRouteHandler : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(
            RequestContext requestContext)
        {
            return new MyHttpControllerHandler(requestContext.RouteData);
        }
    }
}
