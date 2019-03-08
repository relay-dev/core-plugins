using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace Core.Plugins.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ToFormattedString(this Exception e, string startEachLineWith = null)
        {
            if (e == null)
                return null;

            if (startEachLineWith.Count(s => s == ' ') > 20)
                return "Inner Exception count exceeded 10";

            return String.Format(
                "{0}Message: {1}{5}" +
                "{0}Source: {2}{5}" +
                "{0}Stack Trace:{5}\t{0}{3}{5}" +
                "{0}Inner Exception: {4}",
                    startEachLineWith,
                    e.Message.AsSafe(),
                    e.Source.AsSafe(),
                    e.StackTrace.AsSafe().Replace("\r\n", Environment.NewLine + "\t" + startEachLineWith),
                    e.InnerException == null
                        ? "None"
                        : Environment.NewLine + ToFormattedString(e.InnerException, startEachLineWith + "  "), Environment.NewLine);
        }

        public static string ToFormattedString(this ReflectionTypeLoadException e)
        {
            var stringBuilder = new StringBuilder();

            foreach (Exception loaderException in e.LoaderExceptions)
            {
                stringBuilder.AppendLine(loaderException.Message);

                var fileNotFoundException = loaderException as FileNotFoundException;

                if (fileNotFoundException != null)
                {
                    if (!string.IsNullOrEmpty(fileNotFoundException.FusionLog))
                    {
                        stringBuilder.AppendLine("Fusion Log:");
                        stringBuilder.AppendLine(fileNotFoundException.FusionLog);
                    }
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        public static string ToFormattedString(this WebException e)
        {
            string errorCode;
            string errorMessage;

            using (WebResponse response = e.Response)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)response;

                errorCode = httpResponse.StatusCode.ToString();

                using (Stream stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        errorMessage = reader.ReadToEnd();
                    }
                }
            }

            return $"ErrorCode: {0} ErrorMessage: {1}";
        }
    }
}
