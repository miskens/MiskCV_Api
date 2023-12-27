namespace MiskCv_Api.Dtos.SkillDtos
{
    public record struct SkillUpdateDto(
        int Id,
        string Name,
        string Proficiency
        );
}
