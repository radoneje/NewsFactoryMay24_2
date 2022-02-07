using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Blocks
{
    public partial class blockDeleted : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                rows.InnerHtml = "no access<br><a href='"+Application["serverRoot"]+"login'> Use NewsFactory main interface.</a> ";
                return;
            }
            using(var dc= new DataClasses1DataContext())
            {

                dc.vWeb_DeletedBlocksWithSort.OrderByDescending(b => b.EventDate).Take(400).ToList().ForEach(l => {
                    rows.InnerHtml += " <div id='APBlockDeleted' class='news-row'>\r\n";
        
                    rows.InnerHtml += "        <div style='display:inline-block; width:calc(100% - 210px)'>" + l.newsName.Replace("<", "") + " <small>" + l.NewsDate.ToString("dd.MM.yyyy HH:mm:ss") + "</small>" + "</div>\r\n";
                    rows.InnerHtml += "        <div style='display:block; width:100%'>" + l.blockName.Replace("<", "") + "</div>\r\n";
                    rows.InnerHtml += "        <div style='display:inline-block; width:calc(100% - 110px)'><small><b><i>" + l.UserName + "</i></b></small></div>\r\n";
                    rows.InnerHtml += "        <div style='display:inline-block; width:100px'> <input type='button' value='restore' style=' width:100%' class='btn btn-xs btn-warning' onclick='blockUndelete(" + l.Id.ToString() + ")'/></div>\r\n";
                    rows.InnerHtml += "    </div>";

                });
                    /*  dc.vWeb_DeletedBlocks.OrderByDescending(b => b.NewsDate).Take(400).ToList().ForEach(l => {

                          var her = dc.vWeb_BlockDeleteUsers.Where(u => u.ItemID == l.Id);
                          string hero = "";
                          if(her.Count()>0)
                          {
                              var hreF=her.OrderByDescending(uu => uu.EventDate).First();
                              hero += hreF.UserName + " " + hreF.EventDate.ToString("dd.MM.yyyy HH:mm:ss");

                          }

                      rows.InnerHtml+=" <div id='APBlockDeleted"+l.Id.ToString()+"' class='news-row'>\r\n";
                      rows.InnerHtml += "        <div style='display:inline-block; width:200px'>" + l.Id.ToString() + "</div>\r\n";
       rows.InnerHtml+="        <div style='display:inline-block; width:calc(100% - 210px)'>"+l.NewsName.Replace("<","")+" <small>"+l.NewsDate.ToString("dd.MM.yyyy HH:mm:ss")+"</small>"+"</div>\r\n";
       rows.InnerHtml+="        <div style='display:block; width:100%'>"+l.Name.Replace("<","")+"</div>\r\n";
       rows.InnerHtml += "        <div style='display:inline-block; width:calc(100% - 110px)'><small><b><i>" + hero + "</i></b></small></div>\r\n";
       rows.InnerHtml+="        <div style='display:inline-block; width:100px'> <input type='button' value='restore' style=' width:100%' class='btn btn-xs btn-warning' onclick='blockUndelete("+l.Id.ToString()+")'/></div>\r\n";
       rows.InnerHtml += "    </div>";
                      });*/

                }
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