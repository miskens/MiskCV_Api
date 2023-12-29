using System.Diagnostics.Metrics;
using System.IO;

namespace MiskCv_Api.Mapping
{
    public class UserMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserCreateDto, User>()
                .Map(dest => dest.Address, src => src.Address.Select(item =>
                                    new Address
                                    {
                                        Street = item.Street,
                                        PostNr = item.PostNr,
                                        City = item.City,
                                        Country = item.Country
                                    }))
                .Map(dest => dest, src => src);
            config.NewConfig<User, UserCreatedDto>()
                .Map(dest => dest.Address, src => src.Address.Select(item =>
                                    new Address
                                    {
                                        Id = item.Id,
                                        Street = item.Street,
                                        PostNr = item.PostNr,
                                        City = item.City,
                                        Country = item.Country,
                                        UserId = item.UserId
                                    }))
                .Map(dest => dest, src => src);
        }
    }
}
