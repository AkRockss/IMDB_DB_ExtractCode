using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBImport
{
    class TitlePrincipals
    {
        public string tconst { get; set; }
        public string ordering { get; set; }
        public string nconst { get; set; }
        public string catergory { get; set; }
        public string job { get; set; }
        public string characters { get; set; }

        //public bool? isAdultNull
        //{
        //    get
        //    {
        //        if (isAdult.ToLower() == "\\n")
        //        {
        //            return null;
        //        }
        //        return isAdult == "1";
        //    }
        //}
        //public int? startYearNull
        //{
        //    get
        //    {
        //        if (startYear.ToLower() == "\\n")
        //        {
        //            return null;
        //        }
        //        return int.Parse(startYear);
        //    }
        //}
        //public int? endYearNull
        //{
        //    get
        //    {
        //        if (endYear.ToLower() == "\\n")
        //        {
        //            return null;
        //        }
        //        try
        //        {
        //            return int.Parse(endYear);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("endyear is: " + endYear);
        //            throw ex;
        //        }
        //    }
        //}
        //public int? runTimeMinutesNull
        //{
        //    get
        //    {
        //        if (runTimeMinutes.ToLower() == "\\n")
        //        {
        //            return null;
        //        }
        //        return int.Parse(runTimeMinutes);
        //    }
        //}


        public TitlePrincipals()
        {
        }

        public TitlePrincipals(string[] splitLine)
        {
            tconst = splitLine[0];
            ordering = splitLine[1];
            nconst = splitLine[2];
            catergory = splitLine[3];
            job = splitLine[4];
            characters = splitLine[5];

        }
    }
}

