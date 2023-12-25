using MiskCv_Api.Models;

namespace MiskCv_Api.Services.Repositories.AddressesRepository
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>?> GetAddresses();
        Task<Address?> GetAddress(int id);
    }
}
