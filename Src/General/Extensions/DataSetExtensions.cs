using System;
using System.Data;

namespace General.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="DataSet"/> type.
    /// </summary>
    public static class DataSetExtensions
    {
        /// <summary>
        /// Gets XML string of this <see cref="DataSet"/> including cells with <c>null</c> value in the first row.
        /// </summary>
        /// <param name="thisDataSet">Current DataSet.</param>
        /// <returns>A XML string including cells with <c>null</c> value in the first row.</returns>
        public static string GetXmlIncludingNull(this DataSet thisDataSet)
        {
            DataSet dataSet = thisDataSet.Copy();

            foreach (DataTable dataTable in dataSet.Tables)
            {
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataColumn dataColumn in dataTable.Columns)
                    {
                        //check if none of the rows has a value for the column
                        if (dataTable.Select($"{dataColumn.ColumnName} is not null").Length == 0)
                        {
                            // if non of the rows of a "dataColumn" doesn't have value,
                            //   find the best default value for that column (not null!)
                            //   and assign in to the first row.
                            // it will force .GetXml() function to generate that field in the first node of XML.

                            Type type = dataColumn.DataType;
                            object defaultValue;

                            if (type.IsValueType)
                            {
                                defaultValue = Activator.CreateInstance(type);
                            }
                            else
                            {
                                dataTable.Columns[dataColumn.ColumnName].DataType = typeof(string);
                                defaultValue = string.Empty;
                            }

                            dataTable.Rows[0][dataColumn.ColumnName] = defaultValue;
                        }
                    }
                }
            }

            dataSet.AcceptChanges();
            return dataSet.GetXml();
        }
    }

}
