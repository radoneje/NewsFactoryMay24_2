using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.API
{
    /// <summary>
    /// Сводное описание для blockToNewsAdd
    /// </summary>
    public class blockToNewsAdd : IHttpHandler
    {
        private string win1251ToUtf8(string txt)
        {
            var utf8 = System.Text.Encoding.GetEncoding("UTF-8");
            var win1251 = System.Text.Encoding.GetEncoding("Windows-1251");

            byte[] utf8Bytes = win1251.GetBytes(txt);
            byte[] win1251Bytes = System.Text.Encoding.Convert(utf8, win1251, utf8Bytes);
            return win1251.GetString(win1251Bytes);
        }
        public void ProcessRequest(HttpContext context)
        {
            var line = 0;

            try
            {
                if (context.Request.Params["newsId"] == null)
                {
                    context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "no newsId parametr" }));
                    return;
                }
                line = 1;
                var sr = new System.IO.StreamReader(context.Request.InputStream);
                line = 2;
                string json = /*EncryptString.StringCipher.Decrypt*/( sr.ReadToEnd());
                line = 3;
                dynamic param = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                line = 4;
                using (var dc = new WebApplication2.News.NewsDataContext())
                {
                    var items = dc.News.Where(r => r.id == Convert.ToInt64(context.Request.Params["newsId"]) && r.Deleted == false);
                    if (items.Count() == 0)
                    {
                        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message = "news not found" }));
                        return;
                    }
                }
                line = 5;
                
                var item = param[0];
                line = 6;
                using (var dc = new WebApplication2.Blocks.DataClasses1DataContext())
                {
                    long blockId = new Random().Next(100000000);
                    while (dc.Blocks.Where(r => r.Id == blockId).Count() > 0)
                    {
                        blockId = new Random().Next(100000000);
                    }

                    var sort = 0;
                    var last = dc.Blocks.Where(r => r.NewsId == Convert.ToInt64(context.Request.Params["newsId"]) && r.ParentId == 0);
                    if (last.Count() > 0)
                        sort = last.Max(r => r.Sort) + 10;

                 
                    dc.Blocks.InsertOnSubmit( new WebApplication2.Blocks.Blocks()
                    {
                        Id = blockId,
                        NewsId = Convert.ToInt64(context.Request.Params["newsId"]),
                        Approve = false,
                        Ready = false,
                        BlockText = item.lid,
                        BlockTime = ((string)item.text).Length / 17,
                        BLockType = dc.BlockTypes.Where(r => r.Autor == true && r.Operator == false && r.Jockey == true).First().id,
                        CalcTime = ((string)item.lid).Length / 17,
                        CreatorId = -1,
                        deleted = false,
                        CutterId = -1,
                        Description = "autor: " + item.author,
                        JockeyId = -1,
                        Name = item.zag,
                        OperatorId = -1,
                        ParentId = 0,
                        Sort = sort,
                        TaskTime = 0,
                        TextLang1 = "",
                        TextLang2 = "",
                        TextLang3 = ""
                    });
                    dc.SubmitChanges();
                    while (dc.Blocks.Where(r => r.Id == blockId).Count() > 0)
                    {
                        blockId = new Random().Next(100000000);
                    }
                    dc.Blocks.InsertOnSubmit(new WebApplication2.Blocks.Blocks()
                    {
                        Id = blockId,
                        NewsId = Convert.ToInt64(context.Request.Params["newsId"]),
                        Approve = false,
                        Ready = false,
                        BlockText = item.text,
                        BlockTime = ((string)item.text).Length / 17,
                        BLockType = dc.BlockTypes.Where(r => r.Autor == true && r.Operator == true && r.Jockey == false).First().id,
                        CalcTime = ((string)item.text).Length / 17,
                        CreatorId = -1,
                        deleted = false,
                        CutterId = -1,
                        Description = "autor: " + item.author,
                        JockeyId = -1,
                        Name = item.zag,
                        OperatorId = -1,
                        ParentId = 0,
                        Sort = sort + 10,
                        TaskTime = 0,
                        TextLang1 = "",
                        TextLang2 = "",
                        TextLang3 = ""
                    });
                    dc.SubmitChanges();
                    dc.sp_UpdateNewsHrono(blockId);
                    for (int i = 0; i < (int)item.media.Count; i++)
                    {
                        var subBlockId = new Random().Next(100000000);
                        while (dc.Blocks.Where(r => r.Id == subBlockId).Count() > 0)
                        {
                            subBlockId = new Random().Next(100000000);
                        }
                        int blockType = 0;
                        string type = item.media[i].type;
                        if (type.IndexOf("image") > 0)
                            blockType = 1;
                        if (type.IndexOf("video") > 0)
                            blockType = 2;

                        /* dc.Blocks.InsertOnSubmit(new WebApplication2.Blocks.Block() {
                             Id = subBlockId,
                             NewsId = Convert.ToInt64(context.Request.Params["newsId"]),
                             Approve = false,
                             Ready = false,
                             BlockText = item.media[i].link,
                             BlockTime = 0,
                             BLockType = blockType,
                             CalcTime = 0,
                             CreatorId = -1,
                             deleted = false,
                             CutterId = -1,
                             Description = item.media[i].origFilename,
                             JockeyId = -1,
                             Name = item.media[i].origFilename,
                             OperatorId = -1,
                             ParentId = blockId,
                             Sort = i * 10,
                             TaskTime = 0,
                             TextLang1 = "",
                             TextLang2 = "",
                             TextLang3 = ""
                         });*/
                        dc.pWeb_InsertMediaToBlock(blockId, (string)item.media[i].link, (string)item.media[i].origFilename, blockType);
                        System.Threading.Thread.Sleep(100);
                        // dc.SubmitChanges();
                    }
                }
                line = 16;
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { status = 1, message = "" }));
                return;
            }
            catch(Exception ex)
            {
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { status = -1, message ="line="+line.ToString()+"  "+ ex.Message }));
                return;
            }


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public long LongRandom(long min, long max, Random rand)
        {
            long result = rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
            result = (result << 32);
            result = result | (long)rand.Next((Int32)min, (Int32)max);
            return result;
        }
    }
}