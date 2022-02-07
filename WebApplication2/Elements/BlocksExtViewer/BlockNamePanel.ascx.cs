using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace WebApplication2.Elements.BlocksExtViewer
{
    public partial class BlockNamePanel : System.Web.UI.UserControl
    {
        public string Title { set { TitleLiteral.Text  = value; } }
        public string SubTitle { set { SubTitleLiteral.Text = value; } }
        public string Text { set { TextLiteral.Text = AddMediaLink(value); } }
        public bool IsCommentable { set { CommentPanel.Visible = value; } }
        public string CommentText { set { if(!IsPostBack) CommentTb.Text = value; } }
        public string BlockGuid;
        public string BlockId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack && CommentPanel.Visible)
            {
                
            }
            if(BlockGuid==null && BlockId!=null )
            {
                Button btn= new Button();
                btn.Text="Закрыть";
                btn.CssClass="btn btn-default btn-success";
                btn.Click += btnClose_Click;
                SaveButtonsPanel.Controls.Add(btn);
            }
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            if(BlockId==null)
                return;
            Int64 NewsId=0;
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                NewsId = dc.vWeb_Blocks.Where(b => b.Id == Convert.ToInt64(BlockId)).First().NewsId;
            }
            string url="/LNews/"+NewsId.ToString()+"/#blocks";
            Page.Response.Redirect(url);
        }
        private string AddMediaLink(string text)
        {
            string pattern = @"\(\(NF:VIDEO:([\d]+):([\d]+):([\d]+)\)\)";


            string replacement1 = "<a style='cursor:pointer;' onclick='OpenVideoView($1,$2)'><img width='50' src='" + Application["serverRoot"] + "handlers/GetBlockImage.ashx?MediaId=$1'/></a>";
            text = Regex.Replace(text, pattern, replacement1);

            pattern = @"\(\(NF:IMAGE:([\d]+):([\d]+):([\d]+)\)\)";
            replacement1 = "<a style='cursor:pointer;' onclick='OpenImageView($1)'><img width='50' src='" + Application["serverRoot"] + "handlers/GetBlockImage.ashx?MediaId=$1'/></a>";
            text = Regex.Replace(text, pattern, replacement1);

            return text;
        }

        protected void SaveTb_Click(object sender, EventArgs e)
        {
           
            string s = CommentTb.Text.Replace("<", "").Replace(">", "").Replace("'", "");
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                string ret = dc.fWeb_BlockLookingCheckFromBlockId(Convert.ToInt64(BlockId));
                if (ret.Length > 2)
                {
                    string scripttext = "alert('"+ret+"')";
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);
                    return;
                }

                if(BlockGuid!=null)
                    dc.pWeb_BlockDescFromExtViewUpdate(BlockGuid, s);
                if (BlockId != null)
                    dc.pWeb_BlockDescFromLiteEditorUpdtate(Convert.ToInt64(BlockId), s);
            }

        }

        protected void AjaxFileUpload1_UploadComplete(object sender, /*AjaxControlToolkit.AjaxFileUploadEventArgs*/ object e)
        {
            string s;

        }
    }
}