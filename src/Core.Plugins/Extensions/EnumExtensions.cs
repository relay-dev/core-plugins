using System;
using System.Reflection;
using Core.Application;

namespace Core.Plugins.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets a boolean which represents the overall purpose of a runtime Environment
        /// </summary>
        /// <returns><c>true</c> if the given HostEnvironment represents a Mocked runtime Environment; otherwise, <c>false</c>.</returns>
        public static bool IsMockInstance(this HostEnvironment hostEnvironment)
        {
            HostEnvironmentAttribute hostEnvironmentAttribute = GetAttributeOfType<HostEnvironmentAttribute>(hostEnvironment);

            return hostEnvironmentAttribute != null && hostEnvironmentAttribute.IsMockInstance;
        }

        /// <summary>
        /// Gets a boolean which represents the overall purpose of a runtime Environment
        /// </summary>
        /// <returns><c>true</c> if the given HostEnvironment represents a Development runtime Environment; otherwise, <c>false</c>.</returns>
        public static bool IsDevelopmentInstance(this HostEnvironment hostEnvironment)
        {
            HostEnvironmentAttribute hostEnvironmentAttribute = GetAttributeOfType<HostEnvironmentAttribute>(hostEnvironment);

            return hostEnvironmentAttribute != null && hostEnvironmentAttribute.IsDevelopmentInstance;
        }

        /// <summary>
        /// Gets a boolean which represents the overall purpose of a runtime Environment
        /// </summary>
        /// <returns><c>true</c> if the given HostEnvironment represents a Testing runtime Environment; otherwise, <c>false</c>.</returns>
        public static bool IsTestingInstance(this HostEnvironment hostEnvironment)
        {
            HostEnvironmentAttribute hostEnvironmentAttribute = GetAttributeOfType<HostEnvironmentAttribute>(hostEnvironment);

            return hostEnvironmentAttribute != null && hostEnvironmentAttribute.IsTestingInstance;
        }

        /// <summary>
        /// Gets a boolean which represents the overall purpose of a runtime Environment
        /// </summary>
        /// <returns><c>true</c> if the given HostEnvironment represents a Beta runtime Environment; otherwise, <c>false</c>.</returns>
        public static bool IsBetaInstance(this HostEnvironment hostEnvironment)
        {
            HostEnvironmentAttribute hostEnvironmentAttribute = GetAttributeOfType<HostEnvironmentAttribute>(hostEnvironment);

            return hostEnvironmentAttribute != null && hostEnvironmentAttribute.IsBetaInstance;
        }

        /// <summary>
        /// Gets a boolean which represents the overall purpose of a runtime Environment
        /// </summary>
        /// <returns><c>true</c> if the given HostEnvironment represents a Production runtime Environment; otherwise, <c>false</c>.</returns>
        public static bool IsProductionInstance(this HostEnvironment hostEnvironment)
        {
            HostEnvironmentAttribute hostEnvironmentAttribute = GetAttributeOfType<HostEnvironmentAttribute>(hostEnvironment);

            return hostEnvironmentAttribute != null && hostEnvironmentAttribute.IsProductionInstance;
        }

        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example>string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;</example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            // Credit: https://stackoverflow.com/questions/1799370/getting-attributes-of-enums-value</remarks>
            Type type = enumVal.GetType();
            MemberInfo[] memInfo = type.GetMember(enumVal.ToString());
            object[] attributes = memInfo[0].GetCustomAttributes(typeof(T), false);

            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static string ToCssString(this string mdiIcon)
        {
            return mdiIcon.ToString().Replace("_", "-");
        }
    }
}
