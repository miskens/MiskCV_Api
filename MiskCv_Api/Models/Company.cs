using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiskCv_Api.Models;

public partial class Company
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string CompanyName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string BossName { get; set; } = string.Empty;

    [Required]
    public DateTime PeriodStart { get; set; }

    public DateTime? PeriodEnd { get; set; }

    [Required]
    public int SkillsId { get; set; }

    public int? Description { get; set; }
}
