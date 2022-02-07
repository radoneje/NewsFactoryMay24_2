using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Elements
{
    public partial class LoginAlertPanel : System.Web.UI.UserControl
    {
        public string Message { set { MessageLiteral.Text = value; } }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}