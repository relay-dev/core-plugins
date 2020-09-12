using System;
using AutoMapper;

namespace Core.Plugins.AutoMapper.Mappers
{
    public class SystemMappers : Profile
    {
        public SystemMappers()
        {
            CreateMap<bool, bool?>().ConvertUsing(src => src);
            CreateMap<bool?, bool>().ConvertUsing(src => src.GetValueOrDefault());
            CreateMap<int, int?>().ConvertUsing(src => src);
            CreateMap<int?, int>().ConvertUsing(src => src.GetValueOrDefault());
            CreateMap<long, long?>().ConvertUsing(src => src);
            CreateMap<long?, long>().ConvertUsing(src => src.GetValueOrDefault());
            CreateMap<DateTime, DateTime?>().ConvertUsing(src => src);
            CreateMap<DateTime?, DateTime>().ConvertUsing(src => src.GetValueOrDefault());

            CreateMap<DateTimeOffset, DateTime>().ConvertUsing(src => src.DateTime);
            CreateMap<DateTimeOffset, DateTime?>().ConvertUsing(src => src.DateTime);
            CreateMap<DateTimeOffset?, DateTime>().ConvertUsing(src => src.GetValueOrDefault().DateTime);
            CreateMap<DateTimeOffset?, DateTime?>().ConvertUsing(src => src.HasValue ? src.Value.DateTime : (DateTime?)null);
            CreateMap<DateTime, DateTimeOffset>().ConvertUsing(src => (DateTimeOffset)src);
            CreateMap<DateTime, DateTimeOffset?>().ConvertUsing(src => (DateTimeOffset)src);
            CreateMap<DateTime?, DateTimeOffset>().ConvertUsing(src => (DateTimeOffset)src.GetValueOrDefault());
            CreateMap<DateTime?, DateTimeOffset?>().ConvertUsing(src => src.HasValue ? src.Value : (DateTimeOffset?)null);
        }
    }
}
