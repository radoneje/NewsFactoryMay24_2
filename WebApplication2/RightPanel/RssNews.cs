using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;

namespace WebApplication2.RightPanel
{ 
    public class RssNews
    {
        public class RSSStruct
        {
            public  string PubDate;
            public string Title;
            public string Link;
            public string Description;
            public string Image;
            public string guid;

        }

        public static void ImportRssFeeds()
        {
            while (true)
            {
                try
                {
                    bool ret = true;
                    News.NewsDataContext dc = new News.NewsDataContext();
                    foreach (var a in dc.tWeb_RssSources.Where(s => s.Active == true))
                    {
                        try
                        {
                            InsertRssIntoDb(ParseRssFile(a.URL), a.id);
                            dc.ExecuteCommand("UPDATE tWeb_RssSources SET LastGetTime=GetDate() WHERE ID=" + a.id.ToString());
                        }
                        catch (Exception ex)
                        {
                            ret = false;
                            var ssa = ex; ;

                        }
                    }
                }
                catch { };

                System.Threading.Thread.Sleep(1000 * 20);
            }
         }
        public static List<RSSStruct>ParseRssFile(string URL)
        {
            XmlDocument rssXmlDoc = new XmlDocument();

            // Load the RSS file from the RSS URL
            rssXmlDoc.Load(URL);

            // Parse the Items in the RSS file
            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

            StringBuilder rssContent = new StringBuilder();
            List<RSSStruct> rss = new List<RSSStruct>();
            // Iterate through the items in the RSS file
            foreach (XmlNode rssNode in rssNodes)
            {
                RSSStruct rssItem = new RSSStruct();
                XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                rssItem.Title = rssSubNode != null ? rssSubNode.InnerText : "";
                    

                rssSubNode = rssNode.SelectSingleNode("link");
                rssItem.Link= rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("description");
                rssItem.Description   = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("pubDate");
                rssItem.PubDate = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("enclosure");
                rssItem.Image = rssSubNode != null ? rssSubNode.Attributes[0].InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("guid");
                rssItem.guid = rssSubNode != null ? rssSubNode.InnerText : "";

                rss.Add(rssItem);
            }
            return rss;
        }
        public static void InsertRssIntoDb(List<RSSStruct> rss, int SourceId)
        {
            News.NewsDataContext dc = new News.NewsDataContext();
            foreach (var a in rss)
            {
                dc.pWeb_InsertRssFeed(DateTime.Parse(a.PubDate), a.Image, a.Title, a.Description, a.Link, a.guid, SourceId);   
            }
        }

     }
    
}