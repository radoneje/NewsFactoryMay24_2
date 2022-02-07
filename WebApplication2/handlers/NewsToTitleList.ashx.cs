using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Web.Routing;


namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for NewsToTitleList
    /// </summary>
    public class NewsToTitleList : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var NewsId = Convert.ToInt64(RequestContext.RouteData.Values["newsId"]);
            var titles = handlers.NewsToXLS.GetTitles(Convert.ToInt64(RequestContext.RouteData.Values["newsId"]));
            foreach(var a in titles)
            {
                context.Response.Write(a.Title + "\t" + a.Cap + "\r\n");
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
    public class NewsToTitleListRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new NewsToTitleList() { RequestContext = requestContext }; ;
        }
    }
}