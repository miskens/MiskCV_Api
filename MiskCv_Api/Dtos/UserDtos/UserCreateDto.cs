namespace MiskCv_Api.Dtos.UserDtos;

public record struct UserCreateDto(
    string FirstName,
    string LastName,
    string Username,
    DateTime DateOfBirth,
    string ImageUrl,
    List<AddressCreateDto> Address
    );
