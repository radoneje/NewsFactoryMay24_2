using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebApplication2.NFSocket
{
    /// <summary>
    /// Summary description for NFSocketResponce
    /// </summary>
    public class NFSocketResponce : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            if (RequestContext.HttpContext.Session["UserId"] == null)
            {
                context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { msg = "no auth" }));
                return;
            }
            var sr = new System.IO.StreamReader(context.Request.InputStream);
            var json = System.Web.Helpers.Json.Decode(sr.ReadToEnd());
            for (var i = 0; i < json.Length; i++)
            {
                var sock = (NFSocket.CSocket)(context.Application["sock"]);
                sock.commandComplite(RequestContext.HttpContext.Session.SessionID, json[i]);
            }
                context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { msg = "ok" }));

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class NFSocketResponceRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new NFSocketResponce() { RequestContext = requestContext }; ;
        }
    }
}