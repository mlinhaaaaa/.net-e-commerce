﻿using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Account
{
    public int Uid { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string Pass { get; set; } = null!;

    public string? ConfirmPassword { get; set; }

    public int IsAdmin { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Birthdate { get; set; }

    public virtual ICollection<BillingAddress> BillingAddresses { get; set; } = new List<BillingAddress>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
