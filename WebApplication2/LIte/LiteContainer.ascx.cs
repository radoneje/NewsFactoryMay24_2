using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.LIte
{
    public partial class LiteContainer : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string a = (string)Page.RouteData.Values["ProgramId"];
            Session["ProgramId"] = Page.RouteData.Values["ProgramId"];
            Session["NewsGroupSelected"] = Page.RouteData.Values["NewsGroupSelected"];
            Session["NewsId"] = Page.RouteData.Values["NewsId"];
            if(Session["NewsId"]!=null)
            {
                using (LIte.LiteDataClassesDataContext dc = new LiteDataClassesDataContext())
                {
                    var res=dc.vLite_NewsToLists.Where(n => n.id == Convert.ToInt64(Session["NewsId"]));
                    if(res.Count()>0)
                    {
                        Session["ProgramId"] = res.First().ProgramId;
                        if (res.First().NewsDate.Date > DateTime.Now.Date)
                            Session["NewsGroupSelected"] = "1";
                        else
                            Session["NewsGroupSelected"] = (res.First().NewsDate.Date == DateTime.Now.Date)? "2":"3";
                    }
                }
            }
        }
    }
}