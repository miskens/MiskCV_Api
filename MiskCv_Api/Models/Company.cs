using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MiskCv_Api.Models;

public partial class Company
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    public int SkillId { get; set; }

    public string Description { get; set; } = string.Empty;
    [JsonIgnore]
    public ICollection<Skill>? Skill { get; set;}
}
