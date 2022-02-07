using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.IO;
using System.Xml;

namespace WebApplication2.API
{
    /// <summary>
    /// Summary description for News
    /// </summary>
    public class News : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            
            context.Response.ContentType = "text/xml";
            XmlDocument xml = new XmlDocument();
                //(1) the xml declaration is recommended, but not mandatory
            XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration( "1.0", "UTF-8", "yes" );
        

            XmlElement root = xml.DocumentElement;
            xml.InsertBefore( xmlDeclaration, root );
            XmlElement item = xml.CreateElement(string.Empty, "items", string.Empty);



            try
            {
                if (RequestContext.RouteData.Values["programId"] == null)
                //   context.Response.Write("***\r\nERROR\r\nneew NewsId\r\n***");
                {
                    XmlElement err = xml.CreateElement(string.Empty, "Error", string.Empty);
                    err.InnerText = "Need programId";
                    err.AppendChild(item);
                }
                else
                    lock (RequestContext.HttpContext.Application["dblook"])
                    {
                        using (WebApplication2.News.NewsDataContext dc = new WebApplication2.News.NewsDataContext())
                        {
                           
                            dc.News.Where(n => n.ProgramId == Convert.ToInt32(RequestContext.RouteData.Values["programId"]) && n.Deleted == false).OrderByDescending(nn => nn.NewsDate).ToList().ForEach(l => {
                                XmlElement mess = xml.CreateElement(string.Empty, "News", string.Empty);
                               
                                var id = xml.CreateAttribute("id");
                                id.Value = l.id.ToString();
                                mess.Attributes.Append(id);

                                var Name = xml.CreateAttribute("name");
                                Name.Value = l.Name;
                                mess.Attributes.Append(Name);

                               
                                var NewsDate = xml.CreateAttribute("date");
                                NewsDate.Value = l.NewsDate.ToString("dd.MM.yyyy HH.mm.ss");
                                mess.Attributes.Append(NewsDate);
                                item.AppendChild(mess);
                            });
                            
                        }
                    }
            }
            catch (Exception ex)
            {
                XmlElement err = xml.CreateElement(string.Empty, "Error", string.Empty);
                err.InnerText = ex.Message;
                item.AppendChild(err);

            }
            xml.AppendChild(item);
            xml.Save(context.Response.OutputStream);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class NewsRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new News() { RequestContext = requestContext };
        }
    }
}