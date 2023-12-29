namespace MiskCv_Api.Mapping
{
    public class CompanyMappingcs : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Company, CompanyCreatedDto>()
                .Map(dest => dest.Skill, src => src.Skill.Select(item =>
                                    new Skill
                                    {
                                        Id = item.Id,
                                        Name = item.Name,
                                        Proficiency = item.Proficiency
                                    }))
                .Map(dest => dest, src => src);
        }
    }
}
