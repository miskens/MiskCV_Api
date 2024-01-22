namespace MiskCv_Api.Data.Repositories.AddressesRepository;

public interface IAddressRepository
{
    Task<IEnumerable<Address>?> GetAddresses(CancellationToken cancellationToken);
    Task<Address?> GetAddress(int id, CancellationToken cancellationToken);
    Task<Address?> UpdateAddress(int id, Address address, CancellationToken cancellationToken);
    Task<Address?> CreateAddress(Address adress, CancellationToken cancellationToken);
    Task<bool> DeleteAddress(int id, CancellationToken cancellationToken);
}
