namespace CoffeeMachine.Auth;

/// <summary>
/// The interface in which we describe the method for creating a token
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// The method in which the token is created
    /// </summary>
    /// <param name="key"> Key for token assembly </param>
    /// <param name="issuer"> Issuer for token assembly </param>
    /// <param name="user"> User for authentication </param>
    /// <returns> token </returns>
    string BuildToken(string key, string issuer, UserDto user);
}