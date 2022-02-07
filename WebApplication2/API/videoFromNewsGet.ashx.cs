using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.IO;
//using System.Web.Helpers;

namespace WebApplication2.API
{
    /// <summary>
    /// Summary description for PrompterGetCurr
    /// </summary>
    public class videoFromNewsGet : IHttpHandler, System.Web.SessionState.IRequiresSessionState 
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType="application/json";
            if (context.Session["UserId"] == null)
            {
                context.Response.Write(/*Newtonsoft.Json.JsonConvert.SerializeObject*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" }));
                return;
            }
            try
            {
                var newsId = Convert.ToInt64(RequestContext.RouteData.Values["newsId"]);
                var ret=new List<dynamic>();
                using(WebApplication2.Blocks.DataClasses1DataContext dc = new WebApplication2.Blocks.DataClasses1DataContext()){

                    dc.Blocks.Where(b=>b.NewsId==newsId && b.deleted==false && b.ParentId==0).OrderBy(bb=>bb.Sort).ToList().ForEach(bl=>{
                        var rec=dc.Blocks.Where(b=>b.NewsId==newsId && b.deleted==false && b.ParentId==bl.Id).OrderBy(bb=>bb.Sort);
                           if(rec.Count()>0)
                           {
                               var ml=rec.First();
                                if(ml.BLockType==2 && ml.BlockText.Length>3)
                                    {
                                        try{
                                        ret.Add(
                                            new{
                                                id=ml.Id,
                                                blockId=bl.Id,
                                                title=ml.Name,
                                                fileName=ml.BlockText,
                                                filePath=Path.GetDirectoryName(ml.BlockText),
                                                fileNameNoPath=Path.GetFileName(ml.BlockText)
                                            }
                                            );
                                        }
                                        catch(Exception ex){}
                                    }
                             }
                    
                    });
                }

                 context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, items=ret }));
            }
            catch(Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = ex.Message }));
                return;
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
    public class videoFromNewsGetRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new videoFromNewsGet() { RequestContext = requestContext }; ;
        }
    }
}