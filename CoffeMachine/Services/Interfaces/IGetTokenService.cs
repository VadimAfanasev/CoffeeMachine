using CoffeeMachine.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Services.Interfaces;

/// <summary>
/// Interface describing receiving a token
/// </summary>
public interface IGetTokenService
{
    /// <summary>
    /// Method that implements receiving a token
    /// </summary>
    /// <param name="userModel"> Model of user for getting token </param>
    /// <returns> string </returns>
    /// <response code="401"> Invalid User </response>
    Task<string> GetTokenAsync(UserModel userModel);

    ///// <summary>
    ///// Method that implements receiving a token
    ///// </summary>
    ///// <param name="userModel"> Model of user for getting token </param>
    ///// <returns> string </returns>
    ///// <response code="401"> Invalid User </response>
    //Task<IActionResult> GetTokenAsync(UserModel userModel);
}