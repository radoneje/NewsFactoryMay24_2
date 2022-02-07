using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    using (var dc = new mainDataClassesDbataContext())
                    {
                        dc.Blocks.Where(ValueT)
                    }

                }
                catch(Exception ex)
                {
                    Console.WriteLine("ERROR " + ex.Message);
                }
                System.Threading.Thread.Sleep(60 * 60 * 1 * 1000);
            }
        }
    }
}
