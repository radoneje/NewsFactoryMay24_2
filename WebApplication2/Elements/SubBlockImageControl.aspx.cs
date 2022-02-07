using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Elements
{
    public partial class SubBlockImageControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
        }
        public string getState()
        {
            var blockId = Convert.ToInt64(Request.Params["blockId"]);

            var blockState = "Transparent";
            using (Blocks.DataClassesMediaDataContext dc = new Blocks.DataClassesMediaDataContext())
            {
                var rt= dc.vWeb_MediaForLists.Where(f => f.ParentId == blockId).OrderBy(f => f.Sort);
                if (rt.Count() > 0)
                {
                    var rec = rt.First();
                    var status = (rec.Ready ? 1 : 0) + (rec.Approve ? 1 : 0);
                    switch (status)
                    {
                        case 1: blockState = "Ready"; break;
                        case 2: blockState = "Approve"; break;
                        default: blockState = "Blank"; break;
                    }
                }
            }
            return blockState;
        }
    }
}