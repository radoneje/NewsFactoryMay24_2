using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.News
{
    public partial class newsDeleted : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                rows.InnerHtml = "no access<br><a href='"+Application["serverRoot"]+"login'> Use NewsFactory main interface.</a> ";
                return;
            }
            using(var dcn = new News.NewsDataContext())
            using(Blocks.DataClasses1DataContext dc= new Blocks.DataClasses1DataContext())
            {
                dcn.News.Where(t=>t.Deleted==true).OrderByDescending(b => b.NewsDate).Take(400).ToList().ForEach(l => {
                
                                    rows.InnerHtml+=" <div id='APnewsDeleted"+l.id.ToString()+"' class='news-row'>\r\n";
                rows.InnerHtml += "        <div style='display:inline-block; width:200px'>" + l.id.ToString() + "</div>\r\n";
 rows.InnerHtml+="        <div style='display:inline-block; width:calc(100% - 210px)'>"+l.Name.Replace("<","")+" <small>"+l.NewsDate.ToString("dd.MM.yyyy HH:mm:ss")+"</small>"+"</div>\r\n";
 rows.InnerHtml+="        <div style='display:block; width:100%'>"+l.Name.Replace("<","")+"</div>\r\n";
 rows.InnerHtml += "        <div style='display:inline-block; width:calc(100% - 110px)'><small><b><i>"  + "</i></b></small></div>\r\n";
 rows.InnerHtml+="        <div style='display:inline-block; width:100px'> <input type='button' value='restore' style=' width:100%' class='btn btn-xs btn-warning' onclick='newsUndelete("+l.id.ToString()+")'/></div>\r\n";
 rows.InnerHtml += "    </div>";
                });

    
                
            /*
             * <div id="123" class="news-row">
        <div style="display:inline-block; width:100px"> 123</div>
        <div style="display:inline-block; width:200px"> 12-33 fknvkfnvkdf</div>
        <div style="display:inline-block; width:300px"> fvfvfdvdf</div>
        <div style="display:inline-block; width:100px"> <input type="button" value="restore" style=" width:100%" class="btn btn-xs btn-warning"/></div>
    </div>
             * */
        }
        }
    }
}