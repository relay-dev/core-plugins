using System;
using System.Data.Entity.Core.Metadata.Edm;

namespace Core.Plugins.EntityFramework.CodeFirst.CodeGeneration
{
    public class DecimalPrecisionAttributeGenerator : IFluentReplacementAttributeGenerator
    {
        public string ReplacesFluentConfigurationName
        {
            get { return "PrecisionDecimalConfiguration"; }
        }

        public string GenerateAttributeDeclaration(EdmMember member)
        {
            var property = member as EdmProperty;

            if (property == null)
            {
                return "//Nothing to do for DecimalPrecisionAttributeGenerator.";
            }

            return String.Format(@"[DecimalPrecision({0},{1})]", Convert.ToInt32(property.Precision), Convert.ToInt32(property.Scale));
        }
    }
}
