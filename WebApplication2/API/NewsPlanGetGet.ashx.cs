using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.API
{
    /// <summary>
    /// Summary description for NewsPlanGetGet
    /// </summary>
    public class NewsPlanGetGet : IHttpHandler
    {
        public struct newses
        {
            public long id;
            public string name;
            public string date;
        }
        public void ProcessRequest(HttpContext context)
        {
            var prg = new List<newses>();
             using (WebApplication2.News.NewsDataContext dc = new WebApplication2.News.NewsDataContext())
                        {

                            dc.News.Where(n => n.ProgramId == Convert.ToInt32(context.Request.Params["id"]) && n.Deleted == false && n.NewsDate.Date>DateTime.Now.Date).OrderByDescending(nn => nn.NewsDate).ToList().ForEach(l =>
                            {
                                prg.Add(new newses() { 
                                id=l.id,
                                date=l.NewsDate.ToString("dd.MM.yyyy HH:mm:ss"),
                                name=l.Name
                                });
                            });
             }
             context.Response.ContentType = "application/json";
             context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { items = prg }));
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