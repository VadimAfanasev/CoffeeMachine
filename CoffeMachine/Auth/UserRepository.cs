namespace CoffeeMachine.Auth;

public class UserRepository : IUserRepository
{
    private List<User.UserDto> users => new()
    {
        new User.UserDto("Admin", "Admin")
    };

    public async Task<User.UserDto> GetUserAsync(User.UserModel userModel) =>
        await Task.Run(() =>
        {
            var userDto = users.FirstOrDefault(u =>
                string.Equals(u.UserName, userModel.UserName) &&
                string.Equals(u.Password, userModel.Password));

            if (userDto == null)
                throw new Exception("User not found");

            return userDto;
        });
}