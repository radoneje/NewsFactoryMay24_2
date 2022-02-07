using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;


namespace mobile
{
    /// <summary>
    /// Summary description for mobileService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]

    public class mobileService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string HelloWorld(long id)
        {
            return "Hello World"+id;
        }

        [WebMethod(EnableSession = true)]
        public string getComment(long id)
        {
            using (var dc = new MainDataClassesDataContext())
            {
                return dc.Blocks.Where(b => b.Id == id).First().Description;
            }
        }
        
            [WebMethod(EnableSession = true)]
        public string saveComment(long id, string text)
        {
            using (var dc = new MainDataClassesDataContext())
            {
                 dc.Blocks.Where(b => b.Id == id).First().Description=text;
                 dc.SubmitChanges();
            }
            return System.Web.Helpers.Json.Encode(new { status=1});
        }
    }
}
