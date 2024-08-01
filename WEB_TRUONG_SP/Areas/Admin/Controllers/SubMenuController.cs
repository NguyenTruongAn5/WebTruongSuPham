using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WEB_TRUONG_SP.Models;

namespace WEB_TRUONG_SP.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class SubMenuController : Controller
    {
        ApplicationDBContext db = new ApplicationDBContext();
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            var subMenus = db.SubMenu.Where(x => !x.IsDelete).
                OrderByDescending(x => x.CreateDate).ToList();
            ViewBag.Menu = db.Menus.Where(x => !x.IsDelete).ToList();
            int pageSize = 6;
            return View(subMenus.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Add()
        {
            var menus = db.Menus.Where(x => !x.IsDelete).Select(x => new
            {
                x.ID,
                x.Name
            }).ToList();

            ViewBag.MenuID = new SelectList(menus, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(SubMenu menu)
        {
            if (ModelState.IsValid)
            {
                db.SubMenu.Add(menu);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var menus = db.Menus.Where(x => !x.IsDelete).Select(x => new
            {
                x.ID,
                x.Name
            }).ToList();

            ViewBag.MenuID = new SelectList(menus, "ID", "Name");
            return View(menu);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var menus = db.Menus.Where(x => !x.IsDelete).Select(x => new
            {
                x.ID,
                x.Name
            }).ToList();

            ViewBag.MenuID = new SelectList(menus, "ID", "Name");

            var menuEdit = db.SubMenu.Where(x => x.ID == id).FirstOrDefault();
            if (menuEdit == null) return HttpNotFound();
            return View(menuEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SubMenu submenu)
        {
            if (ModelState.IsValid)
            {
                submenu.CreateDate = DateTime.Now;
                db.Entry(submenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var menus = db.Menus.Where(x => !x.IsDelete).Select(x => new
            {
                x.ID,
                x.Name
            }).ToList();

            ViewBag.MenuID = new SelectList(menus, "ID", "Name", submenu.MenuID);
            return View(submenu);
        }

        public ActionResult Remove(int id)
        {
            var menuRemove = db.SubMenu.Where(x => x.ID == id).FirstOrDefault();
            if (menuRemove == null) return HttpNotFound();
            if (ModelState.IsValid)
            {
                menuRemove.IsDelete = true;
                db.Entry(menuRemove).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menuRemove);
        }

        public ActionResult RestoreIndex()
        {
            var menus = db.SubMenu.Where(x => x.IsDelete).ToList();
            ViewBag.Menu = db.Menus.Where(x => !x.IsDelete).ToList();
            return View(menus);
        }
        public ActionResult Restore(int id)
        {
            var menu = db.SubMenu.Where(x => x.ID == id).FirstOrDefault();
            if (menu == null) return HttpNotFound();

            menu.IsDelete = false;
            db.Entry(menu).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("RestoreIndex");
        }
        [HttpGet]
        public ActionResult Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return HttpNotFound();
            var menu = db.SubMenu
             .Where(x => x.Name.ToLower().Contains(searchString.ToLower()) && !x.IsDelete)
             .ToList();
            if (menu == null || menu.Count == 0) return HttpNotFound();
            ViewBag.Menu = db.Menus.Where(x => !x.IsDelete).ToList();
            return View(menu);
        }
    }
}