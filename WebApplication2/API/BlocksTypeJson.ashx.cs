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
    public class BlocksTypeJson : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
          
               
                using (WebApplication2.Blocks.DataClasses1DataContext dc = new WebApplication2.Blocks.DataClasses1DataContext()) {

                    context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(
                         dc.BlockTypes.ToList()
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
    public class BlocksTypeJsonRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new BlocksTypeJson() { RequestContext = requestContext };
        }
    }
}