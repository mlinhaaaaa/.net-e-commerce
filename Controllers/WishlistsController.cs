using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_commmerce.Entities;
using e_commmerce.Entities.Authentication;

namespace e_commmerce.Controllers
{
    public class WishlistsController : Controller
    {
        private readonly ShopContext _context;

        public WishlistsController(ShopContext context)
        {
            _context = context;
        }

        [Authentication]
        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserUid");
            if (userId == null)
            {
                return RedirectToAction("Login", "Access");
            }

            var Items = await _context.Wishlists
                .Include(c => c.Product)
                .Where(c => c.AccountUid == userId.Value)
                .ToListAsync();

            return View(Items);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int ProdId)
        {
            var product = await _context.Products.FindAsync(ProdId);
            if (product == null)
            {
                return NotFound();
            }

            int? userId = HttpContext.Session.GetInt32("UserUid");
            if (userId == null)
            {
                return Unauthorized();
            }

            var cartItem = await _context.Wishlists
                .FirstOrDefaultAsync(ci => ci.AccountUid == userId.Value && ci.ProductId == ProdId);


                cartItem = new Wishlist
                {
                    AccountUid = userId.Value,
                    ProductId = ProdId,
                };
                _context.Wishlists.Add(cartItem);
            

            await _context.SaveChangesAsync();

            return Ok(new { message = "Product added to wish list successfully" });
        }

    }
}
