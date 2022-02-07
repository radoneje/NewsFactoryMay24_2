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
    public class RssAddComplite : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string instr = (new System.IO.StreamReader(RequestContext.HttpContext.Request.InputStream)).ReadToEnd();
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.ExecuteCommand("UPDATE tWeb_RssSources SET LastGetTime=GetDate() WHERE ID=" + instr);
                dc.pWeb_ClearRss();
                NFSocket.SendToAll.SendData("rssUpdate", new { rssId = instr });
            }
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
    public class RssAddCompliteRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new RssAddComplite() { RequestContext = requestContext }; ;
        }
    }
}