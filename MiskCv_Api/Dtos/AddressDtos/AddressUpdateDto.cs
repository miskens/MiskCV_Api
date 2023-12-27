namespace MiskCv_Api.Dtos.AddressDtos;

public record struct AddressUpdateDto(
    int Id,
    string Street,
    string PostNr,
    string City,
    string Country,
    string UserId
    );
