using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for GetBlockVideo
    /// </summary>
    public class GetArchiveBlockVideo : IHttpHandler
    {

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public void ProcessRequest(HttpContext context)
        {


            if (context.Request.Params["BlockId"] == null && context.Request.Params["MediaId"] == null)
            {
                GetBlockVideo.GetNoVideo(context);
                return;
            }
            Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext();
            Int64 BlockId = 0;

            try
            {
                if (context.Request.Params["MediaId"] != null)
                    BlockId = Convert.ToInt64(context.Request.Params["MediaId"]);
                else
                    BlockId = Convert.ToInt64(context.Request.Params["BlockId"]);
            }
            catch (Exception ex)
            {
                GetBlockVideo.GetNoVideo(context);
                return;
            }
            string filename;
            if (context.Request.Params["MediaId"] != null)
                filename = GetMediaVideoFileName(BlockId, dc);
            else
                filename = GetVideoFileName(BlockId, dc);



            if (filename.Length < 5)
            {
                GetBlockVideo.GetNoVideo(context);
                return;
            }
            utils.Streaming(context, filename, System.Web.MimeMapping.GetMimeMapping(filename));
        }

        private string GetVideoFileName(Int64 BlockId, Blocks.DataClassesMediaDataContext dc)
        {
            var a = dc.vWeb_ArchiveMediaForLists.Where(f => f.ParentId == BlockId).OrderBy(f => f.Sort);
            if (a.Count() == 0)
            {
                return "";
            }
            string filename = "";
            foreach (var bl in a)
            {
                if (System.IO.File.Exists(bl.TextLang2))
                {
                    filename = bl.TextLang1;
                    break;
                }
            }
            return filename;
        }
        private string GetMediaVideoFileName(Int64 MediaId, Blocks.DataClassesMediaDataContext dc)
        {
            try
            {
                return dc.vWeb_ArchiveMediaForLists.Where(f => f.Id == MediaId).First().TextLang1;
            }
            catch
            {
                return "";
            }
            return "";
        }
    }
}