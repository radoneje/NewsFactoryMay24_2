using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for GetBlockVideo
    /// </summary>
    public class GetRssVideo : IHttpHandler
    {

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public void ProcessRequest(HttpContext context)
        {
           
            if ( context.Request.Params["MediaId"] == null)
            {
                GetNoVideo(context);
                return;
            }
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext()) {

                var fm = dc.tSocial_feedToMessage.Where(m => m.id == context.Request.Params["MediaId"]);
                if (fm.Count() > 0)
                {
                    dynamic m = Newtonsoft.Json.JsonConvert.DeserializeObject(fm.First().message);
                    string filename = m.video;
                    if(System.IO.File.Exists(filename))
                        utils.Streaming(context, filename, System.Web.MimeMapping.GetMimeMapping(filename));
                    else
                        GetNoVideo(context);
                }
                else
                    GetNoVideo(context);

                
            };

               
        }
        public static void GetNoVideo(HttpContext context)
        {
            responce(context, context.Server.MapPath("../Images/noimage.jpg"));
        }
        public static void responce(HttpContext context, string filename)
        {
            int b;
            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ContentType = System.Web.MimeMapping.GetMimeMapping(filename);
            using (var stream = new System.IO.FileStream(filename, System.IO.FileMode.Open))
            {

                while ((b = stream.ReadByte()) != -1)
                {
                    context.Response.OutputStream.WriteByte((byte)b);
                }
                context.Response.OutputStream.Flush();
            }
        }
        
    }
}