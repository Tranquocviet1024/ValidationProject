using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValidationProject.Models; // Thêm dòng này để sử dụng LoginViewModel và RegisterViewModel

namespace ValidationProject.Controllers
{
    public class HomeController : Controller
    {
        // Giả lập danh sách người dùng (thay bằng database trong thực tế)
        private static List<AppUser> users = new List<AppUser>();

        public class AppUser
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Trang mô tả ứng dụng của bạn.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Trang liên hệ của bạn.";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Username == "admin" && model.Password == "123456")
            {
                Session["User"] = model.Username;
                return RedirectToAction("Dashboard", "Admin");
            }

            var user = users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            if (user != null)
            {
                Session["User"] = model.Username;
                return RedirectToAction("User", "Home");
            }

            ModelState.AddModelError("", "Tên người dùng hoặc mật khẩu không đúng.");
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("", "Tên người dùng đã tồn tại.");
                return View(model);
            }

            if (model.Username.ToLower() == "admin")
            {
                ModelState.AddModelError("", "Tên người dùng 'admin' đã được dành sẵn.");
                return View(model);
            }

            users.Add(new AppUser { Username = model.Username, Password = model.Password });
            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public new ActionResult User()
        {
            if (Session["User"] == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập để truy cập trang này.";
                return RedirectToAction("Login", "Home");
            }

            if (Session["User"].ToString() == "admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Checkout()
        {
            if (Session["User"] == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập để truy cập trang thanh toán.";
                return RedirectToAction("Login", "Home");
            }

            if (Session["User"].ToString() == "admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            return View();
        }
    }
}