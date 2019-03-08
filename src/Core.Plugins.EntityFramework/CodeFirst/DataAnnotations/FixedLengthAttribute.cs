using System.Data.Entity.ModelConfiguration.Configuration;

namespace Core.Plugins.EntityFramework.CodeFirst.DataAnnotations
{
    public class FixedLengthAttribute : DataAnnotationPropertyAttribute
    {
        public bool IsFixedLength { get; set; }

        public FixedLengthAttribute(bool isFixedLength)
        {
            IsFixedLength = isFixedLength;
        }

        public override void Configure(ConventionPrimitivePropertyConfiguration configuration)
        {
            if (IsFixedLength)
            {
                configuration.IsFixedLength();
            }
        }
    }
}
