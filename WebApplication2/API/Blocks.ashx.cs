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
    /// Summary description for Blocks
    /// </summary>
    public class Blocks : IHttpHandler
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
                if (RequestContext.RouteData.Values["newsId"] == null)
                //   context.Response.Write("***\r\nERROR\r\nneew NewsId\r\n***");
                {
                    XmlElement err = xml.CreateElement(string.Empty, "Error", string.Empty);
                    err.InnerText = "Need newsId";
                    err.AppendChild(item);
                }
                else
                    lock (RequestContext.HttpContext.Application["dblook"])
                    {
                        using (WebApplication2.Blocks.DataClasses1DataContext dc = new WebApplication2.Blocks.DataClasses1DataContext())
                        {
                            dc.Blocks.Where(b => b.NewsId == Convert.ToInt64(RequestContext.RouteData.Values["newsId"]) && b.deleted== false && b.ParentId==0).OrderBy(bb => bb.Sort).ToList().ForEach(l => {

                                XmlElement mess = xml.CreateElement(string.Empty, "Block", string.Empty);

                                var id = xml.CreateAttribute("id");
                                id.Value = l.Id.ToString();
                                mess.Attributes.Append(id);

                                var Name = xml.CreateAttribute("name");
                                Name.Value = l.Name;
                                mess.Attributes.Append(Name);

                                var SortOrder = xml.CreateAttribute("SortOrder");
                                SortOrder.Value = l.Sort.ToString();
                                mess.Attributes.Append(SortOrder);

                                var blocktype = dc.BlockTypes.Where(t => t.id == l.BLockType).First();
                                var TypeName = xml.CreateAttribute("typename");
                                TypeName.Value = blocktype.TypeName;
                                mess.Attributes.Append(TypeName);

                                var TypeId = xml.CreateAttribute("typeId");
                                TypeId.Value = blocktype.id.ToString();
                                mess.Attributes.Append(TypeId);


                                var IsJockey = xml.CreateAttribute("isJockey");
                                IsJockey.Value = blocktype.Jockey ? "1" : "0";
                                mess.Attributes.Append(IsJockey);
                                


                                var Autor = xml.CreateAttribute("AutorName");
                                Autor.Value = dc.fWeb_GetUserName(l.CreatorId);
                                mess.Attributes.Append(Autor);
                                item.AppendChild(mess);

                                mess.InnerText = l.BlockText;

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
    public class BlocksRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new Blocks() { RequestContext = requestContext };
        }
    }
}