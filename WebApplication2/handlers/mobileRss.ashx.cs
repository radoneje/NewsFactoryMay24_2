using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Web.Routing;


namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for NewsToTitleList
    /// </summary>
    public class mobileRss : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            try

            {
                context.Response.ContentType = "application/rss+xml";
                var ret = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n";
                ret += "<rss  xmlns:yandex = \"http://news.yandex.ru\" xmlns:media = \"http://search.yahoo.com/mrss/\" version = \"2.0\" > \r\n";
               ret += "<channel>\r\n";
                ret += "<title>Tambov News</title>\r\n";
                ret += "<link>https://video.tvtambov.ru/rss/</link>\r\n";
                ret += "<description>Новости Тамбова</description>\r\n";
                ret += "<language>ru</language>\r\n";

                using (var db = new Blocks.DataClassesMediaDataContext())
                {
                    db.vWeb_mobileView.OrderByDescending(v=>v.insertDate).ToList().ForEach(l => {
                        ret += "<item>\r\n";
                        
                        ret += "<link>" + "https://video.tvtambov.ru/rssPage/"+l.id + "</link>\r\n";
                       
                        ret += "<guid>" +l.id + "</guid>\r\n";
                        ret += "<pubDate>" + l.insertDate.ToString("r") + "</pubDate>\r\n";

                        dynamic m = Newtonsoft.Json.JsonConvert.DeserializeObject(l.message);
                        //{ "title":"аff","subTitle":"","message":"«хочу пускают в сам бам»","mediaId":0,"mediaType":0}
                        ret += "<title>" +((string) m.title).Replace("<","").Replace(">","") + "</title>\r\n";
                        ret += "<description>" + ((string)m.subTitle).Replace("<", "").Replace(">", "") + "</description>\r\n";
                        ret += "<yandex:full-text>" + ((string)m.message).Replace("<", "").Replace(">", "") + "</yandex:full-text>\r\n";
                        if (((int)m.type)==1)
                        {     
                                ret += "<enclosure url=\"https://video.tvtambov.ru/handlers/GetRssImg.ashx?MediaId=" + l.id + "\" type=\"image/jpeg\" " + "></enclosure>\r\n";
                        }
                        else if (m.type == 2)
                        {

                            ret += "<enclosure url=\"https://video.tvtambov.ru/handlers/GetRssImg.ashx?MediaId=" + l.id + "\" type=\"image/jpeg\" " + "></enclosure>\r\n";

                            ret += "<enclosure url=\"https://video.tvtambov.ru/handlers/GetRssVideo.ashx?MediaId=" + l.id + "\" type=\"video/mp4\" " + "></enclosure>\r\n";
                        }
                        else if (l.imgFile.Length != 0) {
                            ret += "<enclosure url=\"https://video.tvtambov.ru/handlers/GetRssImg.ashx?MediaId=" + l.id + "\" type=\"image/jpeg\" " + "></enclosure>\r\n";
                        }

                        ret += "</item>\r\n";
                    });
                    

                }
                ret += "</channel>\r\n";
                ret += "</rss>";
                context.Response.Write(ret);
            }
            catch (Exception ex) {
               
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject( new { status = ex.Message }));
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
    public class mobileRssRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new mobileRss() { RequestContext = requestContext }; ;
        }
    }
}