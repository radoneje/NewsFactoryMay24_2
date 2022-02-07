using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.News
{
    public partial class newsToTitle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                var server = "http://localhost";
                try
                {
                    server = "http://" + dc.tWeb_Settings.Where(r => r.Key == "ServerName").First().value;
                }
                catch { }
                scriptDiv.InnerHtml = "<script>var newsId=" + Request.Params["id"] + ";var serverName='"+server+"'</script>";
            }
        }
    }
}