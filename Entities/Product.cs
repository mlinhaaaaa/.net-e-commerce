using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ImagePath { get; set; }

    public decimal? Price { get; set; }

    public string? Size { get; set; }

    public string? Description { get; set; }

    public int? CateId { get; set; }

    public virtual Category? Cate { get; set; }
}
