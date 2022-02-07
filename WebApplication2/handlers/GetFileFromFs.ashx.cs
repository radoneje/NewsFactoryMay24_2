using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.IO;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for GetFile
    /// </summary>
    public class GetFileFromFs : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            
            
            if (context.Request.QueryString["path"] == null )
            { ErrorExit("path", context); return; }
           
             var filename = System.Web.HttpUtility.UrlDecode((string)context.Request.QueryString["path"]);
            if (!System.IO.File.Exists(filename))
                NotFoundExit(filename + " not found", context);
            else
            {
                utils.Streaming(context, filename, "application/octet-stream");
                    /* var FileInfo = new System.IO.FileInfo(filename);
                     context.Response.Buffer = false;
                     context.Response.Clear();
                     context.Response.ClearHeaders();

                     context.Response.AppendHeader("Content-Length", FileInfo.Length.ToString());
                     context.Response.WriteFile(filename);
                     context.Response.End();*/

            }




        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private void ErrorExit(string message, HttpContext context)
        {
            
           context.Response.ContentType = "text/plain";
           context.Response.Write(message);

        }
        private void  NotFoundExit(string message, HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(message);
        }
    }
    public class GetFileFromFsRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new GetFileFromFs() { RequestContext = requestContext }; ;
        }
    }
}