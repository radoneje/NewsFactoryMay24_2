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
    public class blockText : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            try

            {
                context.Response.ContentType = "application/json";
                var blockId = Convert.ToInt64(RequestContext.RouteData.Values["blockId"]);
                using (var db = new Blocks.DataClasses1DataContext())
                {
                    var settings = db.tWeb_Settings.Where(s => s.Key == "pathToSaveBlockTexts");
                    if (settings.Count() == 0) {
                        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { status = "no pathToSaveBlockTexts key in settings" }));
                        return;
                    }
                    var block = db.Blocks.Where(b => b.Id == blockId).First();
                    var ret = System.Text.RegularExpressions.Regex.Replace(block.BlockText, @"\(\(SOT", "\r\n ...SOT  ");
                    ret = System.Text.RegularExpressions.Regex.Replace(ret, @"\(\([^\)]+\)\)", "\r\n ... \r\n");

                    var filename = block.Name;
                    var reg = new System.Text.RegularExpressions.Regex("[\\/:*'\",_&#^@]");
                    filename = reg.Replace(filename, string.Empty);

                    filename=System.IO.Path.Combine(settings.First().value, DateTime.Now.ToString("ddMMyyyy_HHmmss_") + filename + ".txt");
                
                    File.WriteAllText(filename, ret, Encoding.GetEncoding("windows-1251"));

                    context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { status = "file save to file: "+ filename }));

                }
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
    public class blockTextRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new blockText() { RequestContext = requestContext }; ;
        }
    }
}