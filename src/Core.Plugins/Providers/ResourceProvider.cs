using Core.Exceptions;
using Core.Framework;
using Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace Core.Plugins.Providers
{
    [Component]
    [Injectable]
    public class ResourceProvider : IResourceProvider
    {
        private IDictionary<string, IDictionary<string, string>> _resourceStrings;

        public ResourceProvider()
        {
            _resourceStrings = new Dictionary<string, IDictionary<string, string>>();
        }

        public IDictionary<string, IDictionary<string, string>> Get()
        {
            return _resourceStrings;
        }

        public string GetMessage(string key)
        {
            return GetString("Messages", key);
        }

        public string GetString(string resourceName, string key)
        {
            string resource = this.GetType().Assembly.GetManifestResourceNames()
                .SingleOrDefault(r => String.Equals(resourceName, r, StringComparison.CurrentCultureIgnoreCase));

            if (resource == null)
                throw new CoreException($"Could not find a resource with name {resourceName}");

            return new ResourceManager(resourceName, this.GetType().Assembly).GetString(key);
        }
    }
}
