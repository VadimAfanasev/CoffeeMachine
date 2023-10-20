using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace CoffeeMachine.Auth;

/// <summary>
/// The class in which the token creation method is implemented
/// </summary>
public class TokenService : ITokenService
{
    /// <summary>
    /// Token lifetime
    /// </summary>
    private readonly TimeSpan _expiryDuration;

    /// <summary>
    /// Constructor of class in which the token creation method is implemented 
    /// </summary>
    public TokenService()
    {
        _expiryDuration = new TimeSpan(0, 30, 0);
    }

    /// <inheritdoc />
    public string BuildToken(string key, string issuer, UserDto user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey,
            SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
            expires: DateTime.UtcNow.Add(_expiryDuration), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}