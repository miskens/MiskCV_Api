namespace MiskCv_Api.Models;

public partial class Skill
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Proficiency { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Company>? Company { get; set; }
}
