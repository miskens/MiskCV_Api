using MiskCv_Api.Models;

namespace MiskCv_Api.Services.Repositories.SkillsRepository
{
    public interface ISkillRepository
    {
        Task<IEnumerable<Skill>?> GetSkills();
    }
}
