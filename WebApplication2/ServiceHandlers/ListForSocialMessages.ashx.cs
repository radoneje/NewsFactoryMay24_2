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
    public class ListForSocialMessages : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                
                    context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(
                       dc.tSocial_feedToMessage.Where(s => s.status < 2 && s.publishCount < 10 && s.feedId == RequestContext.RouteData.Values["feedId"])
                       .OrderBy(ss=>ss.publishCount)
                       .OrderByDescending(sss=>sss.insertDate)
                        ));
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
    public class ListForSocialMessagesRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ListForSocialMessages() { RequestContext = requestContext }; ;
        }
    }
}