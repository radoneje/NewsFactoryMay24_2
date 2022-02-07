using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Web.Routing;
using ExcelLibrary.CompoundDocumentFormat;
using ExcelLibrary.SpreadSheet;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for NewsToXLS
    /// </summary>
    public class NewsToXLS : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }

        public  struct TitleObj{
            public string Title;
            public string Cap;
        }

        public static List<TitleObj> GetTitles(Int64 NewsId)
        {
            var ret = new List<TitleObj>();
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.Blocks.Where(b => b.NewsId == NewsId && b.deleted == false).OrderBy(bb => bb.Sort).ToList().ForEach(l =>
                {
                    var t=GetTitlesFromBlock(l.BlockText);
                    foreach (var a in t)
                    {
                        ret.Add(a);
                    }
                });
            }
            ret.Add(new TitleObj() { Cap = "", Title = "" });
            return ret;

        }
        public static List<TitleObj> GetTitlesFromBlock(string txt)
        {
            var ret =new List<TitleObj>();
            txt += "\r\n))";
            while(txt.IndexOf("ТИТР:")>=0)
            {
                var find = new TitleObj();
                var tr=txt.IndexOf("ТИТР:");
                var sh = txt.IndexOf("СИНХРОНИРУЕМЫЙ:");
                if(sh>=0 && sh<tr)
                {
                    var shEnd = txt.IndexOf( "\r\n", sh);
                    find.Title = txt.Substring(sh+15, shEnd - sh-15);
                    find.Title = find.Title.Replace("\r\n","").Trim();
                };
               // var trEnd = txt.IndexOf("))", tr);
                var trEnd1 = txt.IndexOf("))", tr);
                var trEnd2 = txt.IndexOf("\r\n", tr);
                var trEnd = trEnd1 < trEnd2 ? trEnd1 : trEnd2;

                find.Cap = txt.Substring(tr+5, trEnd - tr-5);
                find.Cap=find.Cap.Replace("\r\n","").Trim();
                ret.Add(find);
                txt = txt.Substring(trEnd+5, txt.Length-trEnd-5);

            }
            while (txt.IndexOf("TITLE:") >= 0)
            {
                var find = new TitleObj();
                var tr = txt.IndexOf("TITLE:");
                var sh = txt.IndexOf("NAME:");
                if (sh >= 0 && sh < tr)
                {
                    var shEnd = txt.IndexOf("\r\n", sh);
                    find.Title = txt.Substring(sh + 5, shEnd - sh - 5);
                    find.Title = find.Title.Replace("\r\n", "").Trim();
                };
                // var trEnd = txt.IndexOf("))", tr);
                var trEnd1 = txt.IndexOf("))", tr);
                var trEnd2 = txt.IndexOf("\r\n", tr);
                var trEnd = trEnd1 < trEnd2 ? trEnd1 : trEnd2;

                find.Cap = txt.Substring(tr + 6, trEnd - tr - 6);
                find.Cap = find.Cap.Replace("\r\n", "").Trim();
                ret.Add(find);
                txt = txt.Substring(trEnd + 6, txt.Length - trEnd - 6);

            }

            return ret;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.CacheControl = "no-cache";
            var NewsId = Convert.ToInt64(RequestContext.RouteData.Values["newsId"]);
           var titles= GetTitles(Convert.ToInt64(RequestContext.RouteData.Values["newsId"]));

            context.Response.ContentType = "application/vnd.ms-excel";

            var file = Path.GetTempFileName();
            ExcelLibrary.SpreadSheet.Workbook wb = new ExcelLibrary.SpreadSheet.Workbook();
            ExcelLibrary.SpreadSheet.Worksheet ws = new ExcelLibrary.SpreadSheet.Worksheet("first");
            for (int i = 0; i < 100; i++)
            {
                ws.Cells[i, 0] = new Cell("");
                ws.Cells[i, 1] = new Cell("");
                ws.Cells[i, 2] = new Cell("");
                ws.Cells[i, 3] = new Cell("");
            }
            int sync = 0;
            int title = 0;
            for (int j = 0; j < titles.Count; j++)
            {
                if (titles[j].Title == null)
                {
                    ws.Cells[title, 3] = new ExcelLibrary.SpreadSheet.Cell(titles[j].Cap);
                    title++;
                }
                else
                {
                    ws.Cells[sync, 0] = new ExcelLibrary.SpreadSheet.Cell(titles[j].Title);
                    ws.Cells[sync, 1] = new ExcelLibrary.SpreadSheet.Cell(titles[j].Cap);
                    sync++;
                }
               
               
            }
            ws.Cells.ColumnWidth[0, 0] = 3000;
            ws.Cells.ColumnWidth[0, 1] = 3000;
            wb.Worksheets.Add(ws);

            wb.Save(file);


            byte[] buffer = new byte[32768];
            int read;
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                   context.Response.OutputStream.Write(buffer, 0, read);
                }
            }
            File.Delete(file);
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class NewsToXLSRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new NewsToXLS() { RequestContext = requestContext }; ;
        }
    }
}