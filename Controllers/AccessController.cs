using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using e_commmerce.Entities;
using System.Threading.Tasks;
using e_commmerce.Entities.EmailService;

namespace e_commmerce.Controllers
{
    public class AccessController : Controller
    {
        private readonly ShopContext _db;
        private readonly ILogger<AccessController> _logger;
        private readonly IEmailService _emailService;

        public AccessController(ShopContext context, ILogger<AccessController> logger, IEmailService emailService)
        {
            _db = context;
            _logger = logger;
            _emailService = emailService;
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
                    var userAccount = _db.Accounts.FirstOrDefault(x => x.Email.Equals(account.Email) && x.Pass.Equals(account.Pass));

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
                _logger.LogError(ex, "Error occurred during login");
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
                var existingUser = _db.Accounts.FirstOrDefault(x => x.Email.Equals(account.Email));

                if (existingUser == null)
                {
                    _db.Accounts.Add(account);
                    _db.SaveChanges();

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

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var userAccount = _db.Accounts.FirstOrDefault(x => x.Email.Equals(email));

            if (userAccount != null)
            {
                try
                {
                    string newPassword = GenerateRandomPassword();
                    userAccount.Pass = newPassword;
                    userAccount.ConfirmPassword = newPassword;
                    _db.SaveChanges();

                    await _emailService.SendEmailAsync(userAccount.Email, "Password Reset", $"Your new password is: {newPassword}");

                    TempData["SuccessMessage"] = "A new password has been sent to your email.";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing forgot password request.");
                    TempData["ErrorMessage"] = "An error occurred. Please try again later.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Email not found. Please enter a valid email address.";
            }

            return RedirectToAction("ForgotPassword");
        }

        private string GenerateRandomPassword()
        {
            return "newPassword123";
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Access");
        }
    }
}
