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
    public class RssAdd : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            //  RequestContext.HttpContext.Request.g.Read
            try
            {
                string instr = (new System.IO.StreamReader(RequestContext.HttpContext.Request.InputStream)).ReadToEnd();
                var json = System.Web.Helpers.Json.Decode(instr);

                using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                {

                    dc.pWeb_InsertRssFeed(DateTime.Parse(json.date), json.image, json.title, json.descr, json.linl, json.giud, Convert.ToInt32(json.sourceId));

                }
                context.Response.Write("ok");
            }
            catch {
                context.Response.Write("failed");
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
    public class RssAddRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new RssAdd() { RequestContext = requestContext }; ;
        }
    }
}