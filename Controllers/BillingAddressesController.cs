using Microsoft.AspNetCore.Mvc;
using e_commmerce.Entities;
using Microsoft.EntityFrameworkCore;

namespace e_commmerce.Controllers
{
    public class BillingAddressesController : Controller
    {
        private readonly ShopContext _context;

        public BillingAddressesController(ShopContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BillingAddress billingAddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(billingAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(billingAddress);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BillingAddress billingAddress)
        {
            if (id != billingAddress.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billingAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillingAddressExists(billingAddress.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(billingAddress);
        }

        private bool BillingAddressExists(int id)
        {
            throw new NotImplementedException();
        }
    }
}
