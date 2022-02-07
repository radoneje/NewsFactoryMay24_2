using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for GetMediaSourceFIle
    /// </summary>
    public class GetMediaSourceFIle : IHttpHandler
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
                GetNoFile(context);
                return;
            }
            string filename;
            
            try
            {
                filename = dc.vWeb_MediaForLists.Where(f => f.Id ==Convert.ToInt64( BlockId)).First().BlockText;
            }
            catch
            {
               GetNoFile(context);
                return;
            }


            if (filename.Length < 5)
            {
                GetNoFile(context);
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
        private void GetNoFile(HttpContext context)
        {
            context.Response.StatusCode = 404;
            context.Response.Write("Error: no file found");
            context.Response.End();


        }
    }
}