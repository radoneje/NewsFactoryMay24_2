using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.IO;
using System.Xml;

namespace WebApplication2.API
{
    /// <summary>
    /// Summary description for Blocks
    /// </summary>
    public class BlocksJson : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
          
                if (RequestContext.RouteData.Values["newsId"] == null)
                //   context.Response.Write("***\r\nERROR\r\nneew NewsId\r\n***");
                {
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { err = true }));
                }
                else
                {
                using (WebApplication2.Blocks.DataClasses1DataContext dc = new WebApplication2.Blocks.DataClasses1DataContext()) {
                   
                        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(
                             dc.vWeb_Blocks.Where(n=>(n.NewsId==Convert.ToInt64(RequestContext.RouteData.Values["newsId"]) && n.ParentId==0)).OrderBy(s => s.Sort).ToList()
                            ));
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
    public class BlocksJsonRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new BlocksJson() { RequestContext = requestContext };
        }
    }
}