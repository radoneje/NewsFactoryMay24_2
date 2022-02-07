using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;


namespace WebApplication2.rest
{
    /// <summary>
    /// Сводное описание для api
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    [System.Web.Script.Services.ScriptService]
    public class api : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void programs()
        {
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            var json = "";
            using (var dc = new WebApplication2.News.NewsDataContext())
            {
                json= new JavaScriptSerializer().Serialize( dc.Programs.Where(r => r.Deleted == false && r.id>0).Select(r => new { r.id, r.Name }));
            }

                Context.Response.Write(json);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void news(long programid)
        {
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            var json = "";
            using (var dc = new WebApplication2.News.NewsDataContext())
            {
                json = new JavaScriptSerializer().Serialize(dc.News.Where(r=>r.ProgramId== programid && r.Deleted==false ).OrderByDescending(r=>r.NewsDate));
            }

            Context.Response.Write(json);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void blocks(long newsid)
        {
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            var json = "";
            using (var dc = new WebApplication2.Blocks.DataClasses1DataContext())
            {
                json = new JavaScriptSerializer().Serialize(
                    dc.Blocks.Where(r => r.deleted == false && r.ParentId == 0 && r.NewsId==newsid).OrderBy(r => r.Sort).Select(r => new {
                        r.Id,
                        r.Name,
                        type=dc.BlockTypes.Where(t=>t.id==r.BLockType).First().TypeName,
                        typeId=r.BLockType,
                        author=dc.Users.Where(t=>t.UserID==r.CreatorId).First().UserName,
                        r.Sort,
                        r.TaskTime,
                        r.CalcTime,
                        r.BlockTime
                    })
                    );
            }

            Context.Response.Write(json);
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void newsTitles(long newsid)
        {
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            var ret = new List<dynamic>();
            using (var dc = new WebApplication2.Blocks.DataClasses1DataContext())
            {
                dc.Blocks.Where(r => r.deleted == false && r.ParentId == 0 && r.NewsId == newsid).OrderBy(r => r.Sort).ToList().ForEach(l=> {
                    var titles = new List<dynamic>();

                    var text = l.BlockText;
                    var regex = new Regex(@"\(TITLE\s([A-Z]+):([^)]+)");

                   
                    foreach (Match match in regex.Matches(text))
                    {
                       
                        titles.Add(new { type = match.Groups[1].Value, title = match.Groups[2].Value });
                       
                       
                    }
                     var reg = new Regex(@"TITLE:([^\r]+)");
                     var reg1 = new Regex(@"NAME:([^\r]+)");

                    var ii = 0;
                    var titleMatches = reg.Matches(text);
                    foreach (Match match in reg1.Matches(text))
                    {
                        var subTitle = "";
                        if (titleMatches.Count > ii)
                        {
                            subTitle=titleMatches[ii].Groups[1].Value;
                        }
                        titles.Add(new { type = "SOT", title = match.Groups[1].Value , subTitle= subTitle });
                        ii++;
                    }

                    ret.Add(new { blockid = l.Id, blockName = l.Name, titles = titles });
                        

                });
            }

            var json = new JavaScriptSerializer().Serialize(ret);
            Context.Response.Write(json);


        }
    }
}
