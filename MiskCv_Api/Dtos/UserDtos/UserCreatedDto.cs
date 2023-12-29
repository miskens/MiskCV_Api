namespace MiskCv_Api.Dtos.UserDtos;

public record struct UserCreatedDto(
    int Id,
    string FirstName,
    string LastName,
    string Username,
    DateTime DateOfBirth,
    List<UserAddressCreateDto> Address
    );
