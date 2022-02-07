using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Elements
{
    public partial class adminForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ViewStateMode = ViewStateMode.Disabled;
            
            errorPanel.Visible =( Session["UserId"] == null);
            workPanel.Visible = (Session["UserId"] != null);

            if (Session["UserId"] == null)
                return;
            Int32 userId=Convert.ToInt32(Session["UserId"]);
            bool isAdmin = false;
           using(Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                if (dc.vUsersRights.Where(u => u.UserID == userId && u.RightID == 10).Count() > 0)
                    isAdmin = true;
            }
           APsocialPanel.Visible = isAdmin;
           APsocialPanel.Visible = isAdmin;
            APblockdeletedPanel.Visible = true;// isAdmin;
            APrssPanel.Visible = isAdmin;
            APNewsPanel.Visible = isAdmin;
            APRolePanel.Visible = isAdmin;
            APuserPanel.Visible = isAdmin;
            APplayoutTemplatePanel.Visible = isAdmin;
            APitleOutPanel.Visible = isAdmin;
            APblockTypePanel.Visible = isAdmin;
            APprintTemplatePanel.Visible = isAdmin;
        }
    }
}