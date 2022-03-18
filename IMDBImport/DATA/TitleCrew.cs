using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBImport
{
    class TitleCrew
    {
        public string tconst { get; set; }
        public string directors { get; set; }
        public string writers { get; set; }

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
        
        public int? directorsNull
        {
            get
            {
                if (directors.ToLower() == "\\n")
                {
                    return null;
                }
                return int.Parse(directors);
            }
        }

        public int? writersNull
        {
            get
            {
                if (writers.ToLower() == "\\n")
                {
                    return null;
                }
                return int.Parse(writers);
            }
        }


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


        public TitleCrew()
        {
        }

        public TitleCrew(string[] splitLine)
        {
            tconst = splitLine[0];
            directors = splitLine[1];
            writers = splitLine[2];
     


        }
    }
}
