using System;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace Core.Plugins.EntityFramework.CodeFirst.DataAnnotations
{
    public class DecimalPrecisionAttribute : DataAnnotationPropertyAttribute
    {
        public int Precision { get; set; }
        public int Scale { get; set; }

        public DecimalPrecisionAttribute(int precision)
        {
            Precision = precision;
        }

        public DecimalPrecisionAttribute(int precision, int scale)
        {
            Precision = precision;
            Scale = scale;
        }

        public override void Configure(ConventionPrimitivePropertyConfiguration configuration)
        {
            configuration.HasPrecision(Convert.ToByte(Precision), Convert.ToByte(Scale));
        }
    }
}
