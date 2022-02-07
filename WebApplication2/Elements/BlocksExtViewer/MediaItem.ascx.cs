using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Elements.BlocksExtViewer
{
    public partial class MediaItem : System.Web.UI.UserControl
    {
        public string ImageUrl { set { MediaImage.ImageUrl = value; } }
        public string Name { set { MediaNameLiteral.Text = value; } }
        public string Type { set { MediaTypeLiteral.Text = value; } }
        public Int64  MediaId;
        public int MediaTypeId;
        protected void Page_Load(object sender, EventArgs e)
        {
            DownloadButtonPlaceholder.Text = "<input type='button' class='btn btn-default btn-xs' value='Скачать исходник' onclick='DownloadMedia(" + MediaId.ToString() + ");'/>";
            switch(MediaTypeId)
            {
                case 1: HyperLink1.NavigateUrl = "javascript:OpenImageView('" + MediaId.ToString() + "')"; break;
                case 2: HyperLink1.NavigateUrl = "javascript:OpenVideoView('" + MediaId.ToString() + "',0)"; break;
                default: HyperLink1.NavigateUrl = "javascript:OpenDocumentView('" + MediaId.ToString() + "')"; break;
            }
              
          
        }
    }
}