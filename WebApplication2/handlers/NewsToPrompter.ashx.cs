using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Web.Routing;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for NewsToRtf
    /// </summary>
    public class NewsToPrompter : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }

        public void ProcessRequest(HttpContext context)
        {

            context.Response.CacheControl = "no-cache";
            var NewsId = Convert.ToInt64(RequestContext.RouteData.Values["newsId"]);
            var txt=NewsToText.GetText(NewsId);
            var regexp = new System.Text.RegularExpressions.Regex(@"\(\(JUMP\)\)");
            txt = regexp.Replace(txt, "\r\n*\n\r");

            regexp = new System.Text.RegularExpressions.Regex(@"\r\n\(SOT\)\r\n");
            txt = regexp.Replace(txt, "");
            regexp = new System.Text.RegularExpressions.Regex(@"[\r\n]{8}");
            txt = regexp.Replace(txt, "");
            while (txt.IndexOf("&nbsp;") >= 0)
                txt=txt.Replace("&nbsp;", " ");


            if (!string.IsNullOrEmpty(RequestContext.HttpContext.Request.Params["save"]))
            {
                var pts = System.IO.Path.Combine(
                       System.Web.HttpUtility.UrlDecode(RequestContext.HttpContext.Request.Params["save"]),
                       "promter" + DateTime.Now.ToString("yyyy_MM_dd___HH_mm_ss") + ".txt"
                    );
                    Encoding srcEncodingFormat = Encoding.UTF8;
                    Encoding dstEncodingFormat = Encoding.GetEncoding("windows-1251");
                    byte[] originalByteString = srcEncodingFormat.GetBytes(txt);
                    byte[] convertedByteString = Encoding.Convert(srcEncodingFormat,
                    dstEncodingFormat, originalByteString);
                    txt = dstEncodingFormat.GetString(convertedByteString);          
                try
                {
                   
                    System.IO.File.WriteAllText(pts, txt);
                    context.Response.Write("Text is saved to file: " + pts);
                }
                catch (Exception e)
                {
                    context.Response.Write("Error: " + pts +  e.Message);
                }
            }
            else
            {
                context.Response.Headers.Add("Content-Disposition", " attachment; filename = \"prompter.txt\"");
                context.Response.CacheControl = "no-cache";
                context.Response.ContentType = "text/plain";
                context.Response.Write(txt);
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

    public class NewsToPrompterRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new NewsToPrompter() { RequestContext = requestContext }; ;
        }
    }

}