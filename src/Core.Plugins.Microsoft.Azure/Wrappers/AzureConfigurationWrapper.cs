using System;
using Microsoft.Azure;
using Core.Exceptions;
using Core.Configuration;

namespace Core.Plugins.Microsoft.Azure.Wrappers
{
    public class AzureConfigurationWrapper : IConfiguration
    {
        public TReturn GetAppSetting<TReturn>(string key)
        {
            string value = CloudConfigurationManager.GetSetting(key);

            if (String.IsNullOrEmpty(value))
            {
                throw new CoreException($"Could not find any AppSetting with the key {key}");
            }

            return (TReturn)Convert.ChangeType(value, typeof(TReturn));
        }

        public bool IsAppSettingAvailable(string key)
        {
            string value = CloudConfigurationManager.GetSetting(key);

            return !String.IsNullOrEmpty(value);
        }
    }
}
