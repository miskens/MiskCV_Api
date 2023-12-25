using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiskCv_Api.Data;
using MiskCv_Api.Models;
using MiskCv_Api.Services.Repositories.CompaniesRepository;

namespace MiskCv_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly MiskCvDbContext _context;
        private readonly ICompanyRepository _companiesRepository;

        public CompaniesController(MiskCvDbContext context, ICompanyRepository companiesRepository)
        {
            _context = context;
            _companiesRepository = companiesRepository;
        }

        #region GET

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompany()
        {
            var companies = await _companiesRepository.GetCompanies();

            if (companies == null)
            {
                return NotFound();
            }

            return Ok(companies);
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
          if (_context.Company == null)
          {
              return NotFound();
          }
            var company = await _context.Company.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        #endregion

        #region PUT

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            await _companiesRepository.UpdateCompany(id, company);

            return NoContent();
        }

        #endregion

        #region POST

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Company>?> PostCompany(Company company)
        {
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
            if (_context.Company == null)
            {
                return NotFound();
            }
            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Company.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region HELPERS
        private bool CompanyExists(int id)
        {
            return (_context.Company?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        #endregion
    }
}
