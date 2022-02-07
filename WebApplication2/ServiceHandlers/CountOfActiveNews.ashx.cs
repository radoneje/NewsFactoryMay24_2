using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebApplication2.ServiceHandlers
{
    /// <summary>
    /// Summary description for getServiceConfig
    /// </summary>
    public class CountOfActiveNews : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            using(var dc= new News.NewsDataContext())
            {
                var count = dc.News.Where(r => r.Deleted == false).Count();

                 
                context.Response.ContentType = "text/plain";
                context.Response.Write(count.ToString());
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
    public class CountOfActiveNewsRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new CountOfActiveNews() { RequestContext = requestContext }; ;
        }
    }
}