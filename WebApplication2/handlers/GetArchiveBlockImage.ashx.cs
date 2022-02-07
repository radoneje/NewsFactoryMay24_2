using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;


namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for GetImage
    /// </summary>
    public class GetArchiveBlockImage : IHttpHandler
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
                GetBlockImage.GetNoImage(context);
                return;
            }
            Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext();
            Int64 BlockId = 0;
            string filename;
            try
            {
                if (context.Request.Params["MediaId"] != null)
                    BlockId = Convert.ToInt64(context.Request.Params["MediaId"]);
                else
                    BlockId = Convert.ToInt64(context.Request.Params["BlockId"]);


                if (context.Request.Params["MediaId"] != null)
                    filename = GetMediaImageFileName(BlockId, dc);
                else
                    filename = GetImageFileName(BlockId, dc);

                if (!System.IO.File.Exists(filename))
                {
                    GetNoImage(context);
                    return;
                }
            }
            catch (Exception ex)
            {
                GetNoImage(context);
                return;
            }
            utils.Streaming(context, filename, System.Web.MimeMapping.GetMimeMapping(filename));
        }
        private void GetNoImage(HttpContext context)
        {
            utils.Streaming(context, context.Server.MapPath("../Images/noimage.jpg"), "image/jpeg");
        }
        private string GetMediaImageFileName(Int64 MediaId, Blocks.DataClassesMediaDataContext dc)
        {
            return dc.vWeb_ArchiveMediaForLists.Where(f => f.Id == MediaId).First().TextLang2;

        }
        private string GetImageFileName(Int64 BlockId, Blocks.DataClassesMediaDataContext dc)
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
                    filename = bl.TextLang2;
                    break;
                }
            }
            return filename;
        }

    }
}