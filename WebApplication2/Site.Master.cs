using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks

    
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }

         
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           // this.MainForm.Action = (string)Application["serverRoot"];
            ((System.Web.UI.HtmlControls.HtmlForm)Controls[3]).Action = (string)Application["serverRoot"];
            setPanelVisible(Session["UserId"]);
        }

        protected void LoginBtn_Click(object sender, EventArgs e)
        {
            
        }

        protected void LoginBtn1_Click(object sender, EventArgs e)
        {
            using (Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
            {
                var usrs = dc.vWeb_UsersLoginLists.Where(u => u.UserID == Convert.ToInt32(hiddenLogin.Value) &&
                    u.UserPass == fPassword.Text);
                if (usrs.Count() > 0)
                {
                 /*   if (((int)Application["Sessions"])>=((handlers.utils.Lic)Application["Lic"]).maxUsers)
                   {
                       loginErrorMaxConnectPanel.Visible=true;
                       return;
                   }*/

                    var usr = usrs.First();
                    int cookie = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;


                    Session["UserId"] = usr.UserID;
                    Session["UserName"] = usr.UserName;
                    Response.Cookies.Add(new HttpCookie("NFWSession", cookie.ToString()));
                    Response.Cookies["NFWSession"].Value = cookie.ToString();

                    dc.ExecuteCommand("INSERT INTO tWeb_SessionGuid(UserId, cookie) VALUES (" + usr.UserID.ToString() + ", " + cookie.ToString() + ")");
                    dc.pWeb_log(usr.UserID, 2, -1, -1);

                    var sock = (NFSocket.CSocket)Application["sock"];
                    sock.clientAdd(Session.SessionID, usr.UserID.ToString(), usr.UserName);
                    NFSocket.SendToAll.SendData("userLogin", new { userId = usr.UserID });
                    setPanelVisible(Session["UserId"]);
                  //  Application["Sessions"] = (int)Application["Sessions"] + 1;
                    return;
                }
            }
            incorrectPanel.Visible = true;
            fPassword.Text = "";
        }
         private void setPanelVisible(object param)
        {
            /* if(DateTime.Now>new DateTime(2017,08,30))
             {
                 panelDemoExired.Visible = true;
                 HeadContent.Visible = false;

                 panelLogin.Visible = false;
                 panelWork.Visible = false;
                 return;
             }*/
            if (param != null)
            {
                    HeadContent.Visible = true;
                    headMasterHeadDiv.InnerHtml = " <script src='"+Application["serverRoot"]+"User/" + ((Int32)param).ToString() + "/Right/?id=" + Guid.NewGuid().ToString() + "'> </script>";

                    panelLogin.Visible = false;
                    panelWork.Visible = true;
             }
             else{
                  HeadContent.Visible = false;
                  
                    panelLogin.Visible = true;
                    panelWork.Visible = false;
             }
         }
    }
   

}