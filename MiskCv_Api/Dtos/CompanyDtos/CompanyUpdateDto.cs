namespace MiskCv_Api.Dtos.CompanyDtos
{
    public record struct CompanyUpdateDto(
        int Id,
        string CompanyName,
        string BossName,
        DateTime PeriodStart,
        DateTime? PeriodEnd,
        int SkillId,
        string Description
        );
}
