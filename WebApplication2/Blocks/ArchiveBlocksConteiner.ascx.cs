using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Blocks
{
    public partial class ArchiveBlocksConteiner : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReloadArchiveUsers();
        }
        protected void ReloadArchiveUsers()
        {
            News.NewsDataContext dc = new News.NewsDataContext();
            ArchiveAutorDropDown.Items.Clear();
            ArchiveAutorDropDown.Items.Add(new ListItem() { Text ="Выберете автора", Value = "-1" });
            dc.vWeb_Users.OrderBy(r => r.UserName).ToList().ForEach(u=> { ArchiveAutorDropDown.Items.Add(new ListItem() { Text = u.UserName, Value = u.UserID.ToString() }); });
          
        }
    }
}