using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Elements
{
    public partial class playOutTasks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                dc.tWeb_PlayOutTasks.OrderByDescending(r => r.dateCreate).Take(20).ToList().ForEach(l =>
                {
                    var ctrl = (playoutTask)LoadControl("~/elements/playoutTask.ascx");
                    ctrl.taskId = l.id;
                    playOutForm.Controls.Add(
                       ctrl
                        );
                });
                     
            }
        }
    }
}