using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.API
{
    /// <summary>
    /// Summary description for ProgrammsListGet
    /// </summary>
    public class ProgrammsListGet : IHttpHandler
    {
        public struct programs
        {
            public long id;
            public string name;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            List<programs> progrs = new List<programs>();
            using (WebApplication2.News.NewsDataContext dc = new WebApplication2.News.NewsDataContext())
            {


                dc.Programs.Where(p => p.Deleted == false && p.id>0).OrderBy(pp=>pp.Name).ToList().ForEach(l =>
                {
                    progrs.Add(new programs()
                    {
                        id = l.id,
                        name = l.Name
                    });
                   
                });
            }
            context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { items = progrs }));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}