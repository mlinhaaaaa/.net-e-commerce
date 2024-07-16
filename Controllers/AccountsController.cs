using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_commmerce.Entities;

namespace e_commmerce.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ShopContext _context;

        public AccountsController(ShopContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Accounts.ToListAsync());
        }


        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GrantAdmin(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            account.IsAdmin = 1;

            try
            {
                _context.Update(account);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(account.UserId))
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
        public async Task<IActionResult> RevokeAdmin(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            account.IsAdmin = 0;
            _context.Update(account);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.UserId == id);
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Validate username and password (this part may vary based on your authentication logic)

            var user = await _context.Accounts.SingleOrDefaultAsync(a => a.User == username && a.Pass == password);

            if (user != null)
            {
                // Create a new Cart for the logged-in user if one doesn't exist
                var cart = new Cart
                {
                    UserId = user.UserId, // Assign the UserId from the logged-in user
                   
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();

                // Optionally, store CartId in session or return it to the client
                HttpContext.Session.SetInt32("CartId", cart.CartId);

                return RedirectToAction("Index", "Home"); // Redirect to home page or any other page
            }

            // Handle invalid login
            return View("Login");
        }
    }
}
