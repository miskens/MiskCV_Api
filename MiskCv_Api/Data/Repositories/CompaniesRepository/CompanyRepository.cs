using MiskCv_Api.Models;

namespace MiskCv_Api.Data.Repositories.CompaniesRepository;

public class CompanyRepository : ICompanyRepository
{
    private readonly MiskCvDbContext _context;

    public CompanyRepository(MiskCvDbContext context)
    {
        _context = context;
    }

    #region GET
    public async Task<IEnumerable<Company>?> GetCompanies()
    {
        if (_context.Company == null)
        {
            return null;
        }

        var companies = await _context.Company.ToListAsync();

        if (companies.Count < 0 || companies == null)
        {
            return null;
        }

        return companies;
    }

    public async Task<Company?> GetCompany(int id)
    {
        if (_context.Company == null)
        {
            return null;
        }

        var company = await _context.Company.FindAsync(id);

        if (company == null)
        {
            return null;
        }

        return company;
    }

    #endregion

    #region PUT

    public async Task<Company?> UpdateCompany(int id, Company company)
    {
        if (_context.Company == null) { return null; }

        _context.Entry(company).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DBConcurrencyException)
        {
            if (!EntityExists(id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }
        return company;
    }

    #endregion

    #region POST

    public async Task<Company?> CreateCompany(Company company, int skillId)
    {
        if (_context.Company == null) { return null; }

        if (skillId > 0)
        {
            var skill = await _context.Skill.FindAsync(skillId);

            if (skill != null)
                company.Skill.Add(skill);
        }

        _context.Company.Add(company);
        await _context.SaveChangesAsync();

        return company;
    }

    #endregion

    #region DELETE

    public async Task<bool> DeleteCompany(int id)
    {
        if (_context.Company == null) { return false; }

        var company = await _context.Company.FindAsync(id);

        if (company == null) { return false; }

        _context.Company.Remove(company);
        await _context.SaveChangesAsync();

        return true;
    }

    #endregion

    #region HELPERS

    private bool EntityExists(int id)
    {
        return (_context.Company?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    #endregion


}
