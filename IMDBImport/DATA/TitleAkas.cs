using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBImport
{
    class TitleAkas
    {
        public string titleId { get; set; }
        public string ordering { get; set; }
        public string title { get; set; }
        public string region { get; set; }
        public string language { get; set; }
        public string types { get; set; }
        public string attributes { get; set; }
        public string isOriginalTitle { get; set; }


        public bool? isOriginalTitleNull
        {
            get
            {
                if (isOriginalTitle.ToLower() == "\\n")
                {
                    return null;
                }
                return isOriginalTitle == "1";
            }
        }
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


        public TitleAkas()
        {
        }

        public TitleAkas(string[] splitLine)
        {
            titleId = splitLine[0];
            ordering = splitLine[1];
            title = splitLine[2];
            region = splitLine[3];
            language = splitLine[4];
            types = splitLine[5];
            attributes = splitLine[6];
            isOriginalTitle = splitLine[7];
       
        }

    }
}
