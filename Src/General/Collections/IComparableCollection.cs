using System.Collections.Generic;

namespace General.Collections
{
    /// <summary>
    /// Defines necessary members for an inherited ComparableCollection class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IComparableCollection<T> : ICollection<T>, IEnumerable<T>, IComparisonData
    {
        /// <summary>
        /// Indexes an object of <typeparamref name="T"/> in the current collection.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        T this[int index] { get; }

        /// <summary>
        /// Indicates if the current collection contains <paramref name="listOfT"/> by using <see cref="ComparisonRule"/>
        /// </summary>
        /// <param name="listOfT"></param>
        /// <returns></returns>
        bool Has(IEnumerable<T> listOfT);

        /// <summary>
        /// Indicates if the current collection contains <paramref name="listOfT"/> by using <see cref="ComparisonRule"/>
        /// </summary>
        /// <param name="listOfT"></param>
        /// <returns></returns>
        bool BelongsTo(IEnumerable<T> listOfT);
    }
}
