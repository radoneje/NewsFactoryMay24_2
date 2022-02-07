using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.LIte.BlocksElements
{
    public partial class BlockHead : System.Web.UI.UserControl
    {
        public string NewsName {set{NewsNameLiteral.Text=value;}}
        public string EditorName {set{EditorLiteral.Text=value;}}
        public string Date { set { DateLiteral.Text = value; } }
        public string Duration { set { DurationLiteral.Text = value; } }

        public string Chrono { set { ChronoLiteral.Text = value; } }
        public string Planned { set { PlannedLiteral.Text = value; } }
        public string Calc { set { CalcLiteral.Text = value; } }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}