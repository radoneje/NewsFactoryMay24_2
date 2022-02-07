using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mobile.handlers
{
    /// <summary>
    /// Summary description for programsGet
    /// </summary>
    public class programsGet : IHttpHandler
    {
        public struct progrs
        {
            public long id;
            public string name;
     

        }
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "application/json";
            context.Response.CacheControl = "no-cache";
           
            var prm = CUtils.GetAjaxRes(context.Request.InputStream);
            if(!CUtils.checkGuid(prm.guid, context.Application))
            {
                context.Response.StatusCode = 401;
                context.Response.Write(System.Web.Helpers.Json.Encode(new { state="not autorize"}));   
                return;
            }
            var ret = new List<progrs>();
            using(MainDataClassesDataContext dc = new MainDataClassesDataContext())
            {
                dc.v_ProgramsToLists.Where(p=>p.Deleted==false && p.id>0).OrderBy(pp=>pp.id).ToList().ForEach(l=>{
                    ret.Add(new progrs(){
                     id=l.id,
                     name=l.Name
                    });
                });
            }
            context.Response.Write(System.Web.Helpers.Json.Encode( new { state = "ok", items=ret })); 
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