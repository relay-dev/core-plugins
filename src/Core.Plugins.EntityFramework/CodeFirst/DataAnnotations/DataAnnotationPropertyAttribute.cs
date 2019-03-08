using System;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace Core.Plugins.EntityFramework.CodeFirst.DataAnnotations
{
    public abstract class DataAnnotationPropertyAttribute : Attribute
    {
        public abstract void Configure(ConventionPrimitivePropertyConfiguration configuration);
    }
}
