using MiskCv_Api.Models;

namespace MiskCv_Api.Services.Repositories.CompaniesRepository
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>?> GetCompanies();
        Task<Company?> GetCompany(int id);
    }
}
