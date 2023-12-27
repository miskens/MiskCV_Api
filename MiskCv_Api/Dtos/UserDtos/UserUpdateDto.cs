namespace MiskCv_Api.Dtos.UserDtos;

public record struct UserUpdateDto(
    int Id,
    string FirstName,
    string LastName,
    string Username,
    DateTime DateOfBirth,
    string ImageUrl
    );
