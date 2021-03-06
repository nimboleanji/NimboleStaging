﻿using System.Web;
using System.Web.Optimization;

namespace NIMBOLE.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
            "~/Scripts/kendo/2014.3.1411/kendo.all.min.js",
            "~/Scripts/kendo/kendo.timezones.min.js", // uncomment if using the Scheduler
            "~/Scripts/kendo/2014.3.1411/kendo.aspnetmvc.min.js",
			"~/Scripts/kendo.modernizr.custom.js"));

            bundles.Add(new StyleBundle("~/Content/kendo/2014.3.1119/css").Include(
            "~/Content/kendo/2014.3.1119/kendo.common-bootstrap.min.css",
            "~/Content/kendo/2014.3.1119/kendo.bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/assets/css").Include(

                    "~/assets/js-core/jquery-cookie.js"));

           
            bundles.IgnoreList.Clear();

        }
    }
}
