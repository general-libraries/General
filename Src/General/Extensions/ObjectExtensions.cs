using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using General.Helpers;

namespace General.Extensions
{
    /// <summary>
    /// Encapsulates extentions methods for <see cref="Object"/> class.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// An extension method to convert this <see cref="Object"/> to an instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to convert to.</typeparam>
        /// <param name="obj">This object.</param>
        /// <returns>An instance of <typeparamref name="T"/>.</returns>
        public static T To<T>(this Object obj)
        {
            T result = default(T);
            Type typeOfT = typeof(T);

            if (obj == null || obj == DBNull.Value)
            {
                return result;
            }

            if (typeOfT.IsEnum)
            {
                result = EnumHelper.Parse<T>(obj);
            }
            else if (typeOfT.IsArray) // NOTE: Do I need this?   || typeof(Array).IsAssignableFrom(typeOfT))
            {
                var objAsArrya = obj as Array;

                if (objAsArrya != null)
                {
                    result = objAsArrya.ToArray<T>();
                }
            }
            else
            {
                switch (typeOfT.FullName)
                {
                    case "System.Int32":
                        Double tempDouble = Double.Parse(obj.ToString(), System.Globalization.NumberStyles.Any);
                        Object tempInt = Convert.ToInt32(tempDouble);
                        result = (T)tempInt;
                        break;

                    default:
                        result = (T)Convert.ChangeType(obj, typeOfT);
                        break;
                }
            }

            return result;
        }
    }
}
