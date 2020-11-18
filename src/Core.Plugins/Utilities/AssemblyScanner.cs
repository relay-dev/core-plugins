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
        public IEnumerable<Type> FindTypesWithAttribute<TAttribute>(IEnumerable<Assembly> assemblies, Func<TAttribute, bool> predicate = null) where TAttribute : Attribute
        {
            if (predicate != null)
            {
                return assemblies
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.GetCustomAttributes(typeof(TAttribute), true).Any(a => predicate.Invoke((TAttribute)a)))
                    .ToList();
            }

            return assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttributes(typeof(TAttribute), true).Any())
                .ToList();
        }

        public IEnumerable<Type> FindTypesWithBaseClass<TBaseClass>(IEnumerable<Assembly> assemblies, Func<Type, bool> predicate = null)
        {
            if (predicate != null)
            {
                return assemblies
                    .SelectMany(a => a.GetTypes())
                    .Where(t => (t.IsSubclassOf(typeof(TBaseClass)) || t == typeof(TBaseClass)) && predicate.Invoke(t))
                    .ToList();
            }

            return assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(TBaseClass)) || t == typeof(TBaseClass))
                .ToList();
        }

        public IEnumerable<Type> FindTypesWithInterface<TInterface>(IEnumerable<Assembly> assemblies, Func<Type, bool> predicate = null)
        {
            if (predicate != null)
            {
                return assemblies
                    .SelectMany(a => a.GetTypes())
                    .Where(t => (t.GetInterfaces().Contains(typeof(TInterface)) || t.IsAssignableFrom(typeof(TInterface))) && predicate.Invoke(t))
                    .ToList();
            }

            return assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterfaces().Contains(typeof(TInterface)) || t.IsAssignableFrom(typeof(TInterface)))
                .ToList();
        }

        public IEnumerable<Assembly> GetApplicationAssemblies()
        {
            List<Assembly> loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .OrderBy(a => a.FullName)
                .ToList();

            string[] referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            foreach (string path in referencedPaths)
            {
                loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path)));
            }

            return loadedAssemblies.Distinct();
        }

        public static AssemblyScanner Instance => new AssemblyScanner();
    }
}
