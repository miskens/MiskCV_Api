namespace MiskCv_Api.Data.Repositories.SkillsRepository;

public interface ISkillRepository
{
    Task<IEnumerable<Skill>?> GetSkills(CancellationToken cancellationToken);
    Task<Skill?> GetSkill(int id, CancellationToken cancellationToken);
    Task<Skill?> UpdateSkill(int id, Skill skill, CancellationToken cancellationToken);
    Task<Skill?> CreateSkill(Skill skill, CancellationToken cancellationToken, int companyId);
    Task<bool> DeleteSkill(int id, CancellationToken cancellationToken);
}
