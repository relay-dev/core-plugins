using System.Data.Entity.ModelConfiguration.Configuration;

namespace Core.Plugins.EntityFramework.CodeFirst.DataAnnotations
{
    public class UnicodeAttribute : DataAnnotationPropertyAttribute
    {
        public bool IsUnicode { get; set; }

        public UnicodeAttribute(bool isUnicode)
        {
            IsUnicode = isUnicode;
        }

        public override void Configure(ConventionPrimitivePropertyConfiguration configuration)
        {
            configuration.IsUnicode(IsUnicode);
        }
    }
}
