using AutoMapper;
using System.Linq;
using System.Reflection;

namespace Core.Plugins.AutoMapper.Extensions
{
    public static class AutoMapperExtensions
    {
        public static string[] CreatedFieldNames =
        {
            "CreatedBy",
            "CreatedDate"
        };

        public static string[] ModifiedFieldNames =
        {
            "ModifiedBy",
            "ModifiedDate"
        };

        public static IMappingExpression<TSource, TDestination> IgnoreCreatedFields<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            return IgnoreFields(expression, CreatedFieldNames);
        }

        public static IMappingExpression<TSource, TDestination> IgnoreModifiedFields<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            return IgnoreFields(expression, ModifiedFieldNames);
        }

        public static IMappingExpression<TSource, TDestination> IgnoreAuditFields<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            return IgnoreFields(expression, CreatedFieldNames.Union(ModifiedFieldNames).ToArray());
        }

        public static IMappingExpression<TSource, TDestination> IgnoreFields<TSource, TDestination>(IMappingExpression<TSource, TDestination> expression, string[] fieldsToIgnore)
        {
            var destinationProperties = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in destinationProperties.Where(p => fieldsToIgnore.Contains(p.Name)))
            {
                if (typeof(TSource).GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }

            return expression;
        }
    }
}
