using Microsoft.EntityFrameworkCore;
using MiskCv_Api.Data;
using MiskCv_Api.Models;

namespace MiskCv_Api.Services.Repositories.SkillsRepository
{
    public class SkillRepository : ISkillRepository
    {
        private readonly MiskCvDbContext _context;

        public SkillRepository(MiskCvDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Skill>?> GetSkills()
        {
            if (_context.Skill == null)
            {
                return null;
            }

            var skills = await _context.Skill.ToListAsync();

            if (skills.Count < 0 || skills == null)
            {
                return null;
            }

            return skills;
        }
    }
}
