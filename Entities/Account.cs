using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class Account
{
    public int Uid { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string User { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public string? ConfirmPassword { get; set; }

    public int IsAdmin { get; set; }
}
