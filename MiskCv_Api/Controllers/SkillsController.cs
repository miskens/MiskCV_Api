using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiskCv_Api.Data;
using MiskCv_Api.Models;
using MiskCv_Api.Services.Repositories.SkillsRepository;

namespace MiskCv_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillRepository _skillRepository;

        public SkillsController(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        #region GET

        // GET: api/Skills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkill()
        {
            var skills = await _skillRepository.GetSkills();

            if (skills == null)
            {
                return NotFound();
            }

            return Ok(skills);
        }

        // GET: api/Skills/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Skill>> GetSkill(int id)
        {
            var skill = await _skillRepository.GetSkill(id);

            if (skill == null)
            {
                return NotFound();
            }

            return skill;
        }

        #endregion

        #region PUT

        // PUT: api/Skills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSkill(int id, Skill skill)
        {
            if (id != skill.Id)
            {
                return BadRequest();
            }

            var result = await _skillRepository.UpdateSkill(id, skill);

            if (result == null)
            {
                return Problem("There was a problem updating skill");
            }

            return NoContent();
        }

        #endregion

        #region POST

        // POST: api/Skills
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Skill>> PostSkill(Skill skill)
        {
            var newSkill = await _skillRepository.CreateSkill(skill);

            if (newSkill == null) { return Problem("There was a problem adding skill"); }

            return CreatedAtAction("GetSkill", new { id = newSkill.Id }, newSkill);
        }

        #endregion

        #region DELETE

        // DELETE: api/Skills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var result = await _skillRepository.DeleteSkill(id);

            if (result == false) { return Problem("There was a problem deleting skill"); }

            return NoContent();
        }

        #endregion
    }
}
