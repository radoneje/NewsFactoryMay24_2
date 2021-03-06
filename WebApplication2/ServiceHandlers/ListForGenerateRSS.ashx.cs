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
    public class ListForGenerateRSS : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                if(dc.tWeb_RssSources.Where(s => s.Active == true).Count()>0)
                    context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(dc.tWeb_RssSources.Where(s => s.Active == true)));
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
    public class ListForGenerateRSSRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ListForGenerateRSS() { RequestContext = requestContext }; ;
        }
    }
}