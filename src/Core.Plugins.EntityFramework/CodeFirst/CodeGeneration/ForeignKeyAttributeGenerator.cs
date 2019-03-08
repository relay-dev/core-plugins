using System;
using System.Data.Entity.Core.Metadata.Edm;

namespace Core.Plugins.EntityFramework.CodeFirst.CodeGeneration
{
    public class ForeignKeyAttributeGenerator : IFluentReplacementAttributeGenerator
    {
        public string ReplacesFluentConfigurationName
        {
            get { return "ForeignKeyConfiguration"; }
        }

        public string GenerateAttributeDeclaration(EdmMember member)
        {
            var property = member as EdmProperty;

            if (property == null)
            {
                return "//Nothing to do for ForeignKeyAttributeGenerator.";
            }

            return String.Format(@"[ForeignKey(""{0}"")]", property.Name);
        }
    }
}
