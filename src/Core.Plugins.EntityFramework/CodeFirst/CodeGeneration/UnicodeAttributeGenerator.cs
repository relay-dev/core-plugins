using System;
using System.Data.Entity.Core.Metadata.Edm;

namespace Core.Plugins.EntityFramework.CodeFirst.CodeGeneration
{
    public class UnicodeAttributeGenerator : IFluentReplacementAttributeGenerator
    {
        public string ReplacesFluentConfigurationName
        {
            get { return "NonUnicodeConfiguration"; }
        }

        public string GenerateAttributeDeclaration(EdmMember member)
        {
            var property = member as EdmProperty;

            if (property == null)
            {
                return "//Nothing to do for UnicodeAttributeGenerator.";
            }

            return String.Format(@"[Unicode({0})]", (property.IsUnicode.HasValue && property.IsUnicode.Value).ToString().ToLower());
        }
    }
}
