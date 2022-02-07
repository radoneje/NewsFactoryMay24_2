using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.LIte.BlocksElements
{
    
    public partial class BlockItem : System.Web.UI.UserControl
    {
        public string imgUrl {set {Image1.ImageUrl=value;}}
        public string Name {set{NameLiteral.Text=value;}}
        public string Autor { set { AutorLiteral.Text = value; } }
        public string Type { set { TypeLiteral.Text = value; } }
        public string Planned { set { ChronoPlanLiteral.Text = value.Replace("<", "&lt;").Replace(">", "&rt;"); ; } }
        public string Real { set { ChronoFactLiteral.Text=value; } }
        public string BlockId { set { BlockIdHidden.Value = value; } }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
