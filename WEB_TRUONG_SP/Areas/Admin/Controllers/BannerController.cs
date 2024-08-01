using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WEB_TRUONG_SP.Models;

namespace WEB_TRUONG_SP.Areas.Admin.Controllers
{
    public class BannerController : Controller
    {
        ApplicationDBContext db = new ApplicationDBContext();
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            var department = db.Banner.Where(x => !x.IsDelete).OrderByDescending(x => x.CreateDate).ToList();
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
        public async Task<ActionResult> Add(Banner banner, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var flie = Request.Files["ImageFile"];
                if(flie != null && flie.ContentLength > 0)
                {
                    banner.ImgPath = LoadImgName(ImageFile);
                    db.Banner.Add(banner);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(banner);
        }

        public string LoadImgName(HttpPostedFileBase ImageFile)
        {
            string path = "";
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var uploadDir = "~/ImgPath/Banner";
                var imagePath = Path.Combine(Server.MapPath(uploadDir), Path.GetFileName(ImageFile.FileName));
                path = Path.Combine(uploadDir, Path.GetFileName(ImageFile.FileName));
                ImageFile.SaveAs(imagePath);
            }
            return path;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var banEdit = db.Banner.Where(x => x.ID == id).FirstOrDefault();
            if (banEdit == null) return HttpNotFound();
            return View(banEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Banner banner, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var oldPathImg = banner.ImgPath;

                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    banner.ImgPath = CheckDeleteOrUpdateImg(oldPathImg, ImageFile);
                }
                else
                {
                    banner.ImgPath = oldPathImg;
                }
                banner.CreateDate = DateTime.Now;
                db.Entry(banner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(banner);
        }

        public string CheckDeleteOrUpdateImg(string oldPathImg, HttpPostedFileBase ImageFile)
        {
            string uploadDir = "~/ImgPath/Banner";
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
            var banerRemove = db.Banner.Where(x => x.ID == id).FirstOrDefault();
            if (banerRemove == null) return HttpNotFound();
            if (ModelState.IsValid)
            {
                banerRemove.IsDelete = true;
                db.Entry(banerRemove).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(banerRemove);
        }

        public ActionResult RestoreIndex()
        {
            var banner = db.Banner.Where(x => x.IsDelete).ToList();
            return View(banner);
        }
        public ActionResult Restore(int id)
        {
            var banner = db.Banner.Where(x => x.ID == id).FirstOrDefault();
            if (banner == null) return HttpNotFound();

            banner.IsDelete = false;
            db.Entry(banner).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("RestoreIndex");
        }
        [HttpGet]
        public ActionResult Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return HttpNotFound();
            var banner = db.Banner
             .Where(x => x.Name.ToLower().Contains(searchString.ToLower()) && !x.IsDelete)
             .ToList();
            if (banner == null || banner.Count == 0) return HttpNotFound();
            return View(banner);
        }
    }
}