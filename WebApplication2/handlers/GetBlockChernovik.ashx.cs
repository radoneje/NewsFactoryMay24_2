using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;


namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for GetImage
    /// </summary>
    public class GetBlockChernovik : IHttpHandler
    {

        

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/plain";
            var sw = new StreamWriter(context.Response.OutputStream);

           
  
            if (context.Request.Params["blockId"] == null)
            {
               

                sw.WriteLine("noText");
                sw.Close();
                return;
            }

         

            sw.WriteLine("chermovik");
            sw.Close();
        }
      
    }
}