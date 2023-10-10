namespace CoffeeMachine.Auth;

public interface IUserRepository
{
    Task<User.UserDto> GetUserAsync(User.UserModel userModel);
}