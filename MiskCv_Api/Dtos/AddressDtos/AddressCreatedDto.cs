namespace MiskCv_Api.Dtos.AddressDtos;

public record struct AddressCreatedDto(
    int Id,
    string Street,
    string PostNr,
    string City,
    string Country,
    int? UserId
    );
