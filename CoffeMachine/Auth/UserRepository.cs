namespace CoffeeMachine.Auth;


/// <summary>
/// The class in which we implement obtaining a valid user for authentication
/// </summary>
public class UserRepository : IUserRepository
{
    /// <summary>
    /// List of valid users
    /// </summary>
    private static List<UserDto> Users => new List<UserDto>
    {
        new UserDto("Admin", "Admin")
    };

    /// <inheritdoc />
    public Task<UserDto> GetUserAsync(UserModel userModel)
    {
        var userDto = Users.FirstOrDefault(u =>
            string.Equals(u.UserName, userModel.UserName) &&
            string.Equals(u.Password, userModel.Password));

        if (userDto == null)
            throw new Exception("User not found");

        return Task.FromResult(userDto);
    }
}