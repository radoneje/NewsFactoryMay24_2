using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.LIte.NewsElements
{
    public partial class NewsColumn : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if(Session["UserId"]==null)
            {
                return;
            }
            
            if(!IsPostBack )
                 ProgramReload();
            //NewsPlannedLink.Text = "<div onclick='document.location(\"/l/" + ProgrammDropDown.SelectedValue + "/1/#1\");'></div>";
               
            
        }
        protected void ProgramReload()
        {

            ProgrammDropDown.Items.Clear();
            using (LIte.LiteDataClassesDataContext dc = new LiteDataClassesDataContext())
            {
                bool selected = true;
                int SelectedId = -1;
                if (Session["ProgramId"] != null)
                {
                    SelectedId = Convert.ToInt32(Session["ProgramId"]);
                }
                foreach (var prog in dc.vLite_ProgramsFromUsers.Where(p => p.UserID == (int)Session["UserId"]))
                {
                    ProgrammDropDown.Items.Add(new ListItem() { 
                    Text=prog.Name,
                    Value=prog.ProgramID.ToString(),
                    Selected = (SelectedId>=0) ?SelectedId==prog.ProgramID:selected ,
                    });
                    selected = false;
                }
            }
            if (Session["NewsGroupSelected"] != null)
                ReloadNews();

        }

        protected void ProgrammDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ReloadNews();
            Page.Response.Redirect("/L/" + ProgrammDropDown.SelectedValue + "/");
        }
        protected void ReloadNews()
        {
            
            NewsPlanned.Controls.Clear();
            NewsToday.Controls.Clear();
            NewsLast.Controls.Clear();
            if (Session["NewsGroupSelected"] == null)
             return;
            switch (Convert.ToInt32(Session["NewsGroupSelected"]))
            {
                case 1:NewsPlannedReload();break;
                case 2:NewsTodaydReload();break;
                case 3:NewsLastReload();break;
            }

        }
        protected void NewsPlannedReload(){
            NewsPlanned.Controls.Add(new Literal() { Text = "<a name='1'></a>" });
            using(LIte.LiteDataClassesDataContext dc = new LiteDataClassesDataContext())
            {
              foreach(var news in  dc.vLite_NewsToLists.Where(n => n.NewsDate.Date > DateTime.Now.Date && n.ProgramId==Convert.ToInt32( ProgrammDropDown.SelectedValue)).OrderByDescending(nn => nn.NewsDate))
              {
                  NewsItem Item = (NewsItem)LoadControl("~/LIte/NewsElements/NewsItem.ascx");
                  Item.NewsDate = news.NewsDate.ToString("dd.MM.yyyy hh:mm:ss");
                  Item.NewsName = news.Name;
                  Item.NewsId = news.id;
                  NewsPlanned.Controls.Add(Item);
              }
            }
            
        }
        protected void NewsTodaydReload(){
            NewsToday.Controls.Add(new Literal() { Text = "<a name='1'></a>" });
            using (LIte.LiteDataClassesDataContext dc = new LiteDataClassesDataContext())
            {
                foreach (var news in dc.vLite_NewsToLists.Where(n => n.NewsDate.Date == DateTime.Now.Date && n.ProgramId == Convert.ToInt32(ProgrammDropDown.SelectedValue)).OrderByDescending(nn => nn.NewsDate))
                {
                    NewsItem Item = (NewsItem)LoadControl("~/LIte/NewsElements/NewsItem.ascx");
                    Item.NewsDate = news.NewsDate.ToString("dd.MM.yyyy hh:mm:ss");
                    Item.NewsName = news.Name;
                    Item.NewsId = news.id;
                    NewsToday.Controls.Add(Item);
                }
            }
        }
        protected void NewsLastReload() {
            NewsLast.Controls.Add(new Literal() { Text = "<a name='1'></a>" });
            using (LIte.LiteDataClassesDataContext dc = new LiteDataClassesDataContext())
            {
                foreach (var news in dc.vLite_NewsToLists.Where(n => n.NewsDate.Date < DateTime.Now.Date && n.ProgramId == Convert.ToInt32(ProgrammDropDown.SelectedValue)).OrderByDescending(nn => nn.NewsDate))
                {
                    NewsItem Item = (NewsItem)LoadControl("~/LIte/NewsElements/NewsItem.ascx");
                    Item.NewsDate = news.NewsDate.ToString("dd.MM.yyyy hh:mm:ss");
                    Item.NewsName = news.Name.Replace("<", "&lt;").Replace(">", "&gt;");
                    Item.NewsId = news.id;
                    NewsLast.Controls.Add(Item);
                }
            }
        }
    }
}