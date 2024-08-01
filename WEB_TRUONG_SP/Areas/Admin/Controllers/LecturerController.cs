using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WEB_TRUONG_SP.Models;

namespace WEB_TRUONG_SP.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class LecturerController : Controller
    {
        ApplicationDBContext db = new ApplicationDBContext();
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            var menus = db.Lecturer.Where(x => !x.IsDelete).OrderByDescending(x => x.CreateDate).ToList();
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
        public async Task<ActionResult> Add(Lecturer lecturer, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Files["ImageFile"];
                if (file != null && file.ContentLength > 0)
                {
                    lecturer.ImagePath = LoadImgName(ImageFile);
                    db.Lecturer.Add(lecturer);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("ImageFile", "Ảnh không được để trống");
                    return View(lecturer);
                }
            }
            return View(lecturer);
        }
        public string LoadImgName(HttpPostedFileBase ImageFile)
        {
            string path = "";
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var uploadDir = "~/ImgPath/Lecturer";
                var imagePath = Path.Combine(Server.MapPath(uploadDir), Path.GetFileName(ImageFile.FileName));
                path = Path.Combine(uploadDir, Path.GetFileName(ImageFile.FileName));
                ImageFile.SaveAs(imagePath);
            }
            return path;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var lecEdit = db.Lecturer.Where(x => x.ID == id).FirstOrDefault();
            if (lecEdit == null) return HttpNotFound();
            return View(lecEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Lecturer lecturer, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var oldPathImg = lecturer.ImagePath;

                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    lecturer.ImagePath = CheckDeleteOrUpdateImg(oldPathImg, ImageFile);
                }
                else
                {
                    lecturer.ImagePath = oldPathImg;
                }
                lecturer.CreateDate = DateTime.Now;
                db.Entry(lecturer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lecturer);
        }

        public string CheckDeleteOrUpdateImg(string oldPathImg, HttpPostedFileBase ImageFile)
        {
            string uploadDir = "~/ImgPath/Lecturer";
            string fileName = Path.GetFileName(ImageFile.FileName);
            string newPathImg = Path.Combine(uploadDir, fileName);
            string imagePathNew = Server.MapPath(newPathImg);

            if (!string.IsNullOrEmpty(oldPathImg))
            {
                string pathOldImg = Path.Combine(Server.MapPath(uploadDir), Path.GetFileName(oldPathImg));

                if (System.IO.File.Exists(pathOldImg) && !pathOldImg.Equals(imagePathNew, StringComparison.OrdinalIgnoreCase))
                {
                    System.IO.File.Delete(pathOldImg);
                }
            }

            ImageFile.SaveAs(imagePathNew);

            return newPathImg;
        }

        public ActionResult Remove(int id)
        {
            var lecRemove = db.Lecturer.Where(x => x.ID == id).FirstOrDefault();
            if (lecRemove == null) return HttpNotFound();
            if (ModelState.IsValid)
            {
                lecRemove.IsDelete = true;
                db.Entry(lecRemove).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lecRemove);
        }

        public ActionResult RestoreIndex()
        {
            var lecturers = db.Lecturer.Where(x => x.IsDelete).ToList();
            return View(lecturers);
        }
        public ActionResult Restore(int id)
        {
            var lecturer = db.Lecturer.Where(x => x.ID == id).FirstOrDefault();
            if (lecturer == null) return HttpNotFound();

            lecturer.IsDelete = false;
            db.Entry(lecturer).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("RestoreIndex");
        }
        [HttpGet]
        public ActionResult Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return HttpNotFound();
            var lecturers = db.Lecturer
             .Where(x => x.Name.ToLower().Contains(searchString.ToLower()) && !x.IsDelete)
             .ToList();
            if (lecturers == null || lecturers.Count == 0) return HttpNotFound();
            return View(lecturers);
        }
    }
}