using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebApplication2.API
{
    /// <summary>
    /// Summary description for BlocksUndelete
    /// </summary>
    public class BlocksUndelete : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            string ret = "";
            try
            {
               

                if (RequestContext.RouteData.Values["blockId"] == null)
                {
                    ret = "no params";
                    context.Response.StatusCode = 500;
                }
                else
                {
                    using(WebApplication2.Blocks.DataClasses1DataContext dc = new WebApplication2.Blocks.DataClasses1DataContext())
                    {
                        dc.ExecuteCommand("UPDATE blocks SET Deleted=0 WHERE id=" + RequestContext.RouteData.Values["blockId"]);
                        ret = "ok";
                    }
                }
            }
            catch(Exception ex)
            {

            }
            context.Response.ContentType = "application/json";
            context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { ret = ret }));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class BlockUndelRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new BlocksUndelete() { RequestContext = requestContext };
        }
    }
}