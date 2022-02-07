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
    public class rssPage : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            try

            {
                context.Response.ContentType = "text/html";
                var feedId = Convert.ToString(RequestContext.RouteData.Values["feedId"]);
                var ret = "<!DOCTYPE html>\r\n<html><head><meta charset=\"utf-8\"></head><body>";
                using (var db = new Blocks.DataClassesMediaDataContext())
                {
                    db.vWeb_mobileView.Where(v => v.id == feedId).ToList().ForEach(l =>
                    {
                        dynamic m = Newtonsoft.Json.JsonConvert.DeserializeObject(l.message);
                        //{ "title":"аff","subTitle":"","message":"«хочу пускают в сам бам»","mediaId":0,"mediaType":0}
                        ret += "<h1>"+ ((string)m.title).Replace("<", "").Replace(">", "") + "</h1>";
                        ret += "<h2>" + ((string)m.subTitle).Replace("<", "").Replace(">", "") + "</h2>";
                        if (m.mediaType == 1)
                        {
                            ret += "<img src=\"https://video.tvtambov.ru/handlers/GetRssImg.ashx?MediaId=" +l.id + "\" ></img>\r\n";
                        }
                        if (m.mediaType == 2)
                        {
                            ret += "<video poster=\"https://video.tvtambov.ru/handlers/GetRssImg.ashx?MediaId=" + l.id + "\" src=\"https://video.tvtambov.ru/handlers/GetRssVideo?MediaId=" + l.id + "\" width=\"320\" height=\"240\" controls></video>\r\n";
                     
                        }
                        ret += "<p>" + ((string)m.message).Replace("<", "").Replace(">", "").Replace("\n","\n</br>") + "</p>";
                    });

                }
                ret += "</body></html>";
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
    public class rssPageRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new rssPage() { RequestContext = requestContext }; ;
        }
    }
}