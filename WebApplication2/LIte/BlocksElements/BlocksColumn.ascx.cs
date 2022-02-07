using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.LIte.BlocksElements
{
    public partial class BlocksColumn : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BlockHeadPanel.Controls.Clear();
            BlockBodyPanel.Controls.Clear();
            if(Session["NewsId"]!=null)
            {
                ReloadHead();
                ReloadBlocks();
            }
        }
        protected void ReloadBlocks() {
           
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                foreach(var bl in dc.vWeb_Blocks.Where(n => n.NewsId == Convert.ToInt64(Session["NewsId"]) && n.ParentId==0).OrderBy(b => b.Sort))
                {
                    BlockItem item = (BlockItem)Page.LoadControl("~/LIte/BlocksElements/BlockItem.ascx");
                    if(bl.Approve)
                    {
                        item.imgUrl = "/Images/green_list.png";
                    }
                    else
                    {
                        item.imgUrl = bl.Ready ? "/Images/yellow_list.png" : "/Images/red_list.png";
                    }
                    item.Name = bl.Name.Replace("<", "&lt;").Replace(">", "&gt;");;
                    item.Type = bl.TypeName;
                    item.Autor = dc.fWeb_GetUserName(bl.CreatorId);
                    item.Real = bl.CalcTime;
                    item.Planned = bl.TaskTime;
                    item.BlockId = bl.Id.ToString();
                    
                    BlockBodyPanel.Controls.Add(item);
                }
                
            }
        }
        protected void ReloadHead() {
            BlockHead head = (BlockHead)Page.LoadControl("~/LIte/BlocksElements/BlockHead.ascx");
            using (Blocks.DataClasses1DataContext dc= new Blocks.DataClasses1DataContext())
            {
                var ns=dc.vWeb_NewsForBlocksHeads.Where(n => n.id == Convert.ToInt64(Session["NewsId"])).First();
                head.NewsName = ns.Name;
                head.EditorName = ns.UserName;
                head.Date = ns.NewsDate.ToString("dd.MM.yyyy hh.mm.ss");
                head.Duration = SecondsToTimeString(ns.Duration );

                head.Calc = SecondsToTimeString(ns.CalcTime);
                head.Planned = SecondsToTimeString(ns.TaskTime);
                head.Chrono = SecondsToTimeString(ns.NewsTime);
            }
            BlockHeadPanel.Controls.Add(head);

        }
        public string SecondsToTimeString(Int64 sec)
        {
            TimeSpan ts = new TimeSpan(0, 0, (int)sec);
            return ts.ToString();

        }

    }
}