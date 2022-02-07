using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Blocks
{
    public partial class BlockEditor : System.Web.UI.Page
    {
        public bool isEditor;
        public long UserId=0;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Response.CacheControl = "no-cache";
            if (Request.Params["BlockId"] == null)
            {
                string scripttext = "ShowDisabledMessage(\"Не могу прочитать ID блока \", \"Ошибка\");";
                ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);

                //onchange="alert($(this).prop('checked'));if($(this).prop('checked')=='False'){$('#BlockEditApproveDropDown').prop('checked', 'False')}"
                return;
            }
       /*     isEditor = false;
            if (Request.Params["iseditor"] == "true")
                isEditor = true;
            else
                BlockEditApproveDropDown.Visible = false;*/
            UserId = 0;
            try
            {
                UserId = Convert.ToInt64(Request.Params["userid"]);
            }
            catch { }

            long BlockId = 0;
            try
            {
                BlockId = Convert.ToInt64(Request.Params["BlockId"]);
            }
            catch(Exception ex)
            {
                string scripttext = "ShowDisabledMessage(\"Не могу прочитать ID блока \", \"Ошибка\");";
                ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);
                return;
            }

            if (Request.Cookies["NFWSession"] == null)
            {
                Response.Redirect("/login.aspx");
                return;
            }

            string sCookie = Request.Cookies["NFWSession"].Value;
            String LookinUserName ="";
            try
            {
                 LookinUserName = CheckLooking(sCookie, BlockId);
            }
            catch(Exception ex)
            {
                string scripttext = "ShowDisabledMessage(\"Ошибка " + ex.Message + "<br>Cockie=" + sCookie + " BlockId=" + BlockId.ToString() + "\", \"Ошибка\");";

                ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);

                return;
            }
            if(LookinUserName.Length>0 )
            {
                //string scripttext = "ShowDisabledMessage(\"Блок заблокировал "+ LookinUserName +"<br>попробуйте позже.\", \"Ошибка\");";
                //ClientScript.RegisterClientScriptBlock(typeof(Page), "script 1", scripttext, true);

                //return;
            }

            //BlockEditTextTextBoxHidden.Value = BlockId.ToString();
            BlockEditTextTextBoxLiteral.Text = BlockId.ToString();
            BlockEditIdHidden.Value=  BlockId.ToString();
            ReloadBlockTypes(BlockId);
            ReloadPersonsList(BlockId);
            ReloadInsertList();
            LoadData(BlockId);
          //  string scripttext1 = "BlockId="+BlockId.ToString() +"; initLookingPinger('" + BlockId.ToString() + "');";
          //  ClientScript.RegisterClientScriptBlock(typeof(Page), "script 3", scripttext1, true);


        }
        private string CheckLooking(string sCookie,long BlockId)
        {
            var dc = new DataClasses1DataContext();
            return dc.fWeb_CheckLooking_FromCookie(Convert.ToInt32(sCookie), BlockId);    
        }
        private void ReloadInsertList()
        {
        }
        private void ReloadPersonsList(long BlockId)
        {
            BlockEditAutorDropDown.Items.Clear();
            BlockEditJockeyDropDown.Items.Clear();
            BlockEditOperatorDropDown.Items.Clear();
            BlockEditCutterDropDown.Items.Clear();
 
            BlockEditAutorDropDown.Items.Add(new ListItem("----", /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject((new { UserID=-1,  UserRate=17 }))));
            BlockEditJockeyDropDown.Items.Add(new ListItem("----", /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject((new { UserID=-1,  UserRate=17 }))));
            BlockEditOperatorDropDown.Items.Add(new ListItem("----",/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject((new { UserID=-1,  UserRate=17 }))));
            BlockEditCutterDropDown.Items.Add(new ListItem("----", /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject((new { UserID = -1, UserRate = 17 }))));
       

            var dc = new DataClasses1DataContext();
            foreach(var people in dc.fWeb_ListUsersTOBlockEditorsList(BlockId).OrderBy(n=>n.UserName))
            {
                if(people.RightID==34)
                {

                    BlockEditAutorDropDown.Items.Add(new ListItem(people.UserName, /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { UserID = people.UserID, UserRate = people.UserRate })));

                    if (people.UserID == UserId)
                        BlockEditAutorDropDown.SelectedIndex = BlockEditAutorDropDown.Items.Count - 1;
                }
                if (people.RightID == 32)
                {
                    BlockEditOperatorDropDown.Items.Add(new ListItem(people.UserName, /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { UserID = people.UserID, UserRate = people.UserRate })));
                }
                if (people.RightID == 33)
                {
                    BlockEditJockeyDropDown.Items.Add(new ListItem(people.UserName, /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { UserID = people.UserID, UserRate = people.UserRate })));
                }
                if (people.RightID == 57)
                {
                    BlockEditCutterDropDown.Items.Add(new ListItem(people.UserName, /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { UserID = people.UserID, UserRate = people.UserRate })));
                }

            }

        }
        private void ReloadBlockTypes(long BlockId)
        {

            BlockEditTypeDropDown.Items.Clear();
            using (var dc = new DataClasses1DataContext())
            {
                foreach (var type in dc.BlockTypes)
                {
                    var str = new { BlockTypeId = type.id.ToString(), BlockIsOperator = type.Operator.ToString(), BlockIsJockey = type.Jockey.ToString() };

                    //System.Web.Script.Serialization.JavaScriptSerializer js1 = new System.Web.Script.Serialization.JavaScriptSerializer();

                    BlockEditTypeDropDown.Items.Add(new ListItem(type.TypeName, /*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(str)));
                }
                BlockEditTypeDropDown.SelectedIndex = 0;
            }
            
           
        }
        private void LoadData(long BlockId)
        {
            var dc = new DataClasses1DataContext();

            dc.pWeb_UpdateLookingFromCookie(Convert.ToInt32(Request.Cookies["NFWSession"].Value), BlockId);
            var bl=dc.Blocks.Where(b=>b.Id==BlockId).First();
            BlockEditNameTextBox.Text=bl.Name;
            //BlockEditTextTextBox.Text=bl.BlockText;
            //BlockEditTextTextBoxHidden.Value = bl.BlockText;
            BlockEditTextTextBoxLiteral.Text = bl.BlockText.Replace("NF::BOLDSTART", "<b>").Replace("NF::BOLDEND", "</b>");
            BlockEditDescriptionTextBox.Text=bl.Description;

            for (int i=0; i<BlockEditTypeDropDown.Items.Count;i++)
            {

                
                dynamic obg = System.Web.Helpers.Json.Decode(BlockEditTypeDropDown.Items[i].Value);

                if (Convert.ToInt32(obg.BlockTypeId ) == bl.BLockType)
                {
                    try
                    {
                        BlockEditTypeDropDown.SelectedIndex = i;
                    }
                    catch { }
                    break;
                }
            }

            for (int ii = 0; ii < BlockEditAutorDropDown.Items.Count; ii++)
            {
                dynamic obg = System.Web.Helpers.Json.Decode(BlockEditAutorDropDown.Items[ii].Value);
                if (Convert.ToInt32(obg.UserID) == bl.CreatorId )
                {
                    try
                    {
                        BlockEditAutorDropDown.SelectedIndex = ii;
                    }
                    catch { }
                    break;
                }
            }

            for (int i = 0; i < BlockEditOperatorDropDown.Items.Count; i++)
            {
                dynamic obg = System.Web.Helpers.Json.Decode(BlockEditOperatorDropDown.Items[i].Value);
                if (Convert.ToInt32(obg.UserID) == bl.OperatorId)
                {
                    try
                    {
                        BlockEditOperatorDropDown.SelectedIndex = i;
                    }
                    catch { }
                    break;
                }
            }

            for (int i = 0; i < BlockEditJockeyDropDown.Items.Count; i++)
            {
                dynamic obg = System.Web.Helpers.Json.Decode(BlockEditJockeyDropDown.Items[i].Value);

                if (Convert.ToInt32(obg.UserID) == bl.JockeyId)
                {
                    try
                    {
                        BlockEditJockeyDropDown.SelectedIndex = i;
                    }
                    catch { }
                    break;
                }
            }
            for (int i = 0; i < BlockEditCutterDropDown.Items.Count; i++)
            {
                dynamic obg = System.Web.Helpers.Json.Decode(BlockEditCutterDropDown.Items[i].Value);

                if (Convert.ToInt32(obg.UserID) == bl.CutterId)
                {
                    try
                    {
                        BlockEditCutterDropDown.SelectedIndex = i;
                    }
                    catch { };
                    break;
                }
            }

            BlockEditRedyDropDown.Checked = bl.Ready;
            BlockEditApproveDropDown.Checked = bl.Approve;


           // BlockEditRedyDropDown.Attributes.Add("onchange", "alert($(this).prop('checked'))");
          //  BlockEditApproveDropDown.Attributes.Add("onchange", "alert($(this).prop('checked'))");
            // onchange="if($(this).prop('checked')=='True'){$('#BlockEditRedyDropDown').prop('checked', 'True')}"


            BlockEditPlannedTextBox.Text=TimeSpan.FromSeconds(bl.TaskTime).ToString();
            BlockEditRealTextBox.Text=TimeSpan.FromSeconds(bl.BlockTime).ToString();
            BlockEditIdHidden.Value = bl.Id.ToString();
         
        }
    }
}