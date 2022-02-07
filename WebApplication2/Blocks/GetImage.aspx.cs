using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Blocks
{
    public partial class GetImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string test = "test";
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            var sw = new System.IO.StreamWriter(ms);
            sw.WriteLine(test);

            Response.Clear();
            Response.ContentType = "text/plain";

            Response.BinaryWrite(ms.ToArray());
            // myMemoryStream.WriteTo(Response.OutputStream); //works too
            Response.Flush();
            Response.Close();
            Response.End();
        }
    }
}