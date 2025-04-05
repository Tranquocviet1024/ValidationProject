using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValidationProject.Filters;

namespace ValidationProject.Controllers
{
    [AuthorizeAdmin]
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return (IActionResult)View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return (IActionResult)RedirectToAction("Login", "Home");
        }
    }
}