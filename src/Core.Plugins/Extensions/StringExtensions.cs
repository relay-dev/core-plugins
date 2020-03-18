using Core.Exceptions;
using Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Plugins.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns an empty string if this string is null
        /// </summary>
        public static string AsSafe(this string str)
        {
            return str ?? String.Empty;
        }

        /// <summary>
        /// Removes a substring from a string
        /// </summary>
        public static string Remove(this string str, string valueToRemove)
        {
            return str.Replace(valueToRemove, "");
        }

        /// <summary>
        /// Determines if the string contains only alpha characters
        /// </summary>
        public static bool IsAlpha(this string str)
        {
            return str.All(Char.IsLetter);
        }

        /// <summary>
        /// Determines if the string contains only numeric characters
        /// </summary>
        public static bool IsNumeric(this string str)
        {
            return str.All(Char.IsDigit);
        }

        /// <summary>
        /// Determines if the string contains only alpha-numeric characters
        /// </summary>
        public static bool IsAlphaNumeric(this string str)
        {
            return str.All(Char.IsLetterOrDigit);
        }

        /// <summary>
        /// Determines if the string contains only uppercase characters
        /// </summary>
        public static bool IsUpper(this string str)
        {
            return str.All(Char.IsUpper);
        }

        /// <summary>
        /// Determines if the string contains only lowercase characters
        /// </summary>
        public static bool IsLower(this string str)
        {
            return str.All(Char.IsLower);
        }

        /// <summary>
        /// Strips out all non-Alpha characters from this string
        /// </summary>
        public static string ToAlpha(this string str)
        {
            return Regex.Replace(str, "[^A-Za-z]", String.Empty);
        }

        /// <summary>
        /// Strips out all Alpha characters from this string
        /// </summary>
        public static string ToNumeric(this string str)
        {
            return Regex.Replace(str, "[^0-9]", String.Empty);
        }

        /// <summary>
        /// Strips out all non-AlphaNumeric characters from this string
        /// </summary>
        public static string ToAlphaNumeric(this string str)
        {
            return Regex.Replace(str, "[^A-Za-z0-9]", String.Empty);
        }

        /// <summary>
        /// Removes leading 0's from this string
        /// </summary>
        public static string StripLeadingZeros(this string str)
        {
            return str.TrimStart(new char[] { '0' });
        }

        /// <summary>
        /// Removes trailing 0's from this string
        /// </summary>
        public static string StripTrailingZeros(this string str)
        {
            return str.TrimEnd(new char[] { '0' });
        }

        /// <summary>
        /// Converts an empty string into null
        /// </summary>
        public static string ToNullIfEmpty(this string str)
        {
            if (str == null)
                return null;

            return String.IsNullOrEmpty(str.Trim()) ? null : str;
        }

        /// <summary>
        /// Turns a string like "ThisIsAString" into "This Is A String"
        /// </summary>
        public static string FromCamcelCase(this string str)
        {
            return Regex.Replace(str, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
        }

        /// <summary>
        /// Lets you do a String.Contains() while using StringComparison
        /// </summary>
        public static bool Contains(this string str, string stringToFind, StringComparison stringComparison)
        {
            return str.IndexOf(stringToFind, stringComparison) >= 0;
        }

        /// <summary>
        /// Given a delimited string, this will split it and return the values cleanly in a List
        /// </summary>
        public static List<string> DelimitedToList(this string str, char delimiter = ',')
        {
            return str.AsSafe().Split(delimiter).Where(s => !String.IsNullOrEmpty(s)).Select(s => s.Trim()).ToList();
        }

        /// <summary>
        /// Given a delimited string, this will split it and return the values cleanly in a Dictionary
        /// </summary>
        public static Dictionary<string, string> DelimitedToDictionary(this string str, char outerDelimiter = '|', char innerDelimiter = ',')
        {
            return str.AsSafe().Split(outerDelimiter).Where(s => !String.IsNullOrEmpty(s))
                .ToDictionary(s => s.DelimitedToList(innerDelimiter)[0],
                              s => s.Contains(innerDelimiter) ? s.DelimitedToList(innerDelimiter)[1] : null);
        }

        /// <summary>
        /// Removes the last occurrence of a substring from a string
        /// </summary>
        public static string RemoveLastOccurrenceOf(this string str, string valueToRemove)
        {
            if (str == null)
                return null;

            int lastIndexOf = str.LastIndexOf(valueToRemove, StringComparison.Ordinal);

            return str.Remove(lastIndexOf, valueToRemove.Length);
        }

        /// <summary>
        /// Throws a <see cref="CoreException"/> if the given string is null or empty
        /// </summary>
        public static string ThrowIfNullOrEmpty(this string str, string objectName, string message = null)
        {
            if (String.IsNullOrEmpty(str))
            {
                string msg = $"{objectName} cannot be null";

                if (message != null)
                {
                    msg = $". {message}";
                }

                throw new CoreException(ErrorCode.INVA, msg);
            }

            return str;
        }

        /// <summary>
        /// Splits this string and will upper the first character of each element. It will then join the elements back as specified (by default, it joins them without any whitespace)
        /// </summary>
        /// <param name="str">The string to format</param>
        /// <param name="splitBy">The character that should be used to split the string into individual elements</param>
        /// <param name="joinWith">The string that should be used to join the elements back together, which is an empty string by default</param>
        /// <returns></returns>
        public static string ToCamelCase(this string str, char splitBy = '_', string joinWith = "")
        {
            if (str == null)
                return null;

            string returnString = String.Empty;

            try
            {
                foreach (string substring in str.Split(splitBy))
                {
                    returnString += ToUpperFirstLetter(substring) + joinWith;
                }

            }
            catch (Exception)
            {
                return str;
            }

            return joinWith == String.Empty
                ? returnString
                : returnString.Substring(0, returnString.LastIndexOf(joinWith));
        }

        /// <summary>
        /// Uppers the first character, lowers all subsequent characters
        /// </summary>
        public static string ToUpperFirstLetter(this string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1).ToLower();

            return str.ToUpper();
        }

        /// <summary>
        /// Takes this string in the singular tense and will make it plural if the count of the items it's associated to is greater than 1
        /// </summary>
        /// <param name="str">The string to potentially make plural</param>
        /// <param name="itemCount">The count of the items that dictates whether or not the string should be plural</param>
        /// <param name="appropriateSuffix">The string that should be used when making the word plural</param>
        /// <returns></returns>
        public static string ToPluralIfNeeded(this string str, int itemCount, string appropriateSuffix = "s")
        {
            return itemCount > 1
                ? str + appropriateSuffix
                : str;
        }

        /// <summary>
        /// Returns a substring starting the first index of the removeAfter parameter, ending at the end of the string
        /// </summary>
        public static string SubstringBefore(this string str, string removeAfter, bool includeRemoveAfterString = false)
        {
            if (str == null)
                return null;

            try
            {
                return str.Substring(0, str.IndexOf(removeAfter, StringComparison.Ordinal) + (includeRemoveAfterString ? 1 : 0));
            }
            catch
            {
                return str;
            }
        }

        /// <summary>
        /// Returns a substring starting the 0th position, ending at the removeAfter parameter
        /// </summary>
        public static string SubstringAfter(this string str, string removeBefore, bool includeRemoveBeforeString = false)
        {
            if (str == null)
                return null;

            if (removeBefore == null || !str.Contains(removeBefore))
                return str;

            try
            {
                return str.Substring(str.IndexOf(removeBefore, StringComparison.Ordinal) + (includeRemoveBeforeString ? 0 : 1));
            }
            catch
            {
                return str;
            }
        }

        /// <summary>
        /// Returns a substring between two other substrings
        /// </summary>
        public static string SubstringBetween(this string str, string firstSubstringToFind, string lastSubstringToFind)
        {
            if (str == null)
                return null;

            int endingIndex = str.LastIndexOf(lastSubstringToFind, StringComparison.Ordinal);

            if (endingIndex > 0 && endingIndex < str.Length)
                str = str.Remove(endingIndex);

            int startingIndex = str.IndexOf(firstSubstringToFind, StringComparison.Ordinal);

            if (startingIndex > -1)
                str = str.Substring(startingIndex + firstSubstringToFind.Length);

            return str;
        }

        /// <summary>
        /// Removes a specified number of characters from the end of a string
        /// </summary>
        public static string RemoveCharactersFromEnd(this string str, int characterCountToRemove)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;

            try
            {
                return str.Substring(0, str.Length - characterCountToRemove);
            }
            catch (Exception)
            {
                return str;
            }
        }

        /// <summary>
        /// This is used when casting our enums to strings. There is always an Unknown value in the 0th position, which should usually be resolved to null
        /// </summary>
        public static string ResolveUnknown(this string str)
        {
            return String.IsNullOrEmpty(str) || str.ToLower() == "unknown"
                ? null
                : str;
        }

        public static string SafeSubstring(this string inputString, int startIndex, int length = -1)
        {
            if (inputString == null)
                return null;

            int inputLength = inputString.Length;
            int adjustedStartIndex = (startIndex >= 0) ? startIndex : 0;

            if (adjustedStartIndex >= inputLength)
                return null;

            if (length < 0 || (adjustedStartIndex + length) > inputLength)
                return inputString.Substring(adjustedStartIndex);

            return inputString.Substring(adjustedStartIndex, length);
        }

        public static string RemoveNonAlphaNumericCharacters(this string inputString, params char[] excludeCharacters)
        {
            if (inputString == null)
                return null;

            var outputString = new StringBuilder();
            bool hasExcludeCharacters = (excludeCharacters != null && excludeCharacters.Count() > 0);
            int stringLength = inputString.Length;

            for (int i = 0; i < stringLength; i++)
            {
                char character = inputString[i];

                if (Char.IsLetterOrDigit(character) || (hasExcludeCharacters && excludeCharacters.Contains(character)))
                    outputString.Append(character);
            }

            return outputString.ToString();
        }

        public static string RemoveNonAlphaCharacters(this string inputString, params char[] excludeCharacters)
        {
            if (inputString == null)
                return null;

            var outputString = new StringBuilder();
            bool hasExcludeCharacters = (excludeCharacters != null && excludeCharacters.Count() > 0);
            int stringLength = inputString.Length;

            for (int i = 0; i < stringLength; i++)
            {
                char character = inputString[i];

                if (Char.IsLetter(character) || (hasExcludeCharacters && excludeCharacters.Contains(character)))
                    outputString.Append(character);
            }

            return outputString.ToString();
        }

        public static string FormatName(this string namePart)
        {
            if (namePart == null)
                return null;

            return namePart.Replace("-", " ").RemoveNonAlphaCharacters(' ');
        }
    }
}
