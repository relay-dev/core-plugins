using AutoMapper;
using Core.Plugins.AutoMapper.Mappers;
using System;
using System.Collections.Generic;

namespace Core.Plugins.AutoMapper.Extensions
{
    public static class MapperConfigurationExtensions
    {
        public static IMapperConfigurationExpression AddCoreAutoMappers(this IMapperConfigurationExpression cfg)
        {
            foreach (Type t in CoreMapperTypes)
            {
                cfg.AddProfile(t);
            }

            return cfg;
        }

        private static readonly List<Type> CoreMapperTypes = new List<Type>
        {
            typeof(PrimitiveMappers),
            typeof(SystemMappers)
        };
    }
}
