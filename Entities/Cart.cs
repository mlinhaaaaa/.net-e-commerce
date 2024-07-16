using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Cart
{
    public int CartId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Account User { get; set; } = null!;
}
