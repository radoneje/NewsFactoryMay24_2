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
    /// Summary description for PrompterGetPrev
    /// </summary>
    public class PrompterGetPrev : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            XmlDocument xml = new XmlDocument();
            //(1) the xml declaration is recommended, but not mandatory
            XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0", "UTF-8", "yes");


            XmlElement root = xml.DocumentElement;
            xml.InsertBefore(xmlDeclaration, root);
            XmlElement item = xml.CreateElement(string.Empty, "item", string.Empty);



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


                            var blocks = dc.Blocks.Where(b => b.NewsId == Convert.ToInt64(RequestContext.RouteData.Values["newsId"]) && b.deleted == false && b.ParentId == 0).OrderBy(bb => bb.Sort);
                            if (blocks.Count() == 0)
                            {
                                XmlElement mess = xml.CreateElement(string.Empty, "Block", string.Empty);
                                var id = xml.CreateAttribute("id");
                                id.Value = "-1";
                                mess.Attributes.Append(id);

                                var Name = xml.CreateAttribute("name");
                                Name.Value = "***** // *****";
                                mess.Attributes.Append(Name);


                                var TypeName = xml.CreateAttribute("typename");
                                TypeName.Value = "NO BLOCKS";
                                mess.Attributes.Append(TypeName);

                                var IsJockey = xml.CreateAttribute("IsJockey");
                                IsJockey.Value = "0";
                                mess.Attributes.Append(IsJockey);


                                mess.InnerText = "***** \u21A6 \u21A6 *****";
                                item.AppendChild(mess);

                            }
                            else
                            {
                                

                                WebApplication2.Blocks.Blocks block = blocks.First();
                                if (RequestContext.RouteData.Values["blockId"] != null)
                                {
                                    if (blocks.Where(bl => bl.Sort < blocks.Where(bs => bs.Id == Convert.ToInt64(RequestContext.RouteData.Values["blockId"])).First().Sort).Count() > 0)
                                        block = blocks.Where(bl => bl.Sort < blocks.Where(bs => bs.Id == Convert.ToInt64(RequestContext.RouteData.Values["blockId"])).First().Sort).OrderByDescending(ss=>ss.Id).First();
                                    else
                                    {
                                        XmlElement mess1 = xml.CreateElement(string.Empty, "Block", string.Empty);
                                        var id1 = xml.CreateAttribute("id");
                                        id1.Value = "-1";
                                        mess1.Attributes.Append(id1);

                                        var Name1 = xml.CreateAttribute("name");
                                        Name1.Value = "** \u21a7 \u21a7 **";
                                        mess1.Attributes.Append(Name1);


                                        var TypeName1 = xml.CreateAttribute("typename");
                                        TypeName1.Value = "";
                                        mess1.Attributes.Append(TypeName1);

                                        var IsJockey1 = xml.CreateAttribute("IsJockey");
                                        IsJockey1.Value = "0";
                                        mess1.Attributes.Append(IsJockey1);


                                        mess1.InnerText = "***** \u21A6 \u21A6 *****";
                                        item.AppendChild(mess1);
                                        xml.AppendChild(item);
                                        xml.Save(context.Response.OutputStream);
                                        return;
                                    }

                                }

                                XmlElement mess = xml.CreateElement(string.Empty, "Block", string.Empty);
                                var id = xml.CreateAttribute("id");
                                id.Value = block.Id.ToString();
                                mess.Attributes.Append(id);

                                var Name = xml.CreateAttribute("name");
                                Name.Value = block.Name;
                                mess.Attributes.Append(Name);

                                var blocktype = dc.BlockTypes.Where(t => t.id == block.BLockType).First();
                                var TypeName = xml.CreateAttribute("typename");
                                TypeName.Value = blocktype.TypeName;
                                mess.Attributes.Append(TypeName);

                                var IsJockey = xml.CreateAttribute("IsJockey");
                                IsJockey.Value = blocktype.Jockey ? "1" : "0";
                                mess.Attributes.Append(IsJockey);


                                mess.InnerText = /*System.Text.RegularExpressions.Regex.Replace(block.BlockText, @"\(\(.+\)\)", "")*/block.BlockText;
                                item.AppendChild(mess);
                            }
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
    public class PrompterGetPrevRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new PrompterGetPrev() { RequestContext = requestContext }; ;
        }
    }
}