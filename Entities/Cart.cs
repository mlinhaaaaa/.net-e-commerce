﻿using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Cart
{
    public int CartId { get; set; }

    public int UserId { get; set; }

    public int Quantity { get; set; }

    public int? ProdId { get; set; }

    public virtual ICollection<Checkout> Checkouts { get; set; } = new List<Checkout>();

    public virtual Product? Prod { get; set; }

    public virtual Account User { get; set; } = null!;
}
