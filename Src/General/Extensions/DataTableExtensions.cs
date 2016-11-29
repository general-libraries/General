using System;
using System.Collections.Generic;
using System.Data;

namespace General.Extensions
{
    /// <summary>
    /// Contains extension methods to work on entities.
    /// </summary>
    public static class DataTableExtension
    {
        /// <summary>
        /// Converts all <see cref="DataTable.Rows"/> of a <see cref="DataTable"/> to a List of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <param name="conversion"></param>
        /// <returns>A list of <typeparamref name="T"/>.</returns>
        public static IEnumerable<T> ConvertRowsToList<T>(this DataTable dataTable, Converter<DataRow, T> conversion)
        {
            if (dataTable != null)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    //result.Add(conversion(dr));
                    yield return  conversion(dr);
                }
            }
        }
    }
}
