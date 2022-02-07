using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebApplication2.API
{
    /// <summary>
    /// Summary description for ProgrammsListGet
    /// </summary>
    public class ProgrammsListWidthRightGet : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public struct programs
        {
            public long id;
            public string name;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var ret=new List<dynamic>();
            using (WebApplication2.News.NewsDataContext dc = new WebApplication2.News.NewsDataContext())
            {
                dc.Programs.Where(p => p.Deleted == false && p.id>0).OrderBy(pp=>pp.Name).ToList().ForEach(l =>
                {

                    var name = l.Name;
                   if((new WebApplication2.Blocks.DataClasses1DataContext()).vUsersRights.Where(r=>r.ProgramID==l.id && r.UserID==Convert.ToInt32(RequestContext.RouteData.Values["id"])).Count()>0)
                   {
                       name = "+&nbsp;" + name;
                   }
                   else
                       name = "&nbsp;&nbsp;" + name;
                   ret.Add(new { id = l.id, name = name });
                   
                });
            }
            context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { items = ret }));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class ProgrammsListWidthRightGetHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ProgrammsListWidthRightGet() { RequestContext = requestContext };
        }
    }
}