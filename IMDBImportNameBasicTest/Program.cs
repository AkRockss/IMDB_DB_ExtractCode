using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace IMDBImportNameBasicTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection sqlConn = new SqlConnection("Server=localhost;Database=IMDBDB;User Id=IMDB_Inserter;Password=123456789;");
            sqlConn.Open();

            List<NameBasic> allTitles = ReadAllNamesBasic(@"C:\TSV\name.basics\data.tsv", 55000);
            Console.WriteLine("Read " + allTitles.Count + " titles from file");


            bool readInput = true;
            while (readInput)
            {

                IInsert inserter = null;


                Console.WriteLine("Write 1 for TitleBasic Bulk insert, " +
                 "2 for NameBasic Bulk insert");

                Console.WriteLine("6 for clearing TitleBasicDB, " +
                    "7 for clearing NameBasicDB");

                string input = Console.ReadLine();
                int inputNumber = int.Parse(input);
                switch (inputNumber)
                {

                    case 2:
                        inserter = new BulkInsertNameBasic();
                        break;


                    case 7:
                        Console.WriteLine("Deleting all rows NameBasic");
                        DeleteAllRowsNameBasic(sqlConn);
                        Console.WriteLine("All rows deleted");
                        break;

                    case 11:
                        readInput = false;
                        break;
                }


                if (inserter != null)
                {
                    DateTime startTime = DateTime.Now;
                    Console.WriteLine("Inserts begin at:" + startTime.ToString());
                    inserter.InsertData(sqlConn, allTitles);
                    DateTime endTime = DateTime.Now;
                    Console.WriteLine("Insert time: " + endTime.Subtract(startTime).TotalSeconds);
                    Console.WriteLine("Inserts end at:" + endTime.ToString());
                }
            }
            sqlConn.Close();

        }

        public static void DeleteAllRowsNameBasic(SqlConnection sqlConn)
        {
            SqlCommand sqlComm = new SqlCommand("DELETE FROM Name_Basic", sqlConn);
            sqlComm.CommandTimeout = 0;
            sqlComm.ExecuteNonQuery();
        }


              
        public static List<NameBasic> ReadAllNamesBasic(string filePath, int maxRows)
        {
            List<NameBasic> allTitles = new List<NameBasic>();
            int counter = 0;

            foreach (string line in System.IO.File.ReadLines(@"C:\TSV\name.basics\data.tsv"))
            {
                //ignore first line with columnnames
                if (counter != 0)
                {
                    string[] splitLine = line.Split("\t");
                    if (splitLine.Length == 9)
                    {
                        allTitles.Add(new NameBasic(splitLine));
                    }
                }

                if (maxRows != 0 && counter++ >= maxRows)
                {
                    break;
                }
            }
            return allTitles;
        }

    }
}

