using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WEB_TRUONG_SP.Models;

namespace WEB_TRUONG_SP.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class MenuController : Controller
    {
        ApplicationDBContext db = new ApplicationDBContext();
        public ActionResult Index(int? page )
        {
            int pageNumber = (page ?? 1);
            var menus = db.Menus.Where(x => !x.IsDelete).OrderByDescending(x=>x.CreateDate).ToList();
            int pageSize = 6;
            return View(menus.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(MenuModel menu)
        {
            if (ModelState.IsValid)
            {
                db.Menus.Add(menu);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var menuEdit = db.Menus.Where(x => x.ID == id).FirstOrDefault();
            if (menuEdit == null) return HttpNotFound();
            return View(menuEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuModel menu)
        {
            if (ModelState.IsValid)
            {
                menu.CreateDate = DateTime.Now;
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        public ActionResult Remove(int id)
        {
            var menuRemove = db.Menus.Where(x => x.ID == id).FirstOrDefault();
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
            var menus = db.Menus.Where(x => x.IsDelete).ToList();
            return View(menus);
        }
        public ActionResult Restore(int id)
        {
            var menu = db.Menus.Where(x => x.ID == id).FirstOrDefault();
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
            var menu = db.Menus
             .Where(x => x.Name.ToLower().Contains(searchString.ToLower()) && !x.IsDelete)
             .ToList();
            if (menu == null || menu.Count == 0) return HttpNotFound();
            return View(menu);
        }
    }
}