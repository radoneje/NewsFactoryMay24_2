using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Elements
{
    public partial class mainVideoPlayerElement : System.Web.UI.Page
    {
        public string _mediaId;
        public bool _approved;
        public bool _ready;
        public long _blockId;

        protected void Page_Load(object sender, EventArgs e)
        {
            panelDocument.Visible = false;
            panelImage.Visible = false;
            panelPlayer.Visible = false;
            panelNoImage.Visible = false;

            showMedia(Request.Params["mediaId"]);
            using(Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
            {
                var re=Request.Params["archive"];
                if (re!="true")
                {
                    var rec = dc.vWeb_MediaForLists.Single(r => r.Id.ToString() == Request.Params["mediaId"]);

                    _approved = rec.Approve;
                    _ready = rec.Ready;
                    _blockId = rec.ParentId;
                }
                else
                {
                    var rec = dc.vWeb_ArchiveMediaForLists.Single(r => r.Id.ToString() == Request.Params["mediaId"]);

                    _approved = rec.Approve;
                    _ready = rec.Ready;
                    _blockId = rec.ParentId;
                }
            }
        }
        protected void showMedia(string mediaId)
        {

             _mediaId = mediaId;


            if (Request.Params["BLockType"] == "1")
            {

                panelImage.Visible = true;

            }
            else
                if (Request.Params["BLockType"] == "2")
                {

                    panelPlayer.Visible = true;
                }
                else
                {
                    panelDocument.Visible = true;
                }


        }
    }
}