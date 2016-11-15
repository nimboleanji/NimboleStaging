using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIMBOLE.Entities;

namespace NIMBOLE.Models.Models
{
    public static class CustomMapper
    {
        public static void MappingModels()
        {
            Mapper.CreateMap<DateTime?, DateTime>().ConvertUsing<NIMBOLE.Common.Formats.DateTimeConverter>();
            Mapper.CreateMap<DateTime?, DateTime?>().ConvertUsing<NIMBOLE.Common.Formats.NullableDateTimeConverter>();
            #region TblEmployeeRole and EmployeeRoleModel
            //Source: TblEmployeeRole, Destination: EmployeeRoleModel
            Mapper.CreateMap<EmployeeRoleModel, TblEmployeeRole>()
            .ForMember(yr => yr.CreatedDate, y => y.MapFrom(x => (x.CreatedDate.Year == 1) ? DateTime.Now : x.CreatedDate))
            .ForMember(yr => yr.ModifiedDate, y => y.MapFrom(x => (x.ModifiedDate.Year == 1) ? DateTime.Now : x.ModifiedDate))
            .ForMember(dest => dest.ModifiedDate, opt => opt.UseValue(DateTime.UtcNow));

            //Source: EmployeeRoleModel, Destination: TblEmployeeRole 
            Mapper.CreateMap<TblEmployeeRole, EmployeeRoleModel>()
            .ForMember(yr => yr.CreatedDate, y => y.MapFrom(x => (x.CreatedDate.Value.Year == 1) ? DateTime.Now : x.CreatedDate))
            .ForMember(yr => yr.ModifiedDate, y => y.MapFrom(x => (x.ModifiedDate.Value.Year == 1) ? DateTime.Now : x.ModifiedDate))
            .ForMember(dest => dest.ModifiedDate, opt => opt.UseValue(DateTime.UtcNow));

            #endregion

            Mapper.AssertConfigurationIsValid();
        }
    }
}
