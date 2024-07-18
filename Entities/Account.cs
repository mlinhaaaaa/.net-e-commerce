using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Account
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string User { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public string? ConfirmPass { get; set; }

    public int IsAdmin { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
