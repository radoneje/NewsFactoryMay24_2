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
    public class NewsToRtf : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                bool showBlockHeader = true;
                var showHeaderItem = dc.tWeb_Settings.Where(s => s.Key == "prompterHeaderShow");
                if (showHeaderItem.Count() > 0 && showHeaderItem.First().value == "0")
                    showBlockHeader = false;





                context.Response.CacheControl = "no-cache";
            var NewsId = Convert.ToInt64(RequestContext.RouteData.Values["newsId"]);
            string txt = GetRtfUnicodeEscapedString("\\cf0\\protect0\\line"+NewsToText.GetText(NewsId, showBlockHeader)).Replace("\r\n", "\\line \r\n ").Replace("&nbsp;"," ").Replace("((JUMP))", "\\par\r\n\\cf1\\protect\\u9660\\(    \\par\r\n\\cf0\\protect0");
            context.Response.ContentType = "application/x-rtf";
            char[] ca = System.Text.Encoding.Unicode.GetChars(new byte[] { 0xeb, 0x00 });
            txt = @"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049\deflangfe1049\deftab708{\fonttbl{\f0\fnil\fcharset0 Arial;}}
{\colortbl ;\red255\green5\blue5;}

\pard\sa200\sl276\slmult1\fs26\lang9 
 " + txt + " \\par}";

            var sw = new StreamWriter(context.Response.OutputStream);
            
            sw.WriteLine(txt);
            sw.Close();
            try
            {
                
                    var rec = dc.tWeb_Settings.Where(s => s.Key == "pathToPrompt");
                    if (rec.Count() > 0) 
                    {  
                        var path = rec.First().value;
                        var filename = (new News.NewsDataContext()).News.Where(n => n.id == NewsId).First().Name;
                        filename=filename.Replace("/", "").Replace("\\", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace(">", "").Replace("<", "").Replace("|", "");
                        filename = System.IO.Path.Combine(path, filename);
                        filename += ".rtf";
                        int i = 0;
                        while (System.IO.File.Exists(filename))
                        {
                            i++;
                            filename = filename.Substring(0, filename.Length - 4);
                            filename += "_" + i.ToString() + ".rtf";
                        }

                        var fw = System.IO.File.CreateText(filename);
                        fw.WriteLine(txt);
                        fw.Close();
                    }
                
            }
            catch { }
        }
 
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

    public class NewsToRtfRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new NewsToRtf() { RequestContext = requestContext }; ;
        }
    }

}