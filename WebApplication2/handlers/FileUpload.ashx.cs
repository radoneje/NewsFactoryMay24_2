using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Http;
using System.Net;

using System.Threading.Tasks;
//using System.Web.Http;
//using System.Net;

//using WebApi.Html5.Upload.Models;
//using WebApi.Html5.Upload.Infrastructure;

namespace WebApplication2.handlers
{
    


    /// <summary>
    /// Summary description for FileUpload
    /// </summary>
    public class FileUpload : IHttpHandler
    {
        
        public void ProcessRequest(HttpContext context)
        {

           
            context.Response.ContentType = "text/plain";


            if (context.Request.QueryString["BlockId"] == null)
            {
                context.Response.StatusCode = 400;
                context.Response.Write("No BlockId");
                context.Response.End();
                return;
            }
            if (context.Request.QueryString["ContainerId"] == null)
            {
                context.Response.StatusCode = 400;
                context.Response.Write("No ContainerId");
                context.Response.End();
                return;
            }
            if (context.Request.QueryString["FileName"] == null)
            {
                context.Response.StatusCode = 400;
                context.Response.Write("No FileName");
                context.Response.End();
                return;
            }

            try {
                string FileToSave = String.Empty;
                string FileName = context.Server.UrlDecode(context.Request.Params["FileName"].ToString());
                int TaskId = 0;
                using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                {
                    try
                    {
                        FileToSave = dc.vWeb_Settings.Where(d => d.id == 1).First().value;
                        FileToSave = Path.Combine(FileToSave, DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM_yyyy"), DateTime.Now.ToString("dd_MM_yyyy"));
                       
                       
                            if (!Directory.Exists(FileToSave))
                                Directory.CreateDirectory(FileToSave);
             

                        FileToSave = Path.Combine(FileToSave, FileName);

                       
                            if (File.Exists(FileToSave))
                            {
                                FileToSave = System.IO.Path.GetDirectoryName(FileToSave) + "\\" + System.IO.Path.GetFileNameWithoutExtension(FileToSave) + "_" + utils.ToUnixTimestamp(DateTime.Now) + System.IO.Path.GetExtension(FileToSave);
                            }
                       

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

                        lock (context.Application["directory"])
                        {
                            TaskId = dc.pWeb_InsertMediaToBlock(Convert.ToInt64(context.Request.QueryString["BlockId"].ToString()), FileToSave, FileName, BlockType);
                        }
                             if (TaskId < 0)
                        {
                            context.Response.Write("Cant insert new media");
                            context.Response.StatusCode = 505;
                            context.Response.End();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {

                        context.Response.Write("Error get file param " + ex.Message);
                        context.Response.Write("filename " + FileName);
                        context.Response.StatusCode = 500;
                        context.Response.End();
                        return;
                    }

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
                            lock (context.Application["directory"])
                            {
                                dc.pMedia_SetTaskUploadComplite(TaskId);
                            }
                            NFSocket.SendToAll.SendData("mediaUploadComplite", new { blockId = context.Request.QueryString["BlockId"] });
                        }
                    }

                    catch (Exception ex)
                    {

                        context.Response.Write("Error open file for write " + ex.Message);
                        context.Response.Write("filename " + FileName);
                        context.Response.StatusCode = 500;
                        context.Response.End();
                        return;
                    }
                }
                
            }
            catch(Exception ex)
            {
                
                context.Response.Write("Error save file 1"+ ex.Message );
                context.Response.StatusCode = 500;
                context.Response.End();
                return;
            }
            var ret= new {ContainerId=context.Request.Params["ContainerId"], BlockId=context.Request.Params["BlockId"]};
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            context.Response.StatusCode = 201;
            context.Response.Write(js.Serialize(ret));
            context.Response.End();

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