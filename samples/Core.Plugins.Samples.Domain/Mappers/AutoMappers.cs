using AutoMapper;
using Core.Plugins.AutoMapper.Extensions;
using Core.Plugins.Samples.Domain.DTO;
using Core.Plugins.Samples.Domain.Entities;

namespace Core.Plugins.Samples.Domain.Mappers
{
    public class AutoMappers : Profile
    {
        public AutoMappers()
        {
            CreateMap<Order, OrderEntity>().IgnoreAuditFields();
            CreateMap<OrderEntity, Order>();
        }
    }
}
