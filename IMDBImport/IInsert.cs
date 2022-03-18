using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace IMDBImport
{
    interface IInsert
    {
        void InsertData(SqlConnection sqlconn, List<TitleBasic> allTitles);
    }

    interface IInsert2
    {
        void InsertData2(SqlConnection sqlconn, List<NameBasic> allTitles2);
    }

    interface IInsert3
    {
        void InsertData3(SqlConnection sqlconn, List<TitleAkas> allTitles3);
    }

    interface IInsert4
    {
        void InsertData4(SqlConnection sqlconn, List<TitleCrew> allTitles4);
    }

    interface IInsert5
    {
        void InsertData5(SqlConnection sqlconn, List<TitlePrincipals> allTitles5);
    }

}
