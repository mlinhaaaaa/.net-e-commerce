using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Account
{
    public int Uid { get; set; }

    public string User { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public int IsAdmin { get; set; }
}
