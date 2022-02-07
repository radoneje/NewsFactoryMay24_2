using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Blocks
{
    public partial class blockSocial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null || Request.Params["id"]==null)
            {
                noAccessPanel.Visible = true;
                workPanel.Visible = false;
                return;
            }
               
            Int64 blockId = Convert.ToInt64(Request.Params["id"]);
            using(var dc = new DataClasses1DataContext())
            {
                var rec = dc.tSocial_Messages.Where(m => m.blockId == blockId);
                if(rec.Count()==0)
                {
                    dc.tSocial_Messages.InsertOnSubmit(new tSocial_Message() {
                        id=Guid.NewGuid().ToString(),
                        blockId = blockId,
                         message="",
                         subtitle="",
                         mediaId=0,
                         mediaType=0,
                         title="",
                         updateTime=DateTime.Now 
                    });
                    dc.SubmitChanges();
                }
                var dt = dc.tSocial_Messages.Where(m => m.blockId == blockId).First();
                BSText.InnerText =testservice.StripHTML( dt.message);
                BStitleText.Value = testservice.StripHTML(dt.title);
                BSsubTitleTextCtrl.InnerText = testservice.StripHTML(dt.subtitle);
                BSImageWr.Attributes["mediaId"] = dt.mediaId.ToString();
                BSImageWr.Attributes["mediaType"] = dt.mediaType.ToString();
                //mediaid="0" mediaType="0"
            }
        }
    }
}