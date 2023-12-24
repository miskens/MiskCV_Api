namespace MiskCv_Api.Dtos
{
    public record struct UserCreateDto(
        string FirstName,
        string LastName,
        string Username,
        DateTime DateOfBirth,
        string ImageUrl,
        List<AddressCreateDto> Address
        );
}
