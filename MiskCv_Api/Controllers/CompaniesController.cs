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
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany(CancellationToken cancellationToken)
    {
        IEnumerable<Company>? companyModels = null;

        var actionName = $"{nameof(GetCompany)}";
        var recordKey = $"{ actionName }_AllCompanies";

        companyModels = await _cache.GetRecordAsync<IEnumerable<Company>>(recordKey, cancellationToken);

        if (companyModels == null )
        {
            companyModels = await _companiesRepository.GetCompanies(cancellationToken);

            if (companyModels != null )
            {
                await _cache.SetRecordAsync<IEnumerable<Company>>(recordKey, companyModels, cancellationToken);
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
    [AllowAnonymous]
    public async Task<ActionResult<CompanyDto>> GetCompany(int id, CancellationToken cancellationToken)
    {
        Company? companyModel = null;

        var recordKey = $"Company_Id_{id}";

        companyModel = await _cache.GetRecordAsync<Company>(recordKey, cancellationToken);

        if (companyModel == null)
        {
            companyModel = await _companiesRepository.GetCompany(id, cancellationToken);

            if (companyModel != null)
            {
                await _cache.SetRecordAsync<Company>(recordKey, companyModel, cancellationToken);
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutCompany([FromBody] CompanyUpdateDto companyDto, int id, CancellationToken cancellationToken)
    {
        var company = _mapper.Map<Company>(companyDto);

        if (id != company.Id)
        {
            return BadRequest();
        }

        var result = await _companiesRepository.UpdateCompany(id, company,cancellationToken);

        if (result == null)
        {
            return Problem("There was a problem updating company");
        }

        var recordKey = $"Company_Id_{result.Id}";

        await _cache.SetRecordAsync<Company>(recordKey, result, cancellationToken);

        return NoContent();
    }

    #endregion

    #region POST

    // POST: api/Companies
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Company>?> PostCompany([FromBody] CompanyCreateDto companyDto, CancellationToken cancellationToken)
    {
        var companyModel = _mapper.Map<Company>(companyDto);

        companyModel = await _companiesRepository.CreateCompany(companyModel, companyDto.SkillId, cancellationToken);

        if (companyModel == null) 
        { 
            return Problem("There was a problem adding company"); 
        }

        try
        {
            var createdCompany = _mapper.Map<CompanyCreatedDto>(companyModel);
            var recordKey = $"Company_Id_{companyModel.Id}";
            await _cache.SetRecordAsync<Company>(recordKey, companyModel, cancellationToken);

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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCompany(int id, CancellationToken cancellationToken)
    {
        var result = await _companiesRepository.DeleteCompany(id, cancellationToken);

        if (result == false) { return NotFound(); }

        return NoContent();
    }

    #endregion
}
