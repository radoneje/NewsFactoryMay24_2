using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.News
{
    public partial class NewsTree : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Reload_News(1);

        }
        protected void Reload_News(int ProgramID)
        {
            NewsDataContext dc= new NewsDataContext();

            NewsTable.Rows.Clear();

            DateTime tommorow=DateTime.Now.Date.AddDays(1);

            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            tr.Cells.Add(tc);
            NewsTable.Rows.Add(tr);
            tc.Text = "<img src='/Images/green_list.png' width=18px> планируемые";
            tc.BackColor = System.Drawing.Color.LightGray;



            foreach(var a in dc.News.Where(d=>d.NewsDate>=tommorow && d.Deleted==false).OrderByDescending(d=>d.NewsDate ))
            {
                System.Web.UI.HtmlControls.HtmlGenericControl  div = new System.Web.UI.HtmlControls.HtmlGenericControl();
                div.InnerHtml = "<div id=\""+a.id.ToString()+"\" onclick=\"alert('click' + this.id);\" >"+a.Name +"</div>";
                //PanelTommorow.Controls.Add(div);
                TableRow tr1 = new TableRow();
                TableCell tc1 = new TableCell();
                tr1.Cells.Add(tc1);
                NewsTable.Rows.Add(tr1);
                tc1.Controls.Add(div);
            }

            DateTime today = DateTime.Now.Date;

             tr = new TableRow();
             tc = new TableCell();
            tr.Cells.Add(tc);
            NewsTable.Rows.Add(tr);
            tc.Text = "<img src='/Images/green_list.png' width=18px> сегодня";
            tc.BackColor = System.Drawing.Color.LightGray; ;

            foreach (var a in dc.News.Where(d => d.NewsDate >= today && d.NewsDate < today.AddDays(1) && d.Deleted == false).OrderByDescending(d => d.NewsDate))
            {
                System.Web.UI.HtmlControls.HtmlGenericControl div = new System.Web.UI.HtmlControls.HtmlGenericControl();
                div.InnerHtml = "<div id=\"" + a.id.ToString() + "\" onclick=\"alert('click' + this.id);\" >" + a.Name + "</div>";
                //PanelToday.Controls.Add(div);

                TableRow tr1 = new TableRow();
                TableCell tc1 = new TableCell();
                tr1.Cells.Add(tc1);
                NewsTable.Rows.Add(tr1);
                tc1.Controls.Add(div);
            }

            tr = new TableRow();
            tc = new TableCell();
            tr.Cells.Add(tc);
            NewsTable.Rows.Add(tr);
            tc.Text = "<img src='/Images/green_list.png' width=18px> прошедшие ";
            tc.BackColor = System.Drawing.Color.LightGray; ;

            foreach (var a in dc.News.Where(d=> d.NewsDate < today  && d.Deleted == false).OrderByDescending(d => d.NewsDate))
            {
                System.Web.UI.HtmlControls.HtmlGenericControl div = new System.Web.UI.HtmlControls.HtmlGenericControl();
                div.InnerHtml = "<div id=\"" + a.id.ToString() + "\" onclick=\"ClickNews(this.id);\" ondblclick=\"alert('dblclick' + this.id);\" style=\"cursor:pointer\">" + a.Name + "</div>";
                //PanelLast.Controls.Add(div);

                TableRow tr1 = new TableRow();
                TableCell tc1 = new TableCell();
                tr1.Cells.Add(tc1);
                NewsTable.Rows.Add(tr1);
                tc1.Controls.Add(div);
            }
        }
    }
}