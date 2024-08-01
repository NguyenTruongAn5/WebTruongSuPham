using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WEB_TRUONG_SP.Models;

namespace WEB_TRUONG_SP.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class HomeController : Controller
    {
        ApplicationDBContext db = new ApplicationDBContext();

        // GET: Admin/Home
        public ActionResult Index()
        {
            var menus = db.Menus.ToList();
            return View(menus);
        }
    }
}