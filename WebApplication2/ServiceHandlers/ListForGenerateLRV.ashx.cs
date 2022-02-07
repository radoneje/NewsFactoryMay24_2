using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebApplication2.ServiceHandlers
{
    /// <summary>
    /// Summary description for getServiceConfig
    /// </summary>
    public class ListForGenerateLRV : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            using (Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
            {
                lock (RequestContext.HttpContext.Application["EncodeTaskLook"])
                {
                    if (dc.vMedia_ListForGenerateLRVs.Count() > 0)
                    {
                        if (dc.vMedia_ListForGenerateLRVs.Count() > 0)
                        {
                            var count = 0;
                            using (var dc1 = new Blocks.DataClasses1DataContext())
                            {
                                if (dc1.tWeb_Settings.Where(s => s.Key == "LRVthreads").Count() == 0)
                                {
                                    dc1.tWeb_Settings.InsertOnSubmit(new Blocks.tWeb_Setting() { Key = "LRVthreads", value = "4", Description = "LRVthreads count" });
                                    dc1.SubmitChanges();
                                }
                                count = Convert.ToInt32(dc1.tWeb_Settings.Where(s => s.Key == "LRVthreads").First().value);
                            }
                            var items = dc.vMedia_ListForGenerateLRVs.Take(count);

                            context.Response.Write(System.Web.Helpers.Json.Encode(new { status = "ok", items = items }));

                        }

                    }
                    else
                    {
                        context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = "-1" }));
                        context.Response.Flush();
                        context.Response.End();
                    }
                }
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
    public class ListForGenerateLRVRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ListForGenerateLRV() { RequestContext = requestContext }; ;
        }
    }
}