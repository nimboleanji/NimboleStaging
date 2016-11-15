using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEMBO.Common
{
    public class FooDTO
    {
        public DateTime? FooDate { get; set; }
    }

    public class FooPoco
    {
        public DateTime? FooDate { get; set; }
    }

    class MainTest
    {
        static void Main(string[] args)
        {
            Mapper.CreateMap<DateTime?, DateTime>().ConvertUsing<DateTimeConverter>();
            Mapper.CreateMap<DateTime?, DateTime?>().ConvertUsing<NullableDateTimeConverter>();
            Mapper.AssertConfigurationIsValid();

            Mapper.CreateMap<FooDTO, FooPoco>();
                 //.ForMember(dest => dest.FooDate, opt => opt.UseValue(DateTime.UtcNow));
            Mapper.CreateMap<DateTime?, DateTime>().ConvertUsing<DateTimeConverter>();
            var poco = new FooPoco();
            Mapper.Map(new FooDTO() { }, poco);

            if (poco.FooDate.HasValue)
                Console.WriteLine(
                    "This should be null : {0}",
                    poco.FooDate.Value.ToString()); //Value is always set 
            else
                Console.WriteLine("Mapping worked");
            Console.ReadKey();
        }
    }
    
}
