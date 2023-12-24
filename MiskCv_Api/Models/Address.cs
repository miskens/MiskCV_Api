using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MiskCv_Api.Models;

public partial class Address
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(50)]
    public string Street { get; set; } = string.Empty;

    [MaxLength(50)]
    public string PostNr { get; set; } = string.Empty;

    [MaxLength(50)]
    public string City { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Country { get; set; } = string.Empty;

    [ForeignKey("User")]
    public int UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; } = new User();
}
