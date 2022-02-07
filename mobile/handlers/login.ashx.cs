using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mobile.handlers
{
    /// <summary>
    /// Summary description for login
    /// </summary>
    public class login : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
              context.Response.ContentType = "application/json";
             context.Response.CacheControl = "no-cache";
            try{
           var prm= CUtils.GetAjaxRes(context.Request.InputStream);
           string usr=prm.user;
           string pass=prm.pass;
           using(MainDataClassesDataContext dc = new MainDataClassesDataContext())
           {
               var users=dc.vWeb_UsersLoginLists.Where(u=>u.UserName==usr && u.UserPass==pass);
               if(users.Count()==0)
               {
                   context.Response.Write( System.Web.Helpers.Json.Encode(new {state="no user", guid=""}));
                   return;
               }
              if( dc.vA_UserRights.Where(r=>r.URightID==10 && r.UserID==users.First().UserID).Count()==0)
              {
                   context.Response.Write( System.Web.Helpers.Json.Encode(new {state="no right", guid=""}));
                   return;
              }

               string guid=Guid.NewGuid().ToString();
               var lst=(List<CUtils.user>)context.Application["users"];
               lst.Add(new CUtils.user(){
                            id=users.First().UserID,
                            name=users.First().UserName,
                            webGuid=guid,
                            lastTime=DateTime.Now
               });
             context.Response.Write( System.Web.Helpers.Json.Encode(new {state="ok", guid=guid}));
             return;
           }
            }catch{}
          
           context.Response.Write( System.Web.Helpers.Json.Encode(new {state="err", guid=""}));   
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}