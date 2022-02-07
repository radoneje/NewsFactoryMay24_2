using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Blocks
{
    public partial class LentaBlocksConteiner : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LentaSourceDropDown.Items.Clear();
            LentaSourceDropDown.Items.Add(new ListItem() { Text = "Любой источник", Value = "0" });
            News.NewsDataContext dc = new News.NewsDataContext();
            foreach (var s in dc.tWeb_RssSources.Where(d => d.Active == true).OrderBy(d => d.Name))
            {
                LentaSourceDropDown.Items.Add(new ListItem() { Text = s.Name , Value = s.id.ToString()  });
            }
        }
    }
}