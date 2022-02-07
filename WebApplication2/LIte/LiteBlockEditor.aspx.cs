using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.LIte
{
    public partial class LiteBlockEditor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["UserId"]==null)
            {
                ErrorMessage("Вы не авторизованы.<br><a href='/login'>Зарегистрируйтесь в сиcтеме</a>");
                return;
            }
            string BlockId =  (string)Page.RouteData.Values["BlockId"];
            if (BlockId == null)
            {
                ErrorMessage(" 404. По ссылке ничего не найдено");
                return;
            }
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                var brec = dc.vWeb_BlockToExtView.Where(t => t.Id == Convert.ToInt64(BlockId));
                {
                    if (brec.Count() == 0)
                    {
                        ErrorMessage(" 404. Блок был удален.");
                        return;
                    }
                    var Block = brec.First();

                    Elements.BlocksExtViewer.BlockNamePanel BlockNamePanel = (Elements.BlocksExtViewer.BlockNamePanel)LoadControl("~/Elements/BlocksExtViewer/BlockNamePanel.ascx");
                    BlockNamePanel.Title = Block.Name;
                    BlockNamePanel.SubTitle = "Автор: " + Block.Creator;
                    BlockNamePanel.Text = Block.BlockText.Replace("<", "").Replace(">", "");
                    BlockNamePanel.IsCommentable = true;
                    BlockNamePanel.CommentText = Block.Description;
                    BlockNamePanel.BlockId = BlockId.ToString();
                    BlockNameContainer.Controls.Add(BlockNamePanel);

                    if (dc.vMedia_MediaToLists.Where(b => b.ParentId == Block.Id).Count() > 0)
                    {
                        Elements.BlocksExtViewer.BlockMediaPanel BlockMediaPanel = (Elements.BlocksExtViewer.BlockMediaPanel)LoadControl("~/Elements/BlocksExtViewer/BlockMediaPanel.ascx");
                        BlockMediaPanel.BlockId = Block.Id;
                        BlockMediaContainer.Controls.Add(BlockMediaPanel);
                    }
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