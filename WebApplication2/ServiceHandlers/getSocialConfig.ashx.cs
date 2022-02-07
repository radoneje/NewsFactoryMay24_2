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
    public class getSocialConfig : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(dc.tSocial_Feeds.Where(s => s.deleted == false && s.typeId == RequestContext.RouteData.Values["typeId"])));
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
    public class getSocialConfigRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new getSocialConfig() { RequestContext = requestContext }; ;
        }
    }
}