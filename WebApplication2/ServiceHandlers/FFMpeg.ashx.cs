using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Threading.Tasks;
using System.IO;

namespace WebApplication2.ServiceHandlers
{
    /// <summary>
    /// Summary description for FFMpeg
    /// </summary>
    public class FFMpeg : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            using(Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
               string path= dc.vWeb_Settings.Where(s => s.id == 8).First().value;

               context.Response.WriteFile(path);
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
    public class FFMpegRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new FFMpeg() { RequestContext = requestContext }; ;
        }
    }
}