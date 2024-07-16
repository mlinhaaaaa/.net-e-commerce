﻿using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Product
{
    public int ProdId { get; set; }

    public string Name { get; set; } = null!;

    public string ImgUrl { get; set; } = null!;

    public decimal Price { get; set; }

    public string Size { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int? CateId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category? Cate { get; set; }
}
