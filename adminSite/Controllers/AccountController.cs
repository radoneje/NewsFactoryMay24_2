using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace adminSite.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public PartialViewResult passForm(string l, string p){
            if (l == null)
                l = "";
            if (l == "Cheshirski" && p == "fabrika12")
            {
                System.Web.Security.FormsAuthentication.SetAuthCookie("Cheshirski", true);
                return PartialView("succ");
            }
            ViewBag.l = l;
            ViewBag.p = "";
            return PartialView();
        }

    }
}
