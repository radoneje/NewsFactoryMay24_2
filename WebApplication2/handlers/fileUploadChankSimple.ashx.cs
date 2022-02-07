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
    public class fileUploadChankSimple : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public  void ProcessRequest(HttpContext context)
        {
            int TaskId = 0;
            var headers = RequestContext.HttpContext.Request.Headers;
            string FileToSave = String.Empty;
            string FileName = context.Server.UrlDecode(headers["x-filename"]);

            if (headers["x-blockId"] == null)
            {
                context.Response.StatusCode = 400;
                context.Response.Write("No BlockId");
                context.Response.End();
                return;
            }
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                 TaskId = Convert.ToInt32( headers["x-taskId"].ToString());
                FileToSave = dc.vWeb_Settings.Where(d => d.id == 1).First().value;
                FileToSave = Path.Combine(FileToSave, DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM_yyyy"), DateTime.Now.ToString("dd_MM_yyyy"));


                if (!Directory.Exists(FileToSave))
                    Directory.CreateDirectory(FileToSave);


                FileToSave = Path.Combine(FileToSave, FileName);
                if (File.Exists(FileToSave) && headers["x-firstChank"].ToString()=="1")
                {
                    FileToSave = System.IO.Path.GetDirectoryName(FileToSave) + "\\" + System.IO.Path.GetFileNameWithoutExtension(FileToSave) + "_" + utils.ToUnixTimestamp(DateTime.Now) + System.IO.Path.GetExtension(FileToSave);
                }
                if (headers["x-firstChank"].ToString() == "1")
                {
                    int BlockType = 0;
                    string mime = System.Web.MimeMapping.GetMimeMapping(FileName);
                    if (mime.IndexOf("image") == 0)
                    {
                        BlockType = 1;
                    }
                    if (mime.IndexOf("video") == 0)
                    {
                        BlockType = 2;
                    }
                    var f = (string)RequestContext.HttpContext.Application["directory"];
                    var i = 0;
                    bool ready = false;
                    while (i < 10 && ready == false)
                    {
                        i++;
                        try
                        {
                            lock (RequestContext.HttpContext.Application["directory"])
                            {
                                TaskId = dc.pWeb_InsertMediaToBlock(Convert.ToInt64(headers["x-blockId"]), FileToSave, FileName, BlockType);
                            }
                            ready = true;
                        }
                        catch (Exception ex)
                        {
                            ready = false;
                            System.Threading.Thread.Sleep(100);
                        }

                    }
                    if (i >= 10 && ready == false)
                    {
                        context.Response.Write("Error dc.pWeb_InsertMediaToBlock");
                        context.Response.Write("filename " + FileName);
                        context.Response.Headers.Add("x-error", context.Server.UrlEncode("dc.pWeb_InsertMediaToBlock"));
                        context.Response.StatusCode = 505;
                        context.Response.Flush();
                        context.Response.End();
                        return;
                    }
                    if (TaskId < 0)
                    {
                        context.Response.Write("Cant insert new media");
                        context.Response.StatusCode = 505;
                        context.Response.End();
                        return;
                    }
                }

                try
                {
                    using (FileStream fs = new System.IO.FileStream(FileToSave, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                    {
                        fs.Seek(fs.Length, SeekOrigin.Begin);
                        int I = 0;
                        Byte[] buffer = new Byte[32 * 1024 * 1024];
                        //  System.IO.File.AppendAllText("c:\\NFUploadLog\\1.txt", context.Server.UrlDecode(headers["x-filename"]) + " readStart \r\n");
                        using (var stream = context.Request.GetBufferlessInputStream())
                        {
                            int read = stream.Read(buffer, 0, buffer.Length);
                            while (read > 0)
                            {
                                I += read;
                                fs.Write(buffer,0, read);
                                read = stream.Read(buffer, 0, buffer.Length);
                                //System.IO.File.AppendAllText("c:\\NFUploadLog\\1.txt", context.Server.UrlDecode(headers["x-firstChank"]) + " last? " + context.Server.UrlDecode(headers["x-lastChank"]) + " " + context.Server.UrlDecode(headers["x-filename"]) + " read: " + read.ToString() + "\r\n");
                            }
                        }
                    if (headers["x-lastChank"].ToString() == "1")
                        {

                            int i = 0;
                            bool ready = false;
                            while (i < 10 && ready == false)
                            {
                                i++;
                                try
                                {
                                    lock (RequestContext.HttpContext.Application["directory"])
                                    {
                                        dc.pMedia_SetTaskUploadComplite(TaskId);
                                    }
                                    ready = true;
                                }
                                catch (Exception ex)
                                {
                                    ready = false;
                                    System.Threading.Thread.Sleep(100);
                                }

                            }
                            if (i >= 10 && ready == false)
                            {
                                context.Response.Write("Error dc.pMedia_SetTaskUploadComplite ");
                                context.Response.Write("filename " + FileName);
                                context.Response.Headers.Add("x-error", context.Server.UrlEncode("Error dc.pMedia_SetTaskUploadComplite "));
                                context.Response.StatusCode = 505;
                                context.Response.Flush();
                                context.Response.End();
                                return;
                            }
                            NFSocket.SendToAll.SendData("mediaUploadComplite", new { blockId = headers["x-blockId"] });

                        }

                    }
                }
                catch (Exception e) {
                    context.Response.Write("chank receive error "+e.Message);
                }


            }
            var ret = Newtonsoft.Json.JsonConvert.SerializeObject(new { taskId = TaskId, fileName = System.IO.Path.GetFileName(FileToSave) });
                context.Response.Write(ret);
            context.Response.StatusCode = 200;
            context.Response.End();
            return;
           

  
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
    public class fileUploadChankSimpleRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new fileUploadChankSimple() { RequestContext = requestContext }; ;
        }
    }
}