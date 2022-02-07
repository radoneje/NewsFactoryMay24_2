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
    public class SessionsHandeler : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {

            var ret=(int)context.Application["Sessions"];
            context.Response.ContentType = "text/plain";
            context.Response.Write(ret);

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class SessionsHandelerRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SessionsHandeler() { RequestContext = requestContext }; ;
        }
    }
}