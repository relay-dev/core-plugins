using System.Text;

namespace Core.Plugins.Extensions
{
    public static class StringBuilderExtensions
    {
        public static string ToStringOrNull(this StringBuilder stringBuilder)
        {
            if (stringBuilder == null)
                return null;

            return stringBuilder.ToString();
        }

        public static void AppendLineWithFormat(this StringBuilder stringBuilder, string str, params object[] args)
        {
            stringBuilder.AppendFormat(str, args);
            stringBuilder.AppendLine();
        }
    }
}
