using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Blocks
{
    public partial class blockHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null || Request.Params["id"] == null)
            {
                noAccessPanel.Visible = true;
                workPanel.Visible = false;
                return;
            }
        }
    }
}