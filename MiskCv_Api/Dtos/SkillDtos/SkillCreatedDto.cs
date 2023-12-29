namespace MiskCv_Api.Dtos.SkillDtos
{
    public record struct SkillCreatedDto(
        int Id,
        string Name,
        string Proficiency,
        ICollection<SkillCompanyCreatedDto> Company
        );
}
