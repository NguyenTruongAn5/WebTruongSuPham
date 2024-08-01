using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WEB_TRUONG_SP.Models;

namespace WEB_TRUONG_SP.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class NewsController : Controller
    {
        ApplicationDBContext db = new ApplicationDBContext();
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            var news = db.News.Where(x => !x.IsDelete).OrderByDescending(x => x.PublishDate).ToList();
            int pageSize = 6;
            return View(news.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.CategoryList = new SelectList(new List<SelectListItem>
    {
        new SelectListItem { Text = "Thông báo", Value = "Thông báo" },
        new SelectListItem { Text = "Hoạt động", Value = "Hoạt động" }
    }, "Value", "Text");

            var menus = db.Menus.Where(x => !x.IsDelete).Select(x => new
            {
                x.ID,
                x.Name
            }).ToList();

            ViewBag.MenuID = new SelectList(menus, "ID", "Name");

            var subMenus = db.SubMenu.Where(x => !x.IsDelete).Select(x => new
            {
                x.ID,
                x.Name
            }).ToList();

            ViewBag.subMenuID = new SelectList(subMenus, "ID", "Name");

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Add(News news, HttpPostedFileBase ImageFile)
        {
            ModelState.Remove("IDMenu");
            ModelState.Remove("IDSubMenu");

            if (ModelState.IsValid)
            {
                var file = Request.Files["ImageFile"];
                if (file != null && file.ContentLength > 0)
                {
                    news.ImagePath = LoadImgName(ImageFile);
                    db.News.Add(news);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.CategoryList = new SelectList(new List<SelectListItem>
    {
        new SelectListItem { Text = "Thông báo", Value = "Thông báo" },
        new SelectListItem { Text = "Hoạt động", Value = "Hoạt động" }
    }, "Value", "Text", news.Category);

            var menus = db.Menus.Where(x => !x.IsDelete).Select(x => new
            {
                x.ID,
                x.Name
            }).ToList();

            ViewBag.MenuID = new SelectList(menus, "ID", "Name");

            var subMenus = db.SubMenu.Where(x => !x.IsDelete).Select(x => new
            {
                x.ID,
                x.Name
            }).ToList();

            ViewBag.subMenuID = new SelectList(subMenus, "ID", "Name");

            return View(news);
        }

        public string LoadImgName(HttpPostedFileBase ImageFile)
        {
            string path = "";
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var uploadDir = "~/ImgPath/ImgBG";
                var imagePath = Path.Combine(Server.MapPath(uploadDir), Path.GetFileName(ImageFile.FileName));
                path = Path.Combine(uploadDir, Path.GetFileName(ImageFile.FileName));
                ImageFile.SaveAs(imagePath);
            }
            return path;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.CategoryList = new SelectList(new List<SelectListItem>
    {
        new SelectListItem { Text = "Thông báo", Value = "Thông báo" },
        new SelectListItem { Text = "Hoạt động", Value = "Hoạt động" }
    }, "Value", "Text");

            var menus = db.Menus.Where(x => !x.IsDelete).Select(x => new
            {
                x.ID,
                x.Name
            }).ToList();

            ViewBag.MenuID = new SelectList(menus, "ID", "Name");

            var subMenus = db.SubMenu.Where(x => !x.IsDelete).Select(x => new
            {
                x.ID,
                x.Name
            }).ToList();

            ViewBag.subMenuID = new SelectList(subMenus, "ID", "Name");

            var newEdit = db.News.Where(x => x.ID == id).FirstOrDefault();
            if (newEdit == null) return HttpNotFound();
            newEdit.PublishDate = DateTime.Parse(newEdit.PublishDate.ToString("yyyy-MM-dd"));
            return View(newEdit);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(News news, HttpPostedFileBase ImageFile)
        {
            ModelState.Remove("IDMenu");
            ModelState.Remove("IDSubMenu");
            if (ModelState.IsValid)
            {
                var oldPathImg = news.ImagePath;

                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    news.ImagePath = CheckDeleteOrUpdateImg(oldPathImg, ImageFile);
                }
                else
                {
                    news.ImagePath = oldPathImg;
                }
                news.CreateDate = DateTime.Now;
                db.Entry(news).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryList = new SelectList(new List<SelectListItem>
    {
        new SelectListItem { Text = "Thông báo", Value = "Thông báo" },
        new SelectListItem { Text = "Hoạt động", Value = "Hoạt động" }
    }, "Value", "Text", news.Category);

            var menus = db.Menus.Where(x => !x.IsDelete).Select(x => new
            {
                x.ID,
                x.Name
            }).ToList();

            ViewBag.MenuID = new SelectList(menus, "ID", "Name");

            var subMenus = db.SubMenu.Where(x => !x.IsDelete).Select(x => new
            {
                x.ID,
                x.Name
            }).ToList();

            ViewBag.subMenuID = new SelectList(subMenus, "ID", "Name");

            return View(news);
        }

        public string CheckDeleteOrUpdateImg(string oldPathImg, HttpPostedFileBase ImageFile)
        {
            string uploadDir = "~/ImgPath/ImgBG";
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
            var remove = db.News.Where(x => x.ID == id).FirstOrDefault();
            if (remove == null) return HttpNotFound();
            if (ModelState.IsValid)
            {
                remove.IsDelete = true;
                db.Entry(remove).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(remove);
        }

        public ActionResult RestoreIndex()
        {
            var news = db.News.Where(x => x.IsDelete).ToList();
            return View(news);
        }
        public ActionResult Restore(int id)
        {
            var news = db.News.Where(x => x.ID == id).FirstOrDefault();
            if (news == null) return HttpNotFound();

            news.IsDelete = false;
            db.Entry(news).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("RestoreIndex");
        }
        [HttpGet]
        public ActionResult Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return HttpNotFound();
            var news = db.News
             .Where(x => x.Content.ToLower().Contains(searchString.ToLower()) && !x.IsDelete)
             .ToList();
            if (news == null || news.Count == 0) return HttpNotFound();
            return View(news);
        }
    }
}