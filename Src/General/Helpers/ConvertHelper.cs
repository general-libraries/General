using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using General.Extensions;

namespace General.Helpers
{
    public static class ConvertHelper
    {
        public static int ToInt32(object obj, int defaultValue = 0)
        {
            if (obj is int) { return (int)obj; }
            return ToInt32(Convert.ToString(obj).RemoveSpaces(), defaultValue);
        }

        public static int ToInt32(string str, int defaultValue = 0)
        {
            if (string.IsNullOrWhiteSpace(str)) { return defaultValue; }

            if (str.Contains("."))
            {
                var decimalValue = ToDecimal(str, defaultValue);
                decimalValue = Math.Round(decimalValue, MidpointRounding.AwayFromZero);
                return decimalValue <= int.MaxValue && decimalValue >= int.MinValue
                    ? (int)decimalValue
                    : defaultValue;
            }

            int result;

            return int.TryParse(str, out result)
                ? result
                : defaultValue;
        }

        public static long ToInt64(object obj, long defaultValue = 0)
        {
            if (obj is long) { return (long)obj; }
            return ToInt64(Convert.ToString(obj).RemoveSpaces(), defaultValue);
        }

        public static long ToInt64(string str, long defaultValue = 0)
        {
            if (string.IsNullOrWhiteSpace(str)) { return defaultValue; }

            if (str.Contains("."))
            {
                var decimalValue = ToDecimal(str, defaultValue);
                decimalValue = Math.Round(decimalValue, MidpointRounding.AwayFromZero);
                return decimalValue <= long.MaxValue && decimalValue >= long.MinValue
                    ? (long)decimalValue
                    : defaultValue;
            }

            long result;

            return long.TryParse(str, out result)
                ? result
                : defaultValue;
        }

        public static decimal ToDecimal(object obj, decimal defaultValue = 0M)
        {
            if (obj is decimal) { return (decimal)obj; }
            return ToDecimal(Convert.ToString(obj).RemoveSpaces(), defaultValue);
        }

        public static decimal ToDecimal(string str, decimal defaultValue = 0M)
        {
            if (string.IsNullOrWhiteSpace(str)) { return defaultValue; }

            decimal result;

            return decimal.TryParse(str, out result)
                ? result
                : defaultValue;
        }
    }
}
