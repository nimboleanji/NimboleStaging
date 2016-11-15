using AutoMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Common
{
    public static class Formats
    {
        public static DateTime getSettingCurrentDate(DateTime Date2Convert)
        {

            string strTimeZone = ConfigurationManager.AppSettings["DefaultTimeZone"].ToString();
            DateTime newDateTime = DateTime.Now;

            strTimeZone = "Pacific Standard Time";

            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(strTimeZone);

            newDateTime = TimeZoneInfo.ConvertTime(Date2Convert, timeZoneInfo);

            return newDateTime;
        }

        public class DateTimeConverter : ITypeConverter<DateTime?, DateTime>
        {
            public DateTime Convert(ResolutionContext context)
            {
                var sourceDate = context.SourceValue as DateTime?;
                if (sourceDate.HasValue)
                    return sourceDate.Value;
                else
                    return default(DateTime);
            }
        }

        public class NullableDateTimeConverter : ITypeConverter<DateTime?, DateTime?>
        {
            public DateTime? Convert(ResolutionContext context)
            {
                var sourceDate = context.SourceValue as DateTime?;
                if (sourceDate.HasValue)
                    return sourceDate.Value;
                else
                    return default(DateTime?);
            }
        }
    }
}
