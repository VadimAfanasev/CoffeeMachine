namespace CoffeeMachine.Auth;

public interface IUserRepository
{
    User.UserDto GetUser(User.UserModel userModel);
}