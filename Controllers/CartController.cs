using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commmerce.Entities;
using System.Linq;
using System.Threading.Tasks;

public class CartController : Controller
{
    private readonly ShopContext _context;

    public CartController(ShopContext context)
    {
        _context = context;
    }

    //public IActionResult Checkout()
    //{
       
    //    return View();
    //}

    public async Task<IActionResult> Checkout()
    {
        int userId = 1; // Replace with logic to get userId from session or user identity

        var cartItems = await _context.Carts
            .Include(c => c.Prod)
            .Where(c => c.UserId == userId)
            .ToListAsync();

        return View(cartItems);
    }

    public async Task<IActionResult> Cart()
    {
        int userId = 1; // Replace with logic to get userId from session or user identity

        var cartItems = await _context.Carts
            .Include(c => c.Prod)
            .Where(c => c.UserId == userId)
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

        int cartId = GetOrCreateCartId();

        var cartItem = await _context.Carts
            .FirstOrDefaultAsync(ci => ci.UserId == cartId && ci.ProdId == ProdId);

        if (cartItem != null)
        {
            cartItem.Quantity += 1;
        }
        else
        {
            cartItem = new Cart
            {
                UserId = cartId,
                ProdId = ProdId,
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

    private int GetOrCreateCartId()
    {
        int cartId = 1; // Replace with actual logic to get or create cartId
        return cartId;
    }
}
