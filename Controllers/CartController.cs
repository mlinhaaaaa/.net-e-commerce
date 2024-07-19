using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commmerce.Entities;
using System.Linq;
using System.Threading.Tasks;
using e_commmerce.Entities.Authentication;
using Microsoft.AspNetCore.Http;

public class CartController : Controller
{
    private readonly ShopContext _context;

    public CartController(ShopContext context)
    {
        _context = context;
    }

    [Authentication]
    public async Task<IActionResult> Cart()
    {
        int? userId = HttpContext.Session.GetInt32("UserUid");
        if (userId == null)
        {
            return RedirectToAction("Login", "Access");
        }

        var cartItems = await _context.Carts
            .Include(c => c.Product)
            .Where(c => c.AccountUid == userId.Value)
            .ToListAsync();

        return View(cartItems);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int ProdId)
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

        var cartItem = await _context.Carts
            .FirstOrDefaultAsync(ci => ci.AccountUid == userId.Value && ci.ProductId == ProdId);

        if (cartItem != null)
        {
            cartItem.Quantity += 1;
        }
        else
        {
            cartItem = new Cart
            {
                AccountUid = userId.Value,
                ProductId = ProdId,
                Quantity = 1
            };
            _context.Carts.Add(cartItem);
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "Product added to cart successfully" });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
    {
        var cartItem = await _context.Carts.FindAsync(cartItemId);

        if (cartItem == null)
        {
            return NotFound();
        }

        if (quantity <= 0)
        {
            return BadRequest("Quantity must be greater than zero.");
        }

        cartItem.Quantity = quantity;
        _context.Carts.Update(cartItem);
        await _context.SaveChangesAsync();

        return RedirectToAction("Cart");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int cartItemId)
    {
        var cartItem = await _context.Carts.FindAsync(cartItemId);

        if (cartItem == null)
        {
            return NotFound();
        }

        _context.Carts.Remove(cartItem);
        await _context.SaveChangesAsync();

        return RedirectToAction("Cart");
    }
}
