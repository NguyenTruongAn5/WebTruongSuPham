using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB_TRUONG_SP.Models;

namespace WEB_TRUONG_SP.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDBContext db = new ApplicationDBContext();
        public ActionResult Index()
        {
            GetMenuAndSubMenu();
            var banner = db.Banner.Where(x => !x.IsDelete).ToList();
            if (banner != null)
            {
                ViewBag.Banner = banner;
            }
            var newsActi = db.News.Where(x => !x.IsDelete && x.Category == "Hoạt động").Take(3).ToList();

            if (newsActi != null)
            {
                ViewBag.NewsActi = newsActi;
            }
            var newsNofi = db.News.Where(x => !x.IsDelete && x.Category == "Thông báo").Take(3).ToList();

            if (newsNofi != null)
            {
                ViewBag.NewsNofi = newsNofi;
            }

            var newsNofiFull = db.News.Where(x => !x.IsDelete && x.Category == "Thông báo").ToList();

            if (newsNofiFull != null)
            {
                ViewBag.NewsNofiFull = newsNofiFull;
            }
            return View();
        }
        public ActionResult SubIndex(int id, int idSub)
        {
            GetMenuAndSubMenu();

            var menu = db.Menus.Where(x=>!x.IsDelete).ToList();
            if (menu != null)
            {
                ViewBag.Menu = menu;
            }

            var newsMenu = db.News.Where(x => !x.IsDelete && x.IDMenu == id).FirstOrDefault();

            if (newsMenu != null)
            {
                var newsSubMenu = db.News.
                    Where(x => !x.IsDelete && x.IDSubMenu == idSub && x.IDMenu == id).FirstOrDefault();
                if (newsSubMenu != null)
                {
                    return View(newsSubMenu);
                }
                else
                {
                    return View(newsMenu);
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult PageNotification(string type, int? page)
        {
            var menu = db.Menus.Where(x => !x.IsDelete).ToList();
            if (menu != null)
            {
                ViewBag.Menu = menu;
            }
            var news = db.News.Where(x => !x.IsDelete && x.Category == type).ToList();
            if (news != null)
            {
                int numberPage = (page ?? 1);
                int pageSize = 6;
                return View(news.ToPagedList(numberPage,pageSize));
            }

            return RedirectToAction("Index");
        }
        public ActionResult PageSyctheticActivity(string type, int? page)
        {
            var news = db.News.Where(x => !x.IsDelete && x.Category == type).ToList();
            if(news != null)
            {
                int numberPage = (page ?? 1);
                int pageSize = 3;
                return View(news.ToPagedList(numberPage,pageSize));
            }
            return RedirectToAction("Index");
        }
        public void GetMenuAndSubMenu()
        {
            var menu = db.Menus.Where(x => !x.IsDelete).ToList();
            var subMenu = db.SubMenu.Where(x => !x.IsDelete).ToList();
            if (menu != null && subMenu != null)
            {
                ViewBag.Menu = menu;
                ViewBag.SubMenu = subMenu;
            }
        }
    }
}