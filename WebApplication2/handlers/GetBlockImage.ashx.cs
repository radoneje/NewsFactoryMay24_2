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
    public class GetBlockImage : IHttpHandler
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
           
            
            if (context.Request.Params["BlockId"]==null && context.Request.Params["MediaId"]==null)
            { 
                GetNoImage(context);
                return;
            }
            Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext();
            Int64 BlockId = 0;
            string filename;
            try{
                if (context.Request.Params["MediaId"] != null)
                    BlockId = Convert.ToInt64(context.Request.Params["MediaId"]);
                else
                    BlockId = Convert.ToInt64(context.Request.Params["BlockId"]);
           
            
            if(context.Request.Params["MediaId"]!=null)
                filename = GetMediaImageFileName(BlockId, dc);
            else
                filename = GetImageFileName(BlockId, dc);

            if (!System.IO.File.Exists(filename))
            {
                GetNoImage(context);
                return;
            } 
            }
            catch(Exception ex) {
                GetNoImage(context);
                return;
            }
           // if (context.Request.Params["MediaId"] != null)
                utils.Streaming(context, filename, System.Web.MimeMapping.GetMimeMapping(filename));
           // else
            //    streamingMediaImageWithStatus(context, BlockId, dc);
        }
        public static void GetNoImage(HttpContext context)
        {
            utils.Streaming(context, context.Server.MapPath("../Images/noimage.jpg"), "image/jpeg");
        }
        private string GetMediaImageFileName(Int64 MediaId, Blocks.DataClassesMediaDataContext dc)
        {
            return dc.vWeb_MediaForLists.Where(f => f.Id == MediaId).First().TextLang2;

        }
        private void streamingMediaImageWithStatus(HttpContext context, Int64 BlockId, Blocks.DataClassesMediaDataContext dc)
        {
            var rec = dc.vWeb_MediaForLists.Where(f => f.ParentId == BlockId).OrderBy(f => f.Sort).First();
            var filename = rec.TextLang2;
            var status =( rec.Ready ? 1 : 0) +( rec.Approve ? 1 : 0);

            var img=System.Drawing.Bitmap.FromFile(filename);
            var sign =new System.Drawing.Bitmap(img);
           // var sign = new System.Drawing.Bitmap(20, 20);

            var color= new System.Drawing.Color();
            switch(status){
                case 1: color=System.Drawing.Color.Yellow;break;
                case 2:color=System.Drawing.Color.Green;break;
                default:color=System.Drawing.Color.Red;break;
            }
            var brush = new System.Drawing.SolidBrush(color);
            var x = 50;
            var y = 50;

            for(int xx=sign.Width-x-10; xx<sign.Width-10; xx++)
            {
                 for(int yy=sign.Height-y-10; yy<sign.Height-10; yy++)
                 {
                     sign.SetPixel(xx, yy, color);
                 }

            }
            var stream= new System.IO.MemoryStream();
            sign.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

            context.Response.ContentType = "image/jpeg";
            stream.CopyTo(context.Response.OutputStream);
            context.Response.End();
           

        }
        private string GetImageFileName(Int64 BlockId, Blocks.DataClassesMediaDataContext dc)
        {
            var a = dc.vWeb_MediaForLists.Where(f => f.ParentId == BlockId ).OrderBy(f=>f.Sort );
            if (a.Count() == 0)
            {
               return "";
            }
            string filename = "";
            foreach(var bl in a)
            {
                   if(System.IO.File.Exists(bl.TextLang2))
                   {
                       filename = bl.TextLang2;
                       break;
                   }
            }
            return filename;
        }
      
    }
}