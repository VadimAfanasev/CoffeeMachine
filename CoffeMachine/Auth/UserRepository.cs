namespace CoffeeMachine.Auth;

public class UserRepository : IUserRepository
{
    private List<User.UserDto> _users => new()
    {
        new User.UserDto("Admin", "Admin")
    };

    public User.UserDto GetUser(User.UserModel userModel) =>
        _users.FirstOrDefault(u =>
            string.Equals(u.UserName, userModel.UserName) &&
            string.Equals(u.Password, userModel.Password)) ??
        throw new Exception("User not found");
}