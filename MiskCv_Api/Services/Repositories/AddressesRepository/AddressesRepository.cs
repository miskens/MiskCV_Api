using Microsoft.EntityFrameworkCore;
using MiskCv_Api.Data;
using MiskCv_Api.Models;

namespace MiskCv_Api.Services.Repositories.AddressesRepository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly MiskCvDbContext _context;

        public AddressRepository(MiskCvDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Address>?> GetAddresses()
        {
            if (_context.Address == null)
            {
                return null;
            }

            var addresses = await _context.Address.ToListAsync();

            if (addresses.Count < 0 || addresses == null)
            {
                return null;
            }

            return addresses;
        }
    }
}
