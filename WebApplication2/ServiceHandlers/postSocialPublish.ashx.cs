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
    public class postSocialPublish : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
           // feedToMessageId
            context.Response.ContentType = "application/json";
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                var rec = dc.tSocial_feedToMessage.Where(m => m.id == RequestContext.RouteData.Values["feedToMessageId"]).First();
                rec.publishCount++;
                rec.updateStatusDate = DateTime.Now;
                if (RequestContext.HttpContext.Request.Params["socialId"].Length > 0)
                {
                    rec.socialId = RequestContext.HttpContext.Request.Params["socialId"];
                    rec.status = 2;
                  
                }
                else
                {
                    rec.socialError = RequestContext.HttpContext.Request.Params["errorMessage"];
                    rec.status = -1;
                }
                dc.SubmitChanges();
                NFSocket.SendToAll.SendData("socialStateChange", new { feedId = rec.tSocial_Feed.id });
            }
       
            context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = "ok" }));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class postSocialPublishRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new postSocialPublish() { RequestContext = requestContext }; ;
        }
    }
}