using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for fileUploadGetNewChunk
    /// </summary>
    public class fileUploadGetNewChunk : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Int64 count = 0;
            string message = "";
            string fileId = (string)context.Request.Headers["Origin-fileId"];
            context.Response.ContentType = "application/json";
            try
            {
                string ContentRange = (string)context.Request.Headers["Content-Range"];

                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"bytes\s(\d+)\-(\d+)\/(\d+)");
                System.Text.RegularExpressions.Match mc = reg.Match(ContentRange);
                if (!mc.Success)
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


            
                var lst = (fileUpload.UploadInfo)context.Application["fileUpload"];
                string filePath="";
                lock (context.Application["fileUpload"])
                {
                 filePath= lst.getFilePath(fileId);
                }
                using (var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite))
                {
                    if (startbyte > 0)
                        fs.Seek(startbyte, System.IO.SeekOrigin.Begin);

                    Byte[] buffer = new Byte[1024];
                    int read = context.Request.GetBufferlessInputStream().Read(buffer, 0, buffer.Length);
                    while (read > 0)
                    {
                        count += read;
                        fs.Write(buffer, 0, read);
                        read = context.Request.GetBufferlessInputStream().Read(buffer, 0, buffer.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { fileId = fileId, bytes = count, message=message }));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}