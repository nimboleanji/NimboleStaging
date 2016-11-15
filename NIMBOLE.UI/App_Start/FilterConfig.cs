using System.Web;
using System.Web.Mvc;

namespace NIMBOLE.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleAndLogErrorAttribute());
        }
        public class HandleAndLogErrorAttribute : HandleErrorAttribute
        {
            public override void OnException(ExceptionContext filterContext)
            {
                //string strException = filterContext.Exception.Message;
                //if (strException.Equals("This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet."))
                //{
                //    if (filterContext.HttpContext.IsCustomErrorEnabled)
                //    {
                //        filterContext.ExceptionHandled = true;
                //    }
                //    base.OnException(filterContext);
                //    //OVERRIDE THE 500 ERROR  
                //    filterContext.HttpContext.Response.StatusCode = 200;
                //}
                //// Log the filterContext.Exception details
                ////base.OnException(filterContext);
                if (filterContext.Exception != null)
                { 
                    
                }
            }
        }
    }
}
