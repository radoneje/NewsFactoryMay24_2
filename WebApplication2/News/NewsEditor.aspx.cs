using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.News
{
    public partial class NewsEditor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Request.Params["NewsId"] == null)
            {
                string scripttext = "ShowDisabledMessage(\"Не могу прочитать ID выпуска \", \"Ошибка\");";
                ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);
                return;
            }
            if (Request.Params["GroupId"] == null)
            {
                string scripttext = "ShowDisabledMessage(\"Не могу прочитать ID группы выпусков \", \"Ошибка\");";
                ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);
                return;
            }
            long NewsId = 0;
            long GroupId = 0;
            try
            {
                NewsId = Convert.ToInt64(Request.Params["NewsId"]);
                GroupId = Convert.ToInt64(Request.Params["GroupId"]);
            }
            catch (Exception ex)
            {
                string scripttext = "ShowDisabledMessage(\"Не могу распознать ID выпуска \", \"Ошибка\");";
                ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);
                return;
            }

            if (Request.Cookies["NFWSession"] == null)
            {
                Response.Redirect("/login.aspx");
                return;
            }

            string sCookie = Request.Cookies["NFWSession"].Value;
            if (Page.IsPostBack == true)
                return;

           
            if (GroupId == 0)
            {
                ReloadEditors(NewsId);
                ReloadNews(NewsId);
            }
            else
            {
                ReloadCopyEditors(NewsId);
                ReloadCopyNews(NewsId);
            }
            
        }
        private void ReloadEditors(Int64 NewsId)
        {
            NewsEditEditorDropDown.Items.Clear();
            News.NewsDataContext dc = new NewsDataContext();
            

            var programId=dc.News.Where(n=>n.id==NewsId).First().ProgramId;

            foreach(var u in dc.vUsersRights.Where(r=>r.ProgramID==programId && ((r.RightID==22 )|| (r.RightID==24))).OrderBy(f=>f.UserName))
            {
                bool find = false;
                for (int i = 0; i < NewsEditEditorDropDown.Items.Count; i++ )
                {
                    find = NewsEditEditorDropDown.Items[i].Value == u.UserID.ToString();
                }
                if(!find)
                       NewsEditEditorDropDown.Items.Add(new ListItem() { Text = u.UserName, Value = u.UserID.ToString() });
            }
        }

        private void ReloadCopyEditors(Int64 NewsId)
        {
            NewsEditEditorDropDown.Items.Clear();
            News.NewsDataContext dc = new NewsDataContext();


            

            foreach (var u in dc.vUsersRights.Where(r =>  ((r.RightID == 22) || (r.RightID == 24))).OrderBy(f => f.UserName))
            {
                bool find = false;
                for (int i = 0; i < NewsEditEditorDropDown.Items.Count; i++)
                {
                    find = NewsEditEditorDropDown.Items[i].Value == u.UserID.ToString();
                }
                if (!find)
                    NewsEditEditorDropDown.Items.Add(new ListItem() { Text = u.UserName, Value = u.UserID.ToString() });
            }
        }
       
        private void  ReloadNews(Int64 NewsId)
        {
            News.NewsDataContext dc = new NewsDataContext();
            var ns= dc.News.Where(n=>n.id==NewsId).First();
            NewsEditorNewsName.Text = ns.Name;
            for (int i = 0; i < NewsEditEditorDropDown.Items.Count; i++)
            {
                if (NewsEditEditorDropDown.Items[i].Value == ns.EditorId.ToString())
                    NewsEditEditorDropDown.SelectedIndex = i;
            }
            NewsEditDate.Text = ns.NewsDate.ToString("dd.MM.yyyy");
            NewsEditTime.Text = ns.NewsDate.ToString("HH:mm:ss");
            ChronoReal.Text = TimeSpan.FromSeconds(ns.NewsTime).ToString();
             BlockEditDescription.Text = ns.Description;
             NewsEditDur.Text  = TimeSpan.FromSeconds(ns.Duration).ToString();
             CronoPlanned.Text = TimeSpan.FromSeconds(ns.TaskTime ).ToString();
             CronoCalc.Text = TimeSpan.FromSeconds(ns.CalcTime ).ToString();
             Cassete.Text = ns.Cassete;
             Timecode.Text = TimeSpan.FromSeconds(ns.Time_Code ).ToString();
             NewsEditorNewsId.Value = ns.id.ToString();
             NewsEditorGroupId.Value = "0";

        }
        private void ReloadCopyNews(Int64 NewsId)
        {
            News.NewsDataContext dc = new NewsDataContext();
            var ns = dc.CopyNews.Where(n => n.id == NewsId).First();
            NewsEditorNewsName.Text = ns.Name;
            for (int i = 0; i < NewsEditEditorDropDown.Items.Count; i++)
            {
                if (NewsEditEditorDropDown.Items[i].Value == ns.EditorId.ToString())
                    NewsEditEditorDropDown.SelectedIndex = i;
            }
            NewsEditDate.Text = ns.NewsDate.ToString("dd.MM.yyyy");
            NewsEditTime.Text = ns.NewsDate.ToString("hh:mm:ss");
            ChronoReal.Text = TimeSpan.FromSeconds(ns.NewsTime).ToString();
            BlockEditDescription.Text = ns.Description;
            NewsEditDur.Text = TimeSpan.FromSeconds(ns.Duration).ToString();
            CronoPlanned.Text = TimeSpan.FromSeconds(ns.TaskTime).ToString();
            CronoCalc.Text = TimeSpan.FromSeconds(ns.CalcTime).ToString();
            Cassete.Text = ns.Cassete;
            Timecode.Text = TimeSpan.FromSeconds(ns.Time_Code).ToString();
            NewsEditorNewsId.Value = ns.id.ToString();
            NewsEditorGroupId.Value = "1";

        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {

        }

        protected void NewsEditorSaveButton_Click(object sender, EventArgs e)
        {
            if (NewsEditorGroupId.Value != "0")
            {
                SaveCopyNews();
            }
            else
            {
                using (News.NewsDataContext dc = new NewsDataContext())
                {

                    var ns = dc.News.Where(n => n.id.ToString() == NewsEditorNewsId.Value).First();
                    ns.Name = NewsEditorNewsName.Text;
                    ns.EditorId = Convert.ToInt32(NewsEditEditorDropDown.SelectedValue);
                    string datestring = NewsEditDate.Text + " " + NewsEditTime.Text;
                    try
                    {
                        ns.NewsDate = DateTime.ParseExact(datestring, "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                    }
                    catch (Exception ex)
                    {
                        string scripttext = "alert('Укажите дату в формате ДД:ММ:ГГГГ чч:мм:сс');NewsEditDur.focus();";
                        ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);
                        return;
                    }
                    try
                    {
                        ns.Duration = (Int64)TimeSpan.Parse(NewsEditDur.Text).TotalSeconds;
                    }
                    catch (Exception ex)
                    {
                        string scripttext = "alert('Укажите продолжительность в формате чч:мм:сс');NewsEditDur.focus();";
                        ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);
                        return;
                    }
                    ns.Cassete = Cassete.Text;
                    try
                    {
                        ns.Time_Code = (Int64)TimeSpan.Parse(Timecode.Text).TotalSeconds;
                    }
                    catch (Exception ex)
                    {
                        string scripttext = "alert('Укажите таймкод в формате чч:мм:сс');NewsEditDur.focus();";
                        ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);
                        return;
                    }

                    try
                    {
                        dc.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        string scripttext = "alert('При сохранении произошла ошибка!');NewsEditDur.focus();";
                        ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);
                        return;
                    }
                    NFSocket.SendToAll.SendData("newsEdit", new { newsId = ns.id });
                    ReloadNews(Convert.ToInt64(NewsEditorNewsId.Value));
                }
            }
            //string script = "alert('Информация сохранена.');";
            //ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", script, true);
            return;
        }
        protected void SaveCopyNews()
        {
            using (News.NewsDataContext dc = new NewsDataContext())
            {

                var ns = dc.CopyNews.Where(n => n.id.ToString() == NewsEditorNewsId.Value).First();
                ns.Name = NewsEditorNewsName.Text;
                ns.EditorId = Convert.ToInt32(NewsEditEditorDropDown.SelectedValue);
                string datestring = NewsEditDate.Text + " " + NewsEditTime.Text;
                try
                {
                    ns.NewsDate = DateTime.ParseExact(datestring, "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    string scripttext = "alert('Укажите дату в формате ДД:ММ:ГГГГ чч:мм:сс');NewsEditDur.focus();";
                    ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);
                    return;
                }
                try
                {
                    ns.Duration = (Int64)TimeSpan.Parse(NewsEditDur.Text).TotalSeconds;
                }
                catch (Exception ex)
                {
                    string scripttext = "alert('Укажите продолжительность в формате чч:мм:сс');NewsEditDur.focus();";
                    ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);
                    return;
                }
                ns.Cassete = Cassete.Text;
                try
                {
                    ns.Time_Code = (Int64)TimeSpan.Parse(Timecode.Text).TotalSeconds;
                }
                catch (Exception ex)
                {
                    string scripttext = "alert('Укажите таймкод в формате чч:мм:сс');NewsEditDur.focus();";
                    ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);
                    return;
                }

                try
                {
                    dc.SubmitChanges();
                }
                catch (Exception ex)
                {
                    string scripttext = "alert('При сохранении произошла ошибка!');NewsEditDur.focus();";
                    ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);
                    return;
                }

                ReloadCopyNews(Convert.ToInt64(NewsEditorNewsId.Value));
                string script = "alert('Информация сохранена.');";
                ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", script, true);

                NFSocket.SendToAll.SendData("newsEdit", new { newsId=ns.id});
                return;
            }
        }

    }
    
  
}