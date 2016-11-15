using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NIMBOLE.Models;

namespace NIMBOLE.UI.Helpers
{
    public class DateTimeZone
    {
        public static DateTime Now
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        public static DateTime DateConvert(DateTime convert)
        {


            //var zoneFromSettings = (from n in dbcontext where n.Subdomain == "http://nimbole.cloudapp.net/" select n.TimeZone).FirstOrDefault();


            //if (string.IsNullOrEmpty(zoneFromSettings))
            //    zoneFromSettings = "Pacific Standard Time";

            //var zone = TimeZoneInfo.FindSystemTimeZoneById(zoneFromSettings);

            //var convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(convert, zone);




            //return convertedDateTime;
            return DateTime.Now;

        }
    }
}