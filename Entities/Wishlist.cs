using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Wishlist
{
    public int Id { get; set; }

    public int? AccountUid { get; set; }

    public int? ProductId { get; set; }

    public virtual Account? AccountU { get; set; }

    public virtual Product? Product { get; set; }
}
