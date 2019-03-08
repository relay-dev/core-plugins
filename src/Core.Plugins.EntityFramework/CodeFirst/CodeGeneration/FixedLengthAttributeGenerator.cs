using System;
using System.Data.Entity.Core.Metadata.Edm;

namespace Core.Plugins.EntityFramework.CodeFirst.CodeGeneration
{
    public class FixedLengthAttributeGenerator : IFluentReplacementAttributeGenerator
    {
        public string ReplacesFluentConfigurationName
        {
            get { return "FixedLengthConfiguration"; }
        }

        public string GenerateAttributeDeclaration(EdmMember member)
        {
            var property = member as EdmProperty;

            if (property == null)
            {
                return "//Nothing to do for FixedLengthAttributeGenerator.";
            }

            return String.Format(@"[FixedLength({0})]", (property.IsFixedLength.HasValue && property.IsFixedLength.Value).ToString().ToLower());
        }
    }
}
