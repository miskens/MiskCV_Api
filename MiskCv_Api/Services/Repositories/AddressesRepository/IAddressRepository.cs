using MiskCv_Api.Models;

namespace MiskCv_Api.Services.Repositories.AddressesRepository
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>?> GetAddresses();
        Task<Address?> GetAddress(int id);
        Task<Address?> UpdateAddress(int id, Address address);
        Task<Address?> CreateAddress(Address adress);
    }
}
