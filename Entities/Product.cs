using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public decimal Price { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int? CateId { get; set; }

    public virtual Category? Cate { get; set; }
}
