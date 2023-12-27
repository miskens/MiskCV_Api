namespace MiskCv_Api.Dtos.AddressDtos;

public record struct AddressUpdateDto(
    int id,
    string Street,
    string PostNr,
    string City,
    string Country
    );
