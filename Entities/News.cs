using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class News
{
    public int IdN { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public DateTime? TimeCreate { get; set; }

    public int? CateId { get; set; }

    public virtual Category? Cate { get; set; }
}
