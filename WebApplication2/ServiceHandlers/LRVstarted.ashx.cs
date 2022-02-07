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
    public class LRVstarted : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {


            if ((RequestContext.RouteData.Values["id"]) == null)
            { context.Response.Write("error"); return; }
            try
            {
                lock (RequestContext.HttpContext.Application["EncodeTaskLook"])
                {
                    var id = (string)RequestContext.RouteData.Values["id"];
                    using (Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
                    {
                        dc.tWeb_MediaTasks.Where(t => t.id == Convert.ToInt32(id)).First().LRVStatus = 1;
                        dc.SubmitChanges();
                    }
                    context.Response.Write(id);
                }
            }
            catch { }
            context.Response.Write("-1");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class LRVstartedRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new LRVstarted() { RequestContext = requestContext }; ;
        }
    }
}