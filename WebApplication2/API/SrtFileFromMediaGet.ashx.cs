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
    public class SrtFileFromMediaGet : IHttpHandler
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
            if (RequestContext.RouteData.Values["layerId"] == null)
           {
               context.Response.StatusCode = 404;
               context.Response.Write("layerId Not Found");
               return;
           }
           using (var dc = new WebApplication2.Blocks.DataClassesMediaDataContext())
           {
               var mediaGraphicsId = dc.tWeb_mediaGraphics.Where(g => g.MediaId == Convert.ToInt64(RequestContext.RouteData.Values["mediaId"]) && g.layerId == Convert.ToInt32(RequestContext.RouteData.Values["layerId"])).First().id;
               var b = dc.tWeb_MediaGraphicsItems.Where(f => f.mediaGraphicsId == mediaGraphicsId);

               b = b.OrderBy(ff => ff.timeInSec);
               var a = b.ToList();
                context.Response.ContentEncoding = System.Text.Encoding.Unicode;
               context.Response.Clear();
               context.Response.ClearContent();
               context.Response.AddHeader("Content-Disposition", "attachment; filename=graphics.srt");
               context.Response.ContentType = "text/srt";

               for (int i = 0; i < a.Count; i++)
               {
                   context.Response.Write((i+1).ToString() + "\r\n");
                   TimeSpan ts = new TimeSpan(0, 0, (int)a[i].timeInSec);
                   TimeSpan tsEnd = new TimeSpan(0, 0, (i < a.Count - 1) ? (int)a[i + 1].timeInSec : (60 * 60 * 24 -1));
                   context.Response.Write(ts.ToString() + ",0 --> "+ tsEnd.ToString()+",0 \r\n");
                   context.Response.Write(a[i].text.Replace("\r\n"," ") + "\r\n\r\n");

               }
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
    public class SrtFileFromMediaGetRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SrtFileFromMediaGet() { RequestContext = requestContext }; ;
        }
    }
}