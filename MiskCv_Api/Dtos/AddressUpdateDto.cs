namespace MiskCv_Api.Dtos;

public record struct AddressUpdateDto(
    int id,
    string Street,
    string PostNr,
    string City,
    string Country,
    int userId
    );
