using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.IO;

namespace WebApplication2.ServiceHandlers
{
    /// <summary>
    /// Summary description for LRVUploadHandler
    /// </summary>
    public class LRVUploadHandler : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            //Dictionary<string, handlers.utils.EncodeStatus> encsatus = (Dictionary<string, handlers.utils.EncodeStatus>)context.Application["EncoderStatus"];
            
            String EncoderGuid=(string)RequestContext.RouteData.Values["EncoderGuid"];
            //handlers.utils.EncodeStatus stat= encsatus[EncoderGuid];
            
            


            string FullFilePath="";
            string FullFileName="";
            string FileToSave="";
            using (Blocks.DataClassesMediaDataContext dc= new Blocks.DataClassesMediaDataContext())
            {
                var FileGuid = dc.vMedia_EncoderTasks.Where(e => e.EncoderGuid == EncoderGuid).First().FileGuid;
                var fl = dc.vMedia_FileWIthFolderToLIsts.Where(d => d.FileGuid == FileGuid).First();
                   FullFilePath =fl.FolderPAth;
                   FullFileName=fl.FileName;
                FileToSave=Path.Combine(FullFilePath, "LRV");
                FileToSave = Path.Combine(FileToSave, Path.GetFileNameWithoutExtension(FullFileName)  
                  + "_"+((string) RequestContext.RouteData.Values["EncoderPass"])
                + ".mp4");

                if(((string) RequestContext.RouteData.Values["EncoderPass"])=="1")
                {
                   System.Threading.Thread.Sleep(500);
                  
                       int ret = dc.pMedia_FilesLrvUpdate(EncoderGuid, Path.GetFileName(FileToSave), Path.GetDirectoryName(FileToSave), ((string)RequestContext.RouteData.Values["EncoderPass"]) == "2", true);
                       //ret = ret;
                   
                    System.Threading.Thread.Sleep(500);
                }
            }

            try
            {
                Directory.CreateDirectory(FileToSave);
            }
            catch { };

            try
            {
                using (FileStream fs = new System.IO.FileStream(FileToSave, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    int I = 0;
                    Byte[] buffer = new Byte[32 * 1024];
                    int read = context.Request.GetBufferlessInputStream().Read(buffer, 0, buffer.Length);
                    while (read > 0)
                    {
                        I += read;
                        fs.Write(buffer, 0, read);
                        read = context.Request.GetBufferlessInputStream().Read(buffer, 0, buffer.Length);
                        //System.Threading.Thread.Sleep(50);
                    }
                    fs.Close();

                }
            }
            catch(Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("Error in file uploaded"  + ex.Message);
            }
            if (((string)RequestContext.RouteData.Values["EncoderPass"]) == "2")
            {
                using (Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
                {
                    dc.pMedia_FilesLrvUpdate(EncoderGuid, Path.GetFileName(FileToSave), Path.GetDirectoryName(FileToSave), ((string)RequestContext.RouteData.Values["EncoderPass"]) == "2", false);
                   
                }


            }

            context.Response.ContentType = "text/plain";
            context.Response.Write("File is uploaded");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class LRVUploadRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new LRVUploadHandler() { RequestContext = requestContext }; ;
        }
    }
}