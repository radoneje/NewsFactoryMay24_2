using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;



namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            using (FileStream fs = new System.IO.FileStream("c:\\tmp\\2.webm", FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                int I = 0;
                Byte[] buffer = new Byte[32 * 1024];
                int read = context.Request.GetBufferlessInputStream().Read(buffer, 0, buffer.Length);
                while (read > 0)
                {
                    I += read;
                    fs.Write(buffer, 0, read);
                    read = context.Request.GetBufferlessInputStream().Read(buffer, 0, buffer.Length);
                    System.Threading.Thread.Sleep(50);
                }
                fs.Close();
                
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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