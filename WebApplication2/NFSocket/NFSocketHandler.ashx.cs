using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;


namespace WebApplication2.NFSocket
{
    /// <summary>
    /// Summary description for NFSocketHandler
    /// </summary>
    public class NFSocketHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState 
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
           
            context.Response.ContentType = "application/json";
            if (RequestContext.HttpContext.Session["UserId"] == null)
            {
                context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { msg = "noAuth" }));
                return;
            }
            var sr = new System.IO.StreamReader(context.Request.InputStream);
            var json = System.Web.Helpers.Json.Decode(sr.ReadToEnd());
            var sock = (NFSocket.CSocket)(context.Application["sock"]);
            context.Response.Write(sock.clientPing(RequestContext.HttpContext.Session.SessionID));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class NFSocketHandlerRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new NFSocketHandler() { RequestContext = requestContext }; ;
        }
    }
}