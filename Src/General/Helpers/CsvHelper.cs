using System;
using System.Collections.Generic;

namespace General.Helpers
{
    /// <summary>
    /// Encapsulates functions in a static class to work with comma-separated values (CSV).
    /// </summary>
    public static partial class CsvHelper
    {
        /// <summary>
        /// Reads all the lines of a CSV string and reaturns all of data in a list of string array.
        /// </summary>
        /// <param name="csvText">A string cotains comma-separated values (CSV), one line or multi lines.</param>
        /// <returns>List of arrays of string.</returns>
        public static List<string[]> ReadAll(string csvText)
        {
            List<string[]> result = new List<string[]>();

            string[] cvsLines = csvText.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string cvsLine in cvsLines)
            {
                //result.Add(cvsLine.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                result.Add(ReadLine(cvsLine));
            }

            return result;
        }

        /// <summary>
        /// Reads a line of a comma-separated values (CSV) and an array of string, each item holds a value, respectively.
        /// </summary>
        /// <param name="csvLine"></param>
        /// <returns>Arry of string.</returns>
        public static string[] ReadLine(string csvLine)
        {
            List<string> row = new List<string>();

            if (String.IsNullOrEmpty(csvLine))
                return row.ToArray();

            int pos = 0;
            int rows = 0;

            while (pos < csvLine.Length)
            {
                string value;

                // Special handling for quoted field
                if (csvLine[pos] == '"')
                {
                    // Skip initial quote
                    pos++;

                    // Parse quoted value
                    int start = pos;
                    while (pos < csvLine.Length)
                    {
                        // Test for quote character
                        if (csvLine[pos] == '"')
                        {
                            // Found one
                            pos++;

                            // If two quotes together, keep one
                            // Otherwise, indicates end of value
                            if (pos >= csvLine.Length || csvLine[pos] != '"')
                            {
                                pos--;
                                break;
                            }
                        }
                        pos++;
                    }
                    value = csvLine.Substring(start, pos - start);
                    value = value.Replace("\"\"", "\"");
                }
                else
                {
                    // Parse unquoted value
                    int start = pos;
                    while (pos < csvLine.Length && csvLine[pos] != ',')
                        pos++;
                    value = csvLine.Substring(start, pos - start);
                }

                // Add field to list
                if (rows < row.Count)
                    row[rows] = value;
                else
                    row.Add(value);
                rows++;

                // Eat up to and including next comma
                while (pos < csvLine.Length && csvLine[pos] != ',')
                    pos++;
                if (pos < csvLine.Length)
                    pos++;
            }
            // Delete any unused items
            while (row.Count > rows)
                row.RemoveAt(rows);

            // Return true if any columns read
            return row.ToArray();
        }
    }
}
