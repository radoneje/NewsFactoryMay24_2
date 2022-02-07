using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Elements
{
    public partial class mainVideoPlayer : System.Web.UI.Page
    {
        public long _mediaId;
         public long _blockType;
       
        protected void Page_Load(object sender, EventArgs e)
        {
           panelPlayerWr.Visible=false;

            var blockId = Convert.ToInt64(Request.Params["BlockId"]);
            using(Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
            {
                if (Request.Params["archive"] == null)
                {
                    var rec = dc.vWeb_MediaForLists.Where(m => m.ParentId == blockId ).OrderBy(mm => mm.Sort);
                    if (rec.Count() < 1)
                    {
                        return;
                    }
                    _mediaId = rec.First().Id;
                    _blockType = rec.First().BLockType;
                    panelPlayerWr.Visible = true;
                    panelNoImage.Visible = false;
                }
                else
                {
                    var rec = dc.vWeb_ArchiveMediaForLists.Where(m => m.ParentId == blockId && (m.BLockType == 1 || m.BLockType == 2)).OrderBy(mm => mm.Sort);
                    if (rec.Count() < 1)
                    {
                        return;
                    }
                    _mediaId = rec.First().Id;
                    _blockType = rec.First().BLockType;
                    panelPlayerWr.Visible = true;
                    panelNoImage.Visible = false;
                }
                
                
            }

           
        }
       

        }
    }
