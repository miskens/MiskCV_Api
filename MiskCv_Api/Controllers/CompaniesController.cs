using MapsterMapper;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.Ceiling_Math;

namespace MiskCv_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyRepository _companiesRepository;
    private readonly IMapper _mapper;

    public CompaniesController(
            ICompanyRepository companiesRepository,
            IMapper mapper)
    {
        _companiesRepository = companiesRepository;
        _mapper = mapper;
    }

    #region GET

    // GET: api/Companies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany()
    {
        var companyModels = await _companiesRepository.GetCompanies();

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
        var companyModel = await _companiesRepository.GetCompany(id);

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
    public async Task<IActionResult> PutCompany(int id, CompanyUpdateDto companyDto)
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

        return NoContent();
    }

    #endregion

    #region POST

    // POST: api/Companies
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Company>?> PostCompany(CompanyCreateDto companyDto)
    {
        var company = _mapper.Map<Company>(companyDto);

        var newCompany = await _companiesRepository.CreateCompany(company);
        if (newCompany == null) { return Problem("There was a problem adding company"); }

        return CreatedAtAction("GetCompany", new { id = newCompany.Id }, newCompany);
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
