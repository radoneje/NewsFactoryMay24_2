using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cinegyRemove
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    using (var dc = new mainDataClassesDataContext())
                    {
                        dc.Blocks.Where(b => b.deleted == true && b.ParentId > 0).ToList().ForEach(bl=> {
                            var filename = bl.BlockText;
                            
                            Console.WriteLine(" find filename:", filename);
                            if (System.IO.File.Exists(filename))
                            {
                                try
                                {
                                    System.IO.File.Delete(filename);
                                    Console.WriteLine(" delete file", filename);
                                    bl.BlockText = "";
                                    dc.SubmitChanges();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("error delete file", ex.Message);
                                }



                            }
                            else
                                Console.WriteLine(" file not exist", filename);
                        });       
                        var blocks = dc.Blocks.Where(b => b.deleted == true && b.ParentId == 0);
                        Console.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " find blocks:", blocks.Count().ToString());
                        blocks.ToList().ForEach(block =>
                           {
                               dc.Blocks.Where(sb => sb.ParentId == block.Id && sb.BlockText.Length > 4)
                               .ToList()
                               .ForEach(subBlock =>
                               {

                                   var filename = subBlock.BlockText;
                                   Console.WriteLine(" find filename:", filename);
                                   if (System.IO.File.Exists(filename))
                                   {
                                       try
                                       {
                                           System.IO.File.Delete(filename);
                                           Console.WriteLine(" delete file", filename);
                                           subBlock.BlockText = "";
                                           dc.SubmitChanges();
                                       }
                                       catch (Exception ex)
                                       {
                                           Console.WriteLine("error delete file", ex.Message);
                                       }



                                   }
                                   else
                                       Console.WriteLine(" file not exist", filename);

                               });

                           });
                    }

                    
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error " + ex.Message);
                }
                Console.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "sleep ");
                System.Threading.Thread.Sleep(60*60*1000);
            }
        }
    }
}
