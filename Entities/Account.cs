using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_commmerce.Entities;

public partial class Account
{
    public int Uid { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
    public string User { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 50 characters")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*]).{8,50}$", ErrorMessage = "contain at least one uppercase letter, one number, and one special character.")]
    public string Pass { get; set; } = null!;

    [Compare("Pass", ErrorMessage = "Password and confirmation password do not match")]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
    public string LastName { get; set; } = null!;

    public int IsAdmin { get; set; }
}
