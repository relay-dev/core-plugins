using System;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace Core.Plugins.EntityFramework.CodeFirst.DataAnnotations
{
    public class DateTimePrecisionAttribute : DataAnnotationPropertyAttribute
    {
        public int Precision { get; set; }

        public DateTimePrecisionAttribute(int precision)
        {
            Precision = precision;
        }

        public override void Configure(ConventionPrimitivePropertyConfiguration configuration)
        {
            configuration.HasPrecision(Convert.ToByte(Precision));
        }
    }
}
