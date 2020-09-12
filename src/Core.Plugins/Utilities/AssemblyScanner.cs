using Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Core.Plugins.Utilities
{
    public class AssemblyScanner : IAssemblyScanner
    {
        public List<Assembly> GetApplicationAssemblies()
        {
            List<Assembly> loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .OrderBy(a => a.FullName)
                .ToList();

            string[] referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            foreach (string path in referencedPaths)
            {
                loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path)));
            }

            return loadedAssemblies.Distinct().ToList();
        }

        public List<Type> GetApplicationTypesWithAttribute<TAttribute>() where TAttribute : Attribute
        {
            return GetApplicationAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttributes(typeof(TAttribute), true).Any())
                .ToList();
        }

        public List<Type> GetApplicationTypesWithAttribute<TAttribute>(Func<TAttribute, bool> predicate) where TAttribute : Attribute
        {
            return GetApplicationAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttributes(typeof(TAttribute), true).Any(a => predicate.Invoke((TAttribute)a)))
                .ToList();
        }
    }
}
