using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiskCv_Api.Models;

public partial class Address
{
    [Key]
    [Required]
    public int Id { get; set; }

    [MaxLength(50)]
    public string Street { get; set; } = string.Empty;

    [MaxLength(50)]
    public string PostalNr { get; set; } = string.Empty;

    [MaxLength(50)]
    public string City { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Country { get; set; } = string.Empty;
}
