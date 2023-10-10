using static CoffeeMachine.Auth.User;

namespace CoffeeMachine.Services.Interfaces;

public interface IGetTokenService
{
    Task<string> GetTokenAsync(UserModel userModel);
}