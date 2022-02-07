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
    public class ListForGenerateIMG : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "application/json";
            using (Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
            {
                lock (RequestContext.HttpContext.Application["EncodeTaskLook"])
                {

                    if (dc.vMedia_ListForGenerateImages.Count() > 0)
                        context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(dc.vMedia_ListForGenerateImages.First()));
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
    public class ListForGenerateIMGRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ListForGenerateIMG() { RequestContext = requestContext }; ;
        }
    }
}