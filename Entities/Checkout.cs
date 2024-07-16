using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Checkout
{
    public int CheckId { get; set; }

    public int ItemId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime CheckoutDate { get; set; }

    public virtual CartItem Item { get; set; } = null!;
}
