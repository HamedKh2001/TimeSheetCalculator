using System;
using AutoMapper;
using SharedKernel.Common;

namespace HCM.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedList<>));
            CreateMap<DateOnly, DateTime>().ConvertUsing(input => input.ToDateTime(TimeOnly.Parse("00:00 AM")));
            CreateMap<DateTime, DateOnly>().ConvertUsing(input => DateOnly.FromDateTime(input));
            CreateMap<DateTime?, DateOnly?>().ConvertUsing(input => input.HasValue ? DateOnly.FromDateTime(input.Value) : null);
        }
    }
}
