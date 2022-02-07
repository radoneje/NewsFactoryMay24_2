using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebApplication2.ServiceHandlers
{
    /// <summary>
    /// Summary description for getServiceConfig
    /// </summary>
    public class LrvUpdateStatus : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

          //  RequestContext.HttpContext.Request.g.Read
            string taskId = "";
            string status = "";
            try
            {
                string instr = (new System.IO.StreamReader(RequestContext.HttpContext.Request.InputStream)).ReadToEnd();
                var json = System.Web.Helpers.Json.Decode(instr);
                 taskId = json.mediaId;
                 status = json.status;
                context.Response.Write("ok");
            }
            catch(Exception ex)
            {
                context.Response.Write("no params");
                return;
            }
          
            context.Response.Flush();
            context.Response.Close();

            using(Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
            {
                lock (RequestContext.HttpContext.Application["EncodeTaskLook"])
                {
                    var rec = dc.tWeb_MediaTasks.Where(t => t.id == Convert.ToInt64(taskId));
                    if (rec.Count() > 0)
                    {
                        NFSocket.SendToAll.SendData("lrvUpdateStaus", new { mediaId = rec.First().MediaId, blockId = dc.vWeb_MediaForLists.Single(r => r.Id == rec.First().MediaId).ParentId, status = status });
                    }
                }
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
    public class LrvUpdateStatusRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new LrvUpdateStatus() { RequestContext = requestContext }; ;
        }
    }
}