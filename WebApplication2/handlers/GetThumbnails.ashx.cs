using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.IO;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description 
    /// 
    
    public class GetThumbnailHandler : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "image/jpeg";
            using (Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
            {
                var blob = dc.vMedia_ThumbnailsForLists.Where(t => t.id == Convert.ToInt32((string)RequestContext.RouteData.Values["ThumbnailId"])).First().image;
                context.Response.OutputStream.Write(blob.ToArray(), 0, blob.Length);
                context.Response.End();
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
    public class GethumbnailRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new GetThumbnailHandler() { RequestContext = requestContext }; ;
        }
    }
}