using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiskCv_Api.Models;

public partial class Skill
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Proficiency { get; set; } = string.Empty;
}
