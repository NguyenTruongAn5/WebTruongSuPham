using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WEB_TRUONG_SP.Models;

namespace WEB_TRUONG_SP.Controllers
{
    public class AccountController : Controller
    {
        ApplicationDBContext db = new ApplicationDBContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Bạn chưa nhập email vào!";
                return View();
            }
            if (string.IsNullOrEmpty(password))
            {
                ViewBag.Message = "Bạn chưa nhập mật khẩu vào!";
                return View();
            }

            var user = db.Users.Where(x=>x.Email == email && !x.IsDelete).FirstOrDefault();
            if(user == null)
            {
                ViewBag.Message = "Tài khoản không tồn tại!";
                return View();
            }

            string passHashInput = SecurityHelper.GetMd5Hash(password.Trim());
            if(user.PasswordHash != passHashInput)
            {
                ViewBag.Message = "Mật khẩu không đúng!";
                return View();
            }
            TempData["Admin"] = "Admin";
            return RedirectToAction("Index", "Home", new {area = "Admin"});
        }
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(Users users)
        {
            if (ModelState.IsValid)
            {
                var userCheck = db.Users.Where(u => u.Email == users.Email && !u.IsDelete).FirstOrDefault();
                if (userCheck != null)
                {
                    ModelState.AddModelError("", "Tài khoản đã tồn tại");
                    return View(users);
                }
                users.PasswordHash = SecurityHelper.GetMd5Hash(users.Password);
                users.Password = SecurityHelper.ShuffleString
                    (SecurityHelper.GetMd5Hash(users.Password) + users.Password);
                users.ConfirmPassword = users.Password;

                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");


            }
            return View(users);
        }
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var user = db.Users.Where(x => x.Email == email && !x.IsDelete).FirstOrDefault();
                if (user == null)
                {
                    ViewBag.Message = "Email không tồn tại trong hệ thống.";
                    return View();
                }
                else
                {
                    string otp = GenerateRandomOtp();
                    string subject = "Thông báo gửi mã OTP từ trường sư phạm ĐHDT";
                    string body = "Mã OTP của bạn là: " + otp + " Vui lòng không chia sẻ với bất kì ai!";
                    SendMailHelper.SendEmail(email, subject, body);

                    TempData["OTP"] = otp;
                    TempData["Email"] = email;

                    ViewBag.Message = "OTP đã được gửi đến email của bạn.";
                    return RedirectToAction("ConfirmOtp");
                }
            }
            return View();
        }

        public ActionResult ConfirmOtp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmOtp(string otp)
        {
            if (!string.IsNullOrEmpty(otp))
            {
                string trimOtp = otp?.Trim();
                string tempOtp = TempData["OTP"] as string;
                string tempEmail = TempData["Email"] as string;
                TempData["Mail"] = tempEmail;

                if (trimOtp == tempOtp)
                {
                    ViewBag.Message = "OTP hợp lệ. Bạn có thể tiếp tục để đặt lại mật khẩu!!";
                    return RedirectToAction("ChangePassword");
                }
                else
                {
                    ViewBag.Message = "OTP không hợp lệ. Vui lòng kiểm tra lại!";
                }
            }
            return View();
        }

        public string GenerateRandomOtp()
        {
            Random random = new Random();
            int otp = random.Next(100000, 999999);
            return otp.ToString();
        }

        public ActionResult ChangePassword()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string password, string confirmPassword)
        {
            string email = TempData["Mail"] as string;

            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Email không hợp lệ hoặc không có dữ liệu.";
                return View();
            }

            var user = db.Users.FirstOrDefault(x => x.Email == email && !x.IsDelete);
            if (user == null)
            {
                ViewBag.Message = "Tài khoản không tồn tại!";
                return View();
            }

            if (string.IsNullOrEmpty(password.Trim()) || string.IsNullOrEmpty(confirmPassword.Trim()))
            {
                ViewBag.Message = "Bạn chưa nhập mật khẩu hoặc xác nhận!";
                return View();
            }

            if (password.Trim() != confirmPassword.Trim())
            {
                ViewBag.Message = "Mật khẩu chưa khớp!";
                return View();
            }

            user.PasswordHash = SecurityHelper.GetMd5Hash(password.Trim());
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            ViewBag.Message = "Đã đổi mật khẩu thành công!";
            return RedirectToAction("Index");
        }
    }
}