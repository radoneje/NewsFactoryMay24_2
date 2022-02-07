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
    /// Summary description for NewsToText
    /// </summary>
    public class exitUser : IHttpHandler, System.Web.SessionState.IRequiresSessionState 
    {
        public RequestContext RequestContext { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            RequestContext.HttpContext.Session["UserId"] = null;
            RequestContext.HttpContext.Session["UserName"] = null;
            context.Session.Abandon();
            context.Application["Sessions"] = (int)context.Application["Sessions"] - 1; ;
         //    RequestContext.HttpContext.Session.Clear();
          //  RequestContext.HttpContext.Session.Abandon();
            context.Response.Write("ok");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class exitUserRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new exitUser() { RequestContext = requestContext }; ;
        }
    }
}