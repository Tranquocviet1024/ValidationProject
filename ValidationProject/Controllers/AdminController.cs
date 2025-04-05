using System.Web.Mvc;
using ValidationProject.Filters;

namespace ValidationProject.Controllers
{
    [AuthorizeAdmin]
    public class AdminController : Controller
    {
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }
    }
}
