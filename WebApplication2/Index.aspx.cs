using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.CacheControl = "no-cache";
            PrintTemplates.PrintTemplatesDataClassesDataContext dc1 = new PrintTemplates.PrintTemplatesDataClassesDataContext();

            string s = "function GenerateBlockScriptButtons(BlockId)\r\n";
            s = s + "{\r\n";
            s = s + "var ret = '<button type=\"button\" class=\"btn btn-default \" data-toggle=\"dropdown\" aria-expanded=\"false\">';\r\n";
            s = s + "ret+='Скрипт';\r\n";
            s = s + "ret+='<span class=\"caret\"></span>';\r\n";
            s = s + "ret+='</button>';\r\n";
            s = s + "ret+='<ul class=\"dropdown-menu\" role=\"menu\">';\r\n";

            foreach (var a in dc1.PrintTemplates)
            {
                s = s + "ret+='<li><a onclick=\"OpenBlockScript('+BlockId+', " + a.id.ToString() + ")\">" + a.name.Replace("<", "").Replace(">", "") + "</a></li>';\r\n";
            }
            s = s + "ret += '</ul>';\r\n";
            s = s + "return ret;}\r\n";

            ClientScript.RegisterClientScriptBlock(typeof(Page), "scriptBlocksButton", s, true);

            s = "function GenerateNewsScriptButtons(BlockId)\r\n";
            s = s + "{\r\n";
            s = s + "var ret = '<button type=\"button\" class=\"btn btn-default \" data-toggle=\"dropdown\" aria-expanded=\"false\">';\r\n";
            s = s + "ret+='Скрипт';\r\n";
            s = s + "ret+='<span class=\"caret\"></span>';\r\n";
            s = s + "ret+='</button>';\r\n";
            s = s + "ret+='<ul class=\"dropdown-menu\" role=\"menu\">';\r\n";
        }
        public string getRouteScript()
        {
            string ret="";
            if(Page.RouteData.Values["route_programId"]!=null)
            {
                ret+="<script>\r\n";
                ret+="var lastRoute = {}\r\n"; 
                if(Page.RouteData.Values["route_groupId"]!=null)
                     ret+="lastRoute.groupId="+Page.RouteData.Values["route_groupId"]+";\r\n";
                if(Page.RouteData.Values["route_newsId"]!=null)
                     ret+="lastRoute.newsId="+Page.RouteData.Values["route_newsId"]+";\r\n";
                 if(Page.RouteData.Values["route_blockId"]!=null)
                     ret+="lastRoute.blockId="+Page.RouteData.Values["route_blockId"]+";\r\n";
                ret+="lastRoute.programId="+Page.RouteData.Values["route_programId"]+";\r\n";
                ret+="</script>\r\n";
            }
            return ret;

        }
    }
}