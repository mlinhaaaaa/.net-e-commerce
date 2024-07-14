using System;
using System.Collections.Generic;

namespace e_commmerce.Entities;

public partial class BillingAddress
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? CompanyName { get; set; }

    public string? Country { get; set; }

    public string? Streetaddress { get; set; }

    public string? City { get; set; }

    public string? County { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? AccountUid { get; set; }

    public virtual Account? AccountU { get; set; }
}
