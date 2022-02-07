using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Blocks
{
    public partial class blockInplaceEditorMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public bool isReady()
        {
            using(var dc = new DataClasses1DataContext())
            {
                return dc.Blocks.First(b => b.Id == Convert.ToInt64(Request.Params["blockId"])).Ready;
            }
        }
        public bool isApprove()
        {
            using (var dc = new DataClasses1DataContext())
            {
                return dc.Blocks.First(b => b.Id == Convert.ToInt64(Request.Params["blockId"])).Approve;
            }
        }
    }
}