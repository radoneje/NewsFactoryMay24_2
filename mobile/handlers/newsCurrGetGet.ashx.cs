using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mobile.handlers
{
    /// <summary>
    /// Summary description for newsCurrGetGet
    /// </summary>
    public class newsCurrGetGet : IHttpHandler
    {

        public struct news
        {   
            public long id;
            public string name;
            public string date;


        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.CacheControl = "no-cache";

            var prm = CUtils.GetAjaxRes(context.Request.InputStream);
            if (!CUtils.checkGuid(prm.guid, context.Application))
            {
                context.Response.StatusCode = 401;
                context.Response.Write(System.Web.Helpers.Json.Encode(new { state = "not autorize" }));
                return;
            }
            var ret = new List<news>();

            using (MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                long prgId = Convert.ToInt64(prm.id);
                dc.vLite_NewsToLists.Where(n => n.ProgramId == prgId && n.NewsDate.Date == DateTime.Now.Date).ToList().ForEach(l =>
                {
                    ret.Add(new news()
                    {
                        id = l.id,
                        date = l.NewsDate.ToString("dd.MM.yyyy HH:mm"),
                        name = CUtils.RemoveHTMLTags(l.Name)
                    });
                });
            }
            context.Response.Write(System.Web.Helpers.Json.Encode(new { state = "ok", items = ret }));
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