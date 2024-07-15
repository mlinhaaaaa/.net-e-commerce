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
            if (HttpContext.Session.GetString("UserUid") == null)
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
                if (HttpContext.Session.GetString("UserUid") == null)
                {
                    var userAccount = db.Accounts
                        .FirstOrDefault(x => x.Email.Equals(account.Email) && x.Pass.Equals(account.Pass));

                    if (userAccount != null)
                    {
                        HttpContext.Session.SetInt32("IsAdmin", userAccount.IsAdmin);
                        HttpContext.Session.SetInt32("UserUid", userAccount.Uid);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred during login");

                return RedirectToAction("Error", "Home");
            }

            return View(account);
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
                    .FirstOrDefault(x => x.Email.Equals(account.Email));

                if (existingUser == null)
                {
                    db.Accounts.Add(account);
                    db.SaveChanges();

                    HttpContext.Session.SetInt32("IsAdmin", account.IsAdmin);

                    TempData["SuccessMessage"] = "Registration successful! Please log in.";

                    return RedirectToAction("Login", "Access");
                }
                else
                {
                    TempData["ErrorMessage"] = "Registration failed. Email already exists.";
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
