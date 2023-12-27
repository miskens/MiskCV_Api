namespace MiskCv_Api.Dtos.UserDtos;

public record struct UserDto(
    int Id,
    string FirstName,
    string LastName,
    string Username,
    DateTime DateOfBirth,
    string ImageUrl
    );
