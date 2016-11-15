using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Routing;
using System.Security.Cryptography;
using System.IO;
using NIMBOLE.UI.Controllers;
using System.Web.Mvc.Html;

namespace NIMBOLE.UI.Helpers
{
    public static class QueryStringEncryption
    {
        public static MvcHtmlString EncodedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            string queryString = string.Empty;
            string htmlAttributesString = string.Empty;
            string AreaName = string.Empty;
            bool IsRoute = false;

            if (routeValues != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(routeValues);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    if (!d.Keys.Contains("IsRoute"))
                    {
                        if (i > 0)
                        {
                            queryString += "?";
                        }
                        queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                    }
                    else
                    {
                        if (!d.Keys.ElementAt(i).Contains("IsRoute"))
                        {
                            queryString += d.Values.ElementAt(i);
                            IsRoute = true;
                        }
                    }
                }
            }

            if (htmlAttributes != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    htmlAttributesString += " " + d.Keys.ElementAt(i).Trim('@') + "=" + d.Values.ElementAt(i);
                }
            }

            //<a href="/Answer?questionId=14">What is Entity Framework??</a>
            StringBuilder ancor = new StringBuilder();
            ancor.Append("<a ");
            if (htmlAttributesString != string.Empty)
            {
                ancor.Append(htmlAttributesString);
            }
            ancor.Append(" href='");
            if (AreaName != string.Empty)
            {
                ancor.Append("/" + AreaName);
            }
            if (controllerName != string.Empty)
            {
                ancor.Append("/" + controllerName);
            }

            if (actionName != "Index")
            {
                ancor.Append("/" + actionName);
            }

            if (queryString != string.Empty)
            {
                ClsCrypto crypto = new ClsCrypto("Crypto");

                if (IsRoute == false)
                {
                    ancor.Append("?q=" + crypto.Encrypt(queryString));
                    //ancor.Append("?q=" + NimboleCommon.Encrypt(queryString));
                }
                else
                {
                    ancor.Append("?q=" + crypto.Encrypt(queryString));
                    //ancor.Append("/" + NimboleCommon.Encrypt(queryString));
                }
            }
            ancor.Append("'");
            ancor.Append(">");
            ancor.Append(linkText);
            ancor.Append("</a>");
            return new MvcHtmlString(ancor.ToString());
        }
    }

    public static class UrlExtensions
    {
        public static string EncodeActionUrl(this UrlHelper urlHelper, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            string queryString = string.Empty;
            string htmlAttributesString = string.Empty;
            string AreaName = string.Empty;
            bool IsRoute = false;

            if (routeValues != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(routeValues);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    if (!d.Keys.Contains("IsRoute"))
                    {
                        if (i > 0)
                        {
                            queryString += "?";
                        }
                        queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                    }
                    else
                    {
                        if (!d.Keys.ElementAt(i).Contains("IsRoute"))
                        {
                            queryString += d.Values.ElementAt(i);
                            IsRoute = true;
                        }
                    }
                }
            }

            if (htmlAttributes != null)
            {
                RouteValueDictionary d = new RouteValueDictionary(htmlAttributes);
                for (int i = 0; i < d.Keys.Count; i++)
                {
                    htmlAttributesString += " " + d.Keys.ElementAt(i).Trim('@') + "=" + d.Values.ElementAt(i);
                }
            }

            StringBuilder ancor = new StringBuilder();
            if (htmlAttributesString != string.Empty)
            {
                ancor.Append(htmlAttributesString);
            }
            if (AreaName != string.Empty)
            {
                ancor.Append("/" + AreaName);
            }
            if (controllerName != string.Empty)
            {
                ancor.Append("/" + controllerName);
            }

            //if (actionName != "Index")
            {
                ancor.Append("/" + actionName);
            }

            if (queryString != string.Empty)
            {
                ClsCrypto crypto = new ClsCrypto("Crypto");

                if (IsRoute == false)
                {
                    ancor.Append("?q=" + crypto.Encrypt(queryString));
                    //ancor.Append("?q=" + NimboleCommon.Encrypt(queryString));
                }
                else
                {
                    ancor.Append("?q=" + crypto.Encrypt(queryString));
                    //ancor.Append("/" + NimboleCommon.Encrypt(queryString));
                }
            }
            return ancor.ToString();
        }

        public static string EncodeString(string plainText)
        {
            ClsCrypto crypto = new ClsCrypto("Crypto");
            string result = crypto.Encrypt(plainText);
            return result;
        }
    }
}