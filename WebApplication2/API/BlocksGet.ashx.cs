using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.API
{
    /// <summary>
    /// Summary description for BlocksGet
    /// </summary>
    public class BlocksGet : IHttpHandler
    {
        struct bls
        {
            public long id;
            public string name;
            public string type;
            public bool ready;
            public bool approve;
       
        }
        public void ProcessRequest(HttpContext context)
        {
            var bl = new List<bls>();
            using (WebApplication2.Blocks.DataClasses1DataContext dc = new WebApplication2.Blocks.DataClasses1DataContext())
            {
                dc.vWeb_Blocks.Where(b => b.NewsId == Convert.ToInt64(context.Request.Params["id"]) && b.ParentId == 0).OrderBy(bb => bb.Sort).ToList().ForEach(l => {

                    bl.Add(new bls() { 
                    id=l.Id,
                    name=l.Name,
                    type=l.TypeName,
                    approve=l.Approve,
                    ready=l.Ready
                    
                    });
                });
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(/*System.Web.Helpers.*/Newtonsoft.Json.JsonConvert.SerializeObject(new { items = bl }));
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