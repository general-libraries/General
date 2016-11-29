using System;
using System.Collections.Generic;
using System.Linq;

namespace General.Extensions
{
    /// <summary>
    /// Encapsulates extentions methods for <see cref="Array"/> class.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Converts this <paramref name="array"/> to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of arrya.</typeparam>
        /// <param name="array"></param>
        /// <returns>Array.</returns>
        public static T ToArray<T>(this Array array)
        // NOTE: should we say:  where T : IList
        {
            T result = default(T);
            Type typeOfT = typeof(T);
            Type typeOfElement = array.GetType().GetElementType();

            //TODO: check if typeOfElement is not an abstarct or interface or... ???

            int arrayRank = array.Rank;
            int[] arrayLengths = new int[arrayRank];

            if (arrayRank == 1)
            {
                arrayLengths[0] = array.Length;
            }
            else if (arrayRank > 1)
            {
                for (int dimension = 0; dimension < arrayRank; dimension++)
                {
                    arrayLengths[dimension] = array.GetUpperBound(dimension);
                }

                throw new Exception("This method still does not support multi-dimention arrays.");
            }

            var newAesult = Array.CreateInstance(typeOfElement, arrayLengths[0]);

            for (int index = 0; index < arrayLengths[0]; index++)
            {
                //TODO: develope this:
                //newAesult.SetValue(array.GetValue(index).ToType(typeOfElement), index);
            }

            result = (T)(object)newAesult;

            return result;
        }

        /// <summary>
        /// Converts this <paramref name="array"/> to array of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of arrya.</typeparam>
        /// <param name="array"></param>
        /// <returns>Array of type <typeparamref name="T"/></returns>
        public static T[] ToArrayOf<T>(this Array array)
        {
            T[] result = null;
            Type typeOfT = typeof(T);
            int arrayRank = array.Rank;
            int[] arrayLengths = new int[arrayRank];

            if (arrayRank == 1)
            {
                arrayLengths[0] = array.Length;
            }
            else if (arrayRank > 1)
            {
                for (int dimension = 0; dimension < arrayRank; dimension++)
                {
                    arrayLengths[dimension] = array.GetUpperBound(dimension);
                }

                throw new Exception("This method still does not support multi-dimention arrays.");
            }

            result = (T[])(object)Array.CreateInstance(typeOfT, arrayLengths[0]);

            for (int index = 0; index < arrayLengths[0]; index++)
            {
                result.SetValue(array.GetValue(index).To<T>(), index);
            }

            return result;
        }
    }
}
