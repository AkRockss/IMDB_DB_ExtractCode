using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace IMDBImport
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection sqlConn = new SqlConnection("Server=localhost;Database=IMDBDB;User Id=IMDB_Inserter;Password=123456789;");
            sqlConn.Open();

            //TITLEBASIC
            List<TitleBasic> allTitles = ReadAllTitlesBasic(@"C:\Users\Sande\Desktop\Datamatiker\4. semester\Database\TSV\title.basics\data.tsv", 900000);
            Console.WriteLine("Read " + allTitles.Count + " titles from file"); 

            //NAMEBASIC
            List<NameBasic> allTitles2 = ReadAllNamesBasic(@"C:\Users\Sande\Desktop\Datamatiker\4. semester\Database\TSV\name.basics\data.tsv", 900000);
            Console.WriteLine("Read " + allTitles2.Count + " titles from file");

            //TITLEAKAS
            List<TitleAkas> allTitles3 = ReadAllTitlesAkas(@"C:\Users\Sande\Desktop\Datamatiker\4. semester\Database\TSV\title.akas\data.tsv", 900000);
            Console.WriteLine("Read " + allTitles3.Count + " titles from file");
            
            //TITLECREW
            List<TitleCrew> allTitles4 = ReadAllTitlesCrew(@"C:\Users\Sande\Desktop\Datamatiker\4. semester\Database\TSV\title.crew\data.tsv", 900000);
            Console.WriteLine("Read " + allTitles4.Count + " titles from file");

            //TITLEPRINCIPALS
            List<TitlePrincipals> allTitles5 = ReadAllTitlesPrincipals(@"C:\Users\Sande\Desktop\Datamatiker\4. semester\Database\TSV\title.principals\data.tsv", 900000);
            Console.WriteLine("Read " + allTitles5.Count + " titles from file");


            bool readInput = true;
            while (readInput)
            {
                IInsert inserter = null;
                IInsert2 inserter2 = null;
                IInsert3 inserter3 = null;
                IInsert4 inserter4 = null;
                IInsert5 inserter5 = null;

                Console.WriteLine("Write 1 for TitleBasic Bulk insert, " +
                    "2 for NameBasic Bulk insert, " +
                    "3 for TitleAkas Bulk insert, " +
                    "4 for TitleCrew Bulk insert, " +
                    "5 for TitlePrincipals Bulk insert");
              
                Console.WriteLine("6 for clearing TitleBasicDB, " +
                    "7 for clearing NameBasicDB, " +
                    "8 for clearing TitleAkasDB, " +
                    "9 for clearing TitleCrewDB, " +
                    "10 for clearing TitlePrincipals,  " +
                    "11 for end");
                string input = Console.ReadLine();
                int inputNumber = int.Parse(input);
                switch (inputNumber)
                {
                    case 1:
                        inserter = new BulkInsertTitleBasic();
                        break;
                    case 2:
                        inserter2 = new BulkInsertNameBasic();
                        break;
                    case 3:
                        inserter3 = new BulkInsertTitleAkas();
                        break;
                    case 4:
                        inserter4 = new BulkInsertTitleCrew();
                        break;
                    case 5:
                        inserter5 = new BulkInsertTitlePrincipals();
                        break;
                    case 6:
                        Console.WriteLine("Deleting all rows TitleBasic");
                        DeleteAllRowsTitleBasic(sqlConn);
                        Console.WriteLine("All rows deleted");
                        break;

                    case 7:
                        Console.WriteLine("Deleting all rows NameBasic");
                        DeleteAllRowsNameBasic(sqlConn);
                        Console.WriteLine("All rows deleted");
                        break;

                    case 8:
                        Console.WriteLine("Deleting all rows TitleAkas");
                        DeleteAllRowsTitleAkas(sqlConn);
                        Console.WriteLine("All rows deleted");
                        break;

                    case 9:
                        Console.WriteLine("Deleting all rows TitleCrew");
                        DeleteAllRowsTitleCrew(sqlConn);
                        Console.WriteLine("All rows deleted");
                        break;

                    case 10:
                        Console.WriteLine("Deleting all rows TitlePrincipals");
                        DeleteAllRowsTitlePrincipals(sqlConn);
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

        public static void DeleteAllRowsTitleBasic(SqlConnection sqlConn)
        {
            SqlCommand sqlComm = new SqlCommand("DELETE FROM Title_Basic", sqlConn);
            sqlComm.CommandTimeout = 0;
            sqlComm.ExecuteNonQuery();
        }

        public static void DeleteAllRowsNameBasic(SqlConnection sqlConn)
        {
            SqlCommand sqlComm = new SqlCommand("DELETE FROM Name_Basic", sqlConn);
            sqlComm.CommandTimeout = 0;
            sqlComm.ExecuteNonQuery();
        }
        
        public static void DeleteAllRowsTitleAkas(SqlConnection sqlConn)
        {
            SqlCommand sqlComm = new SqlCommand("DELETE FROM Title_Akas", sqlConn);
            sqlComm.CommandTimeout = 0;
            sqlComm.ExecuteNonQuery();
        }

        public static void DeleteAllRowsTitleCrew(SqlConnection sqlConn)
        {
            SqlCommand sqlComm = new SqlCommand("DELETE FROM Title_Crew", sqlConn);
            sqlComm.CommandTimeout = 0;
            sqlComm.ExecuteNonQuery();
        }

        public static void DeleteAllRowsTitlePrincipals(SqlConnection sqlConn)
        {
            SqlCommand sqlComm = new SqlCommand("DELETE FROM Title_Principals", sqlConn);
            sqlComm.CommandTimeout = 0;
            sqlComm.ExecuteNonQuery();
        }

        //READ ALL TITLE BASICS
        public static List<TitleBasic> ReadAllTitlesBasic(string filePath, int maxRows)
        {
            List<TitleBasic> allTitles = new List<TitleBasic>();
            int counter = 0;

            foreach (string line in System.IO.File.ReadLines(@"C:\Users\Sande\Desktop\Datamatiker\4. semester\Database\TSV\title.basics\data.tsv"))
            {
                //ignore first line with columnnames
                if (counter != 0)
                {
                    string[] splitLine = line.Split("\t");
                    if (splitLine.Length == 9)
                    {
                        allTitles.Add(new TitleBasic(splitLine));
                    }
                }

                if (maxRows != 0 && counter++ >= maxRows)
                {
                    break;
                }
            }
            return allTitles;
        }


        //READ ALL NAME BASICS
        public static List<NameBasic> ReadAllNamesBasic(string filePath, int maxRows)
        {
            List<NameBasic> allTitles2 = new List<NameBasic>();
            int counter = 0;

            foreach (string line in System.IO.File.ReadLines(@"C:\Users\Sande\Desktop\Datamatiker\4. semester\Database\TSV\name.basics\data.tsv"))
            {
                //ignore first line with columnnames
                if (counter != 0)
                {
                    string[] splitLine = line.Split("\t");
                    if (splitLine.Length == 9)
                    {
                        allTitles2.Add(new NameBasic(splitLine));
                    }
                }

                if (maxRows != 0 && counter++ >= maxRows)
                {
                    break;
                }
            }
            return allTitles2;
        }

        //READ ALL TITLE AKAS
        public static List<TitleAkas> ReadAllTitlesAkas(string filePath, int maxRows)
        {
            List<TitleAkas> allTitles3 = new List<TitleAkas>();
            int counter = 0;

            foreach (string line in System.IO.File.ReadLines(@"C:\Users\Sande\Desktop\Datamatiker\4. semester\Database\TSV\title.akas\data.tsv"))
            {
                //ignore first line with columnnames
                if (counter != 0)
                {
                    string[] splitLine = line.Split("\t");
                    if (splitLine.Length == 9)
                    {
                        allTitles3.Add(new TitleAkas(splitLine));
                    }
                }

                if (maxRows != 0 && counter++ >= maxRows)
                {
                    break;
                }
            }
            return allTitles3;
        }

        //READ ALL TITLE CREW
        public static List<TitleCrew> ReadAllTitlesCrew(string filePath, int maxRows)
        {
            List<TitleCrew> allTitles4 = new List<TitleCrew>();
            int counter = 0;

            foreach (string line in System.IO.File.ReadLines(@"C:\Users\Sande\Desktop\Datamatiker\4. semester\Database\TSV\title.crew\data.tsv"))
            {
                //ignore first line with columnnames
                if (counter != 0)
                {
                    string[] splitLine = line.Split("\t");
                    if (splitLine.Length == 9)
                    {
                        allTitles4.Add(new TitleCrew(splitLine));
                    }
                }

                if (maxRows != 0 && counter++ >= maxRows)
                {
                    break;
                }
            }
            return allTitles4;
        }

        //READ ALL TITLE CREW
        public static List<TitlePrincipals> ReadAllTitlesPrincipals(string filePath, int maxRows)
        {
            List<TitlePrincipals> allTitles5 = new List<TitlePrincipals>();
            int counter = 0;

            foreach (string line in System.IO.File.ReadLines(@"C:\Users\Sande\Desktop\Datamatiker\4. semester\Database\TSV\title.principals\data.tsv"))
            {
                //ignore first line with columnnames
                if (counter != 0)
                {
                    string[] splitLine = line.Split("\t");
                    if (splitLine.Length == 9)
                    {
                        allTitles5.Add(new TitlePrincipals(splitLine));
                    }
                }

                if (maxRows != 0 && counter++ >= maxRows)
                {
                    break;
                }
            }
            return allTitles5;
        }

    }
}

