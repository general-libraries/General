using Common.Extensions;
using System.Collections.Generic;

namespace Common.Collections
{
    /// <summary>
    /// Encapsulates member for ComparableCollectionBase abstarct class.
    /// Inherited classes from this class can represent a collection of <typeparamref name="T"/> which can be compaired with anothe collection of the same type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ComparableCollectionBase<T> : IComparableCollection<T>
    {
        private List<T> items = new List<T>();

        /// <summary>
        /// Gets or sets Comparison Rule.
        /// </summary>
        /// <value>Comparison Rule.</value>
        [System.ComponentModel.DefaultValue(ComparisonRule.AllOf)]
        public ComparisonRule ComparisonRule { get; set; }

        /// <summary>
        /// Gets or set a number which might be used by the associated <see cref="ComparisonRule"/>.
        /// </summary>
        /// <value>A number which might be used by the associated <see cref="ComparisonRule"/>.</value>
        public int X { get; set; }

        /// <summary>
        /// Base Initializer.
        /// </summary>
        public ComparableCollectionBase()
        {
            this.ComparisonRule = ComparisonRule.AllOf;
        }

        /// <summary>
        /// Adds a new series of <typeparamref name="T"/> to the corrent collection.
        /// </summary>
        /// <param name="listOfT"></param>
        /// <returns>Nothing.</returns>
        public void Add(IEnumerable<T> listOfT)
        {
            if (listOfT != null)
            {
                foreach (T t in listOfT) this.Add(t);
            }
        }

        #region IComparableCollection<T>

        /// <summary>
        /// Indexes an object of <typeparamref name="T"/> in the current collection.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>An item of <typeparamref name="T"/>.</returns>
        public T this[int index]
        {
            get { return items[index]; }
        }

        /// <summary>
        /// Indicates if the current collection contains <paramref name="listOfT"/> by using <see cref="ComparisonRule"/>
        /// </summary>
        /// <param name="listOfT"></param>
        /// <returns>true/false</returns>
        public bool Has(IEnumerable<T> listOfT)
        {
            return this.Has<T>(this.ComparisonRule, listOfT, this.X);
        }

        /// <summary>
        /// Indicates if the current collection contains <paramref name="listOfT"/> by using <see cref="ComparisonRule"/>
        /// </summary>
        /// <param name="listOfT"></param>
        /// <returns>true/fasle</returns>
        public bool BelongsTo(IEnumerable<T> listOfT)
        {
            return this.BelongsTo<T>(this.ComparisonRule, listOfT, this.X);
        }

        /// <summary>
        /// Adds a new item of <typeparamref name="T"/> to the corrent collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Nothing.</returns>
        public void Add(T item)
        {
            items.Add(item);
        }

        /// <summary>
        /// Clears the current collection.
        /// </summary>
        /// <returns>Nothing.</returns>
        public void Clear()
        {
            items = new List<T>();
        }

        /// <summary>
        /// Indicates if the corrent collection has <paramref name="item"/>.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true/false</returns>
        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        /// <summary>
        /// Copies items of the corrent collection to <paramref name="array"/> starting at this index: <paramref name="arrayIndex"/>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        /// <returns>Nothing.</returns>
        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Counts items in the current collection.
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// Indicates if the collection is readonly.
        /// </summary>
        /// <value>Always false!</value>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="item">The object to remove from the current collection. The value can be null for reference types.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the collection.</returns>
        public bool Remove(T item)
        {
            return items.Remove(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the List&lt;<typeparamref name="T"/>&gt;.
        /// </summary>
        /// <returns>A List&lt;<typeparamref name="T"/>&gt;.Enumerator for the List&lt;<typeparamref name="T"/>&gt;.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the List&lt;<typeparamref name="T"/>&gt;.
        /// Implementation for <see cref="System.Collections.IEnumerable"/>.
        /// </summary>
        /// <returns>A List&lt;<typeparamref name="T"/>&gt;.Enumerator for the List&lt;<typeparamref name="T"/>&gt;.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #endregion
    }
}
