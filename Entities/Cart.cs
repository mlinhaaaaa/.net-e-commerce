using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Cart
{
    public int Id { get; set; }

    public int? AccountUid { get; set; }

    public int? Quantity { get; set; }

    public int? ProductId { get; set; }

    public virtual Account? AccountU { get; set; }

    public virtual ICollection<Checkout> Checkouts { get; set; } = new List<Checkout>();

    public virtual Product? Product { get; set; }
}
