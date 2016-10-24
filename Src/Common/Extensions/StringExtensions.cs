using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Extensions
{
    /// <summary>
    /// An static class to contain String extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a valid file name string of this string.
        /// It doesn't change the current string value.
        /// </summary>
        /// <param name="str">This string.</param>
        /// <returns>A valid file name string.</returns>
        public static string ToValidFileName(this string str)
        {
            IList<char> invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
            return new string(str.Select(ch => invalidFileNameChars.Contains(ch) ? '_' : ch).ToArray());
        }

        /// <summary>
        /// Returns a list of all positions of <paramref name="value"/> inside this string <paramref name="str"/>.
        /// </summary>
        /// <param name="str">This string.</param>
        /// <param name="value">A string.</param>
        /// <returns>List of all positions of <paramref name="value"/> inside this string <paramref name="str"/>.</returns>
        public static IEnumerable<int> AllIndexesOf(this string str, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", nameof(value));

            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);

                if (index == -1)
                    break;

                yield return index;
            }
        }

        /// <summary>
        /// Removes all leading and trailing white-space characters from the <paramref name="str"/>.
        /// </summary>
        /// <param name="str">A string.</param>
        /// <returns>Cleaned up string.</returns>
        public static string TrimSpacesAndDoubleQuotes(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            string result = str.Trim();
            int length = result.Length;

            // For instance:  "John" --> John
            //                " Mary  " --> Mary
            if (length > 1 && result.StartsWith("\"") && result.EndsWith("\""))
            {
                result = result.Substring(1, length - 2);
                result = result.Trim();
            }

            return result;
        }

        public static string RemoveSpaces(this string str)
        {
            return str.Replace(" ", string.Empty);
        }

        public static void AppendValueOrSpaceAndValue(this StringBuilder builder, string value)
        {
            if (builder.Length != 0)
                builder.AppendFormat(" {0}", value);
            else
                builder.Append(value);
        }

        public static string ToKabobCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            string strLower = str.ToLower();
            int length = str.Length;
            var kabobCaseBuilder = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var c = strLower[i];

                if (c != str[i] && i > 0)
                {
                    kabobCaseBuilder.Append("-");
                }

                if (c == ' ') { c = '-'; }

                kabobCaseBuilder.Append(c);
            }

            return kabobCaseBuilder.ToString();
        }

        public static string ExtractNumbers(this string str)
        {
            var extractedNumbers = string.Empty;
            if (!string.IsNullOrWhiteSpace(str))
            {
                extractedNumbers = new string(str.Where(Char.IsNumber).ToArray());
            }
            return extractedNumbers;
        }
    }

}
