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
    /// Summary description for NewsToWord
    /// </summary>
    public class NewsToWord : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.CacheControl = "no-cache";

            var NewsId = Convert.ToInt64(RequestContext.RouteData.Values["newsId"]);
            string txt = GetRtfUnicodeEscapedString(NewsToText.GetFULLText(NewsId)).Replace("\r\n", "\\line ");
            context.Response.ContentType = "application/x-rtf";
            char[] ca = System.Text.Encoding.Unicode.GetChars(new byte[] { 0xeb, 0x00 });
            var sw = new StreamWriter(context.Response.OutputStream);
            sw.WriteLine(@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049\deflangfe1049\deftab708{\fonttbl{\f0\fnil\fcharset0 Arial;}}
{\colortbl ;\red255\green255\blue255;\red0\green0\blue0;}

\pard\sa200\sl276\slmult1\cf0\fs32\lang9 
 " + txt + " \\par}");
            sw.Close();




        }

        private string GetRtfUnicodeEscapedString(string s)
        {
            s = NewsToText.removeSpaces(s);

            var sb = new StringBuilder();
            foreach (var c in s)
            {
                if (c <= 0x7f)
                    sb.Append(c);
                else
                    sb.Append("\\u" + Convert.ToUInt32(c) + "?");
            }
            return sb.ToString();
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
    public class NewsToWordRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new NewsToWord() { RequestContext = requestContext }; ;
        }
    }
}