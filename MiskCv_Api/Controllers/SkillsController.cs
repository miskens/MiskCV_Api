namespace MiskCv_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SkillsController : ControllerBase
{
    private readonly ISkillRepository _skillRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCachingService _cache;

    public SkillsController(
            ISkillRepository skillRepository,
            IMapper mapper,
            IDistributedCachingService cache)
    {
        _skillRepository = skillRepository;
        _mapper = mapper;
        _cache = cache;
    }

    #region GET

    // GET: api/Skills
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<SkillDto>>> GetSkill()
    {
        IEnumerable<Skill>? skillModels = null;

        var actionName = $"{nameof(GetSkill)}";
        var recordKey = $"{actionName}_AllSkills";

        skillModels = await _cache.GetRecordAsync<IEnumerable<Skill>>(recordKey);

        if(skillModels == null )
        {
            skillModels = await _skillRepository.GetSkills();

            if (skillModels != null)
            {
                await _cache.SetRecordAsync(recordKey, skillModels);
            }
        }

        if (skillModels == null)
        {
            return NotFound();
        }

        var skills = _mapper.Map<List<SkillDto>>(skillModels);

        return Ok(skills);
    }

    // GET: api/Skills/5
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<SkillDto>> GetSkill(int id)
    {
        Skill? skillModel = null;

        var recordKey = $"Skill_Id_{id}";

        skillModel = await _cache.GetRecordAsync<Skill>(recordKey);

        if (skillModel == null )
        {
            skillModel = await _skillRepository.GetSkill(id);

            if (skillModel != null)
            {
                await _cache.SetRecordAsync(recordKey, skillModel);
            }
        }

        if (skillModel == null)
        {
            return NotFound();
        }

        var skill = _mapper.Map<SkillDto>(skillModel);

        return skill;
    }

    #endregion

    #region PUT

    // PUT: api/Skills/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutSkill([FromBody] SkillUpdateDto skillDto, int id)
    {
        var skill = _mapper.Map<Skill>(skillDto);

        if (id != skill.Id)
        {
            return BadRequest();
        }

        var result = await _skillRepository.UpdateSkill(id, skill);

        if (result == null)
        {
            return Problem("There was a problem updating skill");
        }

        var recordKey = $"Skill_Id_{result.Id}";
        await _cache.SetRecordAsync<Skill>(recordKey, skill);

        return NoContent();
    }

    #endregion

    #region POST

    // POST: api/Skills
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SkillCreatedDto>> PostSkill([FromBody] SkillCreateDto skillDto)
    {
        var skillModel = _mapper.Map<Skill>(skillDto);
        
        skillModel = await _skillRepository.CreateSkill(skillModel, skillDto.companyId);

        if (skillModel == null) 
        { 
            return Problem("There was a problem adding skill"); 
        }
        
        try
        {
            var createdSkill = _mapper.Map<SkillCreatedDto>(skillModel);
            var recordKey = $"Skill_Id_{skillModel.Id}";
            await _cache.SetRecordAsync<Skill>(recordKey, skillModel);

            return CreatedAtAction("GetSkill", new { id = createdSkill.Id }, createdSkill);
        }
        catch(Exception ex)
        {
            Console.WriteLine("There was a problem creating skill", ex.Message);
            return Problem(ex.Message);
        }
    }

    #endregion

    #region DELETE

    // DELETE: api/Skills/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSkill(int id)
    {
        var result = await _skillRepository.DeleteSkill(id);

        if (result == false) { return NotFound(); }

        return NoContent();
    }

    #endregion
}
