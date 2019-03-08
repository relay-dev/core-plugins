using System;
using System.Data.Entity.Core.Metadata.Edm;

namespace Core.Plugins.EntityFramework.CodeFirst.CodeGeneration
{
    public class DateTimePrecisionAttributeGenerator : IFluentReplacementAttributeGenerator
    {
        public string ReplacesFluentConfigurationName
        {
            get { return "PrecisionDateTimeConfiguration"; }
        }

        public string GenerateAttributeDeclaration(EdmMember member)
        {
            var property = member as EdmProperty;

            if (property == null)
            {
                return "//Nothing to do for DateTimePrecisionAttributeGenerator.";
            }

            return String.Format(@"[DateTimePrecision({0})]", Convert.ToInt32(property.Precision));
        }
    }
}
