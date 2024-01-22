namespace MiskCv_Api.Data.Repositories.AddressesRepository;

public class AddressRepository : IAddressRepository
{
    private readonly MiskCvDbContext _context;

    public AddressRepository(MiskCvDbContext context)
    {
        _context = context;
    }

    #region GET

    public async Task<IEnumerable<Address>?> GetAddresses(CancellationToken cancellationToken)
    {
        if (_context.Address == null)
        {
            return null;
        }

        var addresses = await _context.Address.ToListAsync(cancellationToken);

        if (addresses.Count < 0 || addresses == null)
        {
            return null;
        }

        return addresses;
    }

    public async Task<Address?> GetAddress(int id, CancellationToken cancellationToken)
    {
        if (_context.Address == null)
        {
            return null;
        }

        var address = await _context.Address.FindAsync(id, cancellationToken);

        if (address == null)
        {
            return null;
        }

        return address;
    }

    #endregion

    #region PUT

    public async Task<Address?> UpdateAddress(int id, Address address, CancellationToken cancellationToken)
    {
        if (_context.Address == null) { return null; }

        _context.Entry(address).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
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

    #region POST

    public async Task<Address?> CreateAddress(Address address, CancellationToken cancellationToken)
    {
        if (_context.Address == null) { return null; }

        _context.Address.Add(address);
        await _context.SaveChangesAsync(cancellationToken);

        return address;
    }

    #endregion

    #region DELETE

    public async Task<bool> DeleteAddress(int id, CancellationToken cancellationToken)
    {
        if (_context.Address == null) { return false; }

        var address = await _context.Address.FindAsync(id, cancellationToken);

        if (address == null) { return false; }

        _context.Address.Remove(address);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    #endregion

    #region HELPERS

    private bool EntityExists(int id)
    {
        return (_context.Address?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    #endregion
}
