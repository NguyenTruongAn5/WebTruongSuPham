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
    public class DepartmentController : Controller
    {
        ApplicationDBContext db = new ApplicationDBContext();
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            var department = db.Department.Where(x => !x.IsDelete).OrderByDescending(x => x.CreateDate).ToList();
            int pageSize = 6;
            return View(department.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(Department department)
        {
            if (ModelState.IsValid)
            {
                db.Department.Add(department);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(department);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var department = db.Department.Where(x => x.ID == id).FirstOrDefault();
            if (department == null) return HttpNotFound();
            return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                department.CreateDate = DateTime.Now;
                db.Entry(department).State = EntityState.Modified;
                db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        public ActionResult Remove(int id)
        {
            var departmentRemove = db.Department.Where(x => x.ID == id).FirstOrDefault();
            if (departmentRemove == null) return HttpNotFound();
            if (ModelState.IsValid)
            {
                departmentRemove.IsDelete = true;
                db.Entry(departmentRemove).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(departmentRemove);
        }

        public ActionResult RestoreIndex()
        {
            var department = db.Department.Where(x => x.IsDelete).ToList();
            return View(department);
        }
        public ActionResult Restore(int id)
        {
            var department = db.Department.Where(x => x.ID == id).FirstOrDefault();
            if (department == null) return HttpNotFound();

            department.IsDelete = false;
            db.Entry(department).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("RestoreIndex");
        }
        [HttpGet]
        public ActionResult Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return HttpNotFound();
            var department = db.Department
             .Where(x => x.Name.ToLower().Contains(searchString.ToLower()) && !x.IsDelete)
             .ToList();
            if (department == null || department.Count == 0) return HttpNotFound();
            return View(department);
        }
    }
}