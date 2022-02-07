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
    public class LrvAdd : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

          //  RequestContext.HttpContext.Request.g.Read
            string instr = (new System.IO.StreamReader(RequestContext.HttpContext.Request.InputStream)).ReadToEnd();
            var json = System.Web.Helpers.Json.Decode(instr);

            using (Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
            {
                lock (RequestContext.HttpContext.Application["EncodeTaskLook"])
                {
                    Int64 id = json.id;
                    dc.pMedia_AddLRV(json.id, json.type, json.title, json.msg);
                    try
                    {
                        var mediaId = dc.tWeb_MediaTasks.Where(t => t.id == id).First().MediaId;
                        var msg = "";
                        switch ((int)json.type)
                        {
                            case 1: { msg = "lrvStarted"; }; break;
                            case 2: { msg = "lrvCreated"; }; break;
                            default: { msg = "lrvFailed"; }; break;
                        }
                        //  var mediaId = dc.tWeb_MediaTasks.Where(t => t.id == taskId).First().MediaId;
                        var blockId = dc.vWeb_MediaForLists.Where(m => m.Id == mediaId).First().ParentId;
                        NFSocket.SendToAll.SendData(msg, new { mediaId = mediaId, blockId = blockId });
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            context.Response.Write("ok");
           
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class LrvAddRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new LrvAdd() { RequestContext = requestContext }; ;
        }
    }
}