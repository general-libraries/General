namespace General.Collections
{
    /// <summary>
    /// Defines necessary members for any inherited class from <see cref="IComparisonData"/>.
    /// </summary>
    public interface IComparisonData
    {
        /// <summary>
        /// Gets or sets Comparison Rule.
        /// </summary>
        ComparisonRule ComparisonRule { get; set; }

        /// <summary>
        /// Gets or sets a number which might be used by <see cref="ComparisonRule"/>.
        /// </summary>
        int X { get; set; }
    }
}
