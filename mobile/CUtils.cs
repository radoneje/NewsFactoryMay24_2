using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;

namespace mobile
{

    

    public class CUtils

    { 
    public void TEST(string phoneNumber) 
    {

    }


        public static string RemoveHTMLTags(string inputHTML)
        {
            return System.Text.RegularExpressions.Regex.Replace(inputHTML, @"<[^>]+>|&nbsp;", "").Trim();
        }
        public static dynamic GetAjaxRes(System.IO.Stream st)
        {
            if (st.CanSeek)
                st.Seek(0, System.IO.SeekOrigin.Begin);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            st.CopyTo(ms);
            string Json = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            return System.Web.Helpers.Json.Decode(Json);
        }
        public static bool checkGuid(string Guid, HttpApplicationState application )
        {
            var lst = (List<user>)application["users"];
            if(lst.Count<0)
                return false;
            var usr=lst.Where(u => u.webGuid == Guid);
            if (usr.Count() == 0)
                return false;
            
               
            usr.First().lastTime = DateTime.Now;
            return true;
        }
        public class user
        {
            public int id;
            public string name;
            public string webGuid;
            public DateTime lastTime;

        }
    }
}