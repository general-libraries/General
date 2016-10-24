using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
    /// <summary>
    /// Encapsulates extentions methods for <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns the Type of T, if <paramref name="type"/> is of type <see cref="IEnumerable{T}"/>; otherwise, returns null.
        /// </summary>
        /// <param name="type">Any type that inherits from <see cref="IEnumerable{T}"/> inteface.</param>
        /// <returns>The Type of T, if <paramref name="type"/> is of type <see cref="IEnumerable{T}"/>; otherwise, returns null.</returns>
        /// <remarks>
        /// Got it from: http://stackoverflow.com/a/1846690/538387
        /// </remarks>
        public static Type GetEnumerableType(this Type type)
        {
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType
                    && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return interfaceType.GetGenericArguments()[0];
                }
            }

            return null;
        }
    }
}
