namespace Core.Plugins
{
    /// <summary>
    /// Constant strings that are referenced project-wide, and potentially by consumers
    /// </summary>
    public static partial class Constants
    {
        /// <summary>
        /// Constant strings related to Configuration, regardless of the configuration souce
        /// </summary>
        public static class Configuration
        {
            /// <summary>
            /// Constant strings related to Configuration AppSettings
            /// </summary>
            public static class AppSettings
            {
                /// <summary>
                /// The Id of the Applicataion (usually stored in a config file and may be tied to a database record)
                /// </summary>
                public const string ApplicationId = "ApplicationId";

                /// <summary>
                /// The name of the Application
                /// </summary>
                public const string ApplicationName = "ApplicationName";

                /// <summary>
                /// The Environment Code of the Application (please note, this should always be a string reprentation of a value that can be found on the EnvironmentCode enum
                /// </summary>
                public const string EnvironmentCode = "EnvironmentCode";

                /// <summary>
                /// A flag usually contained in a config file which indicates whether or not the Application is in Debug mode
                /// </summary>
                public const string IsDebugMode = "IsDebugMode";
            }
        }
    }
}
