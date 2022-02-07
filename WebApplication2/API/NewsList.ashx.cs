using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.API
{
    /// <summary>
    /// Сводное описание для NewsList
    /// </summary>
    public class NewsList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            if (context.Request.Params["programmID"] == null)
            {
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { items = new { }, status = -1, message = "need programmID pearab" }));
                return;
            }
            using (WebApplication2.News.NewsDataContext dc = new WebApplication2.News.NewsDataContext())
            {
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(
                    dc.News.Where(r => r.ProgramId == Convert.ToInt32(context.Request.Params["programmID"]) &&
                        r.Deleted == false).OrderByDescending(r => r.NewsDate).Select(r => new { r.id, r.Name, r.NewsDate })
                    ));
            }
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