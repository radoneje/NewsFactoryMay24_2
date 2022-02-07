using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
//using System.Web.Helpers;
using System.Text;

namespace WebApplication2.ServiceHandlers
{
    /// <summary>
    /// Summary description for ServiceHandler
    /// </summary>
    public class ServiceHandler : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
             byte[] buffer = new byte[1024];
             string instr = "";
             int read=context.Request.InputStream.Read(buffer, 0, buffer.Length);
                while(read>0)
                {
                    char[] chars = new char[read / sizeof(char)];
                    System.Buffer.BlockCopy(buffer, 0, chars, 0, read);
                    string tmp=new string(chars);
                    instr += tmp;
                    //instr += Encoding.Default.GetString(buffer);
                    read = context.Request.InputStream.Read(buffer, 0, buffer.Length);
                }
                dynamic inparam = Newtonsoft.Json.JsonConvert.DeserializeObject(instr);// Json.Decode(instr);

             if (inparam.Type== "VideoLRVEncoder")
             {
                 string FFMpegStr1Pass;
                 string FFMpegStr2Pass;
                 string FFMpegStrTh;

                 using(Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                 {
                     FFMpegStr1Pass = dc.vWeb_Settings.Where(s => s.id == 9).First().value;
                     FFMpegStr2Pass = dc.vWeb_Settings.Where(s => s.id == 12).First().value;
                     FFMpegStrTh = dc.vWeb_Settings.Where(s => s.id == 13).First().value;
                 }
                 if (inparam.Status == "Ready")
                 {




                     /* if (context.Application["EncoderStatus"] == null)
                     {
                         context.Application["EncoderStatus"] = new Dictionary<string, handlers.utils.EncodeStatus>();
                     }
                     Dictionary<string, handlers.utils.EncodeStatus> encsatus = (Dictionary<string, handlers.utils.EncodeStatus>)context.Application["EncoderStatus"];
                
                     if (!encsatus.ContainsKey(inparam.Guid))
                     {
                         encsatus.Add(inparam.Guid, new handlers.utils.EncodeStatus() { FileGuid = "a62dbd0df74d8518045958dc57baa65e", LastActive=DateTime.Now, IsComlite=false, Pass=1, Message="", Th="", IsThComplite=0, });
                     }

                     context.Application["EncoderStatus"] = encsatus;
                     if (context.Application["EncodeStack"] == null)
                     {
                         SayAwait(context);
                         return;
                     }
                     List<handlers.utils.EncodeStack> stack = (List<handlers.utils.EncodeStack>)context.Application["EncodeStack"];
                     if(stack.Count==0)
                     {
                         SayAwait(context);
                         return;
                     }
                     int FindItem = -1;
                     for (int i = 0; i < stack.Count; i++ )
                     {
                         if (stack[i].EncoderStatus != 0)
                             continue;
                         FindItem = i;
                     }
                     if (FindItem<0)
                     {
                         SayAwait(context);
                         return;
                     }
                     stack[FindItem].EncoderStatus=1;
                     stack[FindItem].Errorcount++;
                     stack[FindItem].EncoderGuid = inparam.Guid;
                     stack[FindItem].Message = "";*/

                     using(Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
                     {
                         int i=0;
                         while (((bool)context.Application["EncodeStackLook"]) && i<50)
                         {
                             i++;
                             System.Threading.Thread.Sleep(50);
                         }
                         context.Application["EncodeStackLook"] = true;
                         var stack = dc.vMedia_EncoderTasks.Where(e => e.IsActive == false && e.IsComplite==false && e.ErrorCount < 10).OrderBy(r => r.ErrorCount).OrderBy(t => t.DateLastStart);
                         if(stack.Count()==0)
                         {
                             SayAwait(context);
                             context.Application["EncodeStackLook"] = false; ;
                             return;
                         }
                         var task = stack.First();
                         dc.pMedia_EncoderSetTaskActive((int)task.id, (string)inparam.Guid);
                         var files=dc.vMedia_FileWIthFolderToLIsts.Where(f => f.FileGuid == task.FileGuid);
                         if(files.Count()==0)
                         {
                             SayAwait(context);
                             dc.pMedia_EncoderSetTaskDisActive((string)inparam.Guid);
                             context.Application["EncodeStackLook"] = false;
                             return;;
                         }
                         var mess = new
                         {
                             Message = "Hello",
                             To = inparam.Guid,
                             Command = "Encode",
                             Status = "ok",
                             Params = new
                             {
                                 Src = "/Media/File/" + files.First().FileGuid + "/Get",
                                 Dest = "/Api/",
                                 EncoderString = FFMpegStr1Pass,
                                 DestGuid = Guid.NewGuid().ToString(),
                                 EncoderThString = FFMpegStrTh,
                             }
                         };
                         context.Response.ContentType = "text/JSON";
                         context.Response.Write(/*Newtonsoft.Json.JsonConvert.SerializeObject*/Newtonsoft.Json.JsonConvert.SerializeObject(mess));
                         
                         context.Application["EncodeStackLook"] = false;
                         return;

                     } 
                 }
             }
            if (inparam.Type== "VideoLRVEncoderStatus")
            {
  

                if (inparam.Status == "EncodingEnded")
                {
                    string FFMpegStr2Pass;
                    using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                 {
                       
                    FFMpegStr2Pass = dc.vWeb_Settings.Where(s => s.id == 12).First().value;                   
                 }
                    using (Blocks.DataClassesMediaDataContext dc= new Blocks.DataClassesMediaDataContext())
                    {
                        string EncoderGuid=inparam.Guid;
                        var file = dc.vMedia_EncoderTasks.Where(e => e.IsActive == true && e.EncoderGuid == EncoderGuid).First();
                        if(file.LRVFileName==null || file.LRVPath==null)
                        {
                            dc.pMedia_EncoderSetTaskError(EncoderGuid);
                            SayAwait(context);
                            return;
                        }
                        string FilePath = System.IO.Path.Combine(file.LRVPath,file.LRVFileName );
                        if(!System.IO.File.Exists(FilePath))
                        {
                            dc.pMedia_EncoderSetTaskError(EncoderGuid);
                            SayAwait(context);

                            return;
                        }
                    }
                    context.Response.ContentType = "text/JSON";
                    var mess = new
                    {
                        Message = "Hello",
                        To = inparam.Guid,
                        Command = "Encode2Pass",
                        Status = "ok",
                        Params = new
                        {
                            Src = "",
                            Dest = "/Api/",
                            EncoderString = FFMpegStr2Pass,
                            DestGuid = Guid.NewGuid().ToString(),
                            EncoderThString = "",
                        }
                    };
                    context.Response.Write(/*Newtonsoft.Json.JsonConvert.SerializeObject*/Newtonsoft.Json.JsonConvert.SerializeObject(mess));
                    return;
                }
                else
                {
                   /* Dictionary<string, handlers.utils.EncodeStatus> encsatus = (Dictionary<string, handlers.utils.EncodeStatus>)context.Application["EncoderStatus"];
                    handlers.utils.EncodeStatus prm = encsatus[inparam.Guid];
                    prm.Message = encsatus[inparam.Guid].Message + "\r\n" + inparam.Status;
                    encsatus[inparam.Guid] = prm;
                    context.Application["EncoderStatus"] = encsatus;
                    List<handlers.utils.EncodeStack> stack1 = (List<handlers.utils.EncodeStack>)context.Application["EncodeStack"];
                    if (stack1.Count != 0)
                    {

                        int FindItem1 = -1;
                        for (int i = 0; i < stack1.Count; i++)
                        {
                            if (stack1[i].EncoderStatus != 0)
                                continue;
                            FindItem1 = i;
                        }
                        if (FindItem1 >= 0)
                        {

                            stack1[FindItem1].Message += "\r\n" + inparam.Status;
                            context.Application["EncodeStack"] = stack1;
                        }
                    }*/
                }
            }
           // context.Request.InputStream
            SayAwait( context);
        }
        protected void SayAwait(HttpContext context)
        {
            context.Response.ContentType = "text/JSON";
            var retmess = new { Message = "Hello", To = "", Command = "Await", Status = "Await", Params = "" };
            context.Response.Write(/*Newtonsoft.Json.JsonConvert.SerializeObject*/Newtonsoft.Json.JsonConvert.SerializeObject(retmess));
        }

        protected void ErrorMessage(string msg, HttpContext context)
        {
            context.Response.ContentType = "text/JSON";
            context.Response.Write(/*Newtonsoft.Json.JsonConvert.SerializeObject*/Newtonsoft.Json.JsonConvert.SerializeObject(new { Error = true, Command = "awalt", message = msg }));
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class ServiceRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ServiceHandler() { RequestContext = requestContext }; ;
        }
    }
}