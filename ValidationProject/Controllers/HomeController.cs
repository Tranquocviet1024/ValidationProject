using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValidationProject.Models;

namespace ValidationProject.Controllers
{
    public class HomeController : Controller
    {
        // Giả lập danh sách người dùng (public static để AdminController truy cập)
        public static List<AppUser> users = new List<AppUser>();

        public class AppUser
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public ActionResult Index()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["User"] == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập để truy cập trang này.";
                return RedirectToAction("Login", "Home");
            }

            // Nếu là admin, chuyển về trang Admin Dashboard
            if (Session["User"].ToString() == "admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra nếu là tài khoản admin
            if (model.Username == "admin" && model.Password == "123456")
            {
                Session["User"] = model.Username;
                return RedirectToAction("Dashboard", "Admin");
            }

            // Kiểm tra tài khoản người dùng thông thường
            var user = users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            if (user != null)
            {
                Session["User"] = model.Username;
                return RedirectToAction("Index", "Home"); // Chuyển đến /Home/Index
            }
            else
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View(model);
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string username, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Password and Confirm Password do not match.";
                return View();
            }

            if (users.Any(u => u.Username == username))
            {
                ViewBag.Error = "Username already exists.";
                return View();
            }

            if (username.ToLower() == "admin")
            {
                ViewBag.Error = "The username 'admin' is reserved.";
                return View();
            }

            users.Add(new AppUser { Username = username, Password = password });
            TempData["Success"] = "Registration successful! Please login.";
            return RedirectToAction("Login", "Home");
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
                TempData["Error"] = "Please login to access checkout.";
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