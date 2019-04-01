namespace Core.Plugins
{
    /// <summary>
    /// Constant strings that are referenced project-wide, and potentially by consumers
    /// </summary>
    public static partial class Constants
    {
        /// <summary>
        /// Constant strings related to specific keywords that do not fall into any main category, but still need to be constants
        /// </summary>
        public static class Keywords
        {
            /// <summary>
            /// Active login sessions are cached, and this flag ensures it does not get deleted during a ClearCache()
            /// </summary>
            public const string ActiveLogins = "ActiveLogins";
        }
    }
}
