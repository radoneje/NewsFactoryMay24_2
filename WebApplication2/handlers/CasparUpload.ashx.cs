using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.IO;
using System.Xml;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for CasparUpload
    /// </summary>
    public class CasparUpload : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        private string DayOfWeekFormat(int d)
        {
            switch(d)
            {
                case 1: return "1ПОНЕДЕЛЬНИК";
                case 2: return "2ВТОРНИК";
                case 3: return "3СРЕДА";
                case 4: return "4ЧЕТВЕРГ";
                case 5: return "5ПЯТНИЦА";
                case 6: return "6СУББОТА";
                case 7: return "7ВОСКРЕСЕНЬЕ"; 
            }
            return "";
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.Headers.Add("Content-Type","application/force-download");// .ContentType = "text/xml";
            context.Response.Headers.Add("Content-Type", "application/octet-steam");
            context.Response.Headers.Add("Content-Type", "application/download");
            context.Response.Headers.Add("Content-Disposition: attachment; filename:\"caspar\""+DateTime.Now.ToString("dd_MM_yyyy_HH_mm")+".xml\"", "application/force-download");

            XmlDocument xml = new XmlDocument();
                //(1) the xml declaration is recommended, but not mandatory
            XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration( "1.0", "UTF-8", "yes" );
        

            XmlElement root = xml.DocumentElement;
            xml.InsertBefore( xmlDeclaration, root );
            
         //   var attr = xml.CreateAttribute("standalone");
         //   attr.Value = "yes";
         //   xmlDeclaration.Attributes.Append(attr);


            XmlElement items = xml.CreateElement(string.Empty, "items", string.Empty);
            xml.AppendChild(items);

     

            XmlElement allowremotetriggering = xml.CreateElement(string.Empty, "allowremotetriggering", string.Empty);
            allowremotetriggering.AppendChild(xml.CreateTextNode("false"));
            items.AppendChild(allowremotetriggering);

            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                using (News.NewsDataContext dcn = new News.NewsDataContext())
                {
                    var layer = true;
                    var nws = dcn.News.Where(n => n.id.ToString() == (string)RequestContext.RouteData.Values["newsId"]).First();
                    dc.vWeb_Blocks.Where(bl => bl.ParentId == 0 && bl.NewsId.ToString() == (string)RequestContext.RouteData.Values["newsId"]).OrderBy(bbl => bbl.Sort).ToList().ForEach(ll =>
                    {
                        dc.Blocks.Where(c => c.ParentId == ll.Id && c.deleted == false && c.BLockType == 2 /*video only*/).OrderBy(cc => cc.Sort).ToList().ForEach(l =>
                        {

                            
                            XmlElement item = xml.CreateElement(string.Empty, "item", string.Empty);
                            items.AppendChild(item);
                            item.InnerXml += "<type>" + "MOVIE" + "</type>";
                            item.InnerXml += "<devicename>" + "Local CasparCG" + "</devicename>";
                            item.InnerXml += "<label>" + DayOfWeekFormat((int)nws.NewsDate.DayOfWeek) + "/" + nws.NewsDate.ToString("dd.MM.yyyy") + "/" + System.IO.Path.GetFileNameWithoutExtension(l.BlockText) + "</label>";
                            item.InnerXml += "<name>" + DayOfWeekFormat((int)nws.NewsDate.DayOfWeek) + "/" + nws.NewsDate.ToString("dd.MM.yyyy") + "/" + System.IO.Path.GetFileNameWithoutExtension(l.BlockText) + "</name>";
                            item.InnerXml += "<channel>" + (layer ? "1" : "2") + "</channel>";
                            item.InnerXml += "<videolayer>10</videolayer>";
                            item.InnerXml += "<delay>0</delay>";
                            item.InnerXml += "<duration>0</duration>";
                            item.InnerXml += "<allowgpi>false</allowgpi>";
                            item.InnerXml += "<allowremotetriggering>false</allowremotetriggering>";
                            item.InnerXml += "<remotetriggerid></remotetriggerid>";
                            item.InnerXml += "<storyid></storyid>\r\n";
                            item.InnerXml += "<transition>CUT</transition>";
                            item.InnerXml += "<transitionDuration>1</transitionDuration>";
                            item.InnerXml += "<tween>Linear</tween>";
                            item.InnerXml += "<direction>RIGHT</direction>";
                            item.InnerXml += "<seek>0</seek>";
                            item.InnerXml += "<length>0</length>";
                            item.InnerXml += "<loop>false</loop>";
                            item.InnerXml += "<freezeonload>true</freezeonload>";
                            item.InnerXml += "<triggeronnext>false</triggeronnext>";
                            item.InnerXml += "<autoplay>FALSE</autoplay>";
                            item.InnerXml += "<color>Transparent</color>";
                            item.InnerXml += "<timecode>00:00:00:00</timecode>";
                            layer = !layer;
                        });

                    });

                }
            }
          //  xml.Save(context.Response.OutputStream);

            context.Response.Write(xml.OuterXml.ToString().Replace("\r","").Replace("\n","").Replace("\t",""));
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class CasparUploadRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new CasparUpload() { RequestContext = requestContext }; ;
        }
    }
}