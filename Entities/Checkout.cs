using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Checkout
{
    public int Id { get; set; }

    public int? CartId { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? CheckoutDate { get; set; }

    public virtual Cart? Cart { get; set; }
}
