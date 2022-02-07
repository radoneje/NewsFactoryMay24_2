using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for SynhTemplateGet
    /// </summary>
    public class SynhTemplateGet : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            List<testservice.SyncTemplate> items = new List<testservice.SyncTemplate>();
       
            using(Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.t_SynchTemplates.Where(q => q.Deleted == false).OrderBy(qq=>qq.Name).ToList().ForEach(l => {
                    items.Add(new testservice.SyncTemplate()
                    {
                        id = l.id,
                        name = l.Name,
                        cap = l.Caption
                    });
                });
            }
           
            context.Response.ContentType = "application/json";
            context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { items = items }));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}