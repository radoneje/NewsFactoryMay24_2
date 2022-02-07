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
    public class BlocksChernovik : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/palin";
            context.Response.ContentType = "text/plain";
            var sw = new StreamWriter(context.Response.OutputStream);



            if (RequestContext.RouteData.Values["id"] == null)
            {


                sw.WriteLine("noText");
                sw.Close();
                return;
            }
            
           
            using (var dc = new WebApplication2.Blocks.DataClasses1DataContext())
            {
                var rec = dc.Blocks.Where(t => t.Id == Convert.ToInt64(RequestContext.RouteData.Values["id"]));
                if(rec.Count()>0)
                {
                    sw.WriteLine(rec.First().TextLang1);
                }
            }
             sw.Close();

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class BlocksChernovikRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new BlocksChernovik() { RequestContext = requestContext };
        }
    }
}