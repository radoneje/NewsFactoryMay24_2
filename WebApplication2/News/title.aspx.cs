using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.News
{
    public partial class title : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            scriptDiv.InnerHtml = "<script>var css='"+Request.Params["templateId"]+ "'; var newsId=" + Request.Params["newsId"] + "; </script>";
        }
    }
}