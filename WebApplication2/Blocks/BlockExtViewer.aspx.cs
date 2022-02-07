using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Blocks
{
    public partial class BlockExtViewer : System.Web.UI.Page
    {
    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                
            }

            string BlockGuid;
            string BlockId;
            BlockId = (string)Page.RouteData.Values["BlockID"];
            BlockGuid = (string)Page.RouteData.Values["BlockGuid"];


            if (BlockGuid == null && BlockId==null)
            {
                ErrorMessage(" 404. По ссылке ничего не найдено");
                return;
            }

            using (var dc = new DataClasses1DataContext())
            {
                var rec = dc.vWEB_ExtLinkToLists.Where(b => b.URL == BlockGuid && !b.Deleted);
                if(rec.Count()==0)
                {
                    ErrorMessage(" 404. Доступ не предоставлен.");
                    return;
                }
                var access = rec.First();
                if(access.IsExpirience ==true && access.ExpirienceDate.Date< DateTime.Now.Date )
                {
                    ErrorMessage(" Срок доступа истек: " + access.ExpirienceDate.Date.ToString("dd.MM.yyyy."));
                    return;
                }
                if (access.IsExpirience == true)
                    SuccessMessage("Доступ истекает: " + access.ExpirienceDate.Date.ToString("dd.MM.yyyy."));
                
                var  brec= dc.vWeb_BlockToExtView.Where(t => t.Id==access.BlockId );
                {
                    if (brec.Count() == 0)
                    {
                        ErrorMessage(" 404. Блок был удален.");
                        return;
                    }
                }


                var Block = brec.First();

                Elements.BlocksExtViewer.BlockNamePanel BlockNamePanel = (Elements.BlocksExtViewer.BlockNamePanel)LoadControl("~/Elements/BlocksExtViewer/BlockNamePanel.ascx");
                 BlockNamePanel.Title = Block.Name.Replace("<", "&lt;").Replace(">", "&gt;");
                 BlockNamePanel.SubTitle = "Автор: " + Block.Creator;
                 BlockNamePanel.Text = Block.BlockText.Replace("<", "&lt;").Replace(">", "&gt;");
                 BlockNamePanel.IsCommentable = access.IsCommentable;
                 BlockNamePanel.CommentText = Block.Description;
                 BlockNamePanel.BlockGuid = BlockGuid;
                 BlockNameContainer.Controls.Add(BlockNamePanel);
               
                if(dc.vMedia_MediaToLists.Where(b=>b.ParentId ==Block.Id ).Count()>0)
                {
                    Elements.BlocksExtViewer.BlockMediaPanel  BlockMediaPanel = (Elements.BlocksExtViewer.BlockMediaPanel)LoadControl("~/Elements/BlocksExtViewer/BlockMediaPanel.ascx");
                    BlockMediaPanel.BlockId = Block.Id;
                    BlockMediaContainer.Controls.Add(BlockMediaPanel);
                }
            }
        }
        protected void ErrorMessage(string msg)
        {
            Elements.BlocksExtViewer.AlertPanel AlertPanel = (Elements.BlocksExtViewer.AlertPanel)LoadControl("~/Elements/BlocksExtViewer/AlertPanel.ascx");
            AlertPanel.Message = msg;
            AlertContainer.Controls.Add(AlertPanel);
        }
        protected void SuccessMessage(string msg)
        {
            Elements.BlocksExtViewer.SuccessPanel AlertPanel = (Elements.BlocksExtViewer.SuccessPanel)LoadControl("~/Elements/BlocksExtViewer/SuccessPanel.ascx");
            AlertPanel.Message = msg;
            AlertContainer.Controls.Add(AlertPanel);
        }
    }
}