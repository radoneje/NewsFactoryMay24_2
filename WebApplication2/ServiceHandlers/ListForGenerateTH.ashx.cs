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
    public class ListForGenerateTH : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            using (Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
            {
                lock (RequestContext.HttpContext.Application["EncodeTaskLook"])
                {
                    if (dc.vMedia_ListForGenerateThs.Count() > 0)
                        context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = "ok", items = dc.vMedia_ListForGenerateThs.First() }));
                    else
                    {
                        context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = "-1" }));
                        context.Response.Flush();
                        context.Response.End();
                    }
                }
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
    public class ListForGenerateTHRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ListForGenerateTH() { RequestContext = requestContext }; ;
        }
    }
}