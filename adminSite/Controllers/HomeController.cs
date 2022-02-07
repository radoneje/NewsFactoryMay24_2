using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace adminSite.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.u =
            (new adminSite.Models.MainDataClassesDataContext()).Users.Where(u => u.deleted == false).OrderBy(u => u.UserName);
            return View();
        }

    }
}
