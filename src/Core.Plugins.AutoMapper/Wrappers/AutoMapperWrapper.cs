using AutoMapper;
using IMapper = Core.Mapping.IMapper;

namespace Core.Plugins.AutoMapper.Wrappers
{
    public class AutoMapperWrapper : IMapper
    {
        public TDestination MapToNew<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }

        public void MapOver<TSource, TDestination>(TSource source, TDestination destination)
        {
            Mapper.Map(source, destination);
        }
    }
}
