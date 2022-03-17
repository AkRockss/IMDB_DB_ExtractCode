using System.Collections.Generic;
using System.Data.SqlClient;


namespace IMDBImportNameBasicTest
{

    interface IInsert
    {
        void InsertData(SqlConnection sqlconn, List<NameBasic> allTitles);
    }

}
