using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Сводное описание для GetRssSource
    /// </summary>
    public class GetRssSource : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

           
            if (context.Request.Params["mediaId"] == null)
            {
                context.Response.StatusCode = 404;
                context.Response.Write("not found");
                return;
            }
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext()) {
                var fm = dc.tSocial_feedToMessage.Where(m => m.id == context.Request.Params["MediaId"]);
                if (fm.Count() == 0)
                {
                    context.Response.StatusCode = 404;
                    context.Response.Write("not found");
                    return;
                }
            
                context.Response.Write(fm.First().message);
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