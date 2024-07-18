using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Wishlist
{
    public int WishId { get; set; }

    public int UserId { get; set; }

    public int? ProdId { get; set; }

    public virtual Product? Prod { get; set; }

    public virtual Account User { get; set; } = null!;
}
