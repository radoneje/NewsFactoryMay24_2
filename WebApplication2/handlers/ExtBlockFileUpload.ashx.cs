using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;


namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for ExtBlockFileUpload
    /// </summary>
    public class ExtBlockFileUpload : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
          try
            {
            string PATH ="";
            DateTime StartDate = DateTime.Now;
            Int64 I = 0;
            string Filename = (string)context.Server.UrlDecode((string)context.Request.Headers["Origin-Filename"]);
            //string Filename = context.Server.UrlDecode((string)RequestContext.RouteData.Values["FileName"]);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                PATH = dc.vWeb_Settings.Where(d => d.id == 1).First().value;
                PATH = System.IO.Path.Combine(PATH, DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM_yyyy"), DateTime.Now.ToString("dd_MM_yyyy"), (string)RequestContext.RouteData.Values["FileGuid"]);

                if (!System.IO.Directory.Exists(PATH))
                    System.IO.Directory.CreateDirectory(PATH);
             //  testservice.log(context.Session["UserId"],)
               
            }
            
            string FilePath=System.IO.Path.Combine(PATH,Filename);
            string sBlockId = (string)context.Request.Headers["Origin-BlockId"];
            string sBlockGuid=null;
              if(sBlockId.IndexOf("BlockGiud:")>-1)
              {
                  sBlockGuid = sBlockId.Replace("BlockGiud:", "");
                  sBlockId = null; 

              }
             
            string ContentRange= (string)context.Request.Headers["Content-Range"];
                
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"bytes\s(\d+)\-(\d+)\/(\d+)");
            System.Text.RegularExpressions.Match  mc = reg.Match(ContentRange);
            if(!mc.Success)
            {
                context.Response.StatusCode = 505;
                context.Response.ContentType = "text/plain";
                context.Response.Write("Cant seek Content-Range \r\n" + ContentRange);
                context.Response.End();
                return;
            }
            Int64 startbyte = Convert.ToInt64(mc.Groups[1].Value);
            Int64 endbyte = Convert.ToInt64(mc.Groups[2].Value);
            Int64 tolalbytes = Convert.ToInt64(mc.Groups[3].Value);
            Int64 tChunkId = 0;
            string FolderGuid=(string)(RequestContext.RouteData.Values["FolderGuid"]);
            string FileGuid=(string)(RequestContext.RouteData.Values["FileGuid"]);

            /////////вставляем инфу о чанке в память
            int BlockType = 0;
            string mime = MimeExtensionHelper.GetMimeType(Filename);
            if (mime.IndexOf("image") == 0)
            {
                BlockType = 1;
            }
            if (mime.IndexOf("video") == 0)
            {
                BlockType = 2;
            }
            int j=0;
            //System.Threading.Interlocked.Exchange(ref context.Application["MediaFoldersLookAtom1"], 0);
            while ((bool)context.Application["MediaFoldersLook"])
            {
                j++;
                System.Threading.Thread.Sleep(50);
                if (j > 100)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = false, FileName = RequestContext.RouteData.Values["FileName"], bytes = I, Message = "Loocking error" }));
                    context.Application["MediaFoldersLook"] = false;
                }
            }
            context.Application.Lock();
            context.Application["MediaFoldersLook"] = true;
            context.Application.UnLock();
            using(Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
            {
                dc.pMedia_FoldersAdd(FolderGuid, false, PATH, Convert.ToInt32(RequestContext.RouteData.Values["FolderCount"]));
                dc.pMedia_FilesAdd(FileGuid, FolderGuid, Filename, false, Convert.ToInt32(RequestContext.RouteData.Values["FileNumber"]), tolalbytes, context.Request.UserHostAddress + " " + context.Request.UserHostName, DateTime.Now, BlockType);
            }
            context.Application.Lock();
            context.Application["MediaFoldersLook"] = false;
            context.Application.UnLock();
            utils.MediaChunks Chunk = new utils.MediaChunks()
            {
                iNumberInFile = Convert.ToInt32(RequestContext.RouteData.Values["ChunkNumber"]),
                EndByte = endbyte,
                StartByte = startbyte,
                BytesWrite = 0,
            };

            
                bool newfile = false;
                System.IO.Stream fs;
                if (!System.IO.File.Exists(FilePath))
                {
                    fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);//, bufferSize: 10 * 1024, useAsync: true);
                    fs.SetLength(tolalbytes);
                    newfile = true;
                }
                else
                    fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Open, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);//, bufferSize: 10 * 1024, useAsync: true);

                if (startbyte > 0)
                    fs.Seek(startbyte, System.IO.SeekOrigin.Begin);


                Byte[] buffer = new Byte[1024 * 1024];
                int read = context.Request.GetBufferlessInputStream().Read(buffer, 0, buffer.Length);
                while (read > 0)
                {
                    I += read;
                    Chunk.BytesWrite += read;
                    fs.Write(buffer, 0, read);
                    read = context.Request.GetBufferlessInputStream().Read(buffer, 0, buffer.Length);
                    //System.Threading.Thread.Sleep(50);
                }
                fs.Close();
                Chunk.bReady = Chunk.EndByte - Chunk.StartByte == Chunk.BytesWrite;

                if (!Chunk.bReady)
                {
                    ReturnError(context, "Not compite chank received: " + Chunk.StartByte.ToString() + "-" + Chunk.EndByte.ToString() + "!=" + Chunk.BytesWrite.ToString());
                    return;
                }
                while ((bool)context.Application["MediaFoldersLook"])
                {
                    System.Threading.Thread.Sleep(50);
                    if (j > 100)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = false, FileName = RequestContext.RouteData.Values["FileName"], bytes = I, Message = "Loocking error" }));
                        context.Application["MediaFoldersLook"] = false;
                    }
                }
                context.Application.Lock();
                context.Application["MediaFoldersLook"] = true;
                context.Application.UnLock();
                using (Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
                {
                    dc.pMedia_ChunkAdd(Chunk.StartByte, Chunk.EndByte, Chunk.BytesWrite, FileGuid);
                    dc.pMedia_FilesUpdate(FileGuid, Chunk.BytesWrite, DateTime.Now);

                }
                context.Application.Lock();
               context.Application["MediaFoldersLook"] = false;
               context.Application.UnLock();
  
                if(BlockType==1)
                {
                    try
                    {
                        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(FilePath);
                        int thHeight = Convert.ToInt32( ((float)((float)64 /(float) bm.Width)) * (float)bm.Height);
                        System.Drawing.Bitmap bm1 = new System.Drawing.Bitmap((System.Drawing.Image)bm, new System.Drawing.Size(640, thHeight));
                        thHeight = Convert.ToInt32(((float)((float)32 / (float)bm.Width)) * (float)bm.Height);
                        System.Drawing.Bitmap bm2 = new System.Drawing.Bitmap((System.Drawing.Image)bm, new System.Drawing.Size(32, thHeight));

                        System.IO.Stream st1 = new System.IO.MemoryStream();
                        System.IO.Stream st2 = new System.IO.MemoryStream();

                        ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                        System.Drawing.Imaging.Encoder myEncoder =
                            System.Drawing.Imaging.Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        bm1.Save(st1, jgpEncoder, myEncoderParameters);
                        bm2.Save(st2, jgpEncoder, myEncoderParameters);

                        st1.Seek(0, System.IO.SeekOrigin.Begin);
                        st2.Seek(0, System.IO.SeekOrigin.Begin);
                        byte[] bt1 = new byte[st1.Length];
                        st1.Read(bt1, 0, bt1.Length);
                        byte[] bt2 = new byte[st2.Length];
                        st2.Read(bt2, 0, bt2.Length);

                        using (Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
                        {
                            dc.pMedia_ThumbnailInsertFromImage(FileGuid, new System.Data.Linq.Binary(bt1), new System.Data.Linq.Binary(bt1), new System.Data.Linq.Binary(bt2));

                        }
                    }
                    catch(Exception ex)
                    {

                    }

                }

                context.Response.ContentType = "text/plain";
                context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = true, FileName = RequestContext.RouteData.Values["FileName"], bytes = I, Message = ""}));
               
            }
            catch(Exception ex )
            {
                ReturnError(context, ex.Message);
            }
            
        }
        private void ReturnError(HttpContext context, string msg)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = false, FileName = RequestContext.RouteData.Values["FileName"], bytes = 0, Message = msg }));
               
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
    public class ExtBlockFileUploadRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ExtBlockFileUpload() { RequestContext = requestContext }; ;
        }
    }
}