using Core.Plugins.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace IntegrationTests.Common
{
    public class IntegrationTestBootstrapper
    {
        public void Boostrap()
        {
            //var config = new ConfigurationBuilder()
            //    .SetBasePath(BasePath)
            //    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            //    .AddEnvironmentVariables()
            //    .Build();

            //foreach (KeyValuePair<string, string> kvp in config.AsEnumerable())
            //{
            //    Environment.SetEnvironmentVariable(kvp.Key, kvp.Value);
            //}
        }

        private string BasePath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory.SubstringBefore("tests") + @"tests\IntegrationTests";
            }
        }
    }
}
