namespace MiskCv_Api.Services.Repositories.SkillsRepository;

public interface ISkillRepository
{
    Task<IEnumerable<Skill>?> GetSkills();
    Task<Skill?> GetSkill(int id);
    Task<Skill?> UpdateSkill(int id, Skill skill);
    Task<Skill?> CreateSkill(Skill skill, int companyId);
    Task<bool> DeleteSkill(int id);
}
