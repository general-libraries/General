namespace General.Collections
{
    /// <summary>
    /// Specifies different comparison rule for comparing two lists.
    /// </summary>
    public enum ComparisonRule
    {
        /// <summary>
        /// Lists have no item in common.
        /// </summary>
        NoneOf = 0,

        /// <summary>
        /// Lists have exactly the same items.
        /// </summary>
        ExactlyTheSame = 1,

        /// <summary>
        /// First list contains all of the items of the second list.
        /// </summary>
        AllOf = 2,

        /// <summary>
        /// First list cotains at list one item that the second list also has it.
        /// </summary>
        AnyOf = 3,

        /// <summary>
        /// First list contains exactly X items of the second list. 
        /// X is defined in <see cref="ComparableCollectionBase&lt;T&gt;"/>.
        /// </summary>
        ExactlyXof = 4,

        /// <summary>
        /// First list contains more than X items of the second list. 
        /// X is defined in <see cref="ComparableCollectionBase&lt;T&gt;"/>.
        /// </summary>
        MoreThanXof = 5,

        /// <summary>
        /// First list contains less than X items of the second list. 
        /// X is defined in <see cref="ComparableCollectionBase&lt;T&gt;"/>.
        /// </summary>
        LessThanXof = 6
    }
}
