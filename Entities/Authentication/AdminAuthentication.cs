using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace e_commmerce.Entities.Authentication
{
    public class AdminAuthentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAdmin = context.HttpContext.Session.GetInt32("IsAdmin");
            if (isAdmin == null || isAdmin != 1)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"Controller", "Access"},
                        {"Action", "Login"}
                    });
            }
        }
    }
}
