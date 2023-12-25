using System.Data;
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

        #region GET

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

        public async Task<Address?> GetAddress(int id)
        {
            if (_context.Address == null)
            {
                return null;
            }

            var address = await _context.Address.FindAsync(id);

            if (address == null)
            {
                return null;
            }

            return address;
        }

        #endregion

        #region PUT

        public async Task<Address?> UpdateAddress(int id, Address address)
        {
            if (_context.Address == null) { return null; }

            _context.Entry(address).State = EntityState.Modified;

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

            return address;
        }

        #endregion

        private bool EntityExists(int id)
        {
            return (_context.Address?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
