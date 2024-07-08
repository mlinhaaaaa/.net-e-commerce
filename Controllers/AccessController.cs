using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commmerce.Entities;
namespace e_commmerce.Controllers
{
    public class AccessController : Controller
    {
        ShopContext db = new ShopContext();
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return View();
            }
            else 
            {
                return RedirectToAction("Index","Home");
            }
        }

        [HttpPost]
        public IActionResult Login(Account account)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                var u = db.Accounts.Where(x=>x.User.Equals(account.User) && x.Pass.Equals(account.Pass)).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("User", u.User.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("User");
            return RedirectToAction("Login", "Access");
        }
    }
}
