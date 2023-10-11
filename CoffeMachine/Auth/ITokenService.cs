namespace CoffeeMachine.Auth;

/// <summary>
/// The interface in which we describe the method for creating a token
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// The method in which the token is created
    /// </summary>
    /// <param name="key"> </param>
    /// <param name="issuer"> </param>
    /// <param name="user"> </param>
    /// <returns> token </returns>
    string BuildToken(string key, string issuer, User.UserDto user);
}