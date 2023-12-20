using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiskCv_Api.Models;

public partial class User
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    public string? ImageUrl { get; set; }

    public int? AddressId { get; set; }
}
