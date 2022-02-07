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
    public class LrvUpdateChrono : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

          //  RequestContext.HttpContext.Request.g.Read
            string taskId = "";
            DateTime time;
            TimeSpan videoChrono = new TimeSpan(0);
            try
            {
                string instr = (new System.IO.StreamReader(RequestContext.HttpContext.Request.InputStream)).ReadToEnd();
                var json = System.Web.Helpers.Json.Decode(instr);
                 taskId = json.mediaId;
                 time = DateTime.Parse( json.time);
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
                        var mediaId = rec.First().MediaId;

                        using (Blocks.DataClasses1DataContext dcb = new Blocks.DataClasses1DataContext())
                        {
                            var bl = dcb.Blocks.Where(b => b.Id == mediaId).First();
                            videoChrono = new TimeSpan(0, time.Hour, time.Minute, time.Second, time.Millisecond);
                            bl.BlockTime = (long)videoChrono.TotalSeconds;
                            dcb.SubmitChanges();
                        }

                        NFSocket.SendToAll.SendData("lrvUpdateChrono", new { mediaId = rec.First().MediaId, status = time.ToString("HH:mm:ss") });
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
    public class LrvUpdateChronoRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new LrvUpdateChrono() { RequestContext = requestContext }; ;
        }
    }
}