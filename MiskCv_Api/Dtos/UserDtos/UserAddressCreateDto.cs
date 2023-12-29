namespace MiskCv_Api.Dtos.UserDtos;

public record struct UserAddressCreateDto(
    string Street,
    string PostNr,
    string City,
    string Country
    );
