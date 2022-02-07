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
    /// Summary description for NewsToText
    /// </summary>
    public class NewsToText : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.CacheControl = "no-cache";
            context.Response.ContentType = "text/plain";
            
            var NewsId= Convert.ToInt64(RequestContext.RouteData.Values["newsId"]);
            context.Response.Write(GetText(NewsId));
        }

        public static string GetText(long NewsId, bool showBlockHeader=false)
        {
            string ret = "";
            using(Blocks.DataClasses1DataContext dc= new Blocks.DataClasses1DataContext())
            {
                dc.Blocks.Where(b => b.NewsId == NewsId && b.deleted == false && b.ParentId==0).OrderBy(bb => bb.Sort).ToList().ForEach(l => {
                    bool visible = true;
                    var typeName = "";
                    var blockName = "";
                    try
                    {
                        var bt = dc.BlockTypes.Where(t => t.id == l.BLockType).First();
                         visible=bt.Jockey;
                         typeName=bt.TypeName;
                       // blockName=bt.n
                    }
                    catch{}
                    if (visible)
                    {
                        // ret += "=" + typeName.ToUpper() + "=";
                        if (showBlockHeader)
                            ret += "[" + l.Name + "]\r\n";
                        ret += DeleteComments(" "+l.BlockText) + "((JUMP))";
                    }
                    else
                    {
                       // ret += "\r\n************\r\n";
                        //ret += "\r\n=" + typeName.ToUpper() + "=";
                        ret += "\r\n";
                        if (showBlockHeader)
                            ret += "\r\n[" + l.Name + "]\r\n";
                        ret += "\r\n";
                    }
                });
            }
            return removeSpaces(ret);
        }
        public static string GetAutoscript(long NewsId)
        {
            string ret = "";
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.Blocks.Where(b => b.NewsId == NewsId && b.deleted == false && b.ParentId == 0).OrderBy(bb => bb.Sort).ToList().ForEach(l =>
                {
                    bool visible = true;
                    var typeName = "";
                    var blockName = "";
                    try
                    {
                        var bt = dc.BlockTypes.Where(t => t.id == l.BLockType).First();
                        visible = bt.Jockey;
                        typeName = bt.TypeName;
                      
                    }
                    catch { }
                    ret += "\\par [" + l.Name + "]";
                    if (visible)
                    {
                        // ret += "=" + typeName.ToUpper() + "=";
                        // ret += "=" + l.Name + "=";
                        ret += "\\par "+DeleteComments(" " + l.BlockText);// +"((JUMP))";
                    }
                    else
                    {
                        System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"\[\[([^\]]+)\]\]");
                        foreach (System.Text.RegularExpressions.Match m in re.Matches(l.BlockText))
                        {
                            ret += "\\par " + m.Groups[1].Value + "\r\n";
                        };
                        
                        // ret += "\r\n************\r\n";
                        //ret += "\r\n=" + typeName.ToUpper() + "=";
                        // ret += "\r\n//";
                        // ret += "\r\n//" + l.Name + "\r\n";
                        // ret += "\r\n************\r\n";
                    }
                });
            }
            return removeSpaces(ret);
        }
        public static string GetFULLText(long NewsId)
        {
            string ret = "";
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.Blocks.Where(b => b.NewsId == NewsId && b.deleted == false).OrderBy(bb => bb.Sort).ToList().ForEach(l =>
                {

                    ret += "=="+l.Name+"==\r\n"+ (l.BlockText) + "\r\n***\u9660\r\n";
                });
            }
            return removeSpaces(ret);
        }
        
        public static string removeSpaces(string s)
        {

           //  var ret=System.Text.RegularExpressions.Regex.Replace(s, @"[\s+]", " ");
            var ret = s.Trim();
            while(ret.IndexOf("  ")>=0)
            {
               ret= ret.Replace("  ", " ");
            }
            while (ret.IndexOf("*** ***") >= 0)
            {
                ret = ret.Replace("*** ***", "***");
            }
            ret = ret.Replace(" ***", "\r\n***\r\n");
            return ret;
        }
        public static string DeleteComments(string txt)
        {
            while (txt.IndexOf("((") >= 0)
            {
                int s = txt.IndexOf("((");
                int e = -1;
                if (s >= 0)
                    e = txt.IndexOf("))", s);
                if (s < 0 || e < 0 || e < s)
                    return txt;
                var a = txt.Substring(0, s);
                var b = txt.Substring(e + 2, txt.Length - e - 2);
                txt = a +"\r\n(SOT)\r\n"+ b;
            }


           return txt; // System.Text.RegularExpressions.Regex.Replace(txt, @"\(\([\s\S^\)]+\)\)", "");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class NewsToTextRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new NewsToText() { RequestContext = requestContext }; ;
        }
    }
}