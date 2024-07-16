using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class CartItem
{
    public int ItemId { get; set; }

    public int CartId { get; set; }

    public int Quantity { get; set; }

    public int? ProdId { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual ICollection<Checkout> Checkouts { get; set; } = new List<Checkout>();

    public virtual Product? Prod { get; set; }
}
