namespace MiskCv_Api.Data.Repositories.CompaniesRepository;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>?> GetCompanies();
    Task<Company?> GetCompany(int id);
    Task<Company?> UpdateCompany(int id, Company company);
    Task<Company?> CreateCompany(Company company, int skillId);
    Task<bool> DeleteCompany(int id);
}
