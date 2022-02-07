using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.IO;

namespace WebApplication2.handlers
{
    /// <summary>
    /// Summary description for UserRight
    /// </summary>
    public class UserRight : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/javascript";
            try
            {
                using(Blocks.DataClasses1DataContext dc = new Blocks.DataClasses1DataContext())
                {
                       context.Response.Write("var UR= new Array();\r\n");
                        dc.vUsersRights.Where(ur => ur.UserID.ToString() == (string)RequestContext.RouteData.Values["UserId"]).ToList().ForEach(l => {
                        context.Response.Write("if( typeof(UR['" + l.ProgramID.ToString() + "']) == 'undefined') UR['" + l.ProgramID.ToString() + "'] = new Array(); \r\n ");
                        context.Response.Write("UR["+l.ProgramID.ToString()+"]["+l.RightID.ToString()+"]=1;\r\n");
                    });
                }

            }
            catch { }
            context.Response.Write("console.log(UR);");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
        public class UserRightRouteHandler : IRouteHandler
        {
            public IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return new UserRight() { RequestContext = requestContext }; 
            }
        }
    
}