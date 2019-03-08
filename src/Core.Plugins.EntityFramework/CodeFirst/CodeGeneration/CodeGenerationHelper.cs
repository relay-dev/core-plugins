using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Data.Entity.Core.Metadata.Edm;

namespace Core.Plugins.EntityFramework.CodeFirst.CodeGeneration
{
    public class CodeGenerationHelper
    {
        private Dictionary<string, IFluentReplacementAttributeGenerator> _fluentReplacements;

        public CodeGenerationHelper()
        {
            _fluentReplacements = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && typeof(IFluentReplacementAttributeGenerator).IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t) as IFluentReplacementAttributeGenerator)
                .ToDictionary(generator => generator.ReplacesFluentConfigurationName);
        }

        public bool HasFluentReplacement(object configuration)
        {
            string key = GetKeyForConfiguration(configuration);

            return _fluentReplacements.ContainsKey(key);
        }

        public string GetAttributeDeclaration(EdmMember member, object configuration)
        {
            if (!HasFluentReplacement(configuration))
            {
                return "";
            }

            string key = GetKeyForConfiguration(configuration);

            return _fluentReplacements[key].GenerateAttributeDeclaration(member);
        }

        #region Private Methods

        private string GetKeyForConfiguration(object configuration)
        {
            return configuration.GetType().Name;
        }

        #endregion
    }
}
