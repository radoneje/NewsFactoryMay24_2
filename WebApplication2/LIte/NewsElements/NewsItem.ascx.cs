using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.LIte.NewsElements
{
    
    public partial class NewsItem : System.Web.UI.UserControl
    {
        public string NewsName { set { NewsNameLiteral.Text = value; } }
        public string NewsDate { set { NewsDateLiteral.Text = value; } }
        public Int64 NewsId { set { NewsIdHidden.Value = value.ToString(); } }
    

        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["NewsId"] != null)
               {
                   string SelectedId = (string)Session["NewsId"];
                   if(SelectedId==NewsIdHidden.Value)
                   {
                       ItemPanel.CssClass = "NewsItemSelected";
                   }
               }
            //NewsLinkLiteral .Text = "< a name='NewsItem" + iNewsId.ToString()+ "'></a>";
        }
    }
}