using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IMDBImport
{
  
    class BulkInsertTitleCrew : IInsert4
    {
        public enum TableTypes
        {
            tableString, tableRemoveTT /*tableRemoveNM*/
        }

        public void InsertData4(SqlConnection sqlconn, List<TitleCrew> allTitles4)
        {
            DataTable TitleTable = new DataTable("TitleCrew");
            TitleTable.Columns.Add("tconst", typeof(int));
            TitleTable.Columns.Add("directors", typeof(string));
            TitleTable.Columns.Add("writers", typeof(string));
       

            foreach (TitleCrew crew in allTitles4)
            {
                DataRow row = TitleTable.NewRow();
                AddValueToRow(crew.tconst, row, "tconst", TableTypes.tableRemoveTT);
                AddValueToRow(crew.directors, row, "directors", TableTypes.tableString);
                AddValueToRow(crew.writers, row, "writers", TableTypes.tableString);

                TitleTable.Rows.Add(row);
            }
            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlconn, SqlBulkCopyOptions.KeepNulls, null);
            bulkCopy.BulkCopyTimeout = 0;
            // set the destination table name
            bulkCopy.DestinationTableName = "Title_Crew";

            try
            {
                // write the data in the "dataTable"
                bulkCopy.WriteToServer(TitleTable);
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                {
                    string pattern = @"\d+";
                    Match match = Regex.Match(ex.Message.ToString(), pattern);
                    var index = Convert.ToInt32(match.Value) - 1;

                    FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                    var sortedColumns = fi.GetValue(bulkCopy);
                    var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                    FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                    var metadata = itemdata.GetValue(items[index]);

                    var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                    var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                    throw new Exception(String.Format("Column: {0} contains data with a length greater than: {1}", column, length));
                }
                throw ex;
            }
        }

        public static void AddValueToRow(string value, DataRow row, string rowName, TableTypes type)
        {
            if (value.ToLower() == "\\n")
            {
                row[rowName] = DBNull.Value;
            }
            else
            {
                switch (type)
                {
                    case TableTypes.tableString:
                        row[rowName] = value;
                        break;
                    case TableTypes.tableRemoveTT:
                        row[rowName] = value.TrimStart('t');
                        break;
                        //case TableTypes.tableRemoveNM:
                        //    row[rowName] = value.TrimStart('n', 'm');
                        //    break;


                }
            }
        }
    }
}
 