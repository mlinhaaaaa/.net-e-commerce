using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Category
{
    public int Cid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
