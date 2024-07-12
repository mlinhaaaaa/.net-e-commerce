using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commmerce.Entities;
using Microsoft.AspNetCore.Http;

namespace e_commmerce.Controllers
{
    public class AccessController : Controller
    {
        private readonly ShopContext db;

        public AccessController(ShopContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Login(Account account)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                var userAccount = db.Accounts
                    .Where(x => x.User.Equals(account.User) && x.Pass.Equals(account.Pass))
                    .FirstOrDefault();

                if (userAccount != null)
                {
                    HttpContext.Session.SetString("User", userAccount.User);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Account account)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Accounts
                    .Where(x => x.User.Equals(account.User))
                    .FirstOrDefault();

                if (existingUser == null)
                {
                    db.Accounts.Add(account);
                    db.SaveChanges();

                    HttpContext.Session.SetString("User", account.User);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("User", "Username already exists");
                }
            }
            return View(account);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Access");
        }
    }
}
