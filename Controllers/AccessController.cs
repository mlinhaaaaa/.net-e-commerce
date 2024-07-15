using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commmerce.Entities;
using Microsoft.Extensions.Logging;

namespace e_commmerce.Controllers
{
    public class AccessController : Controller
    {
        private readonly ShopContext db;
        private readonly ILogger<AccessController> logger;

        public AccessController(ShopContext context, ILogger<AccessController> logger)
        {
            db = context;
            this.logger = logger;
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
            try
            {
                if (HttpContext.Session.GetString("User") == null)
                {
                    var userAccount = db.Accounts
                        .FirstOrDefault(x => x.User.Equals(account.User) && x.Pass.Equals(account.Pass));

                    if (userAccount != null)
                    {
                        HttpContext.Session.SetString("User", userAccount.User);
                        HttpContext.Session.SetInt32("IsAdmin", userAccount.IsAdmin);
                        HttpContext.Session.SetInt32("UserUid", userAccount.Uid);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred during login");

                return RedirectToAction("Error", "Home");
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
                    HttpContext.Session.SetInt32("IsAdmin", account.IsAdmin);
                    HttpContext.Session.SetInt32("UserUid", account.Uid);

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
