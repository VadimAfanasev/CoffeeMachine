namespace CoffeeMachine.Auth;

public interface ITokenService
{
    string BuildToken(string key, string issuer, User.UserDto user);
}