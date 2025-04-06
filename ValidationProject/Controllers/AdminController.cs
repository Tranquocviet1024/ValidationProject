using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;
using ValidationProject.Filters;

[AuthorizeAdmin]
public class AdminController : Controller
{
    public ActionResult Dashboard()
    {
        return View();
    }

    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Home");
    }
}