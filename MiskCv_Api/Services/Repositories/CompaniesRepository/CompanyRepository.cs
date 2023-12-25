using Microsoft.EntityFrameworkCore;
using MiskCv_Api.Data;
using MiskCv_Api.Models;

namespace MiskCv_Api.Services.Repositories.CompaniesRepository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly MiskCvDbContext _context;

        public CompanyRepository(MiskCvDbContext context)
        {
            _context = context;
        }

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
    }
}
