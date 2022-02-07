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
    /// Summary description for Programs
    /// </summary>
    public class Programs : IHttpHandler
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
                lock (RequestContext.HttpContext.Application["dblook"])
                {

                    using (WebApplication2.News.NewsDataContext dc = new WebApplication2.News.NewsDataContext())
                    {

                       
                        dc.Programs.Where(p => p.Deleted == false).ToList().ForEach(l=>{

                            XmlElement mess = xml.CreateElement(string.Empty, "Program", string.Empty);
                            var id = xml.CreateAttribute("id");
                            id.Value = l.id.ToString();
                            mess.Attributes.Append(id);

                            var Name = xml.CreateAttribute("name");
                            Name.Value = l.Name;
                            mess.Attributes.Append(Name);
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
    public class ProgramsRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new Programs() { RequestContext = requestContext };
        }
    }
}