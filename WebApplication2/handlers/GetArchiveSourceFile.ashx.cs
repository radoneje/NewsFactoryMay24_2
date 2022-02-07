using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for GetMediaSourceFIle
    /// </summary>
    public class GetArchiveSourceFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            if (context.Request.Params["MediaId"] == null)
            {
                GetNoFile(context);
                return;
            }
            Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext();
            Int64 BlockId = 0;

            try
            {
                    BlockId = Convert.ToInt64(context.Request.Params["MediaId"]);
                
            }
            catch (Exception ex)
            {
                GetNoFile(context, "no media id");
                return;
            }
            string filename;
            
            try
            { 
                filename = dc.vWeb_ArchiveMediaForLists.Where(f => f.Id == BlockId).First().BlockText;
            }
            catch
            {
               GetNoFile(context, "no filename");
                return;
            }


            if (filename.Length < 5)
            {
                GetNoFile(context, "filename is empty");
                return;
            }

            utils.Streaming(context, filename, "application/octet-stream");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private void GetNoFile(HttpContext context, string message="")
        {
            context.Response.StatusCode = 404;
            context.Response.Write("Error: no file found " + message);
            context.Response.End();


        }
    }
}