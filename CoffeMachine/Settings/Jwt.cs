namespace CoffeeMachine.Settings;

/// <summary>
/// Class in which described JWT token 
/// </summary>
public class Jwt
{
    /// <summary>
    /// Key for getting token
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// Issuer for getting token
    /// </summary>
    public string Issuer { get; set; }
    /// <summary>
    /// Audience for getting token
    /// </summary>
    public string Audience { get; set; }
}