using AutoMapper;
using Core.Plugins.AutoMapper.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace Core.Plugins.AutoMapper.Helpers
{
    public static class AutoMapperHelper
    {
        public static MapperConfiguration GenerateMapperConfiguration(Assembly[] assemblies)
        {
            return new MapperConfiguration(cfg =>
            {
                foreach (Assembly assembly in assemblies)
                {
                    foreach (Type type in assembly.GetTypes().Where(t => t.BaseType == typeof(Profile)))
                    {
                        cfg.AddProfile(type);
                    }
                }

                cfg.AddCoreAutoMappers();
            });
        }
    }
}
