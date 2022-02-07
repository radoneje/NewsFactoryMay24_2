using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Elements
{
    public partial class playoutTask : System.Web.UI.UserControl
    {
        public string taskId;
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var dc = new Blocks.DataClasses1DataContext())
            {
                var task=dc.tWeb_PlayOutTasks.Where(r => r.id == taskId).First();
                title.InnerHtml = task.dateCreate.ToString("dd.MM.yyyy HH:mm:ss");
               
                if (task.status == 0)
                { taskb.Attributes["class"] = "panel panel-default"; title.InnerHtml += " starting"; }                 
                if (task.status == 1)
                { taskb.Attributes["class"] = "panel panel-warning"; title.InnerHtml+=" working";}
                if (task.status == 2)
                { taskb.Attributes["class"] = "panel panel-success"; title.InnerHtml+=" complite";}
                if (task.status == -1)
                { taskb.Attributes["class"] = "panel panel-danger";
                    alert.InnerHtml = task.description;
                    title.InnerHtml+=" error";
                }


                files.InnerHtml += "<table>";
                dc.tWeb_PlayOutTaskFiles.Where(r => r.taskId == taskId).OrderBy(r=>r.sort).Take(30).ToList().ForEach(l => { 
                var filetext="<tr>";
                  filetext+="<td>"+l.fileTitle;
                    filetext+= "</td><td style='padding-left:10px'> ";
                    float perc=((float)l.bytesSend/(float)l.bytes)*100;
                    filetext+="<b>"+Convert.ToInt16(perc)+"%"+"</b>";
                    if(l.status<0){
                        filetext+="<div class='alert alert-danger'>"+l.description+"</div>";
                    }

                    filetext+= "</td></tr>";

                    files.InnerHtml += filetext;
                });
                files.InnerHtml += "</table>";

            }

        }
    }
}