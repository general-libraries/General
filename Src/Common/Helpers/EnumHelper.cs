using System;
using Common.Extensions;

namespace Common.Helpers
{
    /// <summary>
    /// Contains static Enum helper methods.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Converts passed <paramref name="value"/> (either string or integer) to a value of type <typeparamref name="TEnum"/>.
        /// <typeparamref name="TEnum"/> must be an enumerator. <paramref name="value"/> can be:
        ///  - A string value equal to one of the <typeparamref name="TEnum"/>'s item (e.g. "Sunday" or "sUndAy" while <typeparamref name="TEnum"/> is <see cref="DayOfWeek"/>, returns DayOfWeek.Sunday).
        ///  - An integer value equal to one of the <typeparamref name="TEnum"/>'s item (e.g. 0 while <typeparamref name="TEnum"/> is <see cref="DayOfWeek"/>, returns DayOfWeek.Sunday).
        ///  - A comma separated string value, if <typeparamref name="TEnum"/> is decorated by <see cref="FlagsAttribute"/> (e.g. "None, RemoveEmptyEntries" or "noNE,removeemptyentries" while <typeparamref name="TEnum"/> is <see cref="StringSplitOptions"/>, returns StringSplitOptions.None|StringSplitOptions.RemoveEmptyEntries)
        /// </summary>
        /// <typeparam name="TEnum">An enum type.</typeparam>
        /// <param name="value">A string or integer value which has to convert to one of <typeparamref name="TEnum"/>'s item.</param>
        /// <returns>Equivalent <typeparamref name="TEnum"/> item for <paramref name="value"/>.</returns>
        public static TEnum Parse<TEnum>(object value)
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum argument must be an enumeraor.", "TEnum");
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            TEnum result;

            try
            {
                int intValue;
                string strValue = value.ToString().RemoveSpaces();

                if (int.TryParse(strValue, out intValue))
                {
                    result = (TEnum)Enum.ToObject(typeof(TEnum), intValue);
                }
                else
                {
                    result = (TEnum)Enum.Parse(typeof(TEnum), strValue, true);
                }

                if (Enum.IsDefined(typeof(TEnum), result) || result.ToString().Contains(","))
                {
                    //Parameter "value" has been succefully converted to a TEnum value.
                    //Everything is fine!
                }
                else
                {
                    throw new Exception(
                        $"\"{value}\" is not an underlying value of the {typeof (TEnum).FullName} enumeration.");
                }
            }
            catch (ArgumentException)
            {
                throw new Exception($"\"{value}\" is not a member of the  {typeof (TEnum).FullName} enumeration.");
            }

            return result;
        }

        /// <summary>
        /// Tries to convert passed <paramref name="value"/> (either string or integer) to a value of type <typeparamref name="TEnum"/>.
        /// If the try fails, it returens given <paramref name="defaultValue"/>.
        /// <typeparamref name="TEnum"/> must be an enumerator. <paramref name="value"/> can be:
        ///  - A string value equal to one of the <typeparamref name="TEnum"/>'s item (e.g. "Sunday" or "sUndAy" while <typeparamref name="TEnum"/> is <see cref="DayOfWeek"/>, returns DayOfWeek.Sunday).
        ///  - An integer value equal to one of the <typeparamref name="TEnum"/>'s item (e.g. 0 while <typeparamref name="TEnum"/> is <see cref="DayOfWeek"/>, returns DayOfWeek.Sunday).
        ///  - A comma separated string value, if <typeparamref name="TEnum"/> is decorated by <see cref="FlagsAttribute"/> (e.g. "None, RemoveEmptyEntries" or "noNE,removeemptyentries" while <typeparamref name="TEnum"/> is <see cref="StringSplitOptions"/>, returns StringSplitOptions.None|StringSplitOptions.RemoveEmptyEntries)
        /// </summary>
        /// <typeparam name="TEnum">An enum type.</typeparam>
        /// <param name="value">A string or integer value which has to convert to one of <typeparamref name="TEnum"/>'s item.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Equivalent <typeparamref name="TEnum"/> item for <paramref name="value"/>.</returns>
        public static TEnum TryParse<TEnum>(object value, TEnum defaultValue = default(TEnum))
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum argument must be an enumeraor.", "TEnum");
            }

            TEnum result = defaultValue;

            if (value != null)
            {
                try
                {
                    result = Parse<TEnum>(value);
                }
                catch (Exception exception)
                {
                    //TODO: Get calling method, not this method....
                    //LogManager.Log(
                    //        logLevel: LogLevel.Warning,
                    //        exception: exception,
                    //        message: string.Format("Cannot convert '{0}' to a valid value of enum '{1}'.", Convert.ToString(value), typeof(TEnum).FullName),
                    //        methodName: System.Reflection.MethodBase.GetCurrentMethod().Name,
                    //        className: "EnumHelper"
                    //);
                }
            }

            return result;
        }

        /// <summary>
        /// Tries to parse <paramref name="value"/> to a valid member of enum <paramref name="enumType"/>.
        /// </summary>
        /// <returns>A valid member of enum <paramref name="enumType"/>.</returns>
        public static object TryParse(object value, Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("TEnum argument must be an enumeraor.", "TEnum");
            }

            object result = Activator.CreateInstance(enumType);

            try
            {
                int intValue;
                string strValue;

                if (value == null || value == DBNull.Value)
                {
                    strValue = "0";
                }
                else
                {
                    strValue = value.ToString();
                }

                if (int.TryParse(strValue, out intValue))
                {
                    result = Enum.ToObject(enumType, intValue);
                }
                else
                {
                    result = Enum.Parse(enumType, strValue, true);
                }

                //Check: 
                if (Enum.IsDefined(enumType, result) || result.ToString().Contains(","))
                {
                    //Parameter "value" has been succefully converted to an enumType value.
                    //Everything is fine!
                }
                else
                {
                    throw new Exception(
                        $"\"{value}\" is not an underlying value of the {enumType.FullName} enumeration.");
                }
            }
            catch (Exception exception)
            {
                //LogManager.Log(
                //    logLevel: LogLevel.Error,
                //    exception: exception,
                //    methodName: System.Reflection.MethodBase.GetCurrentMethod().Name,
                //    className: System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name
                //);
            }

            return result;
        }
    }
}
