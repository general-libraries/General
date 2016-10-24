using Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisListOfT"></param>
        /// <param name="comparingRule"></param>
        /// <param name="listOfT"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        public static bool Has<T>(this IEnumerable<T> thisListOfT, ComparisonRule comparingRule, IEnumerable<T> listOfT, int X = 0)
        {
            bool result = false;

            switch (comparingRule)
            {
                case ComparisonRule.AllOf:
                    result = listOfT.All(t => thisListOfT.Any(thisT => thisT.Equals(t)));
                    break;

                case ComparisonRule.AnyOf:
                    result = thisListOfT.Any(thisT => listOfT.Any(t => thisT.Equals(t)));
                    break;

                case ComparisonRule.NoneOf:
                    result = thisListOfT.All(thisT => listOfT.All(t => !thisT.Equals(t)));
                    break;

                case ComparisonRule.ExactlyTheSame:
                    result =
                        thisListOfT.Count() == listOfT.Count()
                        &&
                        thisListOfT.All(thisT => listOfT.All(t => thisT.Equals(t)));
                    break;

                case ComparisonRule.ExactlyXof:
                    result = thisListOfT.CounEqualItems(listOfT) == X;
                    break;

                case ComparisonRule.MoreThanXof:
                    result = thisListOfT.CounEqualItems(listOfT) > X;
                    break;

                case ComparisonRule.LessThanXof:
                    result = thisListOfT.CounEqualItems(listOfT) < X;
                    break;

                default:
                    throw new Exception("ComparisonRule is not valid!");
            }


            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisListOfT"></param>
        /// <param name="comparingRule"></param>
        /// <param name="listOfT"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        public static bool BelongsTo<T>(this IEnumerable<T> thisListOfT, ComparisonRule comparingRule, IEnumerable<T> listOfT, int X = 0)
        {
            if (thisListOfT == null && listOfT == null)
            {
                return true;
            }

            if (thisListOfT == null || listOfT == null)
            {
                return false;
            }

            bool result = false;

            switch (comparingRule)
            {
                case ComparisonRule.AllOf:
                    result = thisListOfT.All(thisT => listOfT.Any(t => thisT.Equals(t)));
                    break;

                case ComparisonRule.AnyOf:
                    result = thisListOfT.Any(thisT => listOfT.Any(t => thisT.Equals(t)));
                    break;

                case ComparisonRule.NoneOf:
                    result = thisListOfT.All(thisT => listOfT.All(t => !thisT.Equals(t)));
                    break;

                case ComparisonRule.ExactlyTheSame:
                    result =
                        thisListOfT.Count() == listOfT.Count()
                        &&
                        thisListOfT.All(thisT => listOfT.All(t => thisT.Equals(t)));
                    break;

                case ComparisonRule.ExactlyXof:
                    result = thisListOfT.CounEqualItems(listOfT) == X;
                    break;

                case ComparisonRule.MoreThanXof:
                    result = thisListOfT.CounEqualItems(listOfT) > X;
                    break;

                case ComparisonRule.LessThanXof:
                    result = thisListOfT.CounEqualItems(listOfT) < X;
                    break;

                default:
                    throw new Exception("ComparisonRule is not valid!");
            }


            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisListOfT"></param>
        /// <param name="listOfT"></param>
        /// <returns></returns>
        public static int CounEqualItems<T>(this IEnumerable<T> thisListOfT, IEnumerable<T> listOfT)
        {
            int result = 0;

            foreach (T thisT in thisListOfT)
            {
                foreach (T t in listOfT)
                {
                    if (thisT.Equals(t))
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Executes an <paramref name="action"/> on each of <paramref name="source"/>'s items.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">Any IEnumerable{T}.</param>
        /// <param name="action">An action to be executed on each item of <paramref name="source"/>.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (T item in source)
            {
                action(item);
            }
        }

        public static IEnumerable<T> DistinctByCriteria<T>(this IEnumerable<T> headers, Func<T, string> groupCriteria, Func<IEnumerable<T>, T> getSingleItem)
        {

            var distinctItems = new List<T>();

            headers.GroupBy(groupCriteria).ForEach(group => distinctItems.Add(getSingleItem(group)));


            return distinctItems;
        }



    }
}
