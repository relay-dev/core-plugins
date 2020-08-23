using Core.Exceptions;
using System;
using System.Text;

namespace Core.Plugins.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToSafeDateString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return String.Empty;

            return dateTime.Value.ToShortDateString();
        }

        public static bool IsWeekDay(this DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

        public static DateTime AddBusinessDays(this DateTime dateTime, int businessDaysToAdd)
        {
            int completeWeeks = businessDaysToAdd / 5;
            DateTime date = dateTime.AddDays(completeWeeks * 7);

            businessDaysToAdd = businessDaysToAdd % 5;

            for (int i = 0; i < businessDaysToAdd; i++)
            {
                date = date.AddDays(1);

                while (!IsWeekDay(date))
                    date = date.AddDays(1);
            }

            return date;
        }

        public static DateTime ToEasternStandardTime(this DateTime dateTime)
        {
            DateTime dateTimeAsUtc = dateTime.ToUniversalTime();
            TimeZoneInfo easternZone;

            try
            {
                easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); // PC
            }
            catch
            {
                easternZone = TimeZoneInfo.FindSystemTimeZoneById("US/Eastern"); // Mac
            }

            return TimeZoneInfo.ConvertTimeFromUtc(dateTimeAsUtc, easternZone);
        }

        public static string ToPrintFriendly(this TimeSpan timeSpan, bool displaySeconds = true, bool displayMilliseconds = false)
        {
            var stringBuilder = new StringBuilder();

            if (timeSpan.Hours > 0)
                stringBuilder.AppendFormat("{0} hr. ", timeSpan.Hours);

            if (timeSpan.Hours > 0 || timeSpan.Minutes > 0)
                stringBuilder.AppendFormat("{0} min. ", timeSpan.Minutes);

            if (displaySeconds)
                stringBuilder.AppendFormat("{0:D2} sec. ", timeSpan.Seconds);

            if (displayMilliseconds)
                stringBuilder.AppendFormat("{0:D2} ms. ", timeSpan.Milliseconds);

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Throws a <see cref="CoreException"/> if the given DateTime is set to the default (min) value
        /// </summary>
        public static DateTime ThrowIfNullOrEmpty(this DateTime dateTime, string objectName, string errorCode = null)
        {
            if (errorCode == null)
            {
                errorCode = ErrorCode.INVA;
            }

            if (dateTime == DateTime.MinValue)
            {
                throw new CoreException(errorCode, $"{objectName} was DateTime.MinValue");
            }

            return dateTime;
        }
    }
}
