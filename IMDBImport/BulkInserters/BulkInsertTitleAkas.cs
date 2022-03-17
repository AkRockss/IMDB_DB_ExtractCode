using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMDBImport
{

    class BulkInsertTitleAkas : IInsert3
    {
        public enum TableTypes
        {
            tableString, tableBool
        }

        public void InsertData3(SqlConnection sqlconn, List<TitleAkas> allTitles3)
        {
            DataTable TitleTable = new DataTable("TitleAkas");
            TitleTable.Columns.Add("titleId", typeof(string));
            TitleTable.Columns.Add("ordering", typeof(string));
            TitleTable.Columns.Add("title", typeof(string));
            TitleTable.Columns.Add("region", typeof(string));
            TitleTable.Columns.Add("language", typeof(string));
            TitleTable.Columns.Add("types", typeof(string));
            TitleTable.Columns.Add("attributes", typeof(string));
            TitleTable.Columns.Add("isOriginalTitle", typeof(bool));

          
            foreach (TitleAkas akas in allTitles3)
            {
                DataRow row = TitleTable.NewRow();
                AddValueToRow(akas.titleId, row, "titleId", TableTypes.tableString);
                AddValueToRow(akas.ordering, row, "ordering", TableTypes.tableString);
                AddValueToRow(akas.title, row, "title", TableTypes.tableString);
                AddValueToRow(akas.region, row, "region", TableTypes.tableString);
                AddValueToRow(akas.language, row, "language", TableTypes.tableString);
                AddValueToRow(akas.types, row, "types", TableTypes.tableString);
                AddValueToRow(akas.attributes, row, "attributes", TableTypes.tableString);
                AddValueToRow(akas.isOriginalTitle, row, "isOriginalTitle", TableTypes.tableBool);

            
                TitleTable.Rows.Add(row);
            }
            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlconn, SqlBulkCopyOptions.KeepNulls, null);
            bulkCopy.BulkCopyTimeout = 0;
            // set the destination table name
            bulkCopy.DestinationTableName = "Title_Akas";

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
                    //case TableTypes.tableInt:
                    //    row[rowName] = int.Parse(value);
                    //    break;
                    case TableTypes.tableBool:
                        row[rowName] = (value == "1"); ;
                        break;
                }
            }
        }
    }
}
