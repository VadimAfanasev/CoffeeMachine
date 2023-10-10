using static CoffeeMachine.Auth.User;

namespace CoffeeMachine.Services.Interfaces;

/// <summary>
/// Interface describing receiving a token
/// </summary>
public interface IGetTokenService
{

    /// <summary>
    /// Method that implements receiving a token
    /// </summary>
    /// <param name="userModel"></param>
    /// <returns>string</returns>
    /// <response code="401">Invalid User</response> 
    Task<string> GetTokenAsync(UserModel userModel);
}