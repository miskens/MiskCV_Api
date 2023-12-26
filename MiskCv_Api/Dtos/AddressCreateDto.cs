namespace MiskCv_Api.Dtos;

public record struct AddressCreateDto(
    string Street,
    string PostNr,
    string City,
    string Country,
    string? UserId
    );
