using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class BlockScript : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            errorPanel.Visible = (Session["UserId"] == null);
            workPanel.Visible = (Session["UserId"] != null);
            if (workPanel.Visible == true)
            {
                using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                {

                    //TemplatesDropList.SelectedIndexChanged+=TemplatesDropList_SelectedIndexChanged;
                    Literal1.Text = "";
                    if (!Page.IsPostBack)
                    {
                        foreach (var tmp in dc.PrintTemplates.OrderBy(t=>t.name))
                        {
                            TemplatesDropList.Items.Add(new ListItem() { Text = tmp.name, Value = tmp.id.ToString() });
                        }
                    }
                }
                if (Request.Params["TemplateId"] != null)
                { TemplatesDropList.Visible = false; }
                ReloadTemplate();
            }
        }

        protected void TemplatesDropList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadTemplate();
        }
        protected void ReloadTemplate()
        {

           // try{
                int templateId=Convert.ToInt32(TemplatesDropList.Items[TemplatesDropList.SelectedIndex].Value);

                if(Request.Params["TemplateId"] != null)
                {
                    templateId=Convert.ToInt32(Request.Params["TemplateId"]);
                }

                if (Request.Params["NewsId"] != null)
                {
                    Literal1.Text = PrintTemplates.BlockScriptClass.GenerateHTMLNewsTemplate(Convert.ToInt64(Request.Params["NewsId"].ToString()),templateId );
                   
                }
                else
                if (Request.Params["BlockId"] != null)
                {
                    Literal1.Text = PrintTemplates.BlockScriptClass.GenerateHTMLBlockTemplate(Convert.ToInt64(Request.Params["Blockid"].ToString()), templateId);
                }
        

        }
       
    }
}