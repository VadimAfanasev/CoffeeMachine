namespace CoffeeMachine.Auth;

/// <summary>
/// The class in which we implement obtaining a valid user for authentication
/// </summary>
public class UserRepository : IUserRepository
{
    /// <summary>
    /// List of valid users
    /// </summary>
    private static List<User.UserDto> users => new()
    {
        new User.UserDto("Admin", "Admin")
    };

    /// <inheritdoc />
    public async Task<User.UserDto> GetUserAsync(User.UserModel userModel)
    {
        return await Task.Run(() =>
        {
            var userDto = users.FirstOrDefault(u =>
                string.Equals(u.UserName, userModel.UserName) &&
                string.Equals(u.Password, userModel.Password));

            if (userDto == null)
                throw new Exception("User not found");

            return userDto;
        });
    }
}