namespace MiskCv_Api.Dtos.SkillDtos
{
    public record struct SkillCompanyCreatedDto(
        int Id,
        string CompanyName,
        string BossName,
        DateTime PeriodStart,
        DateTime? PeriodEnd,
        string Description
        );
}
