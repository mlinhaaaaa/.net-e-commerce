using e_commmerce.Entities;
using e_commmerce.Entities.Authentication;
using e_commmerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using NuGet.Protocol.Core.Types;

namespace e_commmerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShopContext _context;

        public HomeController(ILogger<HomeController> logger, ShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Cate).ToList();
            var Newproducts = _context.Products.OrderByDescending(p => p.Id).Take(6).Include(p => p.Cate).ToList();
            ViewData["Newproducts"] = Newproducts;
            var categories = _context.Categories.ToList();
            ViewData["Categories"] = categories;
            return View(products);
        }

        [Route("danh-muc/{name}")]
        public async Task<IActionResult> Category(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
            if (category == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Where(p => p.CateId == category.Cid)
                .Include(p => p.Cate)
                .ToListAsync();

            ViewData["ProductsofCategory"] = category;
            return View(products);
        }

        public IActionResult GetCategories()
        {
            var categories = _context.Categories.Select(c => new { Name = c.Name }).ToList();
            return Json(categories);
        }

        [Route("bang-dieu-khien")]
        [AdminAuthentication]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Account()
        {
            var userUid = HttpContext.Session.GetInt32("UserUid");

            if (userUid == null)
            {
                return RedirectToAction("Login", "Access");
            }

            var account = _context.Accounts.FirstOrDefault(a => a.Uid == userUid);
            if (account == null)
            {
                return RedirectToAction("Login", "Access");
            }

            var billingAddress = _context.BillingAddresses.FirstOrDefault(b => b.AccountUid == account.Uid);
            if (billingAddress == null)
            {
                billingAddress = new BillingAddress();
            }

            var viewModel = new AccountViewModel
            {
                Account = account,
                BillingAddress = billingAddress
            };

            return View(viewModel);
        }


        public IActionResult CreateBillingAddress(BillingAddress model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Account"); // Redirect back to account page if validation fails
            }

            try
            {
                // Assuming you have AccountUid already stored in session or retrieved somehow
                var userUid = HttpContext.Session.GetInt32("UserUid");

                if (userUid == null)
                {
                    return RedirectToAction("Login", "Access");
                }

                var account = _context.Accounts.FirstOrDefault(a => a.Uid == userUid);
                if (account == null)
                {
                    return RedirectToAction("Login", "Access");
                }

                model.AccountUid = account.Uid;

                _context.BillingAddresses.Add(model);
                _context.SaveChanges();


                return RedirectToAction("Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " + ex.Message);
                return RedirectToAction("Account");
            }
        }


        [HttpPost]
        public IActionResult UpdateBillingAddress(BillingAddress model)
        {
            if (ModelState.IsValid)
            {
                var existingAddress = _context.BillingAddresses.Find(model.Id);
                if (existingAddress != null)
                {
                    existingAddress.CompanyName = model.CompanyName;
                    existingAddress.Country = model.Country;
                    existingAddress.Streetaddress = model.Streetaddress;
                    existingAddress.City = model.City;
                    existingAddress.County = model.County;
                    existingAddress.Phone = model.Phone;

                    _context.SaveChanges();
                }
                return RedirectToAction("Account", "Home");
            }
            return View("Account", _context.BillingAddresses);
        }

        [HttpPost]
        public IActionResult UpdateAccountDetails(Account model)
        {
            var userUid = HttpContext.Session.GetInt32("UserUid");
            if (userUid == null)
            {
                return RedirectToAction("Login", "Access");
            }

            var account = _context.Accounts.FirstOrDefault(a => a.Uid == userUid);
            if (account == null)
            {
                return RedirectToAction("Login", "Access");
            }

            // Update account details
            account.FirstName = model.FirstName;
            account.LastName = model.LastName;
            account.Email = model.Email;
            account.Pass = model.Pass;
            account.Birthdate = model.Birthdate;

            _context.SaveChanges();

            return RedirectToAction("Account", "Home");
        }


        public async Task<IActionResult> Search(string searchString)
        {
            var Products = from n in _context.Products
                       select n;
            if (!string.IsNullOrEmpty(searchString))
            {
                Products = Products.Where(s => s.Name.Contains(searchString));
            }
            return View("Index", await Products.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
