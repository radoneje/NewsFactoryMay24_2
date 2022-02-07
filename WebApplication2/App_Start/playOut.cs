using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.App_Start
{
    public class playOut
    {
        private struct ll
        {
            public int sort;
            public string file;
            public Int64 subBlockId;
        }
        public static void start()
        {
            while (true)
            {
                try
                {
                    using (var dc = new Blocks.DataClasses1DataContext())
                    {
                        dc.tWeb_PlayOutTasks.OrderByDescending(r=>r.dateCreate).Where(r=>r.status==0).ToList().ForEach(l=>{
                            l.status = 1;
                            dc.SubmitChanges();
                            try
                            {
                                var bl = dc.tWeb_PlayOutTaskFiles.Where(r => r.status == 0  && r.taskId==l.id);
                                if (bl.Count() == 0)
                                {
                                    throw new Exception("no files");
                                }
                                else
                                {
                                    var server = dc.tWeb_PlayOuts.Where(r => r.id == l.serverId).First();
                                    if (server.replacePath.Length == 0)
                                        server.replacePath = server.Path;
                                    var folder = System.IO.Path.Combine(server.Path, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
                                    if (server.typeId == "FWD")
                                    {
                                        // l.
                                        using (var dcNews = new News.NewsDataContext())
                                        {
                                            var news = dcNews.News.Where(r => r.id == l.newsId).First();
                                            var count = 0;
                                            var title = Translit(news.Name);
                                            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[:;*'\",_&#^@\\/]");
                                            title = reg.Replace(title, string.Empty);

                                            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"\s");
                                            title = reg.Replace(title, "_");
                                            title = title.Replace(" ", "_");

                                            folder = System.IO.Path.Combine(server.Path, news.NewsDate.ToString("MMdd_HHmm_") + count.ToString().PadLeft(2, '0') + "_" + title);
                                            while (System.IO.Directory.Exists(folder))
                                            {
                                                count++;
                                                folder = System.IO.Path.Combine(server.Path, news.NewsDate.ToString("MMdd_HHmm_") + count.ToString().PadLeft(2, '0') + "_" + title);
                                            }

                                        }
                                        // folder = System.IO.Path.Combine(server.Path, DateTime.Now.ToString("MMdd_HHmm_00"));
                                    }
                                    System.IO.Directory.CreateDirectory(folder);
                                    var lst = new List<ll>();

                                    bl.OrderBy(r => r.sort).ToList().ForEach(sources =>
                                      {
                                          sources.status = 1;
                                          dc.SubmitChanges();
                                          var sourceFile = sources.fileName;
                                          var sourceFileNameTmp = System.IO.Path.GetFileName(sourceFile);
                                          var sb = dc.Blocks.Where(r => r.Id == sources.subBlockId).First();
                                          var block = dc.Blocks.Where(r => r.Id == sb.ParentId).First();
                                          var title = Translit(block.Name);
                                          System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[:;*'\",_&#^@\\/]");
                                          title = reg.Replace(title, string.Empty);

                                          System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"\s");
                                          title = reg.Replace(title, "_");
                                          title = title.Replace(" ", "_");
                                          var ext = System.IO.Path.GetExtension(sourceFileNameTmp);
                                          var sourceFileName = sources.sort.ToString().PadLeft(8, '0') + "_" + title + ext;

                                        var destFile = System.IO.Path.Combine(folder, sourceFileName);
                                        int i=0;
                                       // System.IO.File.AppendAllText("c:\\tmp\\nf.log", sourceFileName + "->" + destFile);
                                      
                                        while (System.IO.File.Exists(destFile))
                                        {
                                            var tmp =  System.IO.Path.GetFileNameWithoutExtension(sourceFileName)+"_"+i.ToString() +
                                            System.IO.Path.GetExtension(sourceFileName);
                                              
                                            destFile = System.IO.Path.Combine(folder, tmp);
                                         //   System.IO.File.AppendAllText("c:\\tmp\\nf.log", "next=>" + sourceFileName + "->" + destFile);
                                            i++;
                                        }
                                         try {
                                             // System.IO.File.Copy(sourceFile, destFile);

                                              using (var sourceStream = new System.IO.FileStream(sourceFile, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                                              {
                                                  using (var destStream = new System.IO.FileStream(destFile, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                                                  {
                                                      var dt = DateTime.Now;
                                                     byte[] buffer = new byte[10 * 1024 *1024];
                                                      int read;
                                                      long total = 0;
                                                      while ((read = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                                                      {
                                                          total += read;
                                                          destStream.Write(buffer, 0, read);
                                                          var ts = new TimeSpan((DateTime.Now - dt).Ticks);
                                                          if (ts.Seconds > 10)
                                                          {
                                                              dt = DateTime.Now;
                                                              sources.bytesSend = total;
                                                              dc.SubmitChanges();
                                                          }
                                                      }
                                                     // System.IO.File.Copy(sourceFile, destFile);
                                                      sources.bytesSend = total;
                                                      dc.SubmitChanges();
                                                      lst.Add(new ll() { file = destFile, sort = sources.sort, subBlockId = sources.subBlockId });

                                                  }
                                                  }
                                              //sources.bytesSend = sources.bytes;
                                            sources.status = 2;
                                            dc.SubmitChanges();
                                        }
                                        catch (Exception ex)
                                        {
                                            sources.status = -1;
                                            //sources.description=ex.Message;
                                            dc.SubmitChanges();
                                            throw new Exception("cant copy file");
                                        }

                                    });
                                   
                                    
                                    if (server.typeId == "vMix")
                                    {
                                        var txt = "#EXTM3U\r\n";
                                        lst.OrderBy(r => r.sort).ToList().ForEach(d => {
                                            txt += "#EXTINF:-1, " + System.IO.Path.GetFileNameWithoutExtension(d.file) + "\r\n";
                                            txt += d.file.Replace(server.Path, server.replacePath) + "\r\n";
                                        });

                                        System.IO.File.WriteAllText(System.IO.Path.Combine(folder, "playlist.M3U"), txt, System.Text.Encoding.Unicode);


                                        var url = server.URL;
                                        var pathToPl=System.IO.Path.Combine(folder, "playlist.M3U").Replace(server.Path, server.replacePath);
                                        var apiUrl = new Uri(server.URL + "/API/?Function=AddInput&Value=VideoList|" + pathToPl);
                                        var res=(new System.Net.WebClient()).DownloadString(apiUrl);
                                        l.description = res;
                                    }
                                    if (server.typeId == "FWD")
                                    {
                                        /*var txt = "";

                                       lst.OrderBy(r => r.sort).ToList().ForEach(d => {
                                           txt += "wait operator 0\r\n";
                                           txt += "movie  00:00:00  ";
                                           txt += d.file.Replace(server.Path, server.replacePath) + "\r\n\r\n";
                                       });

                                       System.IO.File.WriteAllText(System.IO.Path.Combine(folder, "playlist.air"), txt, System.Text.Encoding.Unicode);
                                       */
                                        var txt = "";// "<wait start=\"operator\" title=\" * * * * * \" />\r\n";
                                            lst.OrderBy(r => r.sort).ToList().ForEach(d => {
                                                txt += "<movie file=\"" + d.file.Replace(server.Path, server.replacePath) + "\" logo=\"1\" titles=\"1\"/>\r\n";
                                                // txt += "<movie file=\""+ d.file + "\" logo=\"1\" titles=\"1\"/>\r\n";
                                                //txt += d.file.Replace(server.Path, server.replacePath) + "\r\n\r\n";
                                            });


                                        var progName = "";
                                        var newsName = "";
                                        var playlistName = "";
                                        using(var ndc= new News.NewsDataContext())
                                        {
                                            var issue=ndc.News.Where(r => r.id == l.newsId).First();
                                            var prg = ndc.Programs.Where(r => r.id == issue.ProgramId).First();

                                            progName = Translit(prg.Name);

                                            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[:;*'\",_&#^@\\/]");
                                            progName = reg.Replace(progName, string.Empty);

                                            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"\s");
                                            progName = reg.Replace(progName, "_");
                                            progName = progName.Replace(" ", "_");


                                            newsName = Translit(issue.Name);
                                            reg = new System.Text.RegularExpressions.Regex("[:;*'\",_&#^@\\/]");
                                            newsName = reg.Replace(progName, string.Empty);

                                            reg1 = new System.Text.RegularExpressions.Regex(@"\s");
                                            newsName = reg.Replace(newsName, "_");
                                            newsName = newsName.Replace(" ", "_");

                                            newsName = issue.NewsDate.ToString("MMdd_HHmm_") + newsName+"_";
                                        }

                                      
                                         
                                         
                                        var playlistFolderNeme = System.IO.Path.Combine(server.URL, progName);
                                        if (!System.IO.Directory.Exists(playlistFolderNeme))
                                        {
                                            System.IO.Directory.CreateDirectory(playlistFolderNeme);
                                        }

                                        var count = 0;
                                        while (System.IO.File.Exists(System.IO.Path.Combine(playlistFolderNeme, newsName + count.ToString().PadLeft(2, '0') + "_" +".airx")))
                                        {
                                            count++;
                                        }

                                        System.IO.File.WriteAllText(System.IO.Path.Combine(playlistFolderNeme, newsName + count.ToString().PadLeft(2, '0') + "_" + ".airx"), txt, System.Text.Encoding.Unicode);
                                        if (lst.Count > 0)
                                        {
                                            var ageFile = System.IO.Path.GetFileNameWithoutExtension(lst.First().file);
                                            ageFile += ".SLIni";
                                            System.IO.File.WriteAllText(System.IO.Path.Combine(folder, ageFile), "Age=16+", System.Text.Encoding.Unicode);
                                        }
                                        foreach (var item in lst)
                                        {
                                            var client = new System.Net.WebClient();
                                            var srtFile = System.IO.Path.GetFileNameWithoutExtension(item.file);
                                            srtFile += server.SrtPrefix+".srt";
                                            srtFile = System.IO.Path.Combine(folder, srtFile);

                                            try { 
                                            using (var dcMedia = new WebApplication2.Blocks.DataClassesMediaDataContext())
                                            {
                                                var mediaGraphicsId = dcMedia.tWeb_mediaGraphics.Where(g => g.MediaId == Convert.ToInt64(item.subBlockId) && g.layerId == Convert.ToInt32(1)).First().id;
                                                var b = dcMedia.tWeb_MediaGraphicsItems.Where(f => f.mediaGraphicsId == mediaGraphicsId);

                                                b = b.OrderBy(ff => ff.timeInSec);
                                                var a = b.ToList();

                                                for (int i = 0; i < a.Count; i++)
                                                {
                                                    System.IO.File.AppendAllText(srtFile, (i + 1).ToString() + "\r\n", System.Text.Encoding.Unicode);


                                                    TimeSpan ts = new TimeSpan(0, 0, (int)a[i].timeInSec);
                                                    TimeSpan tsEnd = new TimeSpan(0, 0, (i < a.Count - 1) ? (int)a[i + 1].timeInSec : (60 * 60 * 24 - 1));
                                                    System.IO.File.AppendAllText(srtFile, ts.ToString() + ",0 --> " + tsEnd.ToString() + ",0 \r\n", System.Text.Encoding.Unicode);
                                                    System.IO.File.AppendAllText(srtFile, a[i].text.Replace("\r\n", " ").Replace("&nbsp;", " ") + "\r\n\r\n", System.Text.Encoding.Unicode);



                                                }
                                                var fi = new System.IO.FileInfo(srtFile);
                                                if (fi.Length < 10)
                                                    System.IO.File.Delete(srtFile);

                                            }
                                        }
                                            catch(Exception ex)
                                            {

                                            }
                                            
                                        }

                                        foreach (var item in lst)
                                        {
                                            var text = dc.Blocks.Where(g => g.Id == Convert.ToInt64(item.subBlockId)).First().TextLang3;
                                            var srtFile = System.IO.Path.GetFileNameWithoutExtension(item.file);
                                            srtFile += ".crawl.txt";
                                            srtFile = System.IO.Path.Combine(folder, srtFile);
                                            try
                                            {
                                                System.IO.File.AppendAllText(srtFile, text.Replace("&nbsp;"," "), System.Text.Encoding.Unicode);
                                                var fi = new System.IO.FileInfo(srtFile);
                                                if (fi.Length < 10)
                                                    System.IO.File.Delete(srtFile);
                                            }
                                            catch (Exception ex)
                                            {

                                            }

                                        }

                                    }
                                    l.status = 2;
                                    dc.SubmitChanges();

                                }
                            }
                            catch (Exception ex)
                            {
                                l.status = -1;
                                l.description = ex.Message;
                                dc.SubmitChanges();
                            }
                        });
                    }
                }
                catch (Exception ex)
                {

                }
                System.Threading.Thread.Sleep(10 * 1000);
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
    }
}