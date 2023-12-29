namespace MiskCv_Api.Mapping;

public class SkillMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Skill, SkillCreatedDto>()
            .Map(dest => dest.Company, src => src.Company.Select(item =>
                        new Company
                        {
                            Id = item.Id,
                            CompanyName = item.CompanyName,
                            BossName = item.BossName,
                            PeriodStart = item.PeriodStart,
                            PeriodEnd = item.PeriodEnd,
                            SkillId = item.SkillId,
                            Description = item.Description
                        }).ToList())
            .Map(dest => dest, src => src);
    }
}
