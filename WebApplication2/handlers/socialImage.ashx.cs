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
    public class socialImag : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            try

            {
                var blockId = Convert.ToString(RequestContext.RouteData.Values["blockId"]);

               
               
                using (var db = new Blocks.DataClasses1DataContext())
                {
                    var f=db.tSocial_feedToMessage.Where(s => s.id == blockId);
                    if (f.Count() > 0)
                    {
                        var imgFile = f.First().imgFile;
                        if (System.IO.File.Exists(imgFile))
                        {

                            context.Response.ContentType = MimeMapping.GetMimeMapping(f.First().imgFile);
                            context.Response.WriteFile(imgFile);
                        }
                        else {
                            context.Response.StatusCode = 404;
                            context.Response.Write("Not Found");
                        }
                    }
                    else {
                        context.Response.StatusCode = 404;
                        context.Response.Write("Not Found");
                    }
                }
            }
            catch (Exception ex) {
                context.Response.ContentType = "application/json";
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
    public class socialImagRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new socialImag() { RequestContext = requestContext }; ;
        }
    }
}