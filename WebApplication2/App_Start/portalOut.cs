using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;

namespace WebApplication2.App_Start
{
    public class portalOut
    {
       
        public static void start()
        { using (var dc = new Blocks.DataClasses1DataContext())
         {
                var portalSettings = dc.tWeb_Settings.Where(s => s.Key.IndexOf("portal") == 0).ToList();
                if (portalSettings.Count() == 0)
                    return;
                if (portalSettings.Where(p => p.Key == "portalEnable").First().value !="1")
                    return;

            while (true)
            {

                try
                {
                   
                        List<Blocks.vWeb_blocksForPortal> blockList = new List<Blocks.vWeb_blocksForPortal>();
                       
                        using (var dcNews = new News.NewsDataContext())
                        {
                            var nns = dcNews.News.Where(n => n.Deleted == false && n.Program.Deleted == false && n.Program.Rustv == true && n.Program.id > 0).ToList();
                            List<dynamic> newsBlocks = new List<dynamic>();
                            nns.ForEach(n => {
                                var blocks = dc.vWeb_blocksForPortal.Where(b => b.NewsId == n.id && b.isChanged!=null ).ToList();
                                blockList.AddRange(blocks);
                                newsBlocks.AddRange(blockList);

                                if (blocks.Count() > 0)
                                {
                                    var signature = Guid.NewGuid().ToString();
                                   var json=new {
                                        client= portalSettings.Where(p => p.Key != "portalClientSecret").Select(s=> new { s.Key, s.value}),
                                        progId =n.Program.id,
                                        progTitle=n.Program.Name,
                                        newsId=n.id,
                                        newsDate=n.NewsDate,
                                        newsTitle=n.Name,
                                        blocks = blocks,
                                       signature= signature,
                                       hash= SignData(signature, portalSettings.Where(p=>p.Key== "portalClientSecret").First().value)
                                   };

                                    var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(json);
                                    using (HttpClient client = new HttpClient())
                                    {
                                        var content = new StringContent(jsonObject, System.Text.Encoding.UTF8, "application/json");
                                        var url = portalSettings.Where(p => p.Key == "portalUrl").First().value+"/rest/api/v1/postnews";
                                        var result = client.PostAsync(url, content).Result;
                                    }

                                        blockList.ForEach(b =>
                                        {
                                            var bl = dc.Blocks.Where(br => br.Id == b.Id).First();
                                            bl.isChanged = null;
                                            dc.SubmitChanges();
                                        });
                                    blockList.Clear();
                                }

                            });
                           
                        }
                    }
                catch (Exception ex)
                {

                }
                System.Threading.Thread.Sleep(60 * 1000);
                }
            }

       }

      

        private static string SignData(string message, string secret)
        {
            var encoding = new System.Text.UTF8Encoding();
            var keyBytes = encoding.GetBytes(secret);
            var messageBytes = encoding.GetBytes(message);
            using (var hmacsha1 = new System.Security.Cryptography.HMACSHA1(keyBytes))
            {
                var hashMessage =  hmacsha1.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashMessage);
            }
        } 

    
    }
   
}