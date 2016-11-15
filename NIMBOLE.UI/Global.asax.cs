using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using NIMBOLE.Models;
using NIMBOLE.Models.Models;
using NIMBOLE.UI.Controllers;
using AutoMapper;
using System.Data.SqlClient;
using System.Web.Helpers;

namespace NIMBOLE.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            SqlConnection.ClearAllPools();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null &&
                HttpContext.Current.User.Identity.IsAuthenticated &&
                HttpContext.Current.Session["User"] == null)
            {
                HttpContext.Current.Session["User"] = HttpContext.Current.User.Identity.Name.ToString();
            }
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var currentController = " ";
            var currentAction = " ";
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
            var controller = new ErrorController();
            var routeData = new RouteData();
            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }
                if (currentRouteData.Values["action"] != null && currentRouteData.Values["action"] != "Login" && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
                else
                {
                    currentAction = "Index";
                }
            }
            var ex = Server.GetLastError();
            var action = "Index";
            if (ex.InnerException == null)
            {
                httpContext.ClearError();
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
                httpContext.Response.TrySkipIisCustomErrors = true;
                //HttpContext.Current.Request.RawUrl.Replace("~/Home/Index");
                Response.Redirect("/Home/Index");
            }
            else
            {
                if (ex is HttpException)
                {
                    var httpEx = ex as HttpException;

                    switch (httpEx.GetHttpCode())
                    {
                        case 404:
                            action = "NotFound";
                            break;
                        // others if any
                    }
                }
                httpContext.ClearError();
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
                httpContext.Response.TrySkipIisCustomErrors = true;
                routeData.Values["controller"] = "Error";
                routeData.Values["action"] = action;
                controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
                ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
                //HttpContext httpContext = HttpContext.Current;
                //if (httpContext != null)
                //{
                //    RequestContext requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;
                //    /* when the request is ajax the system can automatically handle a mistake with a JSON response. then overwrites the default response */
                //    if (requestContext.HttpContext.Request.IsAjaxRequest())
                //    {
                //        httpContext.Response.Clear();
                //        string controllerName = requestContext.RouteData.GetRequiredString("controller");
                //        IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
                //        IController controller = factory.CreateController(requestContext, controllerName);
                //        ControllerContext controllerContext = new ControllerContext(requestContext, (ControllerBase)controller);

                //        JsonResult jsonResult = new JsonResult();
                //        jsonResult.Data = new { success = false, serverError = "500" };
                //        jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                //        jsonResult.ExecuteResult(controllerContext);
                //        httpContext.Response.End();
                //    }
                //    else
                //    {
                //        Response.Clear();
                //        //Response.Redirect("~/Error/Index");
                //        Response.End();
                //    }
                //}
            }
        }
        public string newChar { get; set; }
    }
}
