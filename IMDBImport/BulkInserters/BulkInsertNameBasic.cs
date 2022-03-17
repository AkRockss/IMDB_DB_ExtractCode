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

    class BulkInsertNameBasic : IInsert2
    {
        public enum TableTypes
        {
            tableString, tableInt
        }

        public void InsertData2(SqlConnection sqlconn, List<NameBasic> allTitles2)
        {
            DataTable TitleTable = new DataTable("NameBasic");
            TitleTable.Columns.Add("nconst", typeof(string));
            TitleTable.Columns.Add("primaryName", typeof(string));
            TitleTable.Columns.Add("birthYear", typeof(int));
            TitleTable.Columns.Add("deathYear", typeof(int));
            TitleTable.Columns.Add("primaryProfession", typeof(string));
            TitleTable.Columns.Add("knownForTitles", typeof(string));
        
       
            foreach (NameBasic name in allTitles2)
            {
                DataRow row = TitleTable.NewRow();
                AddValueToRow(name.nconst, row, "nconst", TableTypes.tableString);
                AddValueToRow(name.primaryName, row, "primaryName", TableTypes.tableString);
                AddValueToRow(name.birthYear, row, "birthYear", TableTypes.tableInt);
                AddValueToRow(name.deathYear, row, "deathYear", TableTypes.tableInt);
                AddValueToRow(name.primaryProfession, row, "primaryProfession", TableTypes.tableString);
                AddValueToRow(name.knownForTitles, row, "knownForTitles", TableTypes.tableString);
               

                TitleTable.Rows.Add(row);
            }
            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlconn, SqlBulkCopyOptions.KeepNulls, null);
            bulkCopy.BulkCopyTimeout = 0;
            // set the destination table name
            bulkCopy.DestinationTableName = "Name_Basic";

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
                    case TableTypes.tableInt:
                        row[rowName] = int.Parse(value);
                        break;
                        //case TableTypes.tableBool:
                        //    row[rowName] = (value == "1"); ;
                        //    break;
                }
            }
        }

       
    }
}
