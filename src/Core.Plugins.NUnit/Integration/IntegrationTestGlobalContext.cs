using Microsoft.Extensions.Hosting;

namespace Core.Plugins.NUnit.Integration
{
    /// <summary>
    /// A container for any object that need to be shared by all tests within a namespace
    /// </summary>
    public static class IntegrationTestGlobalContext
    {
        /// <summary>
        /// A read-only singleton of the IHost for the tests to run against
        /// </summary>
        public static IHost Host { get; set; }
    }
}
