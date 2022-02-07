using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.PrintTemplates
{
    public  class Utils
    {
        public static string GetUserName(long UserId)
        {
            PrintTemplatesDataClassesDataContext dc = new PrintTemplatesDataClassesDataContext();
            return dc.fWeb_GetUserName(UserId);
            
        }
        public static string GetTextTimeFromSeconds(long seconds)
        {
            return TimeSpan.FromSeconds(seconds).ToString();
            
        }
    }
}