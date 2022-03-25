using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace IMDBImport
{
    class Program : BulkInsertTitleBasic
    {
        static void Main(string[] args)
        {

            SqlConnection sqlConn = new SqlConnection("Server=192.168.1.12;Database=IMDBDB;User Id=IMDB_Inserter;Password=123456789;");
            sqlConn.Open();

            ////TITLEBASIC
            List<TitleBasic> allTitles = ReadAllTitlesBasic(@"C:\Users\Aleksander K S M\Desktop\Database\TSV\title.basics\data.tsv", 1);
            Console.WriteLine("Read " + allTitles.Count + " titles from file TitlesBasic"); /*8697068*/

            //NAMEBASIC
            List<NameBasic> allTitles2 = ReadAllNamesBasic(@"C:\Users\Aleksander K S M\Desktop\Database\TSV\name.basics\data.tsv", 1);
            Console.WriteLine("Read " + allTitles2.Count + " titles from file NamesBasic"); /*11465753*/

            //TITLEAKAS
            List<TitleAkas> allTitles3 = ReadAllTitlesAkas(@"C:\Users\Aleksander K S M\Desktop\Database\TSV\title.akas\data.tsv", 1);
            Console.WriteLine("Read " + allTitles3.Count + " titles from file TitlesAkas"); /*31377342*/

            //TITLECREW
            List<TitleCrew> allTitles4 = ReadAllTitlesCrew(@"C:\Users\Aleksander K S M\Desktop\Database\TSV\title.crew\data.tsv", 8750731);
            Console.WriteLine("Read " + allTitles4.Count + " titles from file TitlesCrew"); /*8750731*/

            //TITLEPRINCIPALS
            List<TitlePrincipals> allTitles5 = ReadAllTitlesPrincipals(@"C:\Users\Aleksander K S M\Desktop\Database\TSV\title.principals\data.tsv", 1);
            Console.WriteLine("Read " + allTitles5.Count + " titles from file TitlesPrincipals"); /*49264007*/


            bool readInput = true;
            while (readInput)
            {
                IInsert inserter = null;
                IInsert2 inserter2 = null;
                IInsert3 inserter3 = null;
                IInsert4 inserter4 = null;
                IInsert5 inserter5 = null;

                Console.WriteLine();
                Console.WriteLine("::::::::BULKING THE F DATA::::::::");              
                Console.WriteLine("1 for TitleBasic Bulk insert");
                Console.WriteLine("2 for NameBasic Bulk insert");
                Console.WriteLine("3 for TitleAkas Bulk insert");
                Console.WriteLine("4 for TitleCrew Bulk insert");
                Console.WriteLine("5 for TitlePrincipals Bulk insert");
                Console.WriteLine();
                Console.WriteLine("::::::::CLEARING::::::::");
                Console.WriteLine("6 for clearing TitleBasicDB");
                Console.WriteLine("7 for clearing NameBasicDB");
                Console.WriteLine("8 for clearing TitleAkasDB");
                Console.WriteLine("9 for clearing TitleCrewDB");
                Console.WriteLine("10 for clearing TitlePrincipals");
                Console.WriteLine();
                Console.WriteLine("::::::::ENDING::::::::");
                Console.WriteLine("11 for end"); 

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

                if (inserter2 != null)
                {
                    DateTime startTime = DateTime.Now;
                    Console.WriteLine("Inserts begin at:" + startTime.ToString());
                    inserter2.InsertData2(sqlConn, allTitles2);
                    DateTime endTime = DateTime.Now;
                    Console.WriteLine("Insert time: " + endTime.Subtract(startTime).TotalSeconds);
                    Console.WriteLine("Inserts end at:" + endTime.ToString());
                }

                if (inserter3 != null)
                {
                    DateTime startTime = DateTime.Now;
                    Console.WriteLine("Inserts begin at:" + startTime.ToString());
                    inserter3.InsertData3(sqlConn, allTitles3);
                    DateTime endTime = DateTime.Now;
                    Console.WriteLine("Insert time: " + endTime.Subtract(startTime).TotalSeconds);
                    Console.WriteLine("Inserts end at:" + endTime.ToString());
                }

                if (inserter4 != null)
                {
                    DateTime startTime = DateTime.Now;
                    Console.WriteLine("Inserts begin at:" + startTime.ToString());
                    inserter4.InsertData4(sqlConn, allTitles4);
                    DateTime endTime = DateTime.Now;
                    Console.WriteLine("Insert time: " + endTime.Subtract(startTime).TotalSeconds);
                    Console.WriteLine("Inserts end at:" + endTime.ToString());
                }

                if (inserter5 != null)
                {
                    DateTime startTime = DateTime.Now;
                    Console.WriteLine("Inserts begin at:" + startTime.ToString());
                    inserter5.InsertData5(sqlConn, allTitles5);
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

            foreach (string line in System.IO.File.ReadLines(@"C:\Users\Aleksander K S M\Desktop\Database\TSV\title.basics\data.tsv"))
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

            foreach (string line in System.IO.File.ReadLines(@"C:\Users\Aleksander K S M\Desktop\Database\TSV\name.basics\data.tsv"))
            {
                //ignore first line with columnnames
                if (counter != 0)
                {
                    string[] splitLine = line.Split("\t");
                    if (splitLine.Length == 6)
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

            foreach (string line in System.IO.File.ReadLines(@"C:\Users\Aleksander K S M\Desktop\Database\TSV\title.akas\data.tsv"))
            {
                //ignore first line with columnnames
                if (counter != 0)
                {
                    string[] splitLine = line.Split("\t");
                    if (splitLine.Length == 8)
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

            foreach (string line in System.IO.File.ReadLines(@"C:\Users\Aleksander K S M\Desktop\Database\TSV\title.crew\data.tsv"))
            {
                //ignore first line with columnnames
                if (counter != 0)
                {
                    string[] splitLine = line.Split("\t");
                    if (splitLine.Length == 3)
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

            foreach (string line in System.IO.File.ReadLines(@"C:\Users\Aleksander K S M\Desktop\Database\TSV\title.principals\data.tsv"))
            {
                //ignore first line with columnnames
                if (counter != 0)
                {
                    string[] splitLine = line.Split("\t");
                    if (splitLine.Length == 6)
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

