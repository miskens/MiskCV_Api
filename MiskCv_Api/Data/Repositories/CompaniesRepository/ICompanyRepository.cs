namespace MiskCv_Api.Data.Repositories.CompaniesRepository;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>?> GetCompanies(CancellationToken cancellationToken);
    Task<Company?> GetCompany(int id, CancellationToken cancellationToken);
    Task<Company?> UpdateCompany(int id, Company company, CancellationToken cancellationToken);
    Task<Company?> CreateCompany(Company company, int skillId, CancellationToken cancellationToken);
    Task<bool> DeleteCompany(int id, CancellationToken cancellationToken);
}
