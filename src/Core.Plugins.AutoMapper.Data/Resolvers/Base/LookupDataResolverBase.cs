using AutoMapper;
using Core.Plugins.AutoMapper.Data.LookupData;

namespace Core.Plugins.AutoMapper.Data.Resolvers.Base
{
    public abstract class LookupDataResolverBase<TSource, TDestination> : IMemberValueResolver<object, object, TSource, TDestination>
    {
        private const int DefaultCacheTimeoutInHours = 24;
        public const string CacheKeyPrefix = "Reserved::Core::LookupData::";

        protected int GetCacheTimeoutInHours(LookupDataBase lookupData)
        {
            return lookupData.CacheTimeoutInHours > 0 ? lookupData.CacheTimeoutInHours : DefaultCacheTimeoutInHours;
        }

        protected string GetCacheKey(string tableName)
        {
            return $"{CacheKeyPrefix}{tableName}";
        }

        public abstract TDestination Resolve(object source, object destination, TSource sourceMember, TDestination destMember, ResolutionContext context);
    }
}
