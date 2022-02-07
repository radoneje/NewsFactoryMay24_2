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
    /// Summary description for PrompterGetPrev
    /// </summary>
    public class CrowlFileFromMediaGet : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }

        public void ProcessRequest(HttpContext context)
        {

            if (RequestContext.RouteData.Values["mediaId"] == null)
           {
               context.Response.StatusCode = 404;
               context.Response.Write("mediaId Not Found");
               return;
           }
          
           using (var dc = new WebApplication2.Blocks.DataClasses1DataContext())
           {
               var text = dc.Blocks.Where(g => g.Id == Convert.ToInt64(RequestContext.RouteData.Values["mediaId"]) ).First().TextLang3;
              
                context.Response.ContentEncoding = System.Text.Encoding.Unicode;
               context.Response.Clear();
               context.Response.ClearContent();
               context.Response.AddHeader("Content-Disposition", "attachment; filename=graphics.txt");
               context.Response.ContentType = "plain/text";

               

                byte[] bytes = System.Text.Encoding.Default.GetBytes(text);
                text = System.Text.Encoding.UTF8.GetString(bytes);
                context.Response.Write(text);


              
               context.Response.Flush();
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
    public class CrowlFileFromMediaGetRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new CrowlFileFromMediaGet() { RequestContext = requestContext }; ;
        }
    }
}