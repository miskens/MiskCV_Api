using MapsterMapper;
using Microsoft.Extensions.Caching.Distributed;
using MiskCv_Api.Services.DistributedCacheService;

namespace MiskCv_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyRepository _companiesRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCachingService _cache;

    public CompaniesController(
            ICompanyRepository companiesRepository,
            IMapper mapper,
            IDistributedCachingService cache)
    {
        _companiesRepository = companiesRepository;
        _mapper = mapper;
        _cache = cache;
    }

    #region GET

    // GET: api/Companies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany()
    {
        IEnumerable<Company>? companyModels = null;

        var actionName = $"{nameof(GetCompany)}";
        var recordKey = $"{ actionName }_AllCompanies";

        companyModels = await _cache.GetRecordAsync<IEnumerable<Company>>(recordKey);

        if (companyModels == null )
        {
            companyModels = await _companiesRepository.GetCompanies();

            if (companyModels != null )
            {
                await _cache.SetRecordAsync<IEnumerable<Company>>(recordKey, companyModels);
            }
        }

        if (companyModels == null)
        {
            return NotFound();
        }

        var companies = _mapper.Map<List<CompanyDto>>(companyModels);

        return Ok(companies);
    }

    // GET: api/Companies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyDto>> GetCompany(int id)
    {
        Company? companyModel = null;

        var recordKey = $"Company_Id_{id}";

        companyModel = await _cache.GetRecordAsync<Company>(recordKey);

        if (companyModel == null)
        {
            companyModel = await _companiesRepository.GetCompany(id);

            if (companyModel != null)
            {
                await _cache.SetRecordAsync<Company>(recordKey, companyModel);
            }
        }

        if (companyModel == null)
        {
            return NotFound();
        }

        var company = _mapper.Map<CompanyDto>(companyModel);

        return company;
    }

    #endregion

    #region PUT

    // PUT: api/Companies/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCompany([FromBody] CompanyUpdateDto companyDto, int id)
    {
        var company = _mapper.Map<Company>(companyDto);

        if (id != company.Id)
        {
            return BadRequest();
        }

        var result = await _companiesRepository.UpdateCompany(id, company);

        if (result == null)
        {
            return Problem("There was a problem updating company");
        }

        var recordKey = $"Company_Id_{result.Id}";

        await _cache.SetRecordAsync<Company>(recordKey, result);

        return NoContent();
    }

    #endregion

    #region POST

    // POST: api/Companies
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Company>?> PostCompany([FromBody] CompanyCreateDto companyDto)
    {
        var companyModel = _mapper.Map<Company>(companyDto);

        companyModel = await _companiesRepository.CreateCompany(companyModel, companyDto.SkillId);

        if (companyModel == null) 
        { 
            return Problem("There was a problem adding company"); 
        }

        try
        {
            var createdCompany = _mapper.Map<CompanyCreatedDto>(companyModel);
            var recordKey = $"Company_Id_{companyModel.Id}";
            await _cache.SetRecordAsync<Company>(recordKey, companyModel);

            return CreatedAtAction("GetCompany", new { id = createdCompany.Id }, createdCompany);
        }
        catch (Exception ex)
        {
            Console.WriteLine("There was a problem creating skill", ex.Message);

            return Problem(ex.Message);
        }

        
    }

    #endregion

    #region DELETE

    // DELETE: api/Companies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var result = await _companiesRepository.DeleteCompany(id);

        if (result == false) { return NotFound(); }

        return NoContent();
    }

    #endregion
}
