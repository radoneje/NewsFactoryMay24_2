using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Elements.BlocksExtViewer
{
    public partial class BlockMediaPanel : System.Web.UI.UserControl
    {
        private Int64 iBlockId;
        public Int64 BlockId { set { iBlockId = value; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            Elements.BlocksExtViewer.MediaPlayer MediaPlayer = (Elements.BlocksExtViewer.MediaPlayer)LoadControl("~/Elements/BlocksExtViewer/MediaPlayer.ascx");

            MediaPlayerContainer.Controls.Add(MediaPlayer);

           using(Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
           {

               foreach (var media in dc.vWeb_MediaForLists.Where(m=>m.ParentId ==iBlockId).OrderBy(g=>g.Sort ))
               {
                   Elements.BlocksExtViewer.MediaItem MediaItem = (Elements.BlocksExtViewer.MediaItem)LoadControl("~/Elements/BlocksExtViewer/MediaItem.ascx");

                   string TypeName = "Документ";
                   switch (media.BLockType)
                   {
                       case 1: TypeName = "Фото"; break;
                       case 2: TypeName = "Видео"; break;
                   }
                   
                   MediaItem.Name = media.Name;
                   MediaItem.Type = TypeName;
                   MediaItem.MediaTypeId = media.BLockType;
                   MediaItem.MediaId = media.Id;
                   MediaItem.ImageUrl = Application["serverRoot"]+"handlers/GetBlockImage.ashx?MediaId=" + media.Id.ToString();
                   MediaContainer.Controls.Add(MediaItem);

               }
           }
        }
    }
}