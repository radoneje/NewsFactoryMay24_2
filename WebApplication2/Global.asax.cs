using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using WebApplication2;

namespace WebApplication2
{
    public class Global : HttpApplication
    {
        void Session_Start(object sender, EventArgs e)
        {
            Application["Sessions"] = (int)Application["Sessions"] + 1;
        }
        void Session_End(object sender, EventArgs e)
        {
            Application["Sessions"] = (int)Application["Sessions"] - 1;
        }

        void Application_Start(object sender, EventArgs e)
        {
            Application["Sessions"] = 0;

          /*  System.Web.Http.GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(
    new System.Net.Http.Formatting.QueryStringMapping("type", "json", new System.Net.Http.Headers.MediaTypeHeaderValue("application/json")));
*/
            // Code that runs on application startup
            RegisterRoutes(RouteTable.Routes);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Application["MediaFoldersLookAtom"] = 1;
            Application["MediaFoldersLook"] = false;
            Application["EncodeStackLook"] = false;
            Application["EncodeTaskLook"] = false;

            Application["sockloock"] = true;
            Application["sock"] = new NFSocket.CSocket(Application["sockloock"]);
            NFSocket.SendToAll.sock = (NFSocket.CSocket)Application["sock"];
            Application["directory"] = "lock files";
            Application["fileUpload"] = new fileUpload.UploadInfo();
            Application["title"] = new  Dictionary<long, Dictionary<string, string>>();

            System.Threading.Thread RssUpdateThread = new System.Threading.Thread(RightPanel.RssNews.ImportRssFeeds);
            new System.Threading.Thread(() => App_Start.playOut.start()).Start();
            new System.Threading.Thread(() => App_Start.portalOut.start()).Start();
            

            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                try
                {
                    Application["serverRoot"] = dc.vWeb_Settings.Where(s => s.Key == "ServerRoot").First().value;
                }
                catch
                {
                    Application["serverRoot"]="/";
                }
                
                var lic = new handlers.utils.Lic();
                try
                {
                    var rec = dc.vWeb_Settings.Where(s => s.Key == "license");
                    if(rec.Count()>0 && false)
                     {
                        var encoded=rec.First().value;

                      // var json = /*System.Web.Helpers.Json.Decode*///(dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(NFMAY24KeyEncript.worker.DecryptStringAES(encoded,"B**K15kb13"));
                       // lic.admin=json.admin;
                       // lic.lrv=json.lrv;
                       // lic.maxNewsOnDay=json.maxNewsOnDay;
                       // lic.maxProg=json.maxProg;
                       // lic.maxUsers=json.maxUsers;
                       // lic.mediaFiles=json.mediaFiles;
                       // lic.prompter=json.prompter;*/
                    }
                }
                catch(Exception ex){}
                Application["Lic"]=lic;

                var DoOnClickTypeBlockRec = dc.vWeb_Settings.Where(s => s.Key == "doOnClickBlockType");
                if (DoOnClickTypeBlockRec.Count() == 0)
                {

                    dc.ExecuteCommand("INSERT INTO tWeb_Settings ([key], Description, value) VALUES ('doOnClickBlockType',  'что делаем при клике на тип блока','normal')"); ;
                    dc.SubmitChanges();

                }
                DoOnClickTypeBlockRec = dc.vWeb_Settings.Where(s => s.Key == "doOnClickBlockType");

                Application["doOnClickBlockType"] = DoOnClickTypeBlockRec.First().value;
            }

            Application["Sessions"] = 0;


            // send the message to all clients, make sure that the method is camelCase.

        }

        internal protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Context.Request.Path.StartsWith("/testservice.asmx"))
                Context.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.ReadOnly);

           
            // Complete.
          //  base.CompleteRequest();
        }

        void Global_BeginRequest(object sender, EventArgs e)
        {
           
        }

        void Application_End(object sender, EventArgs e)
        {
            
            //  Code that runs on application shutdown
            
                        

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Application_End(sender, e);

        }
        void RegisterRoutes(RouteCollection Routes)
        {
           /* Routes.MapPageRoute("BlockEditor",
                "Block/{BlockId}/Edit/", "~/Blocks/BlockEditor.aspx");*/
            Routes.MapPageRoute("login", "login", "~/Index.aspx");
            Routes.MapPageRoute("login.aspx", "login.aspx", "~/Index.aspx");
            Routes.Add(new Route("logout", new handlers.exitUserRouteHandler()));
            Routes.MapPageRoute("programm", "route/{route_programId}", "~/Index.aspx");
            Routes.MapPageRoute("progr/group", "route/{route_programId}/{route_groupId}/", "~/Index.aspx");
            Routes.MapPageRoute("prog/group/news", "route/{route_programId}/{route_groupId}/{route_newsId}", "~/Index.aspx");
            Routes.MapPageRoute("prog/group/news/block", "route/{route_programId}/{route_groupId}/{route_newsId}/{route_blockId}", "~/Index.aspx");
          
            Routes.MapPageRoute("BlockExtViewer",
                "NewsBlocks/{BlockGuid}/", "~/Blocks/BlockExtViewer.aspx");
           // Routes.MapPageRoute("BlockExtFileUpload",
            //     "NewsBlocks/{BlockGuid}/FileUpload/{FileName}", new handlers.ExtBlockFileUploadRouteHandler());
            
          
         //  url: "mvc/{controller}/{action}/{id}",
       //    defaults: new { action = "Index", id = System.Web.Mvc.UrlParameter.Optional }
           

           Routes.Add(new Route("Block/{BlockId}/Edit/FileUpload/{FolderGuid}/{FolderCount}/{FileNumber}/{FileGuid}/{ChunkNumber}", new handlers.ExtBlockFileUploadRouteHandler()));

           Routes.Add(new Route("FileUpload/{FolderGuid}/{FolderCount}/{FileNumber}/{FileGuid}/{ChunkNumber}", new handlers.ExtBlockFileUploadRouteHandler()));
            Routes.Add(new Route("FileUploadSimple/{BlockId}/{fileNumber}/", new handlers.FileUploadSimpleRouteHandler()));
            Routes.Add(new Route("fileUploadChankSimple/{BlockId}/{fileNumber}/", new handlers.fileUploadChankSimpleRouteHandler()));
            


            Routes.Add(new Route("NewsBlocks/{BlockGuid}/FileUpload/{FolderGuid}/{FolderCount}/{FileNumber}/{FileGuid}/{ChunkNumber}", new handlers.ExtBlockFileUploadRouteHandler()));

            Routes.Add(new Route("Caspar/{newsId}/{date}.xml", new handlers.CasparUploadRouteHandler()));
            Routes.Add(new Route("Rtf/{newsId}/{date}.rtf", new handlers.NewsToRtfRouteHandler()));
            Routes.Add(new Route("Autoscript/{newsId}/{date}.rtf", new handlers.NewsToAutoscriptRouteHandler()));
            Routes.Add(new Route("Prompter/{newsId}/{date}.txt", new handlers.NewsToPrompterRouteHandler()));
            Routes.Add(new Route("Rtf/{newsId}/{date}.txt", new handlers.NewsToTextRouteHandler()));
            Routes.Add(new Route("Rtf/{newsId}/{date}.xls", new handlers.NewsToXLSRouteHandler()));
            Routes.Add(new Route("word/{newsId}/{date}.rtf", new handlers.NewsToWordRouteHandler()));
            Routes.Add(new Route("title/{newsId}/{date}.txt", new handlers.NewsToTitleListRouteHandler() ));
            Routes.Add(new Route("blocktext/{blockId}", new handlers.blockTextRouteHandler()));
            Routes.Add(new Route("GetFileFromFs/{path}", new handlers.GetFileFromFsRouteHandler()));
            Routes.Add(new Route("ws", new handlers.WebSocketRouteHandler()));
            Routes.Add(new Route("socialImg/{blockId}", new handlers.socialImagRouteHandler()));
            

            Routes.Add(new Route("checkDB", new handlers.CheckDbRouteHandler()));
            Routes.Add(new Route("nfsock", new NFSocket.NFSocketHandlerRouteHandler()));
            Routes.Add(new Route("nfsockResponce", new NFSocket.NFSocketResponceRouteHandler()));


            Routes.Add(new Route("Media/{MediaId}/LRV/{FileNumber}/", new handlers.MediaLRVRouteHandler()));
            Routes.Add(new Route("User/{UserId}/Right/", new handlers.UserRightRouteHandler()));

            Routes.Add(new Route("Media/File/{FileGuid}/Get", new handlers.GetFileRouteHandler()));
            Routes.Add(new Route("Api/Service/", new ServiceHandlers.ServiceRouteHandler()));
            Routes.Add(new Route("Api/Service/GetFFMpeg", new ServiceHandlers.FFMpegRouteHandler()));
            Routes.Add(new Route("Api/Service/LRV/{EncoderGuid}/{EncoderPass}/Upload/", new ServiceHandlers.LRVUploadRouteHandler()));
            Routes.Add(new Route("Api/Service/TH/{EncoderGuid}/Upload/{ThNumber}/", new ServiceHandlers.ThUploadRouteHandler()));
            Routes.Add(new Route("Thumbnail/{ThumbnailId}/Get/", new handlers.GethumbnailRouteHandler()));
            Routes.Add(new Route("Api/Service/config/Get", new ServiceHandlers.getServiceConfigRouteHandler()));
            Routes.Add(new Route("Api/Service/ListForGenerateLRV/Get", new ServiceHandlers.ListForGenerateLRVRouteHandler()));
            Routes.Add(new Route("Api/Service/ListForGenerateTH/Get", new ServiceHandlers.ListForGenerateTHRouteHandler()));
            Routes.Add(new Route("Api/Service/ListForGenerateIMG/Get", new ServiceHandlers.ListForGenerateIMGRouteHandler()));
            Routes.Add(new Route("Api/Service/ListForGenerateRSS/Get", new ServiceHandlers.ListForGenerateRSSRouteHandler()));
            Routes.Add(new Route("Api/Service/LRV/Add", new ServiceHandlers.LrvAddRouteHandler()));
            Routes.Add(new Route("Api/Service/LRV/UpdateStatus", new ServiceHandlers.LrvUpdateStatusRouteHandler()));
            Routes.Add(new Route("Api/Service/LRV/UpdateChrono", new ServiceHandlers.LrvUpdateChronoRouteHandler()));          
            Routes.Add(new Route("Api/Service/TH/Add", new ServiceHandlers.THAddRouteHandler()));
            Routes.Add(new Route("Api/Service/IMG/Add", new ServiceHandlers.ImgAddRouteHandler()));
            Routes.Add(new Route("Api/Service/RSS/Add", new ServiceHandlers.RssAddRouteHandler()));
            Routes.Add(new Route("Api/Service/RSS/Complite", new ServiceHandlers.RssAddCompliteRouteHandler()));
            
            Routes.Add(new Route("Api/Social/config/{typeId}/Get", new ServiceHandlers.getSocialConfigRouteHandler()));
            Routes.Add(new Route("Api/Social/Messages/{feedId}/Get", new ServiceHandlers.ListForSocialMessagesRouteHandler()));
            Routes.Add(new Route("Api/Social/Messages/{feedToMessageId}/Post", new ServiceHandlers.postSocialPublishRouteHandler()));

            /// API
            /// 
            Routes.Add(new Route("API/Prompter/Next/{newsId}/", new API.PrompterGetNextRouteHandler()));
            Routes.Add(new Route("API/Prompter/Next/{newsId}/{blockId}/", new API.PrompterGetNextRouteHandler()));
            Routes.Add(new Route("API/Prompter/Prev/{newsId}/", new API.PrompterGetPrevRouteHandler()));
            Routes.Add(new Route("API/Prompter/Prev/{newsId}/{blockId}/", new API.PrompterGetPrevRouteHandler()));
            Routes.Add(new Route("API/Prompter/Curr/{blockId}/", new API.PrompterGetCurrRouteHandler()));
            Routes.Add(new Route("API/Programs/", new API.ProgramsRouteHandler()));
            Routes.Add(new Route("API/News/{programId}/", new API.NewsRouteHandler()));
            Routes.Add(new Route("API/Blocks/{newsId}/", new API.BlocksRouteHandler()));
            Routes.Add(new Route("API/BlocksJson/{newsId}/", new API.BlocksJsonRouteHandler()));
            Routes.Add(new Route("API/BlocksTypeJson/{newsId}/", new API.BlocksTypeJsonRouteHandler()));
            Routes.Add(new Route("API/BlockChernovik/{id}/", new API.BlocksChernovikRouteHandler()));
            Routes.Add(new Route("API/BlocksUndelete/{blockId}/", new API.BlockUndelRouteHandler()));
            Routes.Add(new Route("API/newsUndelete/{blockId}/", new API.newsUndelRouteHandler()));
            Routes.Add(new Route("API/News/Video/{newsId}", new API.videoFromNewsGetRouteHandler()));
            Routes.Add(new Route("API/Block/Media/srt/{mediaId}/{layerId}", new API.SrtFileFromMediaGetRouteHandler()));
            Routes.Add(new Route("API/Block/Media/Crowl/{mediaId}", new API.CrowlFileFromMediaGetRouteHandler()));
            

            Routes.Add(new Route("Api/edl/{blockId}/edl.xml", new ServiceHandlers.EDLRouteHandler()));
            Routes.Add(new Route("Api/Service/LRVstarted/{id}", new ServiceHandlers.LRVstartedRouteHandler()));
            Routes.Add(new Route("Api/rogrammsListWidthRight/{id}", new API.ProgrammsListWidthRightGetHandler()));
            Routes.Add(new Route("Api/Service/Sessions", new ServiceHandlers.SessionsHandelerRouteHandler()));
            Routes.Add(new Route("Api/Service/CountOfActiveNews", new ServiceHandlers.CountOfActiveNewsRouteHandler()));

            Routes.Add(new Route("rss", new handlers.mobileRssRouteHandler()));
            Routes.Add(new Route("rssPage/{feedId}", new handlers.rssPageRouteHandler()));
            


            ////
            Routes.MapPageRoute("L/",
                "LNews/{NewsId}/", "~/Default.aspx");
            Routes.MapPageRoute("L/Edit/",
                "LBlock/{BlockId}/Edit/", "~/Lite/LiteBlockEditor.aspx");
            Routes.Add(new Route("LBlock/{BlockId}/Edit/FileUpload/{FolderGuid}/{FolderCount}/{FileNumber}/{FileGuid}/{ChunkNumber}", new handlers.ExtBlockFileUploadRouteHandler()));

            Routes.MapPageRoute("Lite",
                "L/", "~/Default.aspx");
            Routes.MapPageRoute("LiteProgramm",
                "L/{ProgramId}/", "~/Default.aspx");
            Routes.MapPageRoute("LiteNewsGroup",
                "L/{ProgramId}/{NewsGroupSelected}/", "~/Default.aspx");
            Routes.MapPageRoute("LiteListBlocks",
                "L/{ProgramId}/{NewsGroupSelected}/{NewsId}/{NewsAction}/", "~/Default.aspx");
           

            





            Application["dblook"]=true;

        }
    }
}
