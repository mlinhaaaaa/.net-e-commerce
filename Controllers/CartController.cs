using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commmerce.Entities;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class CartController : Controller
{
    private readonly ShopContext _context;

    public CartController(ShopContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int ProdId)
    {
        // Ensure the product exists
        var product = await _context.Products.FindAsync(ProdId);
        if (product == null)
        {
            return NotFound(); // Or handle the case where product is not found
        }

        // Get or create CartId from session or database
        int cartId = GetOrCreateCartId();

        // Check if the product is already in the cart
        var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProdId == ProdId);
        if (cartItem != null)
        {
            // Increase quantity if the product is already in cart
            cartItem.Quantity += 1;
        }
        else
        {
            // Add new cart item if the product is not in cart
            cartItem = new CartItem
            {
                CartId = cartId,
                ProdId = ProdId,
                Quantity = 1
            };
            _context.CartItems.Add(cartItem);
        }

        // Save changes to database
        await _context.SaveChangesAsync();

        // Return Ok if adding to cart succeeds
        return Ok(new { message = "Product added to cart successfully" });
    }

    // Helper method to get or create CartId from session or database
    private int GetOrCreateCartId()
    {
        // Example implementation to get CartId from session or database
        // Replace with your own logic to retrieve or create CartId
        int cartId = 1; // Replace with actual implementation
        return cartId;
    }
}
