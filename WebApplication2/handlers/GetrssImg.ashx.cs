using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Сводное описание для rssImg
    /// </summary>
    public class GetRssImg : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Params["MediaId"] == null)
            {
                GetNoImage(context);
                return;
            }
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {

                var fm = dc.tSocial_feedToMessage.Where(m => m.id == context.Request.Params["MediaId"]);
                if (fm.Count() > 0)
                {
                    dynamic m = Newtonsoft.Json.JsonConvert.DeserializeObject(fm.First().message);
                    string filename = m.img;
                    if (System.IO.File.Exists(filename))
                        responce(context, filename);
                    //utils.Streaming(context, filename, System.Web.MimeMapping.GetMimeMapping(filename));
                    else
                        GetNoImage(context);
                }
                else
                    GetNoImage(context);


            };
        }
        public static void GetNoImage(HttpContext context)
        {
            responce(context, context.Server.MapPath("../Images/noimage.jpg"));
        }
        public static void responce(HttpContext context, string filename) {
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}