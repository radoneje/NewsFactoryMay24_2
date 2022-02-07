using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for checkDB
    /// </summary>
    public class checkDB : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                {
                    var a = dc.Blocks.First().Id;
                }
            }
            catch(Exception ex) {
                context.Response.StatusCode = 506;
                context.Response.ContentType = "text/plain";
                context.Response.Write("Error connect to DB\r\n");
                context.Response.Write(ex.Message);

            }
            context.Response.StatusCode = 200;
            context.Response.ContentType = "text/plain";
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
    public class CheckDbRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new checkDB() { RequestContext = requestContext }; ;
        }
    }
}