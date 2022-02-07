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
    public class GetFile : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            
            
            if (((string)RequestContext.RouteData.Values["FileGuid"])==null )
            { ErrorExit("noGuid", context); return; }

           

            if (((string)RequestContext.RouteData.Values["FileGuid"]).Length<10)
            { ErrorExit("Guid is not correct ", context); return; }
           
            using(Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
            {

               // var chunks=dc.vMedia_FIlesWidthChunksGets.Where(f => f.FileGuid == (string)RequestContext.RouteData.Values["FileGuid"]).OrderBy(s=>s.StartByte);
                var files = dc.vMedia_FileWIthFolderToLIsts.Where(f => f.FileGuid == (string)RequestContext.RouteData.Values["FileGuid"]);
                if (files.Count() == 0)
                {
                    NotFoundExit("No FileFound, GUID: " + (string)RequestContext.RouteData.Values["FileGuid"], context); return;
                }

                if (files.First().Ready)
                {
                    string FileName = Path.Combine(files.First().FolderPAth, files.First().FileName);
                    utils.Streaming(context,FileName , System.Web.MimeMapping.GetMimeMapping(FileName));
                    return;
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write("found files: " + files.Count().ToString());
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
    public class GetFileRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new GetFile() { RequestContext = requestContext }; ;
        }
    }
}