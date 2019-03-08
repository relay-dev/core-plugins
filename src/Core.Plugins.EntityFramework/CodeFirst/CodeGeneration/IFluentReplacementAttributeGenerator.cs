using System.Data.Entity.Core.Metadata.Edm;

namespace Core.Plugins.EntityFramework.CodeFirst.CodeGeneration
{
    public interface IFluentReplacementAttributeGenerator
    {
        string ReplacesFluentConfigurationName { get; }
        string GenerateAttributeDeclaration(EdmMember member);
    }
}
