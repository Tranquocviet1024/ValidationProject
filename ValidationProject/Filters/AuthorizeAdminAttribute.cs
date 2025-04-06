using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Web.Mvc;
using System.Web.Routing;

namespace ValidationProject.Filters
{
    public class AuthorizeAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (filterContext.HttpContext.Session["User"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Home" },
                        { "action", "Login" }
                    });
                return;
            }

            // Kiểm tra xem người dùng có phải là admin không
            if (filterContext.HttpContext.Session["User"].ToString() != "admin")
        {
                filterContext.HttpContext.Response.StatusCode = 403; // Forbidden
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
            {
                        { "controller", "Home" },
                        { "action", "User" }
                    });
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}