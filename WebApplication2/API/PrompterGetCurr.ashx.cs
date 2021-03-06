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
    /// Summary description for PrompterGetCurr
    /// </summary>
    public class PrompterGetCurr : IHttpHandler
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
               
           
                    lock (RequestContext.HttpContext.Application["dblook"])
                    {

                        using (WebApplication2.Blocks.DataClasses1DataContext dc = new WebApplication2.Blocks.DataClasses1DataContext())
                        {


                            var blocks = dc.Blocks.Where(b => b.Id == Convert.ToInt64(RequestContext.RouteData.Values["blockId"]) );
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
                                TypeName.Value = "END";
                                mess.Attributes.Append(TypeName);

                                var IsJockey = xml.CreateAttribute("IsJockey");
                                IsJockey.Value = "0";
                                mess.Attributes.Append(IsJockey);


                                mess.InnerText = "***** \u21A6 \u21A6 *****";
                                item.AppendChild(mess);

                            }
                            else
                            {
                                var block = blocks.First();

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
    public class PrompterGetCurrRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new PrompterGetCurr() { RequestContext = requestContext }; ;
        }
    }
}