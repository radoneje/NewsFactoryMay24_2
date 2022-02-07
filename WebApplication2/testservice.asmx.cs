using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
//using Binbin.Linq;
using LinqKit;
using System.IO;



namespace WebApplication2
{
    /// <summary>
    /// Summary description for testservice
    /// </summary>
    [WebService()]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]

    public class testservice : System.Web.Services.WebService
    {
        const string HTML_TAG_PATTERN = "\"\'\\<.*?>";
        //static Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext();
        //  public void testservice(){
        //     dc = new Blocks.DataClasses1DataContext();
        // }

        public static string StripHTML(string inputString)
        {
            return Regex.Replace
              (inputString, HTML_TAG_PATTERN, string.Empty);
        }
        public string RemomeHtmlTag(string Instr)
        {
            return Instr.Replace("<", "").Replace(">", "");
        }



        [WebMethod(EnableSession = true)]
        public string GetBlocksFromNews(string sNewsId)
        {
            if (Session["UserId"] == null)
                return "false";
            //  try
            //  {
            //lock (Application["dblook"])
            {

                using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                {
                    var ns = dc.vWeb_NewsForBlocksHeads.Where(d => d.id == (long)Convert.ToInt64(sNewsId));
                    if (ns.Count() < 1)
                        return "";
                    var news = ns.First();
                    var Blocks = dc.vWeb_Blocks.Where(b => b.ParentId == 0 && b.NewsId == (long)Convert.ToInt64(sNewsId)).OrderBy(b => b.Sort).ToList();
                    foreach (var a in Blocks)
                    {
                        a.Name = RemomeHtmlTag((string)a.Name);
                    }
                    var data = new
                    {
                        NewsName = RemomeHtmlTag((string)news.Name),
                        NewsOwner = RemomeHtmlTag((string)news.UserName),
                        NewsDate = news.NewsDate.ToString(),
                        NewsDuration = TimeSpan.FromSeconds((double)news.Duration).ToString(),
                        NewsChrono = TimeSpan.FromSeconds(news.NewsTime).ToString(),
                        NewsChronoPlanned = TimeSpan.FromSeconds(news.TaskTime).ToString(),
                        NewsChronoCalculated = TimeSpan.FromSeconds(news.CalcTime).ToString(),
                        NewsBlocks = Blocks
                    };
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                    return js.Serialize(data);
                }
            }
            //  }
            //   catch (Exception ex)
            // {

            //   }

            System.Web.Script.Serialization.JavaScriptSerializer js1 = new System.Web.Script.Serialization.JavaScriptSerializer();
            return js1.Serialize("");
        }
        [WebMethod(EnableSession = true)]
        public string GetBlocksFromCopyNews(string sNewsId)
        {

            if (Session["UserId"] == null)
                return "false";
            try
            {
                //lock (Application["dblook"])
                {
                    using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                    {
                        var news = dc.vWeb_CopyNewsForBlocksHeads.Where(d => d.id == (long)Convert.ToInt64(sNewsId)).First();
                        var Blocks = dc.vWeb_Blocks.Where(b => b.ParentId == 0 && b.NewsId == news.id).OrderBy(b => b.Sort).ToList();
                        foreach (var a in Blocks)
                        {
                            a.Name = RemomeHtmlTag((string)a.Name);

                        }
                        var data = new
                        {
                            NewsName = RemomeHtmlTag((string)news.Name),
                            NewsOwner = news.UserName,
                            NewsDate = news.NewsDate.ToString(),
                            NewsDuration = TimeSpan.FromSeconds((double)news.Duration).ToString(),
                            NewsChrono = TimeSpan.FromSeconds(news.NewsTime).ToString(),
                            NewsChronoPlanned = TimeSpan.FromSeconds(news.TaskTime).ToString(),
                            NewsChronoCalculated = TimeSpan.FromSeconds(news.CalcTime).ToString(),
                            NewsBlocks = Blocks
                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                        return js.Serialize(data);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                return js.Serialize("");
            }
            System.Web.Script.Serialization.JavaScriptSerializer js1 = new System.Web.Script.Serialization.JavaScriptSerializer();
            return js1.Serialize("");
        }

        /*  [WebMethod(EnableSession = true)]
          public string BlockDown(string sBlockId)
          {
              try
              {
                  Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext();
                  dc.sp_BlockDown(Convert.ToInt64(sBlockId));
              }
              catch { }
              return "";

          }
          public string BlockUp(string sBlockId)
          {
              try
              {
                  Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext();
                  dc.sp_BlockUp(Convert.ToInt64(sBlockId));
              }
              catch { }
              return "";

          }*/
        [WebMethod(EnableSession = true)]
        public string SubBlockData(string sBlockId)
        {
            if (Session["UserId"] == null)
                return "false";

            try
            {

                {
                    using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                    {

                        var bl = dc.vWeb_BlockToExtView.Where(b => b.Id == Convert.ToInt64(sBlockId)).First();

                        var data = new
                        {
                            Id = bl.Id,
                            Name = bl.Name,
                            Creator = bl.Creator,
                            Operator = bl.Operator,
                            Jockey = bl.Jockey,
                            Text = bl.BlockText,
                            Ready = bl.Ready,
                            Approve = bl.Approve,
                            Description = bl.Description,
                            Cutter = bl.Cutter
                        };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                        return js.Serialize(data);
                    }
                }
            }
            catch (System.Exception ex)
            {
                var data = new
                {
                    Id = "error",
                    Name = "error",
                    Creator = "error",
                    Operator = "error",
                    Jockey = "error",
                    Text = "error",
                    Ready = "error",
                    Approve = "error",
                    Description = "error"
                };
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                return js.Serialize(data);
            }

        }/// берем данные для открытого блока
        [WebMethod(EnableSession = true)]
        public string BlockAddFileNameOnly()
        {
            if (Session["UserId"] == null)
                return "false";
            try
            {
                System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
                receiveStream.Position = 0;
                System.IO.StreamReader readStream =
                                   new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
                string rawJson = readStream.ReadToEnd();
                var inParam = System.Web.Helpers.Json.Decode(rawJson);

                using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                {
                    var BlockId = Convert.ToInt64(inParam.blockId);
                    var FileName = inParam.filename;


                    int BlockType = 0;
                    string mime = System.Web.MimeMapping.GetMimeMapping(FileName);
                    if (mime.IndexOf("image") == 0)
                    {
                        BlockType = 1;
                    }
                    if (mime.IndexOf("video") == 0 || mime.IndexOf("audio") == 0)
                    {
                        BlockType = 2;
                    }


                    int TaskId = dc.pWeb_InsertMediaToBlock(BlockId, FileName, FileName, BlockType);

                }

            }
            catch { return "-1"; }
            return "0";

        }
        [WebMethod(EnableSession = true)]
        public string SaveBlock()
        {
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            // var json = JObject.Parse(rawJson);  //Turns your raw string into a key value lookup

            /* BlockSaveClass */
            var InData = System.Web.Helpers.Json.Decode(rawJson);//JsonConvert.DeserializeObject<BlockSaveClass>(rawJson);
            bool disableHistory = InData.disableHistory;


            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                try
                {
                    Int64 BlockId = Convert.ToInt64(InData.BlockId);
                    var bl = dc.Blocks.Where(b => b.Id == BlockId).First();
                    bl.Name = StripHTML((string)InData.BlockName);
                    bl.BLockType = Convert.ToInt32(InData.BlockTypeId);
                    bl.CreatorId = Convert.ToInt32(InData.BlockAutor);
                    bl.OperatorId = Convert.ToInt32(InData.BlockOperator);
                    bl.JockeyId = Convert.ToInt32(InData.BlockJockey);
                    bl.CutterId = Convert.ToInt32(InData.BlockCutter);
                    bl.Ready = InData.BlockReady;//== "True";
                    bl.Approve = InData.BlockApprove;// == "True";
                    if (((string)InData.BlockText).Length > 1)
                        bl.BlockText = StripHTML((string)InData.BlockText);
                    bl.Description = StripHTML((string)InData.BlockDescription);
                    bl.CalcTime = Convert.ToInt64(InData.BlockChronoCalc);
                    bl.TaskTime = Convert.ToInt64(InData.BlockChronoTask);
                    bl.BlockTime = Convert.ToInt64(InData.BlockChronoReal);
                    bl.isChanged = DateTime.Now;
                   
                    dc.SubmitChanges();

                    dc.sp_UpdateNewsHrono(bl.Id);
                    if (!disableHistory)
                    {
                        NFSocket.SendToAll.SendData("blockSave", new { blockId = InData.BlockId });
                        dc.tWeb_BlockHistories.InsertOnSubmit(new Blocks.tWeb_BlockHistory
                        {
                            id = Guid.NewGuid().ToString(),
                            date = DateTime.Now,
                            blockZipText = handlers.utils.Zip((string)InData.BlockText),
                            blockId = BlockId,
                            userId = Convert.ToInt32(Session["UserId"])
                        });
                        dc.SubmitChanges();
                    }

                }
                catch (Exception ex)
                {
                    var OutError = new { Message = "Ошибка," + ex.Message + " не могу найти блок в БД,BlockId=" + InData.BlockId, err = 1 };
                    System.Web.Script.Serialization.JavaScriptSerializer jsErr = new System.Web.Script.Serialization.JavaScriptSerializer();
                    return jsErr.Serialize(OutError);
                }

            }

            var OutData = new { Message = "Блок сохранен, BlockId=" + InData.BlockId, err = 0 };//json["BlockId"].ToObject<string>(), };
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            return js.Serialize(OutData);
        }
        public class BlockSaveClass
        {
            [JsonProperty("BlockTypeId")]
            public string BlockTypeId { get; set; }
            [JsonProperty("BlockName")]
            public string BlockName { get; set; }
            [JsonProperty("BlockAutor")]
            public string BlockAutor { get; set; }
            [JsonProperty("BlockOperator")]
            public string BlockOperator { get; set; }
            [JsonProperty("BlockJockey")]
            public string BlockJockey { get; set; }
            [JsonProperty("BlockReady")]
            public string BlockReady { get; set; }
            [JsonProperty("BlockApprove")]
            public string BlockApprove { get; set; }
            [JsonProperty("BlockText")]
            public string BlockText { get; set; }
            [JsonProperty("BlockDescription")]
            public string BlockDescription { get; set; }
            [JsonProperty("BlockChronoCalc")]
            public string BlockChronoCalc { get; set; }
            [JsonProperty("BlockChronoTask")]
            public string BlockChronoTask { get; set; }
            [JsonProperty("BlockChronoReal")]
            public string BlockChronoReal { get; set; }
            [JsonProperty("BlockId")]
            public string BlockId { get; set; }

        }



        private struct uList
        {
            public string userName;
            public int userId;

        }
        [WebMethod(EnableSession = true)]
        public string loginUsersList()
        {


            List<uList> ret = new List<uList>();
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.vWeb_UsersLoginLists.OrderBy(u => u.UserName).ToList().ForEach(l =>
                {
                    ret.Add(new uList()
                    {
                        userId = l.UserID,
                        userName = l.UserName
                    });
                });
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(ret);

        }

        [WebMethod(EnableSession = true)]
        public string Ping()
        {
            if (Session["UserId"] == null)
                return "NO auth";
            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            PingClass InData = JsonConvert.DeserializeObject<PingClass>(rawJson);


            //string sCookie = json["Cookie"].First().ToString();

            //  try
            //{

            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                Int32 UserId = (Int32)Session["UserId"];

                var UserRecordset = dc.vWeb_UsersLoginLists.Where(v => v.UserID == UserId);
                if (UserRecordset.Count() == 0)
                {
                    var OutData = new { Message = handlers.utils.getLocalString("ErrorNoUser"), Status = 0, Cookie = 0 };//json["BlockId"].ToObject<string>(), };
                    System.Web.Script.Serialization.JavaScriptSerializer js1 = new System.Web.Script.Serialization.JavaScriptSerializer();
                    return js1.Serialize(OutData);
                }
                var usr = UserRecordset.First();
                //     var OutData1 = new { Message = "Работает: " + UserName.UserName + ", обновлено: " + DateTime.Now.ToShortTimeString(), Status = a.ToString(), Cookie = InData.sCookie, NewsData = GetBlocksFromNews(InData.sNewsId) };//json["BlockId"].ToObject<string>(), };
                var OutData1 = new { userName = usr.UserName, Message = "<input type=\"hidden\" id=\"CurrentUserId\" value=\"" + usr.UserID + "\"/></input>", Status = UserId.ToString(), Cookie = InData.sCookie, NewsData = (InData.sGroupId == "0") ? GetBlocksFromNews(InData.sNewsId) : GetBlocksFromCopyNews(InData.sNewsId), InMsg = GetInMsg(UserId), };//json["BlockId"].ToObject<string>(), };
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                return js.Serialize(OutData1);
            }

            //}catch(){};

            return null;
        }
        public class PingClass
        {
            [JsonProperty("Cookie")]
            public string sCookie { get; set; }
            [JsonProperty("NewsId")]
            public string sNewsId { get; set; }
            [JsonProperty("NewsGroupId")]
            public string sGroupId { get; set; }
        }
        [WebMethod(EnableSession = true)]
        public string BlockUp(string sBlockId)
        {
            if (Session["UserId"] == null)
                return "false";
            try
            {
                Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext();
                var a = dc.sp_BlockUp(Convert.ToInt64(sBlockId));
            }
            catch (Exception ex)
            { }

            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            return js.Serialize("ok");

        }
        [WebMethod(EnableSession = true)]
        public string BlockDown(string sBlockId)
        {
            if (Session["UserId"] == null)
                return "false";


            try
            {
                Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext();
                var a = dc.sp_BlockDown(Convert.ToInt64(sBlockId));
            }
            catch (Exception ex)
            { }

            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            return js.Serialize("ok");
        }
        [WebMethod(EnableSession = true)]
        public string BlockColor(List<dynamic> blocks)
        {
            if (Session["UserId"] == null)
                return "false";

            //    try
            {
                using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                {
                    blocks.ForEach(l =>
                    {
                        var id = (string)l["id"];
                        dc.Blocks.Single(b => b.Id.ToString() == id).TextLang3 = l["color"].ToString();
                    });
                    dc.SubmitChanges();

                }
                /* System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
                 receiveStream.Position = 0;
                 System.IO.StreamReader readStream =
                                    new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
                 string rawJson = readStream.ReadToEnd();
                 var param = /*System.Web.Helpers.Json.DecodeNewtonsoft.Json.JsonConvert.DeserializeObject(rawJson);

                 using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                 {
                     long id = Convert.ToInt64(param.sBlockId);
                     dc.Blocks.Where(b => b.Id == id).First().TextLang3 = Convert.ToString(param.sColorId);
                     dc.SubmitChanges();
                 }*/
            }
            //  catch { return "-1"; }

            return "0";
        }
        public class BlockMoveClass
        {
            [JsonProperty("sBlockId")]
            public string sBlockId { get; set; }
            [JsonProperty("sAfterBlockId")]
            public string sAfterBlockId { get; set; }
        }
        [WebMethod(EnableSession = true)]
        public string MoveBlock()
        {
            //[WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            BlockMoveClass InData = JsonConvert.DeserializeObject<BlockMoveClass>(rawJson);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                var a = dc.sp_ChangeBlockPosition(Convert.ToInt64(InData.sBlockId), Convert.ToInt64(InData.sAfterBlockId));
            }

            return "ok";// "a.ToString()";
        }
        [WebMethod(EnableSession = true)]
        public string BlockMoveGroup()
        {
            //[WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";
            // try
            {


                var Indata = handlers.utils.getAjaxResp(HttpContext.Current.Request.InputStream);
                int ii = Indata.arr.Length;
                for (int i = 0; i < Indata.arr.Length; i++)
                {
                    // if (i > 0)
                    {
                        var ss = Indata.arr[i];
                        lock (Application["dblook"])
                        {
                            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                            {
                                long blId = Convert.ToInt64(Indata.arr[i].sBlockId);
                                long AfterBlId = Convert.ToInt64(Indata.arr[i].sAfterBlockId);
                                dc.sp_ChangeBlockPosition(blId, AfterBlId);

                            }
                        }
                    }
                }

            }
            /* catch(Exception e)
             {
                 return e.Message;
             }
             */
            return "";
        }
        public class GetListOfNewsClass
        {
            [JsonProperty("Cookie")]
            public string sBlockId { get; set; }
            [JsonProperty("NewsId")]
            public string sAfterBlockId { get; set; }
        }
        public struct NewsListStruct
        {
            public string NewsName { get; set; }
            public Int64 NewsId { get; set; }
            public int GroupID { get; set; }
            public string NewsDate { get; set; }

        }
        [WebMethod(EnableSession = true)]
        public string GetListOfNews()
        { //[WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();
            PingClass InData = JsonConvert.DeserializeObject<PingClass>(rawJson);

            List<NewsListStruct> NewsList = new List<NewsListStruct>();
            News.NewsDataContext dc = new News.NewsDataContext();

            DateTime tommorow = DateTime.Now.Date.AddDays(1);
            NewsList.Add(new NewsListStruct() { NewsName = /*"планируемые"*/handlers.utils.getLocalString("CapPlannedNews"), NewsId = -10, GroupID = 0, NewsDate = "0" });
            foreach (var a in dc.News.Where(d => d.NewsDate >= tommorow && d.Deleted == false && d.ProgramId == Convert.ToInt32(InData.sNewsId)).OrderByDescending(d => d.NewsDate))
            {
                NewsList.Add(new NewsListStruct() { NewsName = a.Name, NewsId = a.id, GroupID = 0, NewsDate = a.NewsDate.ToString() });
            }

            DateTime today = DateTime.Now.Date;
            NewsList.Add(new NewsListStruct() { NewsName = /*"сегодня"*/handlers.utils.getLocalString("CapTodayNews"), NewsId = -20, GroupID = 0, NewsDate = "0" });
            foreach (var a in dc.News.Where(d => d.NewsDate >= today && d.NewsDate < today.AddDays(1) && d.Deleted == false && d.ProgramId == Convert.ToInt32(InData.sNewsId)).OrderByDescending(d => d.NewsDate))
            {
                NewsList.Add(new NewsListStruct() { NewsName = a.Name, NewsId = a.id, GroupID = 0, NewsDate = a.NewsDate.ToString() });
            }

            NewsList.Add(new NewsListStruct() { NewsName =/* "прошедшие"*/handlers.utils.getLocalString("CapLastNews"), NewsId = -30, GroupID = 0, NewsDate = "0" });
            foreach (var a in dc.News.Where(d => d.NewsDate < today && d.Deleted == false && d.ProgramId == Convert.ToInt32(InData.sNewsId)).OrderByDescending(d => d.NewsDate))
            {
                NewsList.Add(new NewsListStruct() { NewsName = a.Name, NewsId = a.id, GroupID = 0, NewsDate = a.NewsDate.ToString() });
            }
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            NewsList.Add(new NewsListStruct() { NewsName = handlers.utils.getLocalString("CapSharedCopybox"), NewsId = -1002, GroupID = 2, NewsDate = "0" });
            foreach (var a in dc.CopyNews.Where(d => d.Deleted == false && d.GroupID == 2))
            {
                NewsList.Add(new NewsListStruct() { NewsName = a.Name, NewsId = a.id, GroupID = 2, NewsDate = a.NewsDate.ToString() });
            }
            NewsList.Add(new NewsListStruct() { NewsName = handlers.utils.getLocalString("CapPersonalCopybox"), NewsId = -1001, GroupID = 1, NewsDate = "0" });
            foreach (var a in dc.CopyNews.Where(d => d.Deleted == false && d.GroupID == 1 && d.EditorId == Convert.ToInt32((int)Session["UserId"])))
            {
                NewsList.Add(new NewsListStruct() { NewsName = a.Name, NewsId = a.id, GroupID = 1, NewsDate = a.NewsDate.ToString() });
            }

            NewsList.Add(new NewsListStruct() { NewsName = handlers.utils.getLocalString("CapTemplates"), NewsId = -1102, GroupID = 102, NewsDate = "0" });
            foreach (var a in dc.CopyNews.Where(d => d.Deleted == false && d.GroupID == 102).OrderByDescending(dd => dd.Name))
            {
                NewsList.Add(new NewsListStruct() { NewsName = a.Name, NewsId = a.id, GroupID = 102, NewsDate = a.NewsDate.ToString() });
            }

            return js.Serialize(NewsList);

            return "0";

        }
        [WebMethod(EnableSession = true)]
        public string programsListGet()
        {
            //[WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            try
            {
                var NewsList = new List<News.vUsersProgramList>();
                using (News.NewsDataContext dc = new News.NewsDataContext())
                {
                    var userId = Convert.ToInt32(Session["UserId"]);
                    var lst = dc.vUsersProgramLists.Where(p => p.UserID == userId).OrderBy(uu => uu.UserName).ToList();
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                }
            }
            catch (Exception ex)
            {
            }
            return "";

        }


        [WebMethod(EnableSession = true)]
        public string CopyBlockToNews()
        {
            //[WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            BlockMoveClass InData = JsonConvert.DeserializeObject<BlockMoveClass>(rawJson);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                try
                {
                    var a = dc.pWeb_CopyBlockTONewNewsWITHnoSORT(Convert.ToInt64(InData.sBlockId), Convert.ToInt64(InData.sAfterBlockId), 0);
                    dc.sp_UpdateNewsHrono(a);
                    NFSocket.SendToAll.SendData("blockReplace", new { blockId = InData.sBlockId });
                    return a.ToString();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            return "0";
        }
        [WebMethod(EnableSession = true)]
        public string CopyBlockGroupToNews()
        {
            //[WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            var InData = System.Web.Helpers.Json.Decode(rawJson);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                try
                {
                    var a = dc.pWeb_CopyBlockGroupsTONewNews(Convert.ToInt64(InData.sBlockId), Convert.ToInt64(InData.sAfterBlockId));
                    return a.ToString();
                }
                catch (Exception ex) { return ex.Message; }
            }

            return "0";
        }
        [WebMethod(EnableSession = true)]
        public string UnlookBlock()
        {
            //[WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";


            var json = handlers.utils.getAjaxResp(HttpContext.Current.Request.InputStream);

            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                // var a = dc.pWeb_UnlookBlock(Convert.ToInt32(json.cookie));
                dc.pWeb_UnlookBlockFromUser(Convert.ToInt32(Session["UserId"]));
            }
            //return a.ToString();

            return "";
        }
        [WebMethod(EnableSession = true)]
        public string BlockLooker()
        {
            //[WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";


            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            var prm = System.Web.Helpers.Json.Decode(rawJson);

            PingClass InData = new PingClass() { sCookie = prm.sCookie, sNewsId = prm.sNewsId };
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                if (InData.sNewsId == "")
                    return "";
                //   var a = dc.pWeb_UpdateLookingFromCookie(Convert.ToInt32(InData.sCookie), Convert.ToInt64(InData.sNewsId));
                dc.pWeb_UpdateLookingFromUser(Convert.ToInt32(Session["UserId"]), Convert.ToInt64(InData.sNewsId));

            }
            //return a.ToString();

            return "";
        }

        // [WebMethod(EnableSession = true)]
        // public string BlockLooker()
        //  {UpdateBlockLockTimeot
        public class MoveNewsClass
        {
            [JsonProperty("sSourceId")]
            public string sSourceId { get; set; }
            [JsonProperty("sDestGroupId")]
            public string sDestGroupId { get; set; }
            [JsonProperty("sDestProgramId")]
            public string sDestProgramId { get; set; }
            [JsonProperty("Coockie")]
            public string Coockie { get; set; }

        }
        [WebMethod(EnableSession = true)]
        public string MoveNews()
        {
            //[WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            MoveNewsClass InData = JsonConvert.DeserializeObject<MoveNewsClass>(rawJson);

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {

                if (InData.sDestGroupId != "0")
                {
                    var a = dc.sp_NewsToCopybox(Convert.ToInt64(InData.sSourceId), Convert.ToInt64(InData.sDestGroupId), (int)Session["UserId"]);

                }
                else
                {
                    var UserId = (int)Session["UserId"];
                    var a = dc.sp_NewsFromCopyboxTemplate(Convert.ToInt64(InData.sSourceId), UserId, Convert.ToInt32(InData.sDestProgramId));
                }

            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string NewsToArchive()
        {
            if (Session["UserId"] == null)
                return "no auth";
            /*  System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
              receiveStream.Position = 0;
              System.IO.StreamReader readStream =
                                 new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
              string rawJson = readStream.ReadToEnd();

             // MoveNewsClass InData = JsonConvert.DeserializeObject<MoveNewsClass>(rawJson);
              var InData = /*System.Web.Helpers.Json.Decode(dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(rawJson);*/
            var InData = handlers.utils.getAjaxResp(HttpContext.Current.Request.InputStream);

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                dc.sp_NewsToArchive(Convert.ToInt64(InData.id));
            }

            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { id = InData.id });
        }
        [WebMethod(EnableSession = true)]
        public string ArchiveToNews()
        {
            //[WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            MoveNewsClass InData = JsonConvert.DeserializeObject<MoveNewsClass>(rawJson);

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                dc.sp_ArchiveToNews(Convert.ToInt64(InData.sSourceId));
            }

            return "";
        }

        [WebMethod(EnableSession = true)]
        public string DeleteNews()
        {
            //[WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            PingClass InData = JsonConvert.DeserializeObject<PingClass>(rawJson);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                //if (dc.pWeb_ChekUserCookie(Convert.ToInt32(InData.sCookie)) >= 0)

                string sql = "";
                if (InData.sGroupId == "0")
                    sql = "UPDATE NEWS SET Deleted=1 WHERE ID=" + InData.sNewsId;
                else
                    sql = "UPDATE COPYNEWS SET Deleted=1 WHERE ID=" + InData.sNewsId;
                dc.ExecuteCommand(sql);
                var userId = (int)Session["UserId"];
                log(userId, 3, Convert.ToInt64(InData.sNewsId));
                NFSocket.SendToAll.SendData("newsDelete", new { newsId = InData.sNewsId });

            }

            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { id = InData.sNewsId });
        }
        [WebMethod(EnableSession = true)]
        public string DeleteBlock()
        {
            // [WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            PingClass InData = JsonConvert.DeserializeObject<PingClass>(rawJson);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {

                lock (Application)
                {
                    dc.ExecuteCommand("UPDATE Blocks SET Deleted=1, isChanged = GETDATE() WHERE ID=" + InData.sNewsId);

                    var userId = (int)Session["UserId"];
                    log(userId, 4, Convert.ToInt64(InData.sNewsId));
                    dc.sp_UpdateNewsHrono(Convert.ToInt64(InData.sNewsId));
                }

            }

            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { id = InData.sNewsId });
        }
        [WebMethod(EnableSession = true)]
        public string AddNews()
        {
            // [WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            PingClass InData = JsonConvert.DeserializeObject<PingClass>(rawJson);
            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                if (dc.pWeb_ChekUserCookie(Convert.ToInt32(InData.sCookie)) >= 0)
                {
                    var data = new
                    {
                        NewsId = dc.pWeb_NewNews(Convert.ToInt64(InData.sNewsId)),

                    };
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                    NFSocket.SendToAll.SendData("newsNew", data);
                    return js.Serialize(data);
                }
            }

            return "";
        }
        [WebMethod(EnableSession = true)]
        public string AddBlock()
        {
            // [WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";


            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            PingClass InData = JsonConvert.DeserializeObject<PingClass>(rawJson);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                //if (dc.pWeb_ChekUserCookie(Convert.ToInt32(InData.sCookie)) >= 0)
                {
                    var data = new
                    {
                        NewsId = dc.pWeb_NewBlock((long)Convert.ToInt64(InData.sNewsId), ((long)Convert.ToInt64(InData.sGroupId)), (int)Session["UserId"]),

                    };

                    NFSocket.SendToAll.SendData("blockAdd", new { newsId = InData.sNewsId, blockId = data.NewsId });
                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                    return js.Serialize(data);
                }
            }
            return "";

        }

        public struct RssStruct
        {
            public string Title { get; set; }
            public Int64 Id { get; set; }
            public string pubDate { get; set; }
            public string Lid { get; set; }
            public string Image { get; set; }
            public string Link { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public string GetListOfRss()
        {
            // [WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";
            lock (Application["dblook"])
            {
                System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
                receiveStream.Position = 0;
                System.IO.StreamReader readStream =
                                   new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
                string rawJson = readStream.ReadToEnd();
                PingClass InData = JsonConvert.DeserializeObject<PingClass>(rawJson);



                List<RssStruct> RssList = new List<RssStruct>();
                News.NewsDataContext dcr = new News.NewsDataContext();

                int i = 0;///  Take глючит !!!
                foreach (var a in dcr.vWeb_RssFeeds.OrderByDescending(v => v.Date))
                {
                    i++;
                    if (i > 30) break;
                    RssList.Add(new RssStruct() { Title = StripHTML((string)a.Name), pubDate = ((DateTime)a.Date).ToString(), Image = a.ImgLink, Lid = StripHTML((string)a.Lid), Link = a.Link, Id = a.id });
                }


                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                return js.Serialize(RssList);
            }

        }

        public struct ArchiveSearchStruct
        {
            public string Cookie { get; set; }
            public string EditorId { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string Text { get; set; }
            public string ProgramId { get; set; }
        }
        public struct ArchiveFindNewsStruct
        {
            public string NewsName { get; set; }
            public string NewsDate { get; set; }
            public string NewsId { get; set; }
            public string BlockId { get; set; }
            public string RequestString { get; set; }
        }

        [WebMethod(EnableSession = true)]
         public string ArchiveSearch(int Cookie, int EditorId, string Text, int ProgramId, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            
            try
            {

                // [WebMethod(EnableSession = true)]
                if (Session["UserId"] == null)
                    return "false";
                lock (Application["dblook"])
                {


                    System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

                    /*  System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
                      receiveStream.Position = 0;
                      System.IO.StreamReader readStream =
                                         new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
                      string rawJson = readStream.ReadToEnd();
                      ArchiveSearchStruct InData = JsonConvert.DeserializeObject<ArchiveSearchStruct>(rawJson);*/
                    bool error = false;
                    int i = 0;
                    int count = 0;
                    string Message = "";
                    List<ArchiveFindNewsStruct> BlockData = new List<ArchiveFindNewsStruct>();

                    try
                    {
                        using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                        {
                            var find = dc.vWeb_ArchiveSearches.Where(r => r.ProgramId == ProgramId);
                            if (EditorId >= 0)
                                find = find.Where(f => f.CreatorId == Convert.ToInt32(EditorId));
                            if (Text == "     ")
                                Text = "";
                            if (Text.Length > 2)
                                find = find.Where(r => r.BlockText.IndexOf(RemomeHtmlTag((string)Text)) >= 0 || r.NewsName.IndexOf(RemomeHtmlTag((string)Text)) >= 0 || r.BlockName.IndexOf(RemomeHtmlTag((string)Text)) >= 0);
                            find = find.Where(r => r.NewsDate >= StartDate && r.NewsDate <= EndDate);
                            find = find.Take(501);
                            var b = find.OrderBy(d => d.Sort).OrderByDescending(d => d.NewsDate);



                            // var c = dc.vWeb_ArchiveSearches;//.Where(a => a.ProgramId == Convert.ToInt32(InData.ProgramId));
                            /* var predicate = PredicateBuilder.False<Blocks.vWeb_ArchiveSearch>();

                             predicate = predicate.Or(a => a.ProgramId == Convert.ToInt32(InData.ProgramId));

                             if (InData.Text.Length > 0)
                             {
                                 InData.Text = RemomeHtmlTag((string)InData.Text);
                                 predicate = predicate.And(f => f.BlockName.Contains(InData.Text) || f.NewsName.Contains(InData.Text) || f.BlockText.Contains(InData.Text));
                             }

                             if (InData.StartDate != null)
                             {
                                 DateTime tmp = DateTime.Parse(InData.StartDate, null, System.Globalization.DateTimeStyles.RoundtripKind).AddDays(1).Date;

                                 predicate = predicate.And(f => f.NewsDate > tmp.Date);
                                 // 
                             }
                             if (InData.EndDate != null)
                             {
                                 DateTime tmp = DateTime.Parse(InData.EndDate, null, System.Globalization.DateTimeStyles.RoundtripKind).AddDays(1).Date;

                                 predicate = predicate.And(f => f.NewsDate < tmp.Date);
                             }
                             if (InData.ProgramId != null && Convert.ToInt32(InData.EditorId) >= 0)
                             {

                                 predicate = predicate.And(f => f.CreatorId == Convert.ToInt32(InData.EditorId));
                             }



                             var b = dc.vWeb_ArchiveSearches.AsQueryable().Where(predicate).OrderBy(d => d.Sort).OrderByDescending(d => d.NewsDate);
                             */
                            foreach (var block in b)
                            {
                                i++;
                                if (i > 3000)
                                    break;
                                ArchiveFindNewsStruct item = new ArchiveFindNewsStruct();


                                item.NewsName = RemomeHtmlTag((string)block.NewsName);
                                if (Text.Length > 0)
                                {
                                    //item.NewsName=item.NewsName.Replace(InData.Text,"<span class='HiLightArchiveResult'>"+InData.Text+"</span>");

                                    var regex = new Regex(Text, RegexOptions.IgnoreCase);
                                    item.NewsName = regex.Replace(item.NewsName, "<span class='HiLightArchiveResult'>" + Text + "</span>");
                                }

                                item.NewsDate = block.NewsDate.ToString();
                                item.NewsId = block.NewsId.ToString();
                                item.BlockId = block.BlockId.ToString();
                                item.RequestString = RemomeHtmlTag((string)Text);

                                BlockData.Add(item);

                            }

                            count = b.Count();
                        }
                        //      catch(Exception ex)
                        {
                            error = true;
                            //      Message = ex.Message;
                        }
                        var ret = new { Message = Message, Count = count, Overload = (bool)(i >= 3000) ? "1" : "0", Error = error, Blocks = BlockData.ToList() };
                        return js.Serialize(ret);
                    }
                    catch (Exception ex)
                    {
                        var ret = new { Message = Message, Error = ex.Message };
                        return js.Serialize(ret);
                    }

                }

            }
            catch (Exception ex)
            {
                var ret = new { Message = "fatal error ", Error = ex.Message };
                return Newtonsoft.Json.JsonConvert.SerializeObject(ret);
            }
            return "0";

        }

        public struct LentaFindNewsStruct
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string AgencyName { get; set; }
            public string Link { get; set; }
            public string Img { get; set; }
            public string Text { get; set; }
            public string Date { get; set; }

        }
        [WebMethod(EnableSession = true)]
        public string LentaSearch()
        {
            // [WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            ArchiveSearchStruct InData = JsonConvert.DeserializeObject<ArchiveSearchStruct>(rawJson);
            bool error = false;
            int i = 0;
            int count = 0;
            string Message = "";
            List<LentaFindNewsStruct> BlockData = new List<LentaFindNewsStruct>();

            // try
            {
                using (News.NewsDataContext dc = new News.NewsDataContext())
                {
                    if (dc.pWeb_ChekUserCookie(Convert.ToInt32(InData.Cookie)) < 0)
                        return "";
                    ;
                    // var c = dc.vWeb_ArchiveSearches;//.Where(a => a.ProgramId == Convert.ToInt32(InData.ProgramId));
                    var predicate = PredicateBuilder.False<News.vWeb_RssFeed>();

                    // predicate = predicate.Or(f=>f.id==0);
                    predicate = predicate.Or(a => a.Active == true);

                    if (InData.Text.Length > 0)
                    {
                        InData.Text = RemomeHtmlTag((string)InData.Text);
                        predicate = predicate.And(f => f.Name.Contains(InData.Text) || f.Lid.Contains(InData.Text));
                    }

                    if (InData.StartDate != null)
                    {
                        DateTime tmp = DateTime.Parse(InData.StartDate, null, System.Globalization.DateTimeStyles.RoundtripKind).AddDays(1).Date;

                        predicate = predicate.And(f => f.Date > tmp.Date);
                        // 
                    }
                    if (InData.EndDate != null)
                    {
                        DateTime tmp = DateTime.Parse(InData.EndDate, null, System.Globalization.DateTimeStyles.RoundtripKind).AddDays(1).Date;

                        predicate = predicate.And(f => f.Date < tmp.Date);
                    }
                    if (Convert.ToInt32(InData.EditorId) > 0)
                    {

                        predicate = predicate.And(f => f.SourceId == Convert.ToInt32(InData.EditorId));
                    }

                    var b = dc.vWeb_RssFeeds.AsQueryable().Where(predicate).OrderByDescending(d => d.Date);

                    foreach (var block in b)
                    {
                        i++;
                        if (i > 100)
                            break;
                        LentaFindNewsStruct item = new LentaFindNewsStruct();


                        item.Title = RemomeHtmlTag((string)block.Name);
                        item.Text = block.Lid;
                        if (InData.Text.Length > 0)
                        {
                            //item.NewsName=item.NewsName.Replace(InData.Text,"<span class='HiLightArchiveResult'>"+InData.Text+"</span>");

                            var regex = new Regex(InData.Text, RegexOptions.IgnoreCase);
                            item.Title = regex.Replace(item.Title, "<span class='HiLightArchiveResult'>" + InData.Text + "</span>");
                            item.Text = regex.Replace(item.Text, "<span class='HiLightArchiveResult'>" + InData.Text + "</span>");
                        }

                        item.Date = block.Date.ToString();
                        item.Id = block.id.ToString();
                        item.Img = block.ImgLink;
                        item.Link = block.Link;
                        item.AgencyName = block.Expr1;


                        BlockData.Add(item);

                    }/// foreach
                    count = b.Count();

                }
            }
            //catch(Exception ex)
            {
                error = true;
                //      Message = ex.Message;
            }
            var ret = new { Message = Message, Count = count, Overload = (bool)(i >= 100) ? "1" : "0", Error = error, Blocks = BlockData.ToList() };
            return js.Serialize(ret);


        }

        private string HiLiteFind(string Intext, string Pattern)
        {

            if (Pattern.Length > 0)
            {

                var regex = new Regex(Pattern, RegexOptions.IgnoreCase);
                Intext = regex.Replace(Intext, "<span class='HiLightArchiveResult'>" + Pattern + "</span>");
            }
            return Intext;
        }

        [WebMethod(EnableSession = true)]
        public string ArchiveSearchBlocks()
        {
            // [WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();
            ArchiveSearchStruct InData = JsonConvert.DeserializeObject<ArchiveSearchStruct>(rawJson);

            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {


                if (dc.pWeb_ChekUserCookie(Convert.ToInt32(InData.Cookie)) < 0)
                    return "";
                ;

                var predicate = PredicateBuilder.False<Blocks.vWeb_ArchiveFindBlock>();

                predicate = predicate.Or(a => a.Id == 0);

                List<string> Blocks = new List<string>();

                foreach (string sBlockId in InData.Text.Split(new char[] { ',' }, 100, StringSplitOptions.RemoveEmptyEntries))
                {
                    var blz = dc.vWeb_ArchiveFindBlocks.Where(b => b.Id == Convert.ToInt64(sBlockId));
                    if (blz.Count() > 0)
                    {
                        var bl = blz.First();
                        Blocks.Add(js.Serialize(new
                        {
                            BlockId = bl.Id,
                            BlockName = HiLiteFind(RemomeHtmlTag((string)bl.Name), RemomeHtmlTag((string)InData.EditorId)),
                            BlockText = HiLiteFind(RemomeHtmlTag((string)bl.BlockText), RemomeHtmlTag((string)InData.EditorId)),
                            BlockTypeName = bl.TypeName,
                            BlockAutor = bl.Autor,
                            BlockOperator = bl.Operator,
                            Ready = bl.Ready,
                            Approve = bl.Approve,
                            BlockTime = TimeSpan.FromSeconds((double)bl.BlockTime).ToString(),
                            TaskTime = TimeSpan.FromSeconds((double)bl.TaskTime).ToString(),
                        }));
                    }

                }


                return js.Serialize(Blocks.ToList());
            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string SendMessage()
        {
            // [WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            PingClass InData = JsonConvert.DeserializeObject<PingClass>(rawJson);

            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {

                if (Convert.ToInt32(InData.sNewsId) > 0)
                    dc.pWeb_SendMessage((int)Session["UserId"], Convert.ToInt32(InData.sNewsId), RemomeHtmlTag((string)InData.sGroupId));
                else
                {
                    dc.Users.Where(u => u.deleted == false).ToList().ForEach(l =>
                    {
                        dc.pWeb_SendMessage((int)Session["UserId"], l.UserID, RemomeHtmlTag((string)InData.sGroupId));

                    });
                }

            }

            return Convert.ToInt32(InData.sNewsId).ToString();
        }

        private string GetInMsg(int UserId)
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            lock (Application["dblook"])
            {
                using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                {

                    return js.Serialize(dc.vWeb_InMsgs.Where(n => n.to == UserId).ToList());
                }
            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string ReplyMessage()
        {
            // [WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";
            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            PingClass InData = JsonConvert.DeserializeObject<PingClass>(rawJson);
            if (InData.sGroupId == null)
                InData.sGroupId = "";

            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {

                dc.pWeb_ReplyMessage((int)Session["UserId"], Convert.ToInt32(InData.sNewsId), RemomeHtmlTag((string)InData.sGroupId));
            }



            return InData.sNewsId;
        }
        [WebMethod(EnableSession = true)]
        public string RssToNews()
        {
            // [WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";
            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            MoveNewsClass InData = JsonConvert.DeserializeObject<MoveNewsClass>(rawJson);

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                dc.pWeb_InsertRssToNews(Convert.ToInt32(InData.sSourceId), Convert.ToInt64(InData.sDestGroupId), (int)Session["UserId"]);
            }


            return "";
        }
        [WebMethod(EnableSession = true)]
        public string messagerUsersGet()
        {
            var ret = /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = "error" });
            if (HttpContext.Current.Session["UserId"] == null)
                return ret;

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                ret = /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    allUsers = dc.vWeb_UsertForMessagers.OrderBy(d => d.UserName).ToList(),
                    active = NFSocket.SendToAll.getActiveUsers()
                });
            }
            return ret;

        }
        [WebMethod(EnableSession = true)]
        public string GetMediaList()
        {
            // [WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            var InData = System.Web.Helpers.Json.Decode(rawJson);
            long ParentId = Convert.ToInt64(InData.NewsId);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {

                /*if(dc.pWeb_ChekUserCookie(Convert.ToInt32(InData.sCookie ))<0)
                {
                    return "";
                }*/
                if (InData.Archive == null || InData.Archive == false)
                {
                    lock (Application["dblook"])
                    {
                        var mnd = dc.vMedia_MediaToLists.Where(m => m.ParentId == ParentId).OrderBy(g => g.Name).OrderBy(mmm => mmm.Sort);
                        var list = mnd.Skip((int)Convert.ToInt32(InData.NewsGroupId)).Take(11).ToList();

                        var data = new { Media = list, Count = mnd.Count(), StartItem = InData.NewsGroupId };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        return js.Serialize(data);
                    }
                }
                else
                {
                    lock (Application["dblook"])
                    {
                        using (var dd = new Blocks.DataClassesMediaDataContext())
                        {
                            var mnd1 = dd.vWeb_ArchiveMediaForLists.Where(m => m.ParentId == ParentId).OrderBy(mmm => mmm.Sort);
                            var list = mnd1.Skip((int)Convert.ToInt32(InData.NewsGroupId)).Take(11).ToList();

                            var data = new { Media = list, Count = mnd1.Count(), StartItem = InData.NewsGroupId };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            return js.Serialize(data);
                        }
                    }
                }
            }
            return "";

        }
        [WebMethod(EnableSession = true)]
        public string GetMediaListFull()
        {
            // [WebMethod(EnableSession = true)]
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();

            var InData = System.Web.Helpers.Json.Decode(rawJson);
            long ParentId = Convert.ToInt64(InData.NewsId);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {

                /*if(dc.pWeb_ChekUserCookie(Convert.ToInt32(InData.sCookie ))<0)
                {
                    return "";
                }*/
                if (InData.Archive == null || InData.Archive == false)
                {
                    lock (Application["dblook"])
                    {
                        var mnd = dc.vMedia_MediaToLists.Where(m => m.ParentId == ParentId).OrderBy(g => g.Name).OrderBy(mmm => mmm.Sort);
                        var list = mnd.Skip((int)Convert.ToInt32(InData.NewsGroupId)).ToList();

                        var data = new { Media = list, Count = mnd.Count(), StartItem = InData.NewsGroupId };
                        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                        return js.Serialize(data);
                    }
                }
                else
                {
                    lock (Application["dblook"])
                    {
                        using (var dd = new Blocks.DataClassesMediaDataContext())
                        {
                            var mnd1 = dd.vWeb_ArchiveMediaForLists.Where(m => m.ParentId == ParentId).OrderBy(mmm => mmm.Sort);
                            var list = mnd1.Skip((int)Convert.ToInt32(InData.NewsGroupId)).ToList();

                            var data = new { Media = list, Count = mnd1.Count(), StartItem = InData.NewsGroupId };
                            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                            return js.Serialize(data);
                        }
                    }
                }
            }
            return "";

        }

        [WebMethod(EnableSession = true)]
        public string MediaAction()
        {
            // var param = handlers.utils.getAjaxResp(HttpContext.Current.Request.InputStream);
            if (Session["UserId"] == null)
                return "false";

            if (HttpContext.Current.Request.Params["Action"] == "Resort")
            {
                System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
                receiveStream.Position = 0;
                System.IO.StreamReader readStream =
                                   new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
                string rawJson = readStream.ReadToEnd();

                PingClass InData = JsonConvert.DeserializeObject<PingClass>(rawJson);
                return JsonConvert.SerializeObject(ResortMedia(InData.sNewsId));
            }
            if (HttpContext.Current.Request.Params["MediaId"] == null)
            {
                HttpContext.Current.Response.StatusCode = 505;
                HttpContext.Current.Response.Output.Write("No MediaId in Request");
                HttpContext.Current.Response.End();
                return "";
            }
            Int64 MediaId = Convert.ToInt64(HttpContext.Current.Request.Params["MediaId"]);
            if (HttpContext.Current.Request.Params["Action"] == null)
            {
                HttpContext.Current.Response.StatusCode = 505;
                HttpContext.Current.Response.Output.Write("No Action in Request");
                HttpContext.Current.Response.End();
                return "";
            }
            lock (Application["dblook"])
            {
                if (HttpContext.Current.Request.Params["Action"] == "ChangeStatus")
                {

                    bool ready = false;
                    bool approve = false;

                    if (HttpContext.Current.Request.Params["Ready"] == "true")
                    {
                        ready = true;
                    }
                    if (HttpContext.Current.Request.Params["Approve"] == "true")
                    {
                        approve = true;
                    }
                    Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext();
                    dc.pMedia_UpdateStatus(MediaId, ready, approve);
                    var medoas = dc.vWeb_MediaForLists.Where(m => m.Id == Convert.ToInt64(MediaId));
                    if (medoas.Count() > 0)
                    {
                        NFSocket.SendToAll.SendData("mediaStatusChange", new { blockId = medoas.First().ParentId, mediaId = MediaId, ready = ready, approve = approve });
                    }
                    return "Статус медиа изменен.";
                }

                if (HttpContext.Current.Request.Params["Action"] == "DeleteMedia")
                {
                    Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext();
                    dc.pMedia_Delete(MediaId);
                    //  var medoas = dc.vWeb_MediaForLists.Where(m => m.Id == Convert.ToInt64(MediaId));
                    // if (medoas.Count() > 0)
                    {
                        NFSocket.SendToAll.SendData("mediaDelete", new { mediaId = MediaId });
                    }
                    return "Удалено.";
                }
                if (HttpContext.Current.Request.Params["Action"] == "Rename")
                {
                    //  try
                    {
                        Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext();
                        dc.pMedia_Rename(MediaId, HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Params["Name"]));
                        var medoas = dc.vWeb_MediaForLists.Where(m => m.Id == Convert.ToInt64(MediaId));
                        if (medoas.Count() > 0)
                        {
                            NFSocket.SendToAll.SendData("mediaRename", new { blockId = medoas.First().ParentId, mediaId = MediaId });
                        }
                        return "Медиа переименовано.";
                    }
                    //  catch
                    {
                        HttpContext.Current.Response.StatusCode = 505;
                        HttpContext.Current.Response.Output.Write("Ошибка в переименовании медиа");
                        HttpContext.Current.Response.End();
                        return "";

                    }
                }
            }

            return "";
        }
        private string ResortMedia(string sIds)
        {

            using (Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
            {
                int i = 1;

                foreach (string MediaId in sIds.Split(','))
                {

                    dc.pMedia_UpdateSort(Convert.ToInt64(MediaId), (int)(i * 10));
                    i++;
                }
                var medoas = dc.vWeb_MediaForLists.Where(m => m.Id == Convert.ToInt64(sIds.Split(',').First()));
                if (medoas.Count() > 0)
                {
                    NFSocket.SendToAll.SendData("mediaResort", new { blockId = medoas.First().ParentId });
                }
            }

            return "Сортировка выполнена!";
        }
        [WebMethod(EnableSession = true)]
        public string GrantExtLink()
        {

            var param = handlers.utils.getAjaxResp(HttpContext.Current.Request.InputStream);
            if (Session["UserId"] == null)
                return "false";
            int UserId = (int)Session["UserId"];


            if (param.Enabled == false)
            {
                using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                {
                    dc.pWEB_DeleteTempLink(Convert.ToInt64(param.BlockId));
                    return "true";
                }

            }
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                Int64 blockId = Convert.ToInt64(param.BlockId);

                string GuidString = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                GuidString = GuidString.Replace("=", "");
                GuidString = GuidString.Replace("+", "");
                GuidString = GuidString.Replace("/", "");
                GuidString = GuidString.Substring(0, 8);
                var rec = dc.vWEB_ExtLinkToLists.Where(t => t.BlockId == blockId);
                if (rec.Count() == 0)
                    dc.pWeb_ExtListAdd(Convert.ToInt64(param.BlockId));
                else
                    GuidString = rec.First().URL;



                DateTime dt;
                try
                { dt = DateTime.Parse(param.ExpDate); }
                catch
                { dt = DateTime.Now.AddDays(1); }

                dc.pWeb_ExtLinkUpdate(blockId, UserId, param.IsExp, dt, GuidString, param.IsCommentable);

                string url = "http://" + dc.vWeb_Settings.Where(v => v.id == 10).First().value + "/NewsBlocks/" + GuidString + "/";
                var ret = new { Enabled = true, IsExp = param.IsExp, ExpDate = dt.ToString("dd.MM.yyyy"), Link = url, ExperienceWarnining = dt.Date < DateTime.Now.Date, IsCommentable = param.IsCommentable };
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(ret);


            }
            return "";
        }

        [WebMethod(EnableSession = true)]
        public string CheckExtLink()
        {


            var param = handlers.utils.getAjaxResp(HttpContext.Current.Request.InputStream);
            if (Session["UserId"] == null)
                return "false";
            int UserId = (int)Session["UserId"];

            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                Int64 blockId = Convert.ToInt64(param.BlockId);

                //IsCommentable

                var rec = dc.vWEB_ExtLinkToLists.Where(t => t.BlockId == blockId);
                if (rec.Count() == 0)
                    return "false";
                string url = "http://" + dc.vWeb_Settings.Where(v => v.id == 10).First().value + "/NewsBlocks/" + rec.First().URL + "/";
                var ret = new { Enabled = !rec.First().Deleted, IsExp = rec.First().IsExpirience, ExpDate = rec.First().ExpirienceDate.ToString("dd.MM.yyyy"), Link = url, ExperienceWarnining = rec.First().ExpirienceDate.Date < DateTime.Now.Date, IsCommentable = rec.First().IsCommentable };
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(ret);
            }
        }
        [WebMethod(EnableSession = true)]
        public string addPersonalFolder()
        {
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();
            var param = System.Web.Helpers.Json.Decode(rawJson);

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                var s = new Random().Next(100000);
                dc.CopyNews.InsertOnSubmit(new News.CopyNew()
                {
                    id = new Random().Next(100000000),
                    CalcTime = 0,
                    Cassete = "",
                    Deleted = false,
                    Description = "",
                    Duration = 0,
                    EditorId = (int)Session["UserId"],
                    GroupID = 1,
                    Name = "folder",
                    NewsDate = DateTime.Now,
                    NewsTime = 0,
                    ProgramId = Convert.ToInt32(param.ProgrammId),
                    TaskTime = 0,
                    Time_Code = 0,



                });
                dc.SubmitChanges();
                return "ok";
            }
        }

        public struct SyncTemplate
        {
            public string id;
            public string name;
            public string cap;
        }
        [WebMethod(EnableSession = true)]
        public string SyncTemplateGet()
        {

            if (Session["UserId"] == null)
                return "false";

            var items = new List<SyncTemplate>();
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.t_SynchTemplates.Where(t => t.Deleted == false).OrderByDescending(tt => tt.Name).ToList().ForEach(l =>
                {
                    items.Add(new SyncTemplate()
                    {
                        id = l.id,
                        name = l.Name,
                        cap = l.Caption
                    });

                });
            }

            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { items = items });
        }
        [WebMethod(EnableSession = true)]
        public string SyncTemplateAdd()
        {
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();
            var prm = System.Web.Helpers.Json.Decode(rawJson);

            var items = new List<SyncTemplate>();
            items.Add(new SyncTemplate()
            {
                id = Guid.NewGuid().ToString(),
                name = RemomeHtmlTag((string)prm.name),
                cap = RemomeHtmlTag((string)prm.cap)

            });
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.t_SynchTemplates.InsertOnSubmit(new Blocks.t_SynchTemplate()
                {
                    id = items[0].id,
                    Name = items[0].name,
                    Caption = items[0].cap
                });
                dc.SubmitChanges();

            }

            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { items = items });
        }
        [WebMethod(EnableSession = true)]
        public string geoTemplateDel() {
            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();
            var prm = System.Web.Helpers.Json.Decode(rawJson);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                int id = Convert.ToInt32( prm.id);
                var item = dc.t_BlockEditGeo.Where(l => l.id ==id).ToList().First();
                dc.t_BlockEditGeo.DeleteOnSubmit(item);
                dc.SubmitChanges();
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { item = dc.t_BlockEditGeo.OrderBy(l => l.id).ToList().Last() });
            }


        }
        [WebMethod(EnableSession = true)]
        public string geoTemplateEdit()
        {
            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();
            var prm = System.Web.Helpers.Json.Decode(rawJson);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                int id = Convert.ToInt32(prm.id);
                var name = prm.name;
                var item = dc.t_BlockEditGeo.Where(l => l.id == id).ToList();
                item[0].name = name;
               
                dc.SubmitChanges();
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { item = dc.t_BlockEditGeo.OrderBy(l => l.id).ToList().Last() });
            }


        }
        [WebMethod(EnableSession = true)]
        public string geoTemplateAdd() {
            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();
            var prm = System.Web.Helpers.Json.Decode(rawJson);

          
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.t_BlockEditGeo.InsertOnSubmit(new Blocks.t_BlockEditGeo()
                {
                    //id = items[0].id,
                    name = RemomeHtmlTag((string)prm.name),
                    description=""
                  
                });
                dc.SubmitChanges();
                
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new {item= dc.t_BlockEditGeo.OrderBy(l => l.id).ToList().Last() });

            }
        }
        /// <summary>
        /// ///////////////
        /// </summary>
        /// <returns></returns>

        [WebMethod(EnableSession = true)]
        public string srcTemplateDel()
        {
            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();
            var prm = System.Web.Helpers.Json.Decode(rawJson);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                int id = Convert.ToInt32(prm.id);
                var item = dc.t_BlockEditSrc.Where(l => l.id == id).ToList().First();
                dc.t_BlockEditSrc.DeleteOnSubmit(item);
                dc.SubmitChanges();
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { item = dc.t_BlockEditSrc.OrderBy(l => l.id).ToList().Last() });
            }


        }
        [WebMethod(EnableSession = true)]
        public string srcTemplateEdit()
        {
            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();
            var prm = System.Web.Helpers.Json.Decode(rawJson);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                int id = Convert.ToInt32(prm.id);
                var name = prm.name;
                var item = dc.t_BlockEditSrc.Where(l => l.id == id).ToList();
                item[0].name = name;

                dc.SubmitChanges();
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { item = dc.t_BlockEditSrc.OrderBy(l => l.id).ToList().Last() });
            }


        }
        [WebMethod(EnableSession = true)]
        public string srcTemplateAdd()
        {
            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();
            var prm = System.Web.Helpers.Json.Decode(rawJson);


            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.t_BlockEditSrc.InsertOnSubmit(new Blocks.t_BlockEditSrc()
                {
                    //id = items[0].id,
                    name = RemomeHtmlTag((string)prm.name),
                    description = ""

                });
                dc.SubmitChanges();

                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { item = dc.t_BlockEditSrc.OrderBy(l => l.id).ToList().Last() });

            }
        }



        [WebMethod(EnableSession = true)]
        public string SyncTemplateEdit()
        {
            if (Session["UserId"] == null)
                return "false";
            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();
            var prm = System.Web.Helpers.Json.Decode(rawJson);

            var items = new List<SyncTemplate>();
            items.Add(new SyncTemplate()
            {
                id = prm.id,
                name = RemomeHtmlTag((string)prm.name),
                cap = RemomeHtmlTag((string)prm.cap)

            });
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                var itm = dc.t_SynchTemplates.Where(q => q.id == items[0].id).First();


                itm.Name = items[0].name;
                itm.Caption = items[0].cap;

                dc.SubmitChanges();

            }

            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { items = items });
        }
        [WebMethod(EnableSession = true)]
        public string SyncTemplateDel()
        {
            if (Session["UserId"] == null)
                return "false";

            System.IO.Stream receiveStream = HttpContext.Current.Request.InputStream;
            receiveStream.Position = 0;
            System.IO.StreamReader readStream =
                               new System.IO.StreamReader(receiveStream, System.Text.Encoding.UTF8);
            string rawJson = readStream.ReadToEnd();
            var prm = System.Web.Helpers.Json.Decode(rawJson);

            var items = new List<SyncTemplate>();
            items.Add(new SyncTemplate()
            {
                id = prm.id,
                name = "",
                cap = ""

            });
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                var itm = dc.t_SynchTemplates.Where(q => q.id == items[0].id).First();


                itm.Deleted = true;
                itm.Caption = items[0].cap;

                dc.SubmitChanges();

            }

            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { items = items });
        }
        [WebMethod(EnableSession = true)]
        public string SynhTemplateGetName()
        {
            List<string> items = new List<string>();
            items.Add("aa");
            items.Add("bb");

            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { items = items });
        }

        [WebMethod(EnableSession = true)]
        public string passwordChange()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });
            var userId = Convert.ToInt32(Session["UserId"]);
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            string oldPass = data.oldPass;
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                var rs = dc.Users.Where(u => u.UserID == userId && u.pass == oldPass);
                if (rs.Count() > 0 && ((string)data.newPass).Length > 0)
                {
                    rs.First().pass = ((string)data.newPass);
                    dc.SubmitChanges();
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1 });
                }
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 0 });
        }
        [WebMethod(EnableSession = true)]
        public string readRateChange()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });
            var userId = Convert.ToInt32(Session["UserId"]);
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);

            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                var rs = dc.Users.Where(u => u.UserID == userId);
                if (rs.Count() > 0)
                {
                    try
                    {
                        rs.First().ReadRate = (Convert.ToInt32(data.readRate));
                        dc.SubmitChanges();
                        return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1 });
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 0 });
        }

        private struct statBlock
        {
            public string type;
            public string title;
            public Int64 autorId;
            public Int64 cameramenId;
            public Int64 jockeyId;
            public Int64 cutterId;
            public string NewsDate;
            public string newsName;


        }
        private struct statType
        {
            public Int64 typeId;
            public string typeName;
            public List<statBlock> blocks;
            public int count { get { return blocks == null ? 0 : blocks.Count(); } }
        }
        private struct statUser
        {
            public string name;
            public Int64 id;
            public List<statType> types;
            public int count { get { return types == null ? 0 : types.Sum(t => t.blocks.Count()); } }
        }
        [WebMethod(EnableSession = true)]
        public string statGet()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });
            var userId = Convert.ToInt32(Session["UserId"]);
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            var groupBy = (string)data.groupBy;

            DateTime dateStart = DateTime.Parse(data.dateStart);
            DateTime dateEnd = DateTime.Parse(data.dateEnd);

            // System.IO.File.AppendAllText("c:\\tmp\\nf.txt", dateStart.ToString("dd.MM.yyyy HH:mm:ss") + " " + dateEnd.ToString("dd.MM.yyyy HH:mm:ss"));

            List<statBlock> blocks = new List<statBlock>();
            var allUsers = new List<statUser>();


            using (News.NewsDataContext dcn = new News.NewsDataContext())
            {
                using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                {
                    dcn.News.Where(n => n.NewsDate.Date >= dateStart.Date && n.NewsDate.Date <= dateEnd && n.Deleted == false).OrderBy(nn => nn.NewsDate).ToList().ForEach(lNews =>
                    {

                        dcn.vBlocks.Where(b => b.NewsId == lNews.id && b.ParentId == 0).OrderBy(bb => bb.Sort).ToList().ForEach(lBl =>
                       {


                           blocks.Add(new statBlock()
                           {

                               autorId = lBl.CreatorId,
                               NewsDate = lNews.NewsDate.ToString("dd.MM.yyyy"),
                               newsName = StripHTML((string)lNews.Name),
                               title = StripHTML((string)lBl.Name),
                               type = lBl.TypeName,
                               cameramenId = lBl.OperatorId,
                               jockeyId = lBl.JockeyId,
                               cutterId = lBl.CutterId
                           });

                       });


                    });

                    /////////start Arcive////
                    dc.ArcNews.Where(n => n.NewsDate.Date >= dateStart.Date && n.NewsDate.Date <= dateEnd && n.Deleted == false).OrderBy(nn => nn.NewsDate).ToList().ForEach(lNews =>
                    {

                        dc.vArchBlock_for_lists.Where(b => b.NewsId == lNews.id && b.ParentId == 0).OrderBy(bb => bb.Sort).ToList().ForEach(lBl =>
                        {
                            blocks.Add(new statBlock()
                            {
                                autorId = lBl.CreatorId,
                                NewsDate = lNews.NewsDate.ToString("dd.MM.yyyy"),
                                newsName = StripHTML((string)lNews.Name),
                                title = StripHTML((string)lBl.Name),
                                type = lBl.TypeName,
                                cameramenId = lBl.OperatorId,
                                jockeyId = lBl.JockeyId,
                                cutterId = lBl.CutterId
                            });

                        });


                    });
                    ////////////
                    dcn.vWeb_Users.OrderBy(uuu => uuu.UserName).ToList().ForEach(lU =>
                    {
                        var su = new statUser()
                        {
                            id = lU.UserID,
                            name = lU.UserName,
                            types = new List<statType>()
                        };

                        dc.BlockTypes.OrderBy(bbt => bbt.TypeName).ToList().ForEach(btL =>
                        {
                            su.types.Add(new statType()
                            {
                                typeId = btL.id,
                                typeName = btL.TypeName,
                                blocks = new List<statBlock>()
                            });
                        });
                        allUsers.Add(su);

                    });

                    allUsers.ForEach(u =>
                    {

                        u.types.ForEach(t =>
                        {

                            ////                        
                            Int64 conf = 0;
                            blocks.ForEach(ll =>
                            {
                                switch (groupBy)
                                {
                                    case "author": { conf = ll.autorId; }; break;
                                    case "cameraman": { conf = ll.cameramenId; }; break;
                                    case "jockey": { conf = ll.jockeyId; }; break;
                                    case "cutter": { conf = ll.cutterId; }; break;
                                }
                                if (conf == u.id && t.typeName == ll.type)
                                {
                                    t.blocks.Add(ll);
                                }
                            });

                            ////
                            if (t.blocks.Count() == 0)
                                u.types.Remove(t);
                        });

                    });
                }
            }
            allUsers.ForEach(u =>
            {
                u.types.RemoveAll(t => t.count == 0);
            });
            allUsers.RemoveAll(u => u.count == 0);

            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(allUsers);
        }
        [WebMethod(EnableSession = true)]
        public string socialAdd()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });
            var userId = Convert.ToInt32(Session["UserId"]);
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);

            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.tSocial_Feeds.InsertOnSubmit(new Blocks.tSocial_Feed()
                {
                    id = Guid.NewGuid().ToString(),
                    authKey = "",
                    deleted = false,
                    title = "",
                    typeId = dc.tSocial_Types.First().id
                });
                dc.SubmitChanges();
            }
            return socialFeedGet();
        }
        [WebMethod(EnableSession = true)]
        public string rssAdd()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });
            // var userId = Convert.ToInt32(Session["UserId"]);
            //  var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            var ret = "";
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.tWeb_RssSources.InsertOnSubmit(
                    new Blocks.tWeb_RssSource()
                    {
                        Active = true,
                        LastGetTime = DateTime.Now,
                        Name = "",
                        URL = ""
                    }
                    );
                dc.SubmitChanges();
                ret = /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(dc.tWeb_RssSources.Where(r => r.Active == true).OrderBy(rr => rr.Name));
            }
            return ret;
        }
        [WebMethod(EnableSession = true)]
        public string socialDelete()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });
            var userId = Convert.ToInt32(Session["UserId"]);
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            string id = data.id;
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.tSocial_Feeds.Where(f => f.id == id).First().deleted = true;
                dc.SubmitChanges();
            }
            return socialFeedGet();
        }
        [WebMethod(EnableSession = true)]
        public string rssDelete()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });

            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 id = Convert.ToInt32(data.id);
            string ret = "";
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.tWeb_RssSources.Where(f => f.id == id).First().Active = false;
                dc.SubmitChanges();
                ret = /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(dc.tWeb_RssSources.Where(r => r.Active == true).OrderBy(rr => rr.Name));
            }
            return ret;
        }
        [WebMethod(EnableSession = true)]
        public string socialItemChange()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });
            var userId = Convert.ToInt32(Session["UserId"]);
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            string id = data.id;
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                var rec = dc.tSocial_Feeds.Where(f => f.id == id).First();
                rec.authKey = data.authKey;
                rec.title = data.title;
                rec.typeId = data.typeId;
                dc.SubmitChanges();
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = "ok" });// socialGetList();
        }
        [WebMethod(EnableSession = true)]
        public string rssItemChange()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });

            var userId = Convert.ToInt32(Session["UserId"]);
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 id = Convert.ToInt32(data.id);
            string ret = "";
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {

                var rec = dc.tWeb_RssSources.Where(f => f.id == id).First();
                rec.Name = data.name;
                rec.URL = data.Url;
                dc.SubmitChanges();
                ret = /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(dc.tWeb_RssSources.Where(r => r.Active == true).OrderBy(rr => rr.Name));

            }
            return ret;
        }
        [WebMethod(EnableSession = true)]
        public string socialInit()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });
            // var userId = Convert.ToInt32(Session["UserId"]);
            // var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            // Int64 blockId = Convert.ToInt64(data.id);
            return socialFeedGet();

        }
        [WebMethod(EnableSession = true)]
        public string rssInit()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });
            string ret = "";
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                ret = /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(dc.tWeb_RssSources.Where(r => r.Active == true).OrderBy(rr => rr.Name));
            }
            return ret;
        }
        private string socialFeedGet()
        {
            string ret = "";// ret = new List<socialFeed>();
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {

                ret = /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    feeds = dc.tSocial_Feeds.Where(f => f.deleted == false).OrderBy(ff => ff.title),
                    types = dc.tSocial_Types.Where(t => t.deleted == false).OrderBy(tt => tt.title)
                });

            }
            return ret;
        }
        [WebMethod(EnableSession = true)]
        public string socialFeedLoad()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });
            return socialGetList();
        }
        private struct socialFeed
        {
            public string id;
            public string title;
            public int count;
            public int error;
            public int status;
            public string img;
            public string feedArr;
        }
        [WebMethod(EnableSession = true)]
        public string socialGetList()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });

            var userId = Convert.ToInt32(Session["UserId"]);
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int64 blockId = Convert.ToInt64(data.id);

            var ret = new List<socialFeed>();
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.tSocial_Feeds.Where(f => f.deleted == false).OrderBy(ff => ff.title).ToList().ForEach(l =>
                {
                    if (dc.tSocial_Messages.Where(m => m.blockId == blockId).First().tSocial_feedToMessage.Where(fm => fm.feedId == l.id).Count() == 0)
                    {
                        /* dc.tSocial_feedToMessages.InsertOnSubmit(new Blocks.tSocial_feedToMessage()
                         {
                             id = Guid.NewGuid().ToString(),
                             feedId = l.id,
                             insertDate = DateTime.Now,
                             message = /*System.Web.Helpers.Newtonsoft.Json.JsonConvert.SerializeObject(new { message=""}),
                             socialMessId = dc.tSocial_Messages.Where(m => m.blockId == blockId).First().id,
                             status=0,
                             updateStatusDate=DateTime.Now
                         });
                         dc.SubmitChanges();*/
                        ret.Add(new socialFeed()
                        {
                            id = l.id,
                            count = 0,
                            error = 0,
                            status = 0,
                            title = l.title,
                            img = l.tSocial_Type.img,
                            feedArr = "[]"
                        });
                    }
                    else
                    {
                        var rec = dc.tSocial_Messages.Where(m => m.blockId == blockId).First().tSocial_feedToMessage.Where(fm => fm.feedId == l.id);
                        var lst = new List<dynamic>();
                        rec.ForEach(r =>
                        {
                            lst.Add(new
                            {
                                imgFile = r.imgFile,
                                insertDate = r.insertDate,
                                message = r.message,
                                id = r.id
                            });
                        });
                        ret.Add(new socialFeed()
                        {
                            id = l.id,
                            count = rec.Count(),
                            error = rec.OrderBy(re => re.updateStatusDate).Last().status > 0 ? 0 : 1,// rec.Where(rc=>rc.status<0).Count() ,
                            status = rec.OrderBy(re => re.updateStatusDate).Last().status,
                            title = l.title,
                            img = l.tSocial_Type.img,
                            feedArr = Newtonsoft.Json.JsonConvert.SerializeObject(lst)

                        });


                    };

                });
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(ret);

            }
        }
        [WebMethod(EnableSession = true)]
        public string socialMessageSave()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });

            try
            {
                var userId = Convert.ToInt32(Session["UserId"]);
                var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
                Int64 blockId = Convert.ToInt64(data.id);
                var title = StripHTML((string)data.title);
                var subTitle = StripHTML((string)data.subTitle);
                var message = StripHTML((string)data.message);
                var mediaId = Convert.ToInt64(StripHTML((string)data.mediaId));
                var mediaType = Convert.ToInt16(StripHTML((string)data.mediaType));
                if (title == null || subTitle == null || message == null)
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });

                using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                {
                    var rec = dc.tSocial_Messages.Where(m => m.blockId == blockId).First();
                    rec.title = title;
                    rec.subtitle = subTitle;
                    rec.message = message;
                    rec.mediaId = mediaId;
                    rec.mediaType = mediaType;
                    dc.SubmitChanges();
                }
                log(userId, 5, blockId, 5);
            }
            catch (Exception ex)
            {
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, err = ex.Message });
            }

            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = "ok" });
        }
        [WebMethod(EnableSession = true)]

        public string socialMessageCancel()
        {
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            var feedId = StripHTML((string)data.feedId);
            Int64 blockId = Convert.ToInt64(data.id);
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                var socialMessId = dc.tSocial_Messages.Where(m => m.blockId == blockId).First().id;

                dc.tSocial_feedToMessage.DeleteAllOnSubmit(dc.tSocial_feedToMessage.Where(m => m.feedId == feedId && m.socialMessId== socialMessId));
                dc.SubmitChanges();
            }
            return "0";
        }
        [WebMethod(EnableSession = true)]
        public string socialMessagePublish()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });

            var userId = Convert.ToInt32(Session["UserId"]);
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int64 blockId = Convert.ToInt64(data.id);
            var title = StripHTML((string)data.title);
            var subTitle = StripHTML((string)data.subTitle);
            var message = StripHTML((string)data.message);
            var mediaId = Convert.ToInt64(StripHTML((string)data.mediaId));
            var mediaType = Convert.ToInt16(StripHTML((string)data.mediaType));
            var feedId = StripHTML((string)data.feedId);
            var imgFile = data.imgFile;
            var id = Guid.NewGuid().ToString();
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                var socialMessId = dc.tSocial_Messages.Where(m => m.blockId == blockId).First().id;
                //var blockid=dc.tSocial_Messages.Where
                dc.tSocial_feedToMessage.DeleteAllOnSubmit(dc.tSocial_feedToMessage.Where(m => m.feedId == feedId && m.socialMessId== socialMessId));
                dc.SubmitChanges();
                var sm=dc.tSocial_Messages.Where(m => m.id == socialMessId).First();
                var type = sm.mediaType;
                var img = "";
                var video = "";
                if (sm.mediaId > 0) {
                    var medias = dc.Blocks.Where(b => b.Id == sm.mediaId);
                    if (medias.Count() > 0)
                    {
                        if (type == 2 || type==1)
                            img = medias.First().TextLang2;
                        if (type == 2)
                            video = medias.First().TextLang1;
                    }
                }
                if (imgFile != "") {
                    img = imgFile;
                }
               
                dc.tSocial_feedToMessage.InsertOnSubmit(new Blocks.tSocial_feedToMessage()
                {
                    id = id,
                    feedId = feedId,
                    insertDate = DateTime.Now,
                    message = /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new {
                        title = title,
                        subTitle = subTitle,
                        message = message,
                        mediaId = mediaId,
                        mediaType = mediaType,
                        type=type,
                        img=img,
                        video= video
                    }),
                    socialMessId = socialMessId,
                    status = 1,
                    publishCount = 0,
                    socialId = "",
                    socialError = "",
                    updateStatusDate = DateTime.Now,
                    imgFile= imgFile
                });
                dc.SubmitChanges();

            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = "ok", id = id });
        }
        [WebMethod(EnableSession = true)]
        public string socialGetHistory()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1 });

            var userId = Convert.ToInt32(Session["UserId"]);
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            string feedId = data.id;
            Int64 blockd = Convert.ToInt64(data.blockId);
            List<dynamic> ret = new List<dynamic>();
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.tSocial_feedToMessage.Where(ff => ff.feedId == feedId && ff.tSocial_Message.blockId == blockd).OrderByDescending(f => f.insertDate).ToList().ForEach(l =>
                {
                    dynamic d = new
                    {
                        id = l.id,
                        status = l.status,
                        eerMessage = StripHTML((string)l.socialError),
                        socialId = l.socialId,
                        date = l.updateStatusDate.ToString("dd.MM.yyyy HH:mm:ss"),
                        message = l.message,
                        link = l.tSocial_Feed.tSocial_Type.linkPrefix + l.socialId + l.tSocial_Feed.tSocial_Type.linkPostfix
                    };
                    ret.Add(d);

                });
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(ret);
        }
        [WebMethod(EnableSession = true)]
        public string blockHistoryNoTextGet()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            var userId = Convert.ToInt32(Session["UserId"]);
            Int64 id = Convert.ToInt64(handlers.utils.getAjaxResp(Context.Request.InputStream).id);
            List<dynamic> ret = new List<dynamic>();
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.tWeb_BlockHistories.Where(h => h.blockId == (id)).OrderByDescending(hh => hh.date).ToList().ForEach(l =>
                {
                    ret.Add(new
                    {
                        id = l.id,
                        date = l.date.ToString("dd.MM.yyyy HH:mm:ss"),
                        user = dc.fWeb_GetUserName(l.userId),
                        userid = l.userId
                    });
                });
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(ret);
        }
        [WebMethod(EnableSession = true)]
        public string stringblockHistoryTextGet()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            var userId = Convert.ToInt32(Session["UserId"]);
            string id = (handlers.utils.getAjaxResp(Context.Request.InputStream).id);

            string ret = "";
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                dc.tWeb_BlockHistories.Where(h => h.id == (id)).ToList().ForEach(l =>
                 {
                     ret = /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new
                     {
                         id = l.id,
                         date = l.date.ToString("dd.MM.yyyy HH:mm:ss"),
                         user = dc.fWeb_GetUserName(l.userId),
                         userid = l.userId,
                         text = handlers.utils.Unzip(l.blockZipText.ToArray())
                     });
                 });
            }
            return ret;
        }
        [WebMethod(EnableSession = true)]
        public string progInitAP()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", items = dc.Programs.Where(p => p.id > 0 && p.Deleted == false).OrderBy(pp => pp.Name) });
            }
        }
        [WebMethod(EnableSession = true)]
        public string progAdd()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                dc.Programs.InsertOnSubmit(
                    new News.Program()
                    {
                        Name = "",
                        Deleted = false,
                        Director = 0,
                        Rustv = false
                    });
                dc.SubmitChanges();
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", items = dc.Programs.Where(p => p.id > 0 && p.Deleted == false).OrderBy(pp => pp.Name) });
            }
        }
        [WebMethod(EnableSession = true)]
        public string progDel()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 id = Convert.ToInt32(data.id);
            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                try
                {
                    dc.Programs.Where(p => p.id == id).First().Deleted = true;
                    dc.SubmitChanges();
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", items = dc.Programs.Where(p => p.id > 0 && p.Deleted == false).OrderBy(pp => pp.Name) });
                }
                catch (Exception ex)
                {
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = -2, message = ex.Message });

                }
            }
        }
        [WebMethod(EnableSession = true)]
        public string progItemChange()
        {

            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 id = Convert.ToInt32(data.id);
            string name = ((string)RemomeHtmlTag(data.name));
            if (name.Length > 254)
                name = name.Substring(0, 254);
            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                try
                {
                    var rec = dc.Programs.Where(p => p.id == id).First();
                    rec.Deleted = false;
                    rec.Name = name;
                    dc.SubmitChanges();
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", id = id });
                }
                catch (Exception ex)
                {
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = -2, message = ex.Message });

                }
            }
        }
        [WebMethod(EnableSession = true)]
        public string roleInitAP()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", items = userRolesInProg(dc) });
            }
        }
        [WebMethod(EnableSession = true)]
        public string roleAdd()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                dc.URoles.InsertOnSubmit(new News.URole()
                {
                    URoleName = "",
                    URoleProgDepend = true,
                    URoleGroup = 1,
                    URoleDescription = "",
                    URoleUndelete = false
                });
                dc.SubmitChanges();
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", items = userRolesInProg(dc) });
            }
        }
        [WebMethod(EnableSession = true)]
        public string roleCopy()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 id = Convert.ToInt32(data.id);

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                var rec = dc.URoles.Where(u => u.URoleID == id).First();

                dc.URoles.InsertOnSubmit(new News.URole()
                {
                    URoleName = rec.URoleName + " copy",
                    URoleProgDepend = rec.URoleProgDepend,
                    URoleGroup = rec.URoleGroup,
                    URoleDescription = rec.URoleDescription,
                    URoleUndelete = false
                });
                dc.SubmitChanges();

                var newId = dc.URoles.Max(u => u.URoleID);
                dc.URightsToRoles.Where(r => r.URoleID == id).ToList().ForEach(l =>
                {

                    dc.URightsToRoles.InsertOnSubmit(new News.URightsToRole()
                    {
                        URightID = l.URightID,
                        URoleID = newId
                    });
                });
                dc.SubmitChanges();


                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", items = userRolesInProg(dc) });
            }
        }
        [WebMethod(EnableSession = true)]
        public string roleDel()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 id = Convert.ToInt32(data.id);

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                var rec = dc.URoles.Where(u => u.URoleID == id).First();
                dc.URightsToRoles.DeleteAllOnSubmit(dc.URightsToRoles.Where(r => r.URoleID == id));
                dc.SubmitChanges();

                dc.URoles.DeleteOnSubmit(rec);
                dc.SubmitChanges();

                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", items = userRolesInProg(dc) });
            }
        }
        [WebMethod(EnableSession = true)]
        public string roleItemChange()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 id = Convert.ToInt32(data.id);
            string name = ((string)RemomeHtmlTag((string)data.name));
            if (name.Length > 254)
                name = name.Substring(0, 254);
            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                try
                {
                    var rec = dc.URoles.Where(p => p.URoleID == id).First();
                    rec.URoleName = name;
                    dc.SubmitChanges();
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", id = id });
                }
                catch (Exception ex)
                {
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = -2, message = ex.Message });

                }
            }
        }
        [WebMethod(EnableSession = true)]
        public string roleGetForm()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 id = Convert.ToInt32(data.id);
            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                var ret = new List<dynamic>();
                {
                    dc.URights.Where(r => r.URightProgDepend == true && r.URightGroup != 9).OrderBy(rr => rr.URightGroup).ToList().ForEach(l =>
                    {
                        ret.Add(new
                        {
                            URightID = l.URightID,
                            title = l.URightName,
                            URightGroup = l.URightGroup,
                            value = dc.URightsToRoles.Where(rtr => rtr.URoleID == id && rtr.URightID == l.URightID).Count() != 0
                        });
                    });
                }

                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", items = ret });
            }

        }
        [WebMethod(EnableSession = true)]
        public string roleChange()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 URoleID = Convert.ToInt32(data.URoleID);
            Int32 URightID = Convert.ToInt32(data.URightID);
            Boolean val = Convert.ToBoolean(data.val);

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                if (val)
                {
                    try
                    {
                        dc.URightsToRoles.InsertOnSubmit(new News.URightsToRole()
                        {
                            URightID = URightID,
                            URoleID = URoleID
                        });
                        dc.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = ex.Message });
                    }
                }
                else
                {
                    try
                    {
                        dc.URightsToRoles.DeleteOnSubmit(dc.URightsToRoles.Where(r => r.URightID == URightID && r.URoleID == URoleID).First());
                        dc.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = ex.Message });
                    }

                }
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, message = "ok", id = URoleID });
            }
        }
        [WebMethod(EnableSession = true)]
        public string userInitAP()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                if (dc.vUsersRights.Where(u => u.UserID == Convert.ToInt32(Session["UserId"]) && u.RightID == 10).Count() == 0)
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -2, message = "you must be admin" });

                var ret = new List<dynamic>();
                dc.Users.Where(u => u.deleted == false).OrderBy(uu => uu.UserName).ToList().ForEach(l =>
                {
                    ret.Add(new
                    {
                        UserID = l.UserID,
                        name = l.UserName,
                        pass = l.pass
                    });
                });
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", items = ret });
            }
        }
        [WebMethod(EnableSession = true)]
        public string userAdd()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                if (dc.vUsersRights.Where(u => u.UserID == Convert.ToInt32(Session["UserId"]) && u.RightID == 10).Count() == 0)
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -2, message = "you must be admin" });

                dc.Users.InsertOnSubmit(new News.User()
                {
                    AbrigeBlockTypeId = 0,
                    Active = false,
                    BlockTypeId = 0,
                    deleted = false,
                    Enter = false,
                    Last_time = DateTime.Now,
                    OnlyMy = false,
                    pass = "",
                    PrintTemplateId = 0,
                    ReadRate = 17,
                    UserName = ""
                });
                dc.SubmitChanges();

                var ret = new List<dynamic>();
                dc.Users.Where(u => u.deleted == false).OrderBy(uu => uu.UserName).ToList().ForEach(l =>
                {
                    ret.Add(new
                    {
                        UserID = l.UserID,
                        name = l.UserName,
                        pass = l.pass
                    });
                });
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", items = ret });
            }
        }
        [WebMethod(EnableSession = true)]
        public string userDel()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 id = Convert.ToInt32(data.id);

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                if (dc.vUsersRights.Where(u => u.UserID == Convert.ToInt32(Session["UserId"]) && u.RightID == 10).Count() == 0)
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -2, message = "you must be admin" });

                dc.Users.Where(u => u.UserID == id).First().deleted = true;

                dc.SubmitChanges();

                var ret = new List<dynamic>();
                dc.Users.Where(u => u.deleted == false).OrderBy(uu => uu.UserName).ToList().ForEach(l =>
                {
                    ret.Add(new
                    {
                        UserID = l.UserID,
                        name = l.UserName,
                        pass = l.pass
                    });
                });
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", items = ret });
            }
        }
        [WebMethod(EnableSession = true)]
        public string userItemChange()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 id = Convert.ToInt32(data.id);
            string name = ((string)RemomeHtmlTag((string)data.name));
            if (name.Length > 254)
                name = name.Substring(0, 254);
            string pass = data.pass;


            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                if (dc.vUsersRights.Where(u => u.UserID == Convert.ToInt32(Session["UserId"]) && u.RightID == 10).Count() == 0)
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -2, message = "you must be admin" });
                var rec = dc.Users.Where(u => u.UserID == id).First();

                rec.UserName = name;
                rec.pass = pass;
                dc.SubmitChanges();
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", id = id });
            }
        }
        [WebMethod(EnableSession = true)]
        public string userCopy()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 id = Convert.ToInt32(data.id);
            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                if (dc.vUsersRights.Where(u => u.UserID == Convert.ToInt32(Session["UserId"]) && u.RightID == 10).Count() == 0)
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -2, message = "you must be admin" });
                var rec = dc.Users.Where(u => u.UserID == id).First();
                dc.Users.InsertOnSubmit(new News.User()
                {
                    AbrigeBlockTypeId = rec.AbrigeBlockTypeId,
                    Active = false,
                    BlockTypeId = rec.BlockTypeId,
                    deleted = false,
                    Enter = rec.Enter,
                    Last_time = DateTime.Now,
                    OnlyMy = rec.OnlyMy,
                    pass = rec.pass,
                    PrintTemplateId = rec.PrintTemplateId,
                    ReadRate = rec.ReadRate,
                    UserName = rec.UserName + " copy"
                });
                dc.SubmitChanges();
                var newId = dc.Users.Max(u => u.UserID);

                dc.UUserToPrograms.Where(u => u.UserID == id).ToList().ForEach(l =>
                {
                    dc.UUserToPrograms.InsertOnSubmit(new News.UUserToProgram()
                    {
                        ProgramID = l.ProgramID,
                        UserID = newId,
                        URoleID = l.URoleID
                    });
                });
                dc.SubmitChanges();

                var ret = new List<dynamic>();
                dc.Users.Where(u => u.deleted == false).OrderBy(uu => uu.UserName).ToList().ForEach(l =>
                {
                    ret.Add(new
                    {
                        UserID = l.UserID,
                        name = l.UserName,
                        pass = l.pass
                    });
                });
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", items = ret });
            }

        }
        [WebMethod(EnableSession = true)]
        public string userGetForm()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 id = Convert.ToInt32(data.id);
            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                if (dc.vUsersRights.Where(u => u.UserID == Convert.ToInt32(Session["UserId"]) && u.RightID == 10).Count() == 0)
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -2, message = "you must be admin" });
                var rec = dc.Users.Where(u => u.UserID == id).First();

                var admRoles = new List<dynamic>();
                dc.URoles.Where(r => r.URoleID == 1 || r.URoleID == 25).ToList().ForEach(l =>
                {
                    admRoles.Add(new
                    {
                        roleId = l.URoleID,
                        roleName = l.URoleName,
                        value = dc.UUserToPrograms.Where(u => u.UserID == id && u.URoleID == l.URoleID).Count() > 0
                    });
                });
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", admRoles = admRoles });

            }

        }
        [WebMethod(EnableSession = true)]
        public string userAdminChange()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 URoleID = Convert.ToInt32(data.URoleID);
            Int32 UserID = Convert.ToInt32(data.UserID);
            Boolean val = Convert.ToBoolean(data.val);

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                if (dc.vUsersRights.Where(u => u.UserID == Convert.ToInt32(Session["UserId"]) && u.RightID == 10).Count() == 0)
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -2, message = "you must be admin" });

                if (val)
                {
                    try
                    {
                        dc.UUserToPrograms.InsertOnSubmit(new News.UUserToProgram()
                        {
                            UserID = UserID,
                            URoleID = URoleID
                        });
                        dc.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = ex.Message });
                    }
                }
                else
                {
                    try
                    {
                        dc.UUserToPrograms.DeleteOnSubmit(dc.UUserToPrograms.Where(r => r.UserID == UserID && r.URoleID == URoleID).First());
                        dc.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = ex.Message });
                    }

                }
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, message = "ok", id = URoleID });
            }


        }
        [WebMethod(EnableSession = true)]
        public string userGetFormProg()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 progId = Convert.ToInt32(data.progId);
            Int32 userId = Convert.ToInt32(data.userId);


            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                if (dc.vUsersRights.Where(u => u.UserID == Convert.ToInt32(Session["UserId"]) && u.RightID == 10).Count() == 0)
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -2, message = "you must be admin" });

                var retO = new List<dynamic>();
                dc.URoles.Where(r => r.URoleGroup == 3 || r.URoleGroup == 2 || r.URoleGroup == 19).OrderBy(uu => uu.URoleName).ToList().ForEach(l =>
                {
                    retO.Add(
                       new
                       {
                           roleId = l.URoleID,
                           name = l.URoleName,
                           value = dc.UUserToPrograms.Where(up => up.UserID == userId && up.URoleID == l.URoleID && up.ProgramID == progId).Count() > 0
                       }
                    );
                });
                var retRoles = new List<dynamic>();
                retRoles.Add(new { roleId = -1, name = "no role", val = false });
                userRolesInProg(dc).ToList().ForEach(l =>
                {
                    retRoles.Add(new
                    {
                        roleId = l.URoleID,
                        name = l.URoleName,
                        val = dc.UUserToPrograms.Where(uu => uu.UserID == userId && uu.URoleID == l.URoleID && uu.ProgramID == progId).Count() > 0
                    });
                });
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, message = "ok", o = retO, roles = retRoles });
            }
        }
        [WebMethod(EnableSession = true)]
        public string userRightOChange()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 progId = Convert.ToInt32(data.progId);
            Int32 userId = Convert.ToInt32(data.userId);
            Int32 roleId = Convert.ToInt32(data.roleId);
            Boolean val = Convert.ToBoolean(data.val);


            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                if (dc.vUsersRights.Where(u => u.UserID == Convert.ToInt32(Session["UserId"]) && u.RightID == 10).Count() == 0)
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -2, message = "you must be admin" });
                try
                {
                    if (val)
                    {
                        dc.UUserToPrograms.InsertOnSubmit(new News.UUserToProgram()
                        {
                            ProgramID = progId,
                            URoleID = roleId,
                            UserID = userId
                        });
                    }
                    else
                    {
                        dc.UUserToPrograms.DeleteOnSubmit(dc.UUserToPrograms.Where(u => u.ProgramID == progId && u.UserID == userId && u.URoleID == roleId).First());
                    }
                    dc.SubmitChanges();
                }
                catch (Exception ex)
                {
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -2, message = ex.Message });
                }

                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, message = "ok" });

            }
        }
        private IQueryable<News.URole> userRolesInProg(News.NewsDataContext dc)
        {
            return dc.URoles.Where(r => r.URoleGroup == 1).OrderBy(uu => uu.URoleName);
        }
        [WebMethod(EnableSession = true)]
        public string userRoleChange()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 progId = Convert.ToInt32(data.progId);
            Int32 userId = Convert.ToInt32(data.userId);
            Int32 roleId = Convert.ToInt32(data.roleId);

            using (News.NewsDataContext dc = new News.NewsDataContext())
            {
                if (dc.vUsersRights.Where(u => u.UserID == Convert.ToInt32(Session["UserId"]) && u.RightID == 10).Count() == 0)
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -2, message = "you must be admin" });
                userRolesInProg(dc).ToList().ForEach(l =>
                {
                    var rec = dc.UUserToPrograms.Where(u => u.ProgramID == progId && u.UserID == userId && u.URoleID == l.URoleID);
                    if (rec.Count() > 0)
                    {
                        dc.UUserToPrograms.DeleteOnSubmit(rec.First());
                        dc.SubmitChanges();
                    }

                });
                if (roleId >= 0)
                {
                    dc.UUserToPrograms.InsertOnSubmit(new News.UUserToProgram()
                    {

                        ProgramID = progId,
                        URoleID = roleId,
                        UserID = userId
                    });
                    dc.SubmitChanges();
                }
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, message = "ok" });
            }
            //  userRolesInProg(dc)
        }
        [WebMethod(EnableSession = true)]
        public string blockTypeInitAP()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", items = dc.BlockTypes.Where(bt => bt.Extern == false).OrderBy(t => t.TypeName) });
            }
        }
        [WebMethod(EnableSession = true)]
        public string blockTypeEdit()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);

            int id = Convert.ToInt32(data.id);
            string title = RemomeHtmlTag((string)data.title);
            bool isAutor = Convert.ToBoolean(data.isAutor);
            bool isCameramen = Convert.ToBoolean(data.isCameramen);
            bool isJockey = Convert.ToBoolean(data.isJockey);

            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                try
                {
                    var rec = dc.BlockTypes.Where(t => t.id == id).First();
                    rec.Jockey = isJockey;
                    rec.Autor = isAutor;
                    rec.Operator = isCameramen;
                    rec.TypeName = title;
                    dc.SubmitChanges();
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, id = id });
                }
                catch (Exception ex)
                {
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -2, message = ex.Message });
                }
            }
        }
        [WebMethod(EnableSession = true)]
        public string blockTypeAdd()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                try
                {
                    dc.BlockTypes.InsertOnSubmit(new Blocks.BlockType()
                    {
                        TypeName = "",
                        Operator = false,
                        Autor = false,
                        Jockey = false,
                        Extern = false,
                        Description = ""
                    });
                    dc.SubmitChanges();
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, id = dc.BlockTypes.Max(t => t.id), items = dc.BlockTypes.Where(bt => bt.Extern == false).OrderBy(t => t.TypeName) });
                }
                catch (Exception ex)
                {
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -2, message = ex.Message });
                }
            }
        }
        [WebMethod(EnableSession = true)]
        public string printTemplateInitAP()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (PrintTemplates.PrintTemplatesDataClassesDataContext dc = new PrintTemplates.PrintTemplatesDataClassesDataContext())
            {
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, message = "ok", items = dc.PrintTemplates.OrderBy(d => d.name).ToList() });
            }
        }
        [WebMethod(EnableSession = true)]
        public string playOutInitAP()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    staus = 1,
                    message = "ok",
                    items = dc.tWeb_PlayOuts.Where(r => r.isDeleted == false).OrderBy(r => r.Title).Select(r => new { id = r.id, typeId = r.typeId, title = r.Title, path = r.Path, url = r.URL, replace = r.replacePath, SrtPrefix = r.SrtPrefix }),
                    types = dc.tWeb_PlayOutsType.Where(rr => rr.isDeleted == false).OrderBy(r => r.Title)
                });
            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string playOutAdd()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                dc.tWeb_PlayOuts.InsertOnSubmit(new Blocks.tWeb_PlayOuts()
                {
                    id = Guid.NewGuid().ToString(),
                    Title = "",
                    isDeleted = false,
                    Path = "",
                    URL = "",
                    SrtPrefix = "",

                    typeId = dc.tWeb_PlayOutsType.Where(t => t.isDeleted == false).First().id,
                    replacePath = ""
                });
                dc.SubmitChanges();
                return playOutInitAP();
            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string playOutdelete(string id)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                dc.tWeb_PlayOuts.Where(r => r.id == id).First().isDeleted = true;
                dc.SubmitChanges();
                return JsonConvert.SerializeObject(new { status = 1, id = id });

            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string playOutChange(string id, string path, string title, string url, string replace, string type, string SrtPrefix)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                var rec = dc.tWeb_PlayOuts.Where(r => r.id == id).First();
                rec.Title = RemomeHtmlTag(title);
                rec.Path = RemomeHtmlTag(path);
                rec.URL = RemomeHtmlTag(url);
                rec.typeId = type;
                rec.replacePath = RemomeHtmlTag(replace);
                rec.SrtPrefix = RemomeHtmlTag(SrtPrefix);
                dc.SubmitChanges();
                return JsonConvert.SerializeObject(new { status = 1, id = id });
            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string playOutGetList()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                var rec = dc.tWeb_PlayOuts.OrderBy(r => r.Title).Where(r => r.isDeleted == false);
                return JsonConvert.SerializeObject(rec.Select(r => new { id = r.id, title = r.Title }));
            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string playOutSend(string newsid, string serverid)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                var taskId = Guid.NewGuid().ToString();
                dc.tWeb_PlayOutTasks.InsertOnSubmit(new Blocks.tWeb_PlayOutTasks()
                {
                    id = taskId,
                    dateCreate = DateTime.Now,
                    description = "",
                    serverId = serverid,
                    status = 0,
                    newsId = Convert.ToInt64(newsid)
                });
                dc.SubmitChanges();
                var blocks = dc.Blocks.Where(b => b.deleted == false && b.NewsId.ToString() == newsid && b.ParentId == 0).OrderBy(b => b.Sort);


                blocks.ForEach(l =>
                {
                    dc.Blocks.Where(b => b.deleted == false && b.ParentId == l.Id && b.BLockType > 0 && b.Approve == true).OrderBy(bb => bb.Sort).ToList().ForEach(sb =>
                          {
                              if (System.IO.File.Exists(sb.BlockText))
                              {
                                  var fi = new System.IO.FileInfo(sb.BlockText);

                             // new { id = sb.Id , title=sb.Description, file=sb.BlockText});
                             if (fi.Length > 0)
                                  {
                                      dc.tWeb_PlayOutTaskFiles.InsertOnSubmit(new Blocks.tWeb_PlayOutTaskFiles()
                                      {
                                          id = Guid.NewGuid().ToString(),
                                          bytesSend = 0,
                                          date = DateTime.Now,
                                          description = 0,
                                          fileName = sb.BlockText,
                                          fileTitle = sb.Description,
                                          status = 0,
                                          taskId = taskId,
                                          bytes = (new System.IO.FileInfo(sb.BlockText)).Length,
                                          sort = l.Sort * 10000 + sb.Sort,
                                          subBlockId = sb.Id
                                      });
                                      dc.SubmitChanges();
                                  }
                              }
                          });

                });

                return JsonConvert.SerializeObject(new { status = 1, id = taskId });
            }
        }
        [WebMethod(EnableSession = true)]
        public string printTemplAdd()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (PrintTemplates.PrintTemplatesDataClassesDataContext dc = new PrintTemplates.PrintTemplatesDataClassesDataContext())
            {
                dc.PrintTemplates.InsertOnSubmit(new PrintTemplates.PrintTemplate()
                {
                    name = "",
                    news = "",
                    block = "",
                    block_flag = "",
                    depended_block = "",
                    description = ""
                });
                dc.SubmitChanges();
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, id = dc.PrintTemplates.Max(d => d.id), items = dc.PrintTemplates.OrderBy(d => d.name).ToList() });
            }
        }
        [WebMethod(EnableSession = true)]
        public string printTemplateDel()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);
            Int32 id = Convert.ToInt32(data.id);
            using (PrintTemplates.PrintTemplatesDataClassesDataContext dc = new PrintTemplates.PrintTemplatesDataClassesDataContext())
            {
                dc.PrintTemplates.DeleteOnSubmit(dc.PrintTemplates.Where(u => u.id == id).First());
                dc.SubmitChanges();
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, id = dc.PrintTemplates.Max(d => d.id), items = dc.PrintTemplates.OrderBy(d => d.name).ToList() });
            }
        }
        [WebMethod(EnableSession = true)]
        public string printTemplateChangeText()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);

            int id = Convert.ToInt32(data.id);
            using (PrintTemplates.PrintTemplatesDataClassesDataContext dc = new PrintTemplates.PrintTemplatesDataClassesDataContext())
            {
                dc.PrintTemplates.Where(u => u.id == id).First().name = RemomeHtmlTag((string)data.val);
                dc.SubmitChanges();
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, id = dc.PrintTemplates.Max(d => d.id), items = dc.PrintTemplates.OrderBy(d => d.name).ToList() });
            }

        }
        [WebMethod(EnableSession = true)]
        public string printTemplCopy()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);

            int id = Convert.ToInt32(data.id);
            using (PrintTemplates.PrintTemplatesDataClassesDataContext dc = new PrintTemplates.PrintTemplatesDataClassesDataContext())
            {
                var rec = dc.PrintTemplates.Where(u => u.id == id).First();
                dc.PrintTemplates.InsertOnSubmit(new PrintTemplates.PrintTemplate()
                {
                    name = rec.name + " - copy",
                    news = rec.news,
                    block = rec.block,
                    block_flag = rec.block_flag,
                    depended_block = rec.depended_block,
                    description = rec.description
                });
                dc.SubmitChanges();
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, id = dc.PrintTemplates.Max(d => d.id), items = dc.PrintTemplates.OrderBy(d => d.name).ToList() });
            }
        }
        [WebMethod(EnableSession = true)]
        public string printTemplLoad()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);

            int id = Convert.ToInt32(data.id);
            using (PrintTemplates.PrintTemplatesDataClassesDataContext dc = new PrintTemplates.PrintTemplatesDataClassesDataContext())
            {
                var rec = dc.PrintTemplates.Where(u => u.id == id);

                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, items = rec });
            }
        }
        [WebMethod(EnableSession = true)]
        public string printTempSave()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);

            int id = Convert.ToInt32(data.id);
            using (PrintTemplates.PrintTemplatesDataClassesDataContext dc = new PrintTemplates.PrintTemplatesDataClassesDataContext())
            {
                var rec = dc.PrintTemplates.Where(u => u.id == id).First();
                rec.block = data.block;
                rec.news = data.news;
                dc.SubmitChanges();

                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { staus = 1, id = id });
            }
        }

        //3,2,29 group
        public static void log(int userId, int eventId, Int64 ItemId, int sect = 0)
        {
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                try
                {
                    dc.pWeb_log(userId, sect, ItemId, eventId);
                }
                catch { }
            }
        }

        [WebMethod(EnableSession = true)]
        public string fileUploadGetNewFolder()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);

            long id = Convert.ToInt64(data.blockId);
            var lst = (fileUpload.UploadInfo)Application["fileUpload"];
            var baseName = "";
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                try
                {
                    baseName = dc.Blocks.Where(v => v.Id == id).First().Name;
                    baseName = Translit(baseName);

                    Regex r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                    baseName = r.Replace(baseName, String.Empty);
                    baseName = baseName.Replace(" ", "_");

                }
                catch (Exception ex)
                {
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, err = ex.Message });
                }
            }
            baseName += "_" + Guid.NewGuid().ToString().Substring(0, 8);
            lock (Application["fileUpload"])
            {
                var folderId = lst.addFolder(id, baseName);

                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, id = folderId });
            }
        }
        private static string Translit(string str)
        {
            string[] lat_up = { "A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "Kh", "Ts", "Ch", "Sh", "Shch", "\"", "Y", "'", "E", "Yu", "Ya" };
            string[] lat_low = { "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "\"", "y", "'", "e", "yu", "ya" };
            string[] rus_up = { "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };
            string[] rus_low = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };
            for (int i = 0; i <= 32; i++)
            {
                str = str.Replace(rus_up[i], lat_up[i]);
                str = str.Replace(rus_low[i], lat_low[i]);
            }
            return str;
        }
        [WebMethod(EnableSession = true)]
        public string fileUploadGetNewFile()
        {
            if (Session["UserId"] == null)
                return System.Web.Helpers.Json.Encode(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);

            string folderId = (data.folderId);
            var fileInfo = data.fileInfo;
            var test = fileInfo;
            var name = fileInfo.name;
            var size = fileInfo.size;

            var lst = (fileUpload.UploadInfo)Application["fileUpload"];
            lock (Application["fileUpload"])
            {
                var fileId = lst.addFile((string)folderId, (string)name, Convert.ToInt64(size));

                return System.Web.Helpers.Json.Encode(new { status = 1, id = fileId, i = data.i, folderId = fileInfo.folderId });
            }
        }
        [WebMethod(EnableSession = true)]
        public string fileUploadFileComplite()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var data = handlers.utils.getAjaxResp(Context.Request.InputStream);

            string fileId = (data.fileId);
            var lst = (fileUpload.UploadInfo)Application["fileUpload"];
            string filepath = lst.getFilePath(fileId);
            long blockId = lst.getBlockId(fileId);
            string origFileName = lst.getFileOriginal(fileId);

            int BlockType = 0;
            string mime = System.Web.MimeMapping.GetMimeMapping(filepath);
            if (mime.IndexOf("image") == 0)
            {
                BlockType = 1;
            }
            if (mime.IndexOf("video") == 0)
            {
                BlockType = 2;
            }
            int taskId = 0;
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                lock (Application["fileUpload"])
                {
                    taskId = dc.pWeb_InsertMediaToBlock(blockId, filepath, origFileName, BlockType);
                    dc.pMedia_SetTaskUploadComplite(taskId);
                    lst.fileComplited(fileId);
                }
            }

            System.Threading.Thread.Sleep(1000);
            NFSocket.SendToAll.SendData("mediaUploadComplite", new { blockId = blockId });

            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, id = fileId });
        }
        [WebMethod(EnableSession = true)]
        public string graphicsLayerGet()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var ret = new List<dynamic>();
            using (var dc = new Blocks.DataClassesMediaDataContext())
            {
                dc.ExecuteCommand(System.IO.File.ReadAllText(Server.MapPath("/dbUpdates/tWeb_GraphicsLayers.sql")));
                dc.tWeb_GraphicsLayers.Where(g => g.deleted == false).ToList().ForEach(l =>
                {
                    ret.Add(new { id = l.id.ToString(), title = l.title });
                });
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(ret);
        }
        [WebMethod(EnableSession = true)]
        public string mediaGraphisGet(long id, long mediaId)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });


            using (var dc = new Blocks.DataClassesMediaDataContext())
            {
                var rec = dc.tWeb_mediaGraphics.Where(g => g.layerId == id && g.MediaId == mediaId).ToList();
                if (rec.Count() == 0)
                {
                    dc.tWeb_mediaGraphics.InsertOnSubmit(new Blocks.tWeb_mediaGraphic()
                    {
                        id = Guid.NewGuid().ToString(),
                        layerId = Convert.ToInt32(id),
                        MediaId = Convert.ToInt64(mediaId)

                    });
                    dc.SubmitChanges();
                }
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(dc.tWeb_mediaGraphics.Where(g => g.layerId == id && g.MediaId == mediaId).First().tWeb_MediaGraphicsItems.OrderByDescending(ff => ff.timeInSec).ToList());
            }

        }
        [WebMethod(EnableSession = true)]
        public string mediaGraphisNewList(int id, long mediaId, dynamic items)
        {


            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClassesMediaDataContext())
            {
                var mediaGraphicsId = dc.tWeb_mediaGraphics.Where(g => g.layerId == id && g.MediaId == mediaId).First().id;


                List<string> oldItems = new List<string>();

                dc.tWeb_MediaGraphicsItems.Where(f => f.mediaGraphicsId == mediaGraphicsId).ToList().ForEach(l =>
                {
                    oldItems.Add(l.id);
                });

                try
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        var bl = new Blocks.tWeb_MediaGraphicsItem();
                        bl.id = Guid.NewGuid().ToString();
                        bl.text = items[i]["text"];
                        bl.timeInSec = items[i]["time"];
                        bl.mediaGraphicsId = mediaGraphicsId;

                        dc.tWeb_MediaGraphicsItems.InsertOnSubmit(bl);
                    }
                    dc.SubmitChanges();
                    // var mediaId=dc.tWeb_MediaGraphicsItems.Where(f => f.mediaGraphicsId == mediaGraphicsId).First().tWeb_mediaGraphic.id;

                    //dc.tWeb_MediaGraphicsItems.DeleteAllOnSubmit(oldItems);
                    oldItems.ForEach(l =>
                    {
                        dc.tWeb_MediaGraphicsItems.DeleteOnSubmit(dc.tWeb_MediaGraphicsItems.Where(f => f.id == l).First());
                        dc.SubmitChanges();
                    });
                    dc.SubmitChanges();
                    var blockId = dc.vWeb_MediaForLists.Where(bl => bl.Id == mediaId).First().Id;
                    NFSocket.SendToAll.SendData("mediaGraphicsChange", new { blockId = blockId, mediaId = mediaId });

                }
                catch (Exception ex) {
                }
               

                
               
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(getGraphicsByList(mediaGraphicsId));
            }

        }
        private List<dynamic> getGraphicsByList(string mediaGraphicsId)
        {
            var ret = new List<dynamic>();
            using (var dc = new Blocks.DataClassesMediaDataContext())
            {
                var b = dc.tWeb_MediaGraphicsItems.Where(f => f.mediaGraphicsId == mediaGraphicsId);

                b = b.OrderByDescending(ff => ff.timeInSec);
                var a = b.ToList();

                for (int i = 0; i < a.Count; i++)
                {
                    ret.Add(new { id = a[i].id, text = a[i].text, timeInSec = a[i].timeInSec, endtimeInSec = (i < a.Count - 1) ? a[i + 1].timeInSec : (60 * 60 * 24) });
                }

            }

            return ret;
        }

        [WebMethod(EnableSession = true)]
        public string getBlockText(long id)
        {
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                try
                {
                    var rec = dc.Blocks.Where(b => b.Id == id).First();
                    var readrate = 17;
                    try
                    {
                        var type = dc.BlockTypes.Single(t => t.id == rec.BLockType);
                        if (type.Jockey && rec.JockeyId > 0)
                            readrate = dc.Users.Single(u => u.UserID == rec.JockeyId).ReadRate;
                        else if (rec.CreatorId > 0)
                            readrate = dc.Users.Single(u => u.UserID == rec.CreatorId).ReadRate;
                    }
                    catch { }
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { id = id, text = rec.BlockText, status = 1, readrate = readrate });

                }
                catch
                {
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { id = id, text = "", status = -1 });
                }
            }
            return "";

        }
        [WebMethod(EnableSession = true)]
        public string
        updateBlockText(long id, string text, long calcTime)
        {

            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                try
                {
                    var rec = dc.Blocks.Where(b => b.Id == id).First();
                    rec.BlockText = text;
                    rec.CalcTime = calcTime;
                    dc.SubmitChanges();
                    dc.tWeb_BlockHistories.InsertOnSubmit(new Blocks.tWeb_BlockHistory
                    {
                        id = Guid.NewGuid().ToString(),
                        date = DateTime.Now,
                        blockZipText = handlers.utils.Zip(text),
                        blockId = id,
                        userId = Convert.ToInt32(Session["UserId"])
                    });
                    dc.SubmitChanges();
                    dc.sp_UpdateNewsHrono(id);

                }
                catch
                {
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { id = id, text = text, status = -1 });
                }
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { id = id, text = text, status = 1 });
        }

        [WebMethod(EnableSession = true)]
        public string
       updateBlockTitle(long id, string title)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                try
                {
                    dc.Blocks.Where(b => b.Id == id).First().Name = title;
                    dc.SubmitChanges();
                }
                catch
                {
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { id = id, title = title, status = -1 });
                }
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { id = id, title = title, status = 1 });
        }

        [WebMethod(EnableSession = true)]
        public string approveAll(long id)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                try
                {
                    dc.Blocks.Where(b => b.NewsId == id && b.deleted == false && b.ParentId == 0).ToList().ForEach(l =>
                    {
                        l.Ready = true;
                        l.Approve = true;
                    });
                    dc.SubmitChanges();
                }
                catch
                {
                    return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { id = id, status = -1 });
                }
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { id = id, status = 1 });
        }
        [WebMethod(EnableSession = true)]
        public string mainVideoSubTitlesLoad(long mediaId)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            var ret = new List<dynamic>();
            using (var dc = new Blocks.DataClassesMediaDataContext())
            {
                try
                {
                    dc.tWeb_GraphicsLayers.Where(l => l.deleted == false).OrderBy(ll => ll.id).ToList().ForEach(l =>
                    {

                        ret.Add(new { id = l.id, title = l.title, items = getGraphicsByList(dc.tWeb_mediaGraphics.Where(g => g.MediaId == mediaId && g.layerId == l.id).First().id) });
                    });
                }
                catch { }
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(ret);
        }
        [WebMethod(EnableSession = true)]
        public string UpdateBlocksSort(List<long> arr)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    var bl = dc.Blocks.Single(b => b.Id == arr[i]);
                    bl.Sort = (((i + 1) * 10));
                    bl.isChanged = DateTime.Now;
                    dc.SubmitChanges();

                }
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1 });
        }
        [WebMethod(EnableSession = true)]
        public string inplaceBlockReadyApprove(long id, bool ready, bool approve)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {

                {
                    var rec = dc.Blocks.Single(b => b.Id == id);
                    rec.Ready = ready;
                    rec.Approve = approve;
                    rec.isChanged = DateTime.Now;
                    dc.SubmitChanges();

                }
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1 });
        }
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]

        public string inplaceBlockAutorsGet(long id)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            var ret = new List<dynamic>();
            ret.Add(new { id = -1, name = " -- ", active = false });
            using (var dc = new Blocks.DataClasses1DataContext())
                foreach (var people in dc.fWeb_ListUsersTOBlockEditorsList(id).OrderBy(n => n.UserName))
                {
                    if (people.RightID == 34)
                    {
                        var active = false;
                        if (people.UserID == dc.Blocks.Single(b => b.Id == id).CreatorId)
                            active = true;
                        ret.Add(new { id = people.UserID, name = people.UserName, active = active });
                    }


                }

            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(ret);
        }
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]

        public string inplaceBlockCameramanGet(long id)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            var ret = new List<dynamic>();
            ret.Add(new { id = -1, name = " -- ", active = false });
            using (var dc = new Blocks.DataClasses1DataContext())
                foreach (var people in dc.fWeb_ListUsersTOBlockEditorsList(id).OrderBy(n => n.UserName))
                {
                    if (people.RightID == 32)
                    {
                        var active = false;
                        if (people.UserID == dc.Blocks.Single(b => b.Id == id).OperatorId)
                            active = true;
                        ret.Add(new { id = people.UserID, name = people.UserName, active = active });
                    }


                }

            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(ret);
        }
        [WebMethod(EnableSession = true)]
        public string inplaceBlockCutterGet(long id)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            var ret = new List<dynamic>();
            ret.Add(new { id = -1, name = " -- ", active = false });
            using (var dc = new Blocks.DataClasses1DataContext())
                foreach (var people in dc.fWeb_ListUsersTOBlockEditorsList(id).OrderBy(n => n.UserName))
                {
                    if (people.RightID == 57)
                    {
                        var active = false;
                        if (people.UserID == dc.Blocks.Single(b => b.Id == id).CutterId)
                            active = true;
                        ret.Add(new { id = people.UserID, name = people.UserName, active = active });
                    }


                }

            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(ret);
        }
        [WebMethod(EnableSession = true)]
        public string inplaceBlockAutorSet(long id, int autorId)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {

                {
                    var rec = dc.Blocks.Single(b => b.Id == id);
                    rec.CreatorId = autorId;
                    rec.isChanged = DateTime.Now;
                    dc.SubmitChanges();

                }
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1 });
        }
        [WebMethod(EnableSession = true)]
        public string inplaceBlockCameramenSet(long id, int autorId)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {

                {
                    var rec = dc.Blocks.Single(b => b.Id == id);
                    rec.OperatorId = autorId;
                    rec.isChanged = DateTime.Now;
                    dc.SubmitChanges();

                }
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1 });
        }
        [WebMethod(EnableSession = true)]
        public string inplaceBlockCutterSet(long id, int autorId)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {

                {
                    var rec = dc.Blocks.Single(b => b.Id == id);
                    rec.CutterId = autorId;
                    rec.isChanged = DateTime.Now;
                    dc.SubmitChanges();

                }
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1 });
        }
        [WebMethod(EnableSession = true)]
        public string inplaceBlockChronoSet(long id, long time)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {

                {
                    var rec = dc.Blocks.Single(b => b.Id == id);
                    rec.BlockTime = time;
                    rec.isChanged = DateTime.Now;
                    dc.SubmitChanges();
                    dc.sp_UpdateNewsHrono(rec.Id);

                }
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1 });
        }
        [WebMethod(EnableSession = true)]
        public string inplaceBlockTaskTimeSet(long id, long time)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {

                {
                    var rec = dc.Blocks.Single(b => b.Id == id);
                    rec.TaskTime = time;
                    rec.isChanged = DateTime.Now;
                    dc.SubmitChanges();
                    dc.sp_UpdateNewsHrono(rec.Id);

                }
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1 });
        }

        [WebMethod(EnableSession = true)]
        public string getAvBlockTypes(long NewsId)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, val = dc.BlockTypes.Where(t => t.Extern == false) });
            }
            return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1 });

        }
        long LongRandom(long min, long max, Random rand)
        {
            long result = rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
            result = (result << 32);
            result = result | (long)rand.Next((Int32)min, (Int32)max);
            return result;
        }
        [WebMethod(EnableSession = true)]
        public string BlockByTypeCreate(long NewsId, int typeId, int groupId)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {

                long blockId = LongRandom(0, 9000000000000, new Random(DateTime.Now.Millisecond)); // new Random(DateTime.Now.Millisecond).Next(0, 100000) * (new Random(DateTime.Now.Millisecond).Next(0, 100000));

                int count = 0;
                while (dc.Blocks.Where(b => b.Id == blockId).Count() > 0 && count < 100)
                {
                    count++;
                    blockId = LongRandom(0, 9000000000000, new Random(DateTime.Now.Millisecond)); // new Random(DateTime.Now.Millisecond).Next(0, 100000) * (new Random(DateTime.Now.Millisecond).Next(0, 100000));

                }
                if (count == 100)
                    blockId = (new Random()).Next(Int32.MaxValue);
                try
                {


                    dc.Blocks.InsertOnSubmit(new Blocks.Blocks()
                    {
                        Id = blockId,
                        Approve = false,
                        CutterId = -1,
                        CreatorId = (int)Session["UserId"],
                        BlockText = "",
                        NewsId = NewsId,
                        BlockTime = 0,
                        JockeyId = -1,
                        BLockType = typeId,
                        CalcTime = 0,
                        deleted = false,
                        Description = "",
                        Name = " -- ",
                        OperatorId = -1,
                        ParentId = 0,
                        Ready = false,
                        Sort = 5,
                        TaskTime = 0,
                        TextLang1 = "",
                        TextLang2 = "",
                        TextLang3 = ""
                    });
                    dc.SubmitChanges();
                }
                catch (Exception ex)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        status = 1,
                        mess = ex.Message
                    });
                }
                dc.sp_SortBlocks(NewsId);

                NFSocket.SendToAll.SendData("blockAdd", new { newsId = NewsId, blockId = blockId });
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, blockId = blockId, NewsData = (groupId == 0) ? GetBlocksFromNews(NewsId.ToString()) : GetBlocksFromCopyNews(NewsId.ToString()) });
            }


        }
        [WebMethod(EnableSession = true)]
        public string getCopyBoxNews(int progId)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            List<dynamic> ret = new List<dynamic>();
            using (var dc = new News.NewsDataContext())
            {
                var rec = dc.CopyNews.Where(n => n.Deleted == false && n.GroupID == 102).OrderByDescending(nn => nn.NewsDate);
                rec.ForEach(l =>
                {
                    ret.Add(new { id = l.id, name = l.Name, date = l.NewsDate.ToString("dd.MM.yyyy HH:mm:ss") });
                });
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, val = ret });
            }
        }
        [WebMethod(EnableSession = true)]
        public string doOnClockBlockType(string value)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new News.NewsDataContext())
            {

                dc.ExecuteCommand("UPDATE  tWeb_Settings  SET value='" + value + "' WHERE [key]='doOnClickBlockType'"); ;
                dc.SubmitChanges();
            }
            return  /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, value = value });
        }
        [WebMethod(EnableSession = true)]
        public string CopyBoxNewsToNews(long newsId, int progId)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new News.NewsDataContext())
            using (var bdc = new Blocks.DataClasses1DataContext())
            {
                var ns = dc.CopyNews.Single(n => n.id == newsId);

                var newNewsId = (new Random()).Next(Int32.MaxValue); // LongRandom(0, 9000000000, new Random(DateTime.Now.Millisecond));
                while (dc.News.Where(n => n.id == newNewsId).Count() > 0)
                {
                    newNewsId = (new Random()).Next(Int32.MaxValue); //;// LongRandom(0, 9000000000, new Random(DateTime.Now.Millisecond));
                }
                dc.News.InsertOnSubmit(new News.New()
                {
                    id = (long)newNewsId,
                    CalcTime = ns.CalcTime,
                    TaskTime = ns.TaskTime,
                    Cassete = ns.Cassete,
                    Deleted = false,
                    Description = ns.Description,
                    Duration = ns.Duration,
                    EditorId = ns.EditorId,
                    Name = ns.Name,
                    NewsDate = DateTime.Now,
                    NewsTime = ns.NewsTime,
                    ProgramId = progId,
                    Time_Code = ns.Time_Code

                });
                dc.SubmitChanges();
                bdc.Blocks.Where(b => b.NewsId == newsId).ForEach(l =>
                {
                    var newBlockId = (new Random()).Next(Int32.MaxValue); ;// LongRandom(0, 9000000000, new Random(DateTime.Now.Millisecond));
                    while (bdc.Blocks.Where(b => b.Id == newBlockId).Count() > 0)
                    {
                        newBlockId = (new Random()).Next(Int32.MaxValue); ;// LongRandom(0, 9000000000, new Random(DateTime.Now.Millisecond)); // new Random(DateTime.Now.Millisecond).Next(0, 100000) * (new Random(DateTime.Now.Millisecond).Next(0, 100000));

                    }
                    bdc.Blocks.InsertOnSubmit(new Blocks.Blocks()
                    {
                        Id = (long)newBlockId,
                        Ready = l.Ready,
                        Approve = l.Approve,
                        BlockText = l.BlockText,
                        BlockTime = l.BlockTime,
                        BLockType = l.BLockType,
                        OperatorId = l.OperatorId,
                        CalcTime = l.CalcTime,
                        CreatorId = l.CreatorId,
                        CutterId = l.CutterId,
                        deleted = l.deleted,
                        JockeyId = l.JockeyId,
                        Description = l.Description,
                        Name = l.Name,
                        NewsId = newNewsId,
                        ParentId = l.ParentId,
                        Sort = l.Sort,
                        TaskTime = l.TaskTime,
                        TextLang1 = l.TextLang1,
                        TextLang2 = l.TextLang2,
                        TextLang3 = l.TextLang3

                    });
                    bdc.SubmitChanges();


                });
                bdc.SubmitChanges();

                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, newsId = newNewsId });
            }

        }

        [WebMethod(EnableSession = true)]
        public long initialLocking(long id)
        {
            if (Session["UserId"] == null)
                return 0;
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {

                var rec = dc.Lockings.Where(l => l.BlockId == id /*&& l.time>DateTime.Now.AddMinutes(-1)*/);

                if (rec.Count() > 0)
                {
                    if (rec.First().UserId == (int)Session["UserId"])
                    {
                        return 0; return id;
                    }
                    else return 0;
                }
                else
                {

                    dc.Lockings.DeleteAllOnSubmit(dc.Lockings.Where(l => l.BlockId == id));
                    dc.SubmitChanges();
                    var userName = dc.Users.Where(u => u.UserID == (int)Session["UserId"]).First().UserName;
                    dc.Lockings.InsertOnSubmit(new Blocks.Locking() { BlockId = id, time = DateTime.Now, UserId = (int)Session["UserId"], UserName = userName });
                    dc.SubmitChanges();
                    return id;
                }


            }
            return 0;
        }
        [WebMethod(EnableSession = true)]
        public string mediaSortByAZ(string id)
        {
            try
            {
                if (Session["UserId"] == null)
                    return "-1";
                using (var dc = new Blocks.DataClassesMediaDataContext())
                {
                    dc.pMedia_SortAlphablet(Convert.ToInt64(id));
                }
            }
            catch (Exception ex)
            {
                var r = ex.Message;
            }
            return System.Web.Helpers.Json.Encode(new { status = 1, id = id });

        }
        [WebMethod(EnableSession = true)]
        public string BEinsertGeo()
        {
         
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                return System.Web.Helpers.Json.Encode(dc.t_BlockEditGeo.OrderBy(s=>s.name).ToList());
            }
            
        }
        [WebMethod(EnableSession = true)]
        public string BEinsertSrc()
        {
        
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                return System.Web.Helpers.Json.Encode(dc.t_BlockEditSrc.OrderBy(s => s.name).ToList());
            }

        }
        [WebMethod(EnableSession = true)]
        public string BEinsertList()
        {
            if (Session["UserId"] == null)
                return "-1";

            var ret = new List<dynamic>();
            ret.Add(new { id = 1, title = "Титр:", val = "\r\n((TITLE: ))\r\n" });
            ret.Add(new { id = 2, title = "Интершум:", val = "\r\n((INTV ХР 00:00:00 ))\r\n" });
            ret.Add(new { id = 3, title = "Комментарий:", val = "\r\n(( ))\r\n" });
            ret.Add(new { id = 0 });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                dc.BlockEditTemplates.OrderBy(b => b.name).ToList().ForEach(l =>
                {
                    ret.Add(new { id = l.id, title = l.name, val = l.description });
                });
            }
            return System.Web.Helpers.Json.Encode(ret);
        }
        [WebMethod(EnableSession = true)]
        public string BlockChernovikSave(string id, string txt)
        {
            if (Session["UserId"] == null)
                return "-1";
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                var rec = dc.Blocks.Where(t => t.Id == Convert.ToInt64(id));
                if (rec.Count() > 0)
                {
                    rec.First().TextLang1 = txt;
                    dc.SubmitChanges();
                }
            }
            return "ok";

        }
        [WebMethod(EnableSession = true)]
        public string unLockItems(string id)
        {
            if (Session["UserId"] == null)
                return "-1";
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                dc.Lockings.DeleteAllOnSubmit(dc.Lockings.ToList());
                dc.SubmitChanges();
            }

            return "ok";
        }

        ////
        [WebMethod(EnableSession = true)]
        public string titleOutInitAP()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    staus = 1,
                    message = "ok",
                    items = dc.tWeb_TitleOuts.Where(r => r.isDeleted == false).OrderBy(r => r.Title).Select(r => new { id = r.id, title = r.Title, css = r.css }),

                });
            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string titleOutAdd()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                dc.tWeb_TitleOuts.InsertOnSubmit(new Blocks.tWeb_TitleOuts()
                {
                    id = Guid.NewGuid().ToString(),
                    Title = "",
                    css = "",
                    isDeleted = false,


                });
                dc.SubmitChanges();
                return titleOutInitAP();
            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string titleOutdelete(string id)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                dc.tWeb_TitleOuts.Where(r => r.id == id).First().isDeleted = true;
                dc.SubmitChanges();
                return JsonConvert.SerializeObject(new { status = 1, id = id });

            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string totleOutChange(string id, string title, string css)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                var rec = dc.tWeb_TitleOuts.Where(r => r.id == id).First();
                rec.Title = RemomeHtmlTag(title);
                rec.css = RemomeHtmlTag(css);

                dc.SubmitChanges();
                return JsonConvert.SerializeObject(new { status = 1, id = id });
            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string titleOutGetList()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                var rec = dc.tWeb_TitleOuts.OrderBy(r => r.Title).Where(r => r.isDeleted == false);
                return JsonConvert.SerializeObject(rec.Select(r => new { id = r.id, title = r.Title }));
            }
            return "";
        }

        [WebMethod(EnableSession = true)]
        public string blockList(long newsId)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                var rec = dc.Blocks.Where(r => r.deleted == false && r.ParentId == 0 && r.NewsId == newsId).OrderBy(r => r.Sort);
                return JsonConvert.SerializeObject(rec.Select(r => new { id = r.Id, title = r.Name, text = r.BlockText }));
            }
            return "";
        }
        [WebMethod(EnableSession = true)]

        public string titleHide(long newsId, string cssClass, string id)
        {
            lock (Application["title"])
            {
                var dic = (Dictionary<long, Dictionary<string, string>>)Application["title"];
                if (dic.Keys.Where(r => r == newsId).Count() == 0)
                {
                    return "";
                }
                var newsDic = dic[newsId];
                if (newsDic.Keys.Where(r => r == cssClass).Count() == 0)
                {
                    return "";
                }
                else
                    newsDic.Remove(cssClass);

                Application["title"] = dic;
                return Newtonsoft.Json.JsonConvert.SerializeObject(dic);
            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string titleShow(long newsId, string cssClass, string title, string subtitle, string id)
        {


            lock (Application["title"])
            {
                var dic = (Dictionary<long, Dictionary<string, string>>)Application["title"];
                if (dic.Keys.Where(r => r == newsId).Count() == 0)
                {
                    dic.Add(newsId, new Dictionary<string, string>());
                }
                var newsDic = dic[newsId];
                if (newsDic.Keys.Where(r => r == cssClass).Count() == 0)
                {
                    newsDic.Add(cssClass, JsonConvert.SerializeObject(new { title = title, subtitle = subtitle, id = id }));
                }
                else
                    newsDic[cssClass] = JsonConvert.SerializeObject(new { title = title, subtitle = subtitle, id = id });

                Application["title"] = dic;
                return Newtonsoft.Json.JsonConvert.SerializeObject(dic);
            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string titleGet(long newsId)
        {


            lock (Application["title"])
            {
                var dic = (Dictionary<long, Dictionary<string, string>>)Application["title"];
                if (dic.Keys.Where(r => r == newsId).Count() == 0)
                {
                    return JsonConvert.SerializeObject(new { status = -1 });
                }
                var newsDic = dic[newsId];
                return JsonConvert.SerializeObject(new { status = 1, items = newsDic });
            }
            return "";
        }


        [WebMethod(EnableSession = true)]
        public string cssGet(string css)
        {
            // if (Session["UserId"] == null)
            //     return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                var rec = dc.tWeb_TitleOuts.Where(r => r.id == css).First();
                return JsonConvert.SerializeObject(new { css = rec.css }); ;
            }
            return "";
        }
        [WebMethod(EnableSession = true)]
        public string SubTitleTextSet(string text, string blockId)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                dc.Blocks.Where(r => r.Id == Convert.ToInt64(blockId)).First().TextLang3 = text;
                dc.SubmitChanges();
            }
            return blockId;

        }
        [WebMethod(EnableSession = true)]
        public string SubTitleTextGet(string blockId)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                return dc.Blocks.Where(r => r.Id == Convert.ToInt64(blockId)).First().TextLang3;

            }


        }
        [WebMethod(EnableSession = true)]
        public string uploadFile(System.IO.Stream stream)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            return "a";


        }
        [WebMethod(EnableSession = true)]
        public string partnersProgramsGet()
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            using (var client = new System.Net.WebClient())
            {
                using (var db = new Blocks.DataClasses1DataContext())
                {
                    var server = db.tWeb_Settings.Where(s => s.Key == "portalUrl").First().value;
                    var json = client.DownloadString(server + "/rest/api/v1/progs");
                    return from1251(json);
                }
            }
        }
        [WebMethod(EnableSession = true)]
        public string partnersNewsGet(string progId)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            using (var client = new System.Net.WebClient())
            {
                using (var db = new Blocks.DataClasses1DataContext())
                {
                    var server = db.tWeb_Settings.Where(s => s.Key == "portalUrl").First().value;
                    var json = client.DownloadString(server + "/rest/api/v1/newsfromprog/" + progId);
                    return from1251(json);
                }
            }
        }
        [WebMethod(EnableSession = true)]
        public string partnersBlocksGet(string id)
        {
            if (Session["UserId"] == null)
                return /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no auth" });

            using (var client = new System.Net.WebClient())
            {
                using (var db = new Blocks.DataClasses1DataContext())
                {
                    var server = db.tWeb_Settings.Where(s => s.Key == "portalUrl").First().value;
                    var json = client.DownloadString(server + "/rest/api/v1/blocks/" + id);
                    return from1251(json);
                }
            }
        }

        private static string from1251(string txt)
        {
            System.Text.Encoding utf8 = System.Text.Encoding.GetEncoding("UTF-8");
            System.Text.Encoding win1251 = System.Text.Encoding.GetEncoding("Windows-1251");

            byte[] utf8Bytes = win1251.GetBytes(txt);
            byte[] win1251Bytes = System.Text.Encoding.Convert(utf8, win1251, utf8Bytes);

            return win1251.GetString(win1251Bytes);
        }
        [WebMethod(EnableSession = true)]
        public string GetSynegyRoot(string currID)
        {
            using (var db = new cynegy.CynegyDataClassesDataContext())
            {
                var elems = db.nodes.Where(m => m.parent_id == System.Guid.Parse(currID) && m.type < 100).OrderByDescending(o => o.creation_date);// Новости
                return JsonConvert.SerializeObject(new { elem = elems });
            }
        }
        protected virtual bool IsFileLocked(System.IO.FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
        public void insertsubblockFromSynegy(string elemId, string elemTitle, long blockId)
        {
            try
            {
                using (var dc = new Blocks.DataClasses1DataContext())
                {
                    long newblockId = LongRandom(0, 9000000000000, new Random(DateTime.Now.Millisecond)); // new Random(DateTime.Now.Millisecond).Next(0, 100000) * (new Random(DateTime.Now.Millisecond).Next(0, 100000));

                    int count = 0;
                    while (dc.Blocks.Where(b => b.Id == newblockId).Count() > 0 && count < 100)
                    {
                        count++;
                        newblockId = LongRandom(0, 9000000000000, new Random(DateTime.Now.Millisecond)); // new Random(DateTime.Now.Millisecond).Next(0, 100000) * (new Random(DateTime.Now.Millisecond).Next(0, 100000));

                    }
                    if (count == 100)
                        newblockId = (new Random()).Next(Int32.MaxValue);
                    var dateCreate = DateTime.Now;
                    System.Threading.Thread.Sleep(1 * 1000);
                    var sortedFiles = new System.IO.DirectoryInfo(@"\\192.168.2.38\may24").GetFiles()
                                                  .Where(f => f.LastWriteTime > dateCreate)
                                                  .OrderBy(f => f.LastWriteTime)
                                                  .ToList();
                    int i = 0;
                    while (sortedFiles.Count == 0 && i < (20 * 60) /*20 min*/)
                    {
                        i++;

                        sortedFiles = new System.IO.DirectoryInfo(@"\\192.168.2.38\may24").GetFiles()
                                                  .Where(f => f.LastWriteTime > dateCreate)
                                                  .OrderByDescending(f => f.LastWriteTime)
                                                  .ToList();
                        System.Threading.Thread.Sleep(1000);
                    }
                    // System.IO.File.AppendAllText("c:\\\\tmp\\1.txt", "\r\n SORTED FILES" + sortedFiles.Count.ToString());
                    if (sortedFiles.Count > 0)
                    {

                        var videoFileName = sortedFiles.Last().FullName;


                        var j = 0;
                        var firstSize = new System.IO.FileInfo(videoFileName).Length;
                        System.Threading.Thread.Sleep(20*1000);
                        var secondSize = new System.IO.FileInfo(videoFileName).Length;
                        while ((firstSize != secondSize || firstSize == 0) && j < (2 * 600))
                        {
                            j++;

                            firstSize = new System.IO.FileInfo(videoFileName).Length;
                            System.Threading.Thread.Sleep(20*1000);
                            secondSize = new System.IO.FileInfo(videoFileName).Length;
                            // isOpen = new System.IO.FileInfo(videoFileName).Length==0; 
                            // System.Threading.Thread.Sleep(1000);
                            // System.IO.File.AppendAllText("c:\\\\tmp\\1.txt", "\r\n FILE SIZE IS  " + new System.IO.FileInfo(videoFileName).Length.ToString());
                            //  System.IO.File.AppendAllText("c:\\\\tmp\\1.txt", "\r\n J IS  " + j.ToString());
                        }


                        var taskId = dc.pWeb_InsertMediaToBlock(Convert.ToInt64(blockId), videoFileName, elemTitle, 2);

                        // System.IO.File.AppendAllText("c:\\\\tmp\\1.txt", "\r\n INSERT OK" + taskId.ToString());
                        var blocks = dc.Blocks.Where(b => b.ParentId == Convert.ToInt64(blockId) && b.BlockText.IndexOf(videoFileName) == 0 && b.TextLang2.Length < 2);
                        if (blocks.Count() > 0)
                        {

                            var block = blocks.First();
                            // System.IO.File.AppendAllText("c:\\\\tmp\\1.txt", "\r\n BLOCK FIND " + block.Id.ToString());
                            block.TextLang1 = videoFileName;
                            var imageFolder = dc.tWeb_Settings.Where(s => s.Key == "ThumbnailPath").First().value;
                            var imageFileName = System.IO.Path.Combine(imageFolder, Guid.NewGuid().ToString() + ".jpg");// "D:\\\\nfUpload\\" + Guid.NewGuid().ToString()+".jpg";// videoFileName.Replace(".mp4", ".jpg");
                            block.TextLang2 = imageFileName;
                            dc.SubmitChanges();
                            var ffmpegPath = dc.tWeb_Settings.Where(s => s.Key == "FFMpegPath").First();
                            var ff_cmd = ffmpegPath.value + "  -itsoffset -1  -i " + '"' + videoFileName + '"' + " -vcodec mjpeg -vframes 1 -an -f rawvideo -s 640x360 " + '"' + imageFileName + '"';
                            var startInfo = new System.Diagnostics.ProcessStartInfo
                            {
                                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                                FileName = "cmd.exe",
                                Arguments = "/C " + ff_cmd
                            };

                            // System.IO.File.AppendAllText("c:\\\\tmp\\1.txt", "\r\n FFMPEG CMD  " + ff_cmd);

                            var process = new System.Diagnostics.Process
                            {
                                StartInfo = startInfo
                            };

                            process.Start();
                            process.WaitForExit(5000);
                            NFSocket.SendToAll.SendData("mediaUploadComplite", new { blockId = blockId });
                            NFSocket.SendToAll.SendData("mediaGraphicsChange", new { blockId = blockId, mediaId = block.Id });

                            NFSocket.SendToAll.SendData("imgCreated", new { mediaId = block.Id, blockId = blockId });







                        }
                    }

                }
            }
            catch(Exception ex)
            {
               // System.IO.File.AppendAllText("c:\\\\tmp\\1.txt", "\r\n ERROR" + ex.Message);
            }
        }
        [WebMethod(EnableSession = true)]
         public async System.Threading.Tasks.Task<string> titleToExcel(string titles) {
            if (Session["UserId"] == null)
            {
                return "401";
            }
                using (var dc = new Blocks.DataClasses1DataContext())
            {

                var url=dc.tWeb_Settings.Where(s => s.Key == "titleProxy").ToList().First().value;
                var values = new Dictionary<string, string>
                    {
                        { "titles", titles },
                   
                    };
                var content = new System.Net.Http.FormUrlEncodedContent(values);
                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                //var content = new System.Net.Http.StringContent(titles);
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }


        }

        [WebMethod(EnableSession = true)]
        public string socialMessageImageUpdate(string id, string imgFile)
        {
            if (Session["UserId"] == null)
                return "NO auth";
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                var elems = dc.tSocial_feedToMessage.Where(s => s.id == id);
                if (elems.Count() > 0) {
                    var e = elems.First();
                    e.imgFile = imgFile;
                    dc.SubmitChanges();
                }
                return imgFile;
            }
        }
        [WebMethod(EnableSession = true)]
        public string socialMessagePublishUploadImage(string fn, string base64) {
            if (Session["UserId"] == null)
                return "NO auth";
            // получаем имя файла

            // сохраняем файл в папку Files в проекте
            using (var dc = new Blocks.DataClasses1DataContext()) {
                  var filename = System.IO.Path.Combine(dc.tWeb_Settings.Where(s => s.Key == "FolderToUpload").First().value, ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString() +"_"+fn);
                System.IO.File.WriteAllBytes(filename, Convert.FromBase64String(base64));
                return filename;
            }
        }
        [WebMethod(EnableSession = true)]
        public string rssDeleteItem(string id) {
            if (Session["UserId"] == null)
                return "NO auth";
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                dc.tSocial_feedToMessage.DeleteAllOnSubmit(dc.tSocial_feedToMessage.Where(s => s.id == id));
                dc.SubmitChanges();

            }
            return id;
        }
        [WebMethod(EnableSession = true)]
        public string rssChange(string id, string message)
        {
            if (Session["UserId"] == null)
                return "NO auth";
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                  var rec=dc.tSocial_feedToMessage.Where(s => s.id == id);
                  if (rec.Count() > 0) {
                      rec.First().message = message;
                      dc.SubmitChanges();
                  }
            }
            return id;
        }

        [WebMethod(EnableSession = true)]
        public string SaveSynegyRoot(string elemId, string elemTitle, long blockId)
        {
            elemTitle = elemTitle.Replace("'", "");
            var error = "";
            try
            {
                using (var cyndc = new cynegy.CynegyDataClassesDataContext())
                using (var dc = new Blocks.DataClasses1DataContext())
                {
                    
                   
                    var jobId = Guid.NewGuid().ToString();
                    var subjId = Guid.NewGuid().ToString();
                    var cmd = "exec [dbo].[mam_job_create] 'F2FD5562-3A27-4283-81EC-6357516B2E67', /*stay*/  '" + jobId + "' /*new id*/, '"+ elemTitle + "', 'ED74E66F-29C4-4B8C-A0EA-FF9F21003F6C' /*stay*/ , 0, 91, 91, NULL, '" + elemId + "' /*sub node id*/ , 0, 0, NULL, 0 ";
                    error += "\r\n" + cmd;
                    cyndc.ExecuteCommand(cmd);
                    cmd = "exec [dbo].[mam_job_subject_create] '" + jobId + "', /*new id from step 1*/'" + subjId + "', /*new id*/ '"+ elemTitle + "', '" + elemId + "' /*sub node id*/, '" + elemId + "' /*sub node id*/, 0, 0 ";
                    error += "\r\n" + cmd;
                    cyndc.ExecuteCommand(cmd);

                    


                     System.Threading.Tasks.Task.Run(() => { try { insertsubblockFromSynegy(elemId, elemTitle, blockId); } catch (Exception ex) {/* System.IO.File.AppendAllText("c:\\\\tmp\\1.txt", "\r\n ERROR IN TASK" + ex.Message);*/ } });
                }
            }
            catch(Exception e)
            {
                error+= "\r\n" + e.Message;
            }
            return error;

            using (var cyndc = new cynegy.CynegyDataClassesDataContext())
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                long newblockId = LongRandom(0, 9000000000000, new Random(DateTime.Now.Millisecond)); // new Random(DateTime.Now.Millisecond).Next(0, 100000) * (new Random(DateTime.Now.Millisecond).Next(0, 100000));

                int count = 0;
                while (dc.Blocks.Where(b => b.Id == newblockId).Count() > 0 && count < 100)
                {
                    count++;
                    newblockId = LongRandom(0, 9000000000000, new Random(DateTime.Now.Millisecond)); // new Random(DateTime.Now.Millisecond).Next(0, 100000) * (new Random(DateTime.Now.Millisecond).Next(0, 100000));

                }
                if (count == 100)
                    newblockId = (new Random()).Next(Int32.MaxValue);

                var assetNode = cyndc.nodes.Where(n => n.id == Guid.Parse(elemId)).First();
                if (assetNode.type == 16)
                {
                    return JsonConvert.SerializeObject(parseSecvence(elemId, blockId, elemTitle));
                }
                else
                {
                    var files = cyndc.v_files.Where(f => f.parent_id == Guid.Parse(elemId) && f.filetype == "AW1");
                    var ret = new List<dynamic>();
                    var err = "";
                    files.ForEach(f =>
                    {
                        ret.Add(new { s = f.filetype, name = f.filename });
                    });

                    var filename = "";
                    if (files.Count() > 0)
                    {
                        var file = files.First();

                        var taskId = dc.pWeb_InsertMediaToBlock(Convert.ToInt64(blockId), file.name + file.filename, elemTitle, 2);
                        dc.pMedia_SetTaskUploadComplite(taskId);
                    }
                    else
                    {
                        var nodes = cyndc.nodes.Where(n => n.id == Guid.Parse(elemId));
                        if (nodes.Count() > 0)
                        {
                            var node = nodes.First();
                            if (node.predecessor_id != null)
                            {
                                files = cyndc.v_files.Where(f => f.node_id == node.predecessor_id && f.filetype == "AW1");
                                files.ForEach(f =>
                                {
                                    ret.Add(new { s = f.filetype, name = f.filename });
                                });

                                filename = "";
                                if (files.Count() > 0)
                                {
                                    var file = files.First();

                                    var taskId = dc.pWeb_InsertMediaToBlock(Convert.ToInt64(blockId), file.name + file.filename, elemTitle, 2);
                                    dc.pMedia_SetTaskUploadComplite(taskId);
                                }
                            }
                        }
                    }
                    


                    return JsonConvert.SerializeObject(new { satus = 0, elementId = elemId, filename = filename, err = err, list = ret });
                }

            }


            /*  using (var dc = new cynegy.CynegyDataClassesDataContext())
              {
                  var taskid = Guid.NewGuid();
                  dc.nodes.InsertOnSubmit(new cynegy.nodes()
                  {
                      id = taskid,
                      parent_id = Guid.Parse("F2FD5562-3A27-4283-81EC-6357516B2E67"),
                      predecessor_id = null,
                      type = 91,
                      creation_date = DateTime.Now,
                      name = elemTitle,
                      children = 0,
                      deleted_children = 0,
                      sub_type = 91,
                      creator_id = 2
                  });
                  dc.SubmitChanges();

                  dc.nodes_ex1.InsertOnSubmit(new cynegy.nodes_ex1()
                  {
                      id = taskid,
                      modification_date = DateTime.Now,
                      modificator_id = null
                  });
                  dc.nodes_ex2.InsertOnSubmit(new cynegy.nodes_ex2()
                  {
                      id = taskid,
                      order = 1,
                      parent_id2 = Guid.Parse("F2FD5562-3A27-4283-81EC-6357516B2E67"),
                      type2 = 91
                  });
                  dc.nodes_ex3.InsertOnSubmit(new cynegy.nodes_ex3()
                  {
                      id = taskid,
                      parent_id = Guid.Parse("F2FD5562-3A27-4283-81EC-6357516B2E67"),
                      type = 91,
                      deleted = 0,
                  });
                  dc.SubmitChanges();


                  var jobid = Guid.NewGuid();
                  dc.nodes.InsertOnSubmit(new cynegy.nodes()
                  {
                      id = jobid,
                      parent_id = taskid,//Guid.Parse("CA543E6D-77C2-EA11-BA99-F403433E40D3"),
                      predecessor_id = null,
                      type = 92,
                      creation_date = DateTime.Now,
                      name = elemTitle,
                      children = 0,
                      deleted_children = 0,
                      sub_type = 92,
                      creator_id = 2
                  });
                  dc.SubmitChanges();
                  dc.nodes_ex1.InsertOnSubmit(new cynegy.nodes_ex1()
                  {
                      id = jobid,
                      modification_date = DateTime.Now,
                      modificator_id = null
                  });
                  dc.nodes_ex2.InsertOnSubmit(new cynegy.nodes_ex2()
                  {
                      id = jobid,
                      order = 0,
                      parent_id2 = Guid.Parse("CA543E6D-77C2-EA11-BA99-F403433E40D3"),
                      type2 = 92
                  });
                  dc.nodes_ex3.InsertOnSubmit(new cynegy.nodes_ex3()
                  {
                      id = jobid,
                      parent_id = Guid.Parse("CA543E6D-77C2-EA11-BA99-F403433E40D3"),
                      type = 92,
                      deleted = 0,
                  });
                  dc.SubmitChanges();


                  var jobSubjId = Guid.NewGuid();

                  dc.job_subject.InsertOnSubmit(new cynegy.job_subject()
                  {
                      job_node_id = jobid,
                      node_id = Guid.Parse(elemId)
                  });
                  dc.SubmitChanges();

                  dc.job.InsertOnSubmit(new cynegy.job()
                  {

                      job_status  Guid.Parse("6CF96B9D-BFBC-4ECC-9035-3DE7F75D78B2"),
                      job_disabled = false,
                      job_node_id = taskid,
                      job_type = 0,
                      job_data = "<JobDataExtended xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                      "<Version>1</Version>\r\n" +
                      "<Tasks />\r\n" + "</JobDataExtended>",
                      additional_in=fo = "0",
                      percent = 0

                  });
                  dc.SubmitChanges();


              }
              return JsonConvert.SerializeObject(new { satus = 0, elementId= elemId });*/

        }
        public List<dynamic> parseSecvence(string elemId, long blockId, string elemTitle)
        {
            var ret = new List<dynamic>();
            using (var cyndc = new cynegy.CynegyDataClassesDataContext())
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                var rootNodes = cyndc.nodes.Where(n => n.parent_id == Guid.Parse(elemId) && n.name == "Video_Layer");
                rootNodes.ForEach(rootNode =>
                {
                    var videoLayers = cyndc.nodes.Where(n => n.parent_id == rootNode.id);
                    videoLayers.ForEach(videoLayer =>
                    {
                        var tracks = cyndc.nodes.Where(n => n.parent_id == videoLayer.id);
                        tracks.ForEach(track =>
                        {
                            var id = track.id;
                            if (track.predecessor_id != null)
                                id = Guid.Parse(track.predecessor_id.ToString());
                            if (track.type == 16)
                            {
                                var m = parseSecvence(track.id.ToString(), blockId, elemTitle);
                                m.ForEach(mm => ret.Add(mm));
                            }
                            else
                            {
                                var files = cyndc.v_files.Where(f => f.node_id ==id && f.filetype == "AW1");
                                files.ForEach(file =>
                                {
                                    var taskId = dc.pWeb_InsertMediaToBlock(Convert.ToInt64(blockId), file.name + file.filename, elemTitle, 2);
                                    dc.pMedia_SetTaskUploadComplite(taskId);
                                    ret.Add(new { s = file.filetype, name = file.filename });
                                });
                            }


                        });
                    });
                });

            }
            return ret;
        }
    }
}
