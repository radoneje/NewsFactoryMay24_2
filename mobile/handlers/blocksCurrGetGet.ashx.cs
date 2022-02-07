using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace mobile.handlers
{
    /// <summary>
    /// Summary description for blocksCurrGetGet
    /// </summary>
    public class blocksCurrGetGet : IHttpHandler
    {
        public struct block
        {
            public long id;
            public string autor;
            public string name;
            public string type;
            public int ready;
            public string chrono;
            public string text;
        }

        public void ProcessRequest(HttpContext context)
        {
             context.Response.ContentType = "application/json";
            context.Response.CacheControl = "no-cache";

         
            var prm = CUtils.GetAjaxRes(context.Request.InputStream);
            long newsId=Convert.ToInt64(prm.id);
            if (!CUtils.checkGuid(prm.guid, context.Application))
            {
                context.Response.StatusCode = 401;
                context.Response.Write(System.Web.Helpers.Json.Encode(new { state = "not autorize" }));
                return;
            }
            var ret = new List<block>();
            using(MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                dc.vWeb_Blocks.Where(b => b.NewsId == newsId && b.ParentId==0 ).OrderBy(bb => bb.Sort).ToList().ForEach(l => {
                    var ext = dc.vWeb_BlockToExtViews.Where(bl => bl.Id == l.Id ).First();

                    var text = CUtils.RemoveHTMLTags(ext.BlockText);
                    text = System.Text.RegularExpressions.Regex.Replace(text, @"\(\(([^)]+)\)\)", " ");
                    ret.Add(new block() { 
                    id=l.Id,
                    autor=dc.vWeb_UsersLoginLists.Where(u=>u.UserID==l.CreatorId).Count()>0?dc.vWeb_UsersLoginLists.Where(u=>u.UserID==l.CreatorId).First().UserName:"",
                    chrono=l.BlockTime,
                    name=l.Name,
                    type=l.TypeName,
                    text=text,
                    ready=(l.Ready?1:0)+(l.Approve?1:0)

                    });
                
                });
            }

            context.Response.Write(System.Web.Helpers.Json.Encode(new { state = "ok", items = ret }));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}